using Quotation_Management.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Quotation_Management.Models
{
    public partial class AppPagePermission 
    {
        [Key]
        public long AppPagePermissionId { get; set; } 
        public bool Create { get; set; }
        public bool Read { get; set; }
        public bool Update { get; set; }
        public bool Delete { get; set; }
        public string RoleId { get; set; }
        public long AppPageId { get; set; }
        public AppPage AppPage { get; set; }
    }
}
