using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreditRatingModel.Model
{
    public class CollegewiseCourse
    {
        public decimal Id {get; set;}
        public int CourseId {get; set;}
        public int CollegeId { get; set; }
        public bool Active { get; set; }

        public int StreamId { get; set; }
        public int CourseCategoryId { get; set; }

        public string CourseName { get; set; }
        public string CourseExits { get; set; }

        public DateTime CreateDate { get; set;}
        public int CreateId {get; set;}
        public DateTime EditDate {get; set;}
        public int EditId {get; set;}
    }

    public class SaveCollegewiseCourse
    {
        public string SaveCollegewiseCourseData { get; set; }
        public string SavePrimaryKeyData { get; set; }
    }
      
}
