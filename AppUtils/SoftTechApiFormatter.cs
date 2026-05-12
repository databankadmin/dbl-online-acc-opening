using DBHelper.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppUtils
{
    public static class SoftTechApiFormatter
    {
        public static string GetInvestmentTypeCode(int itemId)
        {
            using (var context=new DBLAccountOpeningContext())
            {
                var model = context.InvestmentTypes.FirstOrDefault(x=>x.Id==itemId);
                return model != null ? model.SoftTechCode : string.Empty;
            }
        }

        public static string GetBranchCode(string branchCode)
        {
            using (var context = new DBLAccountOpeningContext())
            {
                var model = context.Branches.FirstOrDefault(x => x.BRANCH_CODE == branchCode);
                return model != null ? model.SoftTechCode : string.Empty;
            }
        }

        public static string GetAccountTypeCode(int itemId)
        {
            using (var context = new DBLAccountOpeningContext())
            {
                var model = context.AccountTypes.FirstOrDefault(x => x.Id == itemId);
                return model != null ? model.SoftTechCode : string.Empty;
            }
        }


        public static string GetCountryCode(int itemId)
        {
            using (var context = new DBLAccountOpeningContext())
            {
                var model = context.Countries.FirstOrDefault(x => x.ID == itemId);
                return model != null ? model.SoftTechCode : string.Empty;
            }
        }

        public static string GetMaritalStatusCode(int itemId)
        {
            using (var context = new DBLAccountOpeningContext())
            {
                var model = context.MaritalStatus.FirstOrDefault(x => x.Id == itemId);
                return model != null ? model.SoftTechCode : string.Empty;
            }
        }

        public static string GetCityCode(int itemId)
        {
            using (var context = new DBLAccountOpeningContext())
            {
                var model = context.Cities.FirstOrDefault(x => x.Id == itemId);
                return model != null ? model.SoftTechCode : string.Empty;
            }
        }

        public static string GetCSDDepositoryParticipantOptionCode(int itemId)
        {
            using (var context = new DBLAccountOpeningContext())
            {
                var model = context.CSDDepositoryParticipantOptions.FirstOrDefault(x => x.Id == itemId);
                return model != null ? model.SoftTechCode : string.Empty;
            }
        }

        public static string GetOccupationCode(string item)
        {
            using (var context = new DBLAccountOpeningContext())
            {
                var model = context.Occupations.FirstOrDefault(x => x.Name.Trim() == item.Trim());
                return model != null ? model.SoftTechCode : string.Empty;
            }
        }


        public static string GetIdTypeCode(int itemId)
        {
            using (var context = new DBLAccountOpeningContext())
            {
                var model = context.IDCardTypes.FirstOrDefault(x => x.Id == itemId);
                return model != null ? model.SoftTechCode : string.Empty;
            }
        }

        public static string GetRelationshipCode(string item)
        {
            using (var context = new DBLAccountOpeningContext())
            {
                var model = context.Relationships.FirstOrDefault(x => x.Name.Trim() == item.Trim());
                return model != null ? model.SoftTechCode : context.Relationships.FirstOrDefault().SoftTechCode;
            }
        }

        public static string GetIncomeCode(int itemId)
        {
            using (var context = new DBLAccountOpeningContext())
            {
                var model = context.SourceOfIncomes.FirstOrDefault(x => x.Id == itemId);
                return model != null ? model.SoftTechCode : string.Empty;
            }
        }

        public static string GetIncomeRangeCode(int itemId)
        {
            using (var context = new DBLAccountOpeningContext())
            {
                var model = context.ApproximateAnnualIncomes.FirstOrDefault(x => x.Id == itemId);
                return model != null ? model.SoftTechCode : string.Empty;
            }
        }
        public static string GetRiskToleranceCode(int itemId)
        {
            using (var context = new DBLAccountOpeningContext())
            {
                var model = context.RiskTolerances.FirstOrDefault(x => x.Id == itemId);
                return model != null ? model.SoftTechCode : string.Empty;
            }
        }


        public static string GetEmploymentStatusCode(int itemId)
        {
            using (var context = new DBLAccountOpeningContext())
            {
                var model = context.EmploymentStatus.FirstOrDefault(x => x.Id == itemId);
                return model != null ? model.SoftTechCode : string.Empty;
            }
        }

        public static string GetExpectedActivityCode(int itemId)
        {
            using (var context = new DBLAccountOpeningContext())
            {
                var model = context.ExpectedAccountActivities.FirstOrDefault(x => x.Id == itemId);
                return model != null ? model.SoftTechCode : string.Empty;
            }
        }


        public static string GetModeOfInstructionCode(int itemId)
        {
            using (var context = new DBLAccountOpeningContext())
            {
                var model = context.ModeOfInstructions.FirstOrDefault(x => x.Id == itemId);
                return model != null ? model.SoftTechCode : string.Empty;
            }
        }

        public static string GetModeOfNotificationCode(int itemId)
        {
            using (var context = new DBLAccountOpeningContext())
            {
                var model = context.ModeOfNotifications.FirstOrDefault(x => x.Id == itemId);
                return model != null ? model.SoftTechCode : string.Empty;
            }
        }
        public static string GetStatementFrequencyCode(int freqId)
        {
            if (freqId==1)
            {
                return "M";
            }
            else if (freqId==2)
            {
                return "Q";
            }
            else if (freqId == 3)
            {
                return "A";
            }
            else
            {
                return "M";
            }
        }

        public static string GetGenderCode(string gender)
        {
            if (gender.Trim().ToUpper()== "MALE")
            {
                return "M";
            }
            else if (gender.Trim().ToUpper() == "FEMALE")
            {
                return "F";
            }
            else
            {
                return "O";
            }
        }

        public static string GetSignatoryCode(int itemId)
        {
            using (var context = new DBLAccountOpeningContext())
            {
                var model = context.SignatureTypes.FirstOrDefault(x => x.Id == itemId);
                return model != null ? model.SoftTechCode : string.Empty;
            }
        }
    }
}
