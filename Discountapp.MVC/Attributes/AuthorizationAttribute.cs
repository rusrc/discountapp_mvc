using Discountapp.Domain.Models;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Discountapp.MVC.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class AuthorizationAttribute : System.Web.Mvc.AuthorizeAttribute
    {
        public RoleType RoleType { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException(nameof(httpContext));

            System.Security.Principal.IPrincipal user = httpContext.User;
            if (!user.Identity.IsAuthenticated)
                return false;

            var roleList = new Regex("\\s+").Replace($"{RoleType}", string.Empty).Split(',');

            return roleList.Any() && roleList.Any(user.IsInRole);
        }
    }
}