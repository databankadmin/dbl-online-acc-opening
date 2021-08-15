using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AppLogger;
using AppModels;
using AppUtils;
using Newtonsoft.Json;

namespace AppMain.Providers
{
    public static class AppServerHelper
    {
        private static readonly string AppCode = ConfigurationManager.AppSettings["APP_CODE"];


        public static async Task<List<UserAppRole>> GetUserRolesAsync(string username)
        {
            string authBaseUrl = ConfigurationManager.AppSettings["AUTH_BASE_URL"];
            using (var httpClient = new HttpClient())
            {
                if (httpClient.BaseAddress == null)
                {
                    httpClient.BaseAddress = new Uri(authBaseUrl);
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                }
                string _url = string.Format(authBaseUrl + "/api/appserver/getuserroles" + "?username={0}", username);
                var result = await httpClient.GetStringAsync(_url);
                var data = JsonConvert.DeserializeObject<List<UserAppRole>>(result);
                return data;
            }
        }



        public static List<AppRoleModel> GetAppRoles()
        {
            string authBaseUrl = ConfigurationManager.AppSettings["AUTH_BASE_URL"];
            using (var httpClient = new HttpClient())
            {
                if (httpClient.BaseAddress == null)
                {
                    httpClient.BaseAddress = new Uri(authBaseUrl);
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                }
                string _url = string.Format(authBaseUrl + "/api/appserver/getapproles" + "?appCode={0}", AppCode);
                var result = httpClient.GetStringAsync(_url).Result;
                var data = JsonConvert.DeserializeObject<List<AppRoleModel>>(result);
                return data;
            }
        }



    


        public static string DeleteUserAccount(string staffId)
        {
            try
            {
                string authBaseUrl = ConfigurationManager.AppSettings["AUTH_BASE_URL"];
                using (var httpClient = new HttpClient())
                {
                    if (httpClient.BaseAddress == null)
                    {
                        httpClient.BaseAddress = new Uri(authBaseUrl);
                        httpClient.DefaultRequestHeaders.Accept.Clear();
                        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    }
                    httpClient.Timeout = TimeSpan.FromMinutes(5);
                    string _url = string.Format(authBaseUrl + "/api/appserver/deactivateuseraccount" + "?staffId={0}" + "&appCode={1}", staffId, AppCode);
                    var result = httpClient.GetAsync(_url.Trim()).Result;
                    string responseBody = result.Content.ReadAsStringAsync().Result;
                    var data = JsonConvert.DeserializeObject<string>(responseBody);
                    return data;
                }


            }
            catch (Exception ex)
            {

                Logger.Instance.logError(ex);
                return null;
            }

        }



        public static UserModel AuthenticateUserAsync(string username, string password)
        {
            try
            {
                string authBaseUrl = ConfigurationManager.AppSettings["AUTH_BASE_URL"];
                password = Utilities.EncodeBase64(password);

                using (var httpClient = new HttpClient())
                {
                    if (httpClient.BaseAddress == null)
                    {
                        httpClient.BaseAddress = new Uri(authBaseUrl);
                        httpClient.DefaultRequestHeaders.Accept.Clear();
                        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    }
                    httpClient.Timeout=TimeSpan.FromMinutes(5);
                    string _url = string.Format(authBaseUrl + "/api/appserver/authenticateuser" + "?username={0}" + "&password={1}" + "&appCode={2}", username, password, AppCode);
                    var result = httpClient.GetAsync(_url.Trim()).Result;
                    if (result.StatusCode==HttpStatusCode.BadRequest)
                    {
                        var message = result.Content.ReadAsStringAsync();
                        return null;

                    }
                    string responseBody = result.Content.ReadAsStringAsync().Result;
                    var data = JsonConvert.DeserializeObject<UserModel>(responseBody);
                    return data;

                }


            }
            catch (Exception ex)
            {

                Logger.Instance.logError(ex);
                return null;
            }  
        }

        public static List<UserModel> GetAppUsers()
        {
            var model=new List<UserModel>();
            try
            {
                string authBaseUrl = ConfigurationManager.AppSettings["AUTH_BASE_URL"];
                using (var httpClient = new HttpClient())
                {
                    if (httpClient.BaseAddress == null)
                    {
                        httpClient.BaseAddress = new Uri(authBaseUrl);
                        httpClient.DefaultRequestHeaders.Accept.Clear();
                        httpClient.DefaultRequestHeaders.Accept.Add(
                            new MediaTypeWithQualityHeaderValue("application/json"));
                    }

                    httpClient.Timeout = TimeSpan.FromMinutes(5);
                    string _url =
                        string.Format(
                            authBaseUrl + "/api/appserver/getapplicationusers?appCode={0}", AppCode);
                    var result = httpClient.GetAsync(_url.Trim()).Result;
                    if (result.StatusCode == HttpStatusCode.BadRequest)
                    {
                        return model;
                    }
                    string responseBody = result.Content.ReadAsStringAsync().Result;
                    var users = JsonConvert.DeserializeObject<List<UserModel>>(responseBody);
                    return users;
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }

        }

        public static void LogActivity(ActivityModel model)
        {
            try
            {
                string authBaseUrl = ConfigurationManager.AppSettings["AUTH_BASE_URL"];
                model.AppCode = AppCode;
                using (var httpClient = new HttpClient())
                {
                    if (httpClient.BaseAddress == null)
                    {
                        httpClient.BaseAddress = new Uri(authBaseUrl);
                        httpClient.DefaultRequestHeaders.Accept.Clear();
                        httpClient.DefaultRequestHeaders.Accept.Add(
                            new MediaTypeWithQualityHeaderValue("application/json"));
                    }

                    string url = httpClient.BaseAddress + "/api/appserver/logactivity";
                    httpClient.Timeout = TimeSpan.FromMinutes(5);
                    var result = httpClient.PostAsJsonAsync(url, model);
                    string responseBody = result.Result.Content.ReadAsStringAsync().Result;
                    var data = JsonConvert.DeserializeObject<string>(responseBody);
                }
            }
            catch (Exception e)
            {
                Logger.Instance.logError(e);
            }
        }


        public static string AddUser(UserModel model)
        {
            try
            {
                string authBaseUrl = ConfigurationManager.AppSettings["AUTH_BASE_URL"];
                model.AppCode = AppCode;
                using (var httpClient = new HttpClient())
                {
                    if (httpClient.BaseAddress == null)
                    {
                        httpClient.BaseAddress = new Uri(authBaseUrl);
                        httpClient.DefaultRequestHeaders.Accept.Clear();
                        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    }
                    string url= httpClient.BaseAddress+ "/api/appserver/adduser";
                    httpClient.Timeout = TimeSpan.FromMinutes(5);
                    var result = httpClient.PostAsJsonAsync(url,model);
                    string responseBody = result.Result.Content.ReadAsStringAsync().Result;
                    var data =JsonConvert.DeserializeObject<string>(responseBody);
                    if (data=="OK")
                    {
                        return "OK";
                    }
                    return data;
                }
            }
            catch (Exception e)
            {
                Logger.Instance.logError(e);
                return e.Message;
            }
        }


        public static string EditUser(UserModel model, List<int> depts, string isAdmin)
        {
            try
            {
                string authBaseUrl = ConfigurationManager.AppSettings["AUTH_BASE_URL"];
                model.AppCode = AppCode;
                if (!string.IsNullOrEmpty(isAdmin))
                {
                    if (isAdmin.ToLower().Trim() == "on")
                    {
                        model.IsAdmin = true;
                    }
                }
                model.GroupIds = string.Join(",", depts);
                using (var httpClient = new HttpClient())
                {
                    if (httpClient.BaseAddress == null)
                    {
                        httpClient.BaseAddress = new Uri(authBaseUrl);
                        httpClient.DefaultRequestHeaders.Accept.Clear();
                        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    }
                    string url = httpClient.BaseAddress + "/api/appserver/editUser";
                    httpClient.Timeout = TimeSpan.FromMinutes(5);
                    var result = httpClient.PostAsJsonAsync(url, model);
                    string responseBody = result.Result.Content.ReadAsStringAsync().Result;
                    var data = JsonConvert.DeserializeObject<string>(responseBody);
                    if (data == "OK")
                    {
                        return "OK";
                    }
                    return data;
                }
            }
            catch (Exception e)
            {
                Logger.Instance.logError(e);
                return e.Message;
            }
        }

        public static string ResetUserAccount(string staffId)
        {
            try
            {
                string authBaseUrl = ConfigurationManager.AppSettings["AUTH_BASE_URL"];
                using (var httpClient = new HttpClient())
                {
                    if (httpClient.BaseAddress == null)
                    {
                        httpClient.BaseAddress = new Uri(authBaseUrl);
                        httpClient.DefaultRequestHeaders.Accept.Clear();
                        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    }
                    httpClient.Timeout = TimeSpan.FromMinutes(5);
                    string _url = string.Format(authBaseUrl + "/api/appserver/resetaccounttodefault" + "?username={0}", staffId);
                    var result = httpClient.GetAsync(_url.Trim()).Result;
                    string responseBody = result.Content.ReadAsStringAsync().Result;
                    var data = JsonConvert.DeserializeObject<string>(responseBody);
                    return data;
                }


            }
            catch (Exception ex)
            {

                Logger.Instance.logError(ex);
                return null;
            }
        }
    }
}