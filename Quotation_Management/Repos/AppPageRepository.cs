using Microsoft.AspNetCore.Identity;
using Quotation_Management.Models;
using Quotation_Management.Repos.Base;
using Quotation_Management.Repos.Interfaces;
 using Quotation_Management.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quotation_Management.Repos
{
    public class AppPageRepository : RepoBase<AppPage>, IAppPageRepository
    {

        public List<ApplicationRole> GetRolesList()
        {
            var data = (from r in Db.Roles 
                        select r).ToList(); 

            return data;  
        }
        
        public List<ApplicationRole> GetRoles(long compID)
        {
            var data = (from r in Db.Roles
                        where r.CompanyId == compID
                        select r).ToList(); 

            return data;  
        }
        
        public List<RoleViewModel> GetRolessss()
        {
            var data = (from r in Db.Roles
                        select new RoleViewModel {  
                            Id = r.Id,
                            Name = r.Name
                        }).ToList(); 

            return data;  
        }


        public List<AppPagePermissionViewModel> GetPagesAll()
        {

            var pagesAll = (from page in Db.AppPages
                         from perm in Db.AppPagePermissions
                         where (perm.AppPageId == page.AppPageId) && (perm.RoleId != null || perm.RoleId != "")
                         select new AppPagePermissionViewModel
                         {
                             AppPageId = page.AppPageId,
                             Create = perm.Create,
                             Read = perm.Read,
                             AppPagePermissionId = perm.AppPagePermissionId,
                             Delete = perm.Delete,
                             PageUrl = page.PageUrl,
                             Update = perm.Update,
                             Icon = page.Icon,
                             RoleId = perm.RoleId,
                             PageName = page.PageName,
                             PageType = page.PageType
                         }).Distinct().ToList();

            return pagesAll;
        }

        public List<AppPagePermissionViewModel> GetPages()
        {

            var pages = (from page in Db.AppPages
                         from perm in Db.AppPagePermissions
                         where (perm.RoleId == null || perm.RoleId == "") && (perm.AppPageId == page.AppPageId)

                         select new AppPagePermissionViewModel
                         {
                             AppPageId = perm.AppPageId,
                             Create = perm.Create,
                             Read = perm.Read,
                             AppPagePermissionId = perm.AppPagePermissionId,
                             Delete = perm.Delete,
                             PageUrl = page.PageUrl,
                             Update = perm.Update,
                             Icon = page.Icon,
                             RoleId = perm.RoleId,
                             PageName = page.PageName,
                             PageType = page.PageType
                         }).ToList();

            return pages;
        }

        public List<AppPagePermissionViewModel> GetAppPages(string Role)
        {

            var pages = (from page in Db.AppPages
                         from perm in Db.AppPagePermissions
                         where (perm.AppPageId == page.AppPageId) && (perm.RoleId == Role)
                         select new AppPagePermissionViewModel
                         {
                             AppPageId = page.AppPageId,
                             Create = perm.Create,
                             Read = perm.Read,
                             AppPagePermissionId = perm.AppPagePermissionId,
                             Delete = perm.Delete,
                             PageUrl = page.PageUrl,
                             Update = perm.Update,
                             Icon = page.Icon,
                             RoleId = perm.RoleId,
                             PageName = page.PageName,
                             PageType = page.PageType
                         }).Distinct().ToList();

            return pages;
        }


        public List<AppUserRoleViewModel> GetRolesByUser(string id)
        { 
            var roles = (from role in Db.Roles
                         join userrole in Db.UserRoles on role.Id equals userrole.RoleId
                         join roleName in Db.Roles on userrole.RoleId equals roleName.Id
                         where
                         userrole.UserId == id
                         select new AppUserRoleViewModel
                         {
                             RoleId = role.Id,
                             RoleName = roleName.Name
                         }).ToList(); 
            return roles;
        }

        public List<AppPagePermissionViewModel> GetPermissionByRole(List<AppUserRoleViewModel> Role)
        {
            var mydataMain = new List<AppPagePermissionViewModel>();

            foreach (var item in Role)
            { 
                var mydata = (from perm in Db.AppPagePermissions
                              join page in Db.AppPages on perm.AppPageId equals page.AppPageId
                              where perm.RoleId == item.RoleId && perm.Read == true
                              select new AppPagePermissionViewModel
                              {
                                  Create = perm.Create,
                                  Read = perm.Read,
                                  Update = perm.Update,
                                  Delete = perm.Delete,
                                  PageName = page.PageName,
                                  Icon = page.Icon,
                                  PageType = page.PageType,
                                  PageUrl = page.PageUrl,
                                  AppPageId = page.AppPageId,
                                  AppPagePermissionId = perm.AppPagePermissionId,
                                  RoleId = perm.RoleId
                              }).Distinct().ToList();

                mydataMain.AddRange(mydata);
            }
            return mydataMain;
        }

         
        public List<AppPagePermissionViewModel> GetMethodPermissionByRole(List<AppUserRoleViewModel> Role)
        {
            var mydataMain = new List<AppPagePermissionViewModel>();

            foreach (var itm in Role)
            {
                var mydata = (from perm in Db.AppPagePermissions
                              join page in Db.AppPages on perm.AppPageId equals page.AppPageId
                              where page.PageType == "Method" && perm.RoleId == itm.RoleId
                              //where page.PageType == "Method" && perm.RoleId == itm.RoleId && perm.Read == true
                              select new AppPagePermissionViewModel
                              {
                                  Create = perm.Create,
                                  Read = perm.Read,
                                  Update = perm.Update,
                                  Delete = perm.Delete,
                                  PageName = page.PageName,
                                  Icon = page.Icon,
                                  PageType = page.PageType,
                                  PageUrl = page.PageUrl,
                                  AppPageId = page.AppPageId,
                                  AppPagePermissionId = perm.AppPagePermissionId,
                                  RoleId = perm.RoleId
                              }).Distinct().ToList();

                mydataMain.AddRange(mydata);
            }
            return mydataMain;
        }


        public List<AppPagePermissionViewModel> GetFlowPermissionByRole(List<AppUserRoleViewModel> role)
        {
            var mydata = new List<AppPagePermissionViewModel>();

            foreach (var item in role)
            {
                var data = (from perm in Db.AppPagePermissions
                              join page in Db.AppPages on perm.AppPageId equals page.AppPageId
                              join rle in Db.Roles on perm.RoleId equals rle.Id
                              where page.PageType == "Flow" && perm.RoleId == item.RoleId && perm.Read == true
                              select new AppPagePermissionViewModel
                              {
                                  Create = perm.Create,
                                  Read = perm.Read,
                                  Update = perm.Update,
                                  Delete = perm.Delete,
                                  PageName = page.PageName,
                                  Icon = page.Icon,
                                  PageType = page.PageType,
                                  PageUrl = page.PageUrl,
                                  AppPageId = page.AppPageId,
                                  AppPagePermissionId = perm.AppPagePermissionId,
                                  RoleId = perm.RoleId,
                                  RoleName = rle.Name
                              }).Distinct().ToList();

                mydata.AddRange(data);
            }
            return mydata;
        }

        public List<string> GetUserRoles(string UserId)
        {
            var roles = (from role in Db.Roles
                         join userrole in Db.UserRoles on role.Id equals userrole.RoleId
                         join user in Db.Users on userrole.UserId equals user.Id
                         where user.Id == UserId
                         select role.Name).ToList();

            return roles;
        }

        public List<AppPagePermissionViewModel> GetManagerPermission()
        { 
                var data = (from perm in Db.AppPagePermissions
                            join page in Db.AppPages on perm.AppPageId equals page.AppPageId
                            join rle in Db.Roles on perm.RoleId equals rle.Id
                            where page.PageType == "Flow" && perm.Read == true
                            select new AppPagePermissionViewModel
                            {
                                Create = perm.Create,
                                Read = perm.Read,
                                Update = perm.Update,
                                Delete = perm.Delete,
                                PageName = page.PageName,
                                Icon = page.Icon,
                                PageType = page.PageType,
                                PageUrl = page.PageUrl,
                                AppPageId = page.AppPageId,
                                AppPagePermissionId = perm.AppPagePermissionId,
                                RoleId = perm.RoleId,
                                RoleName = rle.Name
                            }).Distinct().ToList(); 
            return data;
        }

    }
}
