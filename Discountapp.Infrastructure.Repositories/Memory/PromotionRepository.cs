using System;
using System.Collections.Generic;
using Discountapp.Domain.Models.Application;
using System.IO;

namespace Discountapp.Infrastructure.Repositories.Memory
{
    using Discountapp.Domain;
    using Config = Discountapp.Infrastructure.Constants.Config;
    public class PromotionRepository : Repository<Promotion>, IPromotionRepository
    {
        const string PROMOTION_NAME = "Название акции {0}";
        const string DEFAULT_NAME = "Название / имя по умолчанию '{0}'";
        private string _defaultImagePath = Path.Combine(Config.UploadFolderFullPath, "Default", Config.DefaultPromotionItemImageName);

        public PromotionRepository(DiscountappMemoryContext context) 
            : base(context)
        {
        }

        public IEnumerable<Promotion> GetAllByCategory(long cityId, long? categoryId = default(long?))
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Promotion> GetAllByMerchant(long cityId, long merchantId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Promotion> GetAllByUser(long userId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Promotion> GetAllWithIncludes()
        {
            throw new NotImplementedException();
        }
    }
}
