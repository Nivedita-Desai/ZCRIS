using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CreditRatingModel.Model
{
    public class CollegeMaster_Metadata
    {
        public int id { get; set; }
        public string CollegeName { get; set; }
        public string ColAddress { get; set; }
        public string Address1 { get; set; }  
        public string Address2 { get; set; } 
        public string Address3 { get; set; }
        public decimal CountryId { get; set; }
        //public int CourseId { get; set; }
        public decimal stateId { get; set; }
        public string city { get; set; }
        public decimal cityID { get; set; }
        public string pincode { get; set; }
        public string ContactNo { get; set; }
        public string EmailId { get; set; }
        public string CEmailId { get; set; }
        public string website { get; set; }
        public string ContactPersonName { get; set; }
        public string ContactPersonEmail { get; set; }
        public string ContactPersonMobile { get; set; }
        public string ConPersonDesignation { get; set; }

        public string code { get; set; }
        //[Required(ErrorMessage = "Select Course")]
        //public List<int> CourseListId { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime EditDate { get; set; }
        public int CreateId { get; set; }
        public int EditId { get; set; }

        //-----CourseMaster-----//
        public decimal StreamField { get; set; }
        //public decimal StreamId1 { get; set; }
        public decimal CategoryId { get; set; }

        public string CourseName { get; set; }
        public string CourseType  { get; set; }
        public int duration { get; set; }
        public string durationperiod { get; set; }
        public string month { get; set; }
        public bool Morethan1yr { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<DateTime> Startdate { get; set; }

        public bool status { get; set; }




    }
}
