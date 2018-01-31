using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discountapp.Domain.Models.Application
{
    public interface IWorkTime
    {
        TimeSpan? Begin { get; set; }
        TimeSpan? End { get; set; }
    }
}
