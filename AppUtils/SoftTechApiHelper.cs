using AppLogger;
using AppModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AppUtils
{
   public static class SoftTechApiHelper
    {
        private static readonly HttpClient _client = new HttpClient();
        private static readonly string ApiUrl = GetBaseUrl();

        public static string GetBaseUrl()
        {
            return ConfigurationManager.AppSettings["SOFTTECH_API_BASE_URL"];
        }
        public static void Init()
        {
            if (_client.BaseAddress == null)
            {
                _client.BaseAddress = new Uri(ApiUrl);
                _client.DefaultRequestHeaders.Accept.Clear();
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }
        }


        public async static Task<List<SoftTechInvestmentTypeModel>> GetInvesmentTypes()
        {
            try
            {

                Init();
                HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress + "getInvTypeList");
                var responseContent = response.Content.ReadAsStringAsync().Result;
                var resp = JsonConvert.DeserializeObject<List<SoftTechInvestmentTypeModel>>(responseContent);
                return resp;
            }
            catch (Exception ex)
            {
                Logger.Instance.logError(ex);
                return new List<SoftTechInvestmentTypeModel>() ;
            }
        }


        public async static Task<List<SoftTechCountryModel>> GetCountryList()
        {
            try
            {

                Init();
                HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress + "getCountryList");
                var responseContent = response.Content.ReadAsStringAsync().Result;
                var resp = JsonConvert.DeserializeObject<List<SoftTechCountryModel>>(responseContent);
                return resp;
            }
            catch (Exception ex)
            {
                Logger.Instance.logError(ex);
                return new List<SoftTechCountryModel>();
            }
        }

        public async static Task<List<SoftTechMaritalStatusModel>> GetMartialStatusList()
        {
            try
            {

                Init();
                HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress + "getMaritialStatusList");
                var responseContent = response.Content.ReadAsStringAsync().Result;
                var resp = JsonConvert.DeserializeObject<List<SoftTechMaritalStatusModel>>(responseContent);
                return resp;
            }
            catch (Exception ex)
            {
                Logger.Instance.logError(ex);
                return new List<SoftTechMaritalStatusModel>();
            }
        }


        public async static Task<List<SoftTechCityModel>> GetCityList()
        {
            try
            {

                Init();
                HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress + "getCityList");
                var responseContent = response.Content.ReadAsStringAsync().Result;
                var resp = JsonConvert.DeserializeObject<List<SoftTechCityModel>>(responseContent);
                return resp;
            }
            catch (Exception ex)
            {
                Logger.Instance.logError(ex);
                return new List<SoftTechCityModel>();
            }
        }


        public async static Task<List<SoftTechSuffixModel>> GetSuffixList()
        {
            try
            {

                Init();
                HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress + "getSuffixList");
                var responseContent = response.Content.ReadAsStringAsync().Result;
                var resp = JsonConvert.DeserializeObject<List<SoftTechSuffixModel>>(responseContent);
                return resp;
            }
            catch (Exception ex)
            {
                Logger.Instance.logError(ex);
                return new List<SoftTechSuffixModel>();
            }
        }


        public async static Task<List<SoftTechOccupationModel>> GetOccupationList()
        {
            try
            {

                Init();
                HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress + "getOccupationList");
                var responseContent = response.Content.ReadAsStringAsync().Result;
                var resp = JsonConvert.DeserializeObject<List<SoftTechOccupationModel>>(responseContent);
                return resp;
            }
            catch (Exception ex)
            {
                Logger.Instance.logError(ex);
                return new List<SoftTechOccupationModel>();
            }
        }


        public async static Task<List<SoftTechIDTypeModel>> GetIDTypeList()
        {
            try
            {

                Init();
                HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress + "getIdTypesList");
                var responseContent = response.Content.ReadAsStringAsync().Result;
                var resp = JsonConvert.DeserializeObject<List<SoftTechIDTypeModel>>(responseContent);
                return resp;
            }
            catch (Exception ex)
            {
                Logger.Instance.logError(ex);
                return new List<SoftTechIDTypeModel>();
            }
        }
        public async static Task<List<SoftTechRelationshipModel>> GetRelationshipList()
        {
            try
            {

                Init();
                HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress + "getRelationshipList");
                var responseContent = response.Content.ReadAsStringAsync().Result;
                var resp = JsonConvert.DeserializeObject<List<SoftTechRelationshipModel>>(responseContent);
                return resp;
            }
            catch (Exception ex)
            {
                Logger.Instance.logError(ex);
                return new List<SoftTechRelationshipModel>();
            }
        }
        public async static Task<List<SoftTechIncomeSourceModel>> GetIncomeSources()
        {
            try
            {

                Init();
                HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress + "getIncomeSources");
                var responseContent = response.Content.ReadAsStringAsync().Result;
                var resp = JsonConvert.DeserializeObject<List<SoftTechIncomeSourceModel>>(responseContent);
                return resp;
            }
            catch (Exception ex)
            {
                Logger.Instance.logError(ex);
                return new List<SoftTechIncomeSourceModel>();
            }
        }



        public async static Task<List<SoftTechAnnualIncomeRangesModel>> GetAnnualIncomeRanges()
        {
            try
            {

                Init();
                HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress + "getAnnualIncomeRanges");
                var responseContent = response.Content.ReadAsStringAsync().Result;
                var resp = JsonConvert.DeserializeObject<List<SoftTechAnnualIncomeRangesModel>>(responseContent);
                return resp;
            }
            catch (Exception ex)
            {
                Logger.Instance.logError(ex);
                return new List<SoftTechAnnualIncomeRangesModel>();
            }
        }

        public async static Task<List<SoftTechAnnualRiskToleranceModel>> GetRiskToleranceLevels()
        {
            try
            {

                Init();
                HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress + "getRiskTolLevels");
                var responseContent = response.Content.ReadAsStringAsync().Result;
                var resp = JsonConvert.DeserializeObject<List<SoftTechAnnualRiskToleranceModel>>(responseContent);
                return resp;
            }
            catch (Exception ex)
            {
                Logger.Instance.logError(ex);
                return new List<SoftTechAnnualRiskToleranceModel>();
            }
        }


        public async static Task<List<SoftTechAccTypeModel>> GetAccountTypes()
        {
            try
            {

                Init();
                HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress + "getAccType");
                var responseContent = response.Content.ReadAsStringAsync().Result;
                var resp = JsonConvert.DeserializeObject<List<SoftTechAccTypeModel>>(responseContent);
                return resp;
            }
            catch (Exception ex)
            {
                Logger.Instance.logError(ex);
                return new List<SoftTechAccTypeModel>();
            }
        }

        public async static Task<List<SoftTechBranchModel>> GetBranches()
        {
            try
            {

                Init();
                HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress + "getBranches");
                var responseContent = response.Content.ReadAsStringAsync().Result;
                var resp = JsonConvert.DeserializeObject<List<SoftTechBranchModel>>(responseContent);
                return resp;
            }
            catch (Exception ex)
            {
                Logger.Instance.logError(ex);
                return new List<SoftTechBranchModel>();
            }
        }

        public static async Task<SoftTechCreateAccountResponseModel> CreateAccount(SoftTechCreateAccountModel_V2 model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.AUTH_PERSON_TEL_NUM))
                {
                    model.AUTH_PERSON_TEL_NUM = "0000000000";
                }

                if (string.IsNullOrEmpty(model.MOBILE_NUM))
                {
                    model.MOBILE_NUM = "0000000000";
                }

                if (string.IsNullOrEmpty(model.AUTH_PERSON_MOB_NUM))
                {
                    model.AUTH_PERSON_MOB_NUM = "0000000000";
                }

                if (!string.IsNullOrEmpty(model.AUTH_PERSON_TEL_NUM))
                {
                    model.AUTH_PERSON_TEL_NUM = model.AUTH_PERSON_TEL_NUM.Replace(" ", string.Empty); 
                }
                if (!string.IsNullOrEmpty(model.MOBILE_NUM))
                {
                    model.MOBILE_NUM = model.MOBILE_NUM.Replace(" ", string.Empty); 
                }
                if (!string.IsNullOrEmpty(model.AUTH_PERSON_MOB_NUM))
                {
                    model.AUTH_PERSON_MOB_NUM = model.AUTH_PERSON_MOB_NUM.Replace(" ", string.Empty);
                }
                if (!string.IsNullOrEmpty(model.FIRST_KIN_MOBILE_NUM))
                {
                    model.FIRST_KIN_MOBILE_NUM = model.FIRST_KIN_MOBILE_NUM.Replace(" ", string.Empty); 
                }
                if (!string.IsNullOrEmpty(model.SECOND_KIN_MOBILE_NUM))
                {
                    model.SECOND_KIN_MOBILE_NUM = model.SECOND_KIN_MOBILE_NUM.Replace(" ", string.Empty); 
                }
                if (!string.IsNullOrEmpty(model.FIRST_KIN_TEL_NUM))
                {
                    model.FIRST_KIN_TEL_NUM = model.FIRST_KIN_TEL_NUM.Replace(" ", string.Empty); 
                }
                if (!string.IsNullOrEmpty(model.SECOND_KIN_TEL_NUM))
                {
                    model.SECOND_KIN_TEL_NUM = model.SECOND_KIN_TEL_NUM.Replace(" ", string.Empty); 
                }
                if (!string.IsNullOrEmpty(model.AUTH_PERSON_ZIP))
                {
                    model.AUTH_PERSON_ZIP = model.AUTH_PERSON_ZIP.Replace(" ", string.Empty); 
                }



                //FIRST_KIN_MOBILE_NUM

                Init();
                string jsonString = JsonConvert.SerializeObject(model);
                Logger.Instance.logInfo(jsonString);
                var content = JsonConvert.SerializeObject(model);
                var response = await _client.PostAsync(
                    _client.BaseAddress +
                    "saveClient",
                    new StringContent(content, Encoding.UTF8, "application/json"));
                var responseContent = response.Content.ReadAsStringAsync().Result;
                var save = JsonConvert.DeserializeObject<SoftTechCreateAccountResponseModel>(responseContent);
                
                Logger.Instance.logInfo("REQUEST: "+Environment.NewLine+jsonString+Environment.NewLine+Environment.NewLine+"RESPONSE: "+responseContent+"\n\n"+ responseContent);
                //if (save==null)
                //{
                //    save = new SoftTechCreateAccountResponseModel { 
                //    desc= responseContent,
                    
                //    };
                //}
                //else
                //{
                //    save.desc = save.error + " " + save.path;
                //}
                //if (save!=null)
                //{
                //   // save.ErrorList = save.desc + "\nMissing fiels: " + save.missingFields.ToString() + "\nInvalid fields: " + save.invalidFields.ToString();
                //}
                //save.ErrorList=FormatError(save.ErrorList);
                //  Logger.Instance.logWarning(save.ErrorList);
                return save;
            }
            catch (Exception ex)
            {
                Logger.Instance.logError(ex);
                return new SoftTechCreateAccountResponseModel {responseCode="04",desc=ex.Message };
            }


        }

    }
}
