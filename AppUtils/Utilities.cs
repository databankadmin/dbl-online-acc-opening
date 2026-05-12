using AppLogger;
using AppModels;
using DBHelper.Schema;
using Spire.Pdf;
using Spire.Pdf.Graphics;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AppUtils
{
    public static class Utilities
    {
        public static List<UserModel> AppUsers { get; set; }

        public static AppSetting GetAppSettings()
        {
            using (var context = new DBLAccountOpeningContext())
            {
                return context.AppSettings.FirstOrDefault();
            }
        }
        public static string GetBaseUrl()
        {
            return ConfigurationManager.AppSettings["APP_BASE_URL"];
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

        public static DateTime ConvertStringToDateTime(string dateString)
        {
            string format = "yyyy-MM-dd";
            DateTime thedate = DateTime.ParseExact(dateString, format, CultureInfo.InvariantCulture);
            return thedate;
        }

        public static string GetAMLRating(decimal amlScore)
        {
            if (amlScore <= 60)
            {
                return "Low";
            }
            else if (amlScore <= 99)
            {
                return "Medium";
            }
            else
            {
                return "High";
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
                return context.AppUsers.FirstOrDefault(x => x.Email == username && x.Password == encryptedPassword && x.IsActive);
            }
        }
        public static object GetSessionUser()
        {
            try
            {
                var context = new DBLAccountOpeningContext();
                string username = HttpContext.Current.User.Identity.Name;
                if (string.IsNullOrEmpty(username))
                {
                    return null;
                }
                var selectUser = AppUsers.FirstOrDefault(x => x.Username == username);
                return selectUser;
            }
            catch (Exception ex)
            {

                Logger.Instance.logError(ex);
                return null;
            }

        }


        public static string GetStaffSignature(string username)
        {
            using (var context = new DBLAccountOpeningContext())
            {
                var user = context.AppUsers.FirstOrDefault(x => x.Email.Trim().ToLower() == username.Trim().ToLower());
                return user != null ? user.SignatureImg : string.Empty;
            }

        }

        public static object GetSessionUserClient()
        {
            var context = new DBLAccountOpeningContext();
            string username = HttpContext.Current.User.Identity.Name;
            var selectUser = context.Accounts.FirstOrDefault(x => x.ReferenceNo == username);
            return selectUser;

        }
        public static string GetPdfThumbnail(string sourceFile, int width, int height)
        {
            //file must come in pdf

            try
            {
                //string inputPdfPath = HttpContext.Current.Server.MapPath("~/Images/" + sourceFile);
                //string newName = Guid.NewGuid().ToString();
                //string pageFilePath = HttpContext.Current.Server.MapPath("~/Images/" + newName + ".png");
                //PdfDocument doc = new PdfDocument();
                //doc.LoadFromFile(inputPdfPath);
                //Image bmp = doc.SaveAsImage(0, width, height);
                //bmp.Save(pageFilePath, ImageFormat.Png);
                //string returnVal = newName + ".png";
                //return returnVal;


                PdfDocument documemt = new PdfDocument();
                string path = HttpContext.Current.Server.MapPath("~/Images/" + sourceFile);
                documemt.LoadFromFile(path);
                var image = documemt.SaveAsImage(0, (Spire.Pdf.Graphics.PdfImageType)PdfImageType.Bitmap, 200, 250);
                string saveFileName = Guid.NewGuid().ToString() + ".png";
                string savePath = HttpContext.Current.Server.MapPath("~/Images/" + saveFileName);

                image.Save(savePath);
                documemt.Close();
                return saveFileName;

            }
            catch (Exception ex)
            {
                Logger.Instance.logError(ex);
                return string.Empty;
            }

        }

        public static Account ValidateClientAccountLogin(string username, string password)
        {
            var context = new DBLAccountOpeningContext();
            string encryptedPassword = Utilities.EncodeBase64(password);
            var account = context.Accounts.FirstOrDefault(x => x.ReferenceNo == username && x.Password == encryptedPassword);
            return account;
        }
        public static void LogActivity(string username, string activity)
        {
            using (var context = new DBLAccountOpeningContext())
            {
                context.AuditLogs.Add(new AuditLog
                {
                    Activity = activity,
                    CreatedDate = DateTime.Now,
                    Id = Guid.NewGuid(),
                    UserId = username
                });
                context.SaveChanges();
            }
        }
        public static void ProcessEmails()
        {
            using (var context = new DBLAccountOpeningContext())
            {
                context.SaveChanges();

                foreach (var email in context.MessageHistories.Where(x => x.Type == "EMAIL" && x.IsSent == false && !string.IsNullOrEmpty(x.SentTo)).Take(20))
                {
                    try
                    {
                        string title = !string.IsNullOrEmpty(email.Title) ? email.Title : "Databank";
                        var attachments = new List<string>();
                        //if (!string.IsNullOrEmpty(email.Attachments))
                        //{
                        //    foreach (var file in email.Attachments.Split(','))
                        //    {
                        //        attachments.Add(file);
                        //    }
                        //}

                        var result = SendMail(email.SentTo, email.MessageContent, title, attachments);
                        if (result)
                        {
                            email.IsSent = true;
                        }

                    }
                    catch (Exception ex)
                    {

                        Logger.Instance.logError(ex);
                    }
                }

                context.SaveChanges();

            }


        }


        public static bool SendMail(string recipient, string message, string title, List<string> attachments = null)
        {
            bool flag;
            try
            {
                var context = new DBLAccountOpeningContext();
                var emailSettings = context.MailServerSettings.FirstOrDefault();
                string smtpAddress = emailSettings.HostName;
                int portNumber = int.Parse(emailSettings.PortNumber);
                string emailFromAddress = emailSettings.EmailAddress;
                string password = emailSettings.Password;
                string emailToAddress = recipient;
                string subject = title;
                string body = message;

                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(emailFromAddress);
                    mail.To.Add(emailToAddress);
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = true;
                    if (attachments?.Count > 0)
                    {
                        foreach (var file in attachments)
                        {
                            mail.Attachments.Add(new Attachment(file));//--Uncomment this to send any attachment  

                        }
                    }

                    using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                    {
                        smtp.Credentials = new NetworkCredential(emailFromAddress, password);
                        smtp.EnableSsl = false;
                        smtp.Send(mail);
                    }

                    context.SaveChanges();
                }

            }
            catch (Exception exception)
            {
                Logger.Instance.logError(exception);
                flag = false;
                return flag;
            }
            flag = true;
            return flag;
        }



        public static void MarkIdCardAsValidated(string recordId, string objectType)
        {
            using (var context = new DBLAccountOpeningContext())
            {
                if (objectType == "AccMember")
                {
                    var model = context.AccountMembers.FirstOrDefault(x => x.Id.ToString() == recordId);
                    if (model != null && !model.IdValidated)
                    {
                        model.IdValidated = true;
                        model.IdValidationDate = DateTime.Now;
                        model.IdValidationBy = (GetSessionUser() as UserModel).Username;
                        model.IdValidationMode = "GVIVE";

                    }
                }
                else if (objectType == "AuthPerson")
                {
                    var model = context.AccountAuthorisedPersons.FirstOrDefault(x => x.Id.ToString() == recordId);
                    if (model != null && !model.IdValidated)
                    {
                        model.IdValidated = true;
                        model.IdValidationDate = DateTime.Now;
                        model.IdValidationBy = (GetSessionUser() as UserModel).Username;
                        model.IdValidationMode = "GVIVE";
                    }
                }
                context.SaveChanges();
            }
        }


        public static string GenerateApplicationReference()
        {
            using (var context = new DBLAccountOpeningContext())
            {
                string _ref = string.Empty;
                var rand = new Random();
                while (true)
                {
                    _ref = rand.Next(10000000, 99999999).ToString();
                    if (!context.Accounts.Any(x => x.ReferenceNo == _ref))
                    {
                        break;
                    }
                }
                return _ref;
            }

        }

        public static List<AccountBasicModel> GetApplications(int accountType = 0, int investmentTypeId = 0, string dates = null, string applicantionId = null, int statusId = 0, string key = null)
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
                    if (statusId > 0)
                    {
                        applications = applications.Where(x => x.StatusId == statusId);
                    }

                    if (!string.IsNullOrEmpty(dates))
                    {
                        string[] dateArr = dates.Split(new char[] { '-' });

                        var startDate = DateTime.Parse(string.Concat(dateArr[0], " 00:00:00"));
                        var endDate = DateTime.Parse(string.Concat(dateArr[1], " 23:59:59"));
                        applications = applications.Where(x => x.CreatedDate >= startDate && x.CreatedDate <= endDate).AsEnumerable();

                    }
                    if (!string.IsNullOrEmpty(key))
                    {
                        applications = applications.Where(x => x.ReferenceNo == key);
                    }
                    if (!string.IsNullOrEmpty(applicantionId))
                    {
                        applications = context.Accounts.Where(x => x.Id.ToString() == applicantionId);
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
                            ExpectedAccountActivityName = item.ExpectedAccountActivityId.HasValue ? GetExpectedAccountActivity(item.ExpectedAccountActivityId.Value).Name : string.Empty,
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
                            StatusId = item.StatusId,
                            StatusName = item.ApplicationStatu.Name,
                            SuccesfulReviewBy = item.SuccesfulReviewBy,
                            SuccessfullyReviewwedByName = item.Reviewer,//item.SuccesfulReviewBy.HasValue ? context.AppUsers.Find(item.SuccesfulReviewBy.Value).Name : string.Empty,
                            SuccessfulReviewDate = item.SuccessfulReviewDate,
                            CancelOrRejectBy = item.CancelOrRejectBy,
                            CancelOrrejectByName = item.CancelOrRejectBy.HasValue ? context.AppUsers.Find(item.CancelOrRejectBy.Value).Name : string.Empty,
                            CancelOrRejectComment = item.CancelOrRejectComment,
                            CancelOrRejectDate = item.CancelOrRejectDate,
                            RefNo = item.ReferenceNo,
                            StaffRefCode = item.StaffRefCode,
                            BackConnectAccountNumber = item.BackConnectAccountNumber,
                            StaffRefName = item.StaffRefCode.HasValue ? context.StaffRefLists.Find(item.StaffRefCode).Staff_Name : string.Empty,
                            BranchCode = item.BranchCode,
                            CreatedBy = item.CreatedBy,

                            Password = !string.IsNullOrEmpty(item.Password) ? DecodeBase64(item.Password) : string.Empty,





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

        public static bool IsNumber(string zipCode)
        {
            var stringNumber = zipCode;
            int numericValue;
            bool isNumber = int.TryParse(stringNumber, out numericValue);
            return isNumber;
        }

        public static string SaveBase64AsImage(string FileName, string FileType, string Base64ImageString)
        {
            try
            {
                Base64ImageString = Base64ImageString.Replace("data:image/png;base64,", "").Trim();

                string folder = System.Web.HttpContext.Current.Server.MapPath("~/Images");

                string filePath = folder + "/" + FileName + "." + FileType;
                File.WriteAllBytes(filePath, Convert.FromBase64String(Base64ImageString));
                string returnFileName = FileName + "." + FileType;
                return returnFileName;
            }
            catch
            {
                return null;
            }

        }


        public static bool IsFilePdf(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return false;
            }
            fileName = fileName.Trim().ToLower();
            if (fileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
        }


        public static string GetRandomBranchCode()
        {
            using (var context = new DBLAccountOpeningContext())
            {
                var rnd = new Random();
                var all = context.Branches.ToList();
                int index = rnd.Next(0, (all.Count() - 1));
                var branch = all.ElementAt(index);
                return branch.BRANCH_CODE;
            }

        }

        public static Country GetCountry(int countryId)
        {
            using (var context = new DBLAccountOpeningContext())
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

        public static int ComputeYears(string instructionsEmploymentDetailsCurrentEmployerFrom, string instructionsEmploymentDetailsCurrentEmployerTo)
        {
            try
            {
                var startDate = DateTime.Parse(instructionsEmploymentDetailsCurrentEmployerFrom);
                var endDate = DateTime.Parse(instructionsEmploymentDetailsCurrentEmployerTo);
                var yr = endDate.Year - startDate.Year - 1 +
             (endDate.Month >= startDate.Month && endDate.Day >= startDate.Day ? 1 : 0);
                return yr < 0 ? 0 : yr;
            }
            catch (Exception)
            {

                return -1;
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
                        var members = account.AccountMembers.OrderBy(x => x.CreatedDate);
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

            var model = new List<AccountAMLReponseModel>();
            using (var context = new DBLAccountOpeningContext())
            {
                foreach (var item in context.AccountAMLResponses.Where(x => x.AccountId == accountId).OrderBy(x => x.QuestionId))
                {
                    model.Add(new AccountAMLReponseModel
                    {
                        AccountId = item.AccountId,
                        CreatedDate = item.CreatedDate,
                        Id = item.Id,
                        QuestionId = item.QuestionId,
                        QuestionText = item.AMLQuestion.Question,
                        Rank = item.Rank,
                        RatingValue = item.RatingValue,
                        Remark = item.Remark,
                        YesNo = item.YesNo,
                        RatingValueTxt = item.RatingValue >= 0 ? item.RatingValue.ToString() : "N/A"

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
            if (members.Any(x => !x.IdValidated) || authorisedPersons.Any(x => !x.IdValidated))
            {
                hasUnverifiedId = true;
            }
            return hasUnverifiedId;
        }

        public static string GetTitleText(int? id)
        {
            using (DBLAccountOpeningContext accountOpeningContext = new DBLAccountOpeningContext())
                return accountOpeningContext.Titles.Find(id).Name;
        }

        public static List<AccountAuthorisedPersonModel> GetAccountAuthorisedPersons(Guid accountId)
        {
            var model = new List<AccountAuthorisedPersonModel>();
            try
            {
                using (var context = new DBLAccountOpeningContext())
                {
                    if (!context.AccountAuthorisedPersons.Any(x => x.AccountId == accountId))
                    {
                        context.AccountAuthorisedPersons.Add(new AccountAuthorisedPerson
                        {

                            Id = Guid.NewGuid(),
                            AccountId = accountId,
                            CreatedDate = DateTime.Now,
                            Name = "N/A",
                            Tel = "0000000000",
                            Mobile = "0000000000",
                            Fax = "N/A",
                            Email = "",
                            RelationToAccountHolder = "N/A",
                            StreetAddress = "N/A",
                            City = "N/A",
                            ZipCode = "N/A",
                            IdNumber = "N/A",
                            IdIssueAuthority = "N/A",
                            IssueDate = "N/A",
                            IDExpiryDate = "N/A",
                            IdPath = null,// !string.IsNullOrEmpty(_instAuthorisedOfficer1PhotoId) ? _instAuthorisedOfficer1PhotoId : null,
                            MailingAddress = null,
                            SignaturePath = null,

                        });
                        context.SaveChanges();
                    }
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
                            IdValidated = item.IdValidated,
                            IdValidationBy = item.IdValidationBy,
                            IdValidationDate = item.IdValidationDate,
                            IdValidationMode = item.IdValidationMode,
                            IdCardValidationStatus = item.IdValidated ? "Verified-" + item.IdValidationMode : "Not Verified",

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


        public static string SoftTechDateFormatter(string date)
        {
            //date: yyyy-MM-dd
            //return: dd-MM-yyyy
            try
            {
                var arr = date.Split('-');
                string formattedDate = arr[2] + "/" + arr[1] + "/" + arr[0];
                return formattedDate;
            }
            catch (Exception ex)
            {

                return null;
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
                    if (item == null)
                    {
                        return null;
                    }
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
                    foreach (var item in context.AccountFileUploads.Where(x => x.AccountId == accountId && x.IsActive).OrderByDescending(x => x.CreatedDate))
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


        public static string GetInitialsFromString(string str)
        {
            string[] output = str.Split(' ');
            string initials = string.Empty;
            foreach (string s in output)
            {
                initials = initials + " " + s[0];
            }
            return initials;


        }

        public static AccountETIModel GetAccountETI(Guid accountId)
        {

            try
            {
                using (var context = new DBLAccountOpeningContext())
                {
                    var account = context.Accounts.Find(accountId);
                    if (!context.AccountETIs.Any(x => x.AccountId == accountId))
                    {
                        context.AccountETIs.Add(new AccountETI
                        {
                            AccountId = accountId,
                            CreatedDate = DateTime.Now,
                            Id = Guid.NewGuid(),
                            Name1 = account.DeclarationIWe,


                        });
                        context.SaveChanges();
                    }
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
                    if (item == null)
                    {
                        return null;
                    }
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
                    if (!context.AccountSettlementDetails.Any(x => x.AccountId == accountId))
                    {

                        context.AccountSettlementDetails.Add(new AccountSettlementDetail
                        {
                            AccountId = accountId,
                            AccountName = "N/A",
                            AccountNumber = "N/A",
                            Id = Guid.NewGuid(),
                            BankId = null,
                            Branch = "N/A",
                            CreatedDate = DateTime.Now,
                            SwiftCode = "N/A",
                        });
                        context.SaveChanges();
                    }
                    var item = context.AccountSettlementDetails.FirstOrDefault(x => x.AccountId == accountId);
                    if (item == null)
                    {
                        var createOnFly = new AccountSettlementDetailModel
                        {
                            AccountId = accountId,
                            AccountName = "N/A",
                            AccountNumber = "N/A",
                            BankId = null,
                            BankName = "N/A",
                            BIC = "N/A",
                            Branch = "N/A",
                            CorrespondentBankId = null,
                            CorrespondentBankName = "N/A",
                            CorrespondentBankSwiftCode = "N/A",
                            CreatedDate = item.CreatedDate,
                            Id = Guid.NewGuid(),
                            IntermediaryBankId = null,
                            IntermediaryBankName = "N/A",
                            IntermediaryBankSwiftCode = "N/A",
                            MarginTradingOption = "N/A",
                            NameOfBeneficiary = "N/A",
                            OnlineTradingFacility = "YES",
                            SwiftCode = "NA",

                        };
                        return createOnFly;
                        //context.AccountSettlementDetails.Add(createOnFly);

                    }
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
                    if (!context.AccountNextOfKinDetails.Any(x => x.AccountId == accountId))
                    {
                        context.AccountNextOfKinDetails.Add(new AccountNextOfKinDetail
                        {
                            AccountId = accountId,
                            CreatedDate = DateTime.Now,
                            Email = "N/A",
                            Fax = "N/A",
                            Id = Guid.NewGuid(),
                            MailingAddress = "N/A",
                            Mobile = "N/A",
                            Name = "N/A",
                            RelationToAccount = "N/A",
                            Telephone = "N/A"
                        });
                        context.SaveChanges();
                    }

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

        public static List<AccountBeneficiaryModel> GetBeneficiaries(Guid accountId)
        {
            try
            {
                List<AccountBeneficiaryModel> beneficiaries = new List<AccountBeneficiaryModel>();
                using (DBLAccountOpeningContext accountOpeningContext = new DBLAccountOpeningContext())
                {
                    DbSet<AccountBeneficiary> accountBeneficiaries = accountOpeningContext.AccountBeneficiaries;
                    Expression<Func<AccountBeneficiary, bool>> predicate = (Expression<Func<AccountBeneficiary, bool>>)(x => x.AccountId == accountId);
                    foreach (AccountBeneficiary accountBeneficiary in (IEnumerable<AccountBeneficiary>)accountBeneficiaries.Where<AccountBeneficiary>(predicate))
                    {
                        List<AccountBeneficiaryModel> beneficiaryModelList = beneficiaries;
                        AccountBeneficiaryModel beneficiaryModel = new AccountBeneficiaryModel();
                        beneficiaryModel.AccountId = accountBeneficiary.AccountId;
                        beneficiaryModel.Fullname = accountBeneficiary.Fullname;
                        beneficiaryModel.Phone = accountBeneficiary.Phone;
                        beneficiaryModel.PercentageAllocation = accountBeneficiary.PercentageAllocation;
                        beneficiaryModel.CreatedDate = accountBeneficiary.CreatedDate;
                        beneficiaryModel.Relation = accountBeneficiary.Relation;
                        int? nullable = accountBeneficiary.TitleId;
                        int num1;
                        if (!nullable.HasValue)
                        {
                            num1 = 0;
                        }
                        else
                        {
                            nullable = accountBeneficiary.TitleId;
                            num1 = nullable.Value;
                        }
                        beneficiaryModel.TitleId = num1;
                        beneficiaryModel.OtherTitleDetails = accountBeneficiary.OtherTitleDetails;
                        beneficiaryModel.Surname = accountBeneficiary.Surname;
                        beneficiaryModel.Othernames = accountBeneficiary.Othernames;
                        beneficiaryModel.MaritalStatusId = accountBeneficiary.MaritalStatusId;
                        nullable = accountBeneficiary.GenderId;
                        int num2;
                        if (!nullable.HasValue)
                        {
                            num2 = 0;
                        }
                        else
                        {
                            nullable = accountBeneficiary.GenderId;
                            num2 = nullable.Value;
                        }
                        beneficiaryModel.GenderId = num2;
                        beneficiaryModel.BirthDate = accountBeneficiary.BirthDate;
                        beneficiaryModel.PlaceOfBirth = accountBeneficiary.PlaceOfBirth;
                        nullable = accountBeneficiary.CountryOfOriginId;
                        int num3;
                        if (!nullable.HasValue)
                        {
                            num3 = 0;
                        }
                        else
                        {
                            nullable = accountBeneficiary.CountryOfOriginId;
                            num3 = nullable.Value;
                        }
                        beneficiaryModel.CountryOfOriginId = num3;
                        nullable = accountBeneficiary.CountryOfResidenceId;
                        int num4;
                        if (!nullable.HasValue)
                        {
                            num4 = 0;
                        }
                        else
                        {
                            nullable = accountBeneficiary.CountryOfResidenceId;
                            num4 = nullable.Value;
                        }
                        beneficiaryModel.CountryOfResidenceId = num4;
                        beneficiaryModel.IDCardTypeId = accountBeneficiary.IDCardTypeId;
                        beneficiaryModel.IDNumber = accountBeneficiary.IDNumber;
                        beneficiaryModel.IDPlaceOfIssue = accountBeneficiary.IDPlaceOfIssue;
                        beneficiaryModel.IDIssueDate = accountBeneficiary.IDIssueDate;
                        beneficiaryModel.IDExpiryDate = accountBeneficiary.IDExpiryDate;
                        beneficiaryModelList.Add(beneficiaryModel);
                    }
                }
                return beneficiaries;
            }
            catch (Exception ex)
            {
                Logger.Instance.logError(ex);
                return (List<AccountBeneficiaryModel>)null;
            }
        }

        public static List<AccountMemberModel> GetAccountMembers(Guid accountId)
        {
            try
            {
                var model = new List<AccountMemberModel>();
                using (var context = new DBLAccountOpeningContext())
                {
                    foreach (var item in context.AccountMembers.Where(x => x.AccountId == accountId).OrderBy(x => x.CreatedDate))
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
                            IdCardValidationStatus = item.IdValidated ? "Verified-" + item.IdValidationMode : "Not Verified",
                            ResidentPermitNo = item.ResidentPermitNumber,
                            ResidentPermitExpiryDate = item.ResidentPermitNumberExpiryDate,
                            ResidentPermitIssueDate = item.ResidentPermitNumberIssueDate,
                            ResidentPermitPlaceOfIssue = item.ResidentPermitNumberPlaceOfIssue,
                            Profession = item.Profession,
                            ProfessionLinNumber = item.ProfessionalLicenceNumber,
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
                    foreach (var item in context.AccountInstSignatoriesMandates.Where(x => x.AccountId == accountId).OrderBy(x => x.CreatedDate))
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


        public static string GetSourceOfFundsList(string listIds)
        {
            try
            {
                using (var context = new DBLAccountOpeningContext())
                {
                    var names = new List<string>();
                    var ids = listIds.Split(',');
                    foreach (var item in ids)
                    {
                        int id = int.Parse(item);
                        var find = context.SourceOfIncomes.Find(id);
                        names.Add(find.Name);
                    }
                    return string.Join(",", names);
                }
            }
            catch (Exception)
            {

                return string.Empty;
            }
        }

        public static string GeneratePassword(int length)
        {
            try
            {
                string arr = ConfigurationManager.AppSettings["PasswordStringArray"];
                Random rnd = new Random();
                string pass = string.Empty;
                for (int i = 1; i <= length; i++)
                {
                    char chr = arr[rnd.Next(0, 50)];
                    pass = string.Concat(pass, chr.ToString());
                }
                return pass.Trim();
            }
            catch (Exception ex)
            {

                Logger.Instance.logError(ex);
                return string.Empty;
            }
        }


        public static AccountInstructionEmploymentDetailModel GetAccountInstructionEmploymentDetailModel(Guid accountId)
        {
            try
            {
                using (var context = new DBLAccountOpeningContext())
                {
                    if (!context.AccountInstructionEmploymentDetails.Any(x => x.AccountId == accountId))
                    {
                        var instructionEmploymentDetails = new AccountInstructionEmploymentDetail
                        {
                            AccountId = accountId,
                            CreatedDate = DateTime.Now,
                            ModeOfInstructionId = 3,//instructionsEmploymentDetailsModeOfInstruction,
                            ModeOfNotificationId = 2,// instructionsEmploymentDetailsModeOfNotification,
                            SourceOfFundId = 18,//instructionsEmploymentDetailsSourceOfIncome,
                            EmploymentStatusId = 1,//instructionsEmploymentDetailsEmploymentStatus,
                            PrevOccupation = "NA",//instructionsEmploymentDetailsPreviousOccupation,
                            PrevEmployer = "NA",
                            CurrentOccupation = "NA",//instructionsEmploymentDetailsCurrentOccupation,
                            CurrentEmployer = "NA",// instructionsEmploymentDetailsCurrentEmployer,
                            CurrentEmployerAddress = "NA", //instructionsEmploymentDetailsCurrentEmployerAddress,
                            EmploymentDateFrom = null,// instructionsEmploymentDetailsCurrentEmployerFrom,
                            EmploymentDateTo = null,//instructionsEmploymentDetailsCurrentEmployerTo,
                            Id = Guid.NewGuid(),
                            YearsOfEmployment = 0

                        };
                        context.AccountInstructionEmploymentDetails.Add(instructionEmploymentDetails);
                        context.SaveChanges();
                    }
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
                        YearsOfEmployment = item.YearsOfEmployment,
                        SourceOfIncomeNamesList = GetSourceOfFundsList(item.SourceOfFundsIds),
                        SourceOfFundsIds = item.SourceOfFundsIds
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
