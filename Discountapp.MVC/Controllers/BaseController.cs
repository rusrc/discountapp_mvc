using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Discountapp.Domain.Models.Identity;
using Discountapp.Infrastructure;
using Discountapp.Infrastructure.Repositories;
using log4net;
using log4net.Config;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Web.Routing;
using Discountapp.Domain.Models.Location;

namespace Discountapp.MVC.Controllers
{
    using AppConfig = Discountapp.Infrastructure.Constants.Config;
    using Const = Discountapp.Infrastructure.Constants.Constant;
    public class BaseController : Controller
    {
        private readonly ApplicationUserManager _userManager;

        public long CityId { get; set; }

        private IUnitOfWork _unitOfWork => DependencyResolver.Current.GetService<IUnitOfWork>();
        public BaseController() { }

        public BaseController(ApplicationUserManager userManager)
        {
            _userManager = userManager;
        }

        public ApplicationUserManager UserManager => _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

        public AppUser AppUser => this.UserManager.FindById(User.Identity.GetUserId<long>());

        /// <summary>
        /// ~/Upload fullUploadFolderPath
        /// </summary>
        public string UploadFolderFullPath
        {
            get
            {
                string uploadPath = Server.MapPath("~/" + AppConfig.UploadFolderName);
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                return uploadPath;
            }
        }

        /// <summary>
        /// ~/App_Date/Upload/temp
        /// </summary>
        public string UploadTempFolderFullPath
        {
            get
            {
                string uploadFolder = Server.MapPath("~/" + AppConfig.UploadFolderName);
                string uploadPath = Path.Combine(uploadFolder, "temp");
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                return uploadPath;
            }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                this.SetCityIdFromUrl();

                if (CityAliasFromUrl.Equals(Const.URL_REGION_RESET))
                {
                    filterContext.Result
                        = GetRedirectToRouteResult(Const.ALL_CITIES);
                }

                if (CityId == 0 && GetCityIdFromCookie() > 0 && !CityAliasFromUrl.Equals(Const.URL_REGION_RESET))
                {
                    filterContext.Result
                        = GetRedirectToRouteResult(GetCityById(GetCityIdFromCookie())?.Alias);
                }

                //Set region cookies
                Response.Cookies.Add(new HttpCookie(Const.COOKIE_NAME_CURRENTCITYID, CityId.ToString()));

                // Set culture globally 
                _SetInitialCurrentCultureFromUrl();
            }
            catch (Exception)
            {

                throw;
            }

        }

        #region Regions set

        protected RedirectToRouteResult GetRedirectToRouteResult(string cityUrlSection)
        {
            return
                new RedirectToRouteResult("Default",
                    new RouteValueDictionary(
                        new
                        {
                            city = cityUrlSection,
                            culture = Culture.GetDefaultCulture()
                        }));
        }
        /// <summary>
        /// Set global region variants with value from URL
        /// </summary>
        /// <returns>bool</returns>
        protected void SetCityIdFromUrl()
        {
            this.CityId = GetCityByAlias(CityAliasFromUrl)?.Id ?? 0;
        }

        /// <summary>
        /// Set global region variants with value from Cookies
        /// </summary>
        protected void SetCityIdFromCookie()
        {
            CityId = GetCityIdFromCookie();
        }

        /// <summary>
        /// Get city id based on cookie
        /// </summary>
        /// <returns></returns>
        public long GetCityIdFromCookie()
        {
            #region set cookes of city and region
            HttpCookie cookieCity = Request.Cookies.Get(Const.COOKIE_NAME_CURRENTCITYID);
            cookieCity = (cookieCity == null || cookieCity.Value == "0" || cookieCity.Value == "") ? null : cookieCity;
            #endregion

            return long.Parse(cookieCity?.Value ?? "0");
        }

        /// <summary>
        /// Get alies from url section "city"
        /// </summary>
        public string CityAliasFromUrl => $"{ RouteData.Values?.SingleOrDefault(e => e.Key == "city").Value ?? string.Empty}";

        /// <summary>
        /// Get city by alias
        /// </summary>
        /// <param name="cityAlias">alias usually from url</param>
        /// <returns></returns>
        public City GetCityByAlias(string cityAlias) => new Lazy<City>(() => _unitOfWork.Cities.Get(c => c.Alias == cityAlias)).Value;

        /// <summary>
        /// City by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public City GetCityById(long id) => new Lazy<City>(() => _unitOfWork.Cities.Get(id)).Value;

        #endregion

        #region Log class

        /// <summary>
        /// Log file from <see cref="log4net"/> 
        /// </summary>
        /// <example>
        /// <code>
        /// public class HomeController{
        ///     ActionResult Index(){
        ///         Log.For(this).Info("New test");    
        ///     }
        /// }
        /// </code>
        /// </example>
        protected class Log
        {
            static Log()
            {
                XmlConfigurator.Configure();
            }

            public static ILog For(object loggedObject)
            {
                if (loggedObject != null)
                    return For(loggedObject.GetType());
                else
                    return For(null);
            }

            public static ILog For(Type objectType)
            {
                if (objectType != null)
                    return LogManager.GetLogger(objectType.Name);
                else
                    return LogManager.GetLogger(string.Empty);
            }
        }

        #endregion

        #region Exceptions

        protected override void OnException(ExceptionContext filterContext)
        {
            ViewDataDictionary viewData = filterContext.Controller.ViewData;
            HttpBrowserCapabilitiesBase browser = filterContext.HttpContext.Request.Browser;

            #region Metadate collection
            var userId = User.Identity.IsAuthenticated
                // ReSharper disable once PossiblyMistakenUseOfParamsMethod
                ? string.Concat(User.Identity.GetUserId<long>().ToString())
                : "UnAuthenticated";

            var httpHeaders = new System.Text.StringBuilder();
            //var headers = filterContext.HttpContext.Request.Headers;
            //foreach (var item in headers)
            //{
            //    httpHeaders.AppendFormat("->{0}:{1}\r\n", item, headers.Get(item.ToString()));
            //}
            httpHeaders
                .AppendFormat("Browser version: '{0}'\r\n", browser.Version)
                .AppendFormat("IsMobileDevice: '{0}'\r\n", browser.IsMobileDevice)
                .AppendFormat("Browser MinorVersion: '{0}'\r\n", browser.MinorVersion)
                .AppendFormat("Browser MajorVersion: '{0}'\r\n", browser.MajorVersion)
                .AppendFormat("Browser EcmaScriptVersion: '{0}'\r\n", browser.EcmaScriptVersion)
                .AppendFormat("Browser Browser: '{0}'\r\n", browser.Browser)
                .AppendFormat("Browser is Beta: '{0}'\r\n", browser.Beta);

            var metaData = $"Fired time: {DateTime.Now}, UserID:{userId},\r\nHeaders:\r\n{httpHeaders}";
            #endregion

            try
            {
                throw filterContext.Exception;
            }
            catch (NullReferenceException ex)
            {
                #region Log Exception
                var msg = $"{ex.Message}\r\n{metaData}";
                Log.For(this).Fatal(msg, ex);
                #endregion
                filterContext.Result = GetErrorViewResult(ex, viewData);
            }
            catch (Exception ex)
            {
                #region Log Exception
                var msg = $"{ex.Message}\r\n{metaData}";
                Log.For(this).Fatal(msg, ex);
                #endregion
                filterContext.Result = GetErrorViewResult(ex, viewData);
            }

#if DEBUG
            filterContext.ExceptionHandled = false;
#else
            filterContext.ExceptionHandled = true;
#endif
            base.OnException(filterContext);
        }

        /// <summary>
        /// Get ViewResult
        /// </summary>
        /// <param name="exception">current exception</param>
        /// <param name="viewData">filterContext.Controller.ViewData from <see cref="ExceptionContext"/></param>
        /// <param name="viewName">View by default ExceptionForUserView.cshtml</param>
        /// <returns></returns>
        private ViewResult GetErrorViewResult
            (
                Exception exception,
                ViewDataDictionary viewData,
                string viewName = @"~/Views/Shared/Exceptions/ExceptionForUserView.cshtml"
            )
        {
            return new ViewResult
            {
                ViewName = viewName,
                ViewData = new ViewDataDictionary(viewData)
                {
                    Model = exception
                }
            };
        }


        #endregion

        #region culture helpers

        /// <summary>
        /// The current value that getting from Request on BeginExecuteCore
        /// </summary>
        private string _cultureName = null;

        private void _SetInitialCurrentCultureFromCookie()
        {
            HttpCookie cultureCookie = Request.Cookies[Const.COOKIE_NAME_CULTURE];

            if (cultureCookie != null)
                _cultureName = cultureCookie.Value;
            else
                _cultureName =
                    (Request.UserLanguages != null && Request.UserLanguages.Length > 0)
                    ? Request.UserLanguages[0]
                    : null;

            _cultureName = Culture.GetImplementedCulture(_cultureName);

            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(_cultureName);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }

        private void _SetInitialCurrentCultureFromUrl()
        {
            _cultureName = RouteData.Values[Const.COOKIE_NAME_CULTURE] as string;

            if (this._cultureName == null)
                _cultureName = (Request.UserLanguages != null && Request.UserLanguages.Length > 0)
                    ? Request.UserLanguages[0]
                    : null;

            _cultureName = Culture.GetImplementedCulture(_cultureName);

            //Set culture in cookies too
            Response.Cookies.Add(new HttpCookie(Const.COOKIE_NAME_CULTURE, _cultureName));

            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(_cultureName);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

        }

        #endregion
    }
}