using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discountapp.Domain.Models.Application;

namespace Discountapp.Infrastructure.Repositories
{
    public interface IMobileUserRepository : IRepository<MobileUser>
    {
        MobileUser GetByDeviceImei(string deviceImei);
    }
}
