using Microsoft.AspNetCore.Identity;
using Quotation_Management.Models;
using Quotation_Management.Repos.Base;
using Quotation_Management.Repos.Interfaces;
using Quotation_Management.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Quotation_Management.Repos
{
    public class AdminRepo : RepoBase<ApplicationUser>, IAdminRepo
    {
        string from = DateTime.Now.ToString("yyyy/MM/dd") + " 00:00:00";
        string To = DateTime.Now.ToString("yyyy/MM/dd") + " 23:59:59";
        
        
        string strt = DateTime.Now.ToString("yyyy/MM/dd") + " 00:00:00";
        string endDate = DateTime.Now.ToString("yyyy/MM/dd") + " 23:59:59";

        DateTime dt = DateTime.Now;
        public ApplicationUserModel GetUserById(string UserId)
        {
            var user = (from usr in Db.Users
                        where usr.Id == UserId
                        select new ApplicationUserModel
                        {
                            Id = usr.Id,
                            FirstName = usr.FirstName,
                            LastName = usr.LastName,
                            PhoneNumber = usr.PhoneNumber,
                            UserName = usr.UserName,
                            DateOfBirth = usr.DateOfBirth,
                            Email = usr.Email,
                            Designation = usr.Designation,
                             CompanyId = usr.CompanyId,
                            Gender = usr.Gender 
                        }).FirstOrDefault();


            var roles = (from role in Db.Roles
                         join userrole in Db.UserRoles on role.Id equals userrole.RoleId
                         where userrole.UserId == user.Id
                         select role.Name).ToList();
            
            var regions = (from region in Db.ItemRegions
                         join uRegion in Db.UsersRegions on region.RegionId.ToString() equals uRegion.RegionId
                         where uRegion.UserId == user.Id
                          select region.RegionId.ToString()).ToList();
                        //select new UserRegionViewModel
                        //{
                        //    RegionId = region.RegionId.ToString(),
                        //    UserId = uRegion.UserId,
                        //    Name = region.Name

                        //}).ToList();

            user.Role = roles;
            user.RegionId = regions;
            return user;
        }

 
        public List<QuotationStaticViewModel> GetQuotationStatics(long companyId)
        {
            List<QuotationStaticViewModel> das = new List<QuotationStaticViewModel>();


            //Approved
            var approvedData = (from s in Db.Quotations
                                select new QuotationStaticViewModel
                                {
                                    Status = "Approved",
                                    Count = (from x in Db.Quotations where x.Status == "Approved" && x.CompanyId == companyId select x).Count(),

                                }).Distinct().ToList();
            das.AddRange(approvedData);

            //Pending
            var bbData = (from s in Db.Quotations
                          select new QuotationStaticViewModel
                          {
                              Status = "Pending",
                              Count = (from x in Db.Quotations where x.Status == "Pending" && x.CompanyId == companyId select x).Count(),

                          }).Distinct().ToList();
            das.AddRange(bbData);
             
            //Decline
            var activeData = (from s in Db.Quotations
                           select new QuotationStaticViewModel
                           {
                               Status = "Cancelled",
                                Count = (from x in Db.Quotations where x.Status == "Cancelled" && x.CompanyId == companyId select x).Count(),
 
                           }).Distinct().ToList();
            das.AddRange(activeData);
             

            return das;
        }
        
        public List<QuotationStaticViewModel> GetQuotationStaticsSales(string id)
        {
            List<QuotationStaticViewModel> das = new List<QuotationStaticViewModel>();


            //Approved
            var approvedData = (from s in Db.Quotations
                                select new QuotationStaticViewModel
                                {
                                    Status = "Approved",
                                    Count = (from x in Db.Quotations where x.Status == "Approved" && x.UserId == id select x).Count(),

                                }).Distinct().ToList();
            das.AddRange(approvedData);

            //Pending
            var bbData = (from s in Db.Quotations
                          select new QuotationStaticViewModel
                          {
                              Status = "Pending",
                              Count = (from x in Db.Quotations where x.Status == "Pending" && x.UserId == id select x).Count(),

                          }).Distinct().ToList();
            das.AddRange(bbData); 

            //Decline
            var activeData = (from s in Db.Quotations
                           select new QuotationStaticViewModel
                           {
                               Status = "Cancelled",
                                Count = (from x in Db.Quotations where x.Status == "Cancelled" && x.UserId == id select x).Count(),
 
                           }).Distinct().ToList();
            das.AddRange(activeData);

            //Draft
            var draftData = (from s in Db.Quotations
                             select new QuotationStaticViewModel
                             {
                                 Status = "Draft",
                                 Count = (from x in Db.Quotations where x.Status == "Draft" select x).Count(),

                             }).Distinct().ToList();
            das.AddRange(draftData);
            return das;
        }


        public List<QuotationStaticViewModel> GetDailyStatics(long companyId)
        {
            List<QuotationStaticViewModel> allCases = new List<QuotationStaticViewModel>();

            var Proposal = (from s in Db.Quotations
                            select new QuotationStaticViewModel
                            {
                                Status = "Proposal",
                                Count = (from x in Db.Quotations where x.QuotationStatus == "Proposal" && x.CreatedAt >= Convert.ToDateTime(strt) && x.CreatedAt <= Convert.ToDateTime(endDate) && x.CompanyId == companyId select x).Count()
                            }).Distinct().ToList();
            allCases.AddRange(Proposal);

            var Quotation = (from s in Db.Quotations
                            select new QuotationStaticViewModel
                            {
                                Status = "Quotation",
                                Count = (from x in Db.Quotations where x.QuotationStatus == "Quotation" && x.CompanyId == companyId && x.CreatedAt >= Convert.ToDateTime(strt) && x.CreatedAt <= Convert.ToDateTime(endDate) select x).Count()
                            }).Distinct().ToList();
            allCases.AddRange(Quotation);
            
            var WaitingPO = (from s in Db.Quotations
                            select new QuotationStaticViewModel
                            {
                                Status = "Waiting PO",
                                Count = (from x in Db.Quotations where x.QuotationStatus == "Waiting PO" && x.CompanyId == companyId && x.CreatedAt >= Convert.ToDateTime(strt) && x.CreatedAt <= Convert.ToDateTime(endDate) select x).Count()
                            }).Distinct().ToList();
            allCases.AddRange(WaitingPO);


            var POReceived = (from s in Db.Quotations
                          select new QuotationStaticViewModel
                          {
                              Status = "PO Received",
                              Count = (from x in Db.Quotations where x.Status == "PO Received" && x.CompanyId == companyId && x.CreatedAt >= Convert.ToDateTime(strt) && x.CreatedAt <= Convert.ToDateTime(endDate) select x).Count()
                          }).Distinct().ToList();
            allCases.AddRange(POReceived);
            
            var Invoiced = (from s in Db.Quotations
                          select new QuotationStaticViewModel
                          {
                              Status = "Invoiced",
                              Count = (from x in Db.Quotations where x.Status == "Invoiced" && x.CompanyId == companyId && x.CreatedAt >= Convert.ToDateTime(strt) && x.CreatedAt <= Convert.ToDateTime(endDate) select x).Count()
                          }).Distinct().ToList();
            allCases.AddRange(Invoiced);


            var Cancelled = (from s in Db.Quotations
                           select new QuotationStaticViewModel
                           {
                               Status = "Cancelled",
                               Count = (from x in Db.Quotations where x.QuotationStatus == "Cancelled" && x.CompanyId == companyId && x.CreatedAt >= Convert.ToDateTime(strt) && x.CreatedAt <= Convert.ToDateTime(endDate) select x).Count()
                           }).Distinct().ToList();
            allCases.AddRange(Cancelled); 

            return allCases;
        }
        public List<QuotationStaticViewModel> GetWeeklyStatics(long companyId)
        {
            var todate = DateTime.Now;
            var day = (int)CultureInfo.CurrentCulture.Calendar.GetDayOfWeek(todate);
            var fromdate = DateTime.Now.AddDays(-day);
            int month = dt.Month;

            List<QuotationStaticViewModel> allCases = new List<QuotationStaticViewModel>();

            var Proposal = (from s in Db.Quotations
                            select new QuotationStaticViewModel
                            {
                                Status = "Proposal",
                                Count = (from x in Db.Quotations where x.QuotationStatus == "Proposal" && x.CompanyId == companyId && x.CreatedAt >= Convert.ToDateTime(fromdate.ToString("yyyy/MM/dd")) && x.CreatedAt <= Convert.ToDateTime(todate.ToString("yyyy/MM/dd")) select x).Count()
                            }).Distinct().ToList();
            allCases.AddRange(Proposal);


            var Quotation = (from s in Db.Quotations
                            select new QuotationStaticViewModel
                            {
                                Status = "Quotation",
                                Count = (from x in Db.Quotations where x.QuotationStatus == "Quotation" && x.CompanyId == companyId && x.CreatedAt >= Convert.ToDateTime(fromdate.ToString("yyyy/MM/dd")) && x.CreatedAt <= Convert.ToDateTime(todate.ToString("yyyy/MM/dd")) select x).Count()
                            }).Distinct().ToList();
            allCases.AddRange(Quotation);
            
            var WaitingPO = (from s in Db.Quotations
                            select new QuotationStaticViewModel
                            {
                                Status = "Waiting PO",
                                Count = (from x in Db.Quotations where x.QuotationStatus == "Waiting PO" && x.CompanyId == companyId && x.CreatedAt >= Convert.ToDateTime(fromdate.ToString("yyyy/MM/dd")) && x.CreatedAt <= Convert.ToDateTime(todate.ToString("yyyy/MM/dd")) select x).Count()
                            }).Distinct().ToList();
            allCases.AddRange(WaitingPO);

            var POReceived = (from s in Db.Quotations
                           select new QuotationStaticViewModel
                           {
                               Status = "PO Received",
                               Count = (from x in Db.Quotations where x.QuotationStatus == "PO Received" && x.CompanyId == companyId && x.CreatedAt >= Convert.ToDateTime(fromdate.ToString("yyyy/MM/dd")) && x.CreatedAt <= Convert.ToDateTime(todate.ToString("yyyy/MM/dd")) select x).Count()
                           }).Distinct().ToList();
            allCases.AddRange(POReceived);
            
            var Invoiced = (from s in Db.Quotations
                           select new QuotationStaticViewModel
                           {
                               Status = "Invoiced",
                               Count = (from x in Db.Quotations where x.QuotationStatus == "Invoiced" && x.CompanyId == companyId && x.CreatedAt >= Convert.ToDateTime(fromdate.ToString("yyyy/MM/dd")) && x.CreatedAt <= Convert.ToDateTime(todate.ToString("yyyy/MM/dd")) select x).Count()
                           }).Distinct().ToList();
            allCases.AddRange(Invoiced);

            var Cancelled = (from s in Db.Quotations
                          select new QuotationStaticViewModel
                          {
                              Status = "Cancelled",
                              Count = (from x in Db.Quotations where x.QuotationStatus == "Cancelled" && x.CompanyId == companyId && x.CreatedAt >= Convert.ToDateTime(fromdate.ToString("yyyy/MM/dd")) && x.CreatedAt <= Convert.ToDateTime(todate.ToString("yyyy/MM/dd")) select x).Count()
                          }).Distinct().ToList();
            allCases.AddRange(Cancelled);

            return allCases;
        }
         
        public List<QuotationStaticViewModel> GetMonthlyStatics(long companyId)
        {
            List<QuotationStaticViewModel> allCases = new List<QuotationStaticViewModel>();
            int month = dt.Month; 
             
            var Proposal = (from s in Db.Quotations
                          select new QuotationStaticViewModel
                          {
                              Status = "Proposal",
                              Count = (from x in Db.Quotations where x.QuotationStatus == "Proposal" && x.CompanyId == companyId && x.CreatedAt.Value.Month >= month && x.CreatedAt.Value.Month <= month select x).Count()
                          }).Distinct().ToList();
            allCases.AddRange(Proposal);
            
            var Quotation = (from s in Db.Quotations
                          select new QuotationStaticViewModel
                          {
                              Status = "Quotation",
                              Count = (from x in Db.Quotations where x.QuotationStatus == "Quotation" && x.CompanyId == companyId && x.CreatedAt.Value.Month >= month && x.CreatedAt.Value.Month <= month select x).Count()
                          }).Distinct().ToList();
            allCases.AddRange(Quotation);
            
            var WaitingPO = (from s in Db.Quotations
                          select new QuotationStaticViewModel
                          {
                              Status = "Waiting PO",
                              Count = (from x in Db.Quotations where x.QuotationStatus == "Waiting PO" && x.CompanyId == companyId && x.CreatedAt.Value.Month >= month && x.CreatedAt.Value.Month <= month select x).Count()
                          }).Distinct().ToList();
            allCases.AddRange(WaitingPO);


            var POReceived = (from s in Db.Quotations
                           select new QuotationStaticViewModel
                           {
                               Status = "PO Received",
                               Count = (from x in Db.Quotations where x.QuotationStatus == "PO Received" && x.CompanyId == companyId && x.CreatedAt.Value.Month >= month && x.CreatedAt.Value.Month <= month select x).Count()
                           }).Distinct().ToList();
            allCases.AddRange(POReceived);


            var Invoiced = (from s in Db.Quotations
                         select new QuotationStaticViewModel
                         {
                             Status = "Invoiced",
                             Count = (from x in Db.Quotations where x.QuotationStatus == "Invoiced" && x.CompanyId == companyId && x.CreatedAt.Value.Month >= month && x.CreatedAt.Value.Month <= month select x).Count()
                         }).Distinct().ToList();
            allCases.AddRange(Invoiced);
            
            var Cancelled = (from s in Db.Quotations
                         select new QuotationStaticViewModel
                         {
                             Status = "Cancelled",
                             Count = (from x in Db.Quotations where x.QuotationStatus == "Cancelled" && x.CompanyId == companyId && x.CreatedAt.Value.Month >= month && x.CreatedAt.Value.Month <= month select x).Count()
                         }).Distinct().ToList();
            allCases.AddRange(Cancelled);

            return allCases;
        }


        public List<QuotationStaticViewModel> GetFilterStatics(string fromDate, string ToDate, string uId, long companyID, string status)
        { 
            List<QuotationStaticViewModel> allCases = new List<QuotationStaticViewModel>();
            if(uId == "")
            {
                if (status == "")
                {
                    var Proposal = (from s in Db.Quotations
                                    select new QuotationStaticViewModel
                                    {
                                        Status = "Proposal",
                                        Count = (from x in Db.Quotations where x.QuotationStatus == "Proposal" && x.CompanyId == companyID && (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate)) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status) select x).Count(),
                                        Total = Db.Quotations.Where(x => x.QuotationStatus == "Proposal" && x.CompanyId == companyID && (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate)) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status)).Sum(s => Convert.ToInt16(s.Total)),
                                        Quotations = Db.Quotations.Where(x => x.QuotationStatus == "Proposal" && x.CompanyId == companyID && (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate)) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status)).ToList(),
                                    }).Distinct().ToList();
                    allCases.AddRange(Proposal);

                    var Quotation = (from s in Db.Quotations
                                     select new QuotationStaticViewModel
                                     {
                                         Status = "Quotation",
                                         Count = (from x in Db.Quotations where x.QuotationStatus == "Quotation" && x.CompanyId == companyID && (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate)) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status) select x).Count(),
                                         Total = Db.Quotations.Where(x => x.QuotationStatus == "Quotation" && x.CompanyId == companyID && (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate)) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status)).Sum(s => Convert.ToInt16(s.Total)),
                                         Quotations = Db.Quotations.Where(x => x.QuotationStatus == "Quotation" && x.CompanyId == companyID && (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate)) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status)).ToList(),
                                     }).Distinct().ToList();
                    allCases.AddRange(Quotation);


                    var WaitingPO = (from s in Db.Quotations
                                     select new QuotationStaticViewModel
                                     {
                                         Status = "Waiting PO",
                                         Count = (from x in Db.Quotations where x.QuotationStatus == "Waiting PO" && x.CompanyId == companyID && (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate)) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status) select x).Count(),
                                         Total = Db.Quotations.Where(x => x.QuotationStatus == "Waiting PO" && x.CompanyId == companyID && (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate)) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status)).Sum(s => Convert.ToInt16(s.Total)),
                                         Quotations = Db.Quotations.Where(x => x.QuotationStatus == "Waiting PO" && x.CompanyId == companyID && (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate)) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status)).ToList(),
                                     }).Distinct().ToList();
                    allCases.AddRange(WaitingPO);

                    var POReceived = (from s in Db.Quotations
                                      select new QuotationStaticViewModel
                                      {
                                          Status = "PO Received",
                                          Count = (from x in Db.Quotations where x.QuotationStatus == "PO Received" && x.CompanyId == companyID && (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate)) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status) select x).Count(),
                                          Total = Db.Quotations.Where(x => x.QuotationStatus == "PO Received" && x.CompanyId == companyID && (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate)) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status)).Sum(s => Convert.ToInt16(s.Total)),
                                          Quotations = Db.Quotations.Where(x => x.QuotationStatus == "PO Received" && x.CompanyId == companyID && (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate)) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status)).ToList(),
                                      }).Distinct().ToList();
                    allCases.AddRange(POReceived);

                    var Invoiced = (from s in Db.Quotations
                                    select new QuotationStaticViewModel
                                    {
                                        Status = "Invoiced",
                                        Count = (from x in Db.Quotations where x.QuotationStatus == "Invoiced" && x.CompanyId == companyID && (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate)) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status) select x).Count(),
                                        Total = Db.Quotations.Where(x => x.QuotationStatus == "Invoiced" && x.CompanyId == companyID && (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate)) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status)).Sum(s => Convert.ToInt16(s.Total)),
                                        Quotations = Db.Quotations.Where(x => x.QuotationStatus == "Invoiced" && x.CompanyId == companyID && (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate)) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status)).ToList(),
                                    }).Distinct().ToList();
                    allCases.AddRange(Invoiced);


                    var Cancelled = (from s in Db.Quotations
                                     select new QuotationStaticViewModel
                                     {
                                         Status = "Cancelled",
                                         Count = (from x in Db.Quotations where x.QuotationStatus == "Cancelled" && x.CompanyId == companyID && (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate)) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status) select x).Count(),
                                         Total = Db.Quotations.Where(x => x.QuotationStatus == "Cancelled" && x.CompanyId == companyID && (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate)) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status)).Sum(s => Convert.ToInt16(s.Total)),
                                         Quotations = Db.Quotations.Where(x => x.QuotationStatus == "Cancelled" && x.CompanyId == companyID && (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate)) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status)).ToList(),
                                     }).Distinct().ToList();
                    allCases.AddRange(Cancelled);
                }


                else
                {
                    var Proposal = (from s in Db.Quotations
                                    select new QuotationStaticViewModel
                                    {
                                        Status = status,
                                        Count = (from x in Db.Quotations where x.QuotationStatus == status && x.CompanyId == companyID && (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate)) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status) select x).Count(),
                                        Total = Db.Quotations.Where(x => x.QuotationStatus == status && x.CompanyId == companyID && (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate)) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status)).Sum(s => Convert.ToInt16(s.Total)),
                                        Quotations = Db.Quotations.Where(x => x.QuotationStatus == status && x.CompanyId == companyID && (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate)) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status)).ToList(),
                                    }).Distinct().ToList();
                    allCases.AddRange(Proposal);

                    //var Quotation = (from s in Db.Quotations
                    //                 select new QuotationStaticViewModel
                    //                 {
                    //                     Status = "Quotation",
                    //                     Count = (from x in Db.Quotations where x.QuotationStatus == "Quotation" && x.CompanyId == companyID && (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate)) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status) select x).Count(),
                    //                     Total = Db.Quotations.Where(x => x.QuotationStatus == "Quotation" && x.CompanyId == companyID && (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate)) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status)).Sum(s => Convert.ToInt16(s.Total)),
                    //                     Quotations = Db.Quotations.Where(x => x.QuotationStatus == "Quotation" && x.CompanyId == companyID && (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate)) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status)).ToList(),
                    //                 }).Distinct().ToList();
                    //allCases.AddRange(Quotation);


                    //var WaitingPO = (from s in Db.Quotations
                    //                 select new QuotationStaticViewModel
                    //                 {
                    //                     Status = "Waiting PO",
                    //                     Count = (from x in Db.Quotations where x.QuotationStatus == "Waiting PO" && x.CompanyId == companyID && (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate)) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status) select x).Count(),
                    //                     Total = Db.Quotations.Where(x => x.QuotationStatus == "Waiting PO" && x.CompanyId == companyID && (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate)) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status)).Sum(s => Convert.ToInt16(s.Total)),
                    //                     Quotations = Db.Quotations.Where(x => x.QuotationStatus == "Waiting PO" && x.CompanyId == companyID && (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate)) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status)).ToList(),
                    //                 }).Distinct().ToList();
                    //allCases.AddRange(WaitingPO);

                    //var POReceived = (from s in Db.Quotations
                    //                  select new QuotationStaticViewModel
                    //                  {
                    //                      Status = "PO Received",
                    //                      Count = (from x in Db.Quotations where x.QuotationStatus == "PO Received" && x.CompanyId == companyID && (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate)) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status) select x).Count(),
                    //                      Total = Db.Quotations.Where(x => x.QuotationStatus == "PO Received" && x.CompanyId == companyID && (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate)) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status)).Sum(s => Convert.ToInt16(s.Total)),
                    //                      Quotations = Db.Quotations.Where(x => x.QuotationStatus == "PO Received" && x.CompanyId == companyID && (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate)) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status)).ToList(),
                    //                  }).Distinct().ToList();
                    //allCases.AddRange(POReceived);

                    //var Invoiced = (from s in Db.Quotations
                    //                select new QuotationStaticViewModel
                    //                {
                    //                    Status = "Invoiced",
                    //                    Count = (from x in Db.Quotations where x.QuotationStatus == "Invoiced" && x.CompanyId == companyID && (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate)) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status) select x).Count(),
                    //                    Total = Db.Quotations.Where(x => x.QuotationStatus == "Invoiced" && x.CompanyId == companyID && (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate)) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status)).Sum(s => Convert.ToInt16(s.Total)),
                    //                    Quotations = Db.Quotations.Where(x => x.QuotationStatus == "Invoiced" && x.CompanyId == companyID && (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate)) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status)).ToList(),
                    //                }).Distinct().ToList();
                    //allCases.AddRange(Invoiced);


                    //var Cancelled = (from s in Db.Quotations
                    //                 select new QuotationStaticViewModel
                    //                 {
                    //                     Status = "Cancelled",
                    //                     Count = (from x in Db.Quotations where x.QuotationStatus == "Cancelled" && x.CompanyId == companyID && (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate)) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status) select x).Count(),
                    //                     Total = Db.Quotations.Where(x => x.QuotationStatus == "Cancelled" && x.CompanyId == companyID && (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate)) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status)).Sum(s => Convert.ToInt16(s.Total)),
                    //                     Quotations = Db.Quotations.Where(x => x.QuotationStatus == "Cancelled" && x.CompanyId == companyID && (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate)) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status)).ToList(),
                    //                 }).Distinct().ToList();
                    //allCases.AddRange(Cancelled);
                }
            }
            else
            {
                if (status == "")
                {
                    var Proposal = (from s in Db.Quotations
                                    select new QuotationStaticViewModel
                                    {
                                        Status = "Proposal",
                                        Count = (from x in Db.Quotations where x.QuotationStatus == "Proposal" && x.CompanyId == companyID && x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.CreatedBy == uId select x).Count(),
                                        Total = Db.Quotations.Where(x => x.QuotationStatus == "Proposal" && x.CompanyId == companyID && x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.CreatedBy == uId).Sum(s => Convert.ToInt16(s.Total)),
                                        Quotations = Db.Quotations.Where(x => x.QuotationStatus == "Proposal" && x.CompanyId == companyID && x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.CreatedBy == uId ).ToList(),
                                    }).Distinct().ToList();
                    allCases.AddRange(Proposal);

                    var Quotation = (from s in Db.Quotations
                                     select new QuotationStaticViewModel
                                     {
                                         Status = "Quotation",
                                         Count = (from x in Db.Quotations where x.QuotationStatus == "Quotation" && x.CompanyId == companyID && x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.CreatedBy == uId select x).Count(),
                                         Total = Db.Quotations.Where(x => x.QuotationStatus == "Quotation" && x.CompanyId == companyID && x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.CreatedBy == uId).Sum(s => Convert.ToInt16(s.Total)),
                                         Quotations = Db.Quotations.Where(x => x.QuotationStatus == "Quotation" && x.CompanyId == companyID && x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.CreatedBy == uId).ToList(),
                                     }).Distinct().ToList();
                    allCases.AddRange(Quotation);


                    var WaitingPO = (from s in Db.Quotations
                                     select new QuotationStaticViewModel
                                     {
                                         Status = "Waiting PO",
                                         Count = (from x in Db.Quotations where x.QuotationStatus == "Waiting PO" && x.CompanyId == companyID && x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.CreatedBy == uId select x).Count(),
                                         Total = Db.Quotations.Where(x => x.QuotationStatus == "Waiting PO" && x.CompanyId == companyID && x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.CreatedBy == uId).Sum(s => Convert.ToInt16(s.Total)),
                                         Quotations = Db.Quotations.Where(x => x.QuotationStatus == "Waiting PO" && x.CompanyId == companyID && x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.CreatedBy == uId).ToList(),
                                     }).Distinct().ToList();
                    allCases.AddRange(WaitingPO);

                    var POReceived = (from s in Db.Quotations
                                      select new QuotationStaticViewModel
                                      {
                                          Status = "PO Received",
                                          Count = (from x in Db.Quotations where x.QuotationStatus == "PO Received" && x.CompanyId == companyID && x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.CreatedBy == uId select x).Count(),
                                          Total = Db.Quotations.Where(x => x.QuotationStatus == "PO Received" && x.CompanyId == companyID && x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.CreatedBy == uId).Sum(s => Convert.ToInt16(s.Total)),
                                          Quotations = Db.Quotations.Where(x => x.QuotationStatus == "PO Received" && x.CompanyId == companyID && x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.CreatedBy == uId).ToList(),
                                      }).Distinct().ToList();
                    allCases.AddRange(POReceived);

                    var Invoiced = (from s in Db.Quotations
                                    select new QuotationStaticViewModel
                                    {
                                        Status = "Invoiced",
                                        Count = (from x in Db.Quotations where x.QuotationStatus == "Invoiced" && x.CompanyId == companyID && x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.CreatedBy == uId select x).Count(),
                                        Total = Db.Quotations.Where(x => x.QuotationStatus == "Invoiced" && x.CompanyId == companyID && x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.CreatedBy == uId).Sum(s => Convert.ToInt16(s.Total)),
                                        Quotations = Db.Quotations.Where(x => x.QuotationStatus == "Invoiced" && x.CompanyId == companyID && x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.CreatedBy == uId).ToList(),
                                    }).Distinct().ToList();
                    allCases.AddRange(Invoiced);


                    var Cancelled = (from s in Db.Quotations
                                     select new QuotationStaticViewModel
                                     {
                                         Status = "Cancelled",
                                         Count = (from x in Db.Quotations where x.QuotationStatus == "Cancelled" && x.CompanyId == companyID && x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.CreatedBy == uId select x).Count(),
                                         Total = Db.Quotations.Where(x => x.QuotationStatus == "Cancelled" && x.CompanyId == companyID && x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.CreatedBy == uId).Sum(s => Convert.ToInt16(s.Total)),
                                         Quotations = Db.Quotations.Where(x => x.QuotationStatus == "Cancelled" && x.CompanyId == companyID && x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.CreatedBy == uId).ToList(),
                                     }).Distinct().ToList();
                    allCases.AddRange(Cancelled);
                }


                else
                {
                    var Proposal = (from s in Db.Quotations
                                    select new QuotationStaticViewModel
                                    {
                                        Status = status,
                                        Count = (from x in Db.Quotations where x.QuotationStatus == status && x.CompanyId == companyID && x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.CreatedBy == uId select x).Count(),
                                        Total = Db.Quotations.Where(x => x.QuotationStatus == status && x.CompanyId == companyID && x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.CreatedBy == uId).Sum(s => Convert.ToInt16(s.Total)),
                                        Quotations = Db.Quotations.Where(x => x.QuotationStatus == status && x.CompanyId == companyID && x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.CreatedBy == uId).ToList(),
                                    }).Distinct().ToList();
                    allCases.AddRange(Proposal);

                    //var Quotation = (from s in Db.Quotations
                    //                 select new QuotationStaticViewModel
                    //                 {
                    //                     Status = "Quotation",
                    //                     Count = (from x in Db.Quotations where x.QuotationStatus == "Quotation" && x.CompanyId == companyID && (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate)) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status) select x).Count(),
                    //                     Total = Db.Quotations.Where(x => x.QuotationStatus == "Quotation" && x.CompanyId == companyID && (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate)) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status)).Sum(s => Convert.ToInt16(s.Total)),
                    //                     Quotations = Db.Quotations.Where(x => x.QuotationStatus == "Quotation" && x.CompanyId == companyID && (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate)) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status)).ToList(),
                    //                 }).Distinct().ToList();
                    //allCases.AddRange(Quotation);


                    //var WaitingPO = (from s in Db.Quotations
                    //                 select new QuotationStaticViewModel
                    //                 {
                    //                     Status = "Waiting PO",
                    //                     Count = (from x in Db.Quotations where x.QuotationStatus == "Waiting PO" && x.CompanyId == companyID && (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate)) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status) select x).Count(),
                    //                     Total = Db.Quotations.Where(x => x.QuotationStatus == "Waiting PO" && x.CompanyId == companyID && (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate)) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status)).Sum(s => Convert.ToInt16(s.Total)),
                    //                     Quotations = Db.Quotations.Where(x => x.QuotationStatus == "Waiting PO" && x.CompanyId == companyID && (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate)) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status)).ToList(),
                    //                 }).Distinct().ToList();
                    //allCases.AddRange(WaitingPO);

                    //var POReceived = (from s in Db.Quotations
                    //                  select new QuotationStaticViewModel
                    //                  {
                    //                      Status = "PO Received",
                    //                      Count = (from x in Db.Quotations where x.QuotationStatus == "PO Received" && x.CompanyId == companyID && (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate)) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status) select x).Count(),
                    //                      Total = Db.Quotations.Where(x => x.QuotationStatus == "PO Received" && x.CompanyId == companyID && (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate)) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status)).Sum(s => Convert.ToInt16(s.Total)),
                    //                      Quotations = Db.Quotations.Where(x => x.QuotationStatus == "PO Received" && x.CompanyId == companyID && (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate)) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status)).ToList(),
                    //                  }).Distinct().ToList();
                    //allCases.AddRange(POReceived);

                    //var Invoiced = (from s in Db.Quotations
                    //                select new QuotationStaticViewModel
                    //                {
                    //                    Status = "Invoiced",
                    //                    Count = (from x in Db.Quotations where x.QuotationStatus == "Invoiced" && x.CompanyId == companyID && (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate)) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status) select x).Count(),
                    //                    Total = Db.Quotations.Where(x => x.QuotationStatus == "Invoiced" && x.CompanyId == companyID && (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate)) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status)).Sum(s => Convert.ToInt16(s.Total)),
                    //                    Quotations = Db.Quotations.Where(x => x.QuotationStatus == "Invoiced" && x.CompanyId == companyID && (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate)) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status)).ToList(),
                    //                }).Distinct().ToList();
                    //allCases.AddRange(Invoiced);


                    //var Cancelled = (from s in Db.Quotations
                    //                 select new QuotationStaticViewModel
                    //                 {
                    //                     Status = "Cancelled",
                    //                     Count = (from x in Db.Quotations where x.QuotationStatus == "Cancelled" && x.CompanyId == companyID && (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate)) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status) select x).Count(),
                    //                     Total = Db.Quotations.Where(x => x.QuotationStatus == "Cancelled" && x.CompanyId == companyID && (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate)) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status)).Sum(s => Convert.ToInt16(s.Total)),
                    //                     Quotations = Db.Quotations.Where(x => x.QuotationStatus == "Cancelled" && x.CompanyId == companyID && (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate)) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.CreatedBy == uId) || (x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) && x.QuotationStatus == status)).ToList(),
                    //                 }).Distinct().ToList();
                    //allCases.AddRange(Cancelled);
                }
            }
        

            return allCases;
        }

        public List<QuotationStaticViewModel> GetFilterStaticsByDate(string fromDate, string ToDate, long companyID)
        {
            List<QuotationStaticViewModel> allCases = new List<QuotationStaticViewModel>();

            var Proposal = (from s in Db.Quotations
                            select new QuotationStaticViewModel
                            {
                                Status = "Proposal",
                                Count = (from x in Db.Quotations where x.QuotationStatus == "Proposal" && x.CompanyId == companyID && x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) select x).Count(),
                                Total = Db.Quotations.Where(x => x.QuotationStatus == "proposal" && x.CompanyId == companyID && x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate)).Sum(s => Convert.ToInt16(s.Total)),
                                Quotations = Db.Quotations.Where(x => x.QuotationStatus == "proposal" && x.CompanyId == companyID && x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate)).ToList(),
                            }).Distinct().ToList();
            allCases.AddRange(Proposal);

            var Quotation = (from s in Db.Quotations
                             select new QuotationStaticViewModel
                             {
                                 Status = "Quotation",
                                 Count = (from x in Db.Quotations where x.QuotationStatus == "Quotation" && x.CompanyId == companyID && x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate)  select x).Count(),
                                 Total = Db.Quotations.Where(x => x.QuotationStatus == "Quotation" && x.CompanyId == companyID && x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate)).Sum(s => Convert.ToInt16(s.Total)),
                                 Quotations = Db.Quotations.Where(x => x.QuotationStatus == "Quotation" && x.CompanyId == companyID && x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate)).ToList(),
                             }).Distinct().ToList();
            allCases.AddRange(Quotation);


            var WaitingPO = (from s in Db.Quotations
                             select new QuotationStaticViewModel
                             {
                                 Status = "Waiting PO",
                                 Count = (from x in Db.Quotations where x.QuotationStatus == "Waiting PO" && x.CompanyId == companyID && x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) select x).Count(),
                                 Total = Db.Quotations.Where(x => x.QuotationStatus == "Waiting PO" && x.CompanyId == companyID && x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate)).Sum(s => Convert.ToInt16(s.Total)),
                                 Quotations = Db.Quotations.Where(x => x.QuotationStatus == "Waiting PO" && x.CompanyId == companyID && x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate)).ToList(),
                             }).Distinct().ToList();
            allCases.AddRange(WaitingPO);

            var POReceived = (from s in Db.Quotations
                              select new QuotationStaticViewModel
                              {
                                  Status = "PO Received",
                                  Count = (from x in Db.Quotations where x.QuotationStatus == "PO Received" && x.CompanyId == companyID && x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) select x).Count(),
                                  Total = Db.Quotations.Where(x => x.QuotationStatus == "PO Received" && x.CompanyId == companyID && x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate)).Sum(s => Convert.ToInt16(s.Total)),
                                  Quotations = Db.Quotations.Where(x => x.QuotationStatus == "PO Received" && x.CompanyId == companyID && x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate)).ToList(),
                              }).Distinct().ToList();
            allCases.AddRange(POReceived);

            var Invoiced = (from s in Db.Quotations
                            select new QuotationStaticViewModel
                            {
                                Status = "Invoiced",
                                Count = (from x in Db.Quotations where x.QuotationStatus == "Invoiced" && x.CompanyId == companyID && x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) select x).Count(),
                                Total = Db.Quotations.Where(x => x.QuotationStatus == "Invoiced" && x.CompanyId == companyID && x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate)).Sum(s => Convert.ToInt16(s.Total)),
                                Quotations = Db.Quotations.Where(x => x.QuotationStatus == "Invoiced" && x.CompanyId == companyID && x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate)).ToList(),
                            }).Distinct().ToList();
            allCases.AddRange(Invoiced);


            var Cancelled = (from s in Db.Quotations
                             select new QuotationStaticViewModel
                             {
                                 Status = "Cancelled",
                                 Count = (from x in Db.Quotations where x.QuotationStatus == "Cancelled" && x.CompanyId == companyID && x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate) select x).Count(),
                                 Total = Db.Quotations.Where(x => x.QuotationStatus == "Cancelled" && x.CompanyId == companyID && x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate)).Sum(s => Convert.ToInt16(s.Total)),
                                 Quotations = Db.Quotations.Where(x => x.QuotationStatus == "Cancelled" && x.CompanyId == companyID && x.EmailDate >= Convert.ToDateTime(fromDate) && x.EmailDate <= Convert.ToDateTime(ToDate)).ToList(),
                             }).Distinct().ToList();
            allCases.AddRange(Cancelled);

            return allCases;
        }
    }
}
