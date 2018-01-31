using System;
using System.Data.Entity;
using System.Web;
using System.Web.Http;
using Discountapp.Domain;
using Discountapp.Domain.Models.Identity;
using Discountapp.Infrastructure.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Practices.Unity;

//Discountapp.MVC.App_Start
namespace Discountapp.MVC
{
    using EntityFramework = Infrastructure.Repositories.EntityFramework;
    using Memory = Discountapp.Infrastructure.Repositories.Memory;
    using Config = Discountapp.Infrastructure.Constants.Config;
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig
    {
        #region Unity Container
        private static readonly Lazy<IUnityContainer> Container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return Container.Value;
        }
        #endregion

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your types here
            // container.RegisterType<IProductRepository, ProductRepository>();
            container
                .RegisterType<IUnitOfWork, EntityFramework.UnitOfWork>(new PerRequestLifetimeManager())
                .RegisterType<DbContext, DiscountappDbContext>(new PerRequestLifetimeManager());

            container.RegisterType<IUserStore<AppUser, long>,
                   UserStore<AppUser, AppRole, long, AppUserLogin, AppUserRole, AppUserClaim>>(
                   new PerRequestLifetimeManager())
               .RegisterType<IAuthenticationManager>(
                   new InjectionFactory(c => HttpContext.Current.GetOwinContext().Authentication));

            //container.RegisterType<AccountController>(new PerRequestLifetimeManager());
            //Repositories
            switch (Config.MvcRepository)
            {
                case Infrastructure.Constants.RepositoryType.Database:
                    container
                    //.RegisterType<IMerchantRepository, EntityFramework.MerchantRepository>(new PerRequestLifetimeManager())
                    .RegisterType<ICityRepository, EntityFramework.CityRepository>(new PerRequestLifetimeManager())
                    .RegisterType<IAddressRepository, EntityFramework.AddressRepository>(new PerRequestLifetimeManager())
                    .RegisterType<IMerchantTypeRepository, EntityFramework.MerchantTypeRepository>(new PerRequestLifetimeManager())
                    .RegisterType<ICategoryRepository, EntityFramework.CategoryRepository>(new PerRequestLifetimeManager())
                    .RegisterType<IPromotionRepository, EntityFramework.PromotionRepository>(new PerRequestLifetimeManager())
                    .RegisterType<IPromotionItemRepository, EntityFramework.PromotionItemRepository>(new PerRequestLifetimeManager())
                    .RegisterType<IMobileUserRepository, EntityFramework.MobileUserRepository>(new PerRequestLifetimeManager())
                    .RegisterType<IMerchantCategoryRepository, EntityFramework.MerchantCategoryRepository>(new PerRequestLifetimeManager())
                    .RegisterType<IMerchantEntityRepository, EntityFramework.MerchantEntityRepository>(new PerRequestLifetimeManager())
                    .RegisterType<IRealEstateRepository, EntityFramework.RealEstateRepository>(new PerRequestLifetimeManager());
                    break;
                case Infrastructure.Constants.RepositoryType.Memory:
                    container.RegisterType(typeof(DiscountappMemoryContext))
                        .RegisterType<IUnitOfWork, Memory.UnitOfWork>(new PerRequestLifetimeManager())
                        .RegisterType<IPromotionRepository, Memory.PromotionRepository>(new PerRequestLifetimeManager())
                        .RegisterType<ICityRepository, Memory.CityRepository>(new PerRequestLifetimeManager());
                    break;
            }

       
        }
    }

    public class EntityModule : UnityContainerExtension
    {
        protected override void Initialize()
        {
            throw new NotImplementedException();
        }
    }
}
