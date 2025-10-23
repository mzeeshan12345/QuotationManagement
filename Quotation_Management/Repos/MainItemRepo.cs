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
    public class MainItemRepo : RepoBase<MainItem>, IMainItemRepo
    {

        public List<MainItem> GetAllItems()
        { 
            var user = (from itm in Db.MainItems
                        from mit in Db.MainItemRegions
                        from mir in Db.ItemRegions
                        where itm.MainItemId == mit.MainItemId && mir.RegionId.ToString() == mit.RegionId
                         select new MainItem
                        { 
                            CompanyId = itm.CompanyId,
                            MainItemId = itm.MainItemId,
                            Name = itm.Name,
                            
                        }).ToList();
            return user;
        }

        public List<ItemsViewModel> GetItemsByRegion()
        {
            var iTems = (from itm in Db.MainItems
                         join mir in Db.MainItemRegions on itm.MainItemId equals mir.MainItemId
                         join ur in Db.UsersRegions on mir.RegionId equals ur.RegionId
                         select new ItemsViewModel
                         {
                             Name = itm.Name,
                             RegionId = Convert.ToInt64(mir.RegionId),
                             MainItemId = mir.MainItemId,
                             CompanyId = itm.CompanyId
                         }).Distinct().ToList();
            return iTems;
        }
    }
}
