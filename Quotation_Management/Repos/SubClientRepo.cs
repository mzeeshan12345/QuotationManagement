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
    public class SubClientRepo : RepoBase<SubClients>, ISubClientRepo
    {
        public List<AssignClientViewModel> GetClientAssigns(long clientId)
        {

            var pages = (from sc in Db.SubClients        
                         where sc.ClientId == clientId 
                         select new AssignClientViewModel
                         {
                             SubClientName = sc.ClientName,
                             Email = sc.Email, 
                             Assign = sc.Assign,
                             ClientId = sc.ClientId,
                             CompanyId = sc.CompanyId,
                             ContactPerson = sc.ContactPerson,
                             Landline = sc.Landline,
                             Phone = sc.Phone,
                             Reference = sc.Reference,
                             RefrenceType = sc.RefrenceType,
                             Whatsapp = sc.Whatsapp,  
                             SubClientId = sc.SubClientId,
                         }).Distinct().ToList();

            return pages;
        }



        public List<AssignClientViewModel> GetClientAssigned(string id)
        {

            var pages = (from sc in Db.SubClients
                         from usr in Db.Users
                         where usr.Id == id
                         select new AssignClientViewModel
                         {
                             UserId = usr.Id,
                             SubClientName = sc.ClientName,
                             Email = sc.Email,
                             Name = usr.Name,
                             Assign = sc.Assign,
                             ClientId = sc.ClientId,
                             CompanyId = sc.CompanyId,
                             ContactPerson = sc.ContactPerson,
                             Landline = sc.Landline,
                             Phone = sc.Phone,
                             Reference = sc.Reference,
                             RefrenceType = sc.RefrenceType,
                             Whatsapp = sc.Whatsapp,
                             SubClientId = sc.SubClientId,
                         }).Distinct().ToList();

            return pages;
        }

    }
}
