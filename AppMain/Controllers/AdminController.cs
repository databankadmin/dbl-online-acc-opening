using AppMain.Providers;
using AppModels;
using AppUtils;
using DBHelper.Schema;
using Newtonsoft.Json;
using SofteckSdkSolution.Models;
using SofteckSdkSolution.SofteckAofSdk;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;

namespace AppMain.Controllers
{
    [Authorize]
    [OutputCache(NoStore = true, Duration = 0)]

    public class AdminController : Controller
    {
        public UserModel CurrentUser
        {

            get
            {

                return Utilities.GetSessionUser() as UserModel;
            }
        }
        // GET: Admin
        public ActionResult Applications(int accountType = 0, int investmentTypeId = 0, int statusId = 0, string key = null, string from = null, string to = null)
        {


            string dates = DateTime.Now.AddDays(-90).ToString("MM/dd/yyyy") + "-" + DateTime.Now.ToString("MM/dd/yyyy");
            if (!string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to))
            {
                dates = from + "-" + to;
            }

            var model = Utilities.GetApplications(accountType, investmentTypeId, dates, null, statusId, key);
            ViewBag.accountType = accountType;
            ViewBag.investmentTypeId = investmentTypeId;
            ViewBag.from = string.IsNullOrEmpty(from) ? DateTime.Now.AddDays(-90).ToShortDateString() : from;
            ViewBag.to = string.IsNullOrEmpty(to) ? DateTime.Now.ToShortDateString() : to;
            ViewBag.statusId = statusId;
            ViewBag.key = key;
            return View(model);
        }
        public ActionResult DownloadFile(string path, string _refNumber)
        {
            ViewBag.path = path;
            ViewBag._refNumber = _refNumber;
            return View();
        }


        public ActionResult ForgottenAccount(string message=null)
        {
            if (!string.IsNullOrEmpty(message))
            {
                ViewBag.Message = message.Decrypt();

            }
            using (var context = new DBLAccountOpeningContext())
            {
                var model = context.ForgottenAccountDetails.OrderByDescending(x => x.CreatedDate).ToList();
                return View(model);
            }

        }
        public ActionResult ForgottenAccountDetails(Guid itemId)
        {
            using (var context=new DBLAccountOpeningContext())
            {
                var model = context.ForgottenAccountDetails.Find(itemId);
                return View(model);
            }
        }
        [HttpPost,ValidateAntiForgeryToken,ValidateInput(false)]
        public ActionResult SendAccountDetailsResponse(Guid itemId,int responseType, string response)
        {
            using (var context=new DBLAccountOpeningContext())
            {
                var model = context.ForgottenAccountDetails.Find(itemId);
                if (model!=null)
                {
                    model.StatusId = 2;
                    context.SaveChanges();
                    string message = string.Empty;
                    if (responseType==1)
                    {
                        //account found
                        message = "Dear "+model.Name+ ",<br/>Thank you for getting  in touch with  Databank.<br/>Please below are your brokerage account  details:<br/>"+response+ "<br/><br/>Thank you for choosing  databank.";
                                           }
                    else if (responseType==2)
                    {
                        message = "Dear "+model.Name+"<br/>"+response+"<br/> Kindly call our team on 0302610610";
                    }
                    context.MessageHistories.Add(new MessageHistory
                    {
                        CreatedDate = DateTime.UtcNow,
                        Id = Guid.NewGuid(),
                        IsSent = false,
                        MessageContent = message,
                        SentTo = model.Email,
                        Type = "EMAIL",
                        Title = "Account Details".ToUpper()

                    });
                    model.Response = message + "<br/><br/>Sent On: " + DateTime.UtcNow.ToLongDateString() + "<br/>Sent By: " + CurrentUser.Username;

                    context.SaveChanges();
                    return RedirectToAction("ForgottenAccount", new { message = ("Account details sent to " + model.Email).Encrypt() });

                }
                else
                {
                    return RedirectToAction("ForgottenAccount");
                }
            }
        }


        [HttpPost,ValidateAntiForgeryToken,ValidateInput(false)]
        public ActionResult SendEditRequest(Guid itemId,string errorList)
        {
            var context = new DBLAccountOpeningContext();
            var accountBasic = Utilities.GetApplications(0, 0, null, itemId.ToString()).FirstOrDefault();
            context.AccountEditRequests.Add(new AccountEditRequest {
                AccountId=itemId,
                CreatedBy=CurrentUser.Username,
                CreatedDate=DateTime.Now,
                ErrorList=errorList,
                Id=Guid.NewGuid(),
                IsActive=true,
            });
            string url = ConfigurationManager.AppSettings["appUrl"].ToString();
            string sendTo = string.Empty;
            if (accountBasic.AccountTypeId<=3)
            {
                var accountMember = Utilities.GetAccountMembers(itemId).FirstOrDefault();
                sendTo = accountMember.Email;
            }
            else if (accountBasic.AccountTypeId==4)
            {
                sendTo = accountBasic.InsStreetAddressEmail;
            }
           // sendTo = "fohebenezer@gmail.com";
            string message = "Hello "+accountBasic.AccountName+"<br/>Thank you for getting in touch with Databank. We have received your request to create a new Brokerage account.<br/>However, the following errors were identified with your entries.<br/>Kindly use the link and credentials below to make the needed corrections and resubmit the application.<br/><br/><b>Error List</b><br/>"+errorList+"<br/> Application Reference/Username: "+accountBasic.RefNo+"<br/>Password: "+accountBasic.Password+"<br/><a href="+ url + "><b>Login to Account</b></a>"+ "<br/><br/>Thank you for choosing  databank.";

            context.MessageHistories.Add(new MessageHistory
            {
                CreatedDate = DateTime.UtcNow,
                Id = Guid.NewGuid(),
                IsSent = false,
                MessageContent = message,
                SentTo = sendTo,
                Type = "EMAIL",
                Title = "New Account-Request for Edit".ToUpper()

            });
            var find = context.Accounts.Find(itemId);
            find.StatusId = 5;
            context.SaveChanges();
            return RedirectToAction("ApplicantProfile", new { _refNumber = itemId.ToString().Encrypt(), message = ("client has been notified by mail").Encrypt() });


        }

        public ActionResult ApplicantProfile(string _refNumber, string message = null)
        {
            if (!string.IsNullOrEmpty(message))
            {
                ViewBag.Message = message.Decrypt();
            }
            Guid applicationId = Guid.Parse(_refNumber.Decrypt());
            ViewBag.applicationId = applicationId;
            var model = Utilities.GetApplications(0, 0, null, applicationId.ToString()).FirstOrDefault();
            Utilities.LogActivity(CurrentUser.Username, "Viewed application profile - " + model.RefNo);
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult CancelOrRejectApplication(Guid applicationId, string action, string comments, string notifyApplicant)
        {
            using (var context = new DBLAccountOpeningContext())
            {
                var application = context.Accounts.Find(applicationId);
                application.CancelOrRejectComment = comments;
                application.StatusId = action == "R" ? 3 : 4;
                application.CancelOrRejectDate = DateTime.Now;
                application.CancelledORRejectBy = CurrentUser.Username;
                string msg = action == "R" ? "Rejected application-" + application.ReferenceNo : "Cancelled application-" + application.ReferenceNo;
                Utilities.LogActivity(CurrentUser.Username, msg + " Message: " + comments);

                context.SaveChanges();
                if (!string.IsNullOrEmpty(notifyApplicant) && string.Equals(notifyApplicant, "on", StringComparison.CurrentCultureIgnoreCase))
                {
                    //send message
                }
                return RedirectToAction("ApplicantProfile", new { _refNumber = application.Id.ToString().Encrypt(), message = ("action completed successfully").Encrypt() });

            }

        }

        public ActionResult Settings(string message = null)
        {
            if (!string.IsNullOrEmpty(message))
            {
                ViewBag.Message = message.Decrypt();
            }
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult AddLoopUpItem(int addOrderId, string addName = null, HttpPostedFileBase ImageUrl = null)
        {
            using (var context = new DBLAccountOpeningContext())
            {
                if (addOrderId == 1)
                {
                    //banks
                    if (context.Banks.Any(x => x.Name.Trim() == addName.Trim() && x.IsActive))
                    {
                        return RedirectToAction("Settings", new { message = "Duplicate item".Encrypt(), TabId = 0 });
                    }
                    context.Banks.Add(new Bank {
                        Code = null,
                        IsActive = true,
                        IsCollectionBank = false,
                        Name = addName
                    });
                    context.SaveChanges();
                    return RedirectToAction("Settings", new { message = "New bank added".Encrypt(), TabId = 0 });

                }
                return RedirectToAction("Settings");

            }
        }

        public ActionResult DeleteItem(int order, int itemId)
        {
            using (var context=new DBLAccountOpeningContext())
            {
                if (order == 1)
                {
                    var bank = context.Banks.Find(itemId);
                    if (bank != null)
                    {
                        bank.IsActive = false;
                        context.SaveChanges();
                        return RedirectToAction("Settings", new { message = "bank deleted".Encrypt(), TabId = 0 });
                    }
                }

                return RedirectToAction("Settings");
            }
        }
        [HttpPost,ValidateAntiForgeryToken]
        public ActionResult EditLookUpItem(int editOrderId,int editItemId,string editName=null,HttpPostedFileBase EditImageUrl=null)
        {
            using (var context=new DBLAccountOpeningContext())
            {
                if (editOrderId == 1)
                {
                    var find = context.Banks.Find(editItemId);
                    if (find!=null)
                    {
                        find.Name = editName;
                    }
                    context.SaveChanges();
                    return RedirectToAction("Settings", new { message = "item edited".Encrypt(), TabId = 0 });

                }
                return RedirectToAction("Settings");

            }
        }
        public ActionResult ReviewSuccessfully(Guid _ref)
        {
            //DBLSoftTechServiceReference.ClientOpeningWSClient objPayRef = new DBLSoftTechServiceReference.ClientOpeningWSClient();
            //objPayRef.Open();
            //objPayRef.openClient(new DBLSoftTechServiceReference.clientAofApiBean
            //{


            //});
            //  <return>Client Code IS: 32305-000001</return>

            try
            {
                using (var context = new DBLAccountOpeningContext())
                {
                    var application = context.Accounts.Find(_ref);
                    var applicationId = _ref;
                    var basicProfile = Utilities.GetApplications(0, 0, null, applicationId.ToString()).FirstOrDefault();

                    if (application.AccountTypeId <= 3)
                    {
                        var accountAuthorisedPerson = Utilities.GetAccountAuthorisedPersons(applicationId).OrderBy(x => x.CreatedDate).FirstOrDefault();
                        //  var accountETI = Utilities.GetAccountETI(applicationId);
                        //  var accountUploads = Utilities.GetAccountFilesUploads(applicationId);
                        var accountFinancialInvestmentRiskProfile = Utilities.GetAccountInvestmentRiskProfile(applicationId);
                        //  var accountCustodyDetails = Utilities.GetAccountCustodyDetails(applicationId);
                        //  var accountTradingContacts = Utilities.GetAccountTradingContract(applicationId);
                        var accountInstructionEmploymentDetails = Utilities.GetAccountInstructionEmploymentDetailModel(applicationId);
                        // var accountSignatories = Utilities.GetAccountSignatories(applicationId);
                        var accountMember = Utilities.GetAccountMembers(applicationId).FirstOrDefault();
                        var accountNextOfKins = Utilities.GetAccountNextOfKins(applicationId);
                        var accountSettlementDetails = Utilities.GetAccountSettlementDetail(applicationId);


                        
                        //ind, single or itf
                        using (var client = new AofSdkClient())
                        {
                            var result = client.CreateAccount(
                        new openClientRequest
                        {
                            arg0 = new clientAofApiBean
                            {
                                accStatmentFreq = basicProfile.FrequencyOfStatementsName,
                                appId = null,
                                accountType=basicProfile.AccountTypeName,
                                applicantObj = null,
                                approximateAnnualIncome = accountFinancialInvestmentRiskProfile.ApproximateAnnualIncomeName,
                                authPersonCity = accountAuthorisedPerson != null ? accountAuthorisedPerson.City : accountMember.MailingAddressCity,
                                authPersonCountry = accountAuthorisedPerson != null ? accountAuthorisedPerson.CountryName : accountMember.NationalityName,
                                authPersonEmail = accountAuthorisedPerson != null ? accountAuthorisedPerson.Email : accountMember.Email,
                                authPersonFax = accountAuthorisedPerson != null ? accountAuthorisedPerson.Fax : accountMember.Fax,
                                authPersonFullName = accountAuthorisedPerson != null ? accountAuthorisedPerson.Name : accountMember.Fname +" "+ accountMember.Lname,
                                authPersonMailingAdd = accountAuthorisedPerson != null ? accountAuthorisedPerson.MailingAddress : accountMember.MailingAddressFull,
                                authPersonMob = accountAuthorisedPerson != null ? accountAuthorisedPerson.Mobile : accountMember.Mobile,
                                authPersonNicExpDate = accountAuthorisedPerson != null && !string.IsNullOrEmpty(accountAuthorisedPerson.IDExpiryDate) ? Utilities.SoftTechDateFormatter(accountAuthorisedPerson.IDExpiryDate) : string.Empty,
                                authPersonNicIssueAuthority = accountAuthorisedPerson != null ? accountAuthorisedPerson.IdIssueAuthority : accountMember.IssuingAuthority,
                                authPersonNicIssueDate = accountAuthorisedPerson != null && !string.IsNullOrEmpty(accountAuthorisedPerson.IssueDate) ? Utilities.SoftTechDateFormatter(accountAuthorisedPerson.IssueDate) : string.Empty,
                                authPersonNicPhotoId = accountAuthorisedPerson != null ? accountAuthorisedPerson.IdNumber : accountMember.IdNumber,
                                authPersonRelation = accountAuthorisedPerson != null ? accountAuthorisedPerson.RelationToAccountHolder : "Self",
                                authPersonTel = accountAuthorisedPerson != null ? accountAuthorisedPerson.Tel : accountMember.Mobile,
                                authPersonUploadNicImgID = null,
                                authPersonZipCode = accountAuthorisedPerson != null ? accountAuthorisedPerson.ZipCode : accountMember.MailingAddressZipCode,
                                branch =!string.IsNullOrEmpty(basicProfile.BranchCode)? basicProfile.BranchCode:Utilities.GetRandomBranchCode(),
                                cityAdd = accountMember.MailingAddressCity,
                                commBankAccName = accountSettlementDetails.BankName,
                                commBankAccNo = accountSettlementDetails.AccountNumber,
                                commBankBranch = accountSettlementDetails.Branch,
                                commBankSelection = null,
                                countryAdd = accountMember.NationalityName,
                                courtConviction = basicProfile.DeclarationConvictedOfLaw,
                                courtConvictionDetail = basicProfile.DeclarationConvictedOfLawDetails,
                                currentEmployer = accountInstructionEmploymentDetails.CurrentEmployer,
                                currentEmployerAdd = accountInstructionEmploymentDetails.CurrentEmployerAddress,
                                currentOccupation = accountInstructionEmploymentDetails.CurrentOccupation,
                                declaration = basicProfile.DeclarationIWe,
                                dob = Utilities.SoftTechDateFormatter(accountMember.DOB), //DateTime.ParseExact(Utilities.SoftTechDateFormatter(accountMember.DOB), "dd-MM-yyyy", null).Date,
                              //  dobSpecified = true,
                                email = accountMember.Email,
                                employmentFromDate = !string.IsNullOrEmpty(accountInstructionEmploymentDetails.EmploymentDateFrom) ? Utilities.SoftTechDateFormatter(accountInstructionEmploymentDetails.EmploymentDateFrom) : string.Empty,
                                employmentStatus = accountInstructionEmploymentDetails.EmploymentStatusName,
                                employmentToDate = !string.IsNullOrEmpty(accountInstructionEmploymentDetails.EmploymentDateTo) ? Utilities.SoftTechDateFormatter(accountInstructionEmploymentDetails.EmploymentDateTo) : string.Empty,
                                expDate = !string.IsNullOrEmpty(accountMember.IdCardExpiryDate) ? Utilities.SoftTechDateFormatter(accountMember.IdCardExpiryDate) : string.Empty,
                                expectedAccActivity = basicProfile.ExpectedAccountActivityName,
                                fax = basicProfile.TIN,
                                firstKinEmail = accountNextOfKins.FirstOrDefault().Email,
                                firstKinFaxNum = accountNextOfKins.FirstOrDefault().Fax,
                                firstKinMailingAdd = accountNextOfKins.FirstOrDefault().MailingAddress,
                                firstKinMobNum = accountNextOfKins.FirstOrDefault().Mobile,
                                firstKinName = accountNextOfKins.FirstOrDefault().Name,
                                firstKinRelation = accountNextOfKins.FirstOrDefault().RelationToAccount,
                                firstKinTelNum = accountNextOfKins.FirstOrDefault().Telephone,
                                firstName = accountMember.Fname,
                                gender = accountMember.Gender,
                                incomeFundSource = accountInstructionEmploymentDetails.SourceOfIncomeNamesList,
                                investmentHorizon = accountFinancialInvestmentRiskProfile.InvestmentHorizonName,
                                investmentKnowledge = accountFinancialInvestmentRiskProfile.InvestmentKnowledgeName,
                                investmentType = basicProfile.InvestmentTypeName,
                                issueDate = !string.IsNullOrEmpty(accountMember.IdCardIssueDate) ? Utilities.SoftTechDateFormatter(accountMember.IdCardIssueDate) : string.Empty,
                                madenName = accountMember.MaidenName,
                                mailingAdd =accountMember.NationalityName+"," +accountMember.MailingAddressFull,
                                mailingAddCity = accountMember.MailingAddressCity,
                                mailingAddCountry = accountMember.MailingAddressCountryName,
                                mailingAddZipCode = accountMember.MailingAddressZipCode,
                                maritialStatus = accountMember.MaritalStatusName,
                                mobileNum = accountMember.Mobile,
                                modeOfInstruction = accountInstructionEmploymentDetails.ModeOfInstructionName,
                                modeOfNotification = accountInstructionEmploymentDetails.ModeOfNotificationName,
                                motherMadenName = accountMember.MothersMaidenName,
                                nationality = accountMember.NationalityName,
                                netWorth = accountFinancialInvestmentRiskProfile.NetWorthName,
                                nicIssueAuth = accountMember.IssuingAuthority,
                                nicNum = accountMember.IdNumber,
                                nicPhotoIdType = accountMember.IdTypeName,
                                nomineeTrust = basicProfile.DeclarationActingAsNominee,
                                nomineeTrustName = basicProfile.DeclarationActingAsNomineeName,
                                occupation = accountMember.Occupation,
                                onlineTradingFac = accountSettlementDetails.OnlineTradingFacility,
                                otherName = accountMember.Othername,
                                previousEmployer = accountInstructionEmploymentDetails.PrevEmployer,
                                previousOccupation = accountInstructionEmploymentDetails.PrevOccupation,
                                residentialAdd =!string.IsNullOrEmpty(accountMember.ResidentialAddressFull)?accountMember.ResidentialAddressFull:accountMember.NationalityName+" "+accountMember.PlaceOfBirth,
                                riskTolerence = accountFinancialInvestmentRiskProfile.RiskToleranceName,
                                secKinEmail = accountNextOfKins.Count() > 1 ? accountNextOfKins.LastOrDefault().Email : accountNextOfKins.FirstOrDefault().Email,
                                secKinFax = accountNextOfKins.Count() > 1 ? accountNextOfKins.LastOrDefault().Fax : accountNextOfKins.FirstOrDefault().Fax,
                                secKinFullName = accountNextOfKins.Count() > 1 ? accountNextOfKins.LastOrDefault().Name : accountNextOfKins.FirstOrDefault().Name,
                                secKinMailingAdd = accountNextOfKins.Count() > 1 ? accountNextOfKins.LastOrDefault().MailingAddress : accountNextOfKins.FirstOrDefault().MailingAddress,
                                secKinMobNum = accountNextOfKins.Count() > 1 ? accountNextOfKins.LastOrDefault().Mobile : accountNextOfKins.FirstOrDefault().Mobile,
                                secKinRelation = accountNextOfKins.Count() > 1 ? accountNextOfKins.LastOrDefault().RelationToAccount : accountNextOfKins.FirstOrDefault().RelationToAccount,
                                secKinTelNum = accountNextOfKins.Count() > 1 ? accountNextOfKins.LastOrDefault().Telephone : accountNextOfKins.FirstOrDefault().Telephone,
                                signImgId = null,
                                signNum = null,
                                srName = accountMember.Lname,
                                suffix = accountMember.SelectWhereApplicableName,
                                swiftSortCode = accountSettlementDetails.SwiftCode,
                                taxIdNo = null,
                                tellephoneNum = accountMember.Telephone,
                                title = accountMember.TitleName,
                                totEmploymentYear = accountInstructionEmploymentDetails.YearsOfEmployment.HasValue ? accountInstructionEmploymentDetails.YearsOfEmployment.ToString() : string.Empty,
                                uploadImgId = null,
                                zipCode = accountMember.ResidentialZipCode,
                                appLocalForeign=accountMember.NationalityId==81?"0":"1"
                                
                                
                                 
                            }
                        });


                            var response = result.Message;
                            if (result.Status== "500")
                            {
                              //  success
                              //  var responseArr = response.Split(':');
                                application.Reviewer = CurrentUser.Username;
                                application.SuccessfulReviewDate = DateTime.Now;
                                application.BackConnectAccountNumber = result.Message;
                                application.StatusId = 2;
                                Utilities.LogActivity(CurrentUser.Username,"Successful review-"+application.ReferenceNo);
                                context.SaveChanges();

                            }
                            else
                            {
                                return RedirectToAction("ApplicantProfile", new { _refNumber = application.Id.ToString().Encrypt(), message = ("Error in SoftTech API: "+response).Encrypt() });

                            }
                        }
                    }



                    else if (application.AccountTypeId == 4)
                    {
                        ///institutional
                        var accountAuthorisedPersons = Utilities.GetAccountAuthorisedPersons(applicationId);
                        var authorsedOfficer1 = accountAuthorisedPersons.FirstOrDefault();
                        var authorsedOfficer2 = accountAuthorisedPersons.LastOrDefault();
                        var accountSettlementDetails = Utilities.GetAccountSettlementDetail(applicationId);
                        var accountTradingContacts = Utilities.GetAccountTradingContract(applicationId);
                        var tradingContact1 = accountTradingContacts.FirstOrDefault();
                        var accountInstructionEmploymentDetails = Utilities.GetAccountInstructionEmploymentDetailModel(applicationId);


                        using (var client = new AofSdkClient())
                        {
                            var result = client.CreateAccount(
                        new openClientRequest
                        {
                            arg0 = new clientAofApiBean
                            {
                                accStatmentFreq = basicProfile.FrequencyOfStatementsName,
                                appId = null,
                                accountType = "Corporate",// basicProfile.AccountTypeName,
                                applicantObj = null,
                                approximateAnnualIncome =context.ApproximateAnnualIncomes.OrderByDescending(x=>x.Id).FirstOrDefault().Name,//null,// accountFinancialInvestmentRiskProfile.ApproximateAnnualIncomeName,
                                authPersonCity =authorsedOfficer1.CountryName+" " +authorsedOfficer1.City,//accountAuthorisedPerson != null ? accountAuthorisedPerson.City : accountMember.MailingAddressCity,
                                authPersonCountry = authorsedOfficer1.CountryName,//accountAuthorisedPerson != null ? accountAuthorisedPerson.CountryName : accountMember.NationalityName,
                                authPersonEmail = authorsedOfficer1.Email,//accountAuthorisedPerson != null ? accountAuthorisedPerson.Email : accountMember.Email,
                                authPersonFax = authorsedOfficer1.Fax, //accountAuthorisedPerson != null ? accountAuthorisedPerson.Fax : accountMember.Fax,
                                authPersonFullName = authorsedOfficer1.Name, //accountAuthorisedPerson != null ? accountAuthorisedPerson.Name : accountMember.Fname + " " + accountMember.Lname,
                                authPersonMailingAdd =string.IsNullOrEmpty(authorsedOfficer1.MailingAddress)?authorsedOfficer1.CountryName:authorsedOfficer1.MailingAddress, //accountAuthorisedPerson != null ? accountAuthorisedPerson.MailingAddress : accountMember.MailingAddressFull,
                                authPersonMob = authorsedOfficer1.Mobile,//accountAuthorisedPerson != null ? accountAuthorisedPerson.Mobile : accountMember.Mobile,
                                authPersonNicExpDate = authorsedOfficer1 != null && !string.IsNullOrEmpty(authorsedOfficer1.IDExpiryDate) ? Utilities.SoftTechDateFormatter(authorsedOfficer1.IDExpiryDate) : string.Empty,
                                authPersonNicIssueAuthority = authorsedOfficer1.IdIssueAuthority,//accountAuthorisedPerson != null ? accountAuthorisedPerson.IdIssueAuthority : accountMember.IssuingAuthority,
                                authPersonNicIssueDate =authorsedOfficer1 != null && !string.IsNullOrEmpty(authorsedOfficer1.IssueDate) ? Utilities.SoftTechDateFormatter(authorsedOfficer1.IssueDate) : string.Empty,
                                authPersonNicPhotoId = authorsedOfficer1.IdNumber, //accountAuthorisedPerson != null ? accountAuthorisedPerson.IdNumber : accountMember.IdNumber,
                                authPersonRelation = authorsedOfficer1.RelationToAccountHolder, //accountAuthorisedPerson != null ? accountAuthorisedPerson.RelationToAccountHolder : "Self",
                                authPersonTel = authorsedOfficer1.Tel, //accountAuthorisedPerson != null ? accountAuthorisedPerson.Tel : accountMember.Mobile,
                                authPersonUploadNicImgID = null,
                                authPersonZipCode = authorsedOfficer1.ZipCode,//accountAuthorisedPerson != null ? accountAuthorisedPerson.ZipCode : accountMember.MailingAddressZipCode,
                                branch = !string.IsNullOrEmpty(basicProfile.BranchCode) ? basicProfile.BranchCode : Utilities.GetRandomBranchCode(),
                                cityAdd = authorsedOfficer1.City, //accountMember.MailingAddressCity,
                                commBankAccName = context.Banks.FirstOrDefault().Name,//accountSettlementDetails.BankName, //accountSettlementDetails.BankName,
                                commBankAccNo =accountSettlementDetails.AccountNumber,
                                commBankBranch = accountSettlementDetails.Branch,
                                commBankSelection = null,
                                countryAdd = authorsedOfficer1.CountryName,
                                courtConviction ="NO", //basicProfile.DeclarationConvictedOfLaw,
                                courtConvictionDetail =null,//"NA", //basicProfile.DeclarationConvictedOfLawDetails,
                                currentEmployer = "Not Applicable",//accountInstructionEmploymentDetails.CurrentEmployer,
                                currentEmployerAdd ="Not Available",// accountInstructionEmploymentDetails.CurrentEmployerAddress,
                                currentOccupation ="None",// accountInstructionEmploymentDetails.CurrentOccupation,
                                declaration = basicProfile.InstitutionClientName,
                                dob = Utilities.SoftTechDateFormatter(DateTime.Now.AddYears(-10).Date.ToString("yyyy-MM-dd")),// Utilities.SoftTechDateFormatter(accountMember.DOB), //DateTime.ParseExact(Utilities.SoftTechDateFormatter(accountMember.DOB), "dd-MM-yyyy", null).Date,
                                                                                          //  dobSpecified = true,
                                email = basicProfile.InsStreetAddressEmail,
                                employmentFromDate =null, //!string.IsNullOrEmpty(accountInstructionEmploymentDetails.EmploymentDateFrom) ? Utilities.SoftTechDateFormatter(accountInstructionEmploymentDetails.EmploymentDateFrom) : string.Empty,
                                employmentStatus = "Salaried", //accountInstructionEmploymentDetails.EmploymentStatusName,
                                employmentToDate = null,//!string.IsNullOrEmpty(accountInstructionEmploymentDetails.EmploymentDateTo) ? Utilities.SoftTechDateFormatter(accountInstructionEmploymentDetails.EmploymentDateTo) : string.Empty,
                                expDate = null,//!string.IsNullOrEmpty(accountMember.IdCardExpiryDate) ? Utilities.SoftTechDateFormatter(accountMember.IdCardExpiryDate) : string.Empty,
                                expectedAccActivity = basicProfile.ExpectedAccountActivityName,
                                fax = basicProfile.TIN,
                                firstKinEmail =!string.IsNullOrEmpty(tradingContact1.Email)? tradingContact1.Email:authorsedOfficer1.Email,//accountNextOfKins.FirstOrDefault().Email,
                                firstKinFaxNum =!string.IsNullOrEmpty(tradingContact1.Fax)? tradingContact1.Fax:authorsedOfficer1.Fax,//tradingContact1.FirstOrDefault().Fax,
                                firstKinMailingAdd =null,// basicProfile.InstStreetAddressCity, //tradingContact1.FirstOrDefault().MailingAddress,
                                firstKinMobNum =!string.IsNullOrEmpty(tradingContact1.Mobile)? tradingContact1.Mobile:authorsedOfficer1.Mobile, //accountNextOfKins.FirstOrDefault().Mobile,
                                firstKinName =!string.IsNullOrEmpty(tradingContact1.Name)? tradingContact1.Name:authorsedOfficer1.Name,//accountNextOfKins.FirstOrDefault().Name,
                                firstKinRelation =authorsedOfficer1.RelationToAccountHolder,// "Brother", //accountNextOfKins.FirstOrDefault().RelationToAccount,
                                firstKinTelNum = tradingContact1.Tel, //accountNextOfKins.FirstOrDefault().Telephone,
                                firstName = basicProfile.InstitutionClientName,//accountMember.Fname,
                                gender = "Male",//accountMember.Gender,
                                incomeFundSource = accountInstructionEmploymentDetails.SourceOfIncomeNamesList, //accountInstructionEmploymentDetails.SourceOfIncomeName,
                                investmentHorizon =context.InvestmentHorizons.FirstOrDefault().Name,//null,// accountFinancialInvestmentRiskProfile.InvestmentHorizonName,
                                investmentKnowledge =context.InvestmentKnowledges.FirstOrDefault().Name,//null, //accountFinancialInvestmentRiskProfile.InvestmentKnowledgeName,
                                investmentType = basicProfile.InvestmentTypeName,
                                issueDate = Utilities.SoftTechDateFormatter(DateTime.Now.AddYears(-10).Date.ToString("yyyy-MM-dd")), //!string.IsNullOrEmpty(accountMember.IdCardIssueDate) ? Utilities.SoftTechDateFormatter(accountMember.IdCardIssueDate) : string.Empty,
                                madenName = null,//accountMember.MaidenName,
                                mailingAdd = basicProfile.InsstitutionalCountryOfIncorporationName+" "+basicProfile.MailingAddressFull,//accountMember.NationalityName + "," + accountMember.MailingAddressFull,
                                mailingAddCity = basicProfile.InstStreetAddressCity,//accountMember.MailingAddressCity,
                                mailingAddCountry =basicProfile.InsstitutionalCountryOfIncorporationName, //accountMember.MailingAddressCountryName,
                                mailingAddZipCode =basicProfile.InsStreetAddressZipCode, //accountMember.MailingAddressZipCode,
                                maritialStatus = "Single",//accountMember.MaritalStatusName,
                                mobileNum = basicProfile.InsStreetAddressTel,//accountMember.Mobile,
                                modeOfInstruction = accountInstructionEmploymentDetails.ModeOfInstructionName,
                                modeOfNotification = accountInstructionEmploymentDetails.ModeOfNotificationName,
                                motherMadenName ="None",// accountMember.MothersMaidenName,
                                nationality =basicProfile.InsstitutionalCountryOfIncorporationName,// accountMember.NationalityName,
                                netWorth = context.NetWorths.FirstOrDefault().Name,//accountFinancialInvestmentRiskProfile.NetWorthName,
                                nicIssueAuth =authorsedOfficer1.IdIssueAuthority, //accountMember.IssuingAuthority,
                                nicNum =authorsedOfficer1.IdNumber,// accountMember.IdNumber,
                                nicPhotoIdType =authorsedOfficer1.IdTypeName, //accountMember.IdTypeName,
                                nomineeTrust = "NO", //basicProfile.DeclarationActingAsNominee,
                                nomineeTrustName = basicProfile.DeclarationActingAsNomineeName,
                                occupation ="Not Applicable", //accountMember.Occupation,
                                onlineTradingFac = accountSettlementDetails.OnlineTradingFacility,
                                otherName =null, //basicProfile.InstitutionNatureOfBusiness.ToLower(),//accountMember.Othername,
                                previousEmployer = "Self",//accountInstructionEmploymentDetails.PrevEmployer,
                                previousOccupation ="Not Applicable",// accountInstructionEmploymentDetails.PrevOccupation,
                                residentialAdd =basicProfile.InsstitutionalCountryOfIncorporationName+" "+ basicProfile.StreetAddressFull, //accountMember.ResidentialAddressFull,
                                riskTolerence =context.RiskTolerances.FirstOrDefault().Name, //accountFinancialInvestmentRiskProfile.RiskToleranceName,
                                secKinEmail = accountAuthorisedPersons.Count() > 1 ? accountAuthorisedPersons.LastOrDefault().Email : accountAuthorisedPersons.FirstOrDefault().Email,
                                secKinFax = accountAuthorisedPersons.Count() > 1 ? accountAuthorisedPersons.LastOrDefault().Fax : accountAuthorisedPersons.FirstOrDefault().Fax,
                                secKinFullName = accountAuthorisedPersons.Count() > 1 ? accountAuthorisedPersons.LastOrDefault().Name : accountAuthorisedPersons.FirstOrDefault().Name,
                                secKinMailingAdd = accountAuthorisedPersons.Count() > 1 ? accountAuthorisedPersons.LastOrDefault().MailingAddress : accountAuthorisedPersons.FirstOrDefault().MailingAddress,
                                secKinMobNum = accountAuthorisedPersons.Count() > 1 ? accountAuthorisedPersons.LastOrDefault().Mobile : accountAuthorisedPersons.FirstOrDefault().Mobile,
                                secKinRelation = accountAuthorisedPersons.Count() > 1 ? accountAuthorisedPersons.LastOrDefault().RelationToAccountHolder : accountAuthorisedPersons.FirstOrDefault().RelationToAccountHolder,
                                secKinTelNum = accountAuthorisedPersons.Count() > 1 ? accountAuthorisedPersons.LastOrDefault().Tel : accountAuthorisedPersons.FirstOrDefault().Tel,
                                signImgId = null,
                                signNum = null,
                                srName =" ", // basicProfile.InstitutionClientName,//accountMember.Lname,
                                suffix =basicProfile.InstCompanyType, //accountMember.SelectWhereApplicableName,
                                swiftSortCode = accountSettlementDetails.SwiftCode,
                                taxIdNo = null,
                                tellephoneNum = basicProfile.InsStreetAddressTel,//accountMember.Telephone,
                                title ="Mr.",// null,//accountMember.TitleName,
                                totEmploymentYear = null,//accountInstructionEmploymentDetails.YearsOfEmployment.HasValue ? accountInstructionEmploymentDetails.YearsOfEmployment.ToString() : string.Empty,
                                uploadImgId = null,
                                zipCode = basicProfile.InsStreetAddressZipCode,
                                appLocalForeign = basicProfile.InsstitutionalCountryOfIncorporation.Value == 81 ? "0" : "1"


                            }
                        });


                            var response = result.Message;
                            if (result.Status == "500")
                            {
                                //  success
                                //  var responseArr = response.Split(':');
                                // application.SuccesfulReviewBy = CurrentUser.Id;
                                application.Reviewer = CurrentUser.Username;
                                application.SuccessfulReviewDate = DateTime.Now;
                                application.BackConnectAccountNumber = result.Message;
                                application.StatusId = 2;
                                Utilities.LogActivity(CurrentUser.Username, "Successful review-" + application.ReferenceNo);

                                context.SaveChanges();

                            }
                            else
                            {
                                return RedirectToAction("ApplicantProfile", new { _refNumber = application.Id.ToString().Encrypt(), message = ("Error in SoftTech API: " + response).Encrypt() });

                            }
                        }

                    }
                    return RedirectToAction("ApplicantProfile", new { _refNumber = application.Id.ToString().Encrypt(), message = ("application successfully reviewed").Encrypt() });

                }
            }
            catch (Exception ex)
            {

                return RedirectToAction("ApplicantProfile", new { _refNumber = _ref.ToString().Encrypt(), message = ("Error: "+ex.Message).Encrypt() });

            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult ManualCardValidation(Guid applicationId, string recordId, string objectType, string comments)
        {
            using (var context = new DBLAccountOpeningContext())
            {
                var application = context.Accounts.Find(applicationId);

                if (objectType == "AccMember")
                {
                    var model = context.AccountMembers.FirstOrDefault(x => x.Id.ToString() == recordId);
                    if (model != null && !model.IdValidated)
                    {
                        model.IdValidated = true;
                        model.IdValidationDate = DateTime.Now;
                        model.IdValidationBy = (Utilities.GetSessionUser() as AppUser).Email;
                        model.IdValidationMode = "MANUAL";
                        model.ManualValidationComment = comments;

                    }
                }
                else if (objectType == "AuthPerson")
                {
                    var model = context.AccountAuthorisedPersons.FirstOrDefault(x => x.Id.ToString() == recordId);
                    if (model != null && !model.IdValidated)
                    {
                        model.IdValidated = true;
                        model.IdValidationDate = DateTime.Now;
                        model.IdValidationBy = (Utilities.GetSessionUser() as AppUser).Email;
                        model.IdValidationMode = "MANUAL";
                        model.ManualValidationComment = comments;
                    }
                }
                context.SaveChanges();
                return RedirectToAction("ApplicantProfile", new { _refNumber = application.Id.ToString().Encrypt(), message=("Id verification successful.").Encrypt() });
            }

        }


    }
}