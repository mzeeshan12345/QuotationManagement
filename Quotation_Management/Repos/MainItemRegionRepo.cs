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
    public class MainItemRegionRepo : RepoBase<MainItemRegion>, IMainItemRegionsRepo
    {
        public List<ItemsViewModel> GetCurrentRegions(long id)
        {
            var user = (from itm in Db.ItemRegions
                        from mit in Db.MainItemRegions 
                        where itm.RegionId == Convert.ToInt64(mit.RegionId) && mit.MainItemId == id
                        select new ItemsViewModel
                        { 
                            Name = itm.Name,
                            RegionId = Convert.ToInt64(mit.RegionId),
                            MainItemId = mit.MainItemId 
                        }).ToList();
            return user;
        }

        //public List<ItemsViewModel> GetItemsByRegion()
        //{
        //    var iTems = (from itm in Db.MainItems
        //                join mir in Db.MainItemRegions on itm.MainItemId equals mir.MainItemId 
        //                join ur in Db.UsersRegions on mir.RegionId equals ur.RegionId  
        //                select new ItemsViewModel
        //                {
        //                    Name = itm.Name,
        //                    RegionId = Convert.ToInt64(mir.RegionId),
        //                    MainItemId = itm.MainItemId
        //                }).ToList();
        //    return iTems;
        //}
    }
}
