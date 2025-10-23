using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Quotation_Management.Models
{
    public class Quotation : Base
    {

        public Quotation()
        {
            Master_Detail_Tables = new List<Master_Detail_Table>();
        }

        [Key]
        public long QuotationId { get; set; }
        public long? ClientId { get; set; }
        public string ClientReference { get; set; }
        public long CompanyId { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public string Status { get; set; }
        public string QuotationStatus { get; set; }
        public decimal VAT { get; set; }
        public decimal Discount { get; set; }
        public bool Flat { get; set; }
        public bool Percentage { get; set; }
        public bool IsFlat { get; set; }
        public bool IsPerItem { get; set; }
        //***Terms and conditions**** 
        public string TermsConditions { get; set; }
        public string Manager { get; set; }
        public string Total { get; set; }
        public string Admin { get; set; }
        public string Text { get; set; }
        public DateTime EmailDate { get; set; }
        public List<Master_Detail_Table> Master_Detail_Tables { get; set; }

    }
}
