using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Quotation_Management.Models
{
    public class SubClients : Base
    {
        [Key]
        public long SubClientId { get; set; }
        public string ClientName { get; set; }
        public string Email { get; set; }
        public string Reference { get; set; }
        public string Phone { get; set; }
        public string Landline { get; set; }
        public string Whatsapp { get; set; }
        public string ContactPerson { get; set; }
        public string RefrenceType { get; set; }
        public long CompanyId { get; set; }
        public long ClientId { get; set; }
        public bool Assign { get; set; }
        public string UserId { get; set; }
    }
}
