using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TestIdentity.ChangePrimaryKeyDataType.Models;

namespace TestIdentity.ChangePrimaryKeyDataType.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<long>, long>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
