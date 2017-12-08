using Microsoft.AspNetCore.Identity;

namespace TestIdentity.ChangePrimaryKeyDataType.Models
{
    public class ApplicationRole : IdentityRole<long>
    {
    }
}
