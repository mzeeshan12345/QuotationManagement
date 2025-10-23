using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Quotation_Management.Models;
using Quotation_Management.Repos.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quotation_Management.Controllers
{
    [Authorize]
    public class QuotationController : Controller
    {
        private IMasterRepository _masterRepo;
        private IRegionRepo _regionRepo;
        private IMainItemRepo _mainItemRepo;
        private IMainItemRegionsRepo _mainItemRegionRepo;
        private ISubItemRegionsRepo _subItemRegionRepo;
        private IQuotationRepo _quoteRepo;
        private IQuotationHistoryRepo _quoteHistoryRepo;
        private IQuotationHistoryDetailRepo _quoteHistoryDetailRepo;
        private IClientRepo _custoMerRepo;
        private ISubClientRepo _subcustoMerRepo;
        private IMasterDetailRepository _masterDetailRepo;
        public IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        public QuotationController(IMasterRepository masterRepo,
            IRegionRepo regionRepo,
            IMainItemRepo mainItemRepo,
            IMainItemRegionsRepo mainItemRegionRepo,
            ISubItemRegionsRepo subItemRegionRepo,
            IQuotationRepo quoteRepo,
            IQuotationHistoryRepo quoteHistoryRepo,
            IQuotationHistoryDetailRepo quoteHistoryDetailRepo,
            IClientRepo custoMerRepo,
            ISubClientRepo subcustoMerRepo,
            IMasterDetailRepository masterDetailRepo,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _custoMerRepo = custoMerRepo;
            _masterRepo = masterRepo;
            _quoteRepo = quoteRepo;
            _quoteHistoryRepo = quoteHistoryRepo;
            _quoteHistoryDetailRepo = quoteHistoryDetailRepo;
            _regionRepo = regionRepo;
            _mainItemRegionRepo = mainItemRegionRepo;
            _subItemRegionRepo = subItemRegionRepo;
            _subcustoMerRepo = subcustoMerRepo;
            _mainItemRepo = mainItemRepo;
            _masterDetailRepo = masterDetailRepo;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult MainItems()
        {
            return View();
        }

        public IActionResult Items()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetItemList(long CompanyId)
        { 
            var quote = _masterRepo.GetAll().Where(s => s.CompanyId == CompanyId);
            return Json(quote);
        }
        
        [HttpGet]
        public JsonResult GetItems()
        {
            var usr = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
            var cmp = _userManager.FindByNameAsync(usr).Result;
            var quote = _masterRepo.GetAll().Where(s => s.CompanyId == cmp.CompanyId);
            return Json(quote);
        }

        [HttpGet]
        public JsonResult GetAllSubItems()
        {
            var usr = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
            var cmp = _userManager.FindByNameAsync(usr).Result;
            var quote = _masterRepo.GetSubItemsByRegion().Where(s => s.CompanyId == cmp.CompanyId);
            return Json(quote);
        }

        [HttpGet]
        public JsonResult GetItemsbyMainItems(long id)
        {
            var usr = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
            var cmp = _userManager.FindByNameAsync(usr).Result;
            //var quote = _masterRepo.GetAll().Where(dx=> dx.MainItemId == id && dx.CompanyId == cmp.CompanyId);
            var quote = _masterDetailRepo.GetItemsbymain();
            var filterQuote = quote.Where(dx => dx.MainItemId == id && dx.CompanyId == cmp.CompanyId);
            return Json(quote);
        }

        [HttpGet]
        public JsonResult GetMainItem(long id)
        {
            var miTem = _mainItemRepo.GetAll().Where(mt => mt.MainItemId == id).LastOrDefault();
            return Json(miTem);
        }

        [HttpGet]
        public JsonResult Getregionsss(long id)
        {
            var rgnsList = _regionRepo.GetAll().Where(r => r.RegionId == id).LastOrDefault().Name;
            return Json(rgnsList);
        }
        [HttpGet]
        public JsonResult GetMainItemsList(long CompanyId)
        { 
            var miTems = _mainItemRepo.GetAll().Where(fc => fc.CompanyId == CompanyId);
            return Json(miTems);
        }
        
        [HttpGet]
        public JsonResult GetMainItems()
        {
            var usr = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
            var cmp = _userManager.FindByNameAsync(usr).Result;
            var miTems = _mainItemRepo.GetAll().Where(fc => fc.CompanyId == cmp.CompanyId);
            return Json(miTems);
        }


        [HttpGet]
        public JsonResult GetMainItemRegions(long id)
        {
            var miTems = _mainItemRegionRepo.GetAll().Where(i => i.MainItemId == id);
            return Json(miTems);
        }
  
         [HttpPost]
        public IActionResult AssignQuotations(List<Quotation> quotationS, string salesRep)
        {

            var usr = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
            var currentUser = _userManager.FindByNameAsync(usr).Result;

            foreach (var item in quotationS)
            {
            var data = _quoteRepo.GetAll().Where(x => x.QuotationId == item.QuotationId);
                item.CreatedBy = salesRep;
               _quoteRepo.Update(item);

                var quoteHist = new QuotationHistory
                {
                    CompanyId = item.CompanyId,
                    AssignedBy = currentUser.Name,
                    UserId = item.UserId,
                    CreatedAt = item.CreatedAt,
                    CreatedBy = item.CreatedBy,
                    EditedAt = item.EditedAt,
                    EditedBy = currentUser.Name,
                    QuotationId = item.QuotationId,
                    ClientId = item.ClientId,
                    ClientReference = item.ClientReference,
                    DeletedAt = item.DeletedAt,
                    Description = item.Description,
                    Discount = item.Discount,
                    IsDeleted = item.IsDeleted,
                    QuotationStatus = item.QuotationStatus,
                    Status = item.Status,
                    Manager = item.Manager,
                    Admin = item.Admin,
                    Text = item.Text,
                    Flat = item.Flat,
                    IsFlat = item.IsFlat,
                    IsPerItem = item.IsPerItem,
                    Percentage = item.Percentage,
                    TermsConditions = item.TermsConditions,
                    VAT = item.VAT,
                };
                _quoteHistoryRepo.Add(quoteHist);

            }
            return new OkObjectResult(new { result = "Successfully Assigned" });
        }


        [HttpGet]
        public JsonResult GetSubItemRegions(long id)
        {
            var siTems = _subItemRegionRepo.GetAll().Where(z => z.MasterId == id);
            if (siTems != null)
            {
                return Json(siTems);
            }
            return Json("");
        }

        [HttpGet]
        public JsonResult GetCurrentItemRegions(long id)
        {
            var miTems = _mainItemRegionRepo.GetCurrentRegions(id);
            return Json(miTems);
        }

        [HttpGet]
        public JsonResult Getregions()
        {
            var rgns = _regionRepo.GetAll();
            return Json(rgns);
        }

        [HttpGet]
        public JsonResult GetMainItemssss()
        {
            var miTems = _mainItemRepo.GetAllItems();
            return Json(miTems);
        }

        [HttpGet]
        public JsonResult GetAllItems()
        {
            var usr = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
            var user = _userManager.FindByNameAsync(usr).Result;
            var miTems = _mainItemRepo.GetAll().Where(cd => cd.CompanyId == user.CompanyId);
            return Json(miTems);
        }

        [HttpGet]
        public JsonResult GetItemsList()
        {
            var usr = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
            var user = _userManager.FindByNameAsync(usr).Result;
            var miTems = _mainItemRepo.GetItemsByRegion().Where(g => g.CompanyId == user.CompanyId);
            return Json(miTems);
        }

        [HttpPost]
        public IActionResult CreateMainItems(MainItem model, List<string> regions)
        {
            var mT = new MainItem
            {
                CompanyId = model.CompanyId,
                Name = model.Name,
            };
            _mainItemRepo.Add(mT);
            if (regions.Count > 0)
            {
                List<MainItemRegion> regionDetails = new List<MainItemRegion>();
                foreach (var rg in regions)
                {
                    if (rg != null)
                    {
                        var ur = new MainItemRegion
                        {
                            RegionId = rg,
                            MainItemId = mT.MainItemId,
                        };
                        regionDetails.Add(ur);
                    }

                }
                _mainItemRegionRepo.AddRange(regionDetails);
            }
            else
            {
                return new OkObjectResult(new { Error = "Please Select Region" });
            }
            return new OkObjectResult(new { result = "Successfully Added new item" });
        }


        [HttpPost]
        public IActionResult EditMainItems(MainItem model, List<string> regions)
        {
            var mT = _mainItemRepo.GetAll().Where(fc => fc.MainItemId == model.MainItemId).LastOrDefault();

            if (mT != null)
            {
                mT.CompanyId = model.CompanyId;
                mT.Name = model.Name;
            }
            _mainItemRepo.Update(mT);
            if (regions.Count > 0)
            {
                var dxc = _mainItemRegionRepo.GetAll().Where(c => c.MainItemId == mT.MainItemId);
                _mainItemRegionRepo.DeleteRange(dxc);

                List<MainItemRegion> regionDetails = new List<MainItemRegion>();
                foreach (var rg in regions)
                {
                    if (rg != null)
                    {
                        var ur = new MainItemRegion
                        {
                            RegionId = rg,
                            MainItemId = mT.MainItemId,
                        };
                        regionDetails.Add(ur);
                    }

                }
                _mainItemRegionRepo.AddRange(regionDetails);
            }
            else
            {
                return new OkObjectResult(new { Error = "Please Select Region" });
            }
            return new OkObjectResult(new { result = "Successfully Added new item" });
        }


        [HttpPost]
        public JsonResult UpdateMainItems(MainItem model)
        {
            var umiTems = _mainItemRepo.Update(model);
            return Json(umiTems);
        }



        [HttpPost]
        public JsonResult AddMainItem(MainItem model)
        {
            var miTems = _mainItemRepo.Add(model);
            return Json(miTems);
        }

        [HttpPost]
        public JsonResult UpdateMainItem(MainItem model)
        {
            var umiTms = _mainItemRepo.Update(model);
            return Json(umiTms);
        }

        [HttpPost]
        public void DeleteMainItem(long key)
        {
            var itmS = _mainItemRepo.Find(key);

            _mainItemRepo.Delete(itmS);

        }


        [HttpPost]
        public JsonResult AddMainRegion(MainItemRegion model)
        {
            var umiTems = _mainItemRegionRepo.Add(model);
            return Json(umiTems);
        }

        [HttpPost]
        public JsonResult UpdateMainRegion(MainItemRegion model)
        {
            var umiTems = _mainItemRegionRepo.Update(model);
            return Json(umiTems);
        }

        [HttpPost]
        public void DeleteMainRegion(long key)
        {
            var rgs = _mainItemRegionRepo.Find(key);

            _mainItemRegionRepo.Delete(rgs);

        }




        [HttpPost]
        public IActionResult CreateSubItems(Master_Table model, List<string> regions)
        {
            var mT = new Master_Table
            {
                CompanyId = model.CompanyId,
                Item = model.Item,
                MainItemId = model.MainItemId,
                Price = model.Price
            };
            _masterRepo.Add(mT);
            if (regions.Count > 0)
            {
                List<SubItemRegion> regionDetails = new List<SubItemRegion>();
                foreach (var rg in regions)
                {
                    if (rg != null)
                    {
                        var ur = new SubItemRegion
                        {
                            RegionId = rg,
                            MasterId = mT.MasterId
                        };
                        regionDetails.Add(ur);
                    }

                }
                _subItemRegionRepo.AddRange(regionDetails);
            }
            else
            {
                return new OkObjectResult(new { Error = "Please Select Region" });
            }
            return new OkObjectResult(new { result = "Successfully Added new item" });
        }


        [HttpPost]
        public JsonResult AddSubItem(Master_Table model)
        {
            var miTems = _masterRepo.Add(model);
            return Json(miTems);
        }

        [HttpPost]
        public JsonResult UpdateSubItem(Master_Table model)
        {
            var umiTms = _masterRepo.Update(model);
            return Json(umiTms);
        }

        [HttpPost]
        public void DeleteSubItem(long key)
        {
            var itmS = _masterRepo.Find(key);

            _masterRepo.Delete(itmS);

        }


        [HttpPost]
        public JsonResult AddSubRegion(SubItemRegion model)
        {
            var umiTems = _subItemRegionRepo.Add(model);
            return Json(umiTems);
        }

        [HttpPost]
        public JsonResult UpdateSubRegion(SubItemRegion model)
        {
            var umiTems = _subItemRegionRepo.Update(model);
            return Json(umiTems);
        }

        [HttpPost]
        public void DeleteSubRegion(long key)
        {
            var rgs = _subItemRegionRepo.Find(key);

            _subItemRegionRepo.Delete(rgs);

        }


        [HttpGet]
        public JsonResult GetItemLists()
        {
            var usr = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
            var user = _userManager.FindByNameAsync(usr).Result;
            var quoteList = _masterRepo.GetAll().Where(cmp => cmp.CompanyId == user.CompanyId);
            return Json(quoteList);
        }

        [HttpGet]
        public JsonResult GetQuotations()
        {
            var usr = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
            var user = _userManager.FindByNameAsync(usr).Result;
            var quoteAll = _quoteRepo.GetAll(d => d.Master_Detail_Tables)
                .Where(itm => itm.CompanyId == user.CompanyId && itm.CreatedBy == user.Id).ToList().OrderBy(dx => dx.QuotationId);
            return Json(quoteAll);
        }
        
        [HttpGet]
        public JsonResult GetQuotationss(long id)
        {
            var usr = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
            var user = _userManager.FindByNameAsync(usr).Result;
            var quoteAll = _quoteHistoryRepo.GetAll(d => d.QuotationHistoryDetails).Where(itm => itm.CompanyId == user.CompanyId && itm.CreatedBy == user.Id && itm.QuotationId == id);
            return Json(quoteAll);
        }


        [HttpGet]
        public JsonResult GetAllQuotations()
        {
            var quoteAll = _quoteRepo.GetAll(d => d.Master_Detail_Tables).Where(itm => itm.Status != "Draft");
            return Json(quoteAll);
        }
        
        
        [HttpGet]
        public JsonResult GetAllHQuotations(long iD)
        {
            var quoteAll = _quoteHistoryRepo.GetAll(d => d.QuotationHistoryDetails).Where(itm => itm.Status != "Draft" && itm.QuotationId == iD);
            return Json(quoteAll);
        }

        [HttpGet]
        public JsonResult GetQuotationLists(int companyID)
        {
            var quotesAll = _quoteRepo.GetAll().Where(dx => dx.CompanyId == companyID);
            return Json(quotesAll);
        }

        [HttpGet]
        public JsonResult GetQuotation(long quoteID)
        {
            var quote = _quoteRepo.GetAll(m => m.Master_Detail_Tables).Where(dx => dx.QuotationId == quoteID).LastOrDefault();
            return Json(quote);
        }
        
        
        [HttpGet]
        public JsonResult CheckQuotationHistory(long quoteID, string status)
        {
            var quoteHistoryLast = _quoteHistoryRepo.GetAll().Where(dx => dx.QuotationId == quoteID && dx.QuotationStatus == status).LastOrDefault();
            return Json(quoteHistoryLast);
        }

        [HttpGet]
        public JsonResult GetCustomers()
        {
            var usr = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
            var user = _userManager.FindByNameAsync(usr).Result;
            var dxd = _custoMerRepo.GetAll().Where(cp => cp.CompanyId == user.CompanyId);
            return Json(dxd);
        }



        [HttpGet]
        public JsonResult GetItem(long ID)
        {
            var mr = _masterRepo.GetAll(d => d.MasterId == ID);
            return Json(mr);
        }

        [HttpPost]
        public IActionResult AddMaster(Master_Table model)
        {
            var usr = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
            var user = _userManager.FindByNameAsync(usr).Result;
            if (user != null)
            {
                model.CompanyId = user.CompanyId;
                var mrT = _masterRepo.Add(model);
                return Json(mrT);
            }
            return new OkObjectResult(new { Error = "User Detail Required" });
        }

        public IActionResult CreateQuotation()
        {
            var usr = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
            var user = _userManager.FindByNameAsync(usr).Result;
            ViewData["Customers"] = _custoMerRepo.GetAll().Where(cp => cp.CompanyId == user.CompanyId)
            .Select(v => new SelectListItem { Text = v.ClientName, Value = v.ClientId.ToString() });

            return View();
        }

        [HttpPost]
        public IActionResult AddQuotation(Quotation model, List<Master_Detail_Table> quotdata)
        {
            var datetime = DateTime.Now;
            var usr = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
            var user = _userManager.FindByNameAsync(usr).Result;
            if (user != null)
            {
                model.CompanyId = user.CompanyId;
                model.UserId = user.Id;
                model.CreatedBy = user.Id;
                model.Status = "Draft"; 
                model.CreatedAt = datetime;
                model.QuotationStatus = "Proposal"; 
                model.Manager = "Pending";
                model.Admin = "Pending";
                var mrTD = _quoteRepo.Add(model);

                if (mrTD > 0 && mrTD != null)
                {
                    var details = new List<Master_Detail_Table>();
                    foreach (var item in quotdata)
                    {
                        var master = new Master_Detail_Table
                        {
                            QuotationId = model.QuotationId,
                            MasterId = item.MasterId,
                            Payable = item.Payable,
                            Price = item.Price,
                            CreatedBy = user.Id,
                            Discount =item.Discount,
                            MainItemId = item.MainItemId,
                            DeletedAt = item.DeletedAt,
                            EditedAt = item.EditedAt,
                            EditedBy = item.EditedBy,
                            Quantity = item.Quantity,
                            CreatedAt = datetime,
                            Total = item.Total
                        };
                        details.Add(master);
                    }
                    _masterDetailRepo.AddRange(details);
                    return Json(mrTD);
                }
                else
                {
                    return new OkObjectResult(new { Error = " Detail Required" });

                }
            }
            else
            {
                return new OkObjectResult(new { Error = "No User Found" });

            }
            return new OkObjectResult(new { Mess = " Done!" });
        }


        [HttpPost]
        public IActionResult SubmitQuotation(Quotation model, List<Master_Detail_Table> quotdata)
        {
            var datetime = DateTime.Now;
            var usr = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
            var user = _userManager.FindByNameAsync(usr).Result;
            if (user != null)
            {
                model.CompanyId = user.CompanyId;
                model.UserId = user.Id;
                if (model.Discount == null || model.Discount == 0)
                {
                    model.Status = "Approved";
                }
                else if (model.Discount != null || model.Discount != 0)
                {
                    model.Status = "Pending";
                }
                model.CreatedAt = datetime;
                model.CreatedBy = user.Id;
                //model.QuotationStatus = "Proposal";
                //model.Manager = "Pending";
                //model.Admin = "Pending";
                var mrTD = _quoteRepo.Add(model); 

                var quoteHist = new QuotationHistory
                {
                    CompanyId = model.CompanyId,
                    UserId = model.UserId,
                    CreatedAt = model.CreatedAt,
                    CreatedBy = user.Id,
                    EditedAt = model.EditedAt,
                    EditedBy = user.Name,
                    QuotationId = model.QuotationId,
                    ClientId = model.ClientId,
                    ClientReference = model.ClientReference,
                    DeletedAt = model.DeletedAt,
                    Description = model.Description,
                    Discount = model.Discount,
                    IsDeleted = model.IsDeleted,
                    QuotationStatus = model.QuotationStatus,
                    Status = model.Status,
                    Manager = model.Manager,
                    Admin = model.Admin,
                    Text = model.Text,
                    Flat = model.Flat,
                    IsFlat = model.IsFlat,
                    IsPerItem = model.IsPerItem,
                    Percentage = model.Percentage,
                    TermsConditions = model.TermsConditions,
                    VAT = model.VAT,
                };
                 _quoteHistoryRepo.Add(quoteHist);

                if (mrTD > 0 && mrTD != null)
                {
                    var details = new List<Master_Detail_Table>();
                    var details2 = new List<QuotationHistoryDetail>();
                    foreach (var item in quotdata)
                    {
                        var master = new Master_Detail_Table
                        {
                            QuotationId = model.QuotationId,
                            MasterId = item.MasterId,
                            Payable = item.Payable,
                            CreatedBy = item.CreatedBy,
                            Price = item.Price,
                            MainItemId = item.MainItemId,
                            Discount = item.Discount,
                            EditedAt = item.EditedAt,
                            EditedBy = user.Name,
                            DeletedAt = item.DeletedAt,
                            IsDeleted = item.IsDeleted, 
                            Quantity = item.Quantity,
                            CreatedAt = datetime,
                            Total = item.Total
                        };
                        details.Add(master);

                        var master1 = new QuotationHistoryDetail
                        {
                            QuotationId = model.QuotationId,
                            MasterDetailId = item.MasterDetailId,
                            MasterId = item.MasterId, 
                            CreatedBy = item.CreatedBy,
                            Payable = item.Payable,
                            QuotationHistoryId = quoteHist.QuotationHistoryId,
                            Price = item.Price,
                            Quantity = item.Quantity,
                            Discount = item.Discount,
                            MainItemId = item.MainItemId,
                            DeletedAt = item.DeletedAt,
                            IsDeleted = item.IsDeleted,
                            EditedAt = item.EditedAt,
                            EditedBy = item.EditedBy,
                            CreatedAt = datetime,
                            Total = item.Total
                        };
                        details2.Add(master1);
                    }
                    _masterDetailRepo.AddRange(details); 
                     _quoteHistoryDetailRepo.AddRange(details2);

                    return Json(mrTD);
                }
                else
                {
                    return new OkObjectResult(new { Error = " Detail Required" });

                }
            }
            else
            {
                return new OkObjectResult(new { Error = "No User Found" });

            }
            return new OkObjectResult(new { Mess = " Done!" });
        }


        public IActionResult UpdateQuotation()
        {
            var usr = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
            var user = _userManager.FindByNameAsync(usr).Result;
            ViewData["Customers"] = _custoMerRepo.GetAll().Where(cp => cp.CompanyId == user.CompanyId)
            .Select(v => new SelectListItem { Text = v.ClientName, Value = v.ClientId.ToString() });

            return View();
        }
        
        
        public IActionResult ViewQuotation()
        {
            var usr = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
            var user = _userManager.FindByNameAsync(usr).Result;
            ViewData["Customers"] = _custoMerRepo.GetAll().Where(cp => cp.CompanyId == user.CompanyId)
            .Select(v => new SelectListItem { Text = v.ClientName, Value = v.ClientId.ToString() });

            return View();
        }


        [HttpPost]
        public IActionResult pOSTUpdateQuotation(Quotation model, List<Master_Detail_Table> quotdata)
        {
            var datetime = DateTime.Now;
            var usr = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
            var user = _userManager.FindByNameAsync(usr).Result;

            model.EditedAt = datetime;
            model.EditedBy = user.Id; 
            model.UserId = user.Id;
            //Convert.ToInt64(model.ClientId);
            var mrTD = _quoteRepo.Update(model);

            if (mrTD > 0 && mrTD != null)
            {
                var quoteHist = new QuotationHistory
                {
                    CompanyId = model.CompanyId,
                    UserId = model.UserId,
                    CreatedAt =  model.CreatedAt,
                    EditedAt = model.EditedAt,
                    EditedBy = user.Name,
                    CreatedBy = model.CreatedBy,
                    QuotationId = model.QuotationId,
                    ClientId = model.ClientId,
                    ClientReference = model.ClientReference,
                    DeletedAt = model.DeletedAt,
                    Description = model.Description,
                    Discount = model.Discount,
                    IsDeleted = model.IsDeleted,
                    QuotationStatus = model.QuotationStatus,
                    Status = model.Status,
                    Admin = model.Admin,
                    Manager = model.Manager,
                    Text = model.Text, 
                    TermsConditions = model.TermsConditions,
                    VAT = model.VAT,
                };
                 _quoteHistoryRepo.Add(quoteHist);

                var details = new List<QuotationHistoryDetail>();
                foreach (var item in quotdata)
                {
                    item.QuotationId = model.QuotationId;
                    if (item.MasterDetailId == null || item.MasterDetailId == 0)
                    {
                        item.CreatedAt = datetime; 
                        item.EditedBy = user.Name;
                        _masterDetailRepo.Add(item);

                        var master = new QuotationHistoryDetail
                        {
                            QuotationId = item.QuotationId,
                            QuotationHistoryId = quoteHist.QuotationHistoryId,
                            MasterDetailId = item.MasterDetailId,
                            MasterId = item.MasterId,
                            Payable = item.Payable,
                            Price = item.Price,
                            Quantity = item.Quantity,
                            Total = item.Total,
                            Discount = item.Discount, 
                            MainItemId = item.MainItemId,
                            CreatedAt = datetime, 
                            EditedAt = item.EditedAt,
                            EditedBy = item.EditedBy,
                            IsDeleted = item.IsDeleted,
                            DeletedAt = item.DeletedAt
                        };
                        _quoteHistoryDetailRepo.Add(master);
                    }
                    else
                    {
                        item.EditedAt = datetime;
                        item.EditedBy = user.Name;
                        _masterDetailRepo.Update(item);

                        var master = new QuotationHistoryDetail
                        {
                            QuotationId = item.QuotationId,
                            MasterDetailId = item.MasterDetailId,
                            MasterId = item.MasterId,
                            Payable = item.Payable,
                            QuotationHistoryId = quoteHist.QuotationHistoryId,
                            Price = item.Price,
                            Quantity = item.Quantity,
                            Discount = item.Discount,
                            MainItemId = item.MainItemId,
                            DeletedAt = item.DeletedAt,
                            IsDeleted = item.IsDeleted,
                            EditedAt = item.EditedAt,
                            EditedBy = item.EditedBy,
                            CreatedAt = item.CreatedAt,
                            Total = item.Total
                        };
                        _quoteHistoryDetailRepo.Add(master);
                    }

                 };
            }
            else
            {
                return new OkObjectResult(new { Error = "" });
            }

            return new OkObjectResult(new { Mess = " Done!" });
        }
         


        [HttpPost]
        public JsonResult UpdateQuotation(Master_Detail_Table model)
        {
            var mrTD = _masterDetailRepo.Update(model);
            return Json(mrTD);
        }

        [HttpPost]
        public JsonResult UpdateMaster(Master_Table model)
        {
            var mrT = _masterRepo.Update(model);
            return Json(mrT);
        }

    }
}
