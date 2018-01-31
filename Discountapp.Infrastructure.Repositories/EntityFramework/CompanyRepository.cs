using Discountapp.Domain;
using Discountapp.Domain.Models.Application;

namespace Discountapp.Infrastructure.Repositories.EntityFramework
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        public CompanyRepository(DiscountappDbContext context) : 
            base(context)
        {
        }
    }
}
