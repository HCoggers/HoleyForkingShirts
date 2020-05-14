using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoleyForkingShirt.Models.Interfaces
{
    public interface IEmailSender 
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage);
        
    }
}

