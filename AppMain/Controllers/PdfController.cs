using AppMain.Providers;
using AppUtils;
using IronPdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AppMain.Controllers
{
    public class PdfController : Controller
    {
       

        // GET: Pdf
        public ActionResult DblFormData(string _refNumber, string message = null)
        {
            Guid applicationId = Guid.Parse(_refNumber.Decrypt());
            ViewBag.applicationId = applicationId;
            var model = Utilities.GetApplications(0, 0, null, applicationId.ToString()).FirstOrDefault();

            var Renderer = new IronPdf.HtmlToPdf();
            Renderer.PrintOptions.MarginTop = 20;  //millimeters
            Renderer.PrintOptions.MarginBottom = 20;
            Renderer.PrintOptions.MarginLeft = 1;
            Renderer.PrintOptions.MarginRight = 1;
            //Renderer.PrintOptions.p
            Renderer.PrintOptions.CssMediaType = PdfPrintOptions.PdfCssMediaType.Print;

            Renderer.PrintOptions.Header = new SimpleHeaderFooter()
            {
                CenterText = "DBL Form Data - " + model.AccountName,
                DrawDividerLine = true,
                FontSize = 12
            };

            Renderer.PrintOptions.Footer = new SimpleHeaderFooter()
            {
                LeftText = "{date} {time}",
                RightText = "Page {page} of {total-pages}",
                DrawDividerLine = true,
                FontSize = 6
            };

            string result = this.RenderRazorViewToString("~/Views/Partials/pdfs/dblFormData.cshtml", model);
            string fileName = "DBL_Data_" + model.AccountName.Replace(" ", "_") + "_" + DateTime.Now.Ticks.ToString();
            var PDF = Renderer.RenderHtmlAsPdf(result);
            var OutputPath = Server.MapPath("~/Images/" + fileName + ".pdf");
            PDF.SaveAs(OutputPath);
            //  System.Diagnostics.Process.Start(OutputPath);
            return RedirectToAction("DownloadFile","Admin", new { path =fileName+".pdf", _refNumber =_refNumber});
        }

        public ActionResult AmlFormData(string _refNumber, string message = null)
        {
            Guid applicationId = Guid.Parse(_refNumber.Decrypt());
            ViewBag.applicationId = applicationId;
            var model = Utilities.GetApplications(0, 0, null, applicationId.ToString()).FirstOrDefault();

            var Renderer = new IronPdf.HtmlToPdf();
            Renderer.PrintOptions.MarginTop = 20;  //millimeters
            Renderer.PrintOptions.MarginBottom = 20;
            Renderer.PrintOptions.MarginLeft = 1;
            Renderer.PrintOptions.MarginRight = 1;
            //Renderer.PrintOptions.p
            Renderer.PrintOptions.CssMediaType = PdfPrintOptions.PdfCssMediaType.Print;

            Renderer.PrintOptions.Header = new SimpleHeaderFooter()
            {
                CenterText = "AML Profile - " + model.AccountName,
                DrawDividerLine = true,
                FontSize = 12
            };

            Renderer.PrintOptions.Footer = new SimpleHeaderFooter()
            {
                LeftText = "{date} {time}",
                RightText = "Page {page} of {total-pages}",
                DrawDividerLine = true,
                FontSize = 6
            };

            string result = this.RenderRazorViewToString("~/Views/Partials/pdfs/amlProfile.cshtml", model);
            string fileName = "AML_Profile_" + model.AccountName.Replace(" ", "_") + "_" + DateTime.Now.Ticks.ToString();
            var PDF = Renderer.RenderHtmlAsPdf(result);
            var OutputPath = Server.MapPath("~/Images/" + fileName + ".pdf");
            PDF.SaveAs(OutputPath);
            //   System.Diagnostics.Process.Start(OutputPath);
            return RedirectToAction("DownloadFile", "Admin", new { path = fileName + ".pdf", _refNumber = _refNumber });
        }

        public ActionResult ETIFormData(string _refNumber, string message = null)
        {
            Guid applicationId = Guid.Parse(_refNumber.Decrypt());
            ViewBag.applicationId = applicationId;
            var model = Utilities.GetApplications(0, 0, null, applicationId.ToString()).FirstOrDefault();

            var Renderer = new IronPdf.HtmlToPdf();
            Renderer.PrintOptions.MarginTop = 20;  //millimeters
            Renderer.PrintOptions.MarginBottom = 20;
            Renderer.PrintOptions.MarginLeft = 1;
            Renderer.PrintOptions.MarginRight = 1;
            //Renderer.PrintOptions.p
            Renderer.PrintOptions.CssMediaType = PdfPrintOptions.PdfCssMediaType.Print;

            Renderer.PrintOptions.Header = new SimpleHeaderFooter()
            {
                CenterText = "ETI Profile - " + model.AccountName,
                DrawDividerLine = true,
                FontSize = 12
            };

            Renderer.PrintOptions.Footer = new SimpleHeaderFooter()
            {
                LeftText = "{date} {time}",
                RightText = "Page {page} of {total-pages}",
                DrawDividerLine = true,
                FontSize = 6
            };

            string result = this.RenderRazorViewToString("~/Views/Partials/pdfs/etiProfile.cshtml", model);
            string fileName = "ETI_Profile_" + model.AccountName.Replace(" ", "_") + "_" + DateTime.Now.Ticks.ToString();
            var PDF = Renderer.RenderHtmlAsPdf(result);
            var OutputPath = Server.MapPath("~/Images/" + fileName + ".pdf");
            PDF.SaveAs(OutputPath);
            // System.Diagnostics.Process.Start(OutputPath);
            return RedirectToAction("DownloadFile", "Admin", new { path = fileName + ".pdf", _refNumber = _refNumber });
        }


    }




    public static class RazorViewToString
    {
        public static string RenderRazorViewToString(this Controller controller, string viewName, object model)
        {
            controller.ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var context = new HttpContextWrapper(HttpContext.Current);
                var routeData = new RouteData();
                routeData.Values.Add("controller", "{controller}/{action}/{id}");
                var controllerContext = new ControllerContext(new RequestContext(context, routeData), new FakeController());
                var viewResult = ViewEngines.Engines.FindView(controllerContext, viewName, null);
                var viewContext = new ViewContext(controllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(controllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }
    }
    public class FakeController : Controller
    {
    }

}