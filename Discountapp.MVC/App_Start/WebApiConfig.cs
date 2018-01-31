using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Web.Http;
using System.Web.Http.Dependencies;
using Discountapp.Domain;
using Discountapp.Infrastructure.Repositories;
using Discountapp.MVC.Attributes;
using Microsoft.Practices.Unity;
using Microsoft.Owin.Security.OAuth;
using Unity.WebApi;
using EntityFramework = Discountapp.Infrastructure.Repositories.EntityFramework;
using Memory = Discountapp.Infrastructure.Repositories.Memory;

namespace Discountapp.MVC
{
    using Config = Discountapp.Infrastructure.Constants.Config;
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var container = new UnityContainer();

            switch (Config.ApiRepository)
            {
                case Infrastructure.Constants.RepositoryType.Database:
                    container.RegisterType<IUnitOfWork, EntityFramework.UnitOfWork>(new PerRequestLifetimeManager())
                        .RegisterType<DbContext, DiscountappDbContext>(new PerRequestLifetimeManager())
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
                    container.RegisterType(typeof(DiscountappMemoryContext), new InjectionConstructor())
                        .RegisterType<IUnitOfWork, Memory.UnitOfWork>(new PerRequestLifetimeManager())
                        .RegisterType<IPromotionRepository, Memory.PromotionRepository>(new PerRequestLifetimeManager())
                        .RegisterType<ICityRepository, Memory.CityRepository>(new PerRequestLifetimeManager())
                        .RegisterType<ICategoryRepository, Memory.CategoryRepository>(new PerRequestLifetimeManager())
                        .RegisterType<IPromotionRepository, Memory.PromotionRepository>(new PerRequestLifetimeManager());
                    break;
                default:
                    throw new NotImplementedException("Check repository");
            }

            container.RegisterType(typeof(AuthorizeAttribute));


            config.DependencyResolver = new UnityResolver(container);
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);

            //Areas
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            WebApiUnityActionFilterProvider.RegisterFilterProviders(config);

            // Web API configuration and services
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }

    public class UnityResolver : IDependencyResolver
    {
        protected IUnityContainer container;

        public UnityResolver(IUnityContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }
            this.container = container;
        }

        public object GetService(Type serviceType)
        {
            try
            {
                return container.Resolve(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return container.ResolveAll(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return new List<object>();
            }
        }

        public IDependencyScope BeginScope()
        {
            var child = container.CreateChildContainer();
            return new UnityResolver(child);
        }

        public void Dispose()
        {
            container.Dispose();
        }
    }
}
