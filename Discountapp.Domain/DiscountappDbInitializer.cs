using System.Data.Entity;
using System.Linq;
using Discountapp.Domain.Models.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Discountapp.Domain.Models;

namespace Discountapp.Domain
{
    public class DiscountappDbInitializer : DropCreateDatabaseAlways<DiscountappDbContext>
    {
        protected override void Seed(DiscountappDbContext context)
        {
            this.SeedDatabaseContext(context);
            base.Seed(context);
        }

        public void SeedDatabaseContext(DiscountappDbContext context)
        {
            var data = new DiscountappMemoryContext(false);

            #region Add default admin user

            const string name = "admin@mail.ru";
            const string password = "";
            string adminRoleName = typeof(RoleType).GetEnumNames().SingleOrDefault(a => a == "Admin");

            if (adminRoleName == null)
                throw new System.Exception("No admin role, please add 'Admin' role");
            //BRoleManager roleManager = HttpContext.Current.GetOwinContext().Get<BRoleManager>();
            var roleStore = new RoleStore<AppRole, long, AppUserRole>(context);
            var roleManager = new RoleManager<AppRole, long>(roleStore);

            string[] roles = typeof(RoleType).GetEnumNames();
            foreach (var roleName in roles)
            {
                var role = roleManager.FindByName(roleName);
                if (role == null)
                {
                    role = new AppRole { Name = roleName };
                    roleManager.Create(role);
                }
            }

            var userStore = new UserStore<AppUser, AppRole, long, AppUserLogin, AppUserRole, AppUserClaim>(context);
            var userManager = new UserManager<AppUser, long>(userStore);

            var user = userManager.FindByName(name);
            if (user == null)
            {
                user = new AppUser
                {
                    UserName = name,
                    Email = name,
                };

                userManager.Create(user, password);
                userManager.SetLockoutEnabled(user.Id, false);
            }

            //Добавить пользователя к роли если еще не добавлено
            var rolesForUser = userManager.GetRoles(user.Id);
            if (!rolesForUser.Contains(adminRoleName))
            {
                userManager.AddToRole(user.Id, adminRoleName);
            }

            //Other users
            var user1 = userManager.FindByName("9031184523");
            if (user1 == null)
            {
                user1 = new AppUser
                {
                    UserName = "9031184523",
                    Email = "test1@mail.ru",
                };

                userManager.Create(user1, password);
                userManager.SetLockoutEnabled(user.Id, false);
            }
            #endregion

            data.MerchantTypes.ToList().ForEach(e => context.MerchantTypes.Add(e));
            context.SaveChanges();

            data.Cities.ToList().ForEach(e => context.Cities.Add(e));
            context.SaveChanges();

            data.Categories.ToList().ForEach(c => context.Categories.Add(c));
            context.SaveChanges();

            data.MerchantCategories.ToList().ForEach(c => context.MerchantCategories.Add(c));
            context.SaveChanges();
        }
    }
}
