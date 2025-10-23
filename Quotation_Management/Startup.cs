using DevExpress.AspNetCore;
using DevExpress.AspNetCore.Reporting;
using DevExpress.Security.Resources;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Quotation_Management.Data;
using Quotation_Management.Models;
using Quotation_Management.Repos;
using Quotation_Management.Repos.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Quotation_Management
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var lockout = new LockoutOptions()
            {
                AllowedForNewUsers = true,
                DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1),
                MaxFailedAccessAttempts = 3
            };

            services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(
                        Configuration.GetConnectionString("DefaultConnection")));

            //services.Configure<IdentityOptions>(options =>
            //{ 
            //    options.Password.RequireDigit = false;
            //    options.Password.RequireLowercase = false;
            //    options.Password.RequireNonAlphanumeric = false;
            //    options.Password.RequireUppercase = false;
            //    options.Password.RequiredLength = 0;
            //    options.Password.RequiredUniqueChars = 0;

            //    options.User.RequireUniqueEmail = true;  
            //});
            services.AddDevExpressControls();
            services
            .AddMvc()
            .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0);
            services.ConfigureReportingServices(configurator => {
                configurator.ConfigureWebDocumentViewer(viewerConfigurator => {
                    viewerConfigurator.UseCachedReportSourceBuilder();
                });
            });

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAppPageRepository, AppPageRepository>();
            services.AddScoped<IAppPagePermissionRepository, AppPagePermissionRepository>();
            services.AddScoped<IAdminRepo, AdminRepo>();
            services.AddScoped<ICompanyRepo, CompanyRepo>();
            services.AddScoped<IBankRepo, BankRepo>();
            services.AddScoped<IMasterRepository, MasterRepository>();
            services.AddScoped<IMasterDetailRepository, MasterDetailRepository>();
            services.AddScoped<IClientRepo, ClientRepo>();
            services.AddScoped<ISubClientRepo, SubClientRepo>();
            services.AddScoped<ITermConditionRepo, TermConditionRepo>();
            services.AddScoped<IQuotationRepo, QuotationRepo>();
            services.AddScoped<IMainItemRepo, MainItemRepo>();
            services.AddScoped<IRegionRepo, RegionRepo>();
            services.AddScoped<IUserRegionRepo, UsersRegionRepo>();
            services.AddScoped<IMainItemRegionsRepo, MainItemRegionRepo>();
            services.AddScoped<ISubItemRegionsRepo, SubItemRegionsRepo>();
            services.AddScoped<IAssignClientRepo, AssignClientRepo>();

            services.AddScoped<IQuotationHistoryRepo, QuotationHistoryRepo>();
            services.AddScoped<IQuotationHistoryDetailRepo, QuotationHistoryDetailRepo>();
            services.AddScoped<IFlowStatusRepo, FlowStatusRepo>();

            services.AddScoped<ICountryMasterRepo, CountryMasterRepo>();
             
            services.AddIdentityCore<ApplicationUser>()
                .AddRoles<ApplicationRole>()
                .AddClaimsPrincipalFactory<UserClaimsPrincipalFactory<ApplicationUser, ApplicationRole>>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddDefaultUI();


            services.ConfigureApplicationCookie(options =>
            options.ExpireTimeSpan = TimeSpan.FromMinutes(30)
            );

            services.AddDefaultIdentity<IdentityUser>()
              .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
            services.AddRazorPages();


            services.AddIdentityCore<ApplicationUser>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddDefaultUI();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            string contentPath = env.ContentRootPath;
            AppDomain.CurrentDomain.SetData("DXResourceDirectory", contentPath);
            
            app.UseDevExpressControls();
            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()); 
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseExceptionHandler(
            builder =>
            {
                builder.Run(
                    async context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        context.Response.Headers.Add("Access-Control-Allow-Origin", "*");

                        var error = context.Features.Get<IExceptionHandlerFeature>();
                        if (error != null)
                        {
                              //context.Response.AddApplicationError(error.Error.Message);
                              await context.Response.WriteAsync(error.Error.Message).ConfigureAwait(false);
                        }
                    });
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                 endpoints.MapRazorPages();
            });

            //CreateSuperAdminUser(serviceProvider);

        }

        private void CreateSuperAdminUser(IServiceProvider serviceProvider)
        {

            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            //Check if the admin user exists and create it if not
            //Add to the Administrator role

            //string email = "superadmin@limitlesssol.com";
            string email = "superadmin@domain.com";
            //string username = "superadmin@limitlesssol.com";
            string username = "superadmin@domain.com";

            Task<ApplicationUser> superadmin = userManager.FindByEmailAsync(email);
            superadmin.Wait();


            ApplicationUser administrator = new ApplicationUser();
            administrator.Email = email;
            administrator.UserName = username;
            administrator.CompanyId = 3;

            Task<IdentityResult> newUser = userManager.CreateAsync(administrator, "@Dm1N@$0$5#");
            newUser.Wait();


        }
    }
}
