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
    public class MasterDetailRepository : RepoBase<Master_Detail_Table>, IMasterDetailRepository
    {
        //public List<ItemsViewModel> GetItemsbymain()
        //{
        //    var data = (from r in Db.Master_Tables
        //                select new ItemsViewModel
        //                {
        //                    MasterId = r.MasterId,
        //                    MainItemId = r.MainItemId,
        //                    CompanyId = r.CompanyId,
        //                    Name = r.Item
        //                }).ToList();

        //    return data;
        //}
        public List<ItemsViewModel> GetItemsbymain()
        {
            var data = (from r in Db.Master_Tables
                        from md in Db.MainItems
                        where r.MainItemId == md.MainItemId
                        select new ItemsViewModel
                        { 
                            MasterId = r.MasterId,
                            MainItemId = r.MainItemId,
                            CompanyId = r.CompanyId,
                            Name = md.Name
                        }).ToList();

            return data;
        }
    }
}
