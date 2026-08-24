using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiSAIH.Services.Interfaces
{
    public interface ISendGridEmailService
    {
        Task SendEmailAsyn(string toEmail, string subject, string content);
    }
}
