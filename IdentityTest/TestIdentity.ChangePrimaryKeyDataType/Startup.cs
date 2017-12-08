using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TestIdentity.ChangePrimaryKeyDataType.Data;
using TestIdentity.ChangePrimaryKeyDataType.Models;
using TestIdentity.ChangePrimaryKeyDataType.Services;

namespace TestIdentity.ChangePrimaryKeyDataType
{

    //https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity-primary-key-configuration?tabs=aspnetcore2x

    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("Default")));

            services.AddIdentity<ApplicationUser, IdentityRole<long>>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var task = InitializeDatabase(app);
            Task.WaitAll(task);

            app.UseMvc();
        }

        public class XenaUserDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
        {
            public ApplicationDbContext CreateDbContext(string[] args)
            {
                var configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", true, true)
                    .AddEnvironmentVariables().Build();

                var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("Default"));
                return new ApplicationDbContext(optionsBuilder.Options);
            }
        }

        public async Task InitializeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                // Create DB
                serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.Migrate();

                // Add roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<long>>>();
                if (!roleManager.Roles.Any())
                {
                    foreach (var role in Config.GetTestRolls())
                    {
                        await roleManager.CreateAsync(role);
                    }
                }

                // Add users
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                if (userManager.Users.Any()) return;
                foreach (var user in Config.GetTestUsers())
                {
                    await userManager.CreateAsync(user, "Password123!");
                }
                

                var adminUser = await userManager.FindByEmailAsync(Config.GetTestUsers().FirstOrDefault()?.Email);
                if (adminUser != null)
                {
                    await userManager.AddToRoleAsync(adminUser, Config.GetTestRolls().FirstOrDefault()?.Name);
                }


            }
        }
    }
}
