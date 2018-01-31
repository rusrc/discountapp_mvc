using Discountapp.Domain;
using Discountapp.Domain.Models.Application;

namespace Discountapp.Infrastructure.Repositories.EntityFramework
{
    public class MerchantCategoryRepository : Repository<MerchantCategory>, IMerchantCategoryRepository
    {
        public MerchantCategoryRepository(DiscountappDbContext context) : base(context)
        {
        }
    }
}
