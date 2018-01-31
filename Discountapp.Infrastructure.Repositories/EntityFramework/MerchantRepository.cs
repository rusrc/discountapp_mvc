using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Discountapp.Domain;
using Discountapp.Domain.Models;
using Discountapp.Domain.Models.Application;
using Z.EntityFramework.Plus;

namespace Discountapp.Infrastructure.Repositories.EntityFramework
{
    public class MerchantRepository : Repository<Merchant>, IMerchantRepository
    {
        public MerchantRepository(DiscountappDbContext context)
            : base(context)
        {
        }

        public IEnumerable<Merchant> GetAll(long categoryId, long cityId)
        {
            var listResult = _context.Merchants.SqlQuery($@"select m.* from Merchant m
                                        inner join AddressMerchant am on m.Id = am.MerchantId
                                        inner join Address a on am.AddressId = a.id
                                        where a.CityId in ({cityId});");
            


            return listResult;

        }

        public Task<List<Merchant>> GetAllByUserAsync(long userId)
        {
            throw new NotImplementedException();
            //return this.DiscountAppDbContext.Merchants.Where(b => b.UserId == userId).ToListAsync();
        }

        public Task<List<Merchant>> GetByIdsAsync(IEnumerable<long> ids)
        {
            return this.DiscountAppDbContext.Merchants.Where(b => ids.Contains(b.Id)).ToListAsync();
        }

        public async Task<IEnumerable<TMerchantViewModel>> GetAllByUserAsync<TMerchantViewModel>(long userId)
            where TMerchantViewModel : IMerchant, IIdentifiable, new()
        {
            return (await this.DiscountAppDbContext.Merchants
                    .Include(e => e.MerchantType)
                    //.Include(e => e.Promotions)
                    .ToListAsync())
                .Select(model => new TMerchantViewModel
                {
                    Id = model.Id,
                    Name = model.Name,
                    //UserId = model.UserId,
                    //User = model.User,
                    HotLineNumber = model.HotLineNumber,
                    WebSiteLink = model.WebSiteLink,
                    LogoFolder = model.LogoFolder,
                    MerchantTypeId = model.MerchantTypeId,
                    MerchantType = model.MerchantType,
                    LogoName = model.LogoName,
                    //Promotions = model.Promotions,
                    //PromotionCount = model.Promotions.Count,
                    ActiveStatus = model.ActiveStatus
                })
                .ToList();
        }

        public Task<List<Merchant>> GetSelected(long userId, IEnumerable<long> selectedList)
        {
            throw new NotImplementedException();
            //var selectedArray = selectedList.ToArray();
            //var resultList = _context.Merchants
            //    .Include(e => e.Addresses)
            //    .Where(m => m.UserId == userId && selectedArray.Contains(m.Id));

            //return resultList.ToListAsync();
        }

        public Task<List<Merchant>> GetByCityAsync(long? cityId)
        {
            if (cityId == null || cityId == 0)
                return GetAllAsync();

            return _context.Merchants
                .Include(e => e.Addresses)
                .Where(e => e.Addresses.Any(a => a.Address.CityId == cityId))
                .ToListAsync();
        }

        public DiscountappDbContext DiscountAppDbContext => this._context as DiscountappDbContext;

    }
}