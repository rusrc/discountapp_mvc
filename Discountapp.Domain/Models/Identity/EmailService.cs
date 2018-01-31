using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace Discountapp.Domain.Models.Identity
{
    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            var from = new MailAddress(
                "info@arrba.ru",
                "MailDisplayName",
                Encoding.UTF8);
            ;

            var email = new MailMessage(from,
                new MailAddress(message.Destination))
            {
                Subject = message.Subject,
                Body = message.Body,
                IsBodyHtml = true
            };

            var client = new SmtpClient
            {
                Host = "smtp.yandex.ru",
                UseDefaultCredentials = false,
                Port = Convert.ToInt32(587),
                EnableSsl = true,
                Credentials = new NetworkCredential("info@arrba.ru", "gCpBCLK00N3mCXxdJ9qC")
            };

            client.SendCompleted += (s, e) => { client.Dispose(); };

            await client.SendMailAsync(email);
        }
    }
}
