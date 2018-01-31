using System.Collections.Generic;
using System.Threading.Tasks;
using Discountapp.Domain.Models.Application;

namespace Discountapp.Infrastructure.Repositories
{
    public interface IPromotionRepository : IRepository<Promotion>
    {
        IEnumerable<Promotion> GetAllByUser(long userId);
        IEnumerable<Promotion> GetAllWithIncludes();
        IEnumerable<Promotion> GetAllByMerchant(long cityId, long merchantId);
        IEnumerable<Promotion> GetAllByCategory(long cityId, long? categoryId = null);
    }
}
