using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace Discountapp.Domain.Models.Identity
{
    public class RoleManager : RoleManager<AppRole, long>
    {
        public RoleManager(IRoleStore<AppRole, long> roleStore) //BRole
            : base(roleStore)
        {
        }

        public static RoleManager Create(IdentityFactoryOptions<RoleManager> options, IOwinContext context)
        {
            return new RoleManager(new RoleStore<AppRole, long, AppUserRole>(context.Get<DiscountappDbContext>())); //BRole
        }
    }
}
