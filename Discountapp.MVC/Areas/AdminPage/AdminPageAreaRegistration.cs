using System.Web.Mvc;

namespace Discountapp.MVC.Areas.AdminPage
{
    public class AdminPageAreaRegistration : AreaRegistration 
    {
        public override string AreaName => "AdminPage";
      

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "AdminPage_default",
                "Admin/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                //namespaces: new string[] { "Discountapp.MVC.Areas.AdminPage.Controllers" }
            );
        }
    }
}