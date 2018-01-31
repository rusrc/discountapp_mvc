using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Discountapp.Domain.Models.Application
{
    public class Category : MultiLanguage, ICategory
    {
        public Category()
        {
            this.PromotionItems = new HashSet<PromotionItem>();
        }
        public long Id { get; set; }

        /// <summary>
        ///     id of parent subcatigory, if null then the hightest parent
        /// </summary>
        public long? ParentId { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(ParentId))]
        public virtual Category Parent { get; set; }

        [JsonIgnore]
        public virtual ICollection<PromotionItem> PromotionItems { get; set; }
    }
}
