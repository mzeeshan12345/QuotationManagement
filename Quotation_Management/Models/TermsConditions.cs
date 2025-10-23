using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Quotation_Management.Models
{
    public class TermsConditions
    {
        [Key]
        public long TCId { get; set; }
        public string Remarks { get; set; }
        public string Delivery { get; set; }
        public string CMEApproval { get; set; }
        public string Payment { get; set; }
        public string Validity { get; set; }
    }
}
