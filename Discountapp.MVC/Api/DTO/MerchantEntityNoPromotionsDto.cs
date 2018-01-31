using Discountapp.Domain.Models;
using Discountapp.Domain.Models.Application;
using Discountapp.Domain.Models.Location;

namespace Discountapp.MVC.Api.DTO
{
    public class MerchantEntityNoPromotionsDto
    {
        public MerchantEntityNoPromotionsDto(MerchantEntity m)
        {
            Id = m.Id;
            CompanyId = m.CompanyId;
            MerchantCategoryId = m.MerchantCategoryId;
            Name = m.Name;
            HotLineNumber = m.HotLineNumber;
            WebSiteLink = m.WebSiteLink;
            LogoFolder = m.LogoFolder;
            ModerationPassed = m.ModerationPassed;
            PromotionCount = m.PromotionCount;
            ActiveStatus = m.ActiveStatus;

            AddressId = m.AddressId;
            CityId = m.CityId;
            MapJsonCoord = m.MapJsonCoord;
            Information = m.Information;
            Description = m.Description;
            WorkTime = m.WorkTime;
            WorkTimeSaturday = m.WorkTimeSaturday;
            WorkTimeSunday = m.WorkTimeSunday;
            City = m.City;

            Company = m.Company;
            MerchantType = m.MerchantType;
            MerchantCategory = m.MerchantCategory;
            Address = m.Address;
        }

        public long Id { get; set; }
        public long CompanyId { get; set; }
        public long MerchantCategoryId { get; set; }
        public string Name { get; set; }
        public string HotLineNumber { get; set; }
        public string WebSiteLink { get; set; }
        public string LogoFolder { get; set; }
        public bool ModerationPassed { get; set; }
        public long PromotionCount { get; set; }
        public ActiveStatus ActiveStatus { get; set; }

        public long AddressId { get; set; }
        public long CityId { get; set; }
        public string MapJsonCoord { get; set; }
        public string Information { get; set; }
        public string Description { get; set; }
        public WorkTime WorkTime { get; set; }
        public WorkTime WorkTimeSaturday { get; set; }
        public WorkTime WorkTimeSunday { get; set; }
        public City City { get; set; }

        public Company Company { get; set; }
        public MerchantType MerchantType { get; set; }
        public MerchantCategory MerchantCategory { get; set; }
        public Address Address { get; set; }
    }
}