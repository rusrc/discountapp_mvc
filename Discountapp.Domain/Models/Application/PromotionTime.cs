using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Discountapp.Domain.Models.Application
{
    using Langx = Discountapp.Infrastructure.Resources.Localization.Lang;
    public enum PromotionTimeType
    {
        /// <summary>
        /// Current and future promotions
        /// </summary>
        CurrentAndFuture,
        /// <summary>
        /// Promotion for yestoday
        /// </summary>
        Yestoday,
        /// <summary>
        /// Promotion for today
        /// </summary>
        Today,
        /// <summary>
        /// Promotion for tomorrow
        /// </summary>
        Tomorrow
    }
    public class PromotionTime
    {
        public PromotionTime(PromotionTimeType type)
        {
            this.Type = type;
            switch (type)
            {
                case PromotionTimeType.Yestoday:
                    this.Name = this.PromotionForYestoday;
                    break;
                case PromotionTimeType.Today:
                    this.Name = this.PromotionForToday;
                    break;
                case PromotionTimeType.Tomorrow:
                    this.Name = this.PromotionForTomorrow;
                    break;
                default:
                    this.Name = "Текущие и будущие акции";
                    break;
            }
        }
        public string PromotionForYestoday => $"{Langx.PromotionsForYestoday} {DateTime.Now.AddDays(-1):dd MMMM}";
        public string PromotionForToday => $"{Langx.PromotionsForToday} {DateTime.Now:dd MMMM}";
        public string PromotionForTomorrow => $"{Langx.PromotionsForTomorrow} {DateTime.Now.AddDays(1):dd MMMM}";
        public PromotionTimeType Type { get; set; }
        public string Name { get; set; }
        public static ICollection<PromotionTime> GenerateCollection()
        {
            return new Collection<PromotionTime>
            {   
                null,
                new PromotionTime(PromotionTimeType.CurrentAndFuture),
                new PromotionTime(PromotionTimeType.Yestoday),
                new PromotionTime(PromotionTimeType.Today),
                new PromotionTime(PromotionTimeType.Tomorrow)
            }; ;
        }
    }
}
