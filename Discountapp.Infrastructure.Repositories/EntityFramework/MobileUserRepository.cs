using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discountapp.Domain;
using Discountapp.Domain.Models.Application;

namespace Discountapp.Infrastructure.Repositories.EntityFramework
{
    public class MobileUserRepository : Repository<MobileUser>, IMobileUserRepository
    {
        public MobileUserRepository(DiscountappDbContext context) 
            : base(context)
        {
        }

        public MobileUser GetByDeviceImei(string deviceImei)
        {
            return _context.MobileUser.SingleOrDefault(e => e.DeviceImei == deviceImei);
        }

        public DiscountappDbContext DiscountAppDbContext => this._context as DiscountappDbContext;
    }
}
