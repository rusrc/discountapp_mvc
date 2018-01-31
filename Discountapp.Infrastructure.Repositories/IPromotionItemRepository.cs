using Discountapp.Domain.Models.Application;
using System.Collections.Generic;

namespace Discountapp.Infrastructure.Repositories
{
    public interface IPromotionItemRepository : IRepository<PromotionItem>
    {
        IEnumerable<PromotionItem> GetAllByUser(long userId);
        IEnumerable<PromotionItem> GetAllByUser(long userId, long cityId);
        IEnumerable<PromotionItem> GetAllByCityId(long cityId);
        IEnumerable<PromotionItem> GetAll(long categoryId, long cityId);
        IEnumerable<PromotionItem> GetAllByCategory(long cityId, long? categoryId = null);
        IEnumerable<PromotionItem> GetAllByMerchant(long cityId, long merchantId);
        IEnumerable<PromotionItem> GetAllWithPromotions();
    }
}