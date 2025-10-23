using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Quotation_Management.Models
{
    public class Master_Table : Base
    { 

        [Key]
        public long MasterId { get; set; }
        public string Item { get; set; }
        public string Price { get; set; }
        public long CompanyId { get; set; }
        public long MainItemId { get; set; }
        public List<SubItemRegion> SubItemRegions { get; set; }
    }
}
