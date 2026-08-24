using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using WebApiSAIH.Services.Interfaces;

namespace WebApiSAIH.Services
{
    public class EmailService : IEmailService
    {
        private IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string content)
        {
            string emailOrigen = "proyectoSAIH.21@gmail.com";
            string Password = "proyectoSaih";

            MailMessage oMailMessage = new MailMessage(emailOrigen, toEmail, subject, content);

            oMailMessage.IsBodyHtml = true;

            SmtpClient oSmtpClient = new SmtpClient("smtp.gmail.com");
            oSmtpClient.UseDefaultCredentials = false;
            oSmtpClient.EnableSsl = true;
            oSmtpClient.Port = 587;
            oSmtpClient.Credentials = new System.Net.NetworkCredential(emailOrigen, Password);


            oSmtpClient.Send(oMailMessage);

            oSmtpClient.Dispose();
        }
    }
}
