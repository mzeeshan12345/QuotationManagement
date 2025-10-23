using Quotation_Management.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quotation_Management.Models
{
    public class ApplicationUser : IdentityUser
    {  
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public long CompanyId { get; set; }
          public string Designation { get; set; } 
        public DateTime DateOfBirth { get; set; } 
    }
}
