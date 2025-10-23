using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quotation_Management.ViewModels
{
    public class AssignClientViewModel
    {
        public long ClientAssignId { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string SubClientName { get; set; }
        public string Email { get; set; }
        public long ClientId { get; set; }
        public long SubClientId { get; set; }
        public bool Assign { get; set; }
         
        public string Reference { get; set; }
        public string Phone { get; set; }
        public string Landline { get; set; }
        public string Whatsapp { get; set; }
        public string ContactPerson { get; set; }
        public string RefrenceType { get; set; }
        public long CompanyId { get; set; }
    }
}
