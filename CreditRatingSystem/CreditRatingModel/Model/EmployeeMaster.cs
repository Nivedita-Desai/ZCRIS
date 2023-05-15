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

namespace CreditRatingModel.Model
{
   public class EmployeeMaster
    {
       public int Id { get; set; }

        //------contactdetails-------//
        //public int EmpName { get; set; }
       public string NID { get; set; }
       public string NID1 { get; set; }
       public string Title { get; set; }
       public decimal TitleId1 { get; set; }
       public decimal TitleId { get; set; }
       public string Name { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public decimal CountryId { get; set; }
        public decimal StateId { get; set; }
        public decimal CityId1 { get; set; }
        public decimal CityId { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Pincode { get; set; }
       [DataType(DataType.Date)]
       [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }
        public string Sex { get; set; }
        public int EmployerTypeId { get; set; }
        public int EmployerBranchId { get; set; }
        public int EmployerId { get; set; }
       [DataType(DataType.Date)]
       [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateOfJoining { get; set; }
        public decimal Salary { get; set; }

        public decimal DepartmentId { get; set; }
        public decimal DesignationId { get; set; }
        public decimal CurrentDepartmentId { get; set; }
        public decimal CurrentDesignationId { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime EditDate { get; set; }
        public int CreateId { get; set; }
        public int EditId { get; set; }
        //public string LandlineCode { get; set; }
        //public string LandlineNo { get; set; }
        public string MobileCode { get; set; }
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public string LoanNo { get; set; }

        public int CourseId { get; set; }
        public int CollegeId { get; set; }
        public int MaritalId { get; set; }
        public string CourseName { get; set; }
        public string CollegeName { get; set; }

        public decimal LoanAmount { get; set; }
        public decimal DisburseAmount { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DisburseDate { get; set; }
        public string LoanAccountNo { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime LoanAccountDate { get; set; }

        //----contactdetails finished----//
        public int AddressChangedCount { get; set; }
        public string Status { get; set; }
        public string EmployerName { get; set; }
        public string EmployeeID { get; set; }
        public string EmpDivisionName { get; set; }
        public string BranchName { get; set; }
        public string Remark { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime SalEffectiveDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime lastWorkingDate { get; set; }
        public decimal MonthlyEMI{ get; set; }
        public decimal SlabId { get; set; }
        public decimal TotalLoanAmount { get; set; }
        public decimal TotalBalanceAmount { get; set; }
    }
}
