using HoleyForkingShirt.Models.Interfaces;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace HoleyForkingShirt.Models.Services
{
    public class SendGridService : IEmailSender
    {
        private IConfiguration _configuration;

        public SendGridService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        /// <summary>
        /// This is the method that sets up our email to be able to be sent. With the send grid API
        /// </summary>
        /// <param name="email"></param>
        /// <param name="subject"></param>
        /// <param name="htmlMessage"></param>
        /// <returns></returns>
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            SendGridClient client = new SendGridClient(_configuration["SENDGRID_API_KEY"]);
            SendGridMessage msg = new SendGridMessage();

            msg.SetFrom("admin@HFShirts.com", "Site Admin");
            msg.AddTo(email);
            msg.SetSubject(subject);
            msg.AddContent(MimeType.Html, htmlMessage);

            await client.SendEmailAsync(msg);
        }


    }
}

