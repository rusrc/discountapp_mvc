using System;
using System.Threading.Tasks;
using Discountapp.Domain;

namespace Discountapp.Infrastructure.Repositories.EntityFramework
{
    public class UnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// DbContext should to be DiscountAppDbContext
        /// </summary>
        private readonly DiscountappDbContext _context;
        public UnitOfWork(DiscountappDbContext context)
        {
            _context = context;
        }
        //public IMerchantRepository Merchants
        //    => new Lazy<MerchantRepository>(() => new MerchantRepository(_context)).Value;

        public ICityRepository Cities
            => new Lazy<CityRepository>(() => new CityRepository(_context)).Value;

        public IAddressRepository Addresses =>
            new Lazy<AddressRepository>(() => new AddressRepository(_context)).Value;

        public IMerchantTypeRepository MerchantTypes
            => new Lazy<MerchantTypeRepository>(() => new MerchantTypeRepository(_context)).Value;

        public ICategoryRepository Categories
            => new Lazy<CategoryRepository>(() => new CategoryRepository(_context)).Value;

        public IPromotionRepository Promotions
            => new Lazy<PromotionRepository>(() => new PromotionRepository(_context)).Value;

        public IPromotionItemRepository PromotionItems
            => new Lazy<PromotionItemRepository>(() => new PromotionItemRepository(_context)).Value;

        public IMobileUserRepository MobileUsers
            => new Lazy<MobileUserRepository>(() => new MobileUserRepository(_context)).Value;

        public IMerchantCategoryRepository MerchantCategory 
            => new Lazy<MerchantCategoryRepository>(() => new MerchantCategoryRepository(_context)).Value;

        public IMerchantEntityRepository MerchantEntities
            => new Lazy<MerchantEntityRepository>(() => new MerchantEntityRepository(_context)).Value;

        public IRealEstateRepository RealEstates
            => new Lazy<RealEstateRepository>(() => new RealEstateRepository(_context)).Value;

        public ICompanyRepository Companies
            => new Lazy<CompanyRepository>(() => new CompanyRepository(_context)).Value;

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        private bool _disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
                if (disposing)
                    _context.Dispose();

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
