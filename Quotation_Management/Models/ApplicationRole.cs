using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quotation_Management.Models
{
    public class ApplicationRole : IdentityRole
    {
        public long CompanyId { get; set; }
    }
}
