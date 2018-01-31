using System;

namespace Discountapp.MVC.ViewModels
{
    public class PromotionListViewModel
    {
        public int Page { get; set; }
        public long CityId { get; set; }
        public DateTime PromotionDate { get; set; }
    }
}