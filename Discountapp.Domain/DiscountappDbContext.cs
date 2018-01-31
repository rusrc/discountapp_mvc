using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Discountapp.Domain.Models.Application;
using Discountapp.Domain.Models.Identity;
using Discountapp.Domain.Models.Location;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Discountapp.Domain
{
    enum DiscountappDbType
    {
        DiscountappDbContext,
        DiscountappDbContextAzure
    }
    public class DiscountappDbContext : IdentityDbContext<AppUser, AppRole, long, AppUserLogin, AppUserRole, AppUserClaim>
    {
        public DiscountappDbContext()
            : base(nameof(DiscountappDbType.DiscountappDbContext))
        {
            //Database.SetInitializer(new DiscountappDbInitializer());
        }
        public DbSet<MerchantType> MerchantTypes { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<PromotionItem> PromotionItems { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<MobileUser> MobileUser { get; set; }
        public DbSet<MerchantCategory> MerchantCategories  { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet <RealEstate> RealEstates { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<AppUser>().ToTable("User");
            modelBuilder.Entity<AppRole>().ToTable("Role");
            modelBuilder.Entity<AppUserRole>().ToTable("UserRole");
            modelBuilder.Entity<AppUserClaim>().ToTable("UserClaim");
            modelBuilder.Entity<AppUserLogin>().ToTable("UserLogin");
            modelBuilder.Entity<AppUser>()
                .Property(r => r.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<AppUserClaim>()
                .Property(r => r.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<AppRole>()
                .Property(r => r.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            #region comples type

            modelBuilder.Types<Address>().Configure(e => e.Property(cust => cust.WorkTime.Begin)
             .HasColumnName("WorkTimeBegin")
             .IsOptional());

            modelBuilder.Types<Address>().Configure(e => e.Property(cust => cust.WorkTime.End)
            .HasColumnName("WorkTimeEnd")
            .IsOptional());

            modelBuilder.Types<Address>().Configure(e => e.Property(cast => cast.WorkTimeSaturday.Begin)
            .HasColumnName("WorkTimeSaturdayBegin")
            .IsOptional());

            modelBuilder.Types<Address>().Configure(e => e.Property(cast => cast.WorkTimeSaturday.End)
            .HasColumnName("WorkTimeSaturdayEnd")
            .IsOptional());

            modelBuilder.Types<Address>().Configure(e => e.Property(cast => cast.WorkTimeSunday.Begin)
            .HasColumnName("WorkTimeSundayBegin")
            .IsOptional());

            modelBuilder.Types<Address>().Configure(e => e.Property(cast => cast.WorkTimeSunday.End)
            .HasColumnName("WorkTimeSundayEnd")
            .IsOptional());

            #endregion

        }
    }
}
