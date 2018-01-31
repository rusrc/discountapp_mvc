using System.ComponentModel.DataAnnotations;
using Discountapp.Domain.Models;
using Discountapp.Domain.Models.Application;
using Discountapp.MVC.App_LocalResources;

namespace Discountapp.MVC.ViewModels
{
    public class CompanyViewModel: IIdentifiable, INameable, ICompany
    {
        public CompanyViewModel() { }

        public CompanyViewModel(string tempFolderName)
        {
            this.TempFolderName = tempFolderName;
        }
        public long Id { get; set; }
        public long UserId { get; set; }
        [Required]
        [Display(Name = nameof(Lang.DomainName), ResourceType = typeof(Lang))]
        public string Name { get; set; }
        [Required]
        [Display(Name = nameof(Lang.HotLineNumber), ResourceType = typeof(Lang))]
        [RegularExpression(@"^\d{1,12}$", ErrorMessageResourceName = nameof(Lang.HotLineNumberErrorMessage), ErrorMessageResourceType = typeof(Lang))]
        public string HotLineNumber { get; set; }
        public string LogoFolder { get; set; }
        [Display(Name = nameof(Lang.WebSiteLink), ResourceType = typeof(Lang))]
        [RegularExpression(@"^http(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$", ErrorMessage = "Неверный формат")]
        public string WebSiteLink { get; set; }
        [Display(Name = nameof(Lang.Description), ResourceType = typeof(Lang))]
        public string Description { get; set; }
        public string TempFolderName { get; set; }
        public bool OfferComfirmed { get; set; }

        public static implicit operator Company(CompanyViewModel model)
        {
            return new Company
            {
                Id = model.Id,
                UserId = model.UserId,
                Name = model.Name,
                Description = model.Description,
                HotLineNumber = model.HotLineNumber,
                WebSiteLink = model.WebSiteLink
            };
        }

        public static implicit operator CompanyViewModel(Company model)
        {
            return new CompanyViewModel
            {
                Id = model.Id,
                UserId = model.UserId,
                Name = model.Name,
                Description = model.Description,
                HotLineNumber = model.HotLineNumber,
                WebSiteLink = model.WebSiteLink
            };
        }
    }
}