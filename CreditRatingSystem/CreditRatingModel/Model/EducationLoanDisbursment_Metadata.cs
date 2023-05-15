using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc.Ajax;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace CreditRatingModel.Model
{
   public class EducationLoanDisbursment_Metadata
    {
       public int Id { get; set; }

    
       public string AutoGenrateNumber { get; set; }
       
       public int AddressChangedCount { get; set; }
       public int DisbursementId { get; set; }
       public int Count { get; set; } 
       public string NId { get; set; }
       public string StudentName { get; set; }
       public int TitleId { get; set; }
       public decimal TitleId1 { get; set; }
       public string FirstName { get; set; }
       public string MiddleName { get; set; }
       public string LastName { get; set; }
       public string StudentAddress { get; set; }
       public string Address1 { get; set; }
       public string Address2 { get; set; }
       public string Address3 { get; set; }

       public int CountryId { get; set; }
       public int StateId { get; set; }
       public int CityId { get; set; }
       public string State { get; set; }
       public decimal CountryId1 { get; set; }
       public decimal StateId1 { get; set; }
       public decimal CityId1 { get; set; }

       public string City { get; set; }
      
       public string Pincode { get; set; }
       [DataType(DataType.Date)]
       [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
       public DateTime DateOfBirth { get; set; }
       public string Sex { get; set; }
       public string code { get; set; }
       public string StudentContactNo { get; set; }
       public string StudentEmailId { get; set; }
       public int CourseId { get; set; }
       public int CollegeId { get; set; }
          public decimal DisburseAmount { get; set; }
       public decimal LoanAmount { get; set; }
       [DataType(DataType.Date)]
       [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
       public DateTime DisburseDate { get; set; }
       public string LoanAccountNo { get; set; }
       [DataType(DataType.Date)]
       [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
       public DateTime LoanAccountDate { get; set; }
       public string AccountStatus { get; set; }
       public int CreateId { get; set; }
       public DateTime CreateDate { get; set; }
      
       public string GarName { get; set; }
       public decimal RId { get; set; }
       public int RelationId { get; set; }
       public decimal RelationShipId { get; set; }
       public string GarNID { get; set; }
       public string  GarContact { get; set; }
       public string code1 { get; set; }
       public decimal NationalityId { get; set; }  
       
       public decimal Fee { get; set; }
       public decimal LoanTypeId { get; set; }
       public decimal BalanceAmount { get; set; }
       public string Status { get; set; }
       public DateTime ClosureDate { get; set; }
       public string Remark { get; set; }
       public int AcaYId { get; set; }
       public int AcademicYeaId { get; set; }
  public decimal AcademicYeaId1 { get; set; }
       public int courseyear { get; set; }
       [DataType(DataType.Date)]
       [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
       public DateTime issuedDate { get; set; }
    
       public string issuedBy { get; set; }
       public decimal DFees { get; set; }
       public string Bid { get; set; }
       public string Brid { get; set; }
       public string BName { get; set; }
       public string BrName { get; set; }
       public string PayMode { get; set; }
       public string PayModeID { get; set; }
       public DateTime TranDate { get; set; }
       public int NewcollageID { get; set; }
       public string TranNo { get; set; }
       public string hfentrystatus { get; set; }

       public string NewChildData { get; set; }

       public string enteystatus { get; set; }
       public int MarriedS { get; set; }
       public int MarritalStatus { get; set; }
       public int MarritalStatusId { get; set; }
       public int NewCollegeId { get; set; }
       public int CollageID { get; set; }
       public int hfeditId { get; set; }



    }

      
       

   public class SaveCollectionData
   {
       public string childData { get; set; }
       public string EduLoanchildData { get; set; }

   }

    
}
