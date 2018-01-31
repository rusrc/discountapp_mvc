using System.Data.Entity;
using Discountapp.Domain.Models.Application;
using Discountapp.Domain.Models.Identity;
using Discountapp.Domain.Models.Location;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Discountapp.Infrastructure.EntityFramework
{
    public class DiscountappDbContext
        : IdentityDbContext<User, Role, long, UserLogin, UserRole, UserClaim>
    {
        public DiscountappDbContext() 
            :base(nameof(DiscountappDbContext))
        {
            //Database.SetInitializer(new DiscountappDbInitializer());
        }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<AdvertCampaign> AdvertCampaigns { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<City>  Cities { get; set; }
    }
}
