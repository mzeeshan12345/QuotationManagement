using Microsoft.AspNetCore.Identity;
using Quotation_Management.Models;
using Quotation_Management.Repos.Base;
using Quotation_Management.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quotation_Management.Repos.Interfaces
{
    public interface IAppPageRepository : IRepo<AppPage>
    {
        List<ApplicationRole> GetRolesList();
        List<ApplicationRole> GetRoles(long compID);
        List<RoleViewModel> GetRolessss();
        List<AppPagePermissionViewModel> GetPagesAll();
        List<AppPagePermissionViewModel> GetPages();
        List<AppPagePermissionViewModel> GetAppPages(string Role);
         List<AppPagePermissionViewModel> GetPermissionByRole(List<AppUserRoleViewModel> Role);
        List<AppPagePermissionViewModel> GetMethodPermissionByRole(List<AppUserRoleViewModel> Role);
        List<AppPagePermissionViewModel> GetFlowPermissionByRole(List<AppUserRoleViewModel> role);
        List<AppUserRoleViewModel> GetRolesByUser(string id);

        List<AppPagePermissionViewModel> GetManagerPermission();
        List<string> GetUserRoles(string UserId);
    }
}
