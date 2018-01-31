using System.Collections.Generic;
using Discountapp.Domain.Models.Identity;
using Discountapp.Domain.Models.Location;

namespace Discountapp.Domain.Models.Application
{
    public class MerchantEntity : IMerchant, IMerchantEntity
    {
        #region RealEstate property
        public MerchantEntity() { }
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public long? MerchantTypeId { get; set; }
        public long UserId { get; set; }
        public long MerchantCategoryId { get; set; }
        public string Name { get; set; }
        public string HotLineNumber { get; set; }
        public string WebSiteLink { get; set; }
        public string LogoFolder { get; set; }
        public bool ModerationPassed { get; set; }

        public long PromotionCount
        {
            get
            {
                try
                {
                    if (this.Promotions == null)
                        return 0;
                    return this.Promotions.Count;
                }
                catch (System.Exception)
                {
                    return 0;
                }
            }
            set {}
        }

        public ActiveStatus ActiveStatus { get; set; }
        #endregion

        #region Address property

        public long AddressId { get; set; }
        public long CityId { get; set; }
        public virtual string MapJsonCoord { get; set; }
        public virtual string Information { get; set; }
        public virtual string Description { get; set; }
        public virtual WorkTime WorkTime { get; set; }
        public virtual WorkTime WorkTimeSaturday { get; set; }
        public virtual WorkTime WorkTimeSunday { get; set; }
        public virtual City City { get; set; }
        #endregion

        public virtual Company Company { get; set; }
        public virtual AppUser User { get; set; }
        public virtual MerchantType MerchantType { get; set; }
        public virtual MerchantCategory MerchantCategory { get; set; }
        public virtual Address Address { get; set; }
        public virtual ICollection<Promotion> Promotions { get; set; }

        public IRealEstate ToRealEstate()
        {
            var realEstate = new RealEstate
            {
                CompanyId = this.CompanyId,
                MerchantCategoryId = this.MerchantCategoryId,
                ActiveStatus = this.ActiveStatus,
                LogoFolder = this.LogoFolder,
                MerchantTypeId = this.MerchantTypeId,
                UserId = this.UserId,
            };

            return realEstate;
        }

        public IAddress ToAddress()
        {
            var address = new Address
            {
                CityId = this.CityId,
                Description = this.Description,
                Information = this.Information,
                MapJsonCoord = this.MapJsonCoord,
                WorkTime = this.WorkTime,
                WorkTimeSaturday = this.WorkTimeSaturday,
                WorkTimeSunday = this.WorkTimeSunday
            };

            return address;
        }
    }
}
