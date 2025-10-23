using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Quotation_Management.Models
{
    public class MainItem
    { 
        [Key]
        public long MainItemId { get; set; }
        public string Name { get; set; }
        public long CompanyId { get; set; }
        public List<MainItemRegion> MainItemRegions { get; set; }
    }
}
