using Microsoft.AspNetCore.Identity.UI.Services;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecco.Web.Areas.Identity
{
    public class EccoEmailSender : IEmailSender
    {
        private readonly SendGridClient _client;

        public EccoEmailSender(string apiKey)
        {
            _client = new SendGridClient(apiKey);
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var from = new EmailAddress("noreply@ecco-space.com", "Ecco Space");
            var to = new EmailAddress(email);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, "", htmlMessage);
            var response = await _client.SendEmailAsync(msg);
        }
    }
}