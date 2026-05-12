using AppLogger;
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
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
        public AdminController()
        {
            if (Utilities.AppUsers == null)
            {
                Utilities.AppUsers = AppServerHelper.GetAppUsers();
            }
        }

        public string GetColor(int index)
        {

            var colArray = new List<string> { "#F08080", "#006400", "#1E90FF", "#0B3B39", "#000080", "#DA70D6", "#61210B",
                "#FFDAB9", "#F0E68C", "#2E8B57", "#808000","#556B2F" };
            var rnd = new Random();
            return colArray[index];
        }
        public ActionResult Dashboard()
        {
            using (var context = new DBLAccountOpeningContext())
            {
                var investmentTypesData = new List<NameValueModel>();
                var statusTypesData = new List<NameValueModel>();
                int colIndex = 0;
                foreach (var investmentType in context.InvestmentTypes.Where(x => x.IsActive))
                {
                    var accounts = investmentType.Accounts.Where(x => x.InvestmentTypeId == investmentType.Id);
                    investmentTypesData.Add(new NameValueModel
                    {
                        DeciVal = 0,
                        IntVal = accounts.Count(),
                        Label = investmentType.Name,
                        Color = GetColor(colIndex)
                    });
                    colIndex++;

                }

                colIndex = 0;
                foreach (var status in context.ApplicationStatus.Where(x => x.IsActive))
                {
                    var accounts = status.Accounts.Where(x => x.StatusId == status.Id);
                    statusTypesData.Add(new NameValueModel
                    {
                        DeciVal = 0,
                        IntVal = accounts.Count(),
                        Label = status.Name,
                        Color = GetColor(colIndex)
                    });
                    colIndex++;
                }
                ViewBag.investmentTypesData = investmentTypesData;
                ViewBag.statusTypesData = statusTypesData;
            }

            return View();
        }
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


        public ActionResult ForgottenAccount(string message = null)
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
            using (var context = new DBLAccountOpeningContext())
            {
                var model = context.ForgottenAccountDetails.Find(itemId);
                return View(model);
            }
        }
        [HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult SendAccountDetailsResponse(Guid itemId, int responseType, string response)
        {
            using (var context = new DBLAccountOpeningContext())
            {
                var model = context.ForgottenAccountDetails.Find(itemId);
                if (model != null)
                {
                    model.StatusId = 2;
                    context.SaveChanges();
                    string message = string.Empty;
                    if (responseType == 1)
                    {
                        //account found
                        message = "Dear " + model.Name + ",<br/>Thank you for getting  in touch with  Databank.<br/>Please below are your brokerage account  details:<br/>" + response + "<br/><br/>Thank you for choosing  databank.";
                    }
                    else if (responseType == 2)
                    {
                        message = "Dear " + model.Name + "<br/>" + response + "<br/> Kindly call our team on 0302610610";
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


        [HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult SendEditRequest(Guid itemId, string errorList, string dbl = null, string fileUpload = null, string aml = null, string eti = null)
        {
            var context = new DBLAccountOpeningContext();
            var accountBasic = Utilities.GetApplications(0, 0, null, itemId.ToString()).FirstOrDefault();
            var editReq = new AccountEditRequest
            {

                AccountId = itemId,
                CreatedBy = CurrentUser.Username,
                CreatedDate = DateTime.Now,
                ErrorList = errorList,
                Id = Guid.NewGuid(),
                IsActive = true,
            };

            if (!string.IsNullOrEmpty(dbl) && string.Equals(dbl, "on", StringComparison.CurrentCultureIgnoreCase))
            {
                editReq.DBLFormEnabled = true;
            }
            if (!string.IsNullOrEmpty(fileUpload) && string.Equals(fileUpload, "on", StringComparison.CurrentCultureIgnoreCase))
            {
                editReq.FileUploadEnabled = true;
            }
            if (!string.IsNullOrEmpty(aml) && string.Equals(aml, "on", StringComparison.CurrentCultureIgnoreCase))
            {
                editReq.AMLEnabled = true;
            }
            if (!string.IsNullOrEmpty(eti) && string.Equals(eti, "on", StringComparison.CurrentCultureIgnoreCase))
            {
                editReq.ETIEnabled = true;
            }


            context.AccountEditRequests.Add(editReq);
            context.SaveChanges();
            string url = ConfigurationManager.AppSettings["appUrl"].ToString();
            string sendTo = string.Empty;
            if (accountBasic.AccountTypeId <= 3)
            {
                var accountMember = Utilities.GetAccountMembers(itemId).FirstOrDefault();
                sendTo = accountMember.Email;
            }
            else if (accountBasic.AccountTypeId == 4)
            {
                sendTo = accountBasic.InsStreetAddressEmail;
            }
            // sendTo = "fohebenezer@gmail.com";
            string message = "Hello " + accountBasic.AccountName + "<br/>Thank you for getting in touch with Databank. We have received your request to create a new Brokerage account.<br/>However, the following errors were identified with your entries.<br/>Kindly use the link and credentials below to make the needed corrections and resubmit the application.<br/><br/><b>Error List</b><br/>" + errorList + "<br/> Application Reference/Username: " + accountBasic.RefNo + "<br/>Password: " + accountBasic.Password + "<br/><b>Login with the link below:<br/> <a href=" + url + ">" + url + "</b></a>" + "<br/><br/>Thank you for choosing  databank.";

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
        public string SaveFileUpload(HttpPostedFileBase file, string suggestedName = null)
        {
            Random rnd = new Random();
            string fileAppend = rnd.Next(100000, 999999).ToString() + DateTime.UtcNow.Ticks;
            string ext = Path.GetExtension(file.FileName);
            string finalFileName = !string.IsNullOrEmpty(suggestedName) ? suggestedName : Guid.NewGuid().ToString() + fileAppend + ext;
            string fileSavePath = Server.MapPath("~/Images/" + finalFileName); //create the full path
            file.SaveAs(fileSavePath);
            var saveName = finalFileName;
            return saveName;
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult UploadStaffSignature(Guid appId, HttpPostedFileBase staffSign)
        {
            using (var context = new DBLAccountOpeningContext())
            {
                var user = CurrentUser;
                var staff = context.AppUsers.FirstOrDefault(x => x.Email.Trim().ToLower() == user.Username.Trim().ToLower());
                staff.SignatureImg = SaveFileUpload(staffSign);
                context.SaveChanges();
                return RedirectToAction("ApplicantProfile", new { _refNumber = appId.ToString().Encrypt(), message = ("file upload successful").Encrypt() });

            }
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
            ViewBag.staffSign = Utilities.GetStaffSignature(CurrentUser.Username);
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult CancelOrRejectApplication(Guid applicationId, string action, string comments, string notifyApplicant)
        {
            using (var context = new DBLAccountOpeningContext())
            {
                var application = context.Accounts.Find(applicationId);
                var accountProfile = Utilities.GetApplications(0, 0, null, applicationId.ToString()).FirstOrDefault();

                application.CancelOrRejectComment = comments;
                application.StatusId = action == "R" ? 3 : 4;
                application.CancelOrRejectDate = DateTime.Now;
                application.CancelledORRejectBy = CurrentUser.Username;
                string msg = action == "R" ? "Rejected application-" + application.ReferenceNo : "Cancelled application-" + application.ReferenceNo;
                Utilities.LogActivity(CurrentUser.Username, msg + " Message: " + comments);
                string _action = action == "R" ? "Rejected" : "Cancelled";
                string emailMessage = "Dear " + accountProfile.AccountName + ", your request to open a Brokerage account at Databank has been " + _action + ".Reason is as follows:<br/> " + comments + "<br/>Contact our team on 0302610610 for further assistance";
                context.SaveChanges();
                string sendTo = string.Empty;
                if (accountProfile.AccountTypeId <= 3)
                {
                    var accountMember = Utilities.GetAccountMembers(applicationId).FirstOrDefault();
                    sendTo = accountMember.Email;
                }
                else if (accountProfile.AccountTypeId == 4)
                {
                    sendTo = accountProfile.InsStreetAddressEmail;
                }
                if (!string.IsNullOrEmpty(notifyApplicant) && string.Equals(notifyApplicant, "on", StringComparison.CurrentCultureIgnoreCase))
                {
                    //send message
                    context.MessageHistories.Add(new MessageHistory
                    {
                        CreatedDate = DateTime.Now,
                        Id = Guid.NewGuid(),
                        IsSent = false,
                        MessageContent = emailMessage,
                        SentTo = sendTo,
                        Title = "Databank Brokerage",
                        Type = "EMAIL"

                    });
                    context.SaveChanges();
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
                    context.Banks.Add(new Bank
                    {
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
            using (var context = new DBLAccountOpeningContext())
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
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult EditLookUpItem(int editOrderId, int editItemId, string editName = null, HttpPostedFileBase EditImageUrl = null)
        {
            using (var context = new DBLAccountOpeningContext())
            {
                if (editOrderId == 1)
                {
                    var find = context.Banks.Find(editItemId);
                    if (find != null)
                    {
                        find.Name = editName;
                    }
                    context.SaveChanges();
                    return RedirectToAction("Settings", new { message = "item edited".Encrypt(), TabId = 0 });

                }
                return RedirectToAction("Settings");

            }
        }

        [HttpPost, ValidateAntiForgeryToken]

        public async Task<ActionResult> ReviewSuccessfully(Guid _ref, List<int> checkList = null)
        {


            try
            {

                using (var context = new DBLAccountOpeningContext())
                {
                    if (checkList == null || !checkList.Any())
                    {
                        return RedirectToAction("ApplicantProfile", new { _refNumber = _ref.ToString().Encrypt(), message = "No checklist item selected".Encrypt() });
                    }

                    var application = context.Accounts.Find(_ref);
                    var applicationId = _ref;
                    var expectedActivity = context.AccountExpectedActivities.FirstOrDefault(x => x.AccountId == application.Id);

                    var basicProfile = Utilities.GetApplications(0, 0, null, applicationId.ToString()).FirstOrDefault();

                    if (application.AccountTypeId <= 3)
                    {
                        var accountMemberModel1 = new AccountMemberModel();
                        var accountAuthorisedPerson = Utilities.GetAccountAuthorisedPersons(applicationId).OrderBy(x => x.CreatedDate).FirstOrDefault();
                        var accountFinancialInvestmentRiskProfile = Utilities.GetAccountInvestmentRiskProfile(applicationId);
                        var accountInstructionEmploymentDetails = Utilities.GetAccountInstructionEmploymentDetailModel(applicationId);
                        var accountMember = Utilities.GetAccountMembers(applicationId).FirstOrDefault();
                        var accountNextOfKins = Utilities.GetAccountNextOfKins(applicationId);
                        var accountSettlementDetails = Utilities.GetAccountSettlementDetail(applicationId);


                        if (basicProfile.AccountTypeId == 2 || basicProfile.AccountTypeId == 3)
                            accountMemberModel1 = Utilities.GetAccountMembers(applicationId).LastOrDefault();

                        foreach (var item in checkList)
                        {
                            context.AccountCheckLists.Add(new AccountCheckList
                            {

                                AccountId = _ref,
                                CheckListItemId = item,
                                CreatedDate = DateTime.Now,
                                Id = Guid.NewGuid(),
                                IsChecked = true,

                            });
                        }
                        context.SaveChanges();
                        //ind, single or itf

                        var requestModel = new SoftTechCreateAccountModel_V2
                        {
                            ACCOUNT_TYPE = SoftTechApiFormatter.GetAccountTypeCode(basicProfile.AccountTypeId),
                            ACCT_STATMENT_FREQ = SoftTechApiFormatter.GetStatementFrequencyCode(basicProfile.FrequencyOfStatementsId.Value),
                            APPLICABLE_SUFFIX = SoftTechApiFormatter.GetCSDDepositoryParticipantOptionCode(basicProfile.SelectApplicableId.Value),
                            APPROX_ANNUAL_INCOME = accountFinancialInvestmentRiskProfile == null ? "7" : SoftTechApiFormatter.GetIncomeRangeCode(accountFinancialInvestmentRiskProfile.AnnualIncomeId.Value),
                            AUTH_PERSON_CITY = "137",
                            AUTH_PERSON_COUNTRY = SoftTechApiFormatter.GetCountryCode(accountMember.NationalityId.Value),
                            AUTH_PERSON_EMAIL = accountAuthorisedPerson != null && !string.IsNullOrEmpty(accountAuthorisedPerson.Email) ? accountAuthorisedPerson.Email : accountMember.Email,
                            AUTH_PERSON_FAX = accountAuthorisedPerson != null ? accountAuthorisedPerson.Fax : accountMember.Fax,
                            AUTH_PERSON_MAILING_ADD = accountAuthorisedPerson != null ? accountAuthorisedPerson.MailingAddress : accountMember.MailingAddressFull,
                            AUTH_PERSON_MOB_NUM = accountAuthorisedPerson != null ? accountAuthorisedPerson.Mobile : accountMember.Mobile,
                            AUTH_PERSON_NAME = accountAuthorisedPerson != null ? accountAuthorisedPerson.Name : accountMember.Fname + " " + accountMember.Lname,
                            AUTH_PERSON_NIC_EXPIRY_DATE = accountAuthorisedPerson != null && !string.IsNullOrEmpty(accountAuthorisedPerson.IDExpiryDate) ? Utilities.SoftTechDateFormatter(accountAuthorisedPerson.IDExpiryDate) : null,
                            AUTH_PERSON_NIC_IMAGE_UPLOD_ID = null,
                            AUTH_PERSON_NIC_ISSUE_ATHORITY = accountAuthorisedPerson != null ? accountAuthorisedPerson.IdIssueAuthority : accountMember.IssuingAuthority,
                            AUTH_PERSON_NIC_ISSUE_DATE = accountAuthorisedPerson != null && !string.IsNullOrEmpty(accountAuthorisedPerson.IssueDate) ? Utilities.SoftTechDateFormatter(accountAuthorisedPerson.IssueDate) : null,
                            AUTH_PERSON_NIC_NUM = accountAuthorisedPerson != null && !string.IsNullOrEmpty(accountAuthorisedPerson.IdNumber) ? accountAuthorisedPerson.IdNumber : accountMember.IdNumber,
                            AUTH_PERSON_NIC_PHOTO_ID = accountAuthorisedPerson != null && !string.IsNullOrEmpty(accountAuthorisedPerson.IdNumber) ? accountAuthorisedPerson.IdNumber : accountMember.IdNumber,
                            AUTH_PERSON_RELATION = accountAuthorisedPerson != null && !string.IsNullOrEmpty(accountAuthorisedPerson.RelationToAccountHolder) ? SoftTechApiFormatter.GetRelationshipCode(accountAuthorisedPerson.RelationToAccountHolder) : context.Relationships.FirstOrDefault().SoftTechCode, //"NOT AVAILABLE",
                            AUTH_PERSON_TEL_NUM = accountAuthorisedPerson != null ? accountAuthorisedPerson.Tel : accountMember.Mobile,
                            AUTH_PERSON_ZIP = accountAuthorisedPerson != null && Utilities.IsNumber(accountAuthorisedPerson.ZipCode) ? accountAuthorisedPerson.ZipCode : null,
                            AUTH_TITLE = "NA",
                            BANK_BRANCH = accountSettlementDetails.Branch,
                            BRANCH_CODE = "01", //!string.IsNullOrEmpty(basicProfile.BranchCode) ? SoftTechApiFormatter.GetBranchCode(basicProfile.BranchCode) : context.Branches.FirstOrDefault().SoftTechCode,
                            COMM_BANK_ACCOT_NO = accountSettlementDetails.AccountNumber,
                            COMM_BANK_ACCT_NAME = accountSettlementDetails.AccountName,
                            COMM_BANK_SELECTION = accountSettlementDetails.BankName,
                            COURT_CONVICTION = basicProfile.DeclarationConvictedOfLaw,
                            COURT_CONV_DET = basicProfile.DeclarationConvictedOfLawDetails,
                            CURRENT_EMPLOYER = accountInstructionEmploymentDetails.CurrentEmployer,
                            CURRENT_EMPLOYER_ADD = accountInstructionEmploymentDetails.CurrentEmployerAddress,
                            CURRENT_OCCUPATION = !string.IsNullOrEmpty(accountMember.Occupation) ? SoftTechApiFormatter.GetOccupationCode(accountMember.Occupation) : null,
                            DATE_OF_BIRTH = Utilities.SoftTechDateFormatter(accountMember.DOB),
                            DECLARATION = basicProfile.DeclarationIWe,
                            EMAIL = accountMember.Email,
                            EMPLOYMENT_FROM_DATE = !string.IsNullOrEmpty(accountInstructionEmploymentDetails.EmploymentDateFrom) ? Utilities.SoftTechDateFormatter(accountInstructionEmploymentDetails.EmploymentDateFrom) : null,
                            EMPLOYMENT_STATUS = accountInstructionEmploymentDetails.EmploymentStatusId.HasValue ? SoftTechApiFormatter.GetEmploymentStatusCode(accountInstructionEmploymentDetails.EmploymentStatusId.Value) : null,
                            EMPLOYMENT_TO_DATE = !string.IsNullOrEmpty(accountInstructionEmploymentDetails.EmploymentDateTo) ? Utilities.SoftTechDateFormatter(accountInstructionEmploymentDetails.EmploymentDateTo) : null,
                            EXPECTED_ACCOUNT_ACTIVITY = basicProfile.ExpectedAccountActivityId.HasValue ? SoftTechApiFormatter.GetExpectedActivityCode(basicProfile.ExpectedAccountActivityId.Value) : "E",
                            FAX_NUM = accountMember.Fax,
                            FIRST_KIN_EMAIL = accountNextOfKins.FirstOrDefault().Email,
                            FIRST_KIN_FAX_NUM = accountNextOfKins.FirstOrDefault().Fax,
                            FIRST_KIN_MAILING_ADDRESS = accountNextOfKins.FirstOrDefault().MailingAddress,
                            FIRST_KIN_MOBILE_NUM = accountNextOfKins.FirstOrDefault().Mobile,
                            FIRST_KIN_NAME = accountNextOfKins.FirstOrDefault().Name,
                            FIRST_KIN_RELATIONSHIP = !string.IsNullOrEmpty(accountNextOfKins.FirstOrDefault().RelationToAccount) ? SoftTechApiFormatter.GetRelationshipCode(accountNextOfKins.FirstOrDefault().RelationToAccount) : context.Relationships.FirstOrDefault().SoftTechCode,
                            FIRST_KIN_TEL_NUM = accountNextOfKins.FirstOrDefault().Telephone,
                            FIRST_NAME = accountMember.Fname,
                            GENDER = SoftTechApiFormatter.GetGenderCode(accountMember.Gender),
                            ID_EXPIRY_DATE = !string.IsNullOrEmpty(accountMember.IdCardExpiryDate) ? Utilities.SoftTechDateFormatter(accountMember.IdCardExpiryDate) : null,
                            ID_ISSUE_AUTHORITY = accountMember.IssuingAuthority,
                            ID_ISSUE_DATE = !string.IsNullOrEmpty(accountMember.IdCardIssueDate) ? Utilities.SoftTechDateFormatter(accountMember.IdCardIssueDate) : null,
                            ID_NUMBER = accountMember.IdNumber,
                            ID_TYPE = SoftTechApiFormatter.GetIdTypeCode(accountMember.IdTypeId.Value),
                            ID_UPLOAD_IMAGE_ID = null,
                            INCOME_FUNDS_SOURCE = SoftTechApiFormatter.GetIncomeRangeCode(accountFinancialInvestmentRiskProfile.AnnualIncomeId.Value),// !string.IsNullOrEmpty(accountInstructionEmploymentDetails.SourceOfFundsIds) ? SoftTechApiFormatter.GetIncomeCode(int.Parse(accountInstructionEmploymentDetails.SourceOfFundsIds.Split(',').FirstOrDefault())) : context.SourceOfIncomes.FirstOrDefault().SoftTechCode,
                            INVESTMENT_HORIZON = accountFinancialInvestmentRiskProfile.InvestmentHorizonName,
                            INVESTMENT_KNOWLEDGE = accountFinancialInvestmentRiskProfile.InvestmentKnowledgeName,
                            INVESTMENT_TYPE = SoftTechApiFormatter.GetInvestmentTypeCode(basicProfile.InvestmentTypeId.Value),
                            LOCAL_FOREIGN = accountMember.NationalityName.Trim().ToUpper() == "GHANA" ? "0" : "1",
                            MAIDEN_NAME = accountMember.MaidenName,
                            MAILING_ADDRESS = /*accountMember.NationalityName + "," +*/ accountMember.MailingAddressFull,
                            MAILING_CITY = context.Cities.Any(x => x.Name == "ACCRA") ? context.Cities.FirstOrDefault(x => x.Name == "ACCRA").SoftTechCode : context.Cities.FirstOrDefault().SoftTechCode,//accountMember.MailingAddressCity,
                            MAILING_COUNTRY = accountMember.MailingAddressCountryId.HasValue ? SoftTechApiFormatter.GetCountryCode(accountMember.MailingAddressCountryId.Value) : null,
                            MAILING_ZIP_CODE = accountMember.MailingAddressZipCode,
                            MARITIAL_STATUS = accountMember.MaritalStatusId.HasValue ? SoftTechApiFormatter.GetMaritalStatusCode(accountMember.MaritalStatusId.Value) : string.Empty,
                            MOBILE_NUM = accountMember.Mobile,
                            MODE_OF_INSTRUCTIONS = SoftTechApiFormatter.GetModeOfInstructionCode(accountInstructionEmploymentDetails.ModeOfInstructionId.Value),
                            MODE_OF_NOTIFICATION = SoftTechApiFormatter.GetModeOfNotificationCode(accountInstructionEmploymentDetails.ModeOfNotificationId.Value),
                            MOTHER_MAIDEN_NAME = accountMember.MothersMaidenName,
                            NATIONALITY = SoftTechApiFormatter.GetCountryCode(accountMember.NationalityId.Value),
                            NET_WORTH = SoftTechApiFormatter.GetIncomeRangeCode(accountFinancialInvestmentRiskProfile.AnnualIncomeId.Value),
                            NOMINEE_TRUST = basicProfile.DeclarationActingAsNominee,
                            NOMINEE_TRUST_NAME = basicProfile.DeclarationActingAsNomineeName,
                            OBJECTIVE = accountFinancialInvestmentRiskProfile.ObjectiveName,
                            OCCUPATION = SoftTechApiFormatter.GetOccupationCode(accountMember.Occupation),
                            ONLINE_TRD_FACILITY = "1",//accountSettlementDetails.OnlineTradingFacility,
                            OTHER_NAME = accountMember.Othername,
                            PREVIOUS_EMPLOYER = accountInstructionEmploymentDetails.PrevEmployer,
                            PREVIOUS_OCCUPATION = !string.IsNullOrEmpty(accountInstructionEmploymentDetails.PrevOccupation) ? SoftTechApiFormatter.GetOccupationCode(accountInstructionEmploymentDetails.PrevOccupation) : null,
                            RESIDENTIAL_ADDRESS = !string.IsNullOrEmpty(accountMember.ResidentialAddressFull) ? accountMember.ResidentialAddressFull : "NA",
                            RESIDENTIAL_CITY = context.Cities.Any(x => x.Name == "ACCRA") ? context.Cities.FirstOrDefault(x => x.Name == "ACCRA").SoftTechCode : context.Cities.FirstOrDefault().SoftTechCode,
                            RESIDENTIAL_COUNTRY = accountMember.ResidentialCountryId.HasValue ? SoftTechApiFormatter.GetCountryCode(accountMember.ResidentialCountryId.Value) : null,
                            RESIDENTIAL_ZIP_CODE = accountMember.ResidentialZipCode,
                            RISK_TOLERANCE = accountFinancialInvestmentRiskProfile.RiskToleranceId.HasValue ? SoftTechApiFormatter.GetRiskToleranceCode(accountFinancialInvestmentRiskProfile.RiskToleranceId.Value) : null,
                            SECOND_KIN_EMAIL = accountNextOfKins.Count() > 1 ? accountNextOfKins.LastOrDefault().Email : accountNextOfKins.FirstOrDefault().Email,
                            SECOND_KIN_FAX_NUM = accountNextOfKins.Count() > 1 ? accountNextOfKins.LastOrDefault().Fax : accountNextOfKins.FirstOrDefault().Fax,
                            SECOND_KIN_MAILING_ADDRESS = accountNextOfKins.Count() > 1 ? accountNextOfKins.LastOrDefault().MailingAddress : accountNextOfKins.FirstOrDefault().MailingAddress,
                            SECOND_KIN_MOBILE_NUM = accountNextOfKins.Count() > 1 ? accountNextOfKins.LastOrDefault().Mobile : accountNextOfKins.FirstOrDefault().Mobile,
                            SECOND_KIN_NAME = accountNextOfKins.Count() > 1 ? accountNextOfKins.LastOrDefault().Name : accountNextOfKins.FirstOrDefault().Name,
                            SECOND_KIN_RELATION = accountNextOfKins.Count() > 1 && !string.IsNullOrEmpty(accountNextOfKins.LastOrDefault().RelationToAccount) ? SoftTechApiFormatter.GetRelationshipCode(accountNextOfKins.LastOrDefault().RelationToAccount) : SoftTechApiFormatter.GetRelationshipCode(accountNextOfKins.FirstOrDefault().RelationToAccount),
                            SECOND_KIN_TEL_NUM = accountNextOfKins.Count() > 1 ? accountNextOfKins.LastOrDefault().Telephone : accountNextOfKins.FirstOrDefault().Telephone,
                            SIGNATORIES_NUM = SoftTechApiFormatter.GetSignatoryCode(basicProfile.SignatureTypeId.Value),
                            SIGNATURE_IMAGE_ID = null,
                            SUR_NAME = accountMember.Lname,
                            SWIFT_SORT_CODE = accountSettlementDetails.SwiftCode,
                            TEL_NUM = accountMember.Telephone,
                            TIN_NUM = basicProfile.TIN,
                            TOT_EMPLOYMENT_YEAR = accountInstructionEmploymentDetails.YearsOfEmployment.ToString(),
                            OTHER_TOP_UPS_ACTIVITY = null,
                            TOP_UPS_ACTIVITY = expectedActivity != null && expectedActivity.TopUpOption != null ? expectedActivity.TopUpOption.Name.Trim().ToUpper().ElementAt(0).ToString() : null,

                            PLACE_OF_BIRTH = accountMember.PlaceOfBirth,
                            REFERENCE = basicProfile.RefNo,
                            APPROVED_BY = CurrentUser.Username,
                            MAIN_CLIENT_NAME = basicProfile.AccountName,
                            MODE_OF_APPLICATION = "ONLINE",
                            INITIAL_INVESTMENT_AMOUNT = application.InitialInvestmentAmount.ToString(),
                            EMAIL_2 = accountMember.Email,

                            SECOND_KIN_FAX = accountNextOfKins.Count() > 1 ? accountNextOfKins.LastOrDefault().Fax : accountNextOfKins.FirstOrDefault().Fax,
                            AUTH_PERSON_ZIP_CODE = accountAuthorisedPerson != null && Utilities.IsNumber(accountAuthorisedPerson.ZipCode) ? accountAuthorisedPerson.ZipCode : null,
                            MODE_OF_APP = application.MODE_OF_APP.Trim(),
                            RESIDENTIAL_GHANIAN = accountMember.NationalityName.Trim().ToUpper() == "GHANA" ? "1" : "0",
                            RESIDENTIAL_FORIEGNER = accountMember.NationalityName.Trim().ToUpper() == "GHANA" ? "0" : "1",
                            COUNTRY_OF_ORIGIN = SoftTechApiFormatter.GetCountryCode(accountMember.NationalityId.Value),
                            RESIDENT_PERMIT_NO = accountMember.ResidentPermitNo,
                            PERMIT_ISSUE_DATE = accountMember != null && !string.IsNullOrEmpty(accountMember.ResidentPermitIssueDate) ? Utilities.SoftTechDateFormatter(accountMember.ResidentPermitIssueDate) : null,
                            PERMIT_EXPIRE_DATE = accountMember != null && !string.IsNullOrEmpty(accountMember.ResidentPermitExpiryDate) ? Utilities.SoftTechDateFormatter(accountMember.ResidentPermitExpiryDate) : null,
                            PLACE_OF_ISSUE = accountMember.ResidentPermitPlaceOfIssue,
                            INVESTMENT_OBJECTIVE = application.InvestmentObjective,
                            INIT_INV_AMOUNT = Convert.ToInt32(application.InitialInvestmentAmount),
                            REG_WITHDRAWAL_ACTIVITY = expectedActivity != null && expectedActivity.WithdrawalOption != null ? expectedActivity.WithdrawalOption.Name.Trim().ToUpper().ElementAt(0).ToString() : null,
                            OTHER_REG_WITHDRAWAL_ACTIVITY = null,
                            TOP_UPS_AMOUNT = expectedActivity != null && expectedActivity.TopUpOption != null ? expectedActivity.ExpectedTopUpAmt.Value.ToString() : "0",
                            REG_WITHDRAWAL_AMOUNT = expectedActivity != null && expectedActivity.WithdrawalOption != null ? expectedActivity.ExpectedWithdrawalAmt.Value.ToString() : "0",
                            NOM_MAIDEN_NAME = accountMember.MaidenName,
                            NOM_DATE_OF_BIRTH = null,
                            NOM_PLACE_OF_BIRTH = null,
                            NOM_GENDER = null,
                            NOM_COUNTRY_OF_ORIGIN = SoftTechApiFormatter.GetCountryCode(accountMember.NationalityId.Value),
                            NOM_COUNTRY_OF_RESIDENCE = SoftTechApiFormatter.GetCountryCode(accountMember.NationalityId.Value),
                            NOM_ID_TYPES = null,
                            NOM_ID_ISSUE_DATE = null, //accountAuthorisedPerson != null && !string.IsNullOrEmpty(accountAuthorisedPerson.IssueDate) ? Utilities.SoftTechDateFormatter(accountAuthorisedPerson.IssueDate) : null,

                            NOM_PLACE_OF_ISSUE = accountAuthorisedPerson != null ? accountAuthorisedPerson.IdIssueAuthority : accountMember.IssuingAuthority,

                            NOM_ID_EXPIRE_DATE = null,




                        };

                        if (basicProfile.AccountTypeId == 2 || basicProfile.AccountTypeId == 3)
                        {
                            requestModel.JOINT_NAME = accountMemberModel1.Lname;
                            requestModel.JOINT_OTHER_NAME = accountMemberModel1.Fname + " " + accountMemberModel1.Othername;
                            requestModel.JOINT_NATIONALITY = SoftTechApiFormatter.GetCountryCode(accountMemberModel1.NationalityId.Value);
                            requestModel.JOINT_DATE_OF_BIRTH = Utilities.SoftTechDateFormatter(accountMemberModel1.DOB);
                            requestModel.JOINT_TELEPHONE = accountMemberModel1.Telephone;
                            requestModel.JOINT_MOBILE = accountMemberModel1.Mobile;
                            requestModel.JOINT_FAX = accountMemberModel1.Fax;
                            requestModel.JOINT_EMAIL = accountMemberModel1.Email;
                            requestModel.JOINT_OCCUPATION = SoftTechApiFormatter.GetOccupationCode(accountMemberModel1.Occupation);
                            requestModel.JOINT_ID_TYPE = SoftTechApiFormatter.GetIdTypeCode(accountMemberModel1.IdTypeId.Value);
                            requestModel.JOINT_ID_NUMBER = accountMemberModel1.IdNumber;
                            requestModel.JOINT_ISSUE_DATE = Utilities.SoftTechDateFormatter(accountMemberModel1.IdCardIssueDate);
                            requestModel.JOINT_ISSUE_AUTHORITY = accountMemberModel1.IssuingAuthority;
                            requestModel.JOINT_EXPIRY_DATE = Utilities.SoftTechDateFormatter(accountMemberModel1.IdCardExpiryDate);
                        }

                        if (accountAuthorisedPerson != null && accountAuthorisedPerson.IdType.HasValue)
                        {
                            requestModel.AUTH_PERSON_Photo_ID_TYPE = SoftTechApiFormatter.GetIdTypeCode(accountAuthorisedPerson.IdType.Value);
                        }
                        var softTechResponse = await SoftTechApiHelper.CreateAccount(requestModel);
                        if (softTechResponse != null && softTechResponse.responseCode == "03")
                        {
                            //  success
                            //  var responseArr = response.Split(':');
                            application.Reviewer = CurrentUser.Username;
                            application.SuccessfulReviewDate = DateTime.Now;
                            string accNumber = softTechResponse.desc.Split(' ').LastOrDefault();
                            application.BackConnectAccountNumber = accNumber;
                            application.StatusId = 2;
                            Utilities.LogActivity(CurrentUser.Username, "Successful review-" + application.ReferenceNo);
                            context.SaveChanges();
                        }
                        else
                        {
                            return RedirectToAction("ApplicantProfile", new { _refNumber = application.Id.ToString().Encrypt(), message = ("Error in SoftTech API: " + softTechResponse.desc).Encrypt() });
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

                        var requestModel = new SoftTechCreateAccountModel_V2
                        {
                            ACCOUNT_TYPE = SoftTechApiFormatter.GetAccountTypeCode(basicProfile.AccountTypeId),
                            ACCT_STATMENT_FREQ = SoftTechApiFormatter.GetStatementFrequencyCode(basicProfile.FrequencyOfStatementsId.Value),
                            APPLICABLE_SUFFIX = "2",//SoftTechApiFormatter.GetCSDDepositoryParticipantOptionCode(basicProfile.SelectApplicableId.Value),
                            APPROX_ANNUAL_INCOME = null,//context.ApproximateAnnualIncomes.LastOrDefault().SoftTechCode, //SoftTechApiFormatter.GetIncomeRangeCode(accountFinancialInvestmentRiskProfile.AnnualIncomeId.Value),
                            AUTH_PERSON_CITY = "137",
                            AUTH_PERSON_COUNTRY = SoftTechApiFormatter.GetCountryCode(authorsedOfficer1.CountryId.Value),
                            AUTH_PERSON_EMAIL = authorsedOfficer1.Email,//accountAuthorisedPerson != null && !string.IsNullOrEmpty(accountAuthorisedPerson.Email) ? accountAuthorisedPerson.Email : accountMember.Email,
                            AUTH_PERSON_FAX = authorsedOfficer1.Fax,// accountAuthorisedPerson != null ? accountAuthorisedPerson.Fax : accountMember.Fax,
                            AUTH_PERSON_MAILING_ADD = string.IsNullOrEmpty(authorsedOfficer1.MailingAddress) ? authorsedOfficer1.CountryName : authorsedOfficer1.MailingAddress, // accountAuthorisedPerson != null ? accountAuthorisedPerson.MailingAddress : accountMember.MailingAddressFull,
                            AUTH_PERSON_MOB_NUM = authorsedOfficer1.Mobile,//accountAuthorisedPerson != null ? accountAuthorisedPerson.Mobile : accountMember.Mobile,
                            AUTH_PERSON_NAME = authorsedOfficer1.Name,//accountAuthorisedPerson != null ? accountAuthorisedPerson.Name : accountMember.Fname + " " + accountMember.Lname,
                            AUTH_PERSON_NIC_EXPIRY_DATE = authorsedOfficer1 != null && !string.IsNullOrEmpty(authorsedOfficer1.IDExpiryDate) ? Utilities.SoftTechDateFormatter(authorsedOfficer1.IDExpiryDate) : null,
                            AUTH_PERSON_NIC_IMAGE_UPLOD_ID = null,
                            AUTH_PERSON_NIC_ISSUE_ATHORITY = authorsedOfficer1.IdIssueAuthority,// accountAuthorisedPerson != null ? accountAuthorisedPerson.IdIssueAuthority : accountMember.IssuingAuthority,
                            AUTH_PERSON_NIC_ISSUE_DATE = authorsedOfficer1 != null && !string.IsNullOrEmpty(authorsedOfficer1.IssueDate) ? Utilities.SoftTechDateFormatter(authorsedOfficer1.IssueDate) : null,
                            AUTH_PERSON_NIC_NUM = authorsedOfficer1 != null && !string.IsNullOrEmpty(authorsedOfficer1.IdNumber) ? authorsedOfficer1.IdNumber : "NOT AVAILABLE",
                            AUTH_PERSON_NIC_PHOTO_ID = null,//authorsedOfficer1 != null && !string.IsNullOrEmpty(authorsedOfficer1.IdNumber) ? authorsedOfficer1.IdNumber : accountMember.IdNumber,
                            AUTH_PERSON_RELATION = string.IsNullOrEmpty(SoftTechApiFormatter.GetRelationshipCode(authorsedOfficer1.RelationToAccountHolder)) ? context.Relationships.FirstOrDefault().SoftTechCode : SoftTechApiFormatter.GetRelationshipCode(authorsedOfficer1.RelationToAccountHolder), //accountAuthorisedPerson != null && !string.IsNullOrEmpty(accountAuthorisedPerson.RelationToAccountHolder) ? SoftTechApiFormatter.GetRelationshipCode(accountAuthorisedPerson.RelationToAccountHolder) : context.Relationships.FirstOrDefault().SoftTechCode, //"NOT AVAILABLE",
                            AUTH_PERSON_TEL_NUM = authorsedOfficer1.Tel,//accountAuthorisedPerson != null ? accountAuthorisedPerson.Tel : accountMember.Mobile,
                            AUTH_PERSON_ZIP = authorsedOfficer1.ZipCode,//accountAuthorisedPerson != null && Utilities.IsNumber(accountAuthorisedPerson.ZipCode) ? accountAuthorisedPerson.ZipCode : null,
                            AUTH_TITLE = "NA",
                            BANK_BRANCH = accountSettlementDetails.Branch,
                            BRANCH_CODE = "01", //!string.IsNullOrEmpty(basicProfile.BranchCode) ? SoftTechApiFormatter.GetBranchCode(basicProfile.BranchCode) : context.Branches.FirstOrDefault().SoftTechCode,
                            COMM_BANK_ACCOT_NO = accountSettlementDetails.AccountNumber,
                            COMM_BANK_ACCT_NAME = accountSettlementDetails.AccountName,
                            COMM_BANK_SELECTION = accountSettlementDetails.BankName, //"01",
                            COURT_CONVICTION = basicProfile.DeclarationConvictedOfLaw,
                            COURT_CONV_DET = basicProfile.DeclarationConvictedOfLawDetails,
                            CURRENT_EMPLOYER = accountInstructionEmploymentDetails.CurrentEmployer,
                            CURRENT_EMPLOYER_ADD = accountInstructionEmploymentDetails.CurrentEmployerAddress,
                            CURRENT_OCCUPATION = null,//!string.IsNullOrEmpty(accountMember.Occupation) ? SoftTechApiFormatter.GetOccupationCode(accountMember.Occupation) : null,
                            DATE_OF_BIRTH = null,// Utilities.SoftTechDateFormatter(accountMember.DOB),
                            DECLARATION = basicProfile.DeclarationIWe,
                            EMAIL = authorsedOfficer1.Email,//accountMember.Email,
                            EMPLOYMENT_FROM_DATE = !string.IsNullOrEmpty(accountInstructionEmploymentDetails.EmploymentDateFrom) ? Utilities.SoftTechDateFormatter(accountInstructionEmploymentDetails.EmploymentDateFrom) : null,
                            EMPLOYMENT_STATUS = accountInstructionEmploymentDetails.EmploymentStatusId.HasValue ? SoftTechApiFormatter.GetEmploymentStatusCode(accountInstructionEmploymentDetails.EmploymentStatusId.Value) : null,
                            EMPLOYMENT_TO_DATE = !string.IsNullOrEmpty(accountInstructionEmploymentDetails.EmploymentDateTo) ? Utilities.SoftTechDateFormatter(accountInstructionEmploymentDetails.EmploymentDateTo) : null,
                            EXPECTED_ACCOUNT_ACTIVITY = basicProfile.ExpectedAccountActivityId.HasValue ? SoftTechApiFormatter.GetExpectedActivityCode(basicProfile.ExpectedAccountActivityId.Value) : "E",
                            FAX_NUM = authorsedOfficer1.Fax,
                            FIRST_KIN_EMAIL = !string.IsNullOrEmpty(tradingContact1.Email) ? tradingContact1.Email : authorsedOfficer1.Email,// accountNextOfKins.FirstOrDefault().Email,
                            FIRST_KIN_FAX_NUM = !string.IsNullOrEmpty(tradingContact1.Fax) ? tradingContact1.Fax : authorsedOfficer1.Fax,
                            FIRST_KIN_MAILING_ADDRESS = null,//!string.IsNullOrEmpty(tradingContact1.add) ? tradingContact1.Mobile : authorsedOfficer1.Mobile,
                            FIRST_KIN_MOBILE_NUM = !string.IsNullOrEmpty(tradingContact1.Mobile) ? tradingContact1.Mobile : authorsedOfficer1.Mobile, //accountNextOfKins.FirstOrDefault().Mobile,
                            FIRST_KIN_NAME = !string.IsNullOrEmpty(tradingContact1.Name) ? tradingContact1.Name : authorsedOfficer1.Name, //accountNextOfKins.FirstOrDefault().Name,
                            FIRST_KIN_RELATIONSHIP = string.IsNullOrEmpty(SoftTechApiFormatter.GetRelationshipCode(authorsedOfficer1.RelationToAccountHolder)) ? context.Relationships.FirstOrDefault().SoftTechCode : SoftTechApiFormatter.GetRelationshipCode(authorsedOfficer1.RelationToAccountHolder),// !string.IsNullOrEmpty(accountNextOfKins.FirstOrDefault().RelationToAccount) ? SoftTechApiFormatter.GetRelationshipCode(accountNextOfKins.FirstOrDefault().RelationToAccount) : context.Relationships.FirstOrDefault().SoftTechCode,
                            FIRST_KIN_TEL_NUM = tradingContact1.Tel, //accountNextOfKins.FirstOrDefault().Telephone,
                            FIRST_NAME = basicProfile.InstitutionClientName,// accountMember.Fname,
                            GENDER = "O",//SoftTechApiFormatter.GetGenderCode(accountMember.Gender),
                            ID_EXPIRY_DATE = !string.IsNullOrEmpty(authorsedOfficer1.IDExpiryDate) ? Utilities.SoftTechDateFormatter(authorsedOfficer1.IDExpiryDate) : null,
                            ID_ISSUE_AUTHORITY = authorsedOfficer1.IdIssueAuthority,//accountMember.IssuingAuthority,
                            ID_ISSUE_DATE = !string.IsNullOrEmpty(authorsedOfficer1.IssueDate) ? Utilities.SoftTechDateFormatter(authorsedOfficer1.IssueDate) : null,
                            ID_NUMBER = authorsedOfficer1.IdNumber,// accountMember.IdNumber,
                            ID_TYPE = SoftTechApiFormatter.GetIdTypeCode(authorsedOfficer1.IdType.Value),
                            ID_UPLOAD_IMAGE_ID = null,
                            INCOME_FUNDS_SOURCE = "5", //!string.IsNullOrEmpty(accountInstructionEmploymentDetails.SourceOfFundsIds) ? SoftTechApiFormatter.GetIncomeCode(int.Parse(accountInstructionEmploymentDetails.SourceOfFundsIds.Split(',').FirstOrDefault())) : context.SourceOfIncomes.FirstOrDefault().SoftTechCode,
                            INVESTMENT_HORIZON = null,// accountFinancialInvestmentRiskProfile.InvestmentHorizonName,
                            INVESTMENT_KNOWLEDGE = null,//accountFinancialInvestmentRiskProfile.InvestmentKnowledgeName,
                            INVESTMENT_TYPE = SoftTechApiFormatter.GetInvestmentTypeCode(basicProfile.InvestmentTypeId.Value),
                            LOCAL_FOREIGN = basicProfile.InsstitutionalCountryOfIncorporationName.Trim().ToUpper() == "GHANA" ? "0" : "1",
                            MAIDEN_NAME = null,//accountMember.MaidenName,
                            MAILING_ADDRESS = /*basicProfile.InsstitutionalCountryOfIncorporationName + " " +*/ basicProfile.MailingAddressFull, //accountMember.NationalityName + "," + accountMember.MailingAddressFull,
                            MAILING_CITY = context.Cities.Any(x => x.Name == "ACCRA") ? context.Cities.FirstOrDefault(x => x.Name == "ACCRA").SoftTechCode : context.Cities.FirstOrDefault().SoftTechCode,//accountMember.MailingAddressCity,
                            MAILING_COUNTRY = basicProfile.InsstitutionalCountryOfIncorporation.HasValue ? SoftTechApiFormatter.GetCountryCode(basicProfile.InsstitutionalCountryOfIncorporation.Value) : null,
                            MAILING_ZIP_CODE = authorsedOfficer1.ZipCode,
                            MARITIAL_STATUS = null,// SoftTechApiFormatter.GetMaritalStatusCode(accountMember.MaritalStatusId.Value),
                            MOBILE_NUM = basicProfile.InsStreetAddressTel,//accountMember.Mobile,
                            MODE_OF_INSTRUCTIONS = SoftTechApiFormatter.GetModeOfInstructionCode(accountInstructionEmploymentDetails.ModeOfInstructionId.Value),
                            MODE_OF_NOTIFICATION = SoftTechApiFormatter.GetModeOfNotificationCode(accountInstructionEmploymentDetails.ModeOfNotificationId.Value),
                            MOTHER_MAIDEN_NAME = "NA",//basicProfile.MothersMaidenName,
                            NATIONALITY = SoftTechApiFormatter.GetCountryCode(basicProfile.InsstitutionalCountryOfIncorporation.Value),
                            // NET_WORTH = context.ApproximateAnnualIncomes.OrderByDescending(x=>x.Id).FirstOrDefault().SoftTechCode,//SoftTechApiFormatter.GetIncomeRangeCode(accountFinancialInvestmentRiskProfile.AnnualIncomeId.Value),
                            NET_WORTH = "4",// SoftTechApiFormatter.GetIncomeRangeCode(accountFinancialInvestmentRiskProfile.AnnualIncomeId.Value),

                            NOMINEE_TRUST = basicProfile.DeclarationActingAsNominee,
                            NOMINEE_TRUST_NAME = basicProfile.DeclarationActingAsNomineeName,
                            OBJECTIVE = "TRADING",//accountFinancialInvestmentRiskProfile.ObjectiveName,
                            OCCUPATION = null,//SoftTechApiFormatter.GetOccupationCode(accountMember.Occupation),
                            ONLINE_TRD_FACILITY = "1",//accountSettlementDetails.OnlineTradingFacility,
                            OTHER_NAME = null,//accountMember.Othername,
                            PREVIOUS_EMPLOYER = accountInstructionEmploymentDetails.PrevEmployer,
                            PREVIOUS_OCCUPATION = null,// !string.IsNullOrEmpty(accountInstructionEmploymentDetails.PrevOccupation) ? SoftTechApiFormatter.GetOccupationCode(accountInstructionEmploymentDetails.PrevOccupation) : null,
                            RESIDENTIAL_ADDRESS = !string.IsNullOrEmpty(basicProfile.MailingAddressFull) ? basicProfile.MailingAddressFull : "NA",
                            RESIDENTIAL_CITY = context.Cities.Any(x => x.Name == "ACCRA") ? context.Cities.FirstOrDefault(x => x.Name == "ACCRA").SoftTechCode : context.Cities.FirstOrDefault().SoftTechCode,
                            RESIDENTIAL_COUNTRY = SoftTechApiFormatter.GetCountryCode(basicProfile.InsstitutionalCountryOfIncorporation.Value), //accountMember.ResidentialCountryId.HasValue ? SoftTechApiFormatter.GetCountryCode(accountMember.ResidentialCountryId.Value) : null,
                            RESIDENTIAL_ZIP_CODE = authorsedOfficer1.ZipCode,
                            RISK_TOLERANCE = "2",//accountFinancialInvestmentRiskProfile.RiskToleranceId.HasValue ? SoftTechApiFormatter.GetRiskToleranceCode(accountFinancialInvestmentRiskProfile.RiskToleranceId.Value) : null,
                            SECOND_KIN_EMAIL = accountTradingContacts.Count() > 1 ? accountTradingContacts.LastOrDefault().Email : accountTradingContacts.FirstOrDefault().Email,//tradingContact1.Email, //tradingContact1.Count() > 1 ? accountAuthorisedPersons.LastOrDefault().Email : accountAuthorisedPersons.FirstOrDefault().Email, //accountNextOfKins.Count() > 1 ? accountNextOfKins.LastOrDefault().Email : accountNextOfKins.FirstOrDefault().Email,
                            SECOND_KIN_FAX_NUM = accountTradingContacts.Count() > 1 ? accountTradingContacts.LastOrDefault().Fax : accountTradingContacts.FirstOrDefault().Fax,//accountNextOfKins.Count() > 1 ? accountNextOfKins.LastOrDefault().Fax : accountNextOfKins.FirstOrDefault().Fax,
                            SECOND_KIN_MAILING_ADDRESS = null, //accountTradingContacts.Count() > 1 ? accountTradingContacts.LastOrDefault().add : accountTradingContacts.FirstOrDefault().Email, //accountNextOfKins.Count() > 1 ? accountNextOfKins.LastOrDefault().MailingAddress : accountNextOfKins.FirstOrDefault().MailingAddress,
                            SECOND_KIN_MOBILE_NUM = accountTradingContacts.Count() > 1 ? accountTradingContacts.LastOrDefault().Mobile : accountTradingContacts.FirstOrDefault().Mobile, //accountNextOfKins.Count() > 1 ? accountNextOfKins.LastOrDefault().Mobile : accountNextOfKins.FirstOrDefault().Mobile,
                            SECOND_KIN_NAME = accountTradingContacts.Count() > 1 ? accountTradingContacts.LastOrDefault().Name : accountTradingContacts.FirstOrDefault().Name, //accountNextOfKins.Count() > 1 ? accountNextOfKins.LastOrDefault().Name : accountNextOfKins.FirstOrDefault().Name,
                            SECOND_KIN_RELATION = string.IsNullOrEmpty(SoftTechApiFormatter.GetRelationshipCode(authorsedOfficer1.RelationToAccountHolder)) ? context.Relationships.FirstOrDefault().SoftTechCode : SoftTechApiFormatter.GetRelationshipCode(authorsedOfficer1.RelationToAccountHolder), //accountNextOfKins.Count() > 1 && !string.IsNullOrEmpty(accountNextOfKins.LastOrDefault().RelationToAccount) ? SoftTechApiFormatter.GetRelationshipCode(accountNextOfKins.LastOrDefault().RelationToAccount) : SoftTechApiFormatter.GetRelationshipCode(accountNextOfKins.FirstOrDefault().RelationToAccount),
                            SECOND_KIN_TEL_NUM = accountTradingContacts.Count() > 1 ? accountTradingContacts.LastOrDefault().Tel : accountTradingContacts.FirstOrDefault().Tel, //accountNextOfKins.Count() > 1 ? accountNextOfKins.LastOrDefault().Telephone : accountNextOfKins.FirstOrDefault().Telephone,
                            SIGNATORIES_NUM = SoftTechApiFormatter.GetSignatoryCode(basicProfile.SignatureTypeId.Value),
                            SIGNATURE_IMAGE_ID = null,
                            SUR_NAME = "NA",//accountMember.Lname,
                            SWIFT_SORT_CODE = accountSettlementDetails.SwiftCode,
                            TEL_NUM = basicProfile.InsStreetAddressTel,//accountMember.Telephone,
                            TIN_NUM = basicProfile.TIN,
                            TOT_EMPLOYMENT_YEAR = null,// accountInstructionEmploymentDetails.YearsOfEmployment.ToString(),
                            OTHER_TOP_UPS_ACTIVITY = "O",
                            TOP_UPS_ACTIVITY = "O",

                            PLACE_OF_BIRTH = null, //accountMember.PlaceOfBirth,
                            REFERENCE = basicProfile.RefNo,
                            APPROVED_BY = CurrentUser.Username,
                            MAIN_CLIENT_NAME = basicProfile.AccountName,
                            MODE_OF_APPLICATION = "ONLINE",
                            INITIAL_INVESTMENT_AMOUNT = application.InitialInvestmentAmount.ToString(),
                            EMAIL_2 = null,// accountMember.Email,

                            SECOND_KIN_FAX = null,//accountNextOfKins.Count() > 1 ? accountNextOfKins.LastOrDefault().Fax : accountNextOfKins.FirstOrDefault().Fax,
                            AUTH_PERSON_ZIP_CODE = null,// accountAuthorisedPerson != null && Utilities.IsNumber(accountAuthorisedPerson.ZipCode) ? accountAuthorisedPerson.ZipCode : null,
                            MODE_OF_APP = application.MODE_OF_APP.Trim(),
                            INVESTMENT_OBJECTIVE = application.InvestmentObjective,
                            INIT_INV_AMOUNT = Convert.ToInt32(application.InitialInvestmentAmount),
                            REG_WITHDRAWAL_ACTIVITY = expectedActivity != null && expectedActivity.WithdrawalOption != null ? expectedActivity.WithdrawalOption.Name.Trim().ToUpper().ElementAt(0).ToString() : null,
                            OTHER_REG_WITHDRAWAL_ACTIVITY = null,
                            TOP_UPS_AMOUNT = expectedActivity != null && expectedActivity.TopUpOption != null ? expectedActivity.ExpectedTopUpAmt.Value.ToString() : "0",
                            REG_WITHDRAWAL_AMOUNT = expectedActivity != null && expectedActivity.WithdrawalOption != null ? expectedActivity.ExpectedWithdrawalAmt.Value.ToString() : "0",
                            NOM_DATE_OF_BIRTH = null,
                            NOM_PLACE_OF_BIRTH = null,
                            NOM_GENDER = null,
                            NOM_ID_TYPES = null,
                            NOM_ID_ISSUE_DATE = null,
                            NOM_ID_EXPIRE_DATE = null,
                            JOINT_NAME = authorsedOfficer2.Name,
                            JOINT_NATIONALITY = SoftTechApiFormatter.GetCountryCode(authorsedOfficer2.CountryId.Value),
                            JOINT_TELEPHONE = authorsedOfficer2.Tel,
                            JOINT_MOBILE = authorsedOfficer2.Mobile,
                            JOINT_FAX = authorsedOfficer2.Fax,
                            JOINT_EMAIL = authorsedOfficer2.Email,
                            JOINT_ID_TYPE = SoftTechApiFormatter.GetIdTypeCode(authorsedOfficer2.IdType.Value),

                            JOINT_ID_NUMBER = authorsedOfficer2.IdNumber,
                            JOINT_ISSUE_DATE = Utilities.SoftTechDateFormatter(authorsedOfficer2.IssueDate),
                            JOINT_ISSUE_AUTHORITY = authorsedOfficer2.IdIssueAuthority,
                            JOINT_EXPIRY_DATE = Utilities.SoftTechDateFormatter(authorsedOfficer2.IDExpiryDate),
                        };

                        int num2 = 0;
                        var list = context.AccountAuthorisedPersons.Where(x => x.AccountId == applicationId).OrderBy(d => d.CreatedDate).ToList();
                        int count = list.Count;

                        foreach (AccountAuthorisedPerson authorisedPerson in list)
                        {
                            ++num2;
                            switch (num2)
                            {
                                case 2:
                                    requestModel.SECOND_AUTH_PERSON_NAME = authorisedPerson.Name;
                                    requestModel.SECOND_AUTH_PERSON_TEL_NUM = authorisedPerson.Tel;
                                    requestModel.SECOND_AUTH_PERSON_MOB_NUM = authorisedPerson.Mobile;
                                    requestModel.SECOND_AUTH_PERSON_FAX = authorisedPerson.Fax;
                                    requestModel.SECOND_AUTH_PERSON_RELATION = authorisedPerson.RelationToAccountHolder;
                                    requestModel.SECOND_AUTH_PERSON_EMAIL = authorisedPerson.Email;
                                    requestModel.SECOND_AUTH_PERSON_MAILING_ADD = authorisedPerson.MailingAddress;
                                    requestModel.SECOND_AUTH_PERSON_CITY = "137";
                                    requestModel.SECOND_AUTH_PERSON_ZIP_CODE = authorisedPerson.ZipCode;
                                    requestModel.SECOND_AUTH_PERSON_COUNTRY = authorisedPerson.CountryId.HasValue ? SoftTechApiFormatter.GetCountryCode(authorisedPerson.CountryId.Value) : "";
                                    requestModel.SECOND_AUTH_PERSON_NIC_PHOTO_ID = authorisedPerson.IdType.HasValue ? SoftTechApiFormatter.GetIdTypeCode(authorisedPerson.IdType.Value) : "";
                                    requestModel.SECOND_AUTH_PERSON_NIC_ISSUE_ATHORITY = authorisedPerson.IdIssueAuthority;
                                    requestModel.SECOND_AUTH_PERSON_NIC_ISSUE_DATE = Utilities.SoftTechDateFormatter(authorisedPerson.IssueDate);
                                    requestModel.SECOND_AUTH_PERSON_NIC_EXPIRY_DATE = Utilities.SoftTechDateFormatter(authorisedPerson.IDExpiryDate);
                                    requestModel.SECOND_AUTH_PERSON_NIC_IMAGE_UPLOD_ID = (string)null;
                                    requestModel.SECOND_AUTH_PERSON_NIC_NUM = authorisedPerson.IdNumber;
                                    requestModel.SECOND_AUTH_TITLE = authorisedPerson.TitleId.HasValue ? Utilities.GetTitleText(authorisedPerson.TitleId) : "";
                                    continue;
                                case 3:
                                    requestModel.THIRD_AUTH_PERSON_NAME = authorisedPerson.Name;
                                    requestModel.THIRD_AUTH_PERSON_TEL_NUM = authorisedPerson.Tel;
                                    requestModel.THIRD_AUTH_PERSON_MOB_NUM = authorisedPerson.Mobile;
                                    requestModel.THIRD_AUTH_PERSON_FAX = authorisedPerson.Fax;
                                    requestModel.THIRD_AUTH_PERSON_RELATION = authorisedPerson.RelationToAccountHolder;
                                    requestModel.THIRD_AUTH_PERSON_EMAIL = authorisedPerson.Email;
                                    requestModel.THIRD_AUTH_PERSON_MAILING_ADD = authorisedPerson.MailingAddress;
                                    requestModel.THIRD_AUTH_PERSON_CITY = "137";
                                    requestModel.THIRD_AUTH_PERSON_ZIP_CODE = authorisedPerson.ZipCode;
                                    requestModel.THIRD_AUTH_PERSON_COUNTRY = authorisedPerson.CountryId.HasValue ? SoftTechApiFormatter.GetCountryCode(authorisedPerson.CountryId.Value) : "";
                                    requestModel.THIRD_AUTH_PERSON_NIC_PHOTO_ID = authorisedPerson.IdType.HasValue ? SoftTechApiFormatter.GetIdTypeCode(authorisedPerson.IdType.Value) : "";
                                    requestModel.THIRD_AUTH_PERSON_NIC_ISSUE_ATHORITY = authorisedPerson.IdIssueAuthority;
                                    requestModel.THIRD_AUTH_PERSON_NIC_ISSUE_DATE = Utilities.SoftTechDateFormatter(authorisedPerson.IssueDate);
                                    requestModel.THIRD_AUTH_PERSON_NIC_EXPIRY_DATE = Utilities.SoftTechDateFormatter(authorisedPerson.IDExpiryDate);
                                    requestModel.THIRD_AUTH_PERSON_NIC_IMAGE_UPLOD_ID = (string)null;
                                    requestModel.THIRD_AUTH_PERSON_NIC_NUM = authorisedPerson.IdNumber;
                                    requestModel.THIRD_AUTH_TITLE = authorisedPerson.TitleId.HasValue ? Utilities.GetTitleText(authorisedPerson.TitleId) : "";
                                    continue;
                                case 4:
                                    requestModel.FOURTH_AUTH_PERSON_NAME = authorisedPerson.Name;
                                    requestModel.FOURTH_AUTH_PERSON_TEL_NUM = authorisedPerson.Tel;
                                    requestModel.FOURTH_AUTH_PERSON_MOB_NUM = authorisedPerson.Mobile;
                                    requestModel.FOURTH_AUTH_PERSON_FAX = authorisedPerson.Fax;
                                    requestModel.FOURTH_AUTH_PERSON_RELATION = authorisedPerson.RelationToAccountHolder;
                                    requestModel.FOURTH_AUTH_PERSON_EMAIL = authorisedPerson.Email;
                                    requestModel.FOURTH_AUTH_PERSON_MAILING_ADD = authorisedPerson.MailingAddress;
                                    requestModel.FOURTH_AUTH_PERSON_CITY = "137";
                                    requestModel.FOURTH_AUTH_PERSON_ZIP_CODE = authorisedPerson.ZipCode;
                                    requestModel.FOURTH_AUTH_PERSON_COUNTRY = authorisedPerson.CountryId.HasValue ? SoftTechApiFormatter.GetCountryCode(authorisedPerson.CountryId.Value) : "";
                                    requestModel.FOURTH_AUTH_PERSON_NIC_PHOTO_ID = authorisedPerson.IdType.HasValue ? SoftTechApiFormatter.GetIdTypeCode(authorisedPerson.IdType.Value) : "";
                                    requestModel.FOURTH_AUTH_PERSON_NIC_ISSUE_ATHORITY = authorisedPerson.IdIssueAuthority;
                                    requestModel.FOURTH_AUTH_PERSON_NIC_ISSUE_DATE = Utilities.SoftTechDateFormatter(authorisedPerson.IssueDate);
                                    requestModel.FOURTH_AUTH_PERSON_NIC_EXPIRY_DATE = Utilities.SoftTechDateFormatter(authorisedPerson.IDExpiryDate);
                                    requestModel.FOURTH_AUTH_PERSON_NIC_IMAGE_UPLOD_ID = (string)null;
                                    requestModel.FOURTH_AUTH_PERSON_NIC_NUM = authorisedPerson.IdNumber;
                                    requestModel.FOURTH_AUTH_TITLE = authorisedPerson.TitleId.HasValue ? Utilities.GetTitleText(authorisedPerson.TitleId) : "";
                                    continue;
                                case 5:
                                    requestModel.FIFTH_AUTH_PERSON_NAME = authorisedPerson.Name;
                                    requestModel.FIFTH_AUTH_PERSON_TEL_NUM = authorisedPerson.Tel;
                                    requestModel.FIFTH_AUTH_PERSON_MOB_NUM = authorisedPerson.Mobile;
                                    requestModel.FIFTH_AUTH_PERSON_FAX = authorisedPerson.Fax;
                                    requestModel.FIFTH_AUTH_PERSON_RELATION = authorisedPerson.RelationToAccountHolder;
                                    requestModel.FIFTH_AUTH_PERSON_EMAIL = authorisedPerson.Email;
                                    requestModel.FIFTH_AUTH_PERSON_MAILING_ADD = authorisedPerson.MailingAddress;
                                    requestModel.FIFTH_AUTH_PERSON_CITY = "137";
                                    requestModel.FIFTH_AUTH_PERSON_ZIP_CODE = authorisedPerson.ZipCode;
                                    requestModel.FIFTH_AUTH_PERSON_COUNTRY = authorisedPerson.CountryId.HasValue ? SoftTechApiFormatter.GetCountryCode(authorisedPerson.CountryId.Value) : "";
                                    requestModel.FIFTH_AUTH_PERSON_NIC_PHOTO_ID = authorisedPerson.IdType.HasValue ? SoftTechApiFormatter.GetIdTypeCode(authorisedPerson.IdType.Value) : "";
                                    requestModel.FIFTH_AUTH_PERSON_NIC_ISSUE_ATHORITY = authorisedPerson.IdIssueAuthority;
                                    requestModel.FIFTH_AUTH_PERSON_NIC_ISSUE_DATE = Utilities.SoftTechDateFormatter(authorisedPerson.IssueDate);
                                    requestModel.FIFTH_AUTH_PERSON_NIC_EXPIRY_DATE = Utilities.SoftTechDateFormatter(authorisedPerson.IDExpiryDate);
                                    requestModel.FIFTH_AUTH_PERSON_NIC_IMAGE_UPLOD_ID = (string)null;
                                    requestModel.FIFTH_AUTH_PERSON_NIC_NUM = authorisedPerson.IdNumber;
                                    requestModel.FIFTH_AUTH_TITLE = authorisedPerson.TitleId.HasValue ? Utilities.GetTitleText(authorisedPerson.TitleId) : "";
                                    continue;
                                default:
                                    continue;
                            }
                        }


                        var softTechResponse = await SoftTechApiHelper.CreateAccount(requestModel);
                        if (softTechResponse != null && softTechResponse.responseCode == "03")
                        {
                            //  success
                            //  var responseArr = response.Split(':');
                            application.Reviewer = CurrentUser.Username;
                            application.SuccessfulReviewDate = DateTime.Now;
                            // application.SuccesfulReviewBy=cu
                            string accNumber = softTechResponse.desc.Split(' ').LastOrDefault();
                            application.BackConnectAccountNumber = accNumber;
                            application.StatusId = 2;
                            Utilities.LogActivity(CurrentUser.Username, "Successful review-" + application.ReferenceNo);
                            context.SaveChanges();
                        }
                        else
                        {
                            Logger.Instance.logInfo(softTechResponse.desc + " " + softTechResponse.invalidFields.ToString() + softTechResponse.desc.ToString());
                            return RedirectToAction("ApplicantProfile", new { _refNumber = application.Id.ToString().Encrypt(), message = ("Error in SoftTech API: " + softTechResponse.desc).Encrypt() });
                        }
                    }
                    return RedirectToAction("ApplicantProfile", new { _refNumber = application.Id.ToString().Encrypt(), message = ("application successfully reviewed: " + application.BackConnectAccountNumber).Encrypt() });

                }
            }
            catch (Exception ex)
            {
                Logger.Instance.logError(ex);
                return RedirectToAction("ApplicantProfile", new { _refNumber = _ref.ToString().Encrypt(), message = ("Error: " + ex.Message).Encrypt() });

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
                        model.IdValidationBy = HttpContext.User.Identity.Name;// (Utilities.GetSessionUser() as AppUser).Email;
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
                        model.IdValidationBy = HttpContext.User.Identity.Name;// (Utilities.GetSessionUser() as AppUser).Email;
                        model.IdValidationMode = "MANUAL";
                        model.ManualValidationComment = comments;
                    }
                }
                context.SaveChanges();
                return RedirectToAction("ApplicantProfile", new { _refNumber = application.Id.ToString().Encrypt(), message = ("Id verification successful.").Encrypt() });
            }

        }


    }
}