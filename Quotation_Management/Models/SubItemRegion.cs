using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Quotation_Management.Models
{
    public class SubItemRegion
    {
        [Key]
        public long Id { get; set; } 
        public long MasterId { get; set; }
        public string RegionId { get; set; }
    }
}
