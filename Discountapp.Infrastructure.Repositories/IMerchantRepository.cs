using System.Collections.Generic;
using System.Threading.Tasks;
using Discountapp.Domain.Models;
using Discountapp.Domain.Models.Application;

namespace Discountapp.Infrastructure.Repositories
{
    public interface IMerchantRepository : IRepository<Merchant>
    {
        Task<List<Merchant>> GetByIdsAsync(IEnumerable<long> Ids);
        Task<List<Merchant>> GetAllByUserAsync(long UserId);
        Task<IEnumerable<TMerchantViewModel>> GetAllByUserAsync<TMerchantViewModel>(long UserId) where TMerchantViewModel : IMerchant, IIdentifiable, new();
        Task<List<Merchant>> GetSelected(long userId, IEnumerable<long> selectedList);
        Task<List<Merchant>> GetByCityAsync(long? cityId);
        IEnumerable<Merchant> GetAll(long categoryId, long cityId);
    }
}
