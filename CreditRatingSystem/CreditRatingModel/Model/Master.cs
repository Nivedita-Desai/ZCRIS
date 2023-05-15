using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace CreditRatingModel.Model
{
 public  class Master
    {
        
        public decimal Id { get; set; }
        public decimal Id1 { get; set; }
     //----------LoanTypesMaster-----------------
     public decimal CreditTypeId { get; set; }
     public string LoanType { get; set; }
     public string Category { get; set; }

     //-----------CountryMaster---------------
     public string CountryName { get; set; }
     public string CountryCode { get; set; }

     //----------StateMaster-----------------
     public int CountryId { get; set; }
     public decimal CountryId1 { get; set; }
     public string StateName { get; set; }

     //----------LoanTypesMaster-----------------
     public int StateId { get; set; }
     public decimal StateId1 { get; set; }
     public string CityName { get; set; }
     public string CityCode { get; set; }

        //-----------DesignationMaster---------------
     public string DesignationName { get; set; }
     public string DesignationType { get; set; }

        //-----------DepartmentMaster---------------
     public string DepartmentName { get; set; }


        //-----------StreamMaster---------------
     public string StreamName { get; set; }

     //-----------CourseCategory---------------
     public string CourseCategory { get; set; }


     //-----------AcademicYear--------------
     [DataType(DataType.Date)]     
     [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
     public DateTime AyFromDate {get; set; }

     [DataType(DataType.Date)]
     [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
     public DateTime AyToDate {get; set; }

     public string AyId { get; set; }
     //-------Realtionship Master---//
     public string relation { get; set; }

     //----Nationality--//
     public string nationality { get; set; }

        //-----Slab Master------//

     public decimal Rangefrom { get; set; }
     public decimal RangeTo { get; set; }
     public decimal SlabPercentage { get; set; }

     //-----Threshold Master------//
     public decimal ThresholdAmt { get; set; }

     //----------FinancialInstitutionCategoryMaster------//

     public string CategoryName { get; set; }
   



   //-------distributiontypemaster----//

    
     public string Name { get; set; }
     public string  ShortName { get; set; }
     public string CoPartner { get; set; }

        //------DISAGREEMENTYEAR--------//

    
     public string Naration { get; set; }
     [DataType(DataType.Date)]
     //[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
     [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
     public Nullable<DateTime> EffectiveDate { get; set; }

        //-------AccountTypeMaster--------//

     public string AccountType { get; set; }

        //------------DisAgreementDetails--------//

     public int DistributionTypeId { get; set; }
     public int DisAgreementYearId { get; set; }
     public int Percentage { get; set; }


        //------TransactionMaster--------//

     public string  TransactionType { get; set; }
     public string points { get; set; }

        //------RatingRangeMaster--------//

     public int TO_VALUE { get; set; }
     public int FROM_VALUE { get; set; }
     public string RATING_STATUS { get; set; }

        //--------------AreaMaster--------------//


     public decimal CityId { get; set; }
     public string AreaName { get; set; }

        //---------UserTypeMaster--------------//
     public string UserType { get; set; }
     public string UserTypeShortName { get; set; }


     //-----------Role---------------------//
     public string Role { get; set; }
     public string RoleDescription { get; set; }

    //--------NameTitleMaster----------//

     public string TitleName { get; set; }

        //--------PackageMaster----------//
     //public int id { get; set; }
     public string packagecode { get; set; }
     public string packagename { get; set; }
     public double packageamt { get; set; }
     public int packagecredit  { get; set; }


     //---------CardTypeMaster------------//
     public int FinantialInstituteId { get; set; }
     public string CardType { get; set; }
     public int DueDays { get; set; }
     public int Graceperiod { get; set; }
     public decimal CashLimit { get; set; }
     public decimal CreditLimit { get; set; }
     public string FinantialInstituteName { get; set; }

    }
 public class SaveDistributionData
 {
     public string SaveData { get; set; }
 }
}
