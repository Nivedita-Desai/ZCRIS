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
   public  class BorrowerDetails
   {
       //--------Akshata--------//
       //---typeselection---/// 
       public string borrowerType { get; set; }
       public string borrowerTypeId { get; set; }
      
       //--add BOD----///
       public Nullable<DateTime> DOJ { get; set; }
       public int Companyid { get; set; }
       public string FULLANME{ get; set; }
       public string PANinv { get; set; }

       //----file upload 030915----//
       public string fullname { get; set; }
       public string Cname { get; set; }
       public string FilePath { get; set; }
       public Nullable<DateTime> IssueDate { get; set; }
       public Nullable<DateTime> Expirydate { get; set; }
       public string docid { get; set; }
       public int DOC { get; set; }
       //-----file upload finished----//

       //-----personal details---//
       public string USERNAME { get; set; }
       public int id { get; set; }
   
       public List<SelectListItem> AddressType { get; set; }
       public int Addressid { get; set; }
       public List<SelectListItem> country { get; set; }
       public List<SelectListItem> document { get; set; }

       public string CompanyName1 { get; set; }
       public string PAN1 { get; set; }
       public string Designation { get; set; }

       public string ComPAN { get; set; }
       public string CompanyName { get; set; }
       public string CompanyType { get; set; }
       public string BranchName { get; set; }
       public Nullable<DateTime> IncorporationDate { get; set; }
       public Nullable<DateTime> CommencementDate { get; set; }

       public string FirstName { get; set; }
       public string MiddleName { get; set; }
       public string LastName { get; set; }
       public string Sex { get; set; }
       public List<SelectListItem> Name { get; set; }
       public int NID { get; set; }
       public Nullable<DateTime> DateOfBirth { get; set; }
       public List<SelectListItem> Nationality { get; set; }
       public int NationalityId { get; set; }

       public string PAN { get; set; }
       //-----personal details finished----//


       //------contactdetails-------//
       public string Addtype { get; set; }
       public string Address1 { get; set; }
       public string Address2 { get; set; }
       public string Address3 { get; set; }
       public int CountryId { get; set; }
       public int StateId { get; set; }
       public string State { get; set; }
       public string City { get; set; }
       public string Pincode { get; set; }
        public string LandlineCode { get; set; }
       public string LandlineNo { get; set; }
       public string MobileCode { get; set; }
       public string MobileNo { get; set; }  
       public string EmailId1 { get; set; }
       public string EmailId2 { get; set; }


       //----contactdetails finished----//

       //--------Akshata finished-------//


       //-------Darshana------
        public virtual BorrowerDetailscs borrowercustomer { get; set; }

       //---------------------

       //----------------------------------

       //--------------Rutuja-------------
        [DataType(DataType.Date)]
        public DateTime? EditIncorporationDate { get; set; }

       //-------------------------------------------

        public class NameTitleMaster
        {
            public int NId { get; set; }
            public string Name { get; set; }

            public DateTime CreateDate { get; set; }
            public DateTime EditDate { get; set; }
        }
       
        public class NationalityMaster

        {    public int Id { get; set; }
        public string Nationality { get; set; }
   
            
        }

     

        public class BorrowerCompanyBranchMaster
        {
           
            public int Id { get; set; }
            public string BranchName { get; set; }
              public int CompanyTypeId { get; set; }
              public DateTime CreateDate { get; set; }
              public DateTime  EditDate { get; set; }
              public int CreateId { get; set; }
              public int EditId { get; set; }
              public virtual BorrowerCompanyTypeMasters BorrowerCompanyTypeMasters { get; set; }
   
        }
        public class BorrowerCompanyTypeMasters
        {  public int Id { get; set; }
             public string CompanyType { get; set; }
             public int CompanyTypeId { get; set; }
            public DateTime CreateDate { get; set; }
            public DateTime EditDate { get; set; }
            public int CreateId { get; set; }
            public int EditId { get; set; }
            public virtual BorrowerCompanyMasters BorrowerCompanyMasters { get; set; }
   
        }
        public class BorrowerCompanyMasters
        {
            public int Id { get; set; }
            public string CompanyName { get; set; }
            public string PAN { get; set; }
           
            public DateTime CreateDate { get; set; }
            public DateTime EditDate { get; set; }
            public int CreateId { get; set; }
            public int EditId { get; set; }

            //--------------Rutuja-------------
            public Nullable<DateTime> IncorporationDate { get; set; }
            public Nullable<DateTime> CommencementDate { get; set; }

            //----------------------------------
        }
        public class IndividualCustomerMaster
        {
            public int Id { get; set; }
            public string FirstName { get; set; }
            public string MiddleName { get; set; }
            public string LastName { get; set; }
            public string PAN { get; set; }
           
        }
        public class BorrowerDetailsMaster
        {
            public int Id { get; set; }
            public string CompanyName { get; set; }
            public string BorrowerType { get; set; }
            public int BranchMasterId { get; set; }



        }
        public class AddressTypeMaster
        {
            public int id { get; set; }
            public string AddressType { get; set; }
            
        }
        public class ContactDetailsMaster
        { 
          public string Address1 { get; set; }
          public string Address2 { get; set; }
            public string Address3 { get; set; }
            public int CountryId { get; set; }
            public int StateId { get; set; }
           public string City { get; set; }
            public string Pincode { get; set; }
            public string LandlineCode { get; set; }
            public string LandlineNo { get; set; }
            public string MobileCode { get; set; }
            public string MobileNo { get; set; }
            public string EmailId1 { get; set; }
            public string EmailId2 { get; set; }
            public DateTime CreateDate { get; set; }
            public DateTime EditDate { get; set; }
            public int CreateId { get; set; }
            public int EditId { get; set; }
        }

       
     //-------------Rutuja-------------------
        public class BorrowGrid
        {
            public int ID { get; set; }

            [Required(ErrorMessage = "Enter Sub Division Name.")]
            [MaxLength(1000)]
            public string CompanyType { get; set; }

            [Required(ErrorMessage = "Enter Company Name.")]
            [MaxLength(1000)]
            public string CompanyName { get; set; }

            [Required(ErrorMessage = "Enter Branch Name.")]
            [MaxLength(1000)]
            public string BranchName { get; set; }

            [Required(ErrorMessage = "Enter Incorporation Date.")]
            [DataType(DataType.Date)]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
            public DateTime? IncorporationDate { get; set; }

            [Required(ErrorMessage = "Enter Commencement Date.")]
            [DataType(DataType.Date)]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
            public DateTime CommencementDate { get; set; }

            public string UserName { get; set; }
        }
       //-------------------------------

       //--------------------Darshana---------------

        public class BorrowerDetailscs
        {
            public int Id { get; set; }

            [Required(ErrorMessage = "First Name is required.")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "Pan Number is required.")]
            public string Pan { get; set; }

            public DateTime CreateDate { get; set; }
            public int CreateId { get; set; }

            [Required(ErrorMessage = "Middle Name is required.")]
            public string MiddleName { get; set; }

            [Required(ErrorMessage = "Last Name is required.")]
            public string LastName { get; set; }

            [Required(ErrorMessage = "Select Gender.")]
            public string Sex { get; set; }

            [Required(ErrorMessage = "Birth Date is required.")]
            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
            public DateTime DateOfBirth { get; set; }

            public string IsNameChange { get; set; }
            public int TitleId { get; set; }
            public string Name { get; set; }

            [Required(ErrorMessage = "Select Title.")]
            public decimal Name1 { get; set; }

            [Required(ErrorMessage = "Select Nationality.")]
            public decimal Nationality1 { get; set; }

            public int NationalityId { get; set; }
            public string Nationality { get; set; }
            public List<SelectListItem> state { get; set; }
            public List<SelectListItem> CountryItem { get; set; }
            //public List<SelectListItem> Nationality { get; set; }
            //public string Name { get; set; }

            [Required(ErrorMessage = "Address is required.")]
            public string Address1 { get; set; }

            public string Address2 { get; set; }
            public string Address3 { get; set; }

            [Required(ErrorMessage = "Country is required.")]
            public int CountryId { get; set; }
            public int StateId { get; set; }

            [Required(ErrorMessage = "State is required.")]
            public decimal State1 { get; set; }

            public decimal Country { get; set; }

            [Required(ErrorMessage = "City is required.")]
            public string City { get; set; }

            [Required(ErrorMessage = "Pincode is required.")]
            [MinLength(6, ErrorMessage = "Pincode Minimum Length Should Be 6 Char")]
            public string Pincode { get; set; }


            public string LandlineCode { get; set; }

            [MinLength(10, ErrorMessage = "Contact No Minimum Length Should Be 10 Char")]
            [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Enter valid Contact number")]
            public string LandlineNo { get; set; }

            [Required(ErrorMessage = "Code is required.")]
            public string MobileCode { get; set; }

            [Required(ErrorMessage = "Mobile Number is required.")]
            [MinLength(10, ErrorMessage = "Contact No Minimum Length Should Be 10 Char")]
            [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Enter valid Contact number")]
            public string MobileNo { get; set; }

            [Required(ErrorMessage = "Email ID is required.")]
            [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Invalid email")]
            public string EmailId1 { get; set; }

            [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Invalid email")]
            public string EmailId2 { get; set; }

            [Required(ErrorMessage = "New First Name is required.")]
            public string FirstName1 { get; set; }

            [Required(ErrorMessage = "New Middle Name is required.")]
            public string MiddleName1 { get; set; }

            [Required(ErrorMessage = "New Last Name is required.")]
            public string LastName1 { get; set; }


            public class IndividualCustomerLog
            {
                public int Id { get; set; }
                public string PAN { get; set; }
                public string OldFirstName { get; set; }
                public string OldMiddleName { get; set; }
                public string OldLastName { get; set; }
                public string NewFirstName { get; set; }
                public string NewMiddleName { get; set; }
                public string NewLastName { get; set; }

            }
        }
       //-------------------------------------------
                      

    }
}
