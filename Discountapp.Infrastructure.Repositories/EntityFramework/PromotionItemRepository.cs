using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Discountapp.Domain;
using Discountapp.Domain.Models.Application;
using Z.EntityFramework.Plus;

namespace Discountapp.Infrastructure.Repositories.EntityFramework
{
    public class PromotionItemRepository : Repository<PromotionItem>, IPromotionItemRepository
    {
        public PromotionItemRepository(DiscountappDbContext context) : base(context)
        {
        }

        public IEnumerable<PromotionItem> GetAllByCityId(long cityId)
        {
            IEnumerable<PromotionItem> items;

            if (cityId > 0)
                items = _context.RealEstates
                        .Include(r => r.Address)
                        .Include(r => r.Promotions)
                        .Include(r => r.Promotions.Select(p => p.PromotionItems))
                        .Where(r => r.Address.CityId == cityId)
                        .SelectMany(r => r.Promotions.SelectMany(p => p.PromotionItems))
                        .Distinct();
            else
                items = _context
                        .PromotionItems
                        .Include(e => e.Promotion)
                        .Include(e => e.Promotion.RealEstates);


            return items.ToList();
        }

        public IEnumerable<PromotionItem> GetAllByUser(long userId)
        {
            var list = this._context
                .PromotionItems
                .Include(e => e.Promotion.User)
                .Include(e => e.Promotion)
                .Include(e => e.Promotion.RealEstates)
                .Where(e => e.Promotion.UserId == userId)
                .ToList();

            return list;
        }

        public IEnumerable<PromotionItem> GetAllByUser(long userId, long cityId)
        {
            var list = this._context
                .PromotionItems
                .Include(e => e.Promotion.User)
                .Include(e => e.Promotion)
                .Include(e => e.Promotion.RealEstates)
                .Where(e => e.Promotion.UserId == userId)
                //.Where(e => e.Cities.Any(c => c.Id == cityId))
                .ToList();

            return list;
        }

        public IEnumerable<PromotionItem> GetAllByCategory(long cityId, long? categoryId = null)
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


            return realEstates
                            .SelectMany(r => r.Promotions
                                .SelectMany(p => p.PromotionItems))
                            .Distinct()
                            .ToList();
        }

        public IEnumerable<PromotionItem> GetAllByMerchant(long cityId, long merchantId)
        {
            var realEstates = this._context.RealEstates
                    .Include(r => r.Address)
                    .Include(r => r.Promotions)
                    .Include(r => r.Promotions.Select(p => p.PromotionItems))
                    .Where(r => r.Id == merchantId && r.Address.CityId == cityId);

            return realEstates
                    .SelectMany(r => r.Promotions
                        .SelectMany(p => p.PromotionItems))
                    .Distinct()
                    .ToList();
        }
        
        public IEnumerable<PromotionItem> GetAll(long categoryId, long cityId)
        {
            _context.Configuration.LazyLoadingEnabled = false;
            var list = _context.PromotionItems
                         .Include(p => p.Promotion)
                         //.IncludeFilter(p => p.Cities.Where(c => c.Id == cityId))
                         .Where(p => p.CategoryId == categoryId)
                         .ToList();

            return list;
        }

        public IEnumerable<PromotionItem> GetAllWithPromotions()
        {
            _context.Configuration.LazyLoadingEnabled = false;
            var list = _context.PromotionItems
                .Include(p => p.Promotion)
                .ToList();

            return list;
        }

        public DiscountappDbContext DiscountAppDbContext => this._context as DiscountappDbContext;
    }
}
