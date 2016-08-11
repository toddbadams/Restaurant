using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using tba.Core.Email.Interfaces;
using tba.Core.Email.Models;

namespace tba.EmailStub
{
    public class EmailStub : IEmailService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            throw new NotImplementedException();
        }

        public async Task SendAsync(EmailMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
