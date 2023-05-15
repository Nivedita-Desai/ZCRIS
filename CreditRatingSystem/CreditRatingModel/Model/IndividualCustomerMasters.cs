using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
//using System.Runtime.Serialization;

namespace CreditRatingModel.Model
{
    public class IndividualCustomerMasters
    {
        public decimal Id { get; set; }

     //   [Required(ErrorMessage = "Enter First Name.")]
      //  [MaxLength(100)]
        public string FirstName { get; set; }

        public string MiddleName { get; set; }
        public string LastName { get; set; }

      //  [Required(ErrorMessage = "Enter Pan Card No.")]
     //   [MaxLength(50)]
        public string Pan { get; set; }

      //  [Required(ErrorMessage = "Please Select Gender.")]
        //[MaxLength(1)]
        public string Sex { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
       // [Required(ErrorMessage = "Enter Birth Date.")]
        public DateTime DateOfBirth { get; set; }

       // [Required(ErrorMessage = "Please Select Title.")]
        public decimal TitleId { get; set; }

       // [Required(ErrorMessage = "Please Select Nationality.")]
        public decimal NationalityId { get; set; }
        
        public int CreateId { get; set; }
        public int EditId { get; set; }

        public Nullable<DateTime> CreateDate { get; set; }
        public Nullable<DateTime> EditDate { get; set; }

        public int MaritalStatusId { get; set; }

    }
}
