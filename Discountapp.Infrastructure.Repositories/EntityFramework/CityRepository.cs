using System.Collections.Generic;
using System.Linq;
using Discountapp.Domain;
using Discountapp.Domain.Models.Location;

namespace Discountapp.Infrastructure.Repositories.EntityFramework
{
    using Langx = Discountapp.Infrastructure.Resources.Localization.Lang;
    public class CityRepository : Repository<City>, ICityRepository
    {
        public CityRepository(DiscountappDbContext context) : base(context) { }

        public IEnumerable<City> GetTopCities()
        {
            return this.DiscountAppDbContext.Cities.ToList();
        }

        public IEnumerable<City> GetDescendedCities()
        {
            return GetAll().OrderByDescending(city => city.Name);
        }


        public IEnumerable<City> GetOrderedCities()
        {
            return this.GetAll().OrderBy(city => city.Name);
        }

        public IEnumerable<City> GetOrderedCitiesForSelectList()
        {
            return this.GetAll().OrderBy(city => city.Name);
        }

        public DiscountappDbContext DiscountAppDbContext => this._context as DiscountappDbContext;
    }
}
