namespace Discountapp.Domain.Models.Application
{
    public interface ICompany
    {
        string HotLineNumber { get; set; }
        string LogoFolder { get; set; }
        string WebSiteLink { get; set; }
        string Description { get; set; }
    }
}