using Quotation_Management.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Quotation_Management.Models
{
    public partial class AppPage 
    {
        [Key]
        public long AppPageId { get; set; }
        public string PageName { get; set; }
        public string PageUrl { get; set; }
        public string Icon { get; set; }
        public string PageType { get; set; }

        public List<AppPagePermission> AppPagePermissions { get; set; }
    }
}
