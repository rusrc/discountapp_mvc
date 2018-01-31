using System.Collections.Generic;
using Discountapp.Domain.Models.Application;

namespace Discountapp.MVC.Api.DTO
{
    public class PromotionItemNoChildrenDto
    {
        public PromotionItemNoChildrenDto(PromotionItem m)
        {
            m.Promotion.PromotionItems = new List<PromotionItem>();

            Id = m.Id;
            CategoryId = m.CategoryId;
            PromotionId = m.PromotionId;
            Name = m.Name;
            BeginPrice = m.BeginPrice;
            PromotionalPrice = m.PromotionalPrice;
            Discount = m.Discount;
            LikeCount = m.LikeCount;
            DislikeCount = m.DislikeCount;
            FolderWithImagePathSimple = m.FolderWithImagePathSimple;
            FolderWithImagePath = m.FolderWithImagePath;
            DefaultImage = m.DefaultImage;
            DefaultImageFullPath = m.DefaultImageFullPath;
            ImageWidth = m.ImageWidth;
            ImageHeight = m.ImageHeight;
            Promotion = m.Promotion;
        }

        public long CategoryId { get; set; }
        public long PromotionId { get; set; }
        public string Name { get; set; }
        public double BeginPrice { get; set; }
        public double PromotionalPrice { get; set; }
        public double Discount { get; set; }
        public int LikeCount { get; set; }
        public int DislikeCount { get; set; }
        public string FolderWithImagePathSimple { get; set; }
        public string FolderWithImagePath { get; set; }
        public string DefaultImage { get; set; }
        public string DefaultImageFullPath { get; set; }
        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }
        public long Id { get; set; }
        public Promotion Promotion { get; set; }
    }
}