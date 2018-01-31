using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;

namespace Discountapp.Domain.Models.Identity
{
    public class ApplicationUserManager : UserManager<AppUser, long>
    {
        public ApplicationUserManager(IUserStore<AppUser, long> store)
            : base(store)
        {
            this.Create(store);
        }

        public ApplicationUserManager Create(IUserStore<AppUser, long> store)
        {
            //var manager = new ApplicationUserManager(
            //    new UserStore<User, Role, long, UserLogin, UserRole, UserClaim>(context.Get<DiscountappDbContext>()));
            //this.Store = new UserStore<AppUser, AppRole, long, AppUserLogin, AppUserRole, AppUserClaim>(null);

            this.UserValidator = new UserValidator<AppUser, long>(this)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            this.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = true,
                RequireLowercase = false,
                RequireUppercase = false
            };
            //Двухфакторная аутентификация
            this.RegisterTwoFactorProvider(
                "PhoneCode",
                new PhoneNumberTokenProvider<AppUser, long>
                {
                    MessageFormat = "Ваш код безопасности {0}"
                });
            this.RegisterTwoFactorProvider(
                "EmailCode",
                new EmailTokenProvider<AppUser, long>
                {
                    Subject = "Код безопасности",
                    BodyFormat = "Ваш код безопасности {0}"
                });
            this.EmailService = new EmailService();
            this.SmsService = new SmsService();

            var provider = new DpapiDataProtectionProvider("Sample");
            this.UserTokenProvider = new DataProtectorTokenProvider<AppUser, long>(provider.Create("EmailConfirmation"));// as IUserTokenProvider<AppUser, long>;

            return this;
        }
    }
}
