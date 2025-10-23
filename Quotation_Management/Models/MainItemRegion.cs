using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quotation_Management.Models
{
    public class MainItemRegion
    {
        public long Id { get; set; }
        public string RegionId { get; set; }
        public long MainItemId { get; set; }
    }
}
