using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Discountapp.Domain;
using Discountapp.Domain.Models.Application;

namespace Discountapp.Infrastructure.Repositories.EntityFramework
{
    public class PromotionRepository : Repository<Promotion>, IPromotionRepository
    {
        public PromotionRepository(DiscountappDbContext context)
            : base(context)
        {
        }

        public IEnumerable<Promotion> GetAllByCategory(long cityId, long? categoryId = null)
        {
            IQueryable<RealEstate> realEstates = null;

            if (categoryId != null)
                realEstates = _context.RealEstates
                                .Include(r => r.Address)
                                .Include(r => r.Promotions)        
                                .Include(r => r.Promotions.Select(p => p.PromotionItems))
                                .Where(r => r.MerchantCategoryId == categoryId && r.Address.CityId == cityId);
            else
                realEstates = _context.RealEstates
                                .Include(r => r.Address)
                                .Include(r => r.Promotions)
                                .Include(r => r.Promotions.Select(p => p.PromotionItems))
                                .Where(r => r.Address.CityId == cityId);


            return realEstates.SelectMany(r => r.Promotions).ToList();
        }

        public IEnumerable<Promotion> GetAllByMerchant(long cityId, long merchantId)
        {
            var realEstates = this._context.RealEstates
                                .Include(r => r.Address)
                                .Include(r => r.Promotions)
                                .Include(r => r.Promotions.Select(p => p.PromotionItems))
                                .Where(r => r.Id == merchantId && r.Address.CityId == cityId);

            return realEstates.SelectMany(r => r.Promotions).ToList();
        }

        public IEnumerable<Promotion> GetAllByUser(long userId)
        {
            var list = this._context
                .Promotions
                .Include(e => e.RealEstates)
                .Include(e => e.PromotionItems)
                .Include(e => e.User)
                .Where(p => p.UserId == userId)
                .ToList();

            return list;
        }

        public IEnumerable<Promotion> GetAllWithIncludes()
        {
            return _context.Promotions
                           .Include(e => e.RealEstates)
                           .Include(e => e.PromotionItems)
                           .ToList();
        }

        public DiscountappDbContext DiscountAppDbContext => this._context as DiscountappDbContext;
    }
}
