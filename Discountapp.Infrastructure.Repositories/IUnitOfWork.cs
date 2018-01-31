using System;
using System.Threading.Tasks;

namespace Discountapp.Infrastructure.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        //IMerchantRepository Merchants { get; }
        ICityRepository Cities { get; }
        IAddressRepository Addresses { get; }
        IMerchantTypeRepository MerchantTypes { get; }
        ICategoryRepository Categories { get; }
        IPromotionRepository Promotions { get; }
        IPromotionItemRepository PromotionItems { get; }
        IMobileUserRepository MobileUsers { get; }
        IMerchantCategoryRepository MerchantCategory { get; }
        IMerchantEntityRepository MerchantEntities { get; }
        IRealEstateRepository RealEstates { get; }
        ICompanyRepository Companies { get; }
        int Complete();
        Task<int> CompleteAsync();
    }
}
