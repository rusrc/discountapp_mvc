using System.Collections.Generic;
using Discountapp.Domain.Models.Identity;
using Newtonsoft.Json;

namespace Discountapp.Domain.Models.Application
{
    public class RealEstate : BaseEntity, IRealEstate
    {
        public long CompanyId { get; set; }
        public long? MerchantTypeId { get; set; }
        public long UserId { get; set; }
        public long MerchantCategoryId { get; set; }
        public string LogoFolder { get; set; }
        public bool ModerationPassed { get; set; }
        public ActiveStatus ActiveStatus { get; set; }

        [JsonIgnore]
        public virtual AppUser User { get; set; }
        [JsonIgnore]
        public virtual MerchantType MerchantType { get; set; }
        public virtual MerchantCategory MerchantCategory { get; set; }
        public virtual Company Company { get; set; }
        public virtual Address Address { get; set; }
        public virtual ICollection<Promotion> Promotions { get; set; }
    }
}
