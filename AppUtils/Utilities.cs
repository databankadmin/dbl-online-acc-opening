using AppLogger;
using AppModels;
using DBHelper.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AppUtils
{
    public static class Utilities
    {
        public static AppSetting GetAppSettings()
        {
            using (var context = new DBLAccountOpeningContext())
            {
                return context.AppSettings.FirstOrDefault();
            }
        }

        public static string EncodeBase64(string rawString)
        {
            try
            {
                if (string.IsNullOrEmpty(rawString))
                {
                    return string.Empty;
                }
                var plainTextBytes = Encoding.UTF8.GetBytes(rawString);
                string encodedText = Convert.ToBase64String(plainTextBytes);
                return encodedText;
            }
            catch (Exception ex)
            {

                Logger.Instance.logError(ex);
                return null;
            }

        }
        public static string DecodeBase64(string encrypted)
        {
            try
            {
                var encodedTextBytes = Convert.FromBase64String(encrypted);
                string plainText = Encoding.UTF8.GetString(encodedTextBytes);
                return plainText;

            }
            catch (Exception ex)
            {

                Logger.Instance.logError(ex);
                return null;
            }
        }
        public static AppUser ValidateAdmin(string username, string encryptedPassword)
        {
            using (var context = new DBLAccountOpeningContext())
            {
                return context.AppUsers.FirstOrDefault(x => x.Email == username && x.Password == encryptedPassword  && x.IsActive);
            }
        }
        public static object GetSessionUser()
        {
            var context = new DBLAccountOpeningContext();
            string username = HttpContext.Current.User.Identity.Name;
            var admin = context.AppUsers.FirstOrDefault(x => x.Email == username && x.IsActive);
            if (admin != null)
            {
                return admin;
            }
            return null;
        }



        public static void MarkIdCardAsValidated(string recordId,string objectType)
        {
            using (var context=new DBLAccountOpeningContext())
            {
                if (objectType=="AccMember")
                {
                    var model = context.AccountMembers.FirstOrDefault(x=>x.Id.ToString()==recordId);
                    if (model!=null && !model.IdValidated)
                    {
                        model.IdValidated = true;
                        model.IdValidationDate = DateTime.Now;
                        model.IdValidationBy = (GetSessionUser() as AppUser).Email;
                        model.IdValidationMode = "GVIVE";
                   
                    }
                }
                else if (objectType=="AuthPerson")
                {
                    var model = context.AccountAuthorisedPersons.FirstOrDefault(x => x.Id.ToString() == recordId);
                    if (model != null && !model.IdValidated)
                    {
                        model.IdValidated = true;
                        model.IdValidationDate = DateTime.Now;
                        model.IdValidationBy = (GetSessionUser() as AppUser).Email;
                        model.IdValidationMode = "GVIVE";
                    }
                }
                context.SaveChanges();
            }
        }

        public static List<AccountBasicModel> GetApplications(int accountType=0, int  investmentTypeId=0,string dates=null, string applicantionId=null)
        {
            try
            {
                var model = new List<AccountBasicModel>();

                using (var context = new DBLAccountOpeningContext())
                {

                    var applications = context.Accounts.OrderByDescending(x => x.CreatedDate).AsEnumerable();
                    if (accountType > 0)
                    {
                        applications = applications.Where(x => x.AccountTypeId == accountType);
                    }
                    if (investmentTypeId > 0)
                    {
                        applications = applications.Where(x => x.InvestmentTypeId == investmentTypeId);
                    }
                    if (!string.IsNullOrEmpty(applicantionId))
                    {
                        applications = applications.Where(x => x.Id.ToString() == applicantionId);
                    }
                    foreach (var item in applications)
                    {
                        model.Add(new AccountBasicModel
                        {
                            AccountName = GetAccountName(item.Id),
                            AccountNumber = item.AccountNumber,
                            AccountTypeId = item.AccountTypeId,
                            AccountTypeName = item.AccountType.Name,
                            CreatedDate = item.CreatedDate,
                            CSDFormPath = item.CSDFormPath,
                            CSDNumber = item.CSDNumber,
                            DeclarationActingAsNominee = item.DeclarationActingAsNominee,
                            DeclarationActingAsNomineeName = item.DeclarationActingAsNomineeName,
                            DeclarationConvictedOfLaw = item.DeclarationConvictedOfLaw,
                            DeclarationConvictedOfLawDetails = item.DeclarationConvictedOfLawDetails,
                            DeclarationIWe = item.DeclarationIWe,
                            ExpectedAccountActivityId = item.ExpectedAccountActivityId,
                            ExpectedAccountActivityName=item.ExpectedAccountActivityId.HasValue? GetExpectedAccountActivity(item.ExpectedAccountActivityId.Value).Name:string.Empty,
                            FrequencyOfStatementsId = item.FrequencyOfStatementsId,
                            FrequencyOfStatementsName = item.FrequencyOfStatementsId.HasValue ? context.StatementFrequencies.Find(item.FrequencyOfStatementsId).Name : string.Empty,
                            Id = item.Id,
                            InsstitutionalCountryOfIncorporation = item.InsstitutionalCountryOfIncorporation,
                            InsstitutionalCountryOfIncorporationName = item.InsstitutionalCountryOfIncorporation.HasValue ? context.Countries.Find(item.InsstitutionalCountryOfIncorporation).CountryName : string.Empty,
                            InsStreetAddressCountryId = item.InsStreetAddressCountryId,
                            InsStreetAddressCountryIdName = item.InsStreetAddressCountryId.HasValue ? GetCountry(item.InsStreetAddressCountryId.Value).CountryName : string.Empty,
                            InsStreetAddressEmail = item.InsStreetAddressEmail,
                            InsStreetAddressFax = item.InsStreetAddressFax,
                            InsStreetAddressTel = item.InsStreetAddressTel,
                            InsStreetAddressZipCode = item.InsStreetAddressZipCode,
                            InstCompanyType = item.InstCompanyType,
                            InstitutionalPrincipalBroker = item.InstitutionalPrincipalBroker,
                            InstitutionClientName = item.InstitutionClientName,
                            InstitutionNatureOfBusiness = item.InstitutionNatureOfBusiness,
                            InstitutionRegistrationNumber = item.InstitutionRegistrationNumber,
                            InstMailingAddressCountryId = item.InstMailingAddressCountryId,
                            InstMailingAddressCountryIdName = item.InstMailingAddressCountryId.HasValue ? GetCountry(item.InstMailingAddressCountryId.Value).CountryName : string.Empty,
                            InstOtherDetails = item.InstOtherDetails,
                            InstStreetAddressCity = item.InstStreetAddressCity,
                            InvestmentTypeId = item.InvestmentTypeId,
                            InvestmentTypeName = item.InvestmentType.Name,
                            MailingAddressCity = item.MailingAddressCity,
                            MailingAddressFull = item.MailingAddressFull,
                            RegionalInvestmentId = item.RegionalInvestmentId,
                            RegionalInvestmentName = item.RegionalInvestmentId.HasValue ? GetRegionalInterest(item.RegionalInvestmentId.Value).Name : string.Empty,
                            SelectApplicableId = item.SelectApplicableId,
                            SelectApplicableName = item.SelectApplicableId.HasValue ? GetCSDParticipationOption(item.SelectApplicableId.Value).Name : string.Empty,
                            SignatureTypeId = item.SignatureTypeId,
                            SignatureTypeName = item.SignatureTypeId.HasValue ? GetSignatureType(item.SignatureTypeId.Value).Name : string.Empty,
                            StreetAddressFull = item.StreetAddressFull,
                            TIN = item.TIN,
                            YearsOfWorkExperience = item.YearsOfWorkExperience,
                            StatusId=item.StatusId,
                            StatusName=item.ApplicationStatu.Name,
                            SuccesfulReviewBy=item.SuccesfulReviewBy,
                            SuccessfullyReviewwedByName=item.SuccesfulReviewBy.HasValue?context.AppUsers.Find(item.SuccesfulReviewBy.Value).Name:string.Empty,
                            SuccessfulReviewDate=item.SuccessfulReviewDate,
                            CancelOrRejectBy=item.CancelOrRejectBy,
                            CancelOrrejectByName=item.CancelOrRejectBy.HasValue?context.AppUsers.Find(item.CancelOrRejectBy.Value).Name:string.Empty,
                            CancelOrRejectComment=item.CancelOrRejectComment,
                            CancelOrRejectDate=item.CancelOrRejectDate



                        });
                    }
                }
                return model;
            }
            catch (Exception ex)
            {

                Logger.Instance.logError(ex);
                return null;
            }
        }

        public static bool IsFilePdf(string fileName)
        {
            fileName = fileName.Trim().ToLower();
            if (fileName.EndsWith(".pdf"))
            {
                return true;
            }
            return false;
        }


        public static Country GetCountry(int countryId)
        {
            using (var context=new DBLAccountOpeningContext())
            {
                return context.Countries.Find(countryId);
            }
        }

        public static ExpectedAccountActivity GetExpectedAccountActivity(int id)
        {
            using (var context = new DBLAccountOpeningContext())
            {
                return context.ExpectedAccountActivities.Find(id);
            }
        }

        public static Title GetTitle(int id)
        {
            using (var context = new DBLAccountOpeningContext())
            {
                return context.Titles.Find(id);
            }
        }
        public static RegionalInvesmentInterest GetRegionalInterest(int id)
        {
            using (var context = new DBLAccountOpeningContext())
            {
                return context.RegionalInvesmentInterests.Find(id);
            }
        }
        public static CSDDepositoryParticipantOption GetCSDParticipationOption(int id)
        {
            using (var context = new DBLAccountOpeningContext())
            {
                return context.CSDDepositoryParticipantOptions.Find(id);
            }
        }
        public static SignatureType GetSignatureType(int id)
        {
            using (var context = new DBLAccountOpeningContext())
            {
                return context.SignatureTypes.Find(id);
            }
        }
        public static IDCardType GetIdType(int id)
        {
            using (var context = new DBLAccountOpeningContext())
            {
                return context.IDCardTypes.Find(id);
            }
        }
        public static Bank GetBank(int id)
        {
            using (var context = new DBLAccountOpeningContext())
            {
                return context.Banks.Find(id);
            }
        }

        public static string GetAccountName(Guid accountId)
        {
            try
            {
                using (var context = new DBLAccountOpeningContext())
                {
                    string accountName = string.Empty;
                    var account = context.Accounts.Find(accountId);
                    if (account.AccountTypeId <= 3)
                    {
                        var members = account.AccountMembers;
                        if (account.AccountTypeId == 1)
                        {
                            var first = members.FirstOrDefault();
                            accountName = first.Fname + " " + first.Othername + " " + first.Lname;
                        }
                        if (account.AccountTypeId == 2)
                        {
                            //joint
                            var first = members.FirstOrDefault();
                            var last = members.LastOrDefault();
                            accountName = first.Fname + " " + first.Othername + " " + first.Lname + " JOINT " +
                                last.Fname + " " + last.Othername + " " + last.Lname;
                        }
                        if (account.AccountTypeId == 3)
                        {
                            //itf
                            var first = members.FirstOrDefault();
                            var last = members.LastOrDefault();
                            accountName = first.Fname + " " + first.Othername + " " + first.Lname + " ITF " +
                                last.Fname + " " + last.Othername + " " + last.Lname;
                        }

                    }
                    else
                    {
                        accountName = account.InstitutionClientName;
                    }
                    return accountName;
                }
            }
            catch (Exception ex)
            {

                Logger.Instance.logError(ex);
                return null;
            }
        }


        public static List<AccountAMLReponseModel> GetAccountAMlReponses(Guid accountId)
        {

            var model=new List<AccountAMLReponseModel>();
            using (var context=new DBLAccountOpeningContext())
            {
                foreach (var item in context.AccountAMLResponses.Where(x=>x.AccountId==accountId).OrderBy(x=>x.QuestionId))
                {
                    model.Add(new AccountAMLReponseModel {
                        AccountId=item.AccountId,
                        CreatedDate=item.CreatedDate,
                        Id=item.Id,
                        QuestionId=item.QuestionId,
                        QuestionText=item.AMLQuestion.Question,
                        Rank=item.Rank,
                        RatingValue=item.RatingValue,
                        Remark=item.Remark,
                        YesNo=item.YesNo,
                        RatingValueTxt=item.RatingValue>=0?item.RatingValue.ToString():"N/A"
                        
                    });
                }
            }
            return model;
        }

        public static bool HasUnverifiedIds(Guid accountId)
        {
            bool hasUnverifiedId = false;
            var members = GetAccountMembers(accountId);
            var authorisedPersons = GetAccountAuthorisedPersons(accountId);
            if (members.Any(x=>!x.IdValidated) || authorisedPersons.Any(x=>!x.IdValidated))
            {
                hasUnverifiedId = true;
            }
            return hasUnverifiedId;
        }


        public static List<AccountAuthorisedPersonModel> GetAccountAuthorisedPersons(Guid accountId)
        {
            var model = new List<AccountAuthorisedPersonModel>();
            try
            {
                using (var context = new DBLAccountOpeningContext())
                {
                    foreach (var item in context.AccountAuthorisedPersons.Where(x => x.AccountId == accountId))
                    {
                        model.Add(new AccountAuthorisedPersonModel
                        {
                            AccountId = item.AccountId,
                            City = item.City,
                            CountryId = item.CountryId,
                            CountryName = item.CountryId.HasValue ? GetCountry(item.CountryId.Value).CountryName : string.Empty,
                            CreatedDate = item.CreatedDate,
                            Email = item.Email,
                            Fax = item.Fax,
                            Id = item.Id,
                            IDExpiryDate = item.IDExpiryDate,
                            IdIssueAuthority = item.IdIssueAuthority,
                            IdNumber = item.IdNumber,
                            IdPath = item.IdPath,
                            IdType = item.IdType,
                            IdTypeName = item.IdType.HasValue ? GetIdType(item.IdType.Value).Name : string.Empty,
                            IssueDate = item.IssueDate,
                            MailingAddress = item.MailingAddress,
                            Mobile = item.Mobile,
                            Name = item.Name,
                            RelationToAccountHolder = item.RelationToAccountHolder,
                            SignaturePath = item.SignaturePath,
                            StreetAddress = item.StreetAddress,
                            Tel = item.Tel,
                            Title = item.TitleId.HasValue ? GetTitle(item.TitleId.Value).Name : string.Empty,
                            TitleId = item.TitleId,
                            ZipCode = item.ZipCode,
                            IdValidated=item.IdValidated,
                            IdValidationBy=item.IdValidationBy,
                            IdValidationDate=item.IdValidationDate,
                            IdValidationMode=item.IdValidationMode,
                            IdCardValidationStatus = item.IdValidated ? "Verified-" + item.IdValidationMode : "Not Verified"
                        });
                    }
                    return model;
                }
            }
            catch (Exception ex)
            {

                Logger.Instance.logError(ex);
                return null;
            }

        }
        public static ApproximateAnnualIncome GetApproximateIncome(int id)
        {
            using (var context = new DBLAccountOpeningContext())
            {
                return context.ApproximateAnnualIncomes.Find(id);
            }
        }

        public static InvestmentHorizon GetInvestmentHorizon(int id)
        {
            using (var context = new DBLAccountOpeningContext())
            {
                return context.InvestmentHorizons.Find(id);
            }
        }

        public static InvestmentKnowledge GetInvestmentKnowledge(int id)
        {
            using (var context = new DBLAccountOpeningContext())
            {
                return context.InvestmentKnowledges.Find(id);
            }
        }
        public static NetWorth GetNetWorth(int id)
        {
            using (var context = new DBLAccountOpeningContext())
            {
                return context.NetWorths.Find(id);
            }
        }
        public static Objective GetObjective(int id)
        {
            using (var context = new DBLAccountOpeningContext())
            {
                return context.Objectives.Find(id);
            }
        }

        public static EmploymentStatu GetEmployeeStatus(int id)
        {
            using (var context = new DBLAccountOpeningContext())
            {
                return context.EmploymentStatus.Find(id);
            }
        }

        public static ModeOfInstruction GetModeOfInstruction(int id)
        {
            using (var context = new DBLAccountOpeningContext())
            {
                return context.ModeOfInstructions.Find(id);
            }
        }
        public static ModeOfNotification GetModeOfNotification(int id)
        {
            using (var context = new DBLAccountOpeningContext())
            {
                return context.ModeOfNotifications.Find(id);
            }
        }
        public static ModeOfReceivingStatement GetModeOfReceivingStatement(int id)
        {
            using (var context = new DBLAccountOpeningContext())
            {
                return context.ModeOfReceivingStatements.Find(id);
            }
        }

        public static SourceOfIncome GetSourceOfIncome(int id)
        {
            using (var context = new DBLAccountOpeningContext())
            {
                return context.SourceOfIncomes.Find(id);
            }
        }

        public static RiskTolerance GetRiskTolerance(int id)
        {
            using (var context = new DBLAccountOpeningContext())
            {
                return context.RiskTolerances.Find(id);
            }
        }

        public static AccountFinancialInvestmentRiskProfileModel GetAccountInvestmentRiskProfile(Guid accountId)
        {
            try
            {
                using (var context = new DBLAccountOpeningContext())
                {
                    var item = context.AccountFinancialInvestmentRiskProfiles.FirstOrDefault(x => x.AccountId == accountId);
                    var model = new AccountFinancialInvestmentRiskProfileModel
                    {
                        AccountId = item.AccountId,
                        AnnualIncomeId = item.AnnualIncomeId,
                        ApproximateAnnualIncomeName = item.AnnualIncomeId.HasValue ? GetApproximateIncome(item.AnnualIncomeId.Value).Name : string.Empty,
                        CreatedDate = item.CreatedDate,
                        Id = item.Id,
                        InvestmentHorizonId = item.InvestmentHorizonId,
                        InvestmentHorizonName = item.InvestmentHorizonId.HasValue ? GetInvestmentHorizon(item.InvestmentHorizonId.Value).Name : string.Empty,
                        InvestmentKnowledgeId = item.InvestmentKnowledgeId,
                        InvestmentKnowledgeName = item.InvestmentKnowledgeId.HasValue ? GetInvestmentKnowledge(item.InvestmentKnowledgeId.Value).Name : string.Empty,
                        NetworthId = item.NetworthId,
                        NetWorthName = item.NetworthId.HasValue ? GetNetWorth(item.NetworthId.Value).Name : string.Empty,
                        ObjectiveName = item.ObjectivesId.HasValue ? GetObjective(item.ObjectivesId.Value).Name : string.Empty,
                        ObjectivesId = item.ObjectivesId,
                        OnlineTradingFacility = item.OnlineTradingFacility,
                        RiskToleranceId = item.RiskToleranceId,
                        RiskToleranceName = item.RiskToleranceId.HasValue ? GetRiskTolerance(item.RiskToleranceId.Value).Name : string.Empty,
                        
                    };
                    return model;
                }
            }
            catch (Exception ex)
            {

                Logger.Instance.logError(ex);
                return null;
            }
        }

        public static List<AccountFileUploadModel> GetAccountFilesUploads(Guid accountId)
        {

            try
            {
                using (var context = new DBLAccountOpeningContext())
                {
                    var model = new List<AccountFileUploadModel>();
                    foreach (var item in context.AccountFileUploads.Where(x => x.AccountId == accountId && x.IsActive))
                    {
                        model.Add(new AccountFileUploadModel
                        {
                            AccountId = item.AccountId,
                            CreatedDate = item.CreatedDate,
                            FileUploadTypeName = item.FileUploadType.Name,
                            Id = item.Id,
                            IsActive = true,
                            Path = item.Path,
                            TypeId = item.TypeId,
                        });
                    }

                    return model;
                }

            }
            catch (Exception ex)
            {

                Logger.Instance.logError(ex);
                return null;
            }
        }
        public static AccountETIModel GetAccountETI(Guid accountId)
        {

            try
            {
                using (var context = new DBLAccountOpeningContext())
                {
                    var item = context.AccountETIs.FirstOrDefault(x => x.AccountId == accountId);
                    var model = new AccountETIModel
                    {
                        AccountId = item.AccountId,
                        CreatedDate = item.CreatedDate,
                        Email1 = item.Email1,
                        Email2 = item.Email2,
                        Id = item.Id,
                        Name1 = item.Name1,
                        Name2 = item.Name2,
                        Text1 = item.Text1,
                        Text2 = item.Text2,
                        Text3 = item.Text3,
                        Text4 = item.Text4,
                        Text5 = item.Text5
                    };
                    return model;
                }
            }
            catch (Exception ex)
            {

                Logger.Instance.logError(ex);
                return null;
            }
        }

        public static List<AccountTradingContactModel> GetAccountTradingContract(Guid accountId)
        {
            try
            {
                var model = new List<AccountTradingContactModel>();
                using (var context = new DBLAccountOpeningContext())
                {
                    foreach (var item in context.AccountInsTradingContacts.Where(x => x.AccountId == accountId))
                    {
                        model.Add(new AccountTradingContactModel
                        {
                            AccountId = accountId,
                            CreatedDate = item.CreatedDate,
                            Email = item.Email,
                            Fax = item.Fax,
                            Id = item.Id,
                            Mobile = item.Mobile,
                            Name = item.Name,
                            Tel = item.Tel,
                            TitleId = item.TitleId,
                            TitleName = item.TitleId.HasValue ? GetTitle(item.TitleId.Value).Name : string.Empty,
                        });
                    }
                    return model;
                }

            }
            catch (Exception ex)
            {

                Logger.Instance.logError(ex);
                return null;
            }
        }
        public static AccountCustomerDetailsModel GetAccountCustodyDetails(Guid accountId)
        {

            try
            {
                using (var context = new DBLAccountOpeningContext())
                {
                    var item = context.AccountInsCustodyAccountDetails.FirstOrDefault(x => x.AccountId == accountId);
                    var model = new AccountCustomerDetailsModel
                    {
                        AccountId = accountId,
                        Address = item.Address,
                        CashAccountNumber = item.CashAccountNumber,
                        CreatedDate = item.CreatedDate,
                        CustodianName = item.CustodianName,
                        Fax = item.Fax,
                        Id = item.Id,
                        SecuritiesAccountNumber = item.SecuritiesAccountNumber,
                        Telephone = item.Telephone
                    };
                    return model;
                }
            }
            catch (Exception ex)
            {

                Logger.Instance.logError(ex);
                return null;
            }

        }
        public static AccountSettlementDetailModel GetAccountSettlementDetail(Guid accountId)
        {
            try
            {
                using (var context = new DBLAccountOpeningContext())
                {
                    var item = context.AccountSettlementDetails.FirstOrDefault(x => x.AccountId == accountId);
                    var model = new AccountSettlementDetailModel
                    {
                        AccountId = item.AccountId,
                        AccountName = item.AccountName,
                        AccountNumber = item.AccountNumber,
                        BankId = item.BankId,
                        BankName = item.BankId.HasValue ? GetBank(item.BankId.Value).Name : string.Empty,
                        BIC = item.BIC,
                        Branch = item.Branch,
                        CorrespondentBankId = item.CorrespondentBankId,
                        CorrespondentBankName = item.CorrespondentBankId.HasValue ? GetBank(item.CorrespondentBankId.Value).Name : string.Empty,
                        CorrespondentBankSwiftCode = item.CorrespondentBankSwiftCode,
                        CreatedDate = item.CreatedDate,
                        Id = item.Id,
                        IntermediaryBankId = item.IntermediaryBankId,
                        IntermediaryBankName = item.IntermediaryBankId.HasValue ? GetBank(item.IntermediaryBankId.Value).Name : string.Empty,
                        IntermediaryBankSwiftCode = item.IntermediaryBankSwiftCode,
                        MarginTradingOption = item.MarginTradingOption,
                        NameOfBeneficiary = item.NameOfBeneficiary,
                        OnlineTradingFacility = item.OnlineTradingFacility,
                        SwiftCode = item.SwiftCode,

                    };
                    return model;
                }

            }
            catch (Exception ex)
            {

                Logger.Instance.logError(ex);
                return null;
            }
        }

        public static List<AccountNextOfKinDetailsModel> GetAccountNextOfKins(Guid accountId)
        {
            try
            {
                var model = new List<AccountNextOfKinDetailsModel>();
                using (var context = new DBLAccountOpeningContext())
                {
                    foreach (var item in context.AccountNextOfKinDetails.Where(x => x.AccountId == accountId))
                    {
                        model.Add(new AccountNextOfKinDetailsModel
                        {
                            AccountId = item.AccountId,
                            CreatedDate = item.CreatedDate,
                            Email = item.Email,
                            Fax = item.Fax,
                            Id = item.Id,
                            MailingAddress = item.MailingAddress,
                            Mobile = item.Mobile,
                            Name = item.Name,
                            RelationToAccount = item.RelationToAccount,
                        });
                    }
                }
                return model;
            }
            catch (Exception ex)
            {

                Logger.Instance.logError(ex);
                return null;
            }

        }
        public static List<AccountMemberModel> GetAccountMembers(Guid accountId)
        {
            try
            {
                var model = new List<AccountMemberModel>();
                using (var context = new DBLAccountOpeningContext())
                {
                    foreach (var item in context.AccountMembers.Where(x => x.AccountId == accountId))
                    {
                        model.Add(new AccountMemberModel
                        {
                            AccountId = item.AccountId,
                            CreatedDate = item.CreatedDate,
                            DOB = item.DOB,
                            Email = item.Email,
                            Fax = item.Fax,
                            Fname = item.Fname,
                            Gender = item.Gender,
                            Id = item.Id,
                            IdCardExpiryDate = item.IdCardExpiryDate,
                            IdCardIssueDate = item.IdCardIssueDate,
                            IdNumber = item.IdNumber,
                            IdPath = item.IdPath,
                            IdTypeId = item.IdTypeId,
                            IdTypeName = item.IdTypeId.HasValue ? GetIdType(item.IdTypeId.Value).Name : string.Empty,
                            IssuingAuthority = item.IssuingAuthority,
                            Lname = item.Lname,
                            MaidenName = item.MaidenName,
                            MailingAddressCity = item.MailingAddressCity,
                            MailingAddressCountryId = item.MailingAddressCountryId,
                            MailingAddressCountryName = item.MailingAddressCountryId.HasValue ? GetCountry(item.MailingAddressCountryId.Value).CountryName : string.Empty,
                            MailingAddressFull = item.MailingAddressFull,
                            MailingAddressZipCode = item.MailingAddressZipCode,
                            MaritalStatusId = item.MaritalStatusId,
                            MaritalStatusName = item.MaritalStatusId.HasValue ? context.MaritalStatus.Find(item.MaritalStatusId.Value).Name : string.Empty,
                            Mobile = item.Mobile,
                            MothersMaidenName = item.MothersMaidenName,
                            NationalityId = item.NationalityId,
                            NationalityName = item.NationalityId.HasValue ? GetCountry(item.NationalityId.Value).CountryName : string.Empty,
                            Occupation = item.Occupation,
                            Othername = item.Othername,
                            PlaceOfBirth = item.PlaceOfBirth,
                            ResidentialAddressFull = item.ResidentialAddressFull,
                            ResidentialCity = item.ResidentialCity,
                            ResidentialCountryId = item.ResidentialCountryId,
                            ResidentialCountryName = item.ResidentialCountryId.HasValue ? GetCountry(item.ResidentialCountryId.Value).CountryName : string.Empty,
                            ResidentialZipCode = item.ResidentialZipCode,
                            SelectWhereApplicableId = item.SelectWhereApplicableId,
                            SelectWhereApplicableName = item.SelectWhereApplicableId.HasValue ? GetCSDParticipationOption(item.SelectWhereApplicableId.Value).Name : string.Empty,
                            SignaturePath = item.SignaturePath,
                            Telephone = item.Telephone,
                            TitleId = item.TitleId,
                            TitleName = item.TitleId.HasValue ? GetTitle(item.TitleId.Value).Name : string.Empty,

                            IdValidated = item.IdValidated,
                            IdValidationBy = item.IdValidationBy,
                            IdValidationDate = item.IdValidationDate,
                            IdValidationMode = item.IdValidationMode,
                            IdCardValidationStatus=item.IdValidated?"Verified-"+item.IdValidationMode:"Not Verified"
                        });
                    }
                }
                return model;
            }
            catch (Exception ex)
            {

                Logger.Instance.logError(ex);
                return null;
            }

        }

        public static List<AccountSignatoryModel> GetAccountSignatories(Guid accountId)
        {

            try
            {
                using (var context = new DBLAccountOpeningContext())
                {
                    var model = new List<AccountSignatoryModel>();
                    foreach (var item in context.AccountInstSignatoriesMandates.Where(x => x.AccountId == accountId))
                    {
                        model.Add(new AccountSignatoryModel
                        {
                            AccountId = item.AccountId,
                            CreatedDate = item.CreatedDate,
                            Id = item.Id,
                            Name = item.Name,
                            Position = item.Position,
                            SignaturePath = item.SignaturePath
                        });
                    }
                    return model;
                }

            }
            catch (Exception ex)
            {

                Logger.Instance.logError(ex);
                return null;
            }
        }



        public static AccountInstructionEmploymentDetailModel GetAccountInstructionEmploymentDetailModel(Guid accountId)
        {
            try
            {
                using (var context = new DBLAccountOpeningContext())
                {
                    var item = context.AccountInstructionEmploymentDetails.FirstOrDefault(x => x.AccountId == accountId);
                    var model = new AccountInstructionEmploymentDetailModel
                    {
                        AccountId = item.AccountId,
                        CreatedDate = item.CreatedDate,
                        CurrentEmployer = item.CurrentEmployer,
                        CurrentEmployerAddress = item.CurrentEmployerAddress,
                        CurrentOccupation = item.CurrentOccupation,
                        EmploymentDateFrom = item.EmploymentDateFrom,
                        EmploymentDateTo = item.EmploymentDateTo,
                        EmploymentStatusId = item.EmploymentStatusId,
                        EmploymentStatusName = item.EmploymentStatusId.HasValue ? GetEmployeeStatus(item.EmploymentStatusId.Value).Name : string.Empty,
                        Id = item.Id,
                        ModeOfInstructionId = item.ModeOfInstructionId,
                        ModeOfInstructionName = item.ModeOfInstructionId.HasValue ? GetModeOfInstruction(item.ModeOfInstructionId.Value).Name : string.Empty,
                        ModeOfNotificationId = item.ModeOfNotificationId,
                        ModeOfNotificationName = item.ModeOfNotificationId.HasValue ? GetModeOfNotification(item.ModeOfNotificationId.Value).Name : string.Empty,
                        PrevEmployer = item.PrevEmployer,
                        PrevOccupation = item.PrevOccupation,
                        SourceOfFundId = item.SourceOfFundId,
                        SourceOfIncomeName = item.SourceOfFundId.HasValue ? GetSourceOfIncome(item.SourceOfFundId.Value).Name : string.Empty,
                        YearsOfEmployment = item.YearsOfEmployment
                    };
                    return model;
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
