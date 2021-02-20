using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppUtils;

namespace AppMain.Controllers
{
    public class NewAccountController : Controller
    {
        // GET: NewAccount
        public ActionResult Initiate(string acc_type, string param)
        {
            MvcApplication.AccountType =int.Parse(Utilities.DecodeBase64(acc_type));
            return View();
        }
    }
}