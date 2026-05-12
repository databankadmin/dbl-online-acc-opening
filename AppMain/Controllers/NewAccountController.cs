using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AppLogger;
using AppMain.Providers;
using AppModels;
using AppUtils;
using DBHelper.Schema;

namespace AppMain.Controllers
{
    public class NewAccountController : Controller
    {
        // GET: NewAccount
        public ActionResult Initiate(string acc_type, string param, string auth = null)
        {
            if (Utilities.AppUsers == null)
            {
                Utilities.AppUsers = AppServerHelper.GetAppUsers();
            }
            MvcApplication.AccountType = int.Parse(Utilities.DecodeBase64(acc_type));
            var context = new DBLAccountOpeningContext();
            if (!string.IsNullOrEmpty(auth))
            {
                var _userParams = Utilities.DecodeBase64(auth);
                var _userParamsArray = _userParams.Split(':');
                string _userName = _userParamsArray[0];
                string _userEmail = _userParamsArray[1];
                var user = context.AppUsers.FirstOrDefault(x => x.Email == _userEmail);
                if (user == null)
                {
                    var newUser = new AppUser
                    {
                        Name = _userName,
                        Email = _userEmail,
                        RoleId = 1,
                        Phone = "0000000000",
                        IsActive = true,
                        CreatedDate = DateTime.Now,
                        Id = Guid.NewGuid(),
                        LastLogin = DateTime.Now,

                    };
                    context.AppUsers.Add(newUser);
                }
                var userModel = new UserModel
                {
                    AppCode = ConfigurationManager.AppSettings["APP_CODE"],
                    Fullname = _userName,
                    IsActive = true,
                    IsSuperUser = false,
                    LocationId = 10,
                    RoleId = 1,
                    Username = _userEmail,
                };
                string add = AppServerHelper.AddUser(userModel);
                Utilities.AppUsers = null;
                Utilities.AppUsers = AppServerHelper.GetAppUsers();
                FormsAuthentication.SetAuthCookie(_userEmail, false);
                Profile.Initialize(_userEmail, true);
            }
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


        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult ForgottenAccount(string name, string address, string email, string phone)
        {
            using (var context = new DBLAccountOpeningContext())
            {
                Guid _ref = Guid.NewGuid();
                context.ForgottenAccountDetails.Add(new ForgottenAccountDetail
                {
                    Address = address,
                    CreatedDate = DateTime.Now,
                    Email = email,
                    Id = _ref,
                    Name = name,
                    Phone = phone,
                    StatusId = 1
                });
                context.SaveChanges();
                return RedirectToAction("ForgotAccountCompleted", new { reference = _ref });

            }

        }


        public UserModel CurrentUser
        {

            get
            {
                return Utilities.GetSessionUser() as UserModel;
            }
        }

        public ActionResult ForgotAccountCompleted(Guid reference)
        {
            using (var context = new DBLAccountOpeningContext())
            {
                var model = context.ForgottenAccountDetails.Find(reference);
                return View(model);

            }
        }

        [HttpPost]
        public ActionResult GetFileUploads(HttpPostedFileBase myPhoto)
        {

            return Json(new { status = true, Message = "" });
        }

        [HttpPost]
        public ActionResult Initiate(int investmentType = 0, int firstApplicantTitle = 0, string firstApplicantSurname = null, string firstApplicantFname = null, string firstApplicantOname = null, int firstApplicantNationality = 0,
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
            int numberOfSignatories = 0,

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

            string indemnityTxt1 = null, string indemnityTxt2 = null, string indemnityTxt3 = null, string indemnityTxt4 = null, string indemnityName1 = null, string indemnityName2 = null, string indemnityEmail1 = null, string indemnityEmail2 = null,
            int yearsOfEmployment = 0, string tin = null, string firstApplicantMaidenName = null, int statementFreqId = 0, int expectedAccountActivityId = 0, string firstApplicantGender = null, string jointApplicantGender = null, string itfApplicantGender = null,
            List<string> remark = null, int staffRefCode = 0, List<int> instructionsEmploymentDetailsSourceOfIncomeIds = null, List<int> intSourceOfIncomeIds = null,

            string _firstApplicantSignatureCapture = null, string _jointApplicantSignatureCapture = null, string _instSign1Capture = null, string _instSign2Capture = null, string _instSign3Capture = null, string _instSign4Capture = null,

            string _firstApplicantIdPhoto = null, string _jointApplicantIdPhoto = null, string _itfApplicantIdPhoto = null, string _firstJointItfAuthorisedPerson = null, string _instAuthorisedOfficer1PhotoId = null,
            string _instAuthorisedOfficer2PhotoId = null, string _instSignatory1 = null, string _instSignatory2 = null, string _instSignatory3 = null, string _instSignatory4 = null,
            string _csdCompletedForm = null, string _firstApplicantSignature = null, string _jointApplicantSignature = null,
            string passportPhotosList = null, string residenceList = null, string businessDocsList = null,
             string residentPermitNumber = null, string residentPermitNumberPlaceOfIssue = null, string residentPermitIssueDate = null, string residentPermitExpiryDate = null,
             string residentPermitNumber2 = null, string residentPermitNumberPlaceOfIssue2 = null, string residentPermitIssueDate2 = null, string residentPermitExpiryDate2 = null,
             decimal initialInvestmentAmt = 0,
            List<int> sourceOfFunds = null, string sourceOfFundsOthers = null, int topUpoptionId = 0, string topUpoptionOthers = null, int withdrawalOptionsId = 0, string withdrawalOptionsOthers = null, decimal expectedTopUpAmt = 0, decimal expectedWithdrawalAmt = 0,

             int beneficiary1TitleId = 0, string beneficiary1TitleOther = null, string beneficiary1Surname = null, string beneficiary1Fname = null, string beneficiary1Relation = null, decimal beneficiaryPcnt1 = 0,
            int beneficiary1GenderId = 0, int beneficiary1MaritalStatusId = 0, string beneficiary1BirthDate = null, string beneficiary1placeOfBirth = null, int beneficiary1NationalityId = 0, int beneficiary1countryOfResidenceId = 0, int beneficiary1photoIdType = 0,
            string beneficiary1IDNumber = null, string beneficiary1IDPlaceOfIssue = null, string beneficiary1IDIssueDate = null, string beneficiary1ExpiryDate = null,

             int beneficiary2TitleId = 0, string beneficiary2TitleOther = null, string beneficiary2Surname = null, string beneficiary2Fname = null, string beneficiary2Relation = null, int beneficiary2GenderId = 0,
            int beneficiary2MaritalStatusId = 0, string beneficiary2BirthDate = null, string beneficiary2placeOfBirth = null,
            int beneficiary2NationalityId = 0, int beneficiary2countryOfResidenceId = 0, int beneficiary2photoIdType = 0, string beneficiary2IDNumber = null, string beneficiary2IDPlaceOfIssue = null,
            string beneficiary2IDIssueDate = null, string beneficiary2ExpiryDate = null, decimal beneficiaryPcnt2 = 0, string investmentObjective = null,

             int beneficiary3TitleId = 0, string beneficiary3TitleOther = null, int beneficiary3GenderId = 0, int beneficiary3NationalityId = 0, string beneficiary3IDPlaceOfIssue = null, string beneficiary3Surname = null, int beneficiary3MaritalStatusId = 0, int beneficiary3countryOfResidenceId = 0, string beneficiary3IDIssueDate = null,
            string beneficiary3Fname = null, string beneficiary3BirthDate = null, int beneficiary3photoIdType = 0, string beneficiary3ExpiryDate = null, string beneficiary3Relation = null, string beneficiary3placeOfBirth = null, string beneficiary3IDNumber = null, decimal beneficiaryPcnt3 = 0,


            int beneficiary4TitleId = 0, string beneficiary4TitleOther = null, int beneficiary4GenderId = 0, int beneficiary4NationalityId = 0, string beneficiary4IDPlaceOfIssue = null, string beneficiary4Surname = null, int beneficiary4MaritalStatusId = 0, int beneficiary4countryOfResidenceId = 0, string beneficiary4IDIssueDate = null,
            string beneficiary4Fname = null, string beneficiary4BirthDate = null, string beneficiaryPhone4 = null, int beneficiary4photoIdType = 0, string beneficiary4ExpiryDate = null, string beneficiary4Relation = null, string beneficiary4placeOfBirth = null, string beneficiary4IDNumber = null, decimal beneficiaryPcnt4 = 0,

            int beneficiary5TitleId = 0, string beneficiary5TitleOther = null, int beneficiary5GenderId = 0, int beneficiary5NationalityId = 0, string beneficiary5IDPlaceOfIssue = null, string beneficiary5Surname = null, int beneficiary5MaritalStatusId = 0, int beneficiary5countryOfResidenceId = 0, string beneficiary5IDIssueDate = null,
            string beneficiary5Fname = null, string beneficiary5BirthDate = null, int beneficiary5photoIdType = 0, string beneficiary5ExpiryDate = null, string beneficiary5Relation = null, string beneficiary5placeOfBirth = null, string beneficiary5IDNumber = null, decimal beneficiaryPcnt5 = 0,
            string ghCardNo = null, string ghCardNo2 = null, string profession = null, string profession2 = null, string professionLicenceNumber = null, string professionLicenceNumber2 = null,
            string intSettlementDetailsCorrespondentBankBranch = null, string intSettlementDetailsIntermediaryBankBranch = null, string intAuthorisedOfficer3Name = null, string intAuthorisedOfficer3Tel = null, string intAuthorisedOfficer3Mobile = null, string intAuthorisedOfficer3Fax = null, int intAuthorisedOfficer3Title = 0, string intAuthorisedOfficer3Email = null, string intAuthorisedOfficer3Relation = null, string intAuthorisedOfficer3StreetAddress = null, string intAuthorisedOfficer3City = null, string intAuthorisedOfficer3ZipCode = null, int intAuthorisedOfficer3CountryId = 0, int intAuthorisedOfficer3IDType = 0, string intAuthorisedOfficer3IDNumber = null, string intAuthorisedOfficer3IssueAuthority = null, string intAuthorisedOfficer3IdCardIssueDate = null, string intAuthorisedOfficer3IdCardIssueExpDate = null, string intAuthorisedOfficer4Name = null, string intAuthorisedOfficer4Tel = null, string intAuthorisedOfficer4Mobile = null, string intAuthorisedOfficer4Fax = null, int intAuthorisedOfficer4Title = 0, string intAuthorisedOfficer4Email = null, string intAuthorisedOfficer4Relation = null, string intAuthorisedOfficer4StreetAddress = null, string intAuthorisedOfficer4City = null, string intAuthorisedOfficer4ZipCode = null, int intAuthorisedOfficer4CountryId = 0, int intAuthorisedOfficer4IDType = 0, string intAuthorisedOfficer4IDNumber = null, string intAuthorisedOfficer4IssueAuthority = null, string intAuthorisedOfficer4IdCardIssueDate = null, string intAuthorisedOfficer4IdCardIssueExpDate = null, string intAuthorisedOfficer5Name = null, string intAuthorisedOfficer5Tel = null, string intAuthorisedOfficer5Mobile = null, string intAuthorisedOfficer5Fax = null, int intAuthorisedOfficer5Title = 0, string intAuthorisedOfficer5Email = null, string intAuthorisedOfficer5Relation = null, string intAuthorisedOfficer5StreetAddress = null, string intAuthorisedOfficer5City = null, string intAuthorisedOfficer5ZipCode = null, int intAuthorisedOfficer5CountryId = 0, int intAuthorisedOfficer5IDType = 0, string intAuthorisedOfficer5IDNumber = null, string intAuthorisedOfficer5IssueAuthority = null, string intAuthorisedOfficer5IdCardIssueDate = null, string intAuthorisedOfficer5IdCardIssueExpDate = null,
            string _instAuthorisedOfficer3PhotoId = null, string _instAuthorisedOfficer4PhotoId = null, string _instAuthorisedOfficer5PhotoId = null
            )

        {
            try
            {
                int accountType = MvcApplication.AccountType;
                var context = new DBLAccountOpeningContext();
                string password = Utilities.GeneratePassword(8);
                string accountId = string.Empty;
                var currentUser = CurrentUser;
                if (accountType <= 3)
                {
                    // var _csd = _CsdCompletedForm;
                    //single,joint,itf
                    var account = new Account
                    {
                        AccountNumber = null,
                        CSDNumber = null,
                        AccountTypeId = accountType,
                        InvestmentTypeId = investmentType,
                        ExpectedAccountActivityId = expectedAccountActivityId,
                        TIN = tin,
                        FrequencyOfStatementsId = statementFreqId,
                        DeclarationConvictedOfLaw = declarationHasBeenConvicted,
                        DeclarationConvictedOfLawDetails = convictionDetails,
                        DeclarationActingAsNominee = actingAsNominee,
                        DeclarationActingAsNomineeName = actingAsNomineeName,
                        DeclarationIWe = singleAccDeclarationFullName,
                        SignatureTypeId = numberOfSignatories,
                        CreatedDate = DateTime.Now,
                        Id = Guid.NewGuid(),
                        SelectApplicableId = firstApplicantTickApplicable,
                        CSDFormPath = _csdCompletedForm,
                        ReferenceNo = Utilities.GenerateApplicationReference(),
                        StatusId = 1,
                        BranchCode = Utilities.GetRandomBranchCode(),
                        Password = Utilities.EncodeBase64(password),
                        InitialInvestmentAmount = initialInvestmentAmt,
                        MODE_OF_APP = Utilities.GetSessionUser() == null ? "O" : "W",
                        InvestmentObjective = investmentObjective,
                        CreatedBy = currentUser != null ? currentUser.Username : "CLIENT"
                    };

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
                    context.Accounts.Add(account);
                    if (staffRefCode > 0)
                    {
                        var findStaff = context.StaffRefLists.FirstOrDefault(x => x.Code == staffRefCode);
                        if (findStaff != null)
                        {
                            account.StaffRefCode = findStaff.Code;
                        }
                    }
                    //set up first applicant
                    var firstApplicant = new AccountMember
                    {
                        Id = Guid.NewGuid(),
                        AccountId = account.Id,
                        TitleId = firstApplicantTitle,
                        Lname = firstApplicantSurname,
                        Fname = firstApplicantFname,
                        Othername = firstApplicantOname,
                        NationalityId = firstApplicantNationality,
                        DOB = firstApplicantDOB,
                        Gender = firstApplicantGender,
                        PlaceOfBirth = firstApplicantPlaceOfBirth,
                        MaritalStatusId = firstApplicantMaritalStatus,
                        MothersMaidenName = firstApplicantMotherMaidenName,
                        MaidenName = firstApplicantMaidenName,
                        ResidentialAddressFull = firstApplicantFullResidentialAddress,
                        ResidentialCity = firstApplicantResidentialCity,
                        ResidentialZipCode = firstApplicantResidentialZipCode,
                        ResidentialCountryId = firstApplicantResidentialCountry,
                        MailingAddressFull = firstApplicantFullMailingAddress,
                        MailingAddressCity = firstApplicantMailingCity,
                        MailingAddressZipCode = firstApplicantMailingZipCode,
                        MailingAddressCountryId = firstApplicantMailingCountry,
                        SelectWhereApplicableId = firstApplicantTickApplicable,
                        Telephone = firstApplicantTel,
                        Mobile = firstApplicantMobile,
                        Fax = firstApplicantFax,
                        Occupation = firstApplicantOccupation,
                        Email = firstApplicantEmail,
                        IdTypeId = firstApplicantIdType,
                        IdNumber = firstApplicantIdNumber,
                        IssuingAuthority = firstApplicantIdIssuingAuthority,
                        IdCardIssueDate = firstApplicantIdCardIssueDate,
                        IdCardExpiryDate = firstApplicantIdCardExpDate,
                        IdPath = _firstApplicantIdPhoto,
                        SignaturePath = !string.IsNullOrEmpty(_firstApplicantSignatureCapture) ? Utilities.SaveBase64AsImage(Guid.NewGuid().ToString().ToLower(), "png", _firstApplicantSignatureCapture) : _firstApplicantSignature,
                        CreatedDate = DateTime.Now,
                        ResidentPermitNumber = residentPermitNumber,
                        ResidentPermitNumberPlaceOfIssue = residentPermitNumberPlaceOfIssue,
                        ResidentPermitNumberIssueDate = residentPermitIssueDate,
                        ResidentPermitNumberExpiryDate = residentPermitExpiryDate,
                        GhanaCardNo = ghCardNo,
                        Profession = profession,
                        ProfessionalLicenceNumber = professionLicenceNumber,
                    };
                    context.AccountMembers.Add(firstApplicant);
                    context.SaveChanges();

                    if (accountType == 2)
                    {//joint
                        var jointApplicant = new AccountMember
                        {
                            Id = Guid.NewGuid(),
                            AccountId = account.Id,
                            TitleId = jointApplicantTitle,
                            Lname = jointApplicantSurname,
                            Fname = jointApplicantFname,
                            Othername = jointApplicantOname,
                            NationalityId = jointApplicantNationality,
                            DOB = jointApplicantDOB,
                            Gender = jointApplicantGender,
                            PlaceOfBirth = jointApplicantPlaceOfBirth,
                            ResidentialAddressFull = jointApplicantResidentialAddress,
                            Telephone = jointApplicantTelephone,
                            Mobile = jointApplicantMobile,
                            Fax = jointApplicantResidentialFax,
                            Occupation = jointApplicantOccupation,
                            Email = jointApplicantMailingEmail,
                            IdTypeId = jointApplicantIdType,
                            IdNumber = jointApplicantIdNumber,
                            IssuingAuthority = jointApplicantIdIssuingAuthority,
                            IdCardIssueDate = jointApplicantIdCardIssueDate,
                            IdCardExpiryDate = jointApplicantIdCardExpDate,
                            IdPath = _jointApplicantIdPhoto,
                            SignaturePath = !string.IsNullOrEmpty(_jointApplicantSignatureCapture) ? Utilities.SaveBase64AsImage(Guid.NewGuid().ToString().ToLower(), "png", _jointApplicantSignatureCapture) : _jointApplicantSignature,
                            CreatedDate = DateTime.Now,
                            ResidentPermitNumber = residentPermitNumber2,
                            ResidentPermitNumberPlaceOfIssue = residentPermitNumberPlaceOfIssue2,
                            ResidentPermitNumberIssueDate = residentPermitIssueDate2,
                            ResidentPermitNumberExpiryDate = residentPermitExpiryDate2,
                            GhanaCardNo = ghCardNo2,
                            Profession = profession2,
                            ProfessionalLicenceNumber = professionLicenceNumber2,
                        };
                        context.AccountMembers.Add(jointApplicant);
                    }
                    if (accountType == 3)
                    {//itf
                        var itfApplicant = new AccountMember
                        {
                            Id = Guid.NewGuid(),
                            AccountId = account.Id,
                            Lname = itfApplicantSurname,
                            Fname = itfApplicantFname,
                            Othername = itfApplicantOname,
                            NationalityId = itfApplicantNationality,
                            DOB = itfApplicantDOB,
                            PlaceOfBirth = itfApplicantPlaceOfBirth,
                            Gender = itfApplicantGender,
                            IdTypeId = itfApplicantIdType,
                            IdNumber = itfApplicantIdNumber,
                            IssuingAuthority = itfApplicantIdIssuingAuthority,
                            IdCardIssueDate = itfApplicantIdCardIssueDate,
                            IdCardExpiryDate = itfApplicantIdCardExpDate,
                            IdPath = _itfApplicantIdPhoto,
                            CreatedDate = DateTime.Now
                        };
                        context.AccountMembers.Add(itfApplicant);
                    }
                    context.SaveChanges();
                    //next of kin
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

                    //authorisedPerson
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
                            IdPath = !string.IsNullOrEmpty(_firstJointItfAuthorisedPerson) ? _firstJointItfAuthorisedPerson : null,
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
                        context.AccountAuthorisedPersons.Add(authorisedPerson);
                    }

                    //settlement details
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
                    var institutionalAccount = new Account
                    {
                        CSDFormPath = _csdCompletedForm,
                        InvestmentTypeId = instInvestmentType,
                        AccountNumber = null,
                        CSDNumber = null,
                        Id = Guid.NewGuid(),
                        InstitutionClientName = instNameOfClient,
                        ExpectedAccountActivityId = expectedAccountActivityId,
                        InstitutionalPrincipalBroker = instPricipalBroker,
                        InstitutionNatureOfBusiness = instNatureOfBusiness,
                        InsstitutionalCountryOfIncorporation = instCountryOfResidence,
                        InstitutionRegistrationNumber = instRegistrationNumber,
                        TIN = tin,
                        MailingAddressFull = instFullMailingAddress,
                        MailingAddressCity = instMailingAddressCity,
                        InstMailingAddressCountryId = instMailAddressCountryOfResidence,
                        StreetAddressFull = instFullStreetAddress,
                        InstStreetAddressCity = instSteetAddressCity,
                        InsStreetAddressZipCode = instSteetAddressZipCode,
                        InsStreetAddressTel = instTelephone,
                        InsStreetAddressFax = instFax,
                        InsStreetAddressEmail = instEmail,
                        InstCompanyType = instCompanyType,
                        RegionalInvestmentId = instRegionalInvestmentId,
                        FrequencyOfStatementsId = instStatementFreq,
                        AccountTypeId = accountType,
                        CreatedDate = DateTime.Now,
                        SignatureTypeId = instnumberOfSignatories,
                        InstOtherDetails = insOtherDetails,
                        StatusId = 1,
                        BranchCode = Utilities.GetRandomBranchCode(),
                        ReferenceNo = Utilities.GenerateApplicationReference(),
                        Password = Utilities.EncodeBase64(password),
                        MODE_OF_APP = Utilities.GetSessionUser() == null ? "O" : "W",
                        InvestmentObjective = investmentObjective,
                        CreatedBy = currentUser != null ? currentUser.Username : "CLIENT"
                    };
                    if (instSteetAddressCountry > 0)
                    {
                        institutionalAccount.InsStreetAddressCountryId = instSteetAddressCountry;
                    }

                    if (staffRefCode > 0)
                    {
                        var findStaff = context.StaffRefLists.FirstOrDefault(x => x.Code == staffRefCode);
                        if (findStaff != null)
                        {
                            institutionalAccount.StaffRefCode = findStaff.Code;
                        }
                    }
                    accountId = institutionalAccount.Id.ToString();
                    context.Accounts.Add(institutionalAccount);
                    context.SaveChanges();


                    context.AccountInsCustodyAccountDetails.Add(new AccountInsCustodyAccountDetail
                    {

                        AccountId = institutionalAccount.Id,
                        CustodianName = custodyDetailsName,
                        Telephone = custodyDetailsPhone,
                        Address = custodyDetailsAddress,
                        Id = Guid.NewGuid(),
                        CashAccountNumber = custodyDetailsCashAccNumber,
                        Fax = custodyDetailsFax,
                        CreatedDate = DateTime.Now,
                        SecuritiesAccountNumber = custodyDetailsSecuritiesAccNumber,
                    });

                    var settlementDetails = new AccountSettlementDetail
                    {
                        CorrespondentBankSwiftCode = intSettlementDetailsCorrespondentBankSwiftNo,
                        IntermediaryBankSwiftCode = intSettlementDetailsIntermediaryBankSwiftNo,
                        NameOfBeneficiary = intSettlementDetailsNameOfBeneficiary,
                        BIC = intSettlementDetailsBIC,
                        AccountNumber = intSettlementDetailsAccountNumber,
                        MarginTradingOption = instMarginTrationOption,
                        AccountId = institutionalAccount.Id,
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

                    context.AccountInstructionEmploymentDetails.Add(new AccountInstructionEmploymentDetail
                    {
                        ModeOfInstructionId = intModeOfInstruction,
                        ModeOfNotificationId = intModeOfNotification,
                        SourceOfFundId = intSourceOfIncome,
                        AccountId = institutionalAccount.Id,
                        CreatedDate = DateTime.Now,
                        Id = Guid.NewGuid(),
                        SourceOfFundsIds = string.Join(",", intSourceOfIncomeIds.Select(x => x.ToString()))
                    });


                    //authosied Officers
                    var authorisedOfficer1 = new AccountAuthorisedPerson
                    {
                        Id = Guid.NewGuid(),
                        AccountId = institutionalAccount.Id,
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
                        IdPath = !string.IsNullOrEmpty(_instAuthorisedOfficer1PhotoId) ? _instAuthorisedOfficer1PhotoId : null,
                        MailingAddress = null,
                        SignaturePath = null,
                    };
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
                    context.SaveChanges();

                    //officer 2
                    var authorisedOfficer2 = new AccountAuthorisedPerson
                    {
                        Id = Guid.NewGuid(),
                        AccountId = institutionalAccount.Id,
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
                        IdPath = !string.IsNullOrEmpty(_instAuthorisedOfficer2PhotoId) ? _instAuthorisedOfficer2PhotoId : null,
                        MailingAddress = null,
                        SignaturePath = null,
                    };
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


                    //officer 3
                    if (intAuthorisedOfficer3Name != null || intAuthorisedOfficer3Name != "")
                    {
                        var authorisedOfficer3 = new AccountAuthorisedPerson()
                        {
                            Id = Guid.NewGuid(),
                            AccountId = institutionalAccount.Id,
                            CreatedDate = DateTime.Now.AddSeconds(10.0),
                            Name = intAuthorisedOfficer3Name,
                            Tel = intAuthorisedOfficer3Tel,
                            Mobile = intAuthorisedOfficer3Mobile,
                            Fax = intAuthorisedOfficer3Fax,
                            Email = intAuthorisedOfficer3Email,
                            RelationToAccountHolder = intAuthorisedOfficer3Relation,
                            StreetAddress = intAuthorisedOfficer3StreetAddress,
                            City = intAuthorisedOfficer3City,
                            ZipCode = intAuthorisedOfficer3ZipCode,
                            IdNumber = intAuthorisedOfficer3IDNumber,
                            IdIssueAuthority = intAuthorisedOfficer3IssueAuthority,
                            IssueDate = intAuthorisedOfficer3IdCardIssueDate,
                            IDExpiryDate = intAuthorisedOfficer3IdCardIssueExpDate,
                            IdPath = !string.IsNullOrEmpty(_instAuthorisedOfficer3PhotoId) ? _instAuthorisedOfficer3PhotoId : null,
                            MailingAddress = null,
                            SignaturePath = null
                        };
                        if (intAuthorisedOfficer3Title > 0)
                            authorisedOfficer3.TitleId = new int?(intAuthorisedOfficer3Title);
                        if (intAuthorisedOfficer3CountryId > 0)
                            authorisedOfficer3.CountryId = new int?(intAuthorisedOfficer3CountryId);
                        if (intAuthorisedOfficer3IDType > 0)
                            authorisedOfficer3.IdType = new int?(intAuthorisedOfficer3IDType);
                        context.AccountAuthorisedPersons.Add(authorisedOfficer3);
                        context.SaveChanges();
                    }

                    //oficer 4
                    if (intAuthorisedOfficer4Name != null || intAuthorisedOfficer4Name != "")
                    {
                        AccountAuthorisedPerson authorisedOfficer4 = new AccountAuthorisedPerson()
                        {
                            Id = Guid.NewGuid(),
                            AccountId = institutionalAccount.Id,
                            CreatedDate = DateTime.Now.AddSeconds(15.0),
                            Name = intAuthorisedOfficer4Name,
                            Tel = intAuthorisedOfficer4Tel,
                            Mobile = intAuthorisedOfficer4Mobile,
                            Fax = intAuthorisedOfficer4Fax,
                            Email = intAuthorisedOfficer4Email,
                            RelationToAccountHolder = intAuthorisedOfficer4Relation,
                            StreetAddress = intAuthorisedOfficer4StreetAddress,
                            City = intAuthorisedOfficer4City,
                            ZipCode = intAuthorisedOfficer4ZipCode,
                            IdNumber = intAuthorisedOfficer4IDNumber,
                            IdIssueAuthority = intAuthorisedOfficer4IssueAuthority,
                            IssueDate = intAuthorisedOfficer4IdCardIssueDate,
                            IDExpiryDate = intAuthorisedOfficer4IdCardIssueExpDate,
                            IdPath = !string.IsNullOrEmpty(_instAuthorisedOfficer4PhotoId) ? _instAuthorisedOfficer4PhotoId : (string)null,
                            MailingAddress = (string)null,
                            SignaturePath = (string)null
                        };
                        if (intAuthorisedOfficer4Title > 0)
                            authorisedOfficer4.TitleId = new int?(intAuthorisedOfficer4Title);
                        if (intAuthorisedOfficer4CountryId > 0)
                            authorisedOfficer4.CountryId = new int?(intAuthorisedOfficer4CountryId);
                        if (intAuthorisedOfficer4IDType > 0)
                            authorisedOfficer4.IdType = new int?(intAuthorisedOfficer4IDType);
                        context.AccountAuthorisedPersons.Add(authorisedOfficer4);
                        context.SaveChanges();
                    }


                    //officer 5
                    if (intAuthorisedOfficer5Name != null || intAuthorisedOfficer5Name != "")
                    {
                        AccountAuthorisedPerson authorisedOfficer5 = new AccountAuthorisedPerson()
                        {
                            Id = Guid.NewGuid(),
                            AccountId = institutionalAccount.Id,
                            CreatedDate = DateTime.Now.AddSeconds(20.0),
                            Name = intAuthorisedOfficer5Name,
                            Tel = intAuthorisedOfficer5Tel,
                            Mobile = intAuthorisedOfficer5Mobile,
                            Fax = intAuthorisedOfficer5Fax,
                            Email = intAuthorisedOfficer5Email,
                            RelationToAccountHolder = intAuthorisedOfficer5Relation,
                            StreetAddress = intAuthorisedOfficer5StreetAddress,
                            City = intAuthorisedOfficer5City,
                            ZipCode = intAuthorisedOfficer5ZipCode,
                            IdNumber = intAuthorisedOfficer5IDNumber,
                            IdIssueAuthority = intAuthorisedOfficer5IssueAuthority,
                            IssueDate = intAuthorisedOfficer5IdCardIssueDate,
                            IDExpiryDate = intAuthorisedOfficer5IdCardIssueExpDate,
                            IdPath = !string.IsNullOrEmpty(_instAuthorisedOfficer5PhotoId) ? _instAuthorisedOfficer5PhotoId : (string)null,
                            MailingAddress = (string)null,
                            SignaturePath = (string)null
                        };
                        if (intAuthorisedOfficer5Title > 0)
                            authorisedOfficer5.TitleId = new int?(intAuthorisedOfficer5Title);
                        if (intAuthorisedOfficer5CountryId > 0)
                            authorisedOfficer5.CountryId = new int?(intAuthorisedOfficer5CountryId);
                        if (intAuthorisedOfficer5IDType > 0)
                            authorisedOfficer5.IdType = new int?(intAuthorisedOfficer5IDType);
                        context.AccountAuthorisedPersons.Add(authorisedOfficer5);
                        context.SaveChanges();
                    }


                    //trading contacts
                    context.AccountInsTradingContacts.Add(new AccountInsTradingContact
                    {

                        Id = Guid.NewGuid(),
                        AccountId = institutionalAccount.Id,
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
                            AccountId = institutionalAccount.Id,
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
                    context.AccountInstSignatoriesMandates.Add(new AccountInstSignatoriesMandate
                    {

                        Id = Guid.NewGuid(),
                        AccountId = institutionalAccount.Id,
                        CreatedDate = DateTime.Now,
                        Name = instSignName1,
                        Position = instSignPosition1,
                        //SignaturePath = _InstSignatory1!=null? SaveFileUpload(_InstSignatory1):null
                        SignaturePath = !string.IsNullOrEmpty(_instSign1Capture) ?
                        Utilities.SaveBase64AsImage(Guid.NewGuid().ToString().ToLower(), "png", _instSign1Capture) :
                        _instSignatory1,

                    });

                    context.AccountInstSignatoriesMandates.Add(new AccountInstSignatoriesMandate
                    {

                        Id = Guid.NewGuid(),
                        AccountId = institutionalAccount.Id,
                        CreatedDate = DateTime.Now,
                        Name = instSignName2,
                        Position = instSignPosition2,
                        // SignaturePath = _InstSignatory2 != null ? SaveFileUpload(_InstSignatory2) : null
                        SignaturePath = !string.IsNullOrEmpty(_instSign2Capture) ?
                        Utilities.SaveBase64AsImage(Guid.NewGuid().ToString().ToLower(), "png", _instSign2Capture) :
                        _instSignatory2,
                    });

                    if (!string.IsNullOrEmpty(instSignName3))
                    {
                        var sign3 = new AccountInstSignatoriesMandate
                        {

                            Id = Guid.NewGuid(),
                            AccountId = institutionalAccount.Id,
                            CreatedDate = DateTime.Now,
                            Name = instSignName3,
                            Position = instSignPosition3,
                        };
                        if (!string.IsNullOrEmpty(_instSign3Capture))
                        {
                            sign3.SignaturePath = Utilities.SaveBase64AsImage(Guid.NewGuid().ToString().ToLower(), "png", _instSign3Capture);
                        }
                        else if (!string.IsNullOrEmpty(_instSignatory3) && sign3.SignaturePath == null)
                        {
                            sign3.SignaturePath = _instSignatory3;
                        }
                        context.AccountInstSignatoriesMandates.Add(sign3);
                    }
                    if (!string.IsNullOrEmpty(instSignName4))
                    {
                        var sign4 = new AccountInstSignatoriesMandate
                        {

                            Id = Guid.NewGuid(),
                            AccountId = institutionalAccount.Id,
                            CreatedDate = DateTime.Now,
                            Name = instSignName4,
                            Position = instSignPosition4,
                        };
                        if (!string.IsNullOrEmpty(_instSign4Capture))
                        {
                            sign4.SignaturePath = Utilities.SaveBase64AsImage(Guid.NewGuid().ToString().ToLower(), "png", _instSign4Capture);
                        }
                        else if (!string.IsNullOrEmpty(_instSignatory4) && sign4.SignaturePath == null)
                        {
                            sign4.SignaturePath = _instSignatory4;
                        }
                        context.AccountInstSignatoriesMandates.Add(sign4);
                    }
                    context.SaveChanges();
                }

                context.SaveChanges();



                if (sourceOfFunds != null && sourceOfFunds.Any())
                {
                    var sourceOfFundsList = new List<string>();
                    foreach (var item in sourceOfFunds)
                    {
                        sourceOfFundsList.Add(item.ToString());
                    }

                    var expectedAccountActivity = new AccountExpectedActivity
                    {
                        AccountId = Guid.Parse(accountId),
                        ExpectedTopUpAmt = expectedTopUpAmt,
                        ExpectedWithdrawalAmt = expectedWithdrawalAmt,
                        SourceOfFundsOthers = sourceOfFundsOthers,
                        TopUpoptionOthers = topUpoptionOthers,
                        Id = Guid.NewGuid(),
                        WithdrawalOptionsOthers = withdrawalOptionsOthers,
                        SourceOfFundIs = string.Join(",", sourceOfFundsList)
                    };
                    if (topUpoptionId > 1)
                    {
                        expectedAccountActivity.TopUpoptionId = topUpoptionId;
                    }
                    if (withdrawalOptionsId > 0)
                    {
                        expectedAccountActivity.WithdrawalOptionsId = withdrawalOptionsId;
                    }
                    context.AccountExpectedActivities.Add(expectedAccountActivity);
                }
                context.SaveChanges();

                //beneficiary
                var firstBeneficiary = new AccountBeneficiary
                {
                    Id = Guid.NewGuid(),
                    AccountId = Guid.Parse(accountId),
                    CreatedDate = DateTime.Now,
                    Fullname = beneficiary1Fname + " " + beneficiary1Surname,
                    Phone = "NA",
                    PercentageAllocation = beneficiaryPcnt1,
                    Relation = beneficiary1Relation,
                    TitleId = beneficiary1TitleId,
                    OtherTitleDetails = beneficiary1TitleOther,
                    Surname = beneficiary1Surname,
                    Othernames = beneficiary1Fname,
                    BirthDate = beneficiary1BirthDate,
                    PlaceOfBirth = beneficiary1placeOfBirth,
                    IDNumber = beneficiary1IDNumber,
                    IDPlaceOfIssue = beneficiary1IDPlaceOfIssue,
                    IDIssueDate = beneficiary1IDIssueDate,
                    IDExpiryDate = beneficiary1ExpiryDate
                };
                if (beneficiary1GenderId > 0)
                {
                    firstBeneficiary.GenderId = beneficiary1GenderId;
                }
                if (beneficiary1MaritalStatusId > 0)
                {
                    firstBeneficiary.MaritalStatusId = beneficiary1MaritalStatusId;
                }
                if (beneficiary1NationalityId > 0)
                {
                    firstBeneficiary.CountryOfOriginId = beneficiary1NationalityId;
                }
                if (beneficiary1countryOfResidenceId > 0)
                {
                    firstBeneficiary.CountryOfResidenceId = beneficiary1countryOfResidenceId;
                }
                if (beneficiary1photoIdType > 0)
                {
                    firstBeneficiary.IDCardTypeId = beneficiary1photoIdType;
                }
                context.AccountBeneficiaries.Add(firstBeneficiary);


                if (!string.IsNullOrEmpty(beneficiary2Surname) && !string.IsNullOrEmpty(beneficiary2Fname))
                {
                    var secondBeneficiary = new AccountBeneficiary
                    {
                        AccountId = Guid.Parse(accountId),
                        Id = Guid.NewGuid(),
                        CreatedDate = DateTime.Now,
                        Fullname = beneficiary2Fname + " " + beneficiary2Surname,
                        PercentageAllocation = beneficiaryPcnt2,
                        Phone = "NA",
                        Relation = beneficiary2Relation,
                        OtherTitleDetails = beneficiary2TitleOther,
                        Surname = beneficiary2Surname,
                        Othernames = beneficiary2Fname,
                        BirthDate = beneficiary2BirthDate,
                        PlaceOfBirth = beneficiary2placeOfBirth,
                        IDNumber = beneficiary2IDNumber,
                        IDPlaceOfIssue = beneficiary2IDPlaceOfIssue,
                        IDIssueDate = beneficiary2IDIssueDate,
                        IDExpiryDate = beneficiary2ExpiryDate,



                    };
                    if (beneficiary2TitleId > 0)
                    {
                        secondBeneficiary.TitleId = beneficiary2TitleId;
                    }
                    if (beneficiary2GenderId > 0)
                    {
                        secondBeneficiary.GenderId = beneficiary2GenderId;
                    }
                    if (beneficiary2MaritalStatusId > 0)
                    {
                        secondBeneficiary.MaritalStatusId = beneficiary2MaritalStatusId;
                    }
                    if (beneficiary2NationalityId > 0)
                    {
                        secondBeneficiary.CountryOfOriginId = beneficiary2NationalityId;
                    }
                    if (beneficiary2countryOfResidenceId > 0)
                    {
                        secondBeneficiary.CountryOfResidenceId = beneficiary2countryOfResidenceId;
                    }
                    if (beneficiary2photoIdType > 1)
                    {
                        secondBeneficiary.IDCardTypeId = beneficiary2photoIdType;
                    }
                    context.AccountBeneficiaries.Add(secondBeneficiary);

                }

                //third beneficiary
                if (!string.IsNullOrEmpty(beneficiary3Surname) && !string.IsNullOrEmpty(beneficiary3Fname))
                {
                    var thirdBeneficiary = new AccountBeneficiary
                    {
                        AccountId = Guid.Parse(accountId),
                        Id = Guid.NewGuid(),
                        CreatedDate = DateTime.Now,
                        Fullname = beneficiary3Fname + " " + beneficiary3Surname,
                        PercentageAllocation = beneficiaryPcnt3,
                        Phone = "NA",
                        Relation = beneficiary3Relation,
                        OtherTitleDetails = beneficiary3TitleOther,
                        Surname = beneficiary3Surname,
                        Othernames = beneficiary3Fname,
                        BirthDate = beneficiary3BirthDate,
                        PlaceOfBirth = beneficiary3placeOfBirth,
                        IDNumber = beneficiary3IDNumber,
                        IDPlaceOfIssue = beneficiary3IDPlaceOfIssue,
                        IDIssueDate = beneficiary3IDIssueDate,
                        IDExpiryDate = beneficiary3ExpiryDate,



                    };
                    if (beneficiary3TitleId > 0)
                    {
                        thirdBeneficiary.TitleId = beneficiary3TitleId;
                    }
                    if (beneficiary3GenderId > 0)
                    {
                        thirdBeneficiary.GenderId = beneficiary3GenderId;
                    }
                    if (beneficiary3MaritalStatusId > 0)
                    {
                        thirdBeneficiary.MaritalStatusId = beneficiary3MaritalStatusId;
                    }
                    if (beneficiary3NationalityId > 0)
                    {
                        thirdBeneficiary.CountryOfOriginId = beneficiary3NationalityId;
                    }
                    if (beneficiary3countryOfResidenceId > 0)
                    {
                        thirdBeneficiary.CountryOfResidenceId = beneficiary3countryOfResidenceId;
                    }
                    if (beneficiary3photoIdType > 1)
                    {
                        thirdBeneficiary.IDCardTypeId = beneficiary3photoIdType;
                    }
                    context.AccountBeneficiaries.Add(thirdBeneficiary);

                }

                //fourth beneficiary
                if (!string.IsNullOrEmpty(beneficiary4Surname) && !string.IsNullOrEmpty(beneficiary4Fname))
                {
                    var fourthBeneficiary = new AccountBeneficiary
                    {
                        AccountId = Guid.Parse(accountId),
                        Id = Guid.NewGuid(),
                        CreatedDate = DateTime.Now,
                        Fullname = beneficiary4Fname + " " + beneficiary4Surname,
                        PercentageAllocation = beneficiaryPcnt4,
                        Phone = "NA",
                        Relation = beneficiary4Relation,
                        OtherTitleDetails = beneficiary4TitleOther,
                        Surname = beneficiary4Surname,
                        Othernames = beneficiary4Fname,
                        BirthDate = beneficiary4BirthDate,
                        PlaceOfBirth = beneficiary4placeOfBirth,
                        IDNumber = beneficiary4IDNumber,
                        IDPlaceOfIssue = beneficiary4IDPlaceOfIssue,
                        IDIssueDate = beneficiary4IDIssueDate,
                        IDExpiryDate = beneficiary4ExpiryDate,



                    };
                    if (beneficiary4TitleId > 0)
                    {
                        fourthBeneficiary.TitleId = beneficiary4TitleId;
                    }
                    if (beneficiary4GenderId > 0)
                    {
                        fourthBeneficiary.GenderId = beneficiary4GenderId;
                    }
                    if (beneficiary4MaritalStatusId > 0)
                    {
                        fourthBeneficiary.MaritalStatusId = beneficiary4MaritalStatusId;
                    }
                    if (beneficiary4NationalityId > 0)
                    {
                        fourthBeneficiary.CountryOfOriginId = beneficiary4NationalityId;
                    }
                    if (beneficiary4countryOfResidenceId > 0)
                    {
                        fourthBeneficiary.CountryOfResidenceId = beneficiary4countryOfResidenceId;
                    }
                    if (beneficiary4photoIdType > 1)
                    {
                        fourthBeneficiary.IDCardTypeId = beneficiary4photoIdType;
                    }
                    context.AccountBeneficiaries.Add(fourthBeneficiary);

                }

                //fifth beneficiary

                if (!string.IsNullOrEmpty(beneficiary5Surname) && !string.IsNullOrEmpty(beneficiary5Fname))
                {
                    var fifthBeneficiary = new AccountBeneficiary
                    {
                        AccountId = Guid.Parse(accountId),
                        Id = Guid.NewGuid(),
                        CreatedDate = DateTime.Now,
                        Fullname = beneficiary5Fname + " " + beneficiary5Surname,
                        PercentageAllocation = beneficiaryPcnt5,
                        Phone = "NA",
                        Relation = beneficiary5Relation,
                        OtherTitleDetails = beneficiary5TitleOther,
                        Surname = beneficiary5Surname,
                        Othernames = beneficiary5Fname,
                        BirthDate = beneficiary5BirthDate,
                        PlaceOfBirth = beneficiary5placeOfBirth,
                        IDNumber = beneficiary5IDNumber,
                        IDPlaceOfIssue = beneficiary5IDPlaceOfIssue,
                        IDIssueDate = beneficiary5IDIssueDate,
                        IDExpiryDate = beneficiary5ExpiryDate,



                    };
                    if (beneficiary5TitleId > 0)
                    {
                        fifthBeneficiary.TitleId = beneficiary5TitleId;
                    }
                    if (beneficiary5GenderId > 0)
                    {
                        fifthBeneficiary.GenderId = beneficiary5GenderId;
                    }
                    if (beneficiary5MaritalStatusId > 0)
                    {
                        fifthBeneficiary.MaritalStatusId = beneficiary5MaritalStatusId;
                    }
                    if (beneficiary5NationalityId > 0)
                    {
                        fifthBeneficiary.CountryOfOriginId = beneficiary5NationalityId;
                    }
                    if (beneficiary5countryOfResidenceId > 0)
                    {
                        fifthBeneficiary.CountryOfResidenceId = beneficiary5countryOfResidenceId;
                    }
                    if (beneficiary5photoIdType > 1)
                    {
                        fifthBeneficiary.IDCardTypeId = beneficiary5photoIdType;
                    }
                    context.AccountBeneficiaries.Add(fifthBeneficiary);

                }





                //file uploads
                if (!string.IsNullOrEmpty(passportPhotosList))
                {
                    foreach (var item in passportPhotosList.Split(','))
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            context.AccountFileUploads.Add(new AccountFileUpload
                            {
                                AccountId = Guid.Parse(accountId),
                                CreatedDate = DateTime.Now,
                                Id = Guid.NewGuid(),
                                IsActive = true,
                                Path = item,
                                TypeId = 1
                            });

                        }
                    }
                    context.SaveChanges();
                }

                if (!string.IsNullOrEmpty(residenceList))
                {
                    foreach (var item in residenceList.Split(','))
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            context.AccountFileUploads.Add(new AccountFileUpload
                            {
                                AccountId = Guid.Parse(accountId),
                                CreatedDate = DateTime.Now,
                                Id = Guid.NewGuid(),
                                IsActive = true,
                                Path = item,
                                TypeId = 2
                            });

                        }
                    }
                    context.SaveChanges();
                }
                if (!string.IsNullOrEmpty(businessDocsList))
                {
                    foreach (var item in businessDocsList.Split(','))
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            context.AccountFileUploads.Add(new AccountFileUpload
                            {
                                AccountId = Guid.Parse(accountId),
                                CreatedDate = DateTime.Now,
                                Id = Guid.NewGuid(),
                                IsActive = true,
                                Path = item,
                                TypeId = 3
                            });

                        }
                    }
                    context.SaveChanges();
                }
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
                //eti
                context.AccountETIs.Add(new AccountETI
                {
                    AccountId = Guid.Parse(accountId),
                    CreatedDate = DateTime.Now,
                    Text1 = indemnityTxt1,
                    Id = Guid.NewGuid(),
                    Text2 = indemnityTxt2,
                    Text3 = indemnityTxt3,
                    Text4 = indemnityTxt4,
                    Text5 = null,
                    Name1 = indemnityName1,
                    Email1 = indemnityEmail1,
                    Name2 = indemnityName2,
                    Email2 = indemnityEmail2,

                });

                context.SaveChanges();
                return RedirectToAction("AccountCreated", new { accountId = Guid.Parse(accountId), hkey = Guid.NewGuid().ToString().Encrypt() });
            }
            catch (Exception ex)
            {

                Logger.Instance.logError(ex);
                ViewBag.Message = ex.Message;
                return View();
            }

        }


        public ActionResult AccountCreated(Guid accountId, string hkey)
        {
            var context = new DBLAccountOpeningContext();
            var account = context.Accounts.Find(accountId);
            return View(account);
        }


    }
}