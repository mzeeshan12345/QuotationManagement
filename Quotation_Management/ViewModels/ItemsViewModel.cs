using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quotation_Management.ViewModels
{
    public class ItemsViewModel
    {
         public long MasterId { get; set; }
        public long MainItemId { get; set; }
        public long CompanyId { get; set; }
        public long RegionId { get; set; }
        public string Name { get; set; }
        public string Item { get; set; }
        public string Price { get; set; }
    }
}
