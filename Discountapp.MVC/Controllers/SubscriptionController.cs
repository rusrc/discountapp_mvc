using System.Web.Mvc;

namespace Discountapp.MVC.Controllers
{
    public class SubscriptionController : BaseController
    {
        // GET: Subscription
        public ActionResult Index()
        {
            return View();
        }
    }
}