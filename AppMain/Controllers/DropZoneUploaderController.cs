using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppMain.Controllers
{
    public class DropZoneUploaderController : Controller
    {
        public string SaveFileUpload(HttpPostedFileBase file,string suggestedName=null)
        {
            Random rnd = new Random();
            string fileAppend = rnd.Next(100000, 999999).ToString() + DateTime.UtcNow.Ticks;
            string ext = Path.GetExtension(file.FileName);
            string finalFileName =!string.IsNullOrEmpty(suggestedName)?suggestedName: Guid.NewGuid().ToString() + fileAppend + ext;
            string fileSavePath = Server.MapPath("~/Images/" + finalFileName); //create the full path
            file.SaveAs(fileSavePath);
            var saveName = finalFileName;
            return saveName;
        }



        [HttpPost]
        public string Single(HttpPostedFileBase _file = null)
        {
            return SaveFileUpload(_file);
        }

        [HttpPost]
        public string Multi(List<HttpPostedFileBase> _files = null)
        {
            var fileNames =new List<string>();
            foreach (var file in _files)
            {
               fileNames.Add(SaveFileUpload(file,file.FileName));
            }
            return string.Join(",", fileNames);
        }

    }
}