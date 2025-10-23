using Quotation_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quotation_Management.ViewModels
{
    public class AppPagePermissionViewModel
    {
        public long AppPageId { get; set; }
        public string PageName { get; set; }
        public string Icon { get; set; }
        public string PageUrl { get; set; }
        public string PageType { get; set; } 
        public long AppPagePermissionId { get; set; }
        public bool Create { get; set; }
        public bool Read { get; set; }
        public bool Update { get; set; }
        public bool Delete { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
