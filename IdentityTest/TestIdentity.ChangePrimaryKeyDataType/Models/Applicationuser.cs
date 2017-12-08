using Microsoft.AspNetCore.Identity;

namespace TestIdentity.ChangePrimaryKeyDataType.Models
{
    public class ApplicationUser : IdentityUser<long>
    {
    }

    public class ApplicationRole : IdentityRole<long>
    {
    }
}
