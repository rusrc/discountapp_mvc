using System.Collections.Generic;
using System.Threading.Tasks;
using Discountapp.Domain.Models;
using Discountapp.Domain.Models.Application;

namespace Discountapp.Infrastructure.Repositories
{
    public interface IAddressRepository : IRepository<Address>
    {
        Task<IEnumerable<Address>> GetAllWithCityAsync();
        Task<IEnumerable<TAddressViewModel>> GetAllWithCityAsync<TAddressViewModel>(long UserId) where TAddressViewModel : IAddress, new();
        ICollection<Address> GetSelected(long userId, IEnumerable<long> selectedList);
        ICollection<Address> GetAllByUser(long userId);
        ICollection<Address> GetAllByPromotion(long cityId, long promotionId, long merchantId);
        void AddOrUpdate(Address address);
    }
}