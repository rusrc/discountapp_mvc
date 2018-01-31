using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Discountapp.Domain.Models.Location;
using Newtonsoft.Json;

namespace Discountapp.Domain.Models.Application
{
    public class Address : IAddress
    {
        public Address(){}
        public Address(IWorkTime workday, IWorkTime satuday, IWorkTime sunday)
        {
            this.WorkTime = (WorkTime)workday;
            this.WorkTimeSaturday = (WorkTime)satuday;
            this.WorkTimeSunday = (WorkTime)sunday;
        }

        [ForeignKey(nameof(RealEstate))]
        public long AddressId { get; set; }
        public long CityId { get; set; }
        public virtual string MapJsonCoord { get; set; }
        public virtual string Information { get; set; }
        public virtual string Description { get; set; }
        

        public virtual WorkTime WorkTime { get; set; }
        public virtual WorkTime WorkTimeSaturday { get; set; }
        public virtual WorkTime WorkTimeSunday { get; set; }
        public virtual City City { get; set; }

        public virtual RealEstate RealEstate { get; set; }

        #region Not mapped
        /// <summary>
        ///     Список координат для карты |
        ///     Coordination list
        /// </summary>
        [NotMapped]
        public IEnumerable<MapJsonCoord> MapJsonCoordCollection =>
            !string.IsNullOrEmpty(MapJsonCoord)
            ? JsonConvert.DeserializeObject<List<MapJsonCoord>>(MapJsonCoord)
            : null;

        [NotMapped]
        public string ShortInformation {
            get
            {
                if(this.City == null || this.RealEstate == null || this.RealEstate?.Company == null)
                    throw new Exception($"Кажется не испозуется метод Include. Проверить при запросе. Метод {nameof(ShortInformation)}");

                return $"{this.RealEstate.Company.Name}, {this.City.NameMultiLangJsonObject.Value}, {this.Information}";
            }
        }
        #endregion
    }
}
