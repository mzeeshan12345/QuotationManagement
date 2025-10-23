using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Quotation_Management.Models
{
    public class ClientAssign : Base
    {
        [Key]
        public long ClientAssignId { get; set; }
        public string UserId { get; set; }
        public long ClientId { get; set; }
        public long SubClientId { get; set; }
        public bool Read { get; set; }
    }
}
