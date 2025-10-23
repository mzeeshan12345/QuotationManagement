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
    public interface IAdminRepo : IRepo<ApplicationUser>
    {
        ApplicationUserModel GetUserById(string UserId);
        List<QuotationStaticViewModel> GetQuotationStatics(long companyId); 
        List<QuotationStaticViewModel> GetQuotationStaticsSales(string id);

        List<QuotationStaticViewModel> GetDailyStatics(long companyId);
        List<QuotationStaticViewModel> GetWeeklyStatics(long companyId);
        List<QuotationStaticViewModel> GetMonthlyStatics(long companyId);
        List<QuotationStaticViewModel> GetFilterStaticsByDate(string fromDate, string ToDate, long companyID);
        List<QuotationStaticViewModel> GetFilterStatics(string fromDate, string ToDate, string uId, long companyID,string status);
    }
}
