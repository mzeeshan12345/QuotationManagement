using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Quotation_Management.Models
{
    public class Clients : Base
    {
        //public Clients()
        //{
        //    SubClients = new List<SubClients>();
        //}

        [Key]
        public long ClientId { get; set; }
        public string ClientName { get; set; } 
        public string Email { get; set; } 
        public string Landline { get; set; }  
        public string ContactPerson { get; set; } 
        public string RefrenceAlias { get; set; } 
        public string RefrenceType { get; set; } 
        public string Reference { get; set; }  
        public string Address1 { get; set; } 
        public string Address2 { get; set; } 
        public string Address3 { get; set; } 
        public string TRN { get; set; } 
        public string TRNFile { get; set; } 
        public string Country { get; set; } 
        public long RegionId { get; set; } 
        public long CompanyId { get; set; } 
        //public List<SubClients> SubClients { get; set; } 
    }
}
