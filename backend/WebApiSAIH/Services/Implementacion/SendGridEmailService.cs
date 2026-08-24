using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiSAIH.Services.Interfaces;

namespace WebApiSAIH.Services.Implementacion
{
    public class SendGridEmailService : ISendGridEmailService
    {
        private IConfiguration _configuration;

        public SendGridEmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmailAsyn(string toEmail, string subject, string content)
        {
            var apiKey = _configuration["SendGridAPIKey"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("proyectosaih.21@gmail.com", "Proyecto SAIH");
            var to = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, content, content);
            var response = await client.SendEmailAsync(msg);
        }
    }
}
