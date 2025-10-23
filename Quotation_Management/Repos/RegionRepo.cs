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
    public class RegionRepo : RepoBase<ItemRegions>, IRegionRepo
    {
        public List<UserRegionViewModel> GetAllRegionsList(long cmpId)
        {
            var data = (from r in Db.ItemRegions
                        where r.CompanyId == cmpId
                        select new UserRegionViewModel
                        {
                            RegionId = r.RegionId.ToString(),
                            Name = r.Name
                        }).ToList();

            return data;
        }
        
        
        public List<UserRegionViewModel> GetAllRegions()
        {
            var data = (from r in Db.ItemRegions
                        select new UserRegionViewModel
                        {
                            RegionId = r.RegionId.ToString(),
                            Name = r.Name
                        }).ToList();

            return data;
        }
    }
}
