using Discountapp.Domain.Models.Application;
using System;
using System.Collections.Generic;
using Discountapp.Domain;

namespace Discountapp.Infrastructure.Repositories.Memory
{
    public class PromotionItemRepository : Repository<PromotionItem>, IPromotionItemRepository
    {
        public PromotionItemRepository(DiscountappMemoryContext context) 
            : base(context)
        {
        }

        public IEnumerable<PromotionItem> GetAll(long categoryId, long cityId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PromotionItem> GetAllByCategory(long cityId, long? categoryId = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PromotionItem> GetAllByMerchant(long cityId, long merchantId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PromotionItem> GetAllWithPromotions()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PromotionItem> GetAllByCityId(long cityId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PromotionItem> GetAllByUser(long userId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PromotionItem> GetAllByUser(long userId, long cityId)
        {
            throw new NotImplementedException();
        }
    }
}
