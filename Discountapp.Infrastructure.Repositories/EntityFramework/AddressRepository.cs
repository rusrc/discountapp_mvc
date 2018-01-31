using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Discountapp.Domain;
using Discountapp.Domain.Models.Application;
using System.Data.Entity.Migrations;

namespace Discountapp.Infrastructure.Repositories.EntityFramework
{
    public class AddressRepository : Repository<Address>, IAddressRepository
    {
        public AddressRepository(DiscountappDbContext context) : base(context)
        {
        }

        public ICollection<Address> GetAllByPromotion(long cityId, long promotionId, long merchantId)
        {
            var list = _context.RealEstates
                                .Include(rs => rs.Address)
                                .Include(rs => rs.Promotions)
                                .Include(rs => rs.Company)
                                //Выбераем все акции которые есть у этого продавца в этом городе.
                                .Where(rs => rs.Id == merchantId)
                                .Where(rs => rs.Address.CityId == cityId)
                                .SelectMany(rs => rs.Promotions)
                                //-----------------------------
                                //Выбыраем только одну ацкию
                                .Where(p => p.Id == promotionId)
                                //Выбираем все адреса по этой акции
                                .SelectMany(p => p.RealEstates.Select(rs => rs.Address))
                                .ToList();

            return list;
        }

        public async Task<IEnumerable<Address>> GetAllWithCityAsync()
        {
            return await this.DiscountAppDbContext.Addresses.Include(a => a.City).ToListAsync();
        }

        public async Task<IEnumerable<TAddressViewModel>> GetAllWithCityAsync<TAddressViewModel>(long UserId)
            where TAddressViewModel : IAddress, new()
        {
            return (await this.DiscountAppDbContext.Addresses.Include(a => a.City).ToListAsync())
                .Select(addressViewModel => new TAddressViewModel
                {
                    AddressId = addressViewModel.AddressId,
                    Information = addressViewModel.Information,
                    Description = addressViewModel.Description,
                    //MerchantId = addressViewModel.MerchantId,
                    MapJsonCoord = addressViewModel.MapJsonCoord,
                    CityId = addressViewModel.CityId,
                    City = addressViewModel.City,
                    WorkTime = addressViewModel.WorkTime,
                    WorkTimeSaturday = addressViewModel.WorkTimeSaturday,
                    WorkTimeSunday = addressViewModel.WorkTimeSunday,
                })
                .ToList();
        }

        public ICollection<Address> GetAllByUser(long userId)
        {
            //var realEstates = _context.RealEstates.Where(r => r.UserId == userId).Select(r => r.Id).ToArray();
            //var result = _context.Addresses.Where(a => realEstates.Contains(a.AddressId)).ToList();

            var addresses = _context.Addresses
                            .Include(a => a.RealEstate)
                            .Include(a => a.City)
                            .Include(a => a.RealEstate.Company)
                            .Where(a => a.RealEstate.UserId == userId);

            return addresses.ToList();
        }

        public ICollection<Address> GetSelected(long userId, IEnumerable<long> selectedList)
        {
            var realEstates = _context
                .RealEstates
                .Where(re => re.UserId == userId && selectedList.Contains(re.Id))
                .Select(re => re.Id)
                .ToArray();

            var addresses = _context.Addresses.Where(a => realEstates.Contains(a.AddressId)).ToList();

            return addresses;
        }


        public void AddOrUpdate(Address address)
        {
            this._context.Addresses.AddOrUpdate(address);
        }
        public DiscountappDbContext DiscountAppDbContext => this._context as DiscountappDbContext;
    }
}
