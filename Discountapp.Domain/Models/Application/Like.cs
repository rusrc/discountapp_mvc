using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Discountapp.Domain.Models.Identity;

namespace Discountapp.Domain.Models.Application
{
    public class Like
    {
        public Like() { }

        public Like(long userId, long itemId, LikeType value)
        {
            UserId = userId;
            PromotionItemId = itemId;
            Value = value;
        }

        [Key, Column(Order = 1)]
        public long UserId { get; set; }

        [Key, Column(Order = 2)]
        public long PromotionItemId { get; set; }

        public LikeType Value { get; set; }

        public virtual  AppUser User { get; set; }
        public virtual PromotionItem PromotionItem { get; set; }
    }
}
