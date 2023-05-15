using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreditRatingModel.Model
{
  public  class ReportSS
    {
        public int Id { get; set; }
        public string NID { get; set; }
        public int collegeID { get; set; }
        public int AYID { get; set; }
        public int courseId { get; set; }
        public string LoanAccountNo { get; set; }
      
        public DateTime  FromDate { get; set; } 
        public DateTime  ToDate { get; set; }
    }

}
