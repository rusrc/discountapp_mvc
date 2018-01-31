namespace Discountapp.Domain.Models.Application
{
    public interface IRealEstate : IIdentifiable
    {
        ActiveStatus ActiveStatus { get; set; }
        long CompanyId { get; set; }
        string LogoFolder { get; set; }
        long MerchantCategoryId { get; set; }
        long? MerchantTypeId { get; set; }
        bool ModerationPassed { get; set; }
        long UserId { get; set; }
    }
}