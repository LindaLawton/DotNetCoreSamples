using System.Threading.Tasks;

namespace TestIdentity.ChangePrimaryKeyDataType.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
