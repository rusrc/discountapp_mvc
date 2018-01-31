using System.ComponentModel;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Discountapp.MVC.ViewModels;

namespace Discountapp.MVC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected void Application_Start()
        {
#if DEBUG
            //this.StartApp = DateTime.Now;
             Log.Info("\r\n--App Start--\r\n");
            //HibernatingRhinos.Profiler.Appender.EntityFramework.EntityFrameworkProfiler.Initialize();
#endif

            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            GlobalConfiguration.Configuration.Formatters.Remove(GlobalConfiguration.Configuration.Formatters.XmlFormatter);

            //WebApiConfig.Register(GlobalConfiguration.Configuration);
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //ModelBinders.Binders.Add(typeof(AddressViewModel), new CustomModelBinder());
            //BundleConfig.RegisterBundles(BundleTable.Bundles);
            

            //https://code.msdn.microsoft.com/How-to-use-Apache-log4net-0d969339
            log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(Server.MapPath("~/Web.config")));
        }
    }
}
