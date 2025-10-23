using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Quotation_Management.Models;
using Quotation_Management.Repos.Interfaces;
using Quotation_Management.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quotation_Management.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
         private readonly RoleManager<ApplicationRole> _roleManager;
        private IAppPageRepository _appPageRepo;
        private IUserRegionRepo _userRegionRepo;
        private IFlowStatusRepo _assignFlowRepo;
        private IAppPagePermissionRepository _appPermissionRepo;
        private IAdminRepo _adminRepo;

        public UserController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
         IAppPageRepository appPageRepo,
           IAppPagePermissionRepository appPermissionRepo,
           IUserRegionRepo userRegionRepo,
           IFlowStatusRepo assignFlowRepo,
            IAdminRepo adminRepo,
         RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
              _roleManager = roleManager;
            _userRegionRepo = userRegionRepo;
            _appPageRepo = appPageRepo;
            _assignFlowRepo = assignFlowRepo;
            _appPermissionRepo = appPermissionRepo;
            _adminRepo = adminRepo;

        }
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult EditUser()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetUsersList()
        {
            var users = _userManager.Users;
            return Json(users);
        }
        
        [HttpGet]
        public JsonResult GetUsers(long companyId)
        {
            var users = _userManager.Users.Where(uc=> uc.CompanyId == companyId);
            return Json(users);
        }
        
        
        [HttpGet]
        public JsonResult GetUserByID(string UserId)
        {
            var user = _adminRepo.GetUserById(UserId);
            return Json(user);
        }
   
        public async Task<Object> UpdateRegisteration(ApplicationUserModel model, List<AssignFlowStatus> FlowStatus)
        {
            var identityUser = await _userManager.FindByIdAsync(model.Id);

            if (identityUser != null)
            { 
                identityUser.UserName = model.UserName;
                identityUser.Email = model.Email;
                identityUser.PhoneNumber = model.PhoneNumber;
                identityUser.DateOfBirth = Convert.ToDateTime(model.DateOfBirth);
                identityUser.FirstName = model.FirstName;
                identityUser.LastName = model.LastName;
                identityUser.DateOfBirth = model.DateOfBirth;
                 identityUser.Designation = model.Designation;
                identityUser.Gender = model.Gender;
                identityUser.CompanyId = model.CompanyId;
                identityUser.Name = model.FirstName + " " + model.LastName;
                
            }

            var result = await _userManager.UpdateAsync(identityUser);

            if (result.Succeeded)
            { 
                if (model.RegionId.Count > 0)
                {
                    var currentRegions = _userRegionRepo.GetAll().Where(dc => dc.UserId.ToString() == identityUser.Id);
                    _userRegionRepo.DeleteRange(currentRegions);

                    List<UsersRegion> regionDetails = new List<UsersRegion>();
                    foreach (var region in model.RegionId)
                    {
                        if (region != null )
                        {
                            var ur = new UsersRegion
                            {
                                RegionId = region,
                                UserId = identityUser.Id
                            };
                            regionDetails.Add(ur);
                        }
                        
                    }
                   _userRegionRepo.AddRange(regionDetails);
                }
                if (model.Role.Count > 0)
                {
                var currentRoles = _appPageRepo.GetUserRoles(identityUser.Id);

                var rem = await _userManager.RemoveFromRolesAsync(identityUser, currentRoles);
                 
                    foreach (var role in model.Role)
                    {
                        if (role != null)
                        {
                        var newUserRole = await _userManager.AddToRoleAsync(identityUser, role); 
                        }
                    } 
                } 
                else
                {
                    return new OkObjectResult(new { Error = "Role assigning error" });
                }

                if (FlowStatus.Count() > 0)
                {
                    var currentflow = _assignFlowRepo.GetAll().Where(dcs => dcs.UserId == identityUser.Id);
                    if (currentflow.Count() > 0)
                    {
                        _assignFlowRepo.DeleteRange(currentflow);
                    }

                    var details = new List<AssignFlowStatus>();
                    foreach (var item in FlowStatus)
                    {
                        var master = new AssignFlowStatus
                        {
                            AdminApproval = item.AdminApproval,
                            FlowStatus = item.FlowStatus,
                            ManagerApproval = item.ManagerApproval,
                            UserId = identityUser.Id
                        };
                        details.Add(master);
                    }
                    _assignFlowRepo.AddRange(details);
                }
                else
                {
                    return new OkObjectResult(new { Error = "Flow Assigning Error" });
                }
            }
            return new OkObjectResult(new { result = result.Succeeded, roles = model.Role.Count, regions = model.RegionId.Count });
        }

         
        [HttpPost]
        public async Task<IActionResult> UpdatePassword(ApplicationUserModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user != null)
            {
                var newPassword = _userManager.PasswordHasher.HashPassword(user, model.Password);

                user.PasswordHash = newPassword;

                var res = await _userManager.UpdateAsync(user);

                if (res.Succeeded)
                {
                    return Ok(res);
                }
            }

            return Ok(user);

        }

        public async Task<IActionResult> LogoutUser()
        {
            await _signInManager.SignOutAsync();
            return new JsonResult(new { error = false, message = "User logged out." });
        }
    }
}
