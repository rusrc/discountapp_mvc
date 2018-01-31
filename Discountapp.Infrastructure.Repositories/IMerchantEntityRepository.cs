using Discountapp.Domain.Models.Application;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Discountapp.Infrastructure.Repositories
{
    public interface IMerchantEntityRepository : IRepository<MerchantEntity>
    {
        IEnumerable<MerchantEntity> GetAll(long categoryId, long cityId);
    }
}
