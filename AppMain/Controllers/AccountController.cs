using AppMain.Providers;
using AppUtils;
using DBHelper.Schema;
using System;
using System.Collections.Generic;
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
            SetSuperAdmin();
            if (!string.IsNullOrEmpty(message))
            {
                ViewBag.Message = message.Decrypt();
            }
           // RecurringJob.AddOrUpdate(() => Utilities.ProcessMessages(), "*/1 * * * *");
            ViewBag.username = string.Empty;
            return View();
        }


        public void SetSuperAdmin()
        {
            using (var context = new DBLAccountOpeningContext())
            {
                if (!context.AppUsers.Any())
                {
                    context.AppUsers.Add(new AppUser
                    {

                        CreatedDate = DateTime.Now,
                        Email = "admin@dbl",
                        Id = Guid.NewGuid(),
                        IsActive = true,
                        Name = "Administrator",
                        Password = "admin@1234".Encrypt(),
                        Phone = "0302610610",
                        RoleId = 1,
                    });
                }
                foreach (var item in context.Accounts.Where(x => x.ReferenceNo == null))
                {
                    SetRef(item.Id, Utilities.GenerateApplicationReference());
                }
                context.SaveChanges();
            }
        }
        public void SetRef(Guid accountId,string _ref)
        {
            using (var context=new DBLAccountOpeningContext())
            {
                var model = context.Accounts.Find(accountId);
                model.ReferenceNo = _ref;
                context.SaveChanges();
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Index(string username, string password)
        {
           
            string encryptedPassword = password.Encrypt();
            ViewBag.username = username;
            var admin = Utilities.ValidateAdmin(username, encryptedPassword);

            if (admin != null)
            {
                FormsAuthentication.SetAuthCookie(username, false);
                Profile.Initialize(username, true);
                return RedirectToAction("Applications", "Admin");
            }
            else
            {
                ViewBag.Message = "Login failed! Invalid credentials";
                return View();
            }

        }

        public ActionResult Logout(string message = null)
        {

            FormsAuthentication.SignOut();

            if (!string.IsNullOrEmpty(message))
            {
                return RedirectToAction("Index", new { message = message });
            }
            return RedirectToAction("Index");
        }

    }
}