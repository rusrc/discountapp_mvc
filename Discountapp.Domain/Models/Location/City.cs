using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Discountapp.Domain.Models.Application;
using Newtonsoft.Json;

namespace Discountapp.Domain.Models.Location
{
    public class City : MultiLanguage, IIdentifiable
    {
        public City() { }
        public City(string name) : base(name) { }
        public City(NameMultiLanguageJson json) : base(json) { }
        public long Id { get; set; }
        public string Alias { get; set; }
        public string MapJsonCoord { get; set; }
        public ActiveStatus ActiveStatus { get; set; }

        [JsonIgnore]
        public virtual ICollection<Address> Addresses { get; set; }

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

        #endregion
    }
}
