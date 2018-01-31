using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Newtonsoft.Json;

namespace Discountapp.Domain.Models.Application
{
    /// <summary>
    /// Товар или другая услуга в одной акции
    /// </summary>
    public class PromotionItem : ImageEntity, IIdentifiable, INameable, IPromotionItem
    {
        public long CategoryId { get; set; }
        public long PromotionId { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// Begin price of <see cref="PromotionItem"/> promotion item | начальная цена товара
        /// </summary>
        public double BeginPrice { get; set; }
        /// <summary>
        /// Price during active promotion | цена в период акции
        /// </summary>
        public double PromotionalPrice { get; set; }
        /// <summary>
        /// Percentage of discount | процент скидки
        /// </summary>
        public double Discount { get; set; }
        [JsonIgnore]
        public DateTime PublishDate { get; set; }
        [JsonIgnore]
        public DateTime UpdateDate { get; set; }
        public int LikeCount { get; set; }
        public int DislikeCount { get; set; }
        [JsonIgnore]
        public virtual Category Category { get; set; }
        public virtual Promotion Promotion { get; set; }
        [JsonIgnore]
        public virtual ICollection<Like> Likes { get; set; }

        public bool LikeExists(long userId)
        {
            return this.Likes.SingleOrDefault(like => like.UserId == userId) != null;
        }

        public Like GetUserLike(long userId)
        {
            return this.Likes.SingleOrDefault(like => like.UserId == userId);
        }
    }
}
