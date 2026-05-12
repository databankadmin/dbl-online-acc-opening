using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Parsing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PdfFillerDemo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            PdfLoadedDocument loadedDocument = new PdfLoadedDocument(Server.MapPath("~/Images/csd_2_updated.pdf"));
            PdfLoadedForm loadedForm = loadedDocument.Form;
            (loadedForm.Fields["Bank Name"] as PdfLoadedTextBoxField).Text = "Ebenezer Foh";
            (loadedForm.Fields["Branch Name"] as PdfLoadedTextBoxField).Text = "0546326893";
            (loadedForm.Fields["Account No"] as PdfLoadedTextBoxField).Text = "TEST DATA HERE";

            string saveFileName = Guid.NewGuid().ToString() + ".pdf";
            string savePath = Server.MapPath("~/Images/" + saveFileName);

            float x = 447;
            float y = 18;

            float signatureStartX = 380;
            float signatureStartY = 625;


            //draw photoID
            byte[] buff = System.IO.File.ReadAllBytes(Server.MapPath("~/Images/pic.jpg"));
            MemoryStream imageStream = new MemoryStream(buff);
            var page = loadedDocument.Pages[0] as PdfLoadedPage;
            PdfGraphics graphics = page.Graphics;
            PdfBitmap image = new PdfBitmap(imageStream);
            graphics.DrawImage(image, x, y,70,72);

            //draw signature
            byte[] buff2 = System.IO.File.ReadAllBytes(Server.MapPath("~/Images/sign.jpg"));
            MemoryStream imageStream2 = new MemoryStream(buff2);
            var page2 = loadedDocument.Pages[0] as PdfLoadedPage;
            PdfGraphics graphics2 = page.Graphics;
            PdfBitmap image2 = new PdfBitmap(imageStream2);
            graphics2.DrawImage(image2, signatureStartX, signatureStartY, 100, 25);
            graphics2.DrawImage(image2, signatureStartX, 727, 100, 25);


            loadedDocument.Save(savePath);
            loadedDocument.Close(true);
            //This will open the PDF file so, the result will be seen in default PDF Viewer 

            //Pixel to point conversion
           

            //Get the image stream
            //Stream imageStream = Assembly.GetManifestResourceStream("ImageContent.Assets.Image.png");
            // var imageStream = new StreamReader(Server.MapPath("~/Images/csd_fillable.pdf"));

           
          //  Stream imageStream = typeof(App).GetTypeInfo().Assembly.GetManifestResourceStream("ImageContent.Assets.Image.png");

            //Save the document to preserve all the current changes made in the document.
            // Stream documentStream = pdfViewerControl.SaveDocument();

            //Create PdfLoadedDocument instance from the saved stream.
            //  PdfLoadedDocument doc = new PdfLoadedDocument(documentStream);

           
            Process.Start(savePath);
            return View();
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