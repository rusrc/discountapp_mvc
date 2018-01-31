using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

//http://stackoverflow.com/questions/35271881/customizing-system-web-http-authorizeattribute-within-asp-net-web-api-applicatio
namespace Discountapp.MVC.Attributes
{
    public class ApiAuthorizationAttribute : System.Web.Http.AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            return true;
            if (System.Web.HttpContext.Current.Session["Role"] == null) return false;
            string role = (string)System.Web.HttpContext.Current.Session["Role"];

            if (role == "Admin" || role == "Super Admin") return true;

            if (role == "Collaborator") return true;

            return false;
        }
    }
}