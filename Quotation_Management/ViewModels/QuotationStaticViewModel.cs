using Quotation_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quotation_Management.ViewModels
{
    public class QuotationStaticViewModel
    {
        public string Status { get; set; }
        public long Total { get; set; }
        public int Count { get; set; }      
        public List<Quotation> Quotations { get; set; }
    }
}
