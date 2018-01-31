using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Discountapp.Domain.Models.Identity;
using Discountapp.Infrastructure.Captcha;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Discountapp.MVC.ViewModels;

namespace Discountapp.MVC.Controllers
{
    using AppUser = Discountapp.Domain.Models.Identity.AppUser;
    //[Authorize]
    public class AccountController : BaseController
    {
        [AllowAnonymous]
        public ActionResult Index() => RedirectToAction(nameof(Login));

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if(ModelState.IsValid)
            {
                var user = await UserManager.FindByEmailAsync(model.Email);
                if(user == null)
                {
                    ModelState.AddModelError("LoginCustomErrors", "AccountLoginCantFoundUserByEmail");
                }

                if(user != null && !UserManager.CheckPassword(user, model.Password))
                {
                    ModelState.AddModelError("LoginCustomErrors", "AccountLoginYouTypeWrongPassword");
                }

                if(ModelState.IsValid)
                {
                    //if (user != null && !user.EmailConfirmed)
                    //{
                    //    var msg = string.Format("указанный почтовый ящик не подтвержден, зайдите на почту и подтвердите email {0}", (object)user.Email);
                    //    ModelState.AddModelError("LoginCustomErrors", msg);
                    //    return View();
                    //}

                    if(user != null)
                    {
                        await SignInAsync(user, model.RememberMe);

                        if(Url.IsLocalUrl(returnUrl))
                            return Redirect(returnUrl);

                        if(!Url.IsLocalUrl(returnUrl))
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        return Redirect(returnUrl);
                    }
                }
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            HttpContext.GetOwinContext().Authentication.SignOut();
            return RedirectToAction("index", "Home");
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            #region g-recaptcha-response
            //model.g_recaptcha_response = Request.Form.Get("g-recaptcha-response");

            //if(!string.IsNullOrEmpty(model.g_recaptcha_response))
            //{
            //    var grecaptcha = new Grecaptcha().Result(model.g_recaptcha_response);

            //    if(grecaptcha.IsError)
            //        ModelState.AddModelError("LoginCustomErrors", grecaptcha.ErrorMsg);
            //}
            //else
            //{
            //    ModelState.AddModelError("LoginCustomErrors", "CaptchaIsEmpty");
            //}
            #endregion

            if(!ModelState.IsValid) return View(model);

            IdentityResult result = null;
            AppUser user = UserManager.FindByEmail(model.Email);

            if(user != null && !UserManager.HasPassword(user.Id))
            {
                //Регистрируем ананима

                result = await UserManager.AddPasswordAsync(user.Id, model.Password);
            }
            else
            {
                //Регистрируем нового пользователя
                user = new AppUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                };
                result = await UserManager.CreateAsync(user, model.Password);

            }

            if(result.Succeeded)
            {

                //xthrow new NotImplementedException("Error!!!");
                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code }, protocol: Request.Url.Scheme);
                await UserManager.SendEmailAsync(user.Id, _getSubjectAccount(), _getBodyAccount(callbackUrl));

                //scope.Complete();
                return View("DisplayEmail");
            }

            foreach(var err in result.Errors)
                ModelState.AddModelError("", err);


            //Если не валидна отобразить данные в форме еще раз
            return View(model);
        }

        [HttpGet, AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(long userId, string code)
        {
            if(userId == 0 || code == null)
            {
                return View("UserErrorView");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        #region Forgot / Reset password

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if(user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Не показывать, что пользователь не существует или не подтвержден
                    return View("ForgotPasswordConfirmation");
                }

                // Дополнительные сведения о том, как включить подтверждение учетной записи и сброс пароля, см. по адресу: http://go.microsoft.com/fwlink/?LinkID=320771
                // Отправка сообщения электронной почты с этой ссылкой
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code }, protocol: Request.Url.Scheme);
                await UserManager.SendEmailAsync(user.Id, "Сброс пароля", "Сбросьте ваш пароль, щелкнув <a href=\"" + callbackUrl + "\">здесь</a>");
                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // Появление этого сообщения означает наличие ошибки; повторное отображение формы
            return View(model);
        }

        [HttpGet, AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("UserErrorView") : View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await UserManager.FindByNameAsync(model.Email);
            if(user == null)
            {
                // Не показывать, что пользователь не существует
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }

            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if(result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }

            foreach(var err in result.Errors)
                ModelState.AddModelError("", err);

            return View();
        }

        [HttpGet, AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet, AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Производит асинхроную аутентификацию (SignIn) через метод SignIn класса Identity.UserManager
        /// </summary>
        /// <param name="user">объект IdentityUser</param>
        /// <param name="isPersistent">Постоянный клиента или нет</param>
        /// <returns></returns>
        private async Task SignInAsync(AppUser user, bool isPersistent)
        {
            IAuthenticationManager authManager = HttpContext.GetOwinContext().Authentication;
            AuthenticationProperties authProps = new AuthenticationProperties
            {
                IsPersistent = isPersistent
            };

            authManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            authManager.SignIn(
                    properties: authProps,
                   identities: await user.CreateUserIdentityAsync(UserManager));
        }

        private string _getSubjectAccount(string domain = "empty")
        {
            //var curSubj = string.Format(Langx.ConformationAccount, domain);

            return "curSub"; // curSubj;
        }

        private string _getBodyAccount(string callBack)
        {
            //var curBody = string.Format(Langx.ConformationBody, callBack, this.SplitDomain.Domain/*Ссылка для перехода*/);

            return "curBody"; //curBody;
        }

        #endregion
    }
}
