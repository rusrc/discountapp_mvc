using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Discountapp.Domain.Models;
using Discountapp.Domain.Models.Application;
using Discountapp.Domain.Models.Identity;

#pragma warning disable 1591
namespace Discountapp.MVC.ViewModels
{
    public class PromotionViewModel : BaseViewModel, IPromotion, IIdentifiable, INameable
    {
        public long Id { get; set; }
        public long  UserId { get; set; }
        [Required]
        [Display(Name = "Укажите название акции")]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Начало акции")]
        public DateTime Begin { get; set; }
        [Required]
        [Display(Name = "Конец акции")]
        [DataType(DataType.Date)]
        public DateTime End { get; set; }
        public bool SubscriptionNotifierIsActive { get; set; }
        public AppUser User { get; set; }
        public ICollection<PromotionItemViewModel> PromotionItems { get; set; }
        [Display(Name = "Где будут участвовать акции")]
        public IEnumerable<long> MerchantsSelected { get; set; }
        [Display(Name = "В каких магазинах будут акции")]
        public ICollection<Address> Addresses { get; set; }
        public long[] RealEstateSelected { get; set; }
        [Required]
        public bool Agreement { get; set; } = false;
        
        public static explicit operator Promotion(PromotionViewModel model)
        {
            return new Promotion
            {
                Id = model.Id,
                UserId = model.UserId,
                User = model.User,
                Name = model.Name,
                Begin = model.Begin,
                End = model.End,
                SubscriptionNotifierIsActive = model.SubscriptionNotifierIsActive,
                PromotionItems = model.PromotionItems.Select(e => (PromotionItem)e).ToList(),
            };
        }

        public static explicit operator PromotionViewModel(Promotion model)
        {
            return new PromotionViewModel
            {
                Id = model.Id,
                UserId = model.UserId,
                User = model.User,
                Name = model.Name,
                Begin = model.Begin,
                End = model.End,
                SubscriptionNotifierIsActive = model.SubscriptionNotifierIsActive,
                PromotionItems = model.PromotionItems.Select(e => (PromotionItemViewModel)e).ToList(),
            };
        }
    }
}