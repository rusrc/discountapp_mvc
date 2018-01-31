using Discountapp.Domain;
using System;
using System.Threading.Tasks;

namespace Discountapp.Infrastructure.Repositories.Memory
{
    using Memory = Discountapp.Infrastructure.Repositories.Memory;
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DiscountappMemoryContext _context;

        public UnitOfWork(DiscountappMemoryContext context)
        {
            _context = context;
        }
        //public IMerchantRepository Merchants
        //{
        //    get { throw new NotImplementedException(); }
        //}

        public ICityRepository Cities => new Memory.CityRepository(_context);

        public IAddressRepository Addresses
        {
            get { throw new NotImplementedException(); }
        }

        public IMerchantTypeRepository MerchantTypes
        {
            get { throw new NotImplementedException(); }
        }

        public ICategoryRepository Categories => new Memory.CategoryRepository(_context);

        public IPromotionRepository Promotions => new Memory.PromotionRepository(_context);

        public IPromotionItemRepository PromotionItems => new Memory.PromotionItemRepository(_context);

        public IMobileUserRepository MobileUsers
        {
            get { throw new NotImplementedException(); }
        }

        public IMerchantCategoryRepository MerchantCategory
        {
            get { throw new NotImplementedException(); }
        }

        public IMerchantEntityRepository MerchantEntities
        {
            get { throw new NotImplementedException(); }
        }

        public IRealEstateRepository RealEstates
        {
            get { throw new NotImplementedException(); }
        }

        public ICompanyRepository Companies
        {
            get { throw new NotImplementedException(); }
        }

        public int Complete()
        {
            throw new NotImplementedException();
        }

        public Task<int> CompleteAsync()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}
