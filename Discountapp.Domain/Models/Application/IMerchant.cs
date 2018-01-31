using Discountapp.Domain.Models.Identity;
using System.Collections.Generic;

namespace Discountapp.Domain.Models.Application
{
    public interface IMerchant : IIdentifiable, IActivatable, INameable
    {
        long? MerchantTypeId { get; set; }
        string HotLineNumber { get; set; }
        string WebSiteLink { get; set; }
        string LogoFolder { get; set; }
        long PromotionCount { get; set; }
        bool ModerationPassed { get; set; }
    }
}