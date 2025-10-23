using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Quotation_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Quotation_Management.Data
{
    public interface ISoftDeleteModel
    {
        bool IsDeleted { get; set; }
    }
    public static class EFFilterExtensions
    {
        public static void SetSoftDeleteFilter(this ModelBuilder modelBuilder, Type entityType)
        {
            SetSoftDeleteFilterMethod.MakeGenericMethod(entityType)
                .Invoke(null, new object[] { modelBuilder });
        }

        static readonly MethodInfo SetSoftDeleteFilterMethod = typeof(EFFilterExtensions)
                   .GetMethods(BindingFlags.Public | BindingFlags.Static)
                   .Single(t => t.IsGenericMethod && t.Name == "SetSoftDeleteFilter");

        public static void SetSoftDeleteFilter<TEntity>(this ModelBuilder modelBuilder)
            where TEntity : class, ISoftDeleteModel
        {
            modelBuilder.Entity<TEntity>().HasQueryFilter(x => !x.IsDeleted);
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser,ApplicationRole, string>
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)

        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<AppPage>().ToTable("AppPage");
            modelBuilder.Entity<AppPagePermission>().ToTable("AppPagePermission");
            modelBuilder.Entity<Company>().ToTable("Company");
            modelBuilder.Entity<Banks>().ToTable("Banks");
            modelBuilder.Entity<Master_Table>().ToTable("Master_Table");
            modelBuilder.Entity<Master_Detail_Table>().ToTable("Master_Detail_Table");
            modelBuilder.Entity<Clients>().ToTable("Clients");
            modelBuilder.Entity<SubClients>().ToTable("SubClients");
            modelBuilder.Entity<TermsConditions>().ToTable("TermsConditions");
            modelBuilder.Entity<Quotation>().ToTable("Quotations");
            modelBuilder.Entity<MainItem>().ToTable("MainItems");
            modelBuilder.Entity<ItemRegions>().ToTable("ItemRegions");
            modelBuilder.Entity<UsersRegion>().ToTable("UsersRegion");
            modelBuilder.Entity<MainItemRegion>().ToTable("MainItemRegion");

            modelBuilder.Entity<QuotationHistory>().ToTable("QuotationHistory");
            modelBuilder.Entity<QuotationHistoryDetail>().ToTable("QuotationHistoryDetail");
            modelBuilder.Entity<ClientAssign>().ToTable("ClientAssign");
            modelBuilder.Entity<AssignFlowStatus>().ToTable("AssignFlowStatus");
            modelBuilder.Entity<CountryMaster>().ToTable("CountryMaster");
 


            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                modelBuilder.Entity(entityType.ClrType).Property<bool>("IsDeleted");
                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var body = Expression.Equal(
                    Expression.Call(typeof(EF), nameof(EF.Property), new[] { typeof(bool) }, parameter, Expression.Constant("IsDeleted")),
                Expression.Constant(false));
                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(Expression.Lambda(body, parameter));
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var directory = System.IO.Directory.GetCurrentDirectory();

                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(directory)
                    .AddJsonFile("appsettings.json")
                    .Build();

                optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), options => options.ExecutionStrategy(c => new MyExecutionStrategy(c)));

            }
        }

        public DbSet<AppPage> AppPages { get; set; }
        public DbSet<AppPagePermission> AppPagePermissions { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Banks> Banks { get; set; }
        public DbSet<Master_Table> Master_Tables { get; set; }
        public DbSet<Master_Detail_Table> Master_Detail_Tables { get; set; }
        public DbSet<Clients> Clients { get; set; }
        public DbSet<SubClients> SubClients { get; set; }
        public DbSet<TermsConditions> TermsConditions { get; set; }
        public DbSet<Quotation> Quotations { get; set; }
        public DbSet<MainItem> MainItems { get; set; }
        public DbSet<ItemRegions> ItemRegions { get; set; }
        public DbSet<UsersRegion> UsersRegions { get; set; }
        public DbSet<MainItemRegion> MainItemRegions { get; set; }
        public DbSet<SubItemRegion> SubItemRegions { get; set; }
        public DbSet<QuotationHistory> QuotationHistories { get; set; }
        public DbSet<QuotationHistoryDetail> QuotationHistoryDetails { get; set; }
        public DbSet<ClientAssign> ClientAssigns { get; set; }
        public DbSet<AssignFlowStatus> AssignFlowStatuses { get; set; }
        public DbSet<CountryMaster> CountryMasters { get; set; }
        //public DbSet<Configuration> Configurations { get; set; }


        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            UpdteDeleteStatus();
            var result = base.SaveChanges(acceptAllChangesOnSuccess);
            return result;
        }


        public void UpdteDeleteStatus()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        if (entry.CurrentValues.Properties.Any(m => m.Name == "IsDeleted"))
                            entry.CurrentValues["IsDeleted"] = false;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        if (entry.CurrentValues.Properties.Any(k => k.Name == "IsDeleted"))
                        {
                            entry.CurrentValues["IsDeleted"] = true;
                            //entry.CurrentValues["DeletedAt"] = DateTime.Now;
                        }
                        break;
                }
            }
        }
    }
}
