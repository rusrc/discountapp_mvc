using System;
using System.Collections.Generic;
using Discountapp.Domain;
using Discountapp.Domain.Models.Location;

namespace Discountapp.Infrastructure.Repositories.Memory
{
    public class CityRepository : Repository<City>, ICityRepository
    {
        public CityRepository(DiscountappMemoryContext context) 
            : base(context)
        {
        }

        public IEnumerable<City> GetDescendedCities()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<City> GetTopCities()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<City> GetOrderedCities()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<City> GetOrderedCitiesForSelectList()
        {
            throw new NotImplementedException();
        }
    }
}
