using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Quotation_Management.ViewModels
{
    public class RoleViewModel
    {
        [Required]
        public string Name { get; set; }
        public string Id { get; set; }
        public long CompanyId { get; set; }
    }
}
