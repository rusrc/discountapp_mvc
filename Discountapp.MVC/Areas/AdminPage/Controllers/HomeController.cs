using Discountapp.Domain.Models;
using Discountapp.MVC.Attributes;
using System.Web.Mvc;

namespace Discountapp.MVC.Areas.AdminPage.Controllers
{
    [Authorization(RoleType = RoleType.Manager | RoleType.Admin)]
    public class HomeController : Controller
    {
        // GET: AdminPage/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}