using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppModels
{
    public class ChangePasswordModel
    {
        [Compare("NewPassword", ErrorMessage = "Password and confirmation mismatch")]
        [DataType(DataType.Password)]
        [DisplayName("Confirm Password")]
        [Required(ErrorMessage = "Required")]
        public string ConfirmPassword
        {
            get;
            set;
        }

        [DataType(DataType.Password)]
        [DisplayName("New Password")]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d).{6,12}$", ErrorMessage = "Password must meet requirements")]
        [Required(ErrorMessage = "Required")]
        public string NewPassword
        {
            get;
            set;
        }
        [DataType(DataType.Password)]
        [DisplayName("Current Password")]
        [Required(ErrorMessage = "Required")]
        public string CurrentPassword { get; set; }



        public string Fullname { get; set; }
        public string UserId { get; set; }
    }

    public class AccountBasicModel {
        public System.Guid Id { get; set; }
        public int AccountTypeId { get; set; }
        public string AccountTypeName { get; set; }
        public string AccountNumber { get; set; }
        public string  AccountName { get; set; }
        public string CSDNumber { get; set; }
        public string InstitutionClientName { get; set; }
        public string InstitutionalPrincipalBroker { get; set; }
        public string InstitutionNatureOfBusiness { get; set; }
        public Nullable<int> InsstitutionalCountryOfIncorporation { get; set; }
        public string InsstitutionalCountryOfIncorporationName { get; set; }
        public string InstitutionRegistrationNumber { get; set; }
        public string MailingAddressFull { get; set; }
        public string MailingAddressCity { get; set; }
        public Nullable<int> InstMailingAddressCountryId { get; set; }
        public string InstMailingAddressCountryIdName { get; set; }
        public string StreetAddressFull { get; set; }
        public string InstStreetAddressCity { get; set; }
        public string InsStreetAddressZipCode { get; set; }
        public Nullable<int> InsStreetAddressCountryId { get; set; }
        public string InsStreetAddressCountryIdName { get; set; }
        public string InsStreetAddressTel { get; set; }
        public string InsStreetAddressFax { get; set; }
        public string InsStreetAddressEmail { get; set; }
        public Nullable<int> SelectApplicableId { get; set; }
        public string SelectApplicableName { get; set; }
        public Nullable<int> RegionalInvestmentId { get; set; }
        public string RegionalInvestmentName { get; set; }
        public Nullable<int> FrequencyOfStatementsId { get; set; }
        public string FrequencyOfStatementsName { get; set; }
        public string DeclarationConvictedOfLaw { get; set; }
        public string DeclarationConvictedOfLawDetails { get; set; }
        public string DeclarationActingAsNominee { get; set; }
        public string DeclarationActingAsNomineeName { get; set; }
        public string DeclarationIWe { get; set; }
        public Nullable<int> SignatureTypeId { get; set; }
        public string SignatureTypeName { get; set; }
        public string InstOtherDetails { get; set; }
        public string CSDFormPath { get; set; }
        public string InstCompanyType { get; set; }
        public Nullable<int> YearsOfWorkExperience { get; set; }
        public string TIN { get; set; }
        public Nullable<int> ExpectedAccountActivityId { get; set; }
        public string ExpectedAccountActivityName { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> InvestmentTypeId { get; set; }
        public string InvestmentTypeName { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public Nullable<System.DateTime> SuccessfulReviewDate { get; set; }
        public Nullable<System.Guid> SuccesfulReviewBy { get; set; }
        public string SuccessfullyReviewwedByName { get; set; }
        public Nullable<System.DateTime> CancelOrRejectDate { get; set; }
        public Nullable<System.Guid> CancelOrRejectBy { get; set; }
        public string CancelOrrejectByName { get; set; }
        public string CancelOrRejectComment { get; set; }
        public string RefNo { get; set; }
        public double? StaffRefCode { get; set; }
        public string StaffRefName { get; set; }
        public string BranchCode { get; set; }
        public string BackConnectAccountNumber { get; set; }
        public string Password { get; set; }
        public string CreatedBy { get; set; }
    }

    public class AccountBeneficiaryModel
    {
        public Guid AccountId { get; set; }

        public string Fullname { get; set; }

        public string Phone { get; set; }

        public Decimal PercentageAllocation { get; set; }

        public DateTime CreatedDate { get; set; }

        public string Relation { get; set; }

        public int TitleId { get; set; }

        public string OtherTitleDetails { get; set; }

        public string Surname { get; set; }

        public string Othernames { get; set; }

        public int? MaritalStatusId { get; set; }

        public int GenderId { get; set; }

        public string BirthDate { get; set; }

        public string PlaceOfBirth { get; set; }

        public int CountryOfOriginId { get; set; }

        public int CountryOfResidenceId { get; set; }

        public int? IDCardTypeId { get; set; }

        public string IDNumber { get; set; }

        public string IDPlaceOfIssue { get; set; }

        public string IDIssueDate { get; set; }

        public string IDExpiryDate { get; set; }
    }
    public class AccountAMLReponseModel {

        public System.Guid Id { get; set; }
        public System.Guid AccountId { get; set; }
        public int QuestionId { get; set; }
        public string YesNo { get; set; }
        public string Rank { get; set; }
        public string Remark { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public decimal RatingValue { get; set; }

        public string QuestionText { get; set; }
        public string RatingValueTxt { get; set; }
    }

    public class AccountAuthorisedPersonModel {
        public System.Guid Id { get; set; }
        public System.Guid AccountId { get; set; }
        public string Name { get; set; }
        public Nullable<int> TitleId { get; set; }
        public string Title { get; set; }
        public string Tel { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }
        public string Mobile { get; set; }
        public string RelationToAccountHolder { get; set; }
        public string MailingAddress { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public Nullable<int> CountryId { get; set; }
        public string CountryName { get; set; }
        public Nullable<int> IdType { get; set; }
        public string IdTypeName { get; set; }
        public string IdNumber { get; set; }
        public string IssueDate { get; set; }
        public string IDExpiryDate { get; set; }
        public string IdIssueAuthority { get; set; }
        public string IdPath { get; set; }
        public string SignaturePath { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public bool IdValidated { get; set; }
        public Nullable<System.DateTime> IdValidationDate { get; set; }
        public string IdValidationBy { get; set; }
        public string IdValidationMode { get; set; }
        public string IdCardValidationStatus { get; set; }
    }

    public class AccountETIModel {
        public System.Guid Id { get; set; }
        public System.Guid AccountId { get; set; }
        public string Text1 { get; set; }
        public string Text2 { get; set; }
        public string Text3 { get; set; }
        public string Text4 { get; set; }
        public string Text5 { get; set; }
        public string Name1 { get; set; }
        public string Email1 { get; set; }
        public string Name2 { get; set; }
        public string Email2 { get; set; }
        public System.DateTime CreatedDate { get; set; }

    }
    public class AccountFileUploadModel {

        public System.Guid Id { get; set; }
        public System.Guid AccountId { get; set; }
        public int TypeId { get; set; }
        public string Path { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreatedDate { get; set; }

        public string FileUploadTypeName { get; set; }
    }

    public class AccountFinancialInvestmentRiskProfileModel
    {

        public System.Guid Id { get; set; }
        public System.Guid AccountId { get; set; }
        public Nullable<int> AnnualIncomeId { get; set; }
        public Nullable<int> NetworthId { get; set; }
        public Nullable<int> InvestmentHorizonId { get; set; }
        public Nullable<int> ObjectivesId { get; set; }
        public Nullable<int> InvestmentKnowledgeId { get; set; }
        public Nullable<int> RiskToleranceId { get; set; }
        public string OnlineTradingFacility { get; set; }
        public System.DateTime CreatedDate { get; set; }

        public string ApproximateAnnualIncomeName { get; set; }
        public string InvestmentHorizonName { get; set; }
        public string InvestmentKnowledgeName { get; set; }
        public string NetWorthName { get; set; }
        public string ObjectiveName { get; set; }
        public string RiskToleranceName { get; set; }

    }

    public class AccountCustomerDetailsModel {

        public System.Guid Id { get; set; }
        public System.Guid AccountId { get; set; }
        public string CustodianName { get; set; }
        public string Telephone { get; set; }
        public string Address { get; set; }
        public string Fax { get; set; }
        public string CashAccountNumber { get; set; }
        public string SecuritiesAccountNumber { get; set; }
        public System.DateTime CreatedDate { get; set; }


    }
    public class AccountTradingContactModel {
        public System.Guid Id { get; set; }
        public System.Guid AccountId { get; set; }
        public Nullable<int> TitleId { get; set; }
        public string Name { get; set; }
        public string Tel { get; set; }
        public string Mobile { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string TitleName { get; set; }
    }

    public class AccountSignatoryModel {
        public System.Guid Id { get; set; }
        public System.Guid AccountId { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string SignaturePath { get; set; }
        public System.DateTime CreatedDate { get; set; }
    }
    public class AccountSettlementDetailModel {

        public Guid Id { get; set; }

        public Guid AccountId { get; set; }

        public string AccountName { get; set; }

        public string AccountNumber { get; set; }

        public int? BankId { get; set; }

        public string BankName { get; set; }

        public int? CorrespondentBankId { get; set; }

        public string CorrespondentBankName { get; set; }

        public int? IntermediaryBankId { get; set; }

        public string IntermediaryBankName { get; set; }

        public string Branch { get; set; }

        public string SwiftCode { get; set; }

        public string CorrespondentBankSwiftCode { get; set; }

        public string IntermediaryBankSwiftCode { get; set; }

        public string CorrespondentBankBranch { get; set; }

        public string IntermediaryBankBranch { get; set; }

        public string NameOfBeneficiary { get; set; }

        public string BIC { get; set; }

        public string MarginTradingOption { get; set; }

        public string OnlineTradingFacility { get; set; }

        public DateTime CreatedDate { get; set; }

    }

    public class AccountNextOfKinDetailsModel {

        public System.Guid Id { get; set; }
        public System.Guid AccountId { get; set; }
        public string Name { get; set; }
        public string Telephone { get; set; }
        public string Mobile { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string RelationToAccount { get; set; }
        public string MailingAddress { get; set; }
        public System.DateTime CreatedDate { get; set; }
    }

    public class AccountMemberModel {
        public System.Guid Id { get; set; }
        public System.Guid AccountId { get; set; }
        public Nullable<int> TitleId { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string Othername { get; set; }
        public Nullable<int> NationalityId { get; set; }
        public string NationalityName { get; set; }
        public string DOB { get; set; }
        public string PlaceOfBirth { get; set; }
        public Nullable<int> MaritalStatusId { get; set; }
        public string MothersMaidenName { get; set; }
        public string ResidentialAddressFull { get; set; }
        public string ResidentialCity { get; set; }
        public string ResidentialZipCode { get; set; }
        public Nullable<int> ResidentialCountryId { get; set; }
        public string ResidentialCountryName { get; set; }
        public string MailingAddressFull { get; set; }
        public string MailingAddressCity { get; set; }
        public string MailingAddressZipCode { get; set; }
        public Nullable<int> MailingAddressCountryId { get; set; }
        public string MailingAddressCountryName { get; set; }
        public Nullable<int> SelectWhereApplicableId { get; set; }
        public string SelectWhereApplicableName { get; set; }
        public string Telephone { get; set; }
        public string Mobile { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Occupation { get; set; }
        public Nullable<int> IdTypeId { get; set; }
        public string IdTypeName { get; set; }
        public string IdNumber { get; set; }
        public string IssuingAuthority { get; set; }
        public string IdCardIssueDate { get; set; }
        public string IdCardExpiryDate { get; set; }
        public string IdPath { get; set; }
        public string SignaturePath { get; set; }
        public string MaidenName { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string Gender { get; set; }

      
        public string  MaritalStatusName { get; set; }
        public string TitleName { get; set; }

        public bool IdValidated { get; set; }
        public Nullable<System.DateTime> IdValidationDate { get; set; }
        public string IdValidationBy { get; set; }
        public string IdValidationMode { get; set; }
        public string IdCardValidationStatus { get; set; }

        public string ResidentPermitNo { get; set; }
        public string ResidentPermitIssueDate { get; set; }
        public string ResidentPermitExpiryDate { get; set; }
        public string ResidentPermitPlaceOfIssue { get; set; }
        public string Profession { get; set; }
        public string ProfessionLinNumber { get; set; }
    }

    public class AccountInstructionEmploymentDetailModel {

        public System.Guid Id { get; set; }
        public System.Guid AccountId { get; set; }
        public Nullable<int> ModeOfInstructionId { get; set; }
        public Nullable<int> ModeOfNotificationId { get; set; }
        public Nullable<int> SourceOfFundId { get; set; }
        public Nullable<int> EmploymentStatusId { get; set; }
        public string PrevOccupation { get; set; }
        public string PrevEmployer { get; set; }
        public string CurrentOccupation { get; set; }
        public string CurrentEmployer { get; set; }
        public string CurrentEmployerAddress { get; set; }
        public string EmploymentDateFrom { get; set; }
        public string EmploymentDateTo { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> YearsOfEmployment { get; set; }

        public string EmploymentStatusName { get; set; }
        public string ModeOfInstructionName { get; set; }
        public string ModeOfNotificationName { get; set; }
        public string SourceOfIncomeName { get; set; }
        public string SourceOfIncomeNamesList { get; set; }
        public string SourceOfFundsIds { get; set; }

    }


    public class GviveResponseModel
    {
        public int CardTypeId { get; set; }
        public string ResponseCode { get; set; }
        public string PassportNo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public string MiddleName { get; set; }
        public string Nationality { get; set; }
        public string DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string PlaceOfBirth { get; set; }
        public string PlaceOfIssue { get; set; }
        public string IssueDate { get; set; }
        public string ExpiryDate { get; set; }
        public string Picture { get; set; }
        public string Signature { get; set; }

        public bool Error { get; set; }
        public string Message { get; set; }


        //voter
        public string PollingStation { get; set; }
        public string VoterID { get; set; }
        public string Age { get; set; }
        public string Sex { get; set; }
        public string RegDate { get; set; }
        public string Fullname { get; set; }
        public string FathersName { get; set; }
        public string MothersName { get; set; }
        public string ResidentialAddress { get; set; }

        //Driver
        public string SecondName { get; set; }
        public string ProcessingCenter { get; set; }
        public string NationIDNo { get; set; }
        public string ClassOfLicence { get; set; }
        public string DateOfIssue { get; set; }
        public string DateOfFirstLicence { get; set; }
        public string CertificateDate { get; set; }
        public string CertificateOfCompetence { get; set; }
        public string DriverImage { get; set; }
        public string DriverSignature { get; set; }
        public string PIN { get; set; }



        //SSNIT
        public string FSSNo { get; set; }
        public string FullName { get; set; }
        public string BirthDate { get; set; }
        public string CardSerial { get; set; }
        public string Photo { get; set; }

        public int FirstOrSecond { get; set; }

        public bool IsVerified { get; set; }
        public string IDNumber { get; set; }

        public string ClientNameCompare { get; set; }
        public double ComparePcnt { get; set; }
    }

    public class UserAppRole
    {
        public string AppCode { get; set; }
        public string RoleName { get; set; }
        public string AppName { get; set; }
    }
    public class AppRoleModel
    {
        public string RoleName { get; set; }
        public int RoleId { get; set; }

    }

    public class NameValueModel {
        public int IntVal { get; set; }
        public decimal DeciVal { get; set; }
        public string Label { get; set; }
        public string Color { get; set; }

    }
    public class UserModel
    {
        public string UserId { get; set; }

        [Display(Name = "Official Email(*)"), DataType(DataType.EmailAddress), Required]
        public string Username { get; set; }
        [Display(Name = "Phone No.")]
        [StringLength(20, MinimumLength = 10),
         DataType(DataType.PhoneNumber, ErrorMessage = "Please enter a valid phone number"),
         RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4,14})$", ErrorMessage = "Please enter a valid phone number. 10-20 digits only")]

        public string Phone { get; set; }
        [Required]
        public string Fullname { get; set; }
        public int? LocationId { get; set; }
        public string AppName { get; set; }
        public int RoleId { get; set; }
        public string AppCode { get; set; }

        public string RoleName { get; set; }
        public bool IsTeamLeader { get; set; }
        [Display(Name = "Department(*)")]
        public string GroupIds { get; set; }

        public bool IsAdmin { get; set; }
        public bool IsSuperUser { get; set; }

        public bool DefaultPasswordChanged { get; set; }
        public string DeptName { get; set; }
        public bool IsActive { get; set; }
    }

    public class ActivityModel
    {
        public string UserId { get; set; }
        public string Description { get; set; }
        public bool IsStart { get; set; }
        public string SessionId { get; set; }
        public string AppCode { get; set; }
    }
}
