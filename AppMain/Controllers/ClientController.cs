using AppLogger;
using AppMain.Providers;
using AppModels;
using AppUtils;
using DBHelper.Schema;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace AppMain.Controllers
{
    [OutputCache(NoStore = true, Duration = 0)]
    [Authorize]
    public class ClientController : Controller
    {
        public Account CurrentUser
        {

            get
            {

                return Utilities.GetSessionUserClient() as Account;
            }
        }
        
        public ActionResult DBLForm(string message=null)
        {
            if (!string.IsNullOrEmpty(message))
            {
                ViewBag.Message = message.Decrypt();
            }
            return View();
        }

        public ActionResult ChangePassword()
        {
            var account = Utilities.GetSessionUserClient() as Account;
            var basicProfile = Utilities.GetApplications(0, 0, null, account.Id.ToString()).FirstOrDefault();
            var model = new ChangePasswordModel
            {
                ConfirmPassword = String.Empty,
                NewPassword = string.Empty,
                CurrentPassword = string.Empty,
                Fullname = basicProfile.AccountName
            };
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            using (var context = new DBLAccountOpeningContext())
            {
                var accountUser = Utilities.ValidateClientAccountLogin(MvcApplication.TempAccount.ReferenceNo, model.CurrentPassword);
                if (accountUser == null)
                {
                    ViewBag.Message = "Current password is invalid";
                    return View(model);
                }

                var user = context.Accounts.Find(MvcApplication.TempAccount.Id);
                string password = Utilities.EncodeBase64(model.NewPassword);
                user.Password = password;
                context.SaveChanges();
                return RedirectToAction("Logout", "Account", new { message = "Password reset successful. Kindly re-login".Encrypt() });
            }

        }




        [HttpPost,ValidateAntiForgeryToken]

        public ActionResult DBLForm(int investmentType = 0, int firstApplicantTitle = 0, string firstApplicantSurname = null, string firstApplicantFname = null, string firstApplicantOname = null, int firstApplicantNationality = 0,
            string firstApplicantDOB = null, string firstApplicantPlaceOfBirth = null, int firstApplicantMaritalStatus = 0, string firstApplicantMotherMaidenName = null, string firstApplicantFullResidentialAddress = null, string firstApplicantResidentialCity = null,
            string firstApplicantResidentialZipCode = null, int firstApplicantResidentialCountry = 0, string firstApplicantFullMailingAddress = null, string firstApplicantMailingCity = null, string firstApplicantMailingZipCode = null, int firstApplicantMailingCountry = 0, int firstApplicantTickApplicable = 0,
            string firstApplicantTel = null, string firstApplicantMobile = null, string firstApplicantFax = null, string firstApplicantOccupation = null, string firstApplicantEmail = null, int firstApplicantIdType = 0, string firstApplicantIdNumber = null, string firstApplicantIdIssuingAuthority = null,
            string firstApplicantIdCardIssueDate = null, string firstApplicantIdCardExpDate = null,
            int jointApplicantTitle = 0, string jointApplicantSurname = null, string jointApplicantFname = null, string jointApplicantOname = null, int jointApplicantNationality = 0, string jointApplicantDOB = null, string jointApplicantPlaceOfBirth = null,
            string jointApplicantTelephone = null, string jointApplicantMobile = null, string jointApplicantResidentialFax = null, string jointApplicantMailingEmail = null, string jointApplicantResidentialAddress = null, string jointApplicantOccupation = null, int jointApplicantIdType = 0, string jointApplicantIdNumber = null, string jointApplicantIdIssuingAuthority = null,
            string jointApplicantIdCardIssueDate = null, string jointApplicantIdCardExpDate = null,
            string itfApplicantSurname = null, string itfApplicantFname = null, string itfApplicantOname = null, int itfApplicantNationality = 0, string itfApplicantDOB = null, string itfApplicantPlaceOfBirth = null, int itfApplicantIdType = 0, string itfApplicantIdNumber = null,
            string itfApplicantIdIssuingAuthority = null, string itfApplicantIdCardIssueDate = null, string itfApplicantIdCardExpDate = null,
            string nextOfKin1Name = null, string nextOfKin1Phone = null, string nextOfKin1Mobile = null, string nextOfKin1Fax = null, string nextOfKin1Relation = null, string nextOfKin1Email = null,
            string nextOfKin1MailAddress = null, string nextOfKin2Name = null, string nextOfKin2Phone = null, string nextOfKin2Mobile = null, string nextOfKin2Fax = null,
            string nextOfKin2Relation = null, string nextOfKin2Email = null, string nextOfKin2MailAddress = null, string actingAsNomineeName = null,
            string authorisedPersonName = null, string authorisedPersonTel = null, string authorisedPersonMobile = null, string authorisedPersonFax = null, string authorisedPersonRelation = null, string authorisedPersonEmail = null,
            string authorisedPersonMailAddress = null, string authorisedPersonCity = null, string authorisedPersonZipCode = null, int authorisedPersonZipCountryId = 0, int authorisedPersonApplicantIdType = 0,
            string authorisedPersonApplicantIdNumber = null, string firstApplicantIdAuthorisedPersonIssuingAuthority = null, string firstAuthorisedApplicantIdCardIssueDate = null, string firstApplicantIdAuthorisedCardExpDate = null,
            string settlementDetailsAccountName = null, string settlementDetailsAccountNumber = null, int settlementDetailsAccountBankId = 0,
            string settlementDetailsAccountBankBranch = null, string settlementDetailsAccountBankSortCode = null,
            int instructionsEmploymentDetailsModeOfInstruction = 0, int instructionsEmploymentDetailsModeOfNotification = 0, int instructionsEmploymentDetailsSourceOfIncome = 0,
            int instructionsEmploymentDetailsEmploymentStatus = 0, string instructionsEmploymentDetailsPreviousOccupation = null, string instructionsEmploymentDetailsPreviousEmployer = null,
            string instructionsEmploymentDetailsCurrentOccupation = null, string instructionsEmploymentDetailsCurrentEmployer = null, string instructionsEmploymentDetailsCurrentEmployerAddress = null, string instructionsEmploymentDetailsCurrentEmployerFrom = null, string instructionsEmploymentDetailsCurrentEmployerTo = null,
            int annualIncomeId = 0, int networthId = 0, int investmentHorizonId = 0, int objectivesId = 0, int investmentKnowledgeId = 0, int riskToleranceId = 0,
            string onlineTradingFacility = null, string declarationHasBeenConvicted = null, string convictionDetails = null, string actingAsNominee = null, string singleAccDeclarationFullName = null,
            int numberOfSignatories = 0,string staffRefCode=null,


             int instInvestmentType = 0, string instNameOfClient = null, string instPricipalBroker = null, string instNatureOfBusiness = null, int instCountryOfResidence = 0, string instRegistrationNumber = null, string instFullMailingAddress = null, string instMailingAddressCity = null, int instMailAddressCountryOfResidence = 0,
            string instFullStreetAddress = null, string instSteetAddressCity = null, string instSteetAddressZipCode = null, int instSteetAddressCountry = 0, string instTelephone = null, string instFax = null, string instEmail = null, string instCompanyType = null, int instRegionalInvestmentId = 0, int instStatementFreq = 0,
            string custodyDetailsName = null, string custodyDetailsPhone = null, string custodyDetailsAddress = null, string custodyDetailsFax = null, string custodyDetailsCashAccNumber = null, string custodyDetailsSecuritiesAccNumber = null,
            int intSettlementDetailsCorrespondentBankId = 0, string intSettlementDetailsCorrespondentBankSwiftNo = null, int intSettlementDetailsIntermediaryBankId = 0, string intSettlementDetailsIntermediaryBankSwiftNo = null,
            string intSettlementDetailsNameOfBeneficiary = null, string intSettlementDetailsBIC = null, string intSettlementDetailsAccountNumber = null, string instMarginTrationOption = null,
            string instOnlineTradingOption = null, int intModeOfInstruction = 0, int intModeOfNotification = 0, int intSourceOfIncome = 0,
            string intAuthorisedOfficer1Name = null, string intAuthorisedOfficer1Tel = null, string intAuthorisedOfficer1Mobile = null, string intAuthorisedOfficer1Fax = null, int intAuthorisedOfficer1Title = 0,
            string intAuthorisedOfficer1Email = null, string intAuthorisedOfficer1Relation = null, string intAuthorisedOfficer1StreetAddress = null, string intAuthorisedOfficer1City = null, string intAuthorisedOfficer1ZipCode = null, int intAuthorisedOfficer1CountryId = 0, int intAuthorisedOfficer1IDType = 0, string intAuthorisedOfficer1IDNumber = null,
            string intAuthorisedOfficer1IssueAuthority = null, string intAuthorisedOfficer1IdCardIssueDate = null, string intAuthorisedOfficer1IdCardIssueExpDate = null,
            string intAuthorisedOfficer2Name = null, string intAuthorisedOfficer2Tel = null, string intAuthorisedOfficer2Mobile = null, string intAuthorisedOfficer2Fax = null, int intAuthorisedOfficer2Title = 0, string intAuthorisedOfficer2Email = null, string intAuthorisedOfficer2Relation = null, string intAuthorisedOfficer2StreetAddress = null,
            string intAuthorisedOfficer2City = null, string intAuthorisedOfficer2ZipCode = null, int intAuthorisedOfficer2CountryId = 0, int intAuthorisedOfficer2IDType = 0, string intAuthorisedOfficer2IDNumber = null,
            string intAuthorisedOfficer2IssueAuthority = null, string intAuthorisedOfficer2IdCardIssueDate = null,
            string intAuthorisedOfficer2IdCardIssueExpDate = null, int tradingContacts1Title = 0, string tradingContacts1Name = null, string tradingContacts1Telephone = null, string tradingContacts1Mobile = null, string tradingContacts1Fax = null,
            string tradingContacts1Email = null, int tradingContacts2Title = 0, string tradingContacts2Fax = null, string tradingContacts2Name = null, string tradingContacts2Telephone = null, string tradingContacts2Mobile = null, string tradingContacts2Email = null,
            string instSignName1 = null, string instSignPosition1 = null, string instSignName2 = null, string instSignPosition2 = null, string instSignName3 = null, string instSignPosition3 = null, string instSignName4 = null, string instSignPosition4 = null,
            int instnumberOfSignatories = 0, string insOtherDetails = null,

           // firstApplicantAuthPersonIdReplace
            HttpPostedFileBase firstApplicantPhotoReplace=null,HttpPostedFileBase firstApplicantSignatureReplace=null,HttpPostedFileBase firstApplicantAuthPersonIdReplace=null,
            HttpPostedFileBase jointApplicantPhotoReplace=null,HttpPostedFileBase jointApplicantSignatureReplace=null, HttpPostedFileBase itfApplicantPhotoReplace=null,

            HttpPostedFileBase instApplicantFirstAuthIdPhotoReplace=null,HttpPostedFileBase intAuthorisedOfficer2IdPhotoReplace=null,
            HttpPostedFileBase instSign1=null,HttpPostedFileBase instSign2=null,HttpPostedFileBase instSign3=null,HttpPostedFileBase instSign4=null,

           int yearsOfEmployment = 0,  string tin = null, string firstApplicantMaidenName = null, int statementFreqId = 0, int expectedAccountActivityId = 0, string firstApplicantGender = null, string jointApplicantGender = null, string itfApplicantGender = null,
            List<string> remark = null, List<int> instructionsEmploymentDetailsSourceOfIncomeIds = null, List<int> intSourceOfIncomeIds = null

            )
        {
            try
            {
                using (var context = new DBLAccountOpeningContext())
                {
                    var account = context.Accounts.Find(MvcApplication.TempAccount.Id);
                    int accountType = MvcApplication.TempAccount.AccountTypeId;
                    string accountId = string.Empty;

                    if (MvcApplication.TempAccount.AccountTypeId <= 3)
                    {
                        account.AccountNumber = null;
                        account.CSDNumber = null;
                        account.InvestmentTypeId = investmentType;
                        account.ExpectedAccountActivityId = expectedAccountActivityId;
                        account.TIN = tin;
                        account.FrequencyOfStatementsId = statementFreqId;
                        account.DeclarationConvictedOfLaw = declarationHasBeenConvicted;
                        account.DeclarationConvictedOfLawDetails = convictionDetails;
                        account.DeclarationActingAsNominee = actingAsNominee;
                        account.DeclarationActingAsNomineeName = actingAsNomineeName;
                        account.DeclarationIWe = singleAccDeclarationFullName;
                        account.SignatureTypeId = numberOfSignatories;
                        account.SelectApplicableId = firstApplicantTickApplicable;

                        if (!string.IsNullOrEmpty(instructionsEmploymentDetailsCurrentEmployerFrom) && !string.IsNullOrEmpty(instructionsEmploymentDetailsCurrentEmployerTo))
                        {

                            int yrs = Utilities.ComputeYears(instructionsEmploymentDetailsCurrentEmployerFrom, instructionsEmploymentDetailsCurrentEmployerTo);
                            if (yrs > 0)
                            {
                                account.YearsOfWorkExperience = yrs;
                                yearsOfEmployment = yrs;
                            }
                        }
                        accountId = account.Id.ToString();

                        //first applicant
                        var accountMembers = account.AccountMembers.OrderBy(x => x.CreatedDate);
                        var firstApplicant = accountMembers.FirstOrDefault();
                        firstApplicant.TitleId = firstApplicantTitle;
                        firstApplicant.Lname = firstApplicantSurname;
                        firstApplicant.Fname = firstApplicantFname;
                        firstApplicant.Othername = firstApplicantOname;
                        firstApplicant.NationalityId = firstApplicantNationality;
                        firstApplicant.DOB = firstApplicantDOB;
                        firstApplicant.Gender = firstApplicantGender;
                        firstApplicant.PlaceOfBirth = firstApplicantPlaceOfBirth;
                        firstApplicant.MaritalStatusId = firstApplicantMaritalStatus;
                        firstApplicant.MothersMaidenName = firstApplicantMotherMaidenName;
                        firstApplicant.MaidenName = firstApplicantMaidenName;
                        firstApplicant.ResidentialAddressFull = firstApplicantFullResidentialAddress;
                        firstApplicant.ResidentialCity = firstApplicantResidentialCity;
                        firstApplicant.ResidentialZipCode = firstApplicantResidentialZipCode;
                        firstApplicant.MailingAddressFull = firstApplicantFullMailingAddress;
                        firstApplicant.MailingAddressCity = firstApplicantMailingCity;
                        firstApplicant.MailingAddressZipCode = firstApplicantMailingZipCode;
                        firstApplicant.SelectWhereApplicableId = firstApplicantTickApplicable;
                        firstApplicant.Telephone = firstApplicantTel;
                        firstApplicant.Mobile = firstApplicantMobile;
                        firstApplicant.Fax = firstApplicantFax;
                        firstApplicant.Occupation = firstApplicantOccupation;
                        firstApplicant.Email = firstApplicantEmail;
                        firstApplicant.IdTypeId = firstApplicantIdType;
                        firstApplicant.IdNumber = firstApplicantIdNumber;
                        firstApplicant.IssuingAuthority = firstApplicantIdIssuingAuthority;
                        firstApplicant.IdCardIssueDate = firstApplicantIdCardIssueDate;
                        firstApplicant.IdCardExpiryDate = firstApplicantIdCardExpDate;

                        if (firstApplicantResidentialCountry > 0)
                        {
                            firstApplicant.ResidentialCountryId = firstApplicantResidentialCountry;
                        }
                        if (firstApplicantMailingCountry>0)
                        {
                            firstApplicant.MailingAddressCountryId = firstApplicantMailingCountry;
                        }
                        if (firstApplicantPhotoReplace!=null)
                        {
                            firstApplicant.IdPath = SaveFileUpload(firstApplicantPhotoReplace);
                        }
                        if (firstApplicantSignatureReplace!=null)
                        {
                            firstApplicant.SignaturePath = SaveFileUpload(firstApplicantSignatureReplace);
                        }
                        context.SaveChanges();
                        if (accountType == 2)
                        {//joint
                            var jointApplicant = accountMembers.LastOrDefault();

                            jointApplicant.TitleId = jointApplicantTitle;
                            jointApplicant.Lname = jointApplicantSurname;
                            jointApplicant.Fname = jointApplicantFname;
                            jointApplicant.Othername = jointApplicantOname;
                            jointApplicant.NationalityId = jointApplicantNationality;
                            jointApplicant.DOB = jointApplicantDOB;
                            jointApplicant.Gender = jointApplicantGender;
                            jointApplicant.PlaceOfBirth = jointApplicantPlaceOfBirth;
                            jointApplicant.ResidentialAddressFull = jointApplicantResidentialAddress;
                            jointApplicant.Telephone = jointApplicantTelephone;
                            jointApplicant.Mobile = jointApplicantMobile;
                            jointApplicant.Fax = jointApplicantResidentialFax;
                            jointApplicant.Occupation = jointApplicantOccupation;
                            jointApplicant.Email = jointApplicantMailingEmail;
                            jointApplicant.IdTypeId = jointApplicantIdType;
                            jointApplicant.IdNumber = jointApplicantIdNumber;
                            jointApplicant.IssuingAuthority = jointApplicantIdIssuingAuthority;
                            jointApplicant.IdCardIssueDate = jointApplicantIdCardIssueDate;
                            jointApplicant.IdCardExpiryDate = jointApplicantIdCardExpDate;
                            // jointApplicant.IdPath = SaveFileUpload(_JointApplicantIdPhoto),
                            // jointApplicant.SignaturePath = SaveFileUpload(_JointApplicantSignature),
                            if (jointApplicantPhotoReplace!=null)
                            {
                                jointApplicant.IdPath = SaveFileUpload(jointApplicantPhotoReplace);
                            }
                            if (jointApplicantSignatureReplace!=null)
                            {
                                jointApplicant.SignaturePath = SaveFileUpload(jointApplicantSignatureReplace);
                            }
                        }
                        context.SaveChanges();
                        if (accountType == 3)
                        {//itf
                            var itfApplicant= accountMembers.LastOrDefault();

                            itfApplicant.Lname = itfApplicantSurname;
                            itfApplicant.Fname = itfApplicantFname;
                            itfApplicant.Othername = itfApplicantOname;
                            itfApplicant.NationalityId = itfApplicantNationality;
                            itfApplicant.DOB = itfApplicantDOB;
                            itfApplicant.PlaceOfBirth = itfApplicantPlaceOfBirth;
                            itfApplicant.Gender = itfApplicantGender;
                            itfApplicant.IdTypeId = itfApplicantIdType;
                            itfApplicant.IdNumber = itfApplicantIdNumber;
                            itfApplicant.IssuingAuthority = itfApplicantIdIssuingAuthority;
                            itfApplicant.IdCardIssueDate = itfApplicantIdCardIssueDate;
                            itfApplicant.IdCardExpiryDate = itfApplicantIdCardExpDate;
                            if (itfApplicantPhotoReplace!=null)
                            {
                                itfApplicant.IdPath = SaveFileUpload(itfApplicantPhotoReplace);
                            }
                            context.SaveChanges();
                        }


                        //next of kins
                        var nextOfKins = context.AccountNextOfKinDetails.Where(x=>x.AccountId==MvcApplication.TempAccount.Id).ToList();
                        context.AccountNextOfKinDetails.RemoveRange(nextOfKins);
                        var nextOfKin1 = new AccountNextOfKinDetail
                        {
                            Name = nextOfKin1Name,
                            AccountId = account.Id,
                            CreatedDate = DateTime.Now,
                            Email = nextOfKin1Email,
                            Id = Guid.NewGuid(),
                            Fax = nextOfKin1Fax,
                            MailingAddress = nextOfKin1MailAddress,
                            Mobile = nextOfKin1Mobile,
                            RelationToAccount = nextOfKin1Relation,
                            Telephone = nextOfKin1Phone,
                        };
                        context.AccountNextOfKinDetails.Add(nextOfKin1);
                        if (!string.IsNullOrEmpty(nextOfKin2Name))
                        {
                            var nextOfKin2 = new AccountNextOfKinDetail
                            {
                                Name = nextOfKin2Name,
                                AccountId = account.Id,
                                CreatedDate = DateTime.Now,
                                Email = nextOfKin2Email,
                                Id = Guid.NewGuid(),
                                Fax = nextOfKin2Fax,
                                MailingAddress = nextOfKin2MailAddress,
                                Mobile = nextOfKin2Mobile,
                                RelationToAccount = nextOfKin2Relation,
                                Telephone = nextOfKin2Phone,
                            };
                            context.AccountNextOfKinDetails.Add(nextOfKin2);
                        }
                        context.SaveChanges();

                        //authorised person
                        var authPerson = context.AccountAuthorisedPersons.Where(x => x.AccountId == MvcApplication.TempAccount.Id).ToList();
                        context.AccountAuthorisedPersons.RemoveRange(authPerson);
                        if (!string.IsNullOrEmpty(authorisedPersonName))
                        {
                            var authorisedPerson = new AccountAuthorisedPerson
                            {
                                Name = authorisedPersonName,
                                Tel = authorisedPersonTel,
                                Mobile = authorisedPersonMobile,
                                AccountId = account.Id,
                                City = authorisedPersonCity,
                                CreatedDate = DateTime.Now,
                                Email = authorisedPersonEmail,
                                Fax = authorisedPersonFax,
                                Id = Guid.NewGuid(),
                                IDExpiryDate = firstApplicantIdAuthorisedCardExpDate,
                                IdIssueAuthority = firstApplicantIdAuthorisedPersonIssuingAuthority,
                                IdNumber = authorisedPersonApplicantIdNumber,
                                IssueDate = firstAuthorisedApplicantIdCardIssueDate,
                                MailingAddress = authorisedPersonMailAddress,
                                RelationToAccountHolder = authorisedPersonRelation,
                                ZipCode = authorisedPersonZipCode,
                                StreetAddress = null,
                                SignaturePath = null,
                            };
                            if (authorisedPersonApplicantIdType == 0)
                            {
                                authorisedPersonApplicantIdType = 1;
                            }
                            if (authorisedPersonZipCountryId > 0)
                            {
                                authorisedPerson.CountryId = authorisedPersonZipCountryId;
                            }
                            if (authorisedPersonApplicantIdType > 0)
                            {
                                authorisedPerson.IdType = authorisedPersonApplicantIdType;
                            }
                            if (firstApplicantAuthPersonIdReplace!=null)
                            {
                                authorisedPerson.IdPath = SaveFileUpload(firstApplicantAuthPersonIdReplace);
                            }
                            context.AccountAuthorisedPersons.Add(authorisedPerson);
                            context.SaveChanges();
                        }

                        //settlement details
                        var settlementDetails = context.AccountSettlementDetails.Where(x => x.AccountId == MvcApplication.TempAccount.Id).ToList();
                        context.AccountSettlementDetails.RemoveRange(settlementDetails);
                        context.AccountSettlementDetails.Add(new AccountSettlementDetail
                        {
                            AccountId = account.Id,
                            AccountName = settlementDetailsAccountName,
                            AccountNumber = settlementDetailsAccountNumber,
                            Id = Guid.NewGuid(),
                            BankId = settlementDetailsAccountBankId,
                            Branch = settlementDetailsAccountBankBranch,
                            CreatedDate = DateTime.Now,
                            SwiftCode = settlementDetailsAccountBankSortCode,
                        });
                        context.SaveChanges();
                        //instructionEmplDetails
                        var instructionEmpDetails = context.AccountInstructionEmploymentDetails.Where(x => x.AccountId == MvcApplication.TempAccount.Id).ToList();
                        context.AccountInstructionEmploymentDetails.RemoveRange(instructionEmpDetails);
                        var instructionEmploymentDetails = new AccountInstructionEmploymentDetail
                        {
                            AccountId = account.Id,
                            CreatedDate = DateTime.Now,
                            ModeOfInstructionId = instructionsEmploymentDetailsModeOfInstruction,
                            ModeOfNotificationId = instructionsEmploymentDetailsModeOfNotification,
                            SourceOfFundId = instructionsEmploymentDetailsSourceOfIncome,
                            EmploymentStatusId = instructionsEmploymentDetailsEmploymentStatus,
                            PrevOccupation = instructionsEmploymentDetailsPreviousOccupation,
                            PrevEmployer = instructionsEmploymentDetailsPreviousEmployer,
                            CurrentOccupation = instructionsEmploymentDetailsCurrentOccupation,
                            CurrentEmployer = instructionsEmploymentDetailsCurrentEmployer,
                            CurrentEmployerAddress = instructionsEmploymentDetailsCurrentEmployerAddress,
                            EmploymentDateFrom = instructionsEmploymentDetailsCurrentEmployerFrom,
                            EmploymentDateTo = instructionsEmploymentDetailsCurrentEmployerTo,
                            Id = Guid.NewGuid(),

                        };
                        if (instructionsEmploymentDetailsSourceOfIncomeIds != null && instructionsEmploymentDetailsSourceOfIncomeIds.Any())
                        {
                            instructionEmploymentDetails.SourceOfFundsIds = string.Join(",", instructionsEmploymentDetailsSourceOfIncomeIds.Select(x => x.ToString()));
                        }
                        if (yearsOfEmployment > 0)
                        {
                            instructionEmploymentDetails.YearsOfEmployment = yearsOfEmployment;
                        }
                        context.AccountInstructionEmploymentDetails.Add(instructionEmploymentDetails);
                        context.SaveChanges();
                        //financial investment risk
                        var riskProfile = context.AccountFinancialInvestmentRiskProfiles.Where(x => x.AccountId == MvcApplication.TempAccount.Id).ToList();
                        context.AccountFinancialInvestmentRiskProfiles.RemoveRange(riskProfile);

                        context.AccountFinancialInvestmentRiskProfiles.Add(new AccountFinancialInvestmentRiskProfile
                   {
                    AccountId = account.Id,
                    AnnualIncomeId = annualIncomeId,
                    NetworthId = networthId,
                    InvestmentHorizonId = investmentHorizonId,
                    ObjectivesId = objectivesId,
                    InvestmentKnowledgeId = investmentKnowledgeId,
                    RiskToleranceId = riskToleranceId,
                    OnlineTradingFacility = onlineTradingFacility,
                    CreatedDate = DateTime.Now,
                    Id = Guid.NewGuid(),
                });
                    context.SaveChanges();


                    }

                    else
                    {
                        //institutional
                        account.InvestmentTypeId = instInvestmentType;
                        account.AccountNumber = null;
                        account.InstitutionClientName = instNameOfClient;
                        account.ExpectedAccountActivityId = expectedAccountActivityId;
                        account.InstitutionalPrincipalBroker = instPricipalBroker;
                        account.InstitutionNatureOfBusiness = instNatureOfBusiness;
                        account.InsstitutionalCountryOfIncorporation = instCountryOfResidence;
                        account.InstitutionRegistrationNumber = instRegistrationNumber;
                        account.TIN = tin;
                        account.MailingAddressFull = instFullMailingAddress;
                        account.MailingAddressCity = instMailingAddressCity;
                        account.InstMailingAddressCountryId = instMailAddressCountryOfResidence;
                        account.StreetAddressFull = instFullStreetAddress;
                        account.InstStreetAddressCity = instSteetAddressCity;
                        account.InsStreetAddressZipCode = instSteetAddressZipCode;
                        account.InsStreetAddressTel = instTelephone;
                        account.InsStreetAddressFax = instFax;
                        account.InsStreetAddressEmail = instEmail;
                        account.InstCompanyType = instCompanyType;
                        account.RegionalInvestmentId = instRegionalInvestmentId;
                        account.FrequencyOfStatementsId = instStatementFreq;
                      //  account.AccountTypeId = accountType;
                        account.SignatureTypeId = instnumberOfSignatories;
                        account.InstOtherDetails = insOtherDetails;
                        if (instSteetAddressCountry > 0)
                        {
                            account.InsStreetAddressCountryId = instSteetAddressCountry;
                        }

                      
                        accountId = account.Id.ToString();
                        context.SaveChanges();
                        //custody account details
                        var custodyDetails = context.AccountInsCustodyAccountDetails.Where(x => x.AccountId == account.Id).ToList();
                        context.AccountInsCustodyAccountDetails.RemoveRange(custodyDetails);
                        context.AccountInsCustodyAccountDetails.Add(new AccountInsCustodyAccountDetail
                        {

                            AccountId = account.Id,
                            CustodianName = custodyDetailsName,
                            Telephone = custodyDetailsPhone,
                            Address = custodyDetailsAddress,
                            Id = Guid.NewGuid(),
                            CashAccountNumber = custodyDetailsCashAccNumber,
                            Fax = custodyDetailsFax,
                            CreatedDate = DateTime.Now,
                            SecuritiesAccountNumber = custodyDetailsSecuritiesAccNumber,
                        });
                        //settlement details
                        var settlementDet = context.AccountSettlementDetails.Where(x => x.AccountId == account.Id).ToList();
                        context.AccountSettlementDetails.RemoveRange(settlementDet);
                        var settlementDetails = new AccountSettlementDetail
                        {
                            CorrespondentBankSwiftCode = intSettlementDetailsCorrespondentBankSwiftNo,
                            IntermediaryBankSwiftCode = intSettlementDetailsIntermediaryBankSwiftNo,
                            NameOfBeneficiary = intSettlementDetailsNameOfBeneficiary,
                            BIC = intSettlementDetailsBIC,
                            AccountNumber = intSettlementDetailsAccountNumber,
                            MarginTradingOption = instMarginTrationOption,
                            AccountId = account.Id,
                            OnlineTradingFacility = instOnlineTradingOption,
                            AccountName = null,
                            CreatedDate = DateTime.Now,
                            Id = Guid.NewGuid(),
                            };
                        if (intSettlementDetailsCorrespondentBankId > 0)
                        {
                            settlementDetails.CorrespondentBankId = intSettlementDetailsCorrespondentBankId;
                        }
                        if (intSettlementDetailsIntermediaryBankId > 0)
                        {
                            settlementDetails.IntermediaryBankId = intSettlementDetailsIntermediaryBankId;
                        }
                        context.AccountSettlementDetails.Add(settlementDetails);
                        context.SaveChanges();

                        //instruction settlement details
                        var instructionSettlment = context.AccountInstructionEmploymentDetails.Where(x => x.AccountId == account.Id).ToList();
                        context.AccountInstructionEmploymentDetails.RemoveRange(instructionSettlment);
                        context.AccountInstructionEmploymentDetails.Add(new AccountInstructionEmploymentDetail
                        {
                            ModeOfInstructionId = intModeOfInstruction,
                            ModeOfNotificationId = intModeOfNotification,
                            SourceOfFundId = intSourceOfIncome,
                            AccountId = account.Id,
                            CreatedDate = DateTime.Now,
                            Id = Guid.NewGuid(),
                            SourceOfFundsIds = string.Join(",", intSourceOfIncomeIds.Select(x => x.ToString()))
                        });
                        context.SaveChanges();
                        //authorised officers
                        var authPersons = context.AccountAuthorisedPersons.Where(x => x.AccountId == account.Id).ToList();
                        var authPersonsPhotoIds = authPersons.OrderBy(x => x.CreatedDate).Select(x => x.IdPath);
                        var authPersonsSignsIds = authPersons.OrderBy(x => x.CreatedDate).Select(x => x.SignaturePath);
                        context.AccountAuthorisedPersons.RemoveRange(authPersons);
                        var authorisedOfficer1 = new AccountAuthorisedPerson
                        {
                            Id = Guid.NewGuid(),
                            AccountId = account.Id,
                            CreatedDate = DateTime.Now,
                            Name = intAuthorisedOfficer1Name,
                            Tel = intAuthorisedOfficer1Tel,
                            Mobile = intAuthorisedOfficer1Mobile,
                            Fax = intAuthorisedOfficer1Fax,
                            Email = intAuthorisedOfficer1Email,
                            RelationToAccountHolder = intAuthorisedOfficer1Relation,
                            StreetAddress = intAuthorisedOfficer1StreetAddress,
                            City = intAuthorisedOfficer1City,
                            ZipCode = intAuthorisedOfficer1ZipCode,
                            IdNumber = intAuthorisedOfficer1IDNumber,
                            IdIssueAuthority = intAuthorisedOfficer1IssueAuthority,
                            IssueDate = intAuthorisedOfficer1IdCardIssueDate,
                            IDExpiryDate = intAuthorisedOfficer1IdCardIssueExpDate,
                            MailingAddress = null,
                            SignaturePath = authPersonsSignsIds.FirstOrDefault(),
                            IdPath= authPersonsPhotoIds.FirstOrDefault(),
                           
                        };
                        if (instApplicantFirstAuthIdPhotoReplace!=null)
                        {
                            authorisedOfficer1.IdPath = SaveFileUpload(instApplicantFirstAuthIdPhotoReplace);
                        }
                        if (intAuthorisedOfficer1Title > 0)
                        {
                            authorisedOfficer1.TitleId = intAuthorisedOfficer1Title;
                        }
                        if (intAuthorisedOfficer1CountryId > 0)
                        {
                            authorisedOfficer1.CountryId = intAuthorisedOfficer1CountryId;
                        }

                        if (intAuthorisedOfficer1IDType > 0)
                        {
                            authorisedOfficer1.IdType = intAuthorisedOfficer1IDType;
                        }

                        context.AccountAuthorisedPersons.Add(authorisedOfficer1);

                        //officer 2
                        var authorisedOfficer2 = new AccountAuthorisedPerson
                        {
                            Id = Guid.NewGuid(),
                            AccountId = account.Id,
                            CreatedDate = DateTime.Now,
                            Name = intAuthorisedOfficer2Name,
                            Tel = intAuthorisedOfficer2Tel,
                            Mobile = intAuthorisedOfficer2Mobile,
                            Fax = intAuthorisedOfficer2Fax,
                            Email = intAuthorisedOfficer2Email,
                            RelationToAccountHolder = intAuthorisedOfficer2Relation,
                            StreetAddress = intAuthorisedOfficer2StreetAddress,
                            City = intAuthorisedOfficer2City,
                            ZipCode = intAuthorisedOfficer2ZipCode,
                            IdNumber = intAuthorisedOfficer2IDNumber,
                            IdIssueAuthority = intAuthorisedOfficer2IssueAuthority,
                            IssueDate = intAuthorisedOfficer2IdCardIssueDate,
                            IDExpiryDate = intAuthorisedOfficer2IdCardIssueExpDate,
                            MailingAddress = null,
                            SignaturePath = authPersonsSignsIds.LastOrDefault(),
                            IdPath = authPersonsPhotoIds.LastOrDefault(),
                        };
                        if (intAuthorisedOfficer2IdPhotoReplace!=null)
                        {
                            authorisedOfficer2.IdPath = SaveFileUpload(intAuthorisedOfficer2IdPhotoReplace);

                        }
                        if (intAuthorisedOfficer2Title > 0)
                        {
                            authorisedOfficer2.TitleId = intAuthorisedOfficer2Title;
                        }
                        if (intAuthorisedOfficer2CountryId > 0)
                        {
                            authorisedOfficer2.CountryId = intAuthorisedOfficer2CountryId;
                        }

                        if (intAuthorisedOfficer2IDType > 0)
                        {
                            authorisedOfficer2.IdType = intAuthorisedOfficer2IDType;
                        }

                        context.AccountAuthorisedPersons.Add(authorisedOfficer2);
                        context.SaveChanges();
                        //trading contacts
                        var trading = context.AccountInsTradingContacts.Where(x => x.AccountId == account.Id).ToList();
                        context.AccountInsTradingContacts.RemoveRange(trading);

                        context.AccountInsTradingContacts.Add(new AccountInsTradingContact
                        {

                            Id = Guid.NewGuid(),
                            AccountId = account.Id,
                            CreatedDate = DateTime.Now,
                            TitleId = tradingContacts1Title,
                            Name = tradingContacts1Name,
                            Tel = tradingContacts1Telephone,
                            Mobile = tradingContacts1Mobile,
                            Fax = tradingContacts1Fax,
                            Email = tradingContacts1Email,
                        });
                        if (string.IsNullOrEmpty(tradingContacts2Name))
                        {
                            var contactTrading2 = new AccountInsTradingContact
                            {
                                Id = Guid.NewGuid(),
                                AccountId = account.Id,
                                CreatedDate = DateTime.Now,
                                Name = tradingContacts2Name,
                                Tel = tradingContacts2Telephone,
                                Mobile = tradingContacts2Mobile,
                                Fax = tradingContacts2Fax,
                                Email = tradingContacts2Email,

                            };
                            if (tradingContacts2Title > 0)
                            {
                                contactTrading2.TitleId = tradingContacts2Title;
                            }
                            context.AccountInsTradingContacts.Add(contactTrading2);
                        }

                        context.SaveChanges();
                        //signatures
                        
                        var signatures = context.AccountInstSignatoriesMandates.OrderBy(x=>x.CreatedDate).Where(x => x.AccountId == account.Id).ToList();
                        var signaturesExisting = signatures.Select(x => x.SignaturePath);
                        context.AccountInstSignatoriesMandates.RemoveRange(signatures);

                        var sign1 = new AccountInstSignatoriesMandate
                        {

                            Id = Guid.NewGuid(),
                            AccountId = account.Id,
                            CreatedDate = DateTime.Now,
                            Name = instSignName1,
                            Position = instSignPosition1,
                            SignaturePath = signaturesExisting.FirstOrDefault()
                        };
                        if (instSign1!=null)
                        {
                            sign1.SignaturePath = SaveFileUpload(instSign1);
                        }
                        context.AccountInstSignatoriesMandates.Add(sign1);

                        var sign2=new AccountInstSignatoriesMandate
                        {

                            Id = Guid.NewGuid(),
                            AccountId = account.Id,
                            CreatedDate = DateTime.Now,
                            Name = instSignName2,
                            Position = instSignPosition2,
                            SignaturePath=  signaturesExisting.LastOrDefault()
                        };
                        if (instSign2 != null)
                        {
                            sign2.SignaturePath = SaveFileUpload(instSign2);
                        }
                        context.AccountInstSignatoriesMandates.Add(sign2);

                        if (!string.IsNullOrEmpty(instSignName3))
                        {
                            context.AccountInstSignatoriesMandates.Add(new AccountInstSignatoriesMandate
                            {

                                Id = Guid.NewGuid(),
                                AccountId = account.Id,
                                CreatedDate = DateTime.Now,
                                Name = instSignName3,
                                Position = instSignPosition3,
                                SignaturePath = instSign3 != null ? SaveFileUpload(instSign3) : null
                            });
                        }
                        if (!string.IsNullOrEmpty(instSignName4))
                        {
                            context.AccountInstSignatoriesMandates.Add(new AccountInstSignatoriesMandate
                            {

                                Id = Guid.NewGuid(),
                                AccountId = account.Id,
                                CreatedDate = DateTime.Now,
                                Name = instSignName4,
                                Position = instSignPosition4,
                                SignaturePath = instSign4 != null ? SaveFileUpload(instSign4) : null
                            });
                        }
                        context.SaveChanges();

                    }
                    context.SaveChanges();
                    return RedirectToAction("DBLForm", new { message = "Details updated. Thank you".Encrypt() });

                }
            }
            catch (Exception ex)
            {
                Logger.Instance.logError(ex);
                return RedirectToAction("DBLForm", new { message = ex.Message.Encrypt() });

            }

        }

        public ActionResult AppProfile(string message=null)
        {
            if (!string.IsNullOrEmpty(message))
            {
                ViewBag.Message = message.Decrypt();
            }
            return View();
        }

        public ActionResult SudmitApplication()
        {
            using (var context=new DBLAccountOpeningContext())
            {
                var model = context.Accounts.Find(MvcApplication.TempAccount.Id);
                model.StatusId = 1;

                context.SaveChanges();
                return RedirectToAction("AppProfile", new { message = "Application submitted. Thank you".Encrypt() });

            }
        }
        public ActionResult FileUploads(string message=null)
        {
            if (!string.IsNullOrEmpty(message))
            {
                ViewBag.Message = message.Decrypt();
            }
            return View();

        }


        public ActionResult ConfirmSubmit()
        {
            return View();

        }



        public string SaveFileUpload(HttpPostedFileBase file)
        {
            Random rnd = new Random();
            string fileAppend = rnd.Next(100000, 999999).ToString() + DateTime.UtcNow.Ticks;
            string ext = Path.GetExtension(file.FileName);
            string finalFileName = Guid.NewGuid() + fileAppend + ext;
            string fileSavePath = Server.MapPath("~/Images/" + finalFileName); //create the full path
            file.SaveAs(fileSavePath);
            var saveName = finalFileName;
            return saveName;
        }

        [HttpPost,ValidateAntiForgeryToken]
        public ActionResult ReplaceFile(Guid applicationId,int typeId,string oldPath, HttpPostedFileBase imgFile=null,HttpPostedFileBase pdfFile=null)
        {
            using (var context=new DBLAccountOpeningContext())
            {
                var model = context.Accounts.Find(applicationId);
                if (typeId==0)
                {
                    //csd
                    model.CSDFormPath = SaveFileUpload(pdfFile);
                    context.SaveChanges();
                    return RedirectToAction("FileUploads",new { message ="File Uploaded!".Encrypt()});
                }
                else
                {
                    var find = context.AccountFileUploads.FirstOrDefault(x=>x.Path==oldPath);
                    if (find!=null)
                    {
                        find.Path = SaveFileUpload(imgFile);
                        context.SaveChanges();
                        return RedirectToAction("FileUploads", new { message = "File Uploaded!".Encrypt() });

                    }
                }
                return RedirectToAction("FileUploads");

            }
        }


        public ActionResult ETI(string message=null)
        {
            if (!string.IsNullOrEmpty(message))
            {
                ViewBag.Message = message.Decrypt();
            }
            return View();
        }

        [HttpPost,ValidateAntiForgeryToken]
        public ActionResult ETI(string indemnityTxt1 = null, string indemnityTxt2 = null, string indemnityTxt3 = null, string indemnityTxt4 = null, string indemnityName1 = null, string indemnityName2 = null, string indemnityEmail1 = null, string indemnityEmail2 = null)
        {
            using (var context=new DBLAccountOpeningContext())
            {

                var model = context.AccountETIs.FirstOrDefault(x=>x.AccountId==MvcApplication.TempAccount.Id);
                model.Text1 = indemnityTxt1;
                model.Text2 = indemnityTxt2;
                model.Text3 = indemnityTxt3;
                model.Text4 = indemnityTxt4;
                model.Name1 = indemnityName1;
                model.Name2 = indemnityName2;
                model.Email1 = indemnityEmail1;
                model.Email2 = indemnityEmail2;
                context.SaveChanges();
                return RedirectToAction("ETI", new { message = "ETI record updated".Encrypt() });

            }
        }


        public ActionResult AML(string message=null)
        {
            if (!string.IsNullOrEmpty(message))
            {
                ViewBag.Message = message.Decrypt();
            }
            return View();
        }

        [HttpPost,ValidateAntiForgeryToken]
        public ActionResult AML(List<string> remark = null)
        {
            using (var context=new DBLAccountOpeningContext())
            {
                var oldResponses = context.AccountAMLResponses.Where(x => x.AccountId == MvcApplication.TempAccount.Id);
                context.AccountAMLResponses.RemoveRange(oldResponses);
                string accountId = MvcApplication.TempAccount.Id.ToString();
                context.SaveChanges();
                //aml questions
                var aml = context.AMLQuestions.Where(x => x.IsActive).OrderBy(x => x.Id).ToList();
                for (int i = 0; i < aml.Count(); i++)
                {
                    int sum = i + 1;
                    string paramString = "amlQues_" + sum;
                    if (i <= 9 && i != 9)
                    {
                        paramString = paramString.Replace("0", "");
                    }
                    string value = Request[paramString];
                    var amlItem = aml[i];
                    if (value == "Yes" || value == "No")
                    {
                        context.AccountAMLResponses.Add(new AccountAMLRespons
                        {
                            AccountId = Guid.Parse(accountId),
                            CreatedDate = DateTime.Now,
                            Id = Guid.NewGuid(),
                            QuestionId = amlItem.Id,
                            YesNo = value,
                            RatingValue = value == "Yes" ? amlItem.YesRating : amlItem.NoRating,
                            Remark = remark != null && remark.Any() ? remark[i] : string.Empty,

                        });
                    }
                    else
                    {
                        context.AccountAMLResponses.Add(new AccountAMLRespons
                        {
                            AccountId = Guid.Parse(accountId),
                            CreatedDate = DateTime.Now,
                            Id = Guid.NewGuid(),
                            QuestionId = amlItem.Id,
                            YesNo = value,
                            RatingValue = -1,
                            Remark = remark != null && remark.Any() ? remark[i] : string.Empty
                        });
                    }
                }
                context.SaveChanges();


            }

            return RedirectToAction("AML", new { message = "AML profile updated".Encrypt() });

        }


    }
}