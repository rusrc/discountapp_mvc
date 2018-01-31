namespace Discountapp.Domain.Models.Application
{
    public interface IPromotionItem : ICatigoriable, IPromotional
    {
        double BeginPrice { get; set; }
        double Discount { get; set; }
        double PromotionalPrice { get; set; }
        string ImageFolder { get; set; }
        string TempFolder { get; set; }
    }
}