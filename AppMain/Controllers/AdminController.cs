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
        public ActionResult Applications(int accountType = 0, int investmentTypeId = 0, int statusId=0,string key=null, string from = null, string to=null)
        {


            string  dates = DateTime.Now.AddDays(-90).ToString("MM/dd/yyyy") + "-" + DateTime.Now.ToString("MM/dd/yyyy");
            if (!string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to))
            {
                dates = from + "-" + to;
            }

            var model = Utilities.GetApplications(accountType,investmentTypeId,dates,null,statusId,key);
            ViewBag.accountType = accountType;
            ViewBag.investmentTypeId = investmentTypeId;
            ViewBag.from = string.IsNullOrEmpty(from) ? DateTime.Now.AddDays(-90).ToShortDateString() : from;
            ViewBag.to = string.IsNullOrEmpty(to) ? DateTime.Now.ToShortDateString() : to;
            ViewBag.statusId = statusId;
            ViewBag.key = key;
            return View(model);
        }
        public ActionResult DownloadFile(string path, string _refNumber)
        {
            ViewBag.path = path;
            ViewBag._refNumber = _refNumber;
            return View();
        }


        public ActionResult ApplicantProfile(string _refNumber, string message = null)
        {
            if (!string.IsNullOrEmpty(message))
            {
                ViewBag.Message = message.Decrypt();
            }
            Guid applicationId = Guid.Parse(_refNumber.Decrypt());
            ViewBag.applicationId = applicationId;
            var model = Utilities.GetApplications(0, 0, null, applicationId.ToString()).FirstOrDefault();
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult CancelOrRejectApplication(Guid applicationId,string action, string comments, string notifyApplicant)
        {
            using (var context=new DBLAccountOpeningContext())
            {
                var application = context.Accounts.Find(applicationId);
                application.CancelOrRejectComment = comments;
                application.StatusId = action == "R" ? 3 : 4;
                application.CancelOrRejectDate = DateTime.Now;
                application.CancelOrRejectBy = CurrentUser.Id;

                context.SaveChanges();
                if (!string.IsNullOrEmpty(notifyApplicant) && string.Equals(notifyApplicant, "on", StringComparison.CurrentCultureIgnoreCase))
                {
                    //send message
                }
                return RedirectToAction("ApplicantProfile", new { _refNumber = application.Id.ToString().Encrypt(), message = ("action completed successfully").Encrypt() });

            }

        }


        public ActionResult ReviewSuccessfully(Guid _ref)
        {
            using (var context=new DBLAccountOpeningContext())
            {
                var application = context.Accounts.Find(_ref);
                application.SuccesfulReviewBy = CurrentUser.Id;
                application.SuccessfulReviewDate = DateTime.Now;
                application.StatusId = 2;
                context.SaveChanges();
                return RedirectToAction("ApplicantProfile", new { _refNumber = application.Id.ToString().Encrypt(), message = ("application successfully reviewed").Encrypt() });

            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult ManualCardValidation(Guid applicationId, string recordId, string objectType, string comments)
        {
            using (var context = new DBLAccountOpeningContext())
            {
                var application = context.Accounts.Find(applicationId);

                if (objectType == "AccMember")
                {
                    var model = context.AccountMembers.FirstOrDefault(x => x.Id.ToString() == recordId);
                    if (model != null && !model.IdValidated)
                    {
                        model.IdValidated = true;
                        model.IdValidationDate = DateTime.Now;
                        model.IdValidationBy = (Utilities.GetSessionUser() as AppUser).Email;
                        model.IdValidationMode = "MANUAL";
                        model.ManualValidationComment = comments;

                    }
                }
                else if (objectType == "AuthPerson")
                {
                    var model = context.AccountAuthorisedPersons.FirstOrDefault(x => x.Id.ToString() == recordId);
                    if (model != null && !model.IdValidated)
                    {
                        model.IdValidated = true;
                        model.IdValidationDate = DateTime.Now;
                        model.IdValidationBy = (Utilities.GetSessionUser() as AppUser).Email;
                        model.IdValidationMode = "MANUAL";
                        model.ManualValidationComment = comments;
                    }
                }
                context.SaveChanges();
                return RedirectToAction("ApplicantProfile", new { _refNumber = application.Id.ToString().Encrypt(), message=("Id verification successful.").Encrypt() });
            }

        }


    }
}