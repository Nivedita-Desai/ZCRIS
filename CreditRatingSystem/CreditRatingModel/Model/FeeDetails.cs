using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CreditRatingModel.Model
{
    public class FeeDetails
    {
        public decimal Id { get; set; }
        public int CountryId { get; set; }
        public int CollegeId { get; set; }
        public int CourseId { get; set; }
               
        public string CourseName { get; set; }               

        public decimal AcademicYearId{ get; set; }
        
        public decimal Fees { get; set; }
        public decimal OldFees { get; set; } 
        public int Year { get; set; } 

        public DateTime CreateDate { get; set; }
        public int CreateId { get; set; }
        public DateTime EditDate { get; set; }
        public int EditId { get; set; }
        
    }

    public class SaveCollageFeesData
    {
        public string SaveFeesData { get; set; }
    }
      
    
}
