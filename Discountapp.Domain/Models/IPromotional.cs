using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discountapp.Domain.Models.Application;

namespace Discountapp.Domain.Models
{
    public interface IPromotional
    {
        long PromotionId { get; set; }
        Promotion Promotion { get; set; }
    }
}
