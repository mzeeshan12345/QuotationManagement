using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Quotation_Management.Models;
using Quotation_Management.Repos.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Quotation_Management.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IAdminRepo _adminRepo;
        private IQuotationRepo _quoteRepo;
        private ICompanyRepo _companyRepo;
        public IHttpContextAccessor _httpContextAccessor;
        private IClientRepo _custoMerRepo;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ILogger<HomeController> logger,
                        UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor,
            IClientRepo custoMerRepo,
            ICompanyRepo companyRepo,
            IAdminRepo adminRepo,
            IQuotationRepo quoteRepo)
        {
            _logger = logger;
            _quoteRepo = quoteRepo;
            _adminRepo = adminRepo;
            _companyRepo = companyRepo;
            _userManager = userManager;
            _custoMerRepo = custoMerRepo;
            _httpContextAccessor = httpContextAccessor;
        }
          
        public IActionResult Panel()
        {
            return View();
        }
        public IActionResult Index()
        {
            return View();
        } 
        public IActionResult MenuPanel()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetCompanies()
        {
            var cmp = _companyRepo.GetAll().Where(s => s.Status == true);
            return Json(cmp);
        }
        
        [HttpGet]
        public JsonResult GetUserCompanies()
        {
            var usr = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
            var user = _userManager.FindByNameAsync(usr).Result;
            var cmp = _companyRepo.GetAll().Where(s => s.Status == true && user.CompanyId == s.CompanyId);
            return Json(cmp);
        }

        public IActionResult Dashboard()
        {
             return View();
        }
        [HttpGet]
        public long? GetQuotations()
        {
            var usr = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
            var user = _userManager.FindByNameAsync(usr).Result;
            var quoteAll = _quoteRepo.GetAll().Where(itm => itm.CompanyId == user.CompanyId).Count();
            return quoteAll;
        }
        
        public JsonResult GetQuotationstatics(long companyID)
        {
            var quotesStatic = _adminRepo.GetQuotationStatics(companyID);
            return Json(quotesStatic);
        }
        
        public JsonResult GetSalesQuotationstatics()
        {
            var usr = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
            var users = _userManager.FindByNameAsync(usr).Result;
            var quotesSaleStatic = _adminRepo.GetQuotationStaticsSales(users.Id);
            return Json(quotesSaleStatic);
        }
         

        [HttpGet]
        public long? GetApprovedQuotations()
        {
            var usr = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
            var user = _userManager.FindByNameAsync(usr).Result;
            var quoteapprovedAll = _quoteRepo.GetAll().Where(itm => itm.CompanyId == user.CompanyId && itm.Status == "Approved").Count();
            return quoteapprovedAll;
        }
        
        [HttpGet]
        public long? GetApprovedSalesQuotations()
        {
            var usr = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
            var user = _userManager.FindByNameAsync(usr).Result;
            var quoteEmailAll = _quoteRepo.GetAll().Where(itm => itm.CompanyId == user.CompanyId).Count();
            return quoteEmailAll;
        }
        
        
        [HttpGet]
        public long? GetSentQuotations()
        {
            var usr = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
            var user = _userManager.FindByNameAsync(usr).Result;
            var quoteAll = _quoteRepo.GetAll().Where(itm => itm.CompanyId == user.CompanyId && itm.Status == "Approved").Count();
            return quoteAll;
        }


        [HttpGet]
        public long? GetSalesQuotations()
        {
            var usr = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
            var user = _userManager.FindByNameAsync(usr).Result;
            var quoteAll = _quoteRepo.GetAll().Where(itm => itm.CompanyId == user.CompanyId && itm.UserId == user.Id).Count();
            return quoteAll;
        }

        [HttpGet]
        public long? GetSalesApproved()
        {
            var usr = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
            var user = _userManager.FindByNameAsync(usr).Result;
            var clientS = _quoteRepo.GetAll().Where(itm => itm.CompanyId == user.CompanyId && itm.Status == "Approved").Count();
            return clientS;
        }
        [HttpGet]
        public long? GetSalesReject()
        {
            var usr = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
            var user = _userManager.FindByNameAsync(usr).Result;
            var RclientS = _quoteRepo.GetAll().Where(itm => itm.CompanyId == user.CompanyId && itm.Status == "Decline").Count();
            return RclientS;
        }

        [HttpGet]
        public long? GetClients()
        {
            var usr = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
            var user = _userManager.FindByNameAsync(usr).Result;
            var clientS = _custoMerRepo.GetAll().Where(itm => itm.CompanyId == user.CompanyId).Count();
            return clientS;
        }

        [HttpGet]
        public long? GetSalesPending()
        {
            var usr = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
            var user = _userManager.FindByNameAsync(usr).Result;
            var PenDings = _quoteRepo.GetAll().Where(itm => itm.CompanyId == user.CompanyId && itm.Status == "Pending").Count();
            return PenDings;
        }
        
        [HttpGet]
        public long? GetDraftQuotation()
        {
            var usr = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
            var user = _userManager.FindByNameAsync(usr).Result;
            var drAftS = _quoteRepo.GetAll().Where(itm => itm.CompanyId == user.CompanyId && itm.Status == "Draft").Count();
            return drAftS;
        }


        [HttpGet]
        public long? GetUsers()
        {
            var users = _userManager.Users.Where(u=> !u.Name.Contains("admin")).Count();
            return users;
        }

        [HttpGet]
        public JsonResult GetAllQuotations()
        {
            var usr = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
            var user = _userManager.FindByNameAsync(usr).Result;
            var quoteAll = _quoteRepo.GetAll(d => d.Master_Detail_Tables).Where(itm => itm.CompanyId == user.CompanyId && itm.UserId == user.Id);
            return Json(quoteAll);
        }
        
        [HttpGet]
        public JsonResult GetQuotationLists()
        {
            var usr = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
            var user = _userManager.FindByNameAsync(usr).Result;
            var quoteAll = _quoteRepo.GetAll(d => d.Master_Detail_Tables).Where(itm => itm.CompanyId == user.CompanyId && itm.Status != "Draft");
            return Json(quoteAll);
        }
        public IActionResult Privacy()
        {
            return View();
        }
        
        public IActionResult _DashboardAdmin()
        {
            return View();
        }
        public IActionResult _DashboardSales()
        {
            return View();
        }


        [HttpGet]
        public IActionResult GetDailyQuotationStatics(long companyId)
        {
            var cases = _adminRepo.GetDailyStatics(companyId);
            return Ok(cases);

        }

        [HttpGet]
        public IActionResult GetWeeklyQuotationStatics(long companyId)
        {
            var cases = _adminRepo.GetWeeklyStatics(companyId);
            return Ok(cases);

        }

        [HttpGet]
        public IActionResult GetMonthlyQuotationStatics(long companyId)
        {
            var cases = _adminRepo.GetMonthlyStatics(companyId);
            return Ok(cases);

        }
         
        public JsonResult GetStaticsByFilter(DateTime From, DateTime To, string uId, long companyId, string status)
        {
            string dx = From.ToString("yyyy/MM/dd 00:00:01");
            string dx2 = To.ToString("yyyy/MM/dd 23:59:59");
            string dx3 = uId;
            string dx4 = status;
            if (dx4 == "null" || dx3 == null || dx3 == "undefined")
            {
                dx3 = "";
            }
             if (dx4 == "null" || dx4 == null || dx4 == "undefined")
            {
                dx4 = "";
            }
            var casesStates = _adminRepo.GetFilterStatics(dx, dx2, dx3, companyId, dx4);
            return Json(casesStates);

        }
        
        public JsonResult GetStaticsByDateFilter(DateTime From, DateTime To,long companyId)
        {
            string dateFrom = From.ToString("yyyy/MM/dd 00:00:01");
            string dateTo = To.ToString("yyyy/MM/dd 23:59:59"); 
            var caseStates = _adminRepo.GetFilterStaticsByDate(dateFrom, dateTo, companyId);
            return Json(caseStates); 
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
