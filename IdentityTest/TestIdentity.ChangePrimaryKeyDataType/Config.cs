using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestIdentity.ChangePrimaryKeyDataType.Models;

namespace TestIdentity.ChangePrimaryKeyDataType
{
    public class Config
    {
        public static IEnumerable<ApplicationUser> GetTestUsers()
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

    }
}
