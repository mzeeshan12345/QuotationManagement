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
    public class MasterRepository : RepoBase<Master_Table>, IMasterRepository
    {
        public List<ItemsViewModel> GetSubItemsByRegion()
        {
            var iTems = (from itm in Db.Master_Tables
                         join mir in Db.SubItemRegions on itm.MasterId equals mir.MasterId
                         join ur in Db.UsersRegions on mir.RegionId equals ur.RegionId
                         select new ItemsViewModel
                         {
                             Item = itm.Item,
                             RegionId = Convert.ToInt64(mir.RegionId),
                             MainItemId = itm.MainItemId,
                             CompanyId = itm.CompanyId,
                             Price = itm.Price,
                             MasterId = itm.MasterId, 
                         }).Distinct().ToList();
            return iTems;
        }
    }
}
