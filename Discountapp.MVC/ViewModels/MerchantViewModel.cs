using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Discountapp.Domain.Models.Application;
using Discountapp.Domain.Models.Identity;
using Discountapp.Domain.Models;

namespace Discountapp.MVC.ViewModels
{
    using Lang = App_LocalResources.Lang;
    public class MerchantViewModel : BaseViewModel, IMerchant, IIdentifiable
    {
        public MerchantViewModel() { }

        public MerchantViewModel(string tempFolderName)
        {
            this.TempFolderName = tempFolderName;
        }

        public long Id { get; set; }
        public long UserId { get; set; }
        public AppUser User { get; set; }

        [Required]
        [Display(Name = nameof(Lang.DomainName), ResourceType = typeof(Lang))]
        public string Name { get; set; }

        [Display(Name = nameof(Lang.WebSiteLink), ResourceType = typeof(Lang))]
        [RegularExpression(@"^http(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$", ErrorMessage = "Неверный формат")]
        public string WebSiteLink { get; set; }

        [Required]
        [Display(Name = nameof(Lang.MerchantTypeId), ResourceType = typeof(Lang))]
        public long? MerchantTypeId { get; set; }
        [Required]
        [Display(Name = nameof(Lang.MerchantCategoryId), ResourceType = typeof(Lang))]
        public long MerchantCategoryId { get; set; }

        [Required]
        [Display(Name = nameof(Lang.HotLineNumber), ResourceType = typeof(Lang))]
        [RegularExpression(@"^\d{1,12}$", ErrorMessageResourceName = nameof(Lang.HotLineNumberErrorMessage), ErrorMessageResourceType = typeof(Lang))]
        public string HotLineNumber { get; set; }
        public IEnumerable<long> AddressesSelected { get; set; } 
        public ICollection<Promotion> Promotions { get; set; }
        [Display(Name = nameof(Lang.PromotionCount), ResourceType = typeof(Lang))]
        public long PromotionCount { get; set; }

        public bool ModerationPassed { get; set; }

        [Display(Name = nameof(Lang.MerchantType), ResourceType = typeof(Lang))]
        public MerchantType MerchantType { get; set; }
        public string LogoFolder { get; set; }
        [Display(Name = nameof(Lang.LogoName), ResourceType = typeof(Lang))]
        public string LogoName { get; set; }
        public string TempFolderName { get; set; }
        [Display(Name = nameof(Lang.ActiveStatus), ResourceType = typeof(Lang))]
        public ActiveStatus ActiveStatus { get; set; }
        public bool OfferComfirmed { get; set; } = false;
    }
}