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

    public class BorrowerDetailscs
    {
      public int Id { get; set; }

      [Required(ErrorMessage = "Please Enter PanCard Number.")]
      public string Pan { get; set; }

           [Required(ErrorMessage = "Please Enter First Name.")]
     public string FirstName { get; set; }
          
         public DateTime CreateDate { get; set; }
       public int  CreateId { get; set; }

           [Required(ErrorMessage = "Please Enter Middle Name.")]
        public string MiddleName { get; set; }

           [Required(ErrorMessage = "Please Enter Last Name.")]
        public string LastName { get; set; }

           [Required(ErrorMessage = "Please Select Gender.")]
      public string Sex { get; set; }

               [Required(ErrorMessage = "Please Select Birth Date")]
      [DataType(DataType.Date)]
      [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
      public DateTime DateOfBirth { get; set; }

       public string  IsNameChange { get; set; }
       public int TitleId { get; set; }
       public string Name { get; set; }

           [Required(ErrorMessage = "Please Select Title.")]
       public decimal Name1 { get; set; }

           [Required(ErrorMessage = "Please Select Nationality.")]
       public decimal Nationality1 { get; set; }

        public int NationalityId { get; set; }
        public string Nationality { get; set; }
        public List<SelectListItem> state { get; set; }
        public List<SelectListItem> CountryItem { get; set; }
        //public List<SelectListItem> Nationality { get; set; }
        //public string Name { get; set; }

           [Required(ErrorMessage = "Please Enter Address.")]
        public string Address1 { get; set; }

        public string Address2 { get; set; }
        public string Address3 { get; set; }

           [Required(ErrorMessage = "Please Select Country.")]
        public int CountryId { get; set; }
        public int StateId { get; set; }

           [Required(ErrorMessage = "Please Select State.")]
        public decimal State1 { get; set; }

        public decimal Country { get; set; }

           [Required(ErrorMessage = "Please Enter City.")]
        public string City { get; set; }

           [Required(ErrorMessage = "Please Enter Pincode.")]
           [MinLength(6, ErrorMessage = "Pincode Minimum Length Should Be 6 Char")]
        public string Pincode { get; set; }


        public string LandlineCode { get; set; }

        [MinLength(10, ErrorMessage = "Contact No Minimum Length Should Be 10 Char")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Enter valid Contact number")]        
        public string LandlineNo { get; set; }

           [Required(ErrorMessage = "Please Enter Code")]
        public string MobileCode { get; set; }

           [Required(ErrorMessage = "Please Enter Mobile Number.")]
           [MinLength(10, ErrorMessage = "Contact No Minimum Length Should Be 10 Char")]
           [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Enter valid Contact number")]        
        public string MobileNo { get; set; }

           [Required(ErrorMessage = "Please Enter Email ID")]
           [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Invalid email")]
        public string EmailId1 { get; set; }

          [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Invalid email")]
        public string EmailId2 { get; set; }



          [Required(ErrorMessage = "Please Enter New First Name.")]
          public string FirstName1 { get; set; }

          [Required(ErrorMessage = "Please Enter New Middle Name.")]
          public string MiddleName1 { get; set; }

          [Required(ErrorMessage = "Please Enter New Last Name.")]
          public string LastName1 { get; set; }


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
        public class IndividualCustomerMaster
        {
            public int Id { get; set; }
            public string FirstName { get; set; }
            public string MiddleName { get; set; }
            public string LastName { get; set; }
            public string PAN { get; set; }

        }
        public class NameTitleMaster
        {
            public int NId { get; set; }
            public string Name { get; set; }

            public DateTime CreateDate { get; set; }
            public DateTime EditDate { get; set; }
        }

        public class NationalityMaster
        {
            public int Id { get; set; }
            public string Nationality { get; set; }
        }
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
}
