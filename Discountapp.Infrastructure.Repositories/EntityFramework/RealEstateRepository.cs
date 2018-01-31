using Discountapp.Domain.Models.Application;
using Discountapp.Domain;
using System.Collections.Generic;
using System.Linq;

namespace Discountapp.Infrastructure.Repositories.EntityFramework
{
    public class RealEstateRepository : Repository<RealEstate>, IRealEstateRepository
    {
        public RealEstateRepository(DiscountappDbContext context) 
            : base(context)
        {
        }

        public IEnumerable<RealEstate> GetSelected(long userId, long[] realStates)
        {
            return _context.RealEstates.Where(r => realStates.Contains(r.Id)).ToList();
        }

        public DiscountappDbContext DiscountAppDbContext => this._context as DiscountappDbContext;
    }
}
