using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Discountapp.Domain.Models.Identity;
using System.Linq;

namespace Discountapp.Domain.Models.Application
{
    /// <summary>
    /// Акция
    /// </summary>
    public class Promotion : MultiLanguage, IIdentifiable, INameable, IPromotion
    {
        // [ApiExplorerSettings(IgnoreApi = true)]
        public long Id { get; set; }

        [JsonIgnore]
        public long UserId { get; set; }
        /// <summary>
        /// Начало акции
        /// </summary>
        [JsonIgnore]
        public DateTime Begin { get; set; }
        /// <summary>
        /// Конец акции
        /// </summary>
        [JsonIgnore]
        public DateTime End { get; set; }
        /// <summary>
        /// Если активен отправлять уведомление пользователю
        /// </summary>
        public bool SubscriptionNotifierIsActive { get; set; }
        [JsonIgnore]
        public AppUser User { get; set; }
        [JsonIgnore]
        public virtual ICollection<RealEstate> RealEstates { get; set; }
        public virtual ICollection<PromotionItem> PromotionItems { get; set; }

        [NotMapped]
        public long PromotionItemsCount => PromotionItems.Count;
        /// <summary>
        /// Начало акции
        /// </summary>
        [NotMapped]
        [JsonProperty(PropertyName = "Begin")]
        public string BeginShort => Begin.ToString("dd.MM.yyyy");
        /// <summary>
        /// Конец акции
        /// </summary>
        [NotMapped]
        [JsonProperty(PropertyName = "End")]
        public string EndShort => End.ToString("dd.MM.yyyy");
        //TODO upgrade query could be lazyloding
        [NotMapped]
        public string MerchantLogoName => RealEstates?.FirstOrDefault()?.Company.LogoName ?? "http://lorempixel.com/55/55/nature";
    }
}
