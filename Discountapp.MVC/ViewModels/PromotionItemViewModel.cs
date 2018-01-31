using System.ComponentModel.DataAnnotations;
using Discountapp.Domain.Models.Application;
using System;
using Discountapp.Domain.Models;

namespace Discountapp.MVC.ViewModels
{
    public class PromotionItemViewModel : IPromotionItem, IIdentifiable, INameable
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public double BeginPrice { get; set; }
        [Required]
        public double Discount { get; set; }
        [Required]
        public double PromotionalPrice { get; set; }
        public long CategoryId { get; set; }
        public long PromotionId { get; set; }
        public Category Category { get; set; }
        public Promotion Promotion { get; set; }
        public string ImageFolder { get; set; }
        public string TempFolder { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string FolderWithImagePath { get; set; }
        public int LikeCount { get; set; }
        public int DislikeCount { get; set; }
        public string MerchantLogo { get; set; }


        public static explicit operator PromotionItem(PromotionItemViewModel model)
        {
            return new PromotionItem
            {
                Id = model.Id,
                Name = model.Name,
                BeginPrice = model.BeginPrice,
                Discount = model.Discount,
                PromotionalPrice = model.PromotionalPrice,
                CategoryId = model.CategoryId,
                PromotionId = model.PromotionId,
                Category = model.Category,
                Promotion = model.Promotion,
                ImageFolder = model.ImageFolder,
                TempFolder = model.TempFolder,
                PublishDate = model.PublishDate,
                UpdateDate = model.UpdateDate,
                LikeCount = model.LikeCount,
                DislikeCount = model.DislikeCount
            };
        }

        public static explicit operator PromotionItemViewModel(PromotionItem model)
        {
            return new PromotionItemViewModel
            {
                Id = model.Id,
                Name = model.Name,
                BeginPrice = model.BeginPrice,
                Discount = model.Discount,
                PromotionalPrice = model.PromotionalPrice,
                CategoryId = model.CategoryId,
                PromotionId = model.PromotionId,
                Category = model.Category,
                Promotion = model.Promotion,
                ImageFolder = model.ImageFolder,
                TempFolder = model.TempFolder,
                PublishDate = model.PublishDate,
                UpdateDate = model.UpdateDate,
                FolderWithImagePath = model.FolderWithImagePath,
                LikeCount = model.LikeCount,
                DislikeCount = model.DislikeCount,
                MerchantLogo = model?.Promotion?.MerchantLogoName
            };
        }
    }
}