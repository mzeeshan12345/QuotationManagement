using Quotation_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quotation_Management.ViewModels
{
    public class ApplicationUserModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; } 
        public string Id { get; set; } 
        public string FatherName { get; set; }
        public string Gender { get; set; }
        public string CNIC { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Age { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Company { get; set; }
        public long CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string Designation { get; set; }
        public string Email { get; set; } 
        public long UserId { get; set; } 
        public string Password { get; set; } 
        public string UserStatus { get; set; }
        public string EmailAddress { get; set; }
        public string UserType { get; set; } 
        public string PhoneNumber { get; set; }
         public string RoleId { get; set; }
         public string RoleName { get; set; }
        public List<string> Role { get; set; }
        public List<string> RegionId { get; set; }
        public DateTime? CreatedAt { get; set; } 
    }
}
