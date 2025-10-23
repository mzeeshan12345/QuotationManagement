using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
 using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Quotation_Management.Helpers;
using Quotation_Management.Models;
using Quotation_Management.Reports;
using Quotation_Management.Repos.Interfaces;
 using Quotation_Management.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Quotation_Management.Controllers
{

    [Authorize]
    public class AdminController : Controller
    {
        private IConfiguration Configuration; 
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private IAppPageRepository _appPageRepo;
        private IAppPagePermissionRepository _appPermissionRepo;
        private IQuotationRepo _quoteRepo;
        private IClientRepo _clientRepo;
        private IUserRegionRepo _regionuserRepo;
        private IRegionRepo _regionRepo;
        private ISubClientRepo _subclientRepo;
        private ITermConditionRepo _termConditionRepo;
        private IAssignClientRepo _assignClientRepo;
        private IFlowStatusRepo _assignFlowRepo;
        private ICountryMasterRepo _countryRepo;
        private IAdminRepo _adminRepo;
        private ICompanyRepo _companyRepo;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public IHttpContextAccessor _httpContextAccessor;


        public AdminController(RoleManager<ApplicationRole> roleManager,
             IConfiguration _Configuration,
        IAppPageRepository appPageRepo,
            IAppPagePermissionRepository appPermissionRepo,
            IClientRepo clientRepo,
            IAdminRepo  adminRepo,
            ICompanyRepo companyRepo,
            IUserRegionRepo regionuserRepo,
            IRegionRepo regionRepo,
            ICountryMasterRepo countryRepo,
            IFlowStatusRepo assignFlowRepo,
            ISubClientRepo subclientRepo,
            IQuotationRepo quoteRepo,
            ITermConditionRepo termConditionRepo,
            IAssignClientRepo assignClientRepo,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _roleManager = roleManager; 
            _clientRepo = clientRepo;
            _subclientRepo = subclientRepo;
            _termConditionRepo = termConditionRepo;
            _userManager = userManager;
            _adminRepo = adminRepo;
            _companyRepo = companyRepo;
            _regionuserRepo = regionuserRepo;
            _appPageRepo = appPageRepo;
            _regionRepo = regionRepo;
            _assignFlowRepo = assignFlowRepo;
            _countryRepo = countryRepo;
            _assignClientRepo = assignClientRepo;
            _quoteRepo = quoteRepo;
            _appPermissionRepo = appPermissionRepo;
            _signInManager = signInManager; 
            _httpContextAccessor = httpContextAccessor;
            Configuration = _Configuration;
         }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Signup()
        {
            return View();
        }
        public IActionResult AccessDenied()
        {
            return View();
        } 
        public IActionResult Roles()
        {
            return View();
        }
        public IActionResult Regions()
        {
            return View();
        }
        [HttpPost]
        public JsonResult CreateRegions(ItemRegions model)
        {
            var reg = _regionRepo.Add(model);
            return Json(reg);
        }
        [HttpPost]
        public JsonResult UpdateRegions(ItemRegions model)
        {
            var ureg = _regionRepo.Update(model);
            return Json(ureg);
        }
         
        [HttpPost]
        public async Task<IActionResult> CreateRole(RoleViewModel role)
        {
            if (ModelState.IsValid)
            {
                var identityRole = new ApplicationRole { Name = role.Name,
                CompanyId = role.CompanyId};
                var result = await _roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    return RedirectToAction("Roles", "Admin");
                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            } 
            return View(role);
        }
        
        [HttpGet]
         public async Task<IActionResult> EditRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found";
                return View("Not Found");
            }
            var model = new EditRoleViewModel
            {
                Id = role.Id,
                Name = role.Name
            };
            foreach (var user in _userManager.Users)
            {
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }
            return View(model);
        }

        [HttpPost]
         public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.Id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {model.Id} cannot be found";
                return View("Not Found");
            }
            else
            {
                role.Name = model.Name;
                var result = await _roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("Roles");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            ViewBag.roleId = roleId;
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return View("Not Found");
            }
            var model = new List<UserRoleViewModel>();

            foreach (var user in _userManager.Users)
            {
                var userRoleViewModel = new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.Name
                };
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                }
                model.Add(userRoleViewModel);
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> model, string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return View("Not Found");
            }
            for (int i = 0; i < model.Count; i++)
            {
                var user = await _userManager.FindByIdAsync(model[i].UserId);
                IdentityResult result = null;
                if (model[i].IsSelected && !(await _userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await _userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!model[i].IsSelected && await _userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }
                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                        continue;
                    else
                    {
                        return RedirectToAction("EditRole", new { ID = roleId });
                    }
                }
            }

            return RedirectToAction("EditRole", new { ID = roleId });
        }


        [HttpGet]
        public JsonResult GetRoles(long compID)
        {
            var data = _appPageRepo.GetRoles(compID);
            if (data != null)
            {
                return Json(data);
            }
            return Json("");
        }

        [HttpGet]
        public JsonResult GetAllRegions(long comPanYID)
        {
            var rdata = _regionRepo.GetAllRegionsList(comPanYID);
            if (rdata != null)
            {
                return Json(rdata);
            }
            return Json("");
        }
        
        [HttpGet]
        public JsonResult GetRegions()
        {
            var rdata = _regionRepo.GetAllRegions();
            if (rdata != null)
            {
                return Json(rdata);
            }
            return Json("");
        }
        
        [HttpGet]
        public JsonResult GetAllRegionList()
        {
            var rdata = _regionRepo.GetAll();
            if (rdata != null)
            {
                return Json(rdata);
            }
            return Json("");
        }
        
        
        [HttpGet]
        public JsonResult GetRolesS()
        {
            var data = _appPageRepo.GetRolessss();
            if (data != null)
            {
                return Json(data);
            }
            return Json("");
        }
        
        [HttpGet]
        public JsonResult GetFlowStatuses()
        {
            var data = _assignFlowRepo.GetAll().Where(dc=> dc.UserId == null);
            if (data != null)
            {
                return Json(data);
            }
            return Json("");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAspRole(EditRoleViewModel model)
         {
            var role = await _roleManager.FindByIdAsync(model.Id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {model.Id} cannot be found";
                return View("Not Found");
            }
            else
            {
                role.Name = model.Name;
                var result =  await _roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("Roles");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return new OkObjectResult(new { ROLEID = role.Id });
        }
         
        //public IActionResult AssignPermission()
        public IActionResult AssignPages()
        {
            ViewData["Roles"] = _appPageRepo.GetRolesList().Select(r => new SelectListItem { Text = r.Name, Value = r.Id.ToString() });
            return View();
        }
        
        public IActionResult AssignClient()
        {
            ViewData["Users"] = _userManager.Users.Select(r => new SelectListItem { Text = r.Name, Value = r.Id.ToString() });
            ViewData["Clients"] = _clientRepo.GetAll().Select(r => new SelectListItem { Text = r.ClientName, Value = r.ClientId.ToString() });
            return View();
        }
        
        public JsonResult GetSubClientsByMain(long clientId)
        {
            var dxc = _subclientRepo.GetClientAssigns(clientId);
            //var dxc = _subclientRepo.GetAll().Where(cf => cf.ClientId == clientID);
            return Json(dxc);
        }
        
        public JsonResult GetAssignedSubClients(string id)
        {
            //var dxc = _subclientRepo.GetClientAssigned(id);
            var dxc = _subclientRepo.GetAll().Where(cf => cf.UserId == id);
            return Json(dxc);
        }
        
        
        public IActionResult AllPages()
        {
            var pagessAll = _appPageRepo.GetPages();
            return Ok(pagessAll);
        }
         

        [HttpGet]
        public IActionResult GetAppPages(string role)
        {
            if (role != null && role != "")
            {  
            var x = _appPermissionRepo.GetAll().Where(p => p.RoleId == role).ToList();
            if (x.Count > 0)
            {
                var appPage = _appPageRepo.GetAppPages(role);
                    return Ok(appPage);

                }
                else
                { 
                    var pageAll = _appPageRepo.GetPages();
                    return Ok(pageAll);

                }
            }
            else
            {
                var pages = _appPageRepo.GetPages();
                return Ok(pages);
            }
         }


        [HttpGet]
        public IActionResult GetPagesByRole()
        {
            var pages = _appPageRepo.GetPagesAll();
            return Ok(pages); 
        }

        [HttpPost]
        public JsonResult GetPagePermission(List<AppUserRoleViewModel> role)
        {
            var appPage = _appPageRepo.GetPermissionByRole(role); 
            return Json(appPage);
        } 
     

        [HttpPost]
        public IActionResult GetMethodPermission(List<AppUserRoleViewModel> role)
        {
            var appPage = _appPageRepo.GetMethodPermissionByRole(role);
            return Ok(appPage);
        }
        
        [HttpPost]
        public IActionResult GetFlowPermission(List<AppUserRoleViewModel> role)
        {
            var flowPage = _appPageRepo.GetFlowPermissionByRole(role);
            return Ok(flowPage);
        }
        
        [HttpGet]
        public IActionResult GetPermission()
        {
            var chckPermission = _appPageRepo.GetManagerPermission();
            return Ok(chckPermission);
        }

        [HttpGet]
        public JsonResult GetFlowStatusByUser(string iD)
        {
            var userflowStatus = _assignFlowRepo.GetAll().Where(xdx => xdx.UserId == iD);
            return Json(userflowStatus);
        }
        
        [HttpGet]
        public JsonResult GetFlowStatusByCurrentUser()
        {
            var usr = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
            var user =  _userManager.FindByNameAsync(usr).Result; 
            var userflow = _assignFlowRepo.GetAll().Where(xdx => xdx.UserId == user.Id);
            if (userflow.Count() > 0)
            {
                return Json(userflow);
            }
            else { 
            return Json("");
        }
       }
       

        [HttpPost]
        public IActionResult AddPagePermission(string roleId, List<AppPagePermissionViewModel> permission)
        {
            var appPage = _appPermissionRepo.GetAll().Where(p => p.RoleId == roleId).ToList();
            if (appPage.Count > 0)
            {
                _appPermissionRepo.DeleteRange(appPage);
            }

            foreach (var item in permission)
            {
                var x = new AppPagePermission
                {
                    AppPageId = item.AppPageId,
                    Create = item.Create,
                    Delete = item.Delete,
                    Read = item.Read,
                    RoleId = roleId,
                    Update = item.Update
                };
                _appPermissionRepo.Add(x);
            }
            return Ok(permission);
        }



        [HttpPost]
        public JsonResult AddClientPermission(List<AssignClientViewModel> permission)
        {
            var dateTime = DateTime.Now; 

            foreach (var item in permission)
            { 
                var x = new SubClients
                    {
                        SubClientId = item.SubClientId, 
                        UserId = item.UserId, 
                        Assign = item.Assign,
                        Whatsapp = item.Whatsapp,
                        RefrenceType = item.RefrenceType,
                        Reference = item.Reference,
                        Phone = item.Phone,
                        Landline = item.Landline,
                        ContactPerson = item.ContactPerson,
                        CompanyId = item.CompanyId,
                        ClientId = item.ClientId,
                        ClientName = item.SubClientName,
                        Email = item.Email,
                        EditedAt = dateTime,
                        EditedBy = item.UserId,
                    };
                    _subclientRepo.Update(x);  
                  
            }
            return Json(permission);
        }
        
        
        [HttpPost]
        public JsonResult UnAssignClients (List<SubClients> permission)
        {
            var dateTime = DateTime.Now; 

            foreach (var item in permission)
            {
                if (item.Assign == false)
                {

                    var x = new SubClients
                    {
                        SubClientId = item.SubClientId,   
                        Assign = item.Assign,
                        Whatsapp = item.Whatsapp,
                        RefrenceType = item.RefrenceType,
                        Reference = item.Reference,
                        Phone = item.Phone,
                        Landline = item.Landline,
                        ContactPerson = item.ContactPerson,
                        CompanyId = item.CompanyId,
                        ClientId = item.ClientId,
                        ClientName = item.ClientName,
                        Email = item.Email,
                        EditedAt = dateTime,
                        EditedBy = item.UserId,
                    };
                    _subclientRepo.Update(x);  
                }
            }
            return Json(permission);
        }
        public async Task<IActionResult> CurrentUser()
        {
            var usr = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
            var user = await _userManager.FindByNameAsync(usr);
            return Json(user);
        }   
        public async Task<IActionResult> CurrentUserList()
        {
            
            var usr = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
            var user = await _userManager.FindByNameAsync(usr);
            // Get the roles for the user 
            var role1 = _appPageRepo.GetRolesByUser(user.Id);

            return new OkObjectResult(new { result = role1 });
        }
        public async Task<Object> PostRegisteration(ApplicationUserModel model, List<string> roles, List<UsersRegion> regions, List<AssignFlowStatus> FlowStatus)
        {
            var applicationUser = new ApplicationUser()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Name = model.FirstName + " " + model.LastName,
                UserName = model.UserName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                DateOfBirth = Convert.ToDateTime(model.DateOfBirth), 
                 CompanyId = model.CompanyId,
                Designation = model.Designation,
                Gender = model.Gender, 
            };

            var result = await _userManager.CreateAsync(applicationUser, model.Password);

            if (result.Succeeded)
            {
                if (roles != null && roles.Count > 0)
                {
                    foreach (var role in roles)
                    {
                         await _userManager.AddToRoleAsync(applicationUser, role);
                    }
                }
                else
                {
                    return new OkObjectResult(new { Error = "Roles assigning error!" });
                }

                if (regions != null && regions.Count > 0)
                {
                    var regionDetails = new List<UsersRegion>();
                    foreach (var rgns in regions)
                    {
                        var xdx = new UsersRegion
                        {
                            RegionId = rgns.RegionId,
                            UserId = rgns.UserId
                        };
                        //await _userManager.AddToRoleAsync(applicationUser, rgns);
                        regionDetails.Add(xdx);
                    }
                    _regionuserRepo.AddRange(regionDetails);
                }
                else
                {
                    return new OkObjectResult(new { Error = "Region assigning error!" });
                }
                if (FlowStatus.Count() > 0)
                {
                    var details = new List<AssignFlowStatus>();
                    foreach (var item in FlowStatus)
                    {
                        var master = new AssignFlowStatus
                        {
                            AdminApproval = item.AdminApproval,
                            FlowStatus = item.FlowStatus,
                            ManagerApproval = item.ManagerApproval,
                            UserId = applicationUser.Id
                        };
                        details.Add(master);
                    }
                    _assignFlowRepo.AddRange(details);
                }
                else
                {
                    return new OkObjectResult(new { Error = "Flow status assigning error!" });
                }

            }
            else
            {
                return new OkObjectResult(new { Error = "Registration Error!" });
            }
            return new OkObjectResult(new { result = result.Succeeded });
        }

        public IActionResult Clients()
        {
            //var usr = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
            //var user = _userManager.FindByNameAsync(usr).Result;
            //ViewData["Regions"] = _regionRepo.GetAll().Where(cp => cp.CompanyId == user.CompanyId)
            ViewData["Regions"] = _regionRepo.GetAll()
            .Select(v => new SelectListItem { Text = v.Name, Value = v.RegionId.ToString() });
            
            ViewData["Countries"] = _countryRepo.GetAll()
            .Select(v => new SelectListItem { Text = v.Name, Value = v.Name});
            return View();
        }
         

        [HttpPost]
        public async Task<IActionResult> AddClientNew(Clients model, IFormFile f)
        { 
            var company = _companyRepo.GetAll().Where(x => x.ReferenceNo != null && x.ReferenceNo != "").LastOrDefault();
            var usr = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
            var user = _userManager.FindByNameAsync(usr).Result;
            var datetime = DateTime.Now;
            if (f != null)
            {
                string storePath = f.Name;
                string docFolder = Path.Combine("TRNFiles");

                try
                {
                    //    if (f == null)
                    //    {
                    //        return Content("File not selected");
                    //    }

                    string path = FileUploadHandler.GetFilePathForUpload(docFolder, f.FileName.Replace(" ", "_"));

                    using (FileStream stream = new FileStream(path, FileMode.Create))
                    {
                        await f.CopyToAsync(stream);
                    }
                }
                catch (Exception e)
                {
                    return Json(new { result = "Upload Failed", error = e.Message });
                }

                string filePath = FileUploadHandler.FileReturnPath(docFolder, f.FileName.Replace(" ", "_"));
                model.TRNFile = filePath;
            }
            else
            {

                if (model.RefrenceAlias != null && model.Reference == null)
                {
                    model.RefrenceType = "M";
                    model.Reference = GenReferenceSpecialClient(model.RefrenceAlias);
                }
                else if (model.RefrenceAlias == null && model.Reference == null)
                {
                    model.RefrenceAlias = "LS";
                    model.Reference = GenReferenceCode(model.CompanyId);
                    model.RefrenceType = "A";
                }
                else
                {
                    model.Reference = model.RefrenceAlias + "" + model.Reference;
                }
                model.CreatedAt = datetime;
                model.CreatedBy = user.Id;
                model.TRNFile = null;
                _clientRepo.Add(model);
            }
            return new OkObjectResult(new { error = true, message = "Saved" });
        }



        public async Task<IActionResult> UpdateClientDetail(IFormFile f, Clients model)
        {
            var datetime = DateTime.Now;
            var usr = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
            var currentUser = _userManager.FindByNameAsync(usr).Result;

            var clienT = _clientRepo.GetFirst(x => x.ClientId == model.ClientId);

            if (f != null)
            {
                string storePath = f.Name;
                string docFolder = Path.Combine("TRNFiles");

                try
                {
                    if (f == null)
                    {
                        return Content("File not selected");
                    }

                    string path = FileUploadHandler.GetFilePathForUpload(docFolder, f.FileName.Replace(" ", "_"));

                    using (FileStream stream = new FileStream(path, FileMode.Create))
                    {
                        await f.CopyToAsync(stream);
                    }
                }
                catch (Exception e)
                {
                    return Json(new { result = "Upload Failed", error = e.Message });
                }

                string filePath = FileUploadHandler.FileReturnPath(docFolder, f.FileName.Replace(" ", "_"));
                clienT.TRNFile = filePath;
                model.EditedAt = datetime;
                model.EditedBy = currentUser.Id;
                _clientRepo.Update(clienT);

                return new OkObjectResult(new { error = false, message = "Saved" });

            }
           
            model.TRNFile = clienT.TRNFile; 
            model.EditedAt = datetime;
            model.EditedBy = currentUser.Id;
            try
            {
            _clientRepo.Update(model); 

            }
            catch (Exception ex)
            {

                return Json(new { result = "Upload Failed", error = ex.Message });
            }
            return new OkObjectResult(new { error = false, message = "Update Successfully" });
        }

        public IActionResult ClientsList()
        {
            return View();
        }
        
        
        public IActionResult GetAllSubClientsList()
        { 
            var sclients = _subclientRepo.GetAll();
            return Json(sclients);
        }
        
        public IActionResult GetAllSubClients(long iD)
        { 
            var sclients = _subclientRepo.GetAll().Where(c=> c.CompanyId == iD);
            return Json(sclients);
        }
        public IActionResult GetSubClient(long id)
        {
            var client = _subclientRepo.GetAll().Where(x => x.SubClientId == id).LastOrDefault();
            return Json(client);
        }
        
        public IActionResult GetSubClientbyClient(long id)
        {
            var client = _subclientRepo.GetAll().Where(x => x.ClientId == id);
            return Json(client);
        }
        
        public IActionResult GetAllSubClientByClient(long id)
        {
            var usr = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
            var user = _userManager.FindByNameAsync(usr).Result;
            var client = _subclientRepo.GetAll().Where(x => x.ClientId == id && x.UserId == user.Id && x.CompanyId == user.CompanyId && x.Assign == true);
            return Json(client);
        }
        public JsonResult AddNewSubClient(SubClients model)
        {  
            model.RefrenceType = "A"; 
            _subclientRepo.Add(model);
            return Json(model);
        }

        public IActionResult GetAllClientsList()
        {
            var clients = _clientRepo.GetAll();
            return Json(clients);
        }
        
        public IActionResult GetAllClients(long cmpID)
        {
            var clients = _clientRepo.GetAll().Where(cmp=> cmp.CompanyId == cmpID);
            return Json(clients);
        }
    
         
        public IActionResult GetClients(long ID)
        {
            var clients = _clientRepo.GetAll().Where(cmp=> cmp.ClientId == ID).LastOrDefault();
            return Json(clients);
        }
        
        public IActionResult GetCountries()
        {
            var countries = _countryRepo.GetAll();
            return Json(countries);
        }
        
        public IActionResult GetCountriesName()
        {
            var countries = _countryRepo.GetAll().Select(x=> x.Name);
            return Json(countries);
        }
        public IActionResult GetClient(long id, long companyId)
        {
            var client = _clientRepo.GetAll().Where(x=> x.ClientId == id && x.CompanyId == companyId).LastOrDefault();
            return Json(client);
        }
        
        public IActionResult AddClient(Clients model)
        {
            var company = _companyRepo.GetAll().Where(x => x.ReferenceNo != null && x.ReferenceNo != "").LastOrDefault();
            var usr = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
            var user = _userManager.FindByNameAsync(usr).Result;
            var datetime = DateTime.Now;
            if (model.RefrenceAlias != null && model.Reference == null)
            {
                model.RefrenceType = "M";
                model.Reference = GenReferenceSpecialClient(model.RefrenceAlias);
            }
            else if (model.RefrenceAlias == null && model.Reference == null)
            {
                model.RefrenceAlias = "LS";
                model.Reference = GenAutoReferenceCode();
                model.RefrenceType = "A";
            }
            else
            {
                model.Reference = model.RefrenceAlias + "" + model.Reference; 
            }
            model.CreatedAt = datetime;
            model.CreatedBy = user.Id;
            var clientAdd = _clientRepo.Add(model);
            return Json(clientAdd);
        }

        private string GenReferenceSpecialClient(string alias)
        { 
            try
            {
                var usr = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
                var user =  _userManager.FindByNameAsync(usr).Result;
                var customr = _clientRepo.GetAll().Where(x=> x.RefrenceAlias == alias).LastOrDefault();
                if (customr == null)
                {
                    return alias + ""+ "0000001";
                }
                else
                {

                var client = customr;
                string value = client.Reference;
                string number = Regex.Match(value, "[0-9]+$").Value;
                return value.Substring(0, value.Length - number.Length) + (long.Parse(number) + 1).ToString().PadLeft(number.Length, '0');
                //return value.Substring(0, value.Length - number.Length) + "LS-0000000" + (long.Parse(number) + 1).ToString().PadLeft(number.Length, '0');
                }
                 

            }
            catch (NullReferenceException)
            {
                return "Invalid Reference Number";
            }
        }

        private string GenAutoReferenceCode()
        {
            try
            {
                var usr = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
                var user = _userManager.FindByNameAsync(usr).Result;
                var company = _companyRepo.GetAll().Where(x => x.ReferenceNo != null && x.ReferenceNo != "").LastOrDefault();
                if (company == null)
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

        private string GenReferenceCode(long comPanyId)
        {
            try
            {
                var usr = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
                var user = _userManager.FindByNameAsync(usr).Result;
                var quote = _quoteRepo.GetAll().LastOrDefault();
                if (quote.ClientReference == null)
                {
                    var company = _companyRepo.GetAll().Where(x => x.CompanyId == comPanyId && x.ReferenceNo != null && x.ReferenceNo != "").LastOrDefault();
                    if (company == null)
                    {
                        return "LS0000001";
                    }
                    else
                    { 
                        return company.ReferenceNo;
                    }
                }
                else
                {
                    var quoTe = quote.ClientReference;
                    string value = quoTe.ToString();
                    string number = Regex.Match(value, "[0-9]+$").Value;
                    return value.Substring(0, value.Length - number.Length) + (long.Parse(number) + 1).ToString().PadLeft(number.Length, '0');
                } 

            }
            catch (NullReferenceException)
            {
                return "Invalid Reference Number";
            }
        }

        public JsonResult AddNewClient(Clients model)
        {
            var datetime = DateTime.Now;
            var usr = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
            var users = _userManager.FindByNameAsync(usr).Result;
            if (model.RefrenceAlias != null && model.Reference == null)
            {
                model.RefrenceType = "M";
                model.Reference = GenReferenceSpecialClient(model.RefrenceAlias);
            }
            else if (model.RefrenceAlias == null && model.Reference == null)
            {
                model.RefrenceAlias = "LS";
                model.Reference = GenReferenceCode(model.CompanyId);
                model.RefrenceType = "A";
            }
            else
            {
                model.Reference = model.RefrenceAlias + "" + model.Reference;
            }
            model.RefrenceType = "A";
            model.CreatedBy = users.Id;
            model.CreatedAt = datetime;
             _clientRepo.Add(model);
            return Json(model);
        }

        public IActionResult UpdateClient(Clients model)
        {
            var datetime = DateTime.Now;
            model.EditedAt = datetime;
            var clientUpdate = _clientRepo.Update(model);
            return Json(clientUpdate);
        }



        public IActionResult AddSubClient(SubClients model)
        {
            var datetime = DateTime.Now; 
            model.CreatedAt = datetime;
            var clientAdd = _subclientRepo.Add(model);
            return Json(clientAdd);
        }


        public IActionResult UpdateSubClient(SubClients model)
        {
            var datetime = DateTime.Now;
            model.EditedAt = datetime; 
            var clientAdd = _subclientRepo.Update(model);
            return Json(clientAdd);
        }
        public IActionResult TermsConditions()
        {
            return View();
        }
        
        public IActionResult GetAllTermConditions()
        {
            var tc = _termConditionRepo.GetAll().LastOrDefault();
            return Json(tc);
        }
        public IActionResult GetTermConditions()
        {
            var tc = _termConditionRepo.GetAll().LastOrDefault();
            return Json(tc);
        }
        [HttpPost]
        public IActionResult AddTc(string description)
        {
            TermsConditions model = new TermsConditions();
            model.Remarks = description;
            var tcAdd = _termConditionRepo.Add(model);
            return Json(tcAdd);
        }
        
        public IActionResult AddTermsConditions(TermsConditions model)
        {
            var tcAdd = _termConditionRepo.Add(model);
            return Json(tcAdd);
        }
        
        public IActionResult UpdateTermsConditions(TermsConditions model)
        {
            var tcUpdate = _termConditionRepo.Update(model);
            return Json(tcUpdate);
        }



        private static MailMessage CreateMailMessage(MemoryStream stream, string email)
        {
            // Instantiate a report. 
            // Email export options are already specified at design time.   
             QuotationReport report = new QuotationReport();
            MailMessage mail = new MailMessage();
            var memoryStream = new MemoryStream();
            //report.ExportToPdf(stream); 

            //// Create a new attachment and add the PDF document.
            //stream.Seek(0, System.IO.SeekOrigin.Begin);
            //Attachment attachedDoc = new Attachment(stream, "Quotation.pdf", "application/pdf");
 
            mail.Attachments.Add(new Attachment(@"C:\Quotations\QuotationReport.pdf"));
            //mail.Attachments.Add(attachment);

            report.ExportOptions.Email.RecipientAddress = email;
            report.ExportOptions.Email.RecipientName = "Quotation Report";
            // Create a new message.
            //mail.Attachments.Add(attachedDoc);

            // Specify the sender and retrieve the recipient settings from the report export options.
            mail.From = new MailAddress("donotreply@limitlesssol.org", "Quotation@@");
            mail.To.Add(new MailAddress(report.ExportOptions.Email.RecipientAddress,
                report.ExportOptions.Email.RecipientName));

            // Specify other e-mail options.
            mail.Subject = report.ExportOptions.Email.Subject;
            mail.Body = "This is a test e-mail message sent by an application.";

            return mail;
        }
         
        //public async Task<IActionResult> SubmitReport(string email, long QuotationID)
        public IActionResult SubmitReport(QuotationReportViewModel model)
        {
            if (model.Email == null || model.Email == "")
            {
                return new OkObjectResult(new { Error = "Client data not found" });
            }
            else
            {

                var datetime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));
                var sendDate = _quoteRepo.GetFirst(kc => kc.QuotationId == model.QuotationID);
                sendDate.EmailDate = datetime;
                _quoteRepo.Update(sendDate);

                string path = @"C:\Quotations\QuotationReport.pdf";

                MAPI api = new MAPI(IntPtr.Zero, new string[] { path }, "Quotation Report", "Please, see the attached report for Quotation.", new RecipientCollection() {
                new Recipient() { Address = "SMTP:"+model.Email+"",ContactName = model.ClientName, FieldType = RecipientFieldType.TO },
                //new Recipient() { Address = "SMTP:"+email+"", ContactName = "Demo EMail", FieldType = RecipientFieldType.TO },
                //new Recipient() { Address = "SMTP:realemail@gmail.com", ContactName = "RealContactName", FieldType = RecipientFieldType.CC }
                });

                #region comment Code
                //var quotatioN = _quoteRepo.GetAll().Where(d=> d.QuotationId == QuotationID).LastOrDefault();
                //if (quotatioN.EmailStatus == true)
                //{
                //    return new OkObjectResult(new { Message = "Email already sent" });
                //}
                //string SmtpHost = "in-v3.mailjet.com";
                //int SmtpPort = 587;
                //string SmtpUserName = "ea7b0f66867bfb1d4a5428bd17529f5b";
                //string SmtpUserPassword = "8ff3dc9d679f1b67da8530588e244234";
                //NetworkCredential nameAndPassword = new NetworkCredential(SmtpUserName, SmtpUserPassword);
                //var dx = await SendAsync(email, SmtpHost, SmtpPort, nameAndPassword);
                //if (dx == "OK")
                //{
                //    quotatioN.EmailStatus = true;
                //    _quoteRepo.Update(quotatioN);
                //}
                #endregion
            }
            return new OkObjectResult(new { Success = "Your email has been sent" });
        }

        private static async Task<string> SendAsync(string email,  string SmtpHost, int SmtpPort, NetworkCredential nameAndPassword)
        {
            string result = "OK";
            // Create a new memory stream and export the report in PDF.
            using (MemoryStream stream = new MemoryStream())
            {
                using (MailMessage mail = CreateMailMessage(stream, email))
                {
                    using (SmtpClient smtpClient = new SmtpClient(SmtpHost, SmtpPort))
                    {
                        smtpClient.Credentials = nameAndPassword;
                        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtpClient.EnableSsl = false;
                        try
                        {
                            await smtpClient.SendMailAsync(mail);
                        }
                        catch (Exception ex)
                        {
                            result = ex.Message;
                        }

                    }
                }
            }
            return result;
        }
         
        public long getuserCompany()
        { 
            var usr = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
            var usercmp = _userManager.FindByNameAsync(usr).Result;
            return usercmp.CompanyId;
        }
    }
}
