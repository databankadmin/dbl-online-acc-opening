using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppModels
{

    public class SoftTechInvestmentTypeModel
    {
        public string investmentTypeCode { get; set; }
        public string investmentTypeDesc { get; set; }
    }

    public class SoftTechCountryModel
    {
        public string countryCode { get; set; }
        public string countryName { get; set; }
    }

    public class SoftTechMaritalStatusModel
    {
        public int statusCode { get; set; }
        public string statusName { get; set; }
    }

    public class SoftTechCityModel
    {
        public string cityCode { get; set; }
        public string cityName { get; set; }
    }

    public class SoftTechSuffixModel
    {
        public string suffixCode { get; set; }
        public string suffixName { get; set; }
    }


    public class SoftTechOccupationModel
    {
        public string occupationCode { get; set; }
        public string occupationDesc { get; set; }
    }


    public class SoftTechIDTypeModel
    {
        public string typeCode { get; set; }
        public string typeDesc { get; set; }
    }



    public class SoftTechRelationshipModel
    {
        public string relationshipCode { get; set; }
        public string relationshipName { get; set; }
    }

    public class SoftTechIncomeSourceModel
    {
        public int incomeSourceId { get; set; }
        public string incomeSourceDesc { get; set; }
    }


    public class SoftTechAnnualIncomeRangesModel
    {
        public int annualIncomeId { get; set; }
        public string description { get; set; }
    }


    public class SoftTechAnnualRiskToleranceModel
    {
        public int riskToleranceId { get; set; }
        public string riskToleranceDesc { get; set; }
    }


    public class SoftTechAccTypeModel
    {
        public string natureCode { get; set; }
        public string natureName { get; set; }
    }


    public class SoftTechBranchModel
    {
        public string branchCode { get; set; }
        public string branchName { get; set; }
    }


    //public class SoftTechCreateAccountModel
    //{
    //    public string INVESTMENT_TYPE { get; set; }
    //    public string EXPECTED_ACCOUNT_ACTIVITY { get; set; }
    //    public string TIN_NUM { get; set; }
    //    public string AUTH_TITLE { get; set; }
    //    public string SUR_NAME { get; set; }
    //    public string FIRST_NAME { get; set; }
    //    public string OTHER_NAME { get; set; }
    //    public string GENDER { get; set; }
    //    public string NATIONALITY { get; set; }
    //    public string DATE_OF_BIRTH { get; set; }
    //    public string MARITIAL_STATUS { get; set; }
    //    public string MOTHER_MAIDEN_NAME { get; set; }
    //    public string MAIDEN_NAME { get; set; }
    //    public string MAILING_ADDRESS { get; set; }
    //    public string MAILING_CITY { get; set; }
    //    public string MAILING_ZIP_CODE { get; set; }
    //    public string MAILING_COUNTRY { get; set; }
    //    public string RESIDENTIAL_ADDRESS { get; set; }
    //    public string RESIDENTIAL_CITY { get; set; }
    //    public string RESIDENTIAL_ZIP_CODE { get; set; }
    //    public string RESIDENTIAL_COUNTRY { get; set; }
    //    public string APPLICABLE_SUFFIX { get; set; }
    //    public string TEL_NUM { get; set; }
    //    public string MOBILE_NUM { get; set; }
    //    public string FAX_NUM { get; set; }
    //    public string OCCUPATION { get; set; }
    //    public string EMAIL { get; set; }
    //    public string ID_TYPE { get; set; }
    //    public string ID_NUMBER { get; set; }
    //    public string ID_ISSUE_AUTHORITY { get; set; }
    //    public string ID_ISSUE_DATE { get; set; }
    //    public string ID_EXPIRY_DATE { get; set; }
    //    public string ID_UPLOAD_IMAGE_ID { get; set; }
    //    public string SIGNATURE_IMAGE_ID { get; set; }
    //    public string FIRST_KIN_NAME { get; set; }
    //    public string FIRST_KIN_TEL_NUM { get; set; }
    //    public string FIRST_KIN_MOBILE_NUM { get; set; }
    //    public string FIRST_KIN_FAX_NUM { get; set; }
    //    public string FIRST_KIN_RELATIONSHIP { get; set; }
    //    public string FIRST_KIN_EMAIL { get; set; }
    //    public string FIRST_KIN_MAILING_ADDRESS { get; set; }
    //    public string SECOND_KIN_NAME { get; set; }
    //    public string SECOND_KIN_TEL_NUM { get; set; }
    //    public string SECOND_KIN_MOBILE_NUM { get; set; }
    //    public string SECOND_KIN_FAX_NUM { get; set; }
    //    public string SECOND_KIN_RELATION { get; set; }
    //    public string SECOND_KIN_EMAIL { get; set; }
    //    public string SECOND_KIN_MAILING_ADDRESS { get; set; }
    //    public string AUTH_PERSON_NAME { get; set; }
    //    public string AUTH_PERSON_TEL_NUM { get; set; }
    //    public string AUTH_PERSON_MOB_NUM { get; set; }
    //    public string AUTH_PERSON_FAX { get; set; }
    //    public string AUTH_PERSON_RELATION { get; set; }
    //    public string AUTH_PERSON_EMAIL { get; set; }
    //    public string AUTH_PERSON_MAILING_ADD { get; set; }
    //    public string AUTH_PERSON_CITY { get; set; }
    //    public string AUTH_PERSON_ZIP { get; set; }
    //    public string AUTH_PERSON_COUNTRY { get; set; }
    //    public string AUTH_PERSON_NIC_PHOTO_ID { get; set; }
    //    public string AUTH_PERSON_NIC_ISSUE_ATHORITY { get; set; }
    //    public string AUTH_PERSON_NIC_ISSUE_DATE { get; set; }
    //    public string AUTH_PERSON_NIC_EXPIRY_DATE { get; set; }
    //    public string AUTH_PERSON_NIC_IMAGE_UPLOD_ID { get; set; }
    //    public string AUTH_PERSON_NIC_NUM { get; set; }
    //    public string COMM_BANK_ACCT_NAME { get; set; }
    //    public string COMM_BANK_ACCOT_NO { get; set; }
    //    public string COMM_BANK_SELECTION { get; set; }
    //    public string BANK_BRANCH { get; set; }
    //    public string SWIFT_SORT_CODE { get; set; }
    //    public string MODE_OF_INSTRUCTIONS { get; set; }
    //    public string MODE_OF_NOTIFICATION { get; set; }
    //    public string INCOME_FUNDS_SOURCE { get; set; }
    //    public string PREVIOUS_OCCUPATION { get; set; }
    //    public string PREVIOUS_EMPLOYER { get; set; }
    //    public string CURRENT_OCCUPATION { get; set; }
    //    public string CURRENT_EMPLOYER { get; set; }
    //    public string CURRENT_EMPLOYER_ADD { get; set; }
    //    public string EMPLOYMENT_FROM_DATE { get; set; }
    //    public string EMPLOYMENT_TO_DATE { get; set; }
    //    public string TOT_EMPLOYMENT_YEAR { get; set; }
    //    public string APPROX_ANNUAL_INCOME { get; set; }
    //    public string NET_WORTH { get; set; }
    //    public string INVESTMENT_HORIZON { get; set; }
    //    public string OBJECTIVE { get; set; }
    //    public string INVESTMENT_KNOWLEDGE { get; set; }
    //    public string RISK_TOLERANCE { get; set; }
    //    public string ONLINE_TRD_FACILITY { get; set; }
    //    public string ACCT_STATMENT_FREQ { get; set; }
    //    public string COURT_CONVICTION { get; set; }
    //    public string COURT_CONV_DET { get; set; }
    //    public string NOMINEE_TRUST { get; set; }
    //    public string NOMINEE_TRUST_NAME { get; set; }
    //    public string DECLARATION { get; set; }
    //    public string SIGNATORIES_NUM { get; set; }
    //    public string EMPLOYMENT_STATUS { get; set; }
    //    public string BRANCH_CODE { get; set; }
    //    public string LOCAL_FOREIGN { get; set; }
    //    public string ACCOUNT_TYPE { get; set; }

    //    public string MODE_OF_APP { get; set; }
    //    public string RESIDENTIAL_GHANIAN { get; set; }
    //    public string RESIDENTIAL_FORIEGNER { get; set; }
    //    public string COUNTRY_OF_ORIGIN { get; set; }
    //    public string RESIDENT_PERMIT_NO { get; set; }
    //    public string PERMIT_ISSUE_DATE { get; set; }
    //    public string PERMIT_EXPIRE_DATE { get; set; }
    //    public string PLACE_OF_ISSUE { get; set; }
    //    public string INVESTMENT_OBJECTIVE { get; set; }
    //    public string INIT_INV_AMOUNT { get; set; }
    //    public string TOP_UPS_ACTIVITY { get; set; }
    //    public string REG_WITHDRAWAL_ACTIVITY { get; set; }
    //    public string OTHER_TOP_UPS_ACTIVITY { get; set; }
    //    public string OTHER_REG_WITHDRAWAL_ACTIVITY { get; set; }
    //    public string TOP_UPS_AMOUNT { get; set; }
    //    public string REG_WITHDRAWAL_AMOUNT { get; set; }
    //    public string NOM_MAIDEN_NAME { get; set; }
    //    public string NOM_DATE_OF_BIRTH { get; set; }
    //    public string NOM_GENDER { get; set; }
    //    public string NOM_COUNTRY_OF_ORIGIN { get; set; }
    //    public string NOM_COUNTRY_OF_RESIDENCE { get; set; }
    //    public string NOM_ID_TYPES { get; set; }
    //    public string NOM_ID_ISSUE_DATE { get; set; }
    //    public string NOM_PLACE_OF_ISSUE { get; set; }
    //    public string NOM_ID_EXPIRE_DATE { get; set; }
    //}



    public class SoftTechCreateAccountModel_V2
    {
        public string INVESTMENT_TYPE { get; set; }
        public string EXPECTED_ACCOUNT_ACTIVITY { get; set; }
        public string TIN_NUM { get; set; }
        public string AUTH_TITLE { get; set; }
        public string SUR_NAME { get; set; }
        public string FIRST_NAME { get; set; }
        public string OTHER_NAME { get; set; }
        public string GENDER { get; set; }
        public string NATIONALITY { get; set; }
        public string DATE_OF_BIRTH { get; set; }
        public string MARITIAL_STATUS { get; set; }
        public string MOTHER_MAIDEN_NAME { get; set; }

        public string MAIDEN_NAME { get; set; }
        public string MAILING_ADDRESS { get; set; }
        public string MAILING_CITY { get; set; }
        public string MAILING_ZIP_CODE { get; set; }
        public string MAILING_COUNTRY { get; set; }
        public string RESIDENTIAL_ADDRESS { get; set; }
        public string RESIDENTIAL_CITY { get; set; }
        public string RESIDENTIAL_ZIP_CODE { get; set; }
        public string RESIDENTIAL_COUNTRY { get; set; }
        public string APPLICABLE_SUFFIX { get; set; }
        public string TEL_NUM { get; set; }
        public string MOBILE_NUM { get; set; }
        public string FAX_NUM { get; set; }
        public string OCCUPATION { get; set; }
        public string EMAIL { get; set; }
        public string ID_TYPE { get; set; }
        public string ID_NUMBER { get; set; }
        public string ID_ISSUE_AUTHORITY { get; set; }
        public string ID_ISSUE_DATE { get; set; }
        public string ID_EXPIRY_DATE { get; set; }
        public string ID_UPLOAD_IMAGE_ID { get; set; }
        public string SIGNATURE_IMAGE_ID { get; set; }
        public string FIRST_KIN_NAME { get; set; }
        public string FIRST_KIN_TEL_NUM { get; set; }
        public string FIRST_KIN_MOBILE_NUM { get; set; }
        public string FIRST_KIN_FAX_NUM { get; set; }
        public string FIRST_KIN_RELATIONSHIP { get; set; }
        public string FIRST_KIN_EMAIL { get; set; }
        public string FIRST_KIN_MAILING_ADDRESS { get; set; }
        public string SECOND_KIN_NAME { get; set; }
        public string SECOND_KIN_TEL_NUM { get; set; }
        public string SECOND_KIN_MOBILE_NUM { get; set; }
        public string SECOND_KIN_FAX_NUM { get; set; }
        public string SECOND_KIN_RELATION { get; set; }
        public string SECOND_KIN_EMAIL { get; set; }
        public string SECOND_KIN_MAILING_ADDRESS { get; set; }
        public string AUTH_PERSON_NAME { get; set; }


        public string AUTH_PERSON_TEL_NUM { get; set; }
        public string AUTH_PERSON_MOB_NUM { get; set; }
        public string AUTH_PERSON_FAX { get; set; }
        public string AUTH_PERSON_RELATION { get; set; }
        public string AUTH_PERSON_EMAIL { get; set; }
        public string AUTH_PERSON_MAILING_ADD { get; set; }
        public string AUTH_PERSON_CITY { get; set; }
        public string AUTH_PERSON_ZIP { get; set; }
        public string AUTH_PERSON_COUNTRY { get; set; }
        public string AUTH_PERSON_NIC_PHOTO_ID { get; set; }
        public string AUTH_PERSON_NIC_ISSUE_ATHORITY { get; set; }
        public string AUTH_PERSON_NIC_ISSUE_DATE { get; set; }
        public string AUTH_PERSON_NIC_EXPIRY_DATE { get; set; }
        public string AUTH_PERSON_NIC_IMAGE_UPLOD_ID { get; set; }
        public string AUTH_PERSON_NIC_NUM { get; set; }
        public string COMM_BANK_ACCT_NAME { get; set; }
        public string COMM_BANK_ACCOT_NO { get; set; }
        public string COMM_BANK_SELECTION { get; set; }
        public string BANK_BRANCH { get; set; }
        public string SWIFT_SORT_CODE { get; set; }
        public string MODE_OF_INSTRUCTIONS { get; set; }
        public string MODE_OF_NOTIFICATION { get; set; }
        public string INCOME_FUNDS_SOURCE { get; set; }
        public string PREVIOUS_OCCUPATION { get; set; }
        public string PREVIOUS_EMPLOYER { get; set; }
        public string CURRENT_OCCUPATION { get; set; }
        public string CURRENT_EMPLOYER { get; set; }
        public string CURRENT_EMPLOYER_ADD { get; set; }
        public string EMPLOYMENT_FROM_DATE { get; set; }
        public string EMPLOYMENT_TO_DATE { get; set; }
        public string TOT_EMPLOYMENT_YEAR { get; set; }
        public string APPROX_ANNUAL_INCOME { get; set; }
        public string NET_WORTH { get; set; }
        public string INVESTMENT_HORIZON { get; set; }
        public string OBJECTIVE { get; set; }
        public string INVESTMENT_KNOWLEDGE { get; set; }
        public string RISK_TOLERANCE { get; set; }
        public string ONLINE_TRD_FACILITY { get; set; }
        public string ACCT_STATMENT_FREQ { get; set; }
        public string COURT_CONVICTION { get; set; }
        public string COURT_CONV_DET { get; set; }
        public string NOMINEE_TRUST { get; set; }
        public string NOMINEE_TRUST_NAME { get; set; }
        public string DECLARATION { get; set; }
        public string SIGNATORIES_NUM { get; set; }
        public string EMPLOYMENT_STATUS { get; set; }
        public string BRANCH_CODE { get; set; }
        public string LOCAL_FOREIGN { get; set; }
        public string ACCOUNT_TYPE { get; set; }
        public string OTHER_TOP_UPS_ACTIVITY { get; set; }
        public string TOP_UPS_ACTIVITY { get; set; }


        public string PLACE_OF_BIRTH { get; set; }
        public string AUTH_PERSON_Photo_ID_TYPE { get; set; }
        public string REFERENCE { get; set; }
        public string APPROVED_BY { get; set; }
        public string MAIN_CLIENT_NAME { get; set; }
        public string MODE_OF_APPLICATION { get; set; }
        public string INITIAL_INVESTMENT_AMOUNT { get; set; }
        public string EMAIL_2 { get; set; }

        public string SECOND_KIN_FAX { get; set; }
        public string AUTH_PERSON_ZIP_CODE { get; set; }
        public string MODE_OF_APP { get; set; }
        public string RESIDENTIAL_GHANIAN { get; set; }
        public string RESIDENTIAL_FORIEGNER { get; set; }
        public string COUNTRY_OF_ORIGIN { get; set; }
        public string RESIDENT_PERMIT_NO { get; set; }
        public string PERMIT_ISSUE_DATE { get; set; }
        public string PERMIT_EXPIRE_DATE { get; set; }
        public string PLACE_OF_ISSUE { get; set; }
        public string INVESTMENT_OBJECTIVE { get; set; }
        public int INIT_INV_AMOUNT { get; set; }
        //public string TOP_UPS_ACTIVITY { get; set; }
        public string REG_WITHDRAWAL_ACTIVITY { get; set; }
        public string OTHER_REG_WITHDRAWAL_ACTIVITY { get; set; }
        public string TOP_UPS_AMOUNT { get; set; }
        public string REG_WITHDRAWAL_AMOUNT { get; set; }
        public string NOM_MAIDEN_NAME { get; set; }
        public string NOM_DATE_OF_BIRTH { get; set; }
       // public string NOM_MAIDEN_NAME { get; set; }
       // public string NOM_DATE_OF_BIRTH { get; set; }

        public string NOM_PLACE_OF_BIRTH { get; set; }

        public string NOM_GENDER { get; set; }

        public string NOM_COUNTRY_OF_ORIGIN { get; set; }
        public string NOM_COUNTRY_OF_RESIDENCE { get; set; }
        public string NOM_ID_TYPES { get; set; }
        public string NOM_ID_ISSUE_DATE { get; set; }
        public string NOM_PLACE_OF_ISSUE { get; set; }
        public string NOM_ID_EXPIRE_DATE { get; set; }



        public string JOINT_NAME { get; set; }

        public string JOINT_OTHER_NAME { get; set; }

        public string JOINT_NATIONALITY { get; set; }

        public string JOINT_DATE_OF_BIRTH { get; set; }

        public string JOINT_TELEPHONE { get; set; }

        public string JOINT_MOBILE { get; set; }

        public string JOINT_FAX { get; set; }

        public string JOINT_EMAIL { get; set; }

        public string JOINT_OCCUPATION { get; set; }

        public string JOINT_ID_TYPE { get; set; }

        public string JOINT_ID_NUMBER { get; set; }

        public string JOINT_ISSUE_DATE { get; set; }

        public string JOINT_ISSUE_AUTHORITY { get; set; }

        public string JOINT_EXPIRY_DATE { get; set; }

        public string SECOND_AUTH_PERSON_NAME { get; set; }

        public string SECOND_AUTH_PERSON_TEL_NUM { get; set; }

        public string SECOND_AUTH_PERSON_MOB_NUM { get; set; }

        public string SECOND_AUTH_PERSON_FAX { get; set; }

        public string SECOND_AUTH_PERSON_RELATION { get; set; }

        public string SECOND_AUTH_PERSON_EMAIL { get; set; }

        public string SECOND_AUTH_PERSON_MAILING_ADD { get; set; }

        public string SECOND_AUTH_PERSON_CITY { get; set; }

        public string SECOND_AUTH_PERSON_ZIP_CODE { get; set; }

        public string SECOND_AUTH_PERSON_COUNTRY { get; set; }

        public string SECOND_AUTH_PERSON_NIC_PHOTO_ID { get; set; }

        public string SECOND_AUTH_PERSON_NIC_ISSUE_ATHORITY { get; set; }

        public string SECOND_AUTH_PERSON_NIC_ISSUE_DATE { get; set; }

        public string SECOND_AUTH_PERSON_NIC_EXPIRY_DATE { get; set; }

        public string SECOND_AUTH_PERSON_NIC_IMAGE_UPLOD_ID { get; set; }

        public string SECOND_AUTH_PERSON_NIC_NUM { get; set; }

        public string SECOND_AUTH_TITLE { get; set; }

        public string THIRD_AUTH_PERSON_NAME { get; set; }

        public string THIRD_AUTH_PERSON_TEL_NUM { get; set; }

        public string THIRD_AUTH_PERSON_MOB_NUM { get; set; }

        public string THIRD_AUTH_PERSON_FAX { get; set; }

        public string THIRD_AUTH_PERSON_RELATION { get; set; }

        public string THIRD_AUTH_PERSON_EMAIL { get; set; }

        public string THIRD_AUTH_PERSON_MAILING_ADD { get; set; }

        public string THIRD_AUTH_PERSON_CITY { get; set; }

        public string THIRD_AUTH_PERSON_ZIP_CODE { get; set; }

        public string THIRD_AUTH_PERSON_COUNTRY { get; set; }

        public string THIRD_AUTH_PERSON_NIC_PHOTO_ID { get; set; }

        public string THIRD_AUTH_PERSON_NIC_ISSUE_ATHORITY { get; set; }

        public string THIRD_AUTH_PERSON_NIC_ISSUE_DATE { get; set; }

        public string THIRD_AUTH_PERSON_NIC_EXPIRY_DATE { get; set; }

        public string THIRD_AUTH_PERSON_NIC_IMAGE_UPLOD_ID { get; set; }

        public string THIRD_AUTH_PERSON_NIC_NUM { get; set; }

        public string THIRD_AUTH_TITLE { get; set; }

        public string FOURTH_AUTH_PERSON_NAME { get; set; }

        public string FOURTH_AUTH_PERSON_TEL_NUM { get; set; }

        public string FOURTH_AUTH_PERSON_MOB_NUM { get; set; }

        public string FOURTH_AUTH_PERSON_FAX { get; set; }

        public string FOURTH_AUTH_PERSON_RELATION { get; set; }

        public string FOURTH_AUTH_PERSON_EMAIL { get; set; }

        public string FOURTH_AUTH_PERSON_MAILING_ADD { get; set; }

        public string FOURTH_AUTH_PERSON_CITY { get; set; }

        public string FOURTH_AUTH_PERSON_ZIP_CODE { get; set; }

        public string FOURTH_AUTH_PERSON_COUNTRY { get; set; }

        public string FOURTH_AUTH_PERSON_NIC_PHOTO_ID { get; set; }

        public string FOURTH_AUTH_PERSON_NIC_ISSUE_ATHORITY { get; set; }

        public string FOURTH_AUTH_PERSON_NIC_ISSUE_DATE { get; set; }

        public string FOURTH_AUTH_PERSON_NIC_EXPIRY_DATE { get; set; }

        public string FOURTH_AUTH_PERSON_NIC_IMAGE_UPLOD_ID { get; set; }

        public string FOURTH_AUTH_PERSON_NIC_NUM { get; set; }

        public string FOURTH_AUTH_TITLE { get; set; }

        public string FIFTH_AUTH_PERSON_NAME { get; set; }

        public string FIFTH_AUTH_PERSON_TEL_NUM { get; set; }

        public string FIFTH_AUTH_PERSON_MOB_NUM { get; set; }

        public string FIFTH_AUTH_PERSON_FAX { get; set; }

        public string FIFTH_AUTH_PERSON_RELATION { get; set; }

        public string FIFTH_AUTH_PERSON_EMAIL { get; set; }

        public string FIFTH_AUTH_PERSON_MAILING_ADD { get; set; }

        public string FIFTH_AUTH_PERSON_CITY { get; set; }

        public string FIFTH_AUTH_PERSON_ZIP_CODE { get; set; }

        public string FIFTH_AUTH_PERSON_COUNTRY { get; set; }

        public string FIFTH_AUTH_PERSON_NIC_PHOTO_ID { get; set; }

        public string FIFTH_AUTH_PERSON_NIC_ISSUE_ATHORITY { get; set; }

        public string FIFTH_AUTH_PERSON_NIC_ISSUE_DATE { get; set; }

        public string FIFTH_AUTH_PERSON_NIC_EXPIRY_DATE { get; set; }

        public string FIFTH_AUTH_PERSON_NIC_IMAGE_UPLOD_ID { get; set; }

        public string FIFTH_AUTH_PERSON_NIC_NUM { get; set; }

        public string FIFTH_AUTH_TITLE { get; set; }
    }

    public class SoftTechCreateAccountResponseModel
    {
        public object[] invalidFields { get; set; }
        public object[] missingFields { get; set; }
        public string responseCode { get; set; }
        public string desc { get; set; }
        public string ErrorList { get; set; }
        public string error { get; set; }
        public string path { get; set; }
    }

}
