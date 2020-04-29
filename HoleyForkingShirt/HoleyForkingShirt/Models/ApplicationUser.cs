using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoleyForkingShirt.Models
{/// <summary>
/// This is the base User that we reference throughout our code. It is made up of three parts that we will always require for a user. 
/// </summary>
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LatName { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
