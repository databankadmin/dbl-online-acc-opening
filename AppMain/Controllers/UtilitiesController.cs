using AppModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace AppMain.Controllers
{
    public class UtilitiesController : Controller
    {

        // GET: Utilities

        [AllowAnonymous]
        public  async System.Threading.Tasks.Task<object> ValidateIdFromGvive(int idType,string idNumber,string idName,string recordId, string objectType)
        {
            try
            {
                string redemptionBaseUrl = ConfigurationManager.AppSettings["RedemptionBaseUrl"];
               // idType = 4;idNumber = "G1094904";
                if (idType!=1)
                {
                    //not dvla.ignore idName
                    idName = string.Empty;
                }
                using (var _client = new HttpClient())
                {

                    if (_client.BaseAddress == null)
                    {
                        _client.BaseAddress = new Uri(redemptionBaseUrl);
                        _client.DefaultRequestHeaders.Accept.Clear();
                        _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    }
                    HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress + "/api/Utilities/ValidatePhotoID?idType=" + idType + "&idNumber=" + idNumber + "&idName=" + idName);
                    var responseContent = response.Content.ReadAsStringAsync().Result;
                    var resp = JsonConvert.DeserializeObject<GviveResponseModel>(responseContent);
                    if (!resp.Error)
                    {
                        //validated
                        //accountMember, authorisedPerson,
                        AppUtils.Utilities.MarkIdCardAsValidated(recordId, objectType);
                        return PartialView("~/Views/Partials/photoIDVerify.cshtml", resp);
                    }
                    else
                    {
                        var error = new GviveResponseModel
                        {
                            Error = true,
                            Message = "Not Found"
                        };
                        return Json(error, JsonRequestBehavior.AllowGet);

                    }
                 
                }

            }
            catch (Exception ex)
            {

                AppLogger.Logger.Instance.logError(ex);
                return new GviveResponseModel { Error=true,Message=ex.Message};
            }
        }
    }
}