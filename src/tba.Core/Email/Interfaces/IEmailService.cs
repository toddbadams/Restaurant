using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using tba.Core.Email.Models;

namespace tba.Core.Email.Interfaces
{
    public interface IEmailService : IIdentityMessageService
    {
        Task SendAsync(EmailMessage message);
    }
}