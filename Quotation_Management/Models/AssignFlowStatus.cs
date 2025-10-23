using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Quotation_Management.Models
{
    public class AssignFlowStatus
    {
        [Key]
        public long AFSId { get; set; }
        public string FlowStatus { get; set; }
        public string UserId { get; set; }
        public bool ManagerApproval { get; set; }
        public bool AdminApproval { get; set; }
    }
}
