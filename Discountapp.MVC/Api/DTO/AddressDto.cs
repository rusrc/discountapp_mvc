using System.Collections.Generic;
using Discountapp.Domain.Models;
using Discountapp.Domain.Models.Application;
using Discountapp.Domain.Models.Location;

namespace Discountapp.MVC.Api.DTO
{
    public class AddressDto
    {
        public AddressDto(Address m)
        {
            AddressId = m.AddressId;
            CityId = m.CityId;
            MapJsonCoord = m.MapJsonCoord;
            Address = m.Information;
            FullAddress = m.ShortInformation;
            Description = m.Description;
            WorkTime = m.WorkTime;
            WorkTimeSaturday = m.WorkTimeSaturday;
            WorkTimeSunday = m.WorkTimeSunday;
            City = m.City;
            MapJsonCoordCollection = m.MapJsonCoordCollection;
        }

        public long AddressId { get; set; }
        public long CityId { get; set; }
        public string MapJsonCoord { get; set; }
        public string Address { get; set; }
        public string FullAddress { get; set; }
        public string Description { get; set; }


        public WorkTime WorkTime { get; set; }
        public WorkTime WorkTimeSaturday { get; set; }
        public WorkTime WorkTimeSunday { get; set; }
        public City City { get; set; }
        private IEnumerable<MapJsonCoord> MapJsonCoordCollection { get; set; }
    }
}