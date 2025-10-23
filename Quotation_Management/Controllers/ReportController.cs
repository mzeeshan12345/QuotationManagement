using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Quotation_Management.Models;
using Quotation_Management.Repos.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Quotation_Management.Controllers
{
    [Authorize]
    public class ReportController :Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private IClientRepo _clientRepo;
        public IHttpContextAccessor _httpContextAccessor;

        public ReportController(UserManager<ApplicationUser> userManager,
            IClientRepo clientRepo,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _clientRepo = clientRepo;
            _httpContextAccessor = httpContextAccessor;
        }

        public long? CompanyID { get; set; }

        public IActionResult Quotation()
        {
            var usr = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
            var user =  _userManager.FindByNameAsync(usr).Result;
            ViewData["Customers"] = _clientRepo.GetAll().Where(cp => cp.CompanyId == user.CompanyId)
            .Select(v => new SelectListItem { Text = v.ClientName, Value = v.Email });
            return View();
        } 

        public async Task<IActionResult>ViewQuotation(int companyId)
        { 
            var assembly = typeof(ReportStorageWebExtension).Assembly;
            Stream resource = assembly.GetManifestResourceStream("Quotation_Management.Reports.QuotationReport.repx");
            XtraReport report = XtraReport.FromStream(resource);
            var path = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            report.Parameters["Path"].Value = path;
            report.Parameters["CompanyId"].Value = companyId;   
            report.CreateDocument(); 
            // Specify export options.
            PdfExportOptions pdfExportOptions = new PdfExportOptions()
            {
                PdfACompatibility = PdfACompatibility.PdfA1b
            };
            string dirpath = @"C:\Quotations\";
            if (!Directory.Exists(dirpath))
            {
                Directory.CreateDirectory(dirpath);
            }
            // Specify the path for the exported PDF file.  
            string pdfExportFile = dirpath + report.Name +".pdf";

            // Export the report.
            report.ExportToPdf(pdfExportFile, pdfExportOptions);


            MemoryStream ms = new MemoryStream();
            report.ExportToPdf(ms);
            //var dx = File(ms.ToArray(), "application/pdf");
            return PartialView("_Report", report);
        }


        public IActionResult Report()
        {
            return View();
        }
            public IActionResult ViewReport(int Id, int cId, int quoteID)
            {
            var assembly = typeof(ReportStorageWebExtension).Assembly;
            Stream resource = assembly.GetManifestResourceStream("Quotation_Management.Reports.QuotationReport.repx");
            XtraReport report = XtraReport.FromStream(resource);
            var path = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            report.Parameters["Path"].Value = path;
            report.Parameters["CompanyId"].Value = Id;
            report.Parameters["QuotationId"].Value = quoteID;
            report.Parameters["ClientId"].Value = cId;
            report.CreateDocument();
            // Specify export options.
            PdfExportOptions pdfExportOptions = new PdfExportOptions()
            {
                PdfACompatibility = PdfACompatibility.PdfA1b
            };
            string dirpath = @"C:\Quotations\";
            if (!Directory.Exists(dirpath))
            {
                Directory.CreateDirectory(dirpath);
            }
            // Specify the path for the exported PDF file.  
            string pdfExportFile = dirpath + report.Name + ".pdf";

            // Export the report.
            report.ExportToPdf(pdfExportFile, pdfExportOptions);

            return PartialView("_Report", report);
        }

    }
}
