using System.ComponentModel.DataAnnotations.Schema;

namespace Discountapp.Domain.Models
{
    [NotMapped]
    public class MapJsonCoord
    {
        public MapJsonCoord() { }

        public MapJsonCoord(string serviceName, double longitude, double latitude, double altitude)
        {
            ServiceName = serviceName;
            Longitude = longitude;
            Latitude = latitude;
            Altitude = altitude;
        }
        /// <summary>
        ///     Google, yandex, 2gis
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        ///     Долгота
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        ///     Широта
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        ///     Высота
        /// </summary>
        public double Altitude { get; set; }
    }
}
