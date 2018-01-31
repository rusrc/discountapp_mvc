using System.Linq;

namespace Discountapp.Domain.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<Discountapp.Domain.DiscountappDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Discountapp.Domain.DiscountappDbContext context)
        {
            if (!context.Cities.Any())
                new DiscountappDbInitializer().SeedDatabaseContext(context);
        }
    }
}
