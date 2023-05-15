using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CreditRatingModel.Model
{
    public class ContactDetailsMasters
    {
        [Key]
        public decimal Id { get; set; }

     //   [MaxLength(50)]
        public string PAN { get; set; }

        public int IndividualCustomerId { get; set; }

    //    [Required(ErrorMessage = "Please Enter Address.")]
   //     [MaxLength(1000)]
        public string Address1Cont { get; set; }
    //    [MaxLength(1000)]
        public string Address2Cont { get; set; }
      //  [MaxLength(1000)]
        public string Address3Cont { get; set; }

      //  [Required(ErrorMessage = "Please Select Country.")]
        public decimal CountryId { get; set; }

  //      [Required(ErrorMessage = "Please Select State.")]
        public decimal StateId { get; set; }
        public string StateName { get; set; }

   //     [Required(ErrorMessage = "Please Enter City.")]
     //   [MaxLength(1000)]
        public decimal CityId { get; set; }
        public string City { get; set; }

     //   [Required(ErrorMessage = "Please Enter Pin.")]
        //[MaxLength(20)]
        public string Pincode { get; set; }
        
     //   [MaxLength(3)]
        public string LandlineCode { get; set; }

        //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Enter valid Landline Number.")]
       //[MaxLength(8)]
        public string LandlineNo { get; set; }

       // [MaxLength(3)]
        public string MobileCode { get; set; }
        
        //[MaxLength(10)]
        //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Enter valid Mobile Number.")]
        public string MobileNo { get; set; }

       // [Required(ErrorMessage = "Please Enter Email Id.")]
      //  [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Enter valid Email Id.")]
        //[MaxLength(1000)]
        public string EmailId1 { get; set; }
        
        //[MaxLength(1000)]
      //  [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Enter valid Email Id.")]
        public string EmailId2 { get; set; }

        public int CreateId { get; set; }
        public int EditId { get; set; }

        public Nullable<DateTime> CreateDate { get; set; }
        public Nullable<DateTime> EditDate { get; set; }

        public int BorrowerBranchId { get; set; }

      //  [Required(ErrorMessage = "Please Select Country Name.")]                
        public decimal ContCountryId { get; set; }

    //    [Required(ErrorMessage = "Select Address Type.")]
        public decimal AddressTypeId { get; set; }
        public decimal ContRelId { get; set; }
    }  
}
