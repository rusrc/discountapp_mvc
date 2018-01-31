using System.ComponentModel.DataAnnotations;
using Discountapp.Domain.Models;
using Discountapp.Domain.Models.Application;
using Discountapp.Domain.Models.Location;

namespace Discountapp.MVC.ViewModels
{
    using Langx = App_LocalResources.Lang;
    public class MerchantEntityViewModel: IMerchantEntity
    {
        public long Id { get; set; }
        public bool OfferComfirmed { get; set; }
        [Display(Name = "Статус")]
        public ActiveStatus ActiveStatus { get; set; }
        [Display(Name = "Название компании")]
        public string CompanyName { get; set; }

        [Display(Name = nameof(CompanyId), ResourceType = typeof(Langx))]
        public long CompanyId { get; set; }
        [Display(Name = "Лого")]
        public string LogoName { get; set; }
        public string LogoFolder { get; set; }

        [Display(Name = nameof(MerchantCategoryId), ResourceType = typeof(Langx))]
        public long MerchantCategoryId { get; set; }
        public long? MerchantTypeId { get; set; }
        public bool ModerationPassed { get; set; }
        public long UserId { get; set; }
        public long AddressId { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Langx))]
        [Display(Name = nameof(CityId), ResourceType = typeof(Langx))]
        public long CityId { get; set; }

        [Display(Name = nameof(MerchantEntityViewModel) + nameof(Description), ResourceType = typeof(Langx))]
        public string Description { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Langx))]
        [Display(Name = nameof(MerchantEntityViewModel) + nameof(Information), ResourceType = typeof(Langx))]
        public string Information { get; set; }
        public string MapJsonCoord { get; set; }

        [Display(Name = nameof(WorkTime), ResourceType = typeof(Langx))]
        public WorkTime WorkTime { get; set; }

        [Display(Name = nameof(WorkTimeSaturday), ResourceType = typeof(Langx))]
        public WorkTime WorkTimeSaturday { get; set; }

        [Display(Name = nameof(WorkTimeSunday), ResourceType = typeof(Langx))]
        public WorkTime WorkTimeSunday { get; set; }
        public City City { get; set; }
        [Display(Name = "Количество акций")]
        public long PromotionCount { get; set; }
        [Display(Name = "Адрес")]
        public string FullAddress { get; set; }


        public static implicit operator MerchantEntity(MerchantEntityViewModel m)
        {
            return new MerchantEntity
            {
                Id = m.Id,
                CompanyId = m.CompanyId,
                UserId = m.UserId,
                MerchantCategoryId = m.MerchantCategoryId,
                LogoFolder = m.LogoFolder,
                ModerationPassed = m.ModerationPassed,
                ActiveStatus = m.ActiveStatus,
                AddressId = m.AddressId,
                CityId = m.CityId,
                MapJsonCoord = m.MapJsonCoord,
                Information = m.Information,
                Description = m.Description,
                WorkTime = m.WorkTime,
                WorkTimeSaturday = m.WorkTimeSaturday,
                WorkTimeSunday = m.WorkTimeSunday,
                PromotionCount = m.PromotionCount
            };
        }

        public static implicit operator MerchantEntityViewModel(MerchantEntity m)
        {
            return new MerchantEntityViewModel
            {
                Id = m.Id,
                CompanyId = m.CompanyId,
                UserId = m.UserId,
                MerchantCategoryId = m.MerchantCategoryId,
                CompanyName = m.Company.Name,
                LogoName = m.Company.LogoName,
                LogoFolder = m.LogoFolder,
                ModerationPassed = m.ModerationPassed,
                ActiveStatus = m.ActiveStatus,
                AddressId = m.AddressId,
                CityId = m.CityId,
                MapJsonCoord = m.MapJsonCoord,
                Information = m.Information,
                Description = m.Description,
                WorkTime = m.WorkTime,
                WorkTimeSaturday = m.WorkTimeSaturday,
                WorkTimeSunday = m.WorkTimeSunday,
                PromotionCount = m.PromotionCount,
                FullAddress = string.Format("г.{0}, {1}", m?.City?.NameMultiLangJsonObject.Value, m?.Address?.Information)
            };
        }
    }
}