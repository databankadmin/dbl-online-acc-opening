using AppMain.Providers;
using AppModels;
using AppUtils;
using DBHelper.Schema;
using Hangfire;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace AppMain.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index(string message = null)
        {
            Utilities.AppUsers = AppServerHelper.GetAppUsers();

            // SetSuperAdmin();
            if (!string.IsNullOrEmpty(message))
            {
                ViewBag.Message = message.Decrypt();
            }
           // RecurringJob.AddOrUpdate(() => Utilities.ProcessQuickSMS(), "*/1 * * * *");
            RecurringJob.AddOrUpdate(() => Utilities.ProcessEmails(), "*/1 * * * *");
            ViewBag.username = string.Empty;
            return View();
        }


        //public void SetSuperAdmin()
        //{
        //    using (var context = new DBLAccountOpeningContext())
        //    {
        //        if (!context.AppUsers.Any())
        //        {
        //            context.AppUsers.Add(new AppUser
        //            {

        //                CreatedDate = DateTime.Now,
        //                Email = "admin@dbl",
        //                Id = Guid.NewGuid(),
        //                IsActive = true,
        //                Name = "Administrator",
        //                Password = "admin@1234".Encrypt(),
        //                Phone = "0302610610",
        //                RoleId = 1,
        //            });
        //        }
        //        foreach (var item in context.Accounts.Where(x => x.ReferenceNo == null))
        //        {
        //            SetRef(item.Id, Utilities.GenerateApplicationReference());
        //        }
        //        context.SaveChanges();
        //        foreach (var item in context.Accounts)
        //        {
        //            if (string.IsNullOrEmpty(item.BranchCode))
        //            {
        //                item.BranchCode = Utilities.GetRandomBranchCode();
        //            }

        //        }
        //        context.SaveChanges();
        //    }
        //}
        //public void SetRef(Guid accountId,string _ref)
        //{
        //    using (var context=new DBLAccountOpeningContext())
        //    {
        //        var model = context.Accounts.Find(accountId);
        //        model.ReferenceNo = _ref;
        //        context.SaveChanges();
        //    }
        //}

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Index(string username, string password)
        {
           
            string encryptedPassword = password.Encrypt();
            ViewBag.username = username;
            var accountUser = Utilities.ValidateClientAccountLogin(username, password);

            var identityServerUser = AppServerHelper.AuthenticateUserAsync(username, password);


            if (identityServerUser != null)
            {
                if (!identityServerUser.DefaultPasswordChanged)
                {
                    string appUrl = ConfigurationManager.AppSettings["appUrl"];
                    string appServerUrl = ConfigurationManager.AppSettings["AUTH_BASE_URL"] +
                                          "/User/FirstTimeLogin?returnUrl=" + Utilities.EncodeBase64(appUrl) +
                                          "&_username=" + Utilities.EncodeBase64(username);
                    return RedirectPermanent(appServerUrl);
                }
                FormsAuthentication.SetAuthCookie(username, false);
                Profile.Initialize(username, true);
                Utilities.LogActivity(username,"Successful login");
                return RedirectToAction("Applications", "Admin");
            }
            else if (accountUser!=null)
            {
                FormsAuthentication.SetAuthCookie(username, false);
                Profile.Initialize(username, true);
                MvcApplication.TempAccount = accountUser;
                MvcApplication.AccountType = MvcApplication.TempAccount.AccountTypeId;

                return RedirectToAction("AppProfile", "Client");
            }
            else
            {
                ViewBag.Message = "Login failed! Invalid credentials";
                return View();
            }

        }

        public ActionResult Logout(string message = null)
        {
            var user =Utilities.GetSessionUser() as UserModel;
            if (user!=null)
            {
                Utilities.LogActivity(user.Username, "User Logout");

            }

            FormsAuthentication.SignOut();

            if (!string.IsNullOrEmpty(message))
            {
                return RedirectToAction("Index", new { message = message });
            }
            return RedirectToAction("Index");
        }

    }
}