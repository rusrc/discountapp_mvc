using Discountapp.Domain.Models.Location;
using System.Collections.Generic;

namespace Discountapp.Domain.Models.Application
{
    public interface IAddress
    {
        long AddressId { get; set; }
        long CityId { get; set; }
        string Description { get; set; }
        string Information { get; set; }
        string MapJsonCoord { get; set; }


        WorkTime WorkTime { get; set; }
        WorkTime WorkTimeSaturday { get; set; }
        WorkTime WorkTimeSunday { get; set; }
        City City { get; set; }
    }
}