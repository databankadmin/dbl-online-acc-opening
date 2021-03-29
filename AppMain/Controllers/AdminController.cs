using AppMain.Providers;
using AppUtils;
using DBHelper.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppMain.Controllers
{
    [Authorize]
    [OutputCache(NoStore = true, Duration = 0)]

    public class AdminController : Controller
    {
        public AppUser CurrentUser
        {

            get
            {

                return Utilities.GetSessionUser() as AppUser;
            }
        }
        // GET: Admin
        public ActionResult Applications(int accountType = 0, int investmentTypeId = 0, string dates = null)
        {
            var model = Utilities.GetApplications();
            return View(model);
        }

        public ActionResult ApplicantProfile(string _refNumber)
        {
            Guid applicationId = Guid.Parse(_refNumber.Decrypt());
            ViewBag.applicationId = applicationId;
            var model = Utilities.GetApplications(0, 0, null, applicationId.ToString()).FirstOrDefault();
            return View(model);
        }
    }
}