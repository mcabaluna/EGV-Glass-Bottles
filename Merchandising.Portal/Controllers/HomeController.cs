using Merchandising.Portal.Models;
using Merchandising.VM.Portal;
using Merchandising.VM.Results;
using System.Web.Mvc;

namespace Merchandising.Portal.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            return RedirectToAction("Login", "Users");
        }
        [Route("Home/Dashboard")]
        public ActionResult Home_Index()
        {
            var obj = MerchandisingApiWrapper.Get<Dashboard_Results>(
                             "Dashboard" + "/getdashboard");
            var user = User.Identity.Name;
            return View(obj);
        }
        public ActionResult CheckStatus(string transtype, int docentry)
        {
            var obj = MerchandisingApiWrapper.Get<CheckStatusVM>(
                         "Dashboard" + "/checkstatus" + $"?transtype={transtype}&docentry={docentry}");
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
    }
}