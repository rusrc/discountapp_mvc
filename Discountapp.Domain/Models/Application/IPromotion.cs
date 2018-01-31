using System;
using System.Collections.Generic;

namespace Discountapp.Domain.Models.Application
{
    public interface IPromotion
    {
        DateTime Begin { get; set; }
        DateTime End { get; set; }
        bool SubscriptionNotifierIsActive { get; set; }
    }
}