using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Auth.MacorattiEmailService
{
    public interface IEmailSenderMacoratti
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
