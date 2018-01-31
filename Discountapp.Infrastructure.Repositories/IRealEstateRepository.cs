using Discountapp.Domain.Models.Application;
using System.Collections.Generic;

namespace Discountapp.Infrastructure.Repositories
{
    public interface IRealEstateRepository : IRepository<RealEstate>
    {
        IEnumerable<RealEstate> GetSelected(long userId, long[] realStates);
    }
}
