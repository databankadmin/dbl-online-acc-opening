using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppMain.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            MvcApplication.AccountType = 0;
            return View();
        }

        [HttpPost]
        public ActionResult Index(int accountType)
        {
            //document.location.href = "@Url.Action("Initiate", "NewAccount")?acc_type=" + btoa(accType) + "&param=" + btoa(param);
            return RedirectToAction("Initiate", "NewAccount",new { acc_type=AppUtils.Utilities.EncodeBase64(accountType.ToString()), param=AppUtils.Utilities.EncodeBase64(Guid.NewGuid().ToString()) });
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}