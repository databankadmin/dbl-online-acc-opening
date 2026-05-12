using AppMain.Providers;
using AppUtils;
using DBHelper.Schema;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AppMain.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {           
            Utilities.AppUsers = AppServerHelper.GetAppUsers();
            MvcApplication.AccountType = 0;
            if (Utilities.AppUsers == null)
            {
                Utilities.AppUsers = AppServerHelper.GetAppUsers();
            }
            return View();
        }

        private async Task InitSoftTechApi()
        {
            var investmentTypes = await SoftTechApiHelper.GetInvesmentTypes();
            var branches = await SoftTechApiHelper.GetBranches();
            var accountTypes    = await SoftTechApiHelper.GetAccountTypes();
            var countries = await SoftTechApiHelper.GetCountryList();
            var maritalStatus = await SoftTechApiHelper.GetMartialStatusList();
            var cities = await SoftTechApiHelper.GetCityList();
            var suffix = await SoftTechApiHelper.GetSuffixList();
            var occupation = await SoftTechApiHelper.GetOccupationList();
            var idTypes = await SoftTechApiHelper.GetIDTypeList();
            var relationShip = await SoftTechApiHelper.GetRelationshipList();
            var incomeSources = await SoftTechApiHelper.GetIncomeSources();
            var incomeRanges = await SoftTechApiHelper.GetAnnualIncomeRanges();
            var riskTolerance = await SoftTechApiHelper.GetRiskToleranceLevels();

            var context = new DBLAccountOpeningContext();
            if (investmentTypes != null && investmentTypes.Any())
            {
                foreach (var item in investmentTypes)
                {
                    if (!context.InvestmentTypes.Any(x => x.SoftTechCode == item.investmentTypeCode))
                    {
                        context.InvestmentTypes.Add(new InvestmentType
                        {
                            IsActive=true,
                            Name=item.investmentTypeDesc,
                            SoftTechCode=item.investmentTypeCode,
                        });
                    }
                }

                //location
                foreach (var item in branches)
                {
                    if (!context.Branches.Any(x => x.SoftTechCode == item.branchCode))
                    {
                        context.Branches.Add(new Branch
                        {
                          
                            SoftTechCode = item.branchCode,
                            BRANCH_NAME=item.branchName,
                            BRANCH_ADDRESS=null,
                            BRANCH_CODE=item.branchCode
                            
                        });
                    }
                }

                //country
                foreach (var item in countries)
                {
                    if (!context.Countries.Any(x => x.SoftTechCode == item.countryCode))
                    {
                        context.Countries.Add(new Country
                        {

                            SoftTechCode = item.countryCode,
                            Code=item.countryCode,
                            CountryName=item.countryName,
                        });
                    }
                }


                //marital:
                foreach (var item in maritalStatus)
                {
                    if (!context.MaritalStatus.Any(x => x.SoftTechCode == item.statusCode.ToString()))
                    {
                        context.MaritalStatus.Add(new MaritalStatu
                        {

                            SoftTechCode = item.statusCode.ToString(),
                            IsActive=true,
                            Name=item.statusName
                            
                        });
                    }
                }

                //cities
                foreach (var item in cities)
                {
                    try
                    {
                        if (!context.Cities.Any(x => x.SoftTechCode == item.cityCode))
                        {
                            context.Cities.Add(new City
                            {

                                SoftTechCode = item.cityCode.ToString(),
                                Name = item.cityName,

                            });
                            context.SaveChanges();
                        }
                    }
                    catch (Exception ex)
                    {
                        var i = item;
                        continue;
                    }
                }


                foreach (var item in suffix)
                {
                    if (!context.CSDDepositoryParticipantOptions.Any(x => x.SoftTechCode == item.suffixCode.ToString()))
                    {
                        context.CSDDepositoryParticipantOptions.Add(new CSDDepositoryParticipantOption
                        {

                            SoftTechCode = item.suffixCode.ToString(),
                            IsActive = true,
                            Name = item.suffixName,

                        });
                    }
                }

                //occupation
                foreach (var item in occupation)
                {
                    try
                    {
                        if (!context.Occupations.Any(x => x.SoftTechCode == item.occupationCode))
                        {
                            context.Occupations.Add(new Occupation
                            {

                                SoftTechCode = item.occupationCode.ToString(),
                                Name = item.occupationDesc,

                            });
                            context.SaveChanges();
                        }
                    }
                    catch (Exception ex)
                    {
                        var i = item;
                        continue;
                    }
                }


                foreach (var item in relationShip)
                {
                    if (!context.Relationships.Any(x => x.SoftTechCode == item.relationshipCode.ToString()))
                    {
                        context.Relationships.Add(new Relationship
                        {

                            SoftTechCode = item.relationshipCode.ToString(),
                            Name=item.relationshipName,

                        });
                    }
                }

                foreach (var item in incomeSources)
                {
                    if (!context.SourceOfIncomes.Any(x => x.SoftTechCode == item.incomeSourceId.ToString()))
                    {
                        context.SourceOfIncomes.Add(new SourceOfIncome
                        {

                            SoftTechCode = item.incomeSourceId.ToString(),
                            Name = item.incomeSourceDesc,
                            IsActive=true

                        });
                    }
                }

                foreach (var item in incomeRanges)
                {
                    if (!context.ApproximateAnnualIncomes.Any(x => x.SoftTechCode == item.annualIncomeId.ToString()))
                    {
                        context.ApproximateAnnualIncomes.Add(new ApproximateAnnualIncome
                        {

                            SoftTechCode = item.annualIncomeId.ToString(),
                            Name = item.description,
                            IsActive = true,

                        });
                    }
                }
                foreach (var item in riskTolerance)
                {
                    if (!context.RiskTolerances.Any(x => x.SoftTechCode == item.riskToleranceId.ToString()))
                    {
                        context.RiskTolerances.Add(new RiskTolerance
                        {

                            SoftTechCode = item.riskToleranceId.ToString(),
                            Name = item.riskToleranceDesc,
                            IsActive = true,
                            

                        });
                    }
                }

                context.SaveChanges();
            }
        }

        [HttpPost]
        public ActionResult Index(int accountType)
        {
            using (var context = new DBLAccountOpeningContext())
            {
                //foreach (var item in context.AccountInstructionEmploymentDetails)
                //{
                //    if (item.SourceOfFundsIds == null)
                //    {
                //        item.SourceOfFundsIds = item.SourceOfFundId.ToString();
                //    }
                //}
                //foreach (var item in context.Accounts)
                //{
                //    if (string.IsNullOrEmpty(item.Password))
                //    {
                //        string password = "@1234";
                //        item.Password = Utilities.EncodeBase64(password);
                //    }
                //}
                //context.SaveChanges();


            }
            //document.location.href = "@Url.Action("Initiate", "NewAccount")?acc_type=" + btoa(accType) + "&param=" + btoa(param);
            return RedirectToAction("Initiate", "NewAccount",new { acc_type=AppUtils.Utilities.EncodeBase64(accountType.ToString()), param=AppUtils.Utilities.EncodeBase64(Guid.NewGuid().ToString()) });
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