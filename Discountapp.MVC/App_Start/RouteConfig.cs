using System.Web.Mvc;
using System.Web.Routing;
using Discountapp.Infrastructure;
using Discountapp.MVC.App_Start;

namespace Discountapp.MVC
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{culture}/{controller}/{action}/{id}",
            //    defaults: new
            //    {
            //        culture = Culture.GetDefaultCulture(),
            //        controller = "Home",
            //        action = "Index",
            //        id = UrlParameter.Optional
            //    }
            //);

            routes.MapRoute(
                name: "Default",
                url: "{city}/{culture}/{controller}/{action}/{id}",
                defaults: new
                {
                    city = RoutMaps.DefaultCity,
                    culture = Culture.GetDefaultCulture(),
                    controller = RoutMaps.DefaultController,
                    action = RoutMaps.DefaultAction,
                    id = UrlParameter.Optional
                }
                , constraints: new
                {
                    city = @"^([a-zA-Z_-]{1,50})$"
                }, 
                namespaces: new string[] { "Discountapp.MVC.Controllers" });
        }
    }
}
