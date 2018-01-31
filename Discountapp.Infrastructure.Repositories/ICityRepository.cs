using System.Collections.Generic;
using Discountapp.Domain;
using Discountapp.Domain.Models.Location;

namespace Discountapp.Infrastructure.Repositories
{
    public interface ICityRepository : IRepository<City>
    {
        IEnumerable<City> GetDescendedCities();
        IEnumerable<City> GetTopCities();
        IEnumerable<City> GetOrderedCities();
        IEnumerable<City> GetOrderedCitiesForSelectList();
    }
}