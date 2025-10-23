using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quotation_Management.Models
{
    public abstract class Base
    {
        public DateTime? CreatedAt { get; set; }
        public DateTime? EditedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string CreatedBy { get; set; }
        public string EditedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
