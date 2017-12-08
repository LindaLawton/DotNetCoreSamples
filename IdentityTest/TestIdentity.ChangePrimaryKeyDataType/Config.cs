using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using TestIdentity.ChangePrimaryKeyDataType.Models;

namespace TestIdentity.ChangePrimaryKeyDataType
{
    public class Config
    {
        public static List<ApplicationUser> GetTestUsers()
        {
            return new List<ApplicationUser>
            {
                new ApplicationUser()
                {
                    Email = "test@test.com",
                    UserName = "TestUser"
                }
            };
        }
        public static List<IdentityRole<long>> GetTestRolls()
        {
            return new List<IdentityRole<long>>
            {
                new IdentityRole<long>() { Name = "admin",NormalizedName = "ADMIN"},
                new IdentityRole<long>() { Name = "user",NormalizedName = "USER"}
            };
        }
    }
}
