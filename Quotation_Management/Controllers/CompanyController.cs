using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Quotation_Management.Helpers;
using Quotation_Management.Models;
using Quotation_Management.Repos.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Quotation_Management.Controllers
{
    [Authorize]
    public class CompanyController : Controller
    {
        private ICompanyRepo _companyRepo;
        private IBankRepo _bankRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        public IHttpContextAccessor _httpContextAccessor;

        public CompanyController(UserManager<ApplicationUser> userManager ,ICompanyRepo companyRepo, IBankRepo bankRepo, IHttpContextAccessor httpContextAccessor)
        {
            _companyRepo = companyRepo;
            _bankRepo = bankRepo;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetBanks()
        {
            var bnks = _bankRepo.GetAll();
            return Json(bnks);
        }
        
        [HttpGet]
        public JsonResult GetCompanyBank(long iD)
        {
            var bnksCmp = _bankRepo.GetAll().Where(cp=> cp.CompanyId == iD && cp.Active == true).LastOrDefault();
            return Json(bnksCmp);
        } 
        
        public JsonResult GetCompanies()
        {
            var cmp = _companyRepo.GetAll();
            return Json(cmp);
        }
        [HttpGet]
        public JsonResult GetActiveCompanies(long cmpID)
        {
            var cmp = _companyRepo.GetAll().Where(s=> s.Status == true && s.CompanyId == cmpID);
            return Json(cmp);
        }
        
        [HttpGet]
        public JsonResult GetCompany(long cmpID)
        {
            var cmpData = _companyRepo.GetAll(fc=> fc.Banks).Where(c=> c.CompanyId == cmpID).LastOrDefault();
            if (cmpData != null)
            {
                return Json(cmpData);

            }
            else { 
                return Json("");
                }
        }
        public IActionResult AddCompany()
        {
            return View();
        }
        public IActionResult UpdateCompany()
        {
            return View();
        }
         public async Task<IActionResult> UpdateCompanyDetail(IFormFile f, Company model)
        {
            var datetime = DateTime.Now;
            var company = _companyRepo.GetFirst(x => x.CompanyId  == model.CompanyId);

            if(f != null)
            {
                string storePath = f.Name;
                string docFolder = Path.Combine("CompanyImages", storePath);
                 
                try
                {
                    if (f == null || f.Length == 0)
                    {
                        return Content("File not selected");
                    }

                    string path = FileUploadHandler.GetFilePathForUpload(docFolder, f.FileName);

                    using (FileStream stream = new FileStream(path, FileMode.Create))
                    {
                        await f.CopyToAsync(stream);
                    }
                }
                catch (Exception e)
                {
                    return Json(new { result = "Upload Failed", error = e.Message });
                }

                string filePath = FileUploadHandler.FileReturnPath(docFolder, f.FileName); 
                company.FilePath = filePath;
                _companyRepo.Update(company);
                 
                return new OkObjectResult(new { error = false, message = "Saved" });

            }  
            model.FilePath = company.FilePath;
        

            foreach (var itms in model.Banks)
                {
                 if (itms.BankId == null || itms.BankId == 0)
                    {
                        itms.CreatedAt = datetime;
                        itms.CompanyId = model.CompanyId;
                        _bankRepo.Add(itms);
                    }
                    else
                    {
                        itms.EditedAt = datetime; 
                        _bankRepo.Update(itms);
                    }
                };
            _companyRepo.Update(model);

            return new OkObjectResult(new { error = false, message = "Update Successfully" });
        }


        private string GenAutoReferenceCode()
        {
            try
            {
                var usr = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
                var user = _userManager.FindByNameAsync(usr).Result;
                var company = _companyRepo.GetAll().Where(x => x.ReferenceNo != null && x.ReferenceNo != "").LastOrDefault();
                if (company.ReferenceNo == null)
                {
                    return "LS0000001";
                }
                else
                {

                    var cmp = company.ReferenceNo;
                    string value = cmp.ToString();
                    string number = Regex.Match(value, "[0-9]+$").Value;
                    return value.Substring(0, value.Length - number.Length) + (long.Parse(number) + 1).ToString().PadLeft(number.Length, '0');
                }
            }
            catch (NullReferenceException)
            {
                return "Invalid Reference Number";
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddCompanyDetail(Company model, IFormFile f)
        {
            var daTetime = DateTime.Now; 
            string storePath = f.Name;
            string docFolder = Path.Combine("CompanyImages", storePath);

            try
            {
                if (f == null || f.Length == 0)
                {
                    return Content("File not selected");
                }

                string path = FileUploadHandler.GetFilePathForUpload(docFolder, f.FileName);

                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    await f.CopyToAsync(stream);
                }
            }
            catch (Exception e)
            {
                return Json(new { result = "Upload Failed", error = e.Message });
            }

            string filePath = FileUploadHandler.FileReturnPath(docFolder, f.FileName);
            model.FilePath = filePath;
            model.ArabicName =  CultureInfo.GetCultureInfo("ar-SA").ToString();
            model.CreatedAt = daTetime;
            model.ReferenceNo = GenAutoReferenceCode();
            model.Status = true; 
             _companyRepo.Add(model);

            #region commentcode
            //if (xdc != null || xdc.Count() > 0)
            //{
            //    var bankdetails = new List<Banks>();
            //    foreach (var itm in xdc)
            //    {
            //        var bnks = new Banks
            //        {
            //            AccountNumber = itm.AccountNumber,
            //            BankName = itm.BankName,
            //            IBAN = itm.IBAN,
            //            CompanyId = itm.CompanyId,
            //            SwiftCode = itm.SwiftCode,
            //            CreatedAt = daTetime
            //        };
            //        bankdetails.Add(bnks);
            //    }
            //    _bankRepo.AddRange(bankdetails);

            //    return Json(bankdetails);
            //}
            //else
            //{
            //    return new OkObjectResult(new { Error = " Detail Required" });

            //}

            #endregion

            return new OkObjectResult(new { error = true, message = "Saved" });
        }


        [HttpPost]
        public void DeleteCompany(long key)
        {
            var cmps = _companyRepo.Find(key);

            _companyRepo.Delete(cmps);

        }
        private void StoreInFolder(IFormFile file, string fileName)
        {
            using (FileStream fs = System.IO.File.Create(fileName))
            {
                file.CopyTo(fs);
                fs.Flush();
            }
        }

    
    }
}