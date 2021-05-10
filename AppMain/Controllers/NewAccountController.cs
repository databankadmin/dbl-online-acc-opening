using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppLogger;
using AppMain.Providers;
using AppUtils;
using DBHelper.Schema;

namespace AppMain.Controllers
{
    public class NewAccountController : Controller
    {
        public static  HttpPostedFileBase _FirstApplicantIdPhoto { get; set; }
        public static HttpPostedFileBase _JointApplicantIdPhoto { get; set; }
        public static HttpPostedFileBase _ItfApplicantIdPhoto { get; set; }
        public static HttpPostedFileBase _FirstJointItfAuthorisedPerson { get; set; }
        public static HttpPostedFileBase _InstAuthorisedOfficer1PhotoId { get; set; }
        public static HttpPostedFileBase _InstAuthorisedOfficer2PhotoId { get; set; }
        public static HttpPostedFileBase _InstSignatory1 { get; set; }
        public static HttpPostedFileBase _InstSignatory2 { get; set; }
        public static HttpPostedFileBase _InstSignatory3 { get; set; }
        public static HttpPostedFileBase _InstSignatory4 { get; set; }
        public static List<HttpPostedFileBase> _ProofOfResidenceFiles { get; set; }
        public static List<HttpPostedFileBase> _OtherBusinessFiles { get; set; }
        public static HttpPostedFileBase _CsdCompletedForm { get; set; }
        public static List<HttpPostedFileBase> _PassportPhotos { get; set; }

        public static HttpPostedFileBase _FirstApplicantSignature { get; set; }
        public static HttpPostedFileBase _JointApplicantSignature { get; set; }

        // GET: NewAccount
        public ActionResult Initiate(string acc_type, string param)
        {
            MvcApplication.AccountType =int.Parse(Utilities.DecodeBase64(acc_type));
            _ProofOfResidenceFiles = new List<HttpPostedFileBase>();
            _OtherBusinessFiles = new List<HttpPostedFileBase>();
            _PassportPhotos = new List<HttpPostedFileBase>();
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



        [HttpPost]
        public ActionResult GetFileUploads(HttpPostedFileBase myPhoto)
        {

            return Json(new { status = true, Message = "" });
        }

        [HttpPost]
        public ActionResult Initiate(int investmentType = 0, int firstApplicantTitle=0,string firstApplicantSurname=null,string firstApplicantFname=null, string firstApplicantOname=null,int firstApplicantNationality=0,
            string firstApplicantDOB=null,string firstApplicantPlaceOfBirth=null,int firstApplicantMaritalStatus=0,string firstApplicantMotherMaidenName=null,string firstApplicantFullResidentialAddress=null,string firstApplicantResidentialCity=null,
            string firstApplicantResidentialZipCode=null,int firstApplicantResidentialCountry=0, string firstApplicantFullMailingAddress=null,string firstApplicantMailingCity=null,string firstApplicantMailingZipCode=null,int firstApplicantMailingCountry=0,int firstApplicantTickApplicable=0,
            string firstApplicantTel=null,string firstApplicantMobile=null,string firstApplicantFax=null,string firstApplicantOccupation=null,string firstApplicantEmail=null,int firstApplicantIdType=0, string firstApplicantIdNumber=null, string firstApplicantIdIssuingAuthority=null,
            string firstApplicantIdCardIssueDate=null,string firstApplicantIdCardExpDate=null,
            int jointApplicantTitle=0, string jointApplicantSurname=null,string jointApplicantFname=null,string jointApplicantOname=null,int jointApplicantNationality=0,string jointApplicantDOB=null,string jointApplicantPlaceOfBirth=null,
            string jointApplicantTelephone=null,string jointApplicantMobile=null,string jointApplicantResidentialFax=null,string jointApplicantMailingEmail=null,string jointApplicantResidentialAddress=null,string jointApplicantOccupation=null,int jointApplicantIdType=0,string jointApplicantIdNumber=null,string jointApplicantIdIssuingAuthority=null,
            string jointApplicantIdCardIssueDate=null,string jointApplicantIdCardExpDate=null,
            string itfApplicantSurname=null,string itfApplicantFname=null,string itfApplicantOname=null,int itfApplicantNationality=0,string itfApplicantDOB=null,string itfApplicantPlaceOfBirth=null,int itfApplicantIdType=0, string itfApplicantIdNumber=null,
            string itfApplicantIdIssuingAuthority=null,string itfApplicantIdCardIssueDate=null,string itfApplicantIdCardExpDate=null,
            string nextOfKin1Name=null,string nextOfKin1Phone=null,string nextOfKin1Mobile=null,string nextOfKin1Fax=null, string nextOfKin1Relation=null,string nextOfKin1Email=null,
            string nextOfKin1MailAddress=null, string nextOfKin2Name=null,string nextOfKin2Phone=null,string nextOfKin2Mobile=null,string nextOfKin2Fax=null,
            string nextOfKin2Relation=null,string nextOfKin2Email=null,string nextOfKin2MailAddress=null,string actingAsNomineeName=null,
            string authorisedPersonName=null, string authorisedPersonTel=null,string authorisedPersonMobile=null,string authorisedPersonFax=null,string authorisedPersonRelation=null,string authorisedPersonEmail=null,
            string authorisedPersonMailAddress=null,string authorisedPersonCity=null,string authorisedPersonZipCode=null,int authorisedPersonZipCountryId=0,int authorisedPersonApplicantIdType=0,
            string authorisedPersonApplicantIdNumber=null,string firstApplicantIdAuthorisedPersonIssuingAuthority=null,string firstAuthorisedApplicantIdCardIssueDate=null,string firstApplicantIdAuthorisedCardExpDate=null,
            string settlementDetailsAccountName=null,string settlementDetailsAccountNumber=null, int settlementDetailsAccountBankId=0,
            string settlementDetailsAccountBankBranch=null,string settlementDetailsAccountBankSortCode=null,
            int instructionsEmploymentDetailsModeOfInstruction=0,int instructionsEmploymentDetailsModeOfNotification=0,int instructionsEmploymentDetailsSourceOfIncome=0,
            int instructionsEmploymentDetailsEmploymentStatus=0,string instructionsEmploymentDetailsPreviousOccupation=null,string instructionsEmploymentDetailsPreviousEmployer=null,
            string instructionsEmploymentDetailsCurrentOccupation=null,string instructionsEmploymentDetailsCurrentEmployer=null,string instructionsEmploymentDetailsCurrentEmployerAddress=null,string instructionsEmploymentDetailsCurrentEmployerFrom=null,string instructionsEmploymentDetailsCurrentEmployerTo=null,
            int annualIncomeId=0,int networthId=0, int investmentHorizonId=0,int objectivesId=0,int investmentKnowledgeId=0,int riskToleranceId=0,
            string onlineTradingFacility=null,string declarationHasBeenConvicted=null,string convictionDetails=null,string actingAsNominee=null,string singleAccDeclarationFullName=null,
            int numberOfSignatories=0, int instInvestmentType=0,string instNameOfClient=null,string instPricipalBroker=null,string instNatureOfBusiness=null,int instCountryOfResidence=0,string instRegistrationNumber=null,string instFullMailingAddress=null,string instMailingAddressCity=null,int instMailAddressCountryOfResidence=0,
            string instFullStreetAddress=null, string instSteetAddressCity=null,string instSteetAddressZipCode=null,int instSteetAddressCountry=0,string instTelephone=null,string instFax=null,string instEmail=null,string instCompanyType=null,int instRegionalInvestmentId=0,int instStatementFreq=0,
            string custodyDetailsName=null,string custodyDetailsPhone=null,string custodyDetailsAddress=null,string custodyDetailsFax=null,string custodyDetailsCashAccNumber=null,string custodyDetailsSecuritiesAccNumber=null,
            int intSettlementDetailsCorrespondentBankId=0,string intSettlementDetailsCorrespondentBankSwiftNo=null,int intSettlementDetailsIntermediaryBankId=0,string intSettlementDetailsIntermediaryBankSwiftNo=null,
            string intSettlementDetailsNameOfBeneficiary=null,string intSettlementDetailsBIC=null,string intSettlementDetailsAccountNumber=null,string instMarginTrationOption=null,
            string instOnlineTradingOption=null,int intModeOfInstruction=0,int intModeOfNotification=0,int intSourceOfIncome=0,
            string intAuthorisedOfficer1Name=null,string intAuthorisedOfficer1Tel=null,string intAuthorisedOfficer1Mobile=null,string intAuthorisedOfficer1Fax=null,int intAuthorisedOfficer1Title=0,
            string intAuthorisedOfficer1Email=null,string intAuthorisedOfficer1Relation=null,string intAuthorisedOfficer1StreetAddress=null,string intAuthorisedOfficer1City=null,string intAuthorisedOfficer1ZipCode=null,int intAuthorisedOfficer1CountryId=0,int intAuthorisedOfficer1IDType=0,string intAuthorisedOfficer1IDNumber=null,
            string intAuthorisedOfficer1IssueAuthority=null, string intAuthorisedOfficer1IdCardIssueDate=null,string intAuthorisedOfficer1IdCardIssueExpDate=null,
            string intAuthorisedOfficer2Name=null,string intAuthorisedOfficer2Tel=null,string intAuthorisedOfficer2Mobile=null,string intAuthorisedOfficer2Fax=null,int intAuthorisedOfficer2Title=0,string intAuthorisedOfficer2Email=null,string intAuthorisedOfficer2Relation=null,string intAuthorisedOfficer2StreetAddress=null,
            string intAuthorisedOfficer2City=null,string intAuthorisedOfficer2ZipCode=null,int intAuthorisedOfficer2CountryId=0,int intAuthorisedOfficer2IDType=0,string intAuthorisedOfficer2IDNumber=null,
            string intAuthorisedOfficer2IssueAuthority=null,string intAuthorisedOfficer2IdCardIssueDate=null,
            string intAuthorisedOfficer2IdCardIssueExpDate=null,int tradingContacts1Title=0, string tradingContacts1Name=null,string tradingContacts1Telephone=null,string tradingContacts1Mobile=null,string tradingContacts1Fax=null,
            string tradingContacts1Email=null,int tradingContacts2Title=0,string tradingContacts2Fax=null, string tradingContacts2Name=null,string tradingContacts2Telephone=null,string tradingContacts2Mobile=null,string tradingContacts2Email=null,
            string instSignName1=null,string instSignPosition1=null,string instSignName2=null,string instSignPosition2=null,string instSignName3=null,string instSignPosition3=null,string instSignName4=null,string instSignPosition4=null,
            int instnumberOfSignatories = 0,string insOtherDetails = null, 
            
            string indemnityTxt1=null,string indemnityTxt2=null,string indemnityTxt3=null,string indemnityTxt4=null, string indemnityName1=null,string indemnityName2=null,string indemnityEmail1=null,string indemnityEmail2=null,
            int yearsOfEmployment=0,string tin=null,string firstApplicantMaidenName=null,int statementFreqId = 0,int expectedAccountActivityId=0, string firstApplicantGender=null,string jointApplicantGender=null, string itfApplicantGender=null,
            List<string> remark=null

            )

        {
            try
            {
                int accountType = MvcApplication.AccountType;
                var context = new DBLAccountOpeningContext();
                string accountId = string.Empty;
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
                        CSDFormPath = SaveFileUpload(_CsdCompletedForm),
                    };
                    if (yearsOfEmployment > 0)
                    {
                        account.YearsOfWorkExperience = yearsOfEmployment;
                    }
                    accountId = account.Id.ToString();
                    context.Accounts.Add(account);
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
                        Gender=firstApplicantGender,
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
                        IdPath = SaveFileUpload(_FirstApplicantIdPhoto),
                        SignaturePath = SaveFileUpload(_FirstApplicantSignature),
                        CreatedDate = DateTime.Now
                    };
                    context.AccountMembers.Add(firstApplicant);
                    context.SaveChanges();

                    if (accountType==2)
                    {//joint
                        var jointApplicant = new AccountMember
                        {
                            Id = Guid.NewGuid(),
                            AccountId = account.Id,
                            TitleId = jointApplicantTitle,
                            Lname =jointApplicantSurname,
                            Fname = jointApplicantFname,
                            Othername = jointApplicantOname,
                            NationalityId = jointApplicantNationality,
                            DOB = jointApplicantDOB,
                            Gender=jointApplicantGender,
                            PlaceOfBirth = jointApplicantPlaceOfBirth,
                            ResidentialAddressFull = jointApplicantResidentialAddress,
                            Telephone = jointApplicantTelephone,
                            Mobile = jointApplicantMobile,
                            Fax = jointApplicantResidentialFax,
                            Occupation = jointApplicantOccupation,
                            Email = jointApplicantMailingEmail,
                            IdTypeId = jointApplicantIdType,
                            IdNumber = jointApplicantIdNumber,
                            IssuingAuthority =jointApplicantIdIssuingAuthority,
                            IdCardIssueDate = jointApplicantIdCardIssueDate,
                            IdCardExpiryDate = jointApplicantIdCardExpDate,
                            IdPath = SaveFileUpload(_JointApplicantIdPhoto),
                            SignaturePath = SaveFileUpload(_JointApplicantSignature),
                            CreatedDate = DateTime.Now
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
                            Gender=itfApplicantGender,
                            IdTypeId = itfApplicantIdType,
                            IdNumber = itfApplicantIdNumber,
                            IssuingAuthority =itfApplicantIdIssuingAuthority,
                            IdCardIssueDate = itfApplicantIdCardIssueDate,
                            IdCardExpiryDate =itfApplicantIdCardExpDate,
                            IdPath = SaveFileUpload(_ItfApplicantIdPhoto),
                            CreatedDate = DateTime.Now
                        };
                        context.AccountMembers.Add(itfApplicant);
                    }
                    context.SaveChanges();
                    //next of kin
                    var nextOfKin1 = new AccountNextOfKinDetail
                    {
                        Name= nextOfKin1Name,
                        AccountId=account.Id,
                        CreatedDate=DateTime.Now,
                        Email= nextOfKin1Email,
                        Id=Guid.NewGuid(),
                        Fax= nextOfKin1Fax,
                        MailingAddress= nextOfKin1MailAddress,
                        Mobile= nextOfKin1Mobile,
                        RelationToAccount= nextOfKin1Relation,
                        Telephone= nextOfKin1Phone,
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
                        var authorisedPerson = new AccountAuthorisedPerson {
                            Name= authorisedPersonName,
                            Tel= authorisedPersonTel,
                            Mobile= authorisedPersonMobile,
                            AccountId=account.Id,
                            City= authorisedPersonCity,
                            CreatedDate=DateTime.Now,
                            Email= authorisedPersonEmail,
                            Fax= authorisedPersonFax,
                            Id=Guid.NewGuid(),
                            IDExpiryDate= firstApplicantIdAuthorisedCardExpDate,
                            IdIssueAuthority= firstApplicantIdAuthorisedPersonIssuingAuthority,
                            IdNumber= authorisedPersonApplicantIdNumber,
                            IdPath= _FirstJointItfAuthorisedPerson!=null?SaveFileUpload(_FirstJointItfAuthorisedPerson):null,
                            IssueDate= firstAuthorisedApplicantIdCardIssueDate,
                            MailingAddress= authorisedPersonMailAddress,
                            RelationToAccountHolder= authorisedPersonRelation,
                            ZipCode= authorisedPersonZipCode,
                            StreetAddress=null,
                            SignaturePath=null,
                        };

                        if (authorisedPersonZipCountryId>0)
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
                    context.AccountSettlementDetails.Add(new AccountSettlementDetail {
                        AccountId=account.Id,
                        AccountName= settlementDetailsAccountName,
                        AccountNumber= settlementDetailsAccountNumber,
                        Id=Guid.NewGuid(),
                        BankId= settlementDetailsAccountBankId,
                        Branch= settlementDetailsAccountBankBranch,
                        CreatedDate=DateTime.Now,
                        SwiftCode= settlementDetailsAccountBankSortCode,
                    });
                    context.SaveChanges();

                    var instructionEmploymentDetails = new AccountInstructionEmploymentDetail
                    {
                        AccountId=account.Id,
                        CreatedDate=DateTime.Now,
                        ModeOfInstructionId= instructionsEmploymentDetailsModeOfInstruction,
                        ModeOfNotificationId= instructionsEmploymentDetailsModeOfNotification,
                        SourceOfFundId= instructionsEmploymentDetailsSourceOfIncome,
                        EmploymentStatusId= instructionsEmploymentDetailsEmploymentStatus,
                        PrevOccupation= instructionsEmploymentDetailsPreviousOccupation,
                        PrevEmployer= instructionsEmploymentDetailsPreviousEmployer,
                        CurrentOccupation= instructionsEmploymentDetailsCurrentOccupation,
                        CurrentEmployer= instructionsEmploymentDetailsCurrentEmployer,
                        CurrentEmployerAddress= instructionsEmploymentDetailsCurrentEmployerAddress,
                        EmploymentDateFrom= instructionsEmploymentDetailsCurrentEmployerFrom,
                        EmploymentDateTo= instructionsEmploymentDetailsCurrentEmployerTo,
                        Id=Guid.NewGuid(),
                    };
                    if (yearsOfEmployment>0)
                    {
                        instructionEmploymentDetails.YearsOfEmployment = yearsOfEmployment;
                    }
                    context.AccountInstructionEmploymentDetails.Add(instructionEmploymentDetails);

                    context.AccountFinancialInvestmentRiskProfiles.
                 Add(new AccountFinancialInvestmentRiskProfile
                    {
                        AccountId=account.Id,
                        AnnualIncomeId= annualIncomeId,
                        NetworthId= networthId,
                        InvestmentHorizonId= investmentHorizonId,
                        ObjectivesId=objectivesId,
                        InvestmentKnowledgeId= investmentKnowledgeId,
                        RiskToleranceId=riskToleranceId,
                        OnlineTradingFacility= onlineTradingFacility,
                        CreatedDate=DateTime.Now,
                        Id=Guid.NewGuid(),
                        });
                    context.SaveChanges();



                }

                





                else
                {
                    //institutional
                    var institutionalAccount = new Account
                    {
                       
                        CSDFormPath = SaveFileUpload(_CsdCompletedForm),
                        InvestmentTypeId= instInvestmentType,
                        AccountNumber=null,
                        CSDNumber=null,
                        Id=Guid.NewGuid(),
                        InstitutionClientName= instNameOfClient,
                        ExpectedAccountActivityId= expectedAccountActivityId,
                        InstitutionalPrincipalBroker= instPricipalBroker,
                        InstitutionNatureOfBusiness=instNatureOfBusiness,
                        InsstitutionalCountryOfIncorporation= instCountryOfResidence,
                        InstitutionRegistrationNumber= instRegistrationNumber,
                        TIN=tin,
                        MailingAddressFull= instFullMailingAddress,
                        MailingAddressCity= instMailingAddressCity,
                        InstMailingAddressCountryId= instMailAddressCountryOfResidence,
                        StreetAddressFull= instFullStreetAddress,
                        InstStreetAddressCity= instSteetAddressCity,
                        InsStreetAddressZipCode= instSteetAddressZipCode,
                        InsStreetAddressTel= instTelephone,
                        InsStreetAddressFax= instFax,
                        InsStreetAddressEmail= instEmail,
                        InstCompanyType= instCompanyType,
                        RegionalInvestmentId= instRegionalInvestmentId,
                        FrequencyOfStatementsId= instStatementFreq,
                        AccountTypeId= accountType,
                        CreatedDate=DateTime.Now,
                        SignatureTypeId= instnumberOfSignatories,
                        InstOtherDetails= insOtherDetails,
                    };
                    if (instSteetAddressCountry>0)
                    {
                        institutionalAccount.InsStreetAddressCountryId = instSteetAddressCountry;
                    }
                    accountId = institutionalAccount.Id.ToString();
                    context.Accounts.Add(institutionalAccount);
                    context.SaveChanges();
                    context.AccountInsCustodyAccountDetails.Add(new AccountInsCustodyAccountDetail {

                        AccountId=institutionalAccount.Id,
                        CustodianName= custodyDetailsName,
                        Telephone= custodyDetailsPhone,
                        Address= custodyDetailsAddress,
                        Id=Guid.NewGuid(),
                        CashAccountNumber= custodyDetailsCashAccNumber,
                        Fax= custodyDetailsFax,
                        CreatedDate=DateTime.Now,
                        SecuritiesAccountNumber= custodyDetailsSecuritiesAccNumber,
                   });

                    var settlementDetails = new AccountSettlementDetail {
                        CorrespondentBankSwiftCode= intSettlementDetailsCorrespondentBankSwiftNo,
                        IntermediaryBankSwiftCode= intSettlementDetailsIntermediaryBankSwiftNo,
                        NameOfBeneficiary= intSettlementDetailsNameOfBeneficiary,
                        BIC= intSettlementDetailsBIC,
                        AccountNumber= intSettlementDetailsAccountNumber,
                        MarginTradingOption= instMarginTrationOption,
                        AccountId= institutionalAccount.Id,
                        OnlineTradingFacility= instOnlineTradingOption,
                        AccountName=null,
                        CreatedDate=DateTime.Now,
                        Id=Guid.NewGuid(),
                        
                        

                    };
                    if (intSettlementDetailsCorrespondentBankId>0)
                    {
                        settlementDetails.CorrespondentBankId = intSettlementDetailsCorrespondentBankId;
                    }
                    if (intSettlementDetailsIntermediaryBankId>0)
                    {
                        settlementDetails.IntermediaryBankId = intSettlementDetailsIntermediaryBankId;
                    }
                    context.AccountSettlementDetails.Add(settlementDetails);

                    context.AccountInstructionEmploymentDetails.Add(new AccountInstructionEmploymentDetail {
                        ModeOfInstructionId= intModeOfInstruction,
                        ModeOfNotificationId= intModeOfNotification,
                        SourceOfFundId= intSourceOfIncome,
                        AccountId= institutionalAccount.Id,
                        CreatedDate=DateTime.Now,
                        Id=Guid.NewGuid(),
                        

                    });

                    //authosied Officers
                    var authorisedOfficer1 = new AccountAuthorisedPerson
                    {
                        Id=Guid.NewGuid(),
                        AccountId= institutionalAccount.Id,
                        CreatedDate=DateTime.Now,
                        Name= intAuthorisedOfficer1Name,
                        Tel= intAuthorisedOfficer1Tel,
                        Mobile= intAuthorisedOfficer1Mobile,
                        Fax= intAuthorisedOfficer1Fax,
                        Email= intAuthorisedOfficer1Email,
                        RelationToAccountHolder= intAuthorisedOfficer1Relation,
                        StreetAddress= intAuthorisedOfficer1StreetAddress,
                        City= intAuthorisedOfficer1City,
                        ZipCode= intAuthorisedOfficer1ZipCode,
                        IdNumber= intAuthorisedOfficer1IDNumber,
                        IdIssueAuthority= intAuthorisedOfficer1IssueAuthority,
                        IssueDate= intAuthorisedOfficer1IdCardIssueDate,
                        IDExpiryDate= intAuthorisedOfficer1IdCardIssueExpDate,
                        IdPath= _InstAuthorisedOfficer1PhotoId!=null?SaveFileUpload(_InstAuthorisedOfficer1PhotoId):null,
                        MailingAddress=null,
                        SignaturePath=null,
                   };
                    if (intAuthorisedOfficer1Title>0)
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
                        IdPath = _InstAuthorisedOfficer2PhotoId != null ? SaveFileUpload(_InstAuthorisedOfficer2PhotoId) : null,
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

                    //trading contacts
                    context.AccountInsTradingContacts.Add(new AccountInsTradingContact
                    {

                        Id = Guid.NewGuid(),
                        AccountId = institutionalAccount.Id,
                        CreatedDate = DateTime.Now,
                        TitleId= tradingContacts1Title,
                        Name= tradingContacts1Name,
                        Tel= tradingContacts1Telephone,
                        Mobile= tradingContacts1Mobile,
                        Fax= tradingContacts1Fax,
                        Email= tradingContacts1Email,
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
                        if (tradingContacts2Title>0)
                        {
                            contactTrading2.TitleId = tradingContacts2Title;
                        }
                        context.AccountInsTradingContacts.Add(contactTrading2);
                    }

                    context.SaveChanges();

                    //signatures
                    context.AccountInstSignatoriesMandates.Add(new AccountInstSignatoriesMandate {

                        Id = Guid.NewGuid(),
                        AccountId = institutionalAccount.Id,
                        CreatedDate = DateTime.Now,
                        Name = instSignName1,
                        Position = instSignPosition1,
                        SignaturePath = _InstSignatory1!=null? SaveFileUpload(_InstSignatory1):null
                    });

                    context.AccountInstSignatoriesMandates.Add(new AccountInstSignatoriesMandate
                    {

                        Id = Guid.NewGuid(),
                        AccountId = institutionalAccount.Id,
                        CreatedDate = DateTime.Now,
                        Name = instSignName2,
                        Position = instSignPosition2,
                        SignaturePath = _InstSignatory2 != null ? SaveFileUpload(_InstSignatory2) : null
                    });

                    if (!string.IsNullOrEmpty(instSignName3))
                    {
                        context.AccountInstSignatoriesMandates.Add(new AccountInstSignatoriesMandate
                        {

                            Id = Guid.NewGuid(),
                            AccountId = institutionalAccount.Id,
                            CreatedDate = DateTime.Now,
                            Name = instSignName3,
                            Position = instSignPosition3,
                            SignaturePath = _InstSignatory3 != null ? SaveFileUpload(_InstSignatory3) : null
                        });
                    }
                    if (!string.IsNullOrEmpty(instSignName4))
                    {
                        context.AccountInstSignatoriesMandates.Add(new AccountInstSignatoriesMandate
                        {

                            Id = Guid.NewGuid(),
                            AccountId = institutionalAccount.Id,
                            CreatedDate = DateTime.Now,
                            Name = instSignName4,
                            Position = instSignPosition4,
                            SignaturePath = _InstSignatory4 != null ? SaveFileUpload(_InstSignatory4) : null
                        });
                    }
                    context.SaveChanges();
                }

                context.SaveChanges();
                //file uploads
                if (_PassportPhotos!=null && _PassportPhotos.Any())
                {
                    foreach (var item in _PassportPhotos)
                    {
                        context.AccountFileUploads.Add(new AccountFileUpload {
                            AccountId=Guid.Parse(accountId),
                            CreatedDate=DateTime.Now,
                            Id=Guid.NewGuid(),
                            IsActive=true,
                            Path=SaveFileUpload(item),
                            TypeId=1
                        });
                    }
                    context.SaveChanges();
                }

                if (_ProofOfResidenceFiles != null && _ProofOfResidenceFiles.Any())
                {
                    foreach (var item in _ProofOfResidenceFiles)
                    {
                        context.AccountFileUploads.Add(new AccountFileUpload
                        {
                            AccountId = Guid.Parse(accountId),
                            CreatedDate = DateTime.Now,
                            Id = Guid.NewGuid(),
                            IsActive = true,
                            Path = SaveFileUpload(item),
                            TypeId = 2
                        });
                    }
                    context.SaveChanges();
                }
                if (_OtherBusinessFiles != null && _OtherBusinessFiles.Any())
                {
                    foreach (var item in _OtherBusinessFiles)
                    {
                        context.AccountFileUploads.Add(new AccountFileUpload
                        {
                            AccountId = Guid.Parse(accountId),
                            CreatedDate = DateTime.Now,
                            Id = Guid.NewGuid(),
                            IsActive = true,
                            Path = SaveFileUpload(item),
                            TypeId = 3
                        });
                    }
                    context.SaveChanges();
                }
                context.SaveChanges();

                //aml questions
                var aml = context.AMLQuestions.Where(x => x.IsActive).OrderBy(x=>x.Id).ToList();
                for (int i = 0; i < aml.Count(); i++)
                {
                    int sum = i + 1;
                    string paramString = "amlQues_" + sum;
                    if (i<=9 && i!=9)
                    {
                        paramString = paramString.Replace("0", "");
                    }
                    string value= Request[paramString];
                    var amlItem = aml[i];
                    if (value== "Yes" || value=="No")
                    {
                        context.AccountAMLResponses.Add(new AccountAMLRespons
                        {
                            AccountId = Guid.Parse(accountId),
                            CreatedDate = DateTime.Now,
                            Id = Guid.NewGuid(),
                            QuestionId = amlItem.Id,
                            YesNo = value,
                            RatingValue = value == "Yes" ? amlItem.YesRating : amlItem.NoRating,
                            Remark = remark != null && remark.Any() ? remark[i]:string.Empty,
                            
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
                context.AccountETIs.Add(new AccountETI {
                    AccountId=Guid.Parse(accountId),
                    CreatedDate=DateTime.Now,
                    Text1= indemnityTxt1,
                    Id=Guid.NewGuid(),
                    Text2= indemnityTxt2,
                    Text3= indemnityTxt3,
                    Text4= indemnityTxt4,
                    Text5= null,
                    Name1= indemnityName1,
                    Email1= indemnityEmail1,
                    Name2= indemnityName2,
                    Email2= indemnityEmail2,
                    
                });

                context.SaveChanges();
                return RedirectToAction("AccountCreated");
            }
            catch (Exception ex)
            {

                Logger.Instance.logError(ex);
                ViewBag.Message = ex.Message;
                return View();
            }

        }


        public ActionResult AccountCreated()
        {
            return View();
        }

        [HttpPost]
        public void FirstApplicantIdPhoto(HttpPostedFileBase firstApplicantIdPhoto = null)
        {
            _FirstApplicantIdPhoto = firstApplicantIdPhoto;
          
        }

        [HttpPost]
        public void JointApplicantIdPhoto(HttpPostedFileBase jointApplicantIdPhoto = null)
        {
            _JointApplicantIdPhoto = jointApplicantIdPhoto;
        }

        [HttpPost]
        public void ItfApplicantIdPhoto(HttpPostedFileBase itfApplicantIdPhoto = null)
        {
            _ItfApplicantIdPhoto = itfApplicantIdPhoto;
        }

        [HttpPost]
        public void FirstJointItfAuthorisedPerson(HttpPostedFileBase firstJointItfAuthorisedPerson = null)
        {
            _FirstJointItfAuthorisedPerson = firstJointItfAuthorisedPerson;
        }

        [HttpPost]
        public void InstAuthorisedOfficer1PhotoId(HttpPostedFileBase instAuthorisedOfficer1PhotoId = null)
        {
            _InstAuthorisedOfficer1PhotoId = instAuthorisedOfficer1PhotoId;
        }


        [HttpPost]
        public void InstAuthorisedOfficer2PhotoId(HttpPostedFileBase instAuthorisedOfficer2PhotoId = null)
        {
            _InstAuthorisedOfficer2PhotoId = instAuthorisedOfficer2PhotoId;
        }

        [HttpPost]
        public void InstSignatory1(HttpPostedFileBase instSignatory1 = null)
        {
            _InstSignatory1 = instSignatory1;
        }

        [HttpPost]
        public void InstSignatory2(HttpPostedFileBase instSignatory2 = null)
        {
            _InstSignatory2 = instSignatory2;
        }

        [HttpPost]
        public void InstSignatory3(HttpPostedFileBase instSignatory3 = null)
        {
            _InstSignatory3 = instSignatory3;
        }

        [HttpPost]
        public void InstSignatory4(HttpPostedFileBase instSignatory4 = null)
        {
            _InstSignatory4 = instSignatory4;
        }

        [HttpPost]
        public void ProofOfResidence(List<HttpPostedFileBase> proofOfResidenceFiles = null)
        {
            if (_ProofOfResidenceFiles == null)
            {
                _ProofOfResidenceFiles = new List<HttpPostedFileBase>();
            }
            
            else
            {
                _ProofOfResidenceFiles.AddRange(proofOfResidenceFiles);
            }
        }

        [HttpPost]
        public void OtherBusinessFiles(List<HttpPostedFileBase> otherBusinessFiles = null)
        {
            if (_OtherBusinessFiles == null)
            {
                _OtherBusinessFiles = new List<HttpPostedFileBase>();
            }

            else
            {
                _OtherBusinessFiles.AddRange(otherBusinessFiles);
            }
        }



        [HttpPost]
        public void CsdCompletedForm(HttpPostedFileBase csdCompletedForm = null)
        {
            _CsdCompletedForm = csdCompletedForm;
        }

        [HttpPost]
        public void PassportPhotos(List<HttpPostedFileBase> passportPhotos = null)
        {
            if (_PassportPhotos == null)
            {
                _PassportPhotos = new List<HttpPostedFileBase>();
            }

            else
            {
                _PassportPhotos.AddRange(passportPhotos);
            }
        }

        [HttpPost]
        public void FirstApplicantSignature(HttpPostedFileBase firstApplicantSignature = null)
        {
            _FirstApplicantSignature = firstApplicantSignature;
        }


        [HttpPost]
        public void JointApplicantSignature(HttpPostedFileBase jointApplicantSignature = null)
        {
            _JointApplicantSignature = jointApplicantSignature;
        }




        //FILE REMOVAL ACTION
        public void DropFile(string type, string fileName)
        {
            switch (type)
            {
                case "firstApplicantIdPhoto":
                    {
                        _FirstApplicantIdPhoto = null;
                        break;
                    }
                case "jointApplicantIdPhoto":
                    {
                        _JointApplicantIdPhoto = null;
                        break;
                    }
                case "itfApplicantIdPhoto":
                    {
                        _ItfApplicantIdPhoto = null;
                        break;
                    }
                case "firstJointItfAuthorisedPerson":
                    {
                        _FirstJointItfAuthorisedPerson = null;
                        break;
                    }
                case "instAuthorisedOfficer1PhotoId":
                    {
                        _InstAuthorisedOfficer1PhotoId = null;
                        break;
                    }
                case "instAuthorisedOfficer2PhotoId":
                    {
                        _InstAuthorisedOfficer2PhotoId = null;
                        break;
                    }
                case "instSignatory1":
                    {
                        _InstSignatory1 = null;
                        break;
                    }
                case "instSignatory2":
                    {
                        _InstSignatory2 = null;
                        break;
                    }
                case "instSignatory3":
                    {
                        _InstSignatory3 = null;
                        break;
                    }
                case "instSignatory4":
                    {
                        _InstSignatory4 = null;
                        break;
                    }

             

                case "proofOfResidenceFiles":
                    {
                        if (_ProofOfResidenceFiles!=null && _ProofOfResidenceFiles.Any(x=>x.FileName==fileName))
                        {
                            var find = _ProofOfResidenceFiles.FirstOrDefault(x => x.FileName == fileName);
                            _ProofOfResidenceFiles.Remove(find);
                        }
                        break;
                    }
                case "otherBusinessFiles":
                    {

                        if (_OtherBusinessFiles != null && _OtherBusinessFiles.Any(x => x.FileName == fileName))
                        {
                            var find = _OtherBusinessFiles.FirstOrDefault(x => x.FileName == fileName);
                            _OtherBusinessFiles.Remove(find);
                        }
                        break;
                    }
                case "csdCompletedForm":
                    {
                        _CsdCompletedForm = null;
                        break;
                    }

                case "passportPhotos":
                    {
                        if (_PassportPhotos != null && _PassportPhotos.Any(x => x.FileName == fileName))
                        {
                            var find = _PassportPhotos.FirstOrDefault(x => x.FileName == fileName);
                            _PassportPhotos.Remove(find);
                        }
                        break;
                    }

                case "firstApplicantSignature":
                    {
                        _FirstApplicantSignature = null;
                        break;
                    }

                case "jointApplicantSignature": {
                        _JointApplicantSignature = null;
                        break;
                    }
                default:
                    break;
            }
        }

    }
}