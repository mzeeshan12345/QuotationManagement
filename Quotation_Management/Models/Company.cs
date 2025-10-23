using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Quotation_Management.Models
{
    public class Company : Base
    {
        public Company()
        {
            Users = new List<ApplicationUser>();
            Master_Detail_Tables = new List<Master_Detail_Table>();
            Banks = new List<Banks>();
        }
    [Key]
        public long CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string ArabicName { get; set; }
        public string ReferenceNo { get; set; }
        public string TRN { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Website { get; set; }
        public string Footer { get; set; }
        public long BankId { get; set; }
        public string FilePath { get; set; }
        public string Currency { get; set; }
        public decimal Vat { get; set; }
        public string TermsCondition { get; set; }
        public bool? Status { get; set; }
        public List<ApplicationUser> Users { get; set; }
        public List<Master_Detail_Table> Master_Detail_Tables { get; set; }
        public List<Banks> Banks { get; set; }


    }
}
