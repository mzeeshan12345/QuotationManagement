using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Quotation_Management.Models
{
    public class QuotationHistoryDetail : Base
    {
        [Key]
        public long Id { get; set; }
        public long MasterDetailId { get; set; }
        public decimal Payable { get; set; }
        public string Quantity { get; set; }
        public decimal Total { get; set; }
        public decimal Discount { get; set; }
        public string Price { get; set; }
        public long? MainItemId { get; set; }
        public long? MasterId { get; set; }
        public long QuotationId { get; set; }
        public long QuotationHistoryId { get; set; }
    }
}
