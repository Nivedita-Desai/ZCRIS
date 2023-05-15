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
    public class EmployerMaster_Metadata
    {
        //-----personal details ----//
        public decimal id { get; set; }
        public int EmployerID { get; set; }
        public string PanNo { get; set; }
          [DataType(DataType.Date)]
         [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        
        public DateTime IncorporationDt { get; set; }
          [DataType(DataType.Date)]
         [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CommencementDt { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime EditDate { get; set; }
        public int CreateId { get; set; }
        public int EditId { get; set; }
        public string EmployerType { get; set; }
        public List<string>EmployerTypes { get; set; }
        public List<string> EmployerBranches { get; set; }
        public string EmployerBranch { get; set; }
        public string EmployerName { get; set; }
        public String ImagePath
        {
            get
            {
                return "~/CreditRatingSystem/Images/dialog_cancel.png";
            }

        }
        //-----personal details finished----//

        //------contactdetails-------//
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public decimal CountryId { get; set; }
        public decimal StateId { get; set; }
        public string State { get; set; }
     
        public string City { get; set; }
        public decimal Cityid { get; set; }

        public string Pincode { get; set; }
        public string LandlineCode { get; set; }
        public string LandlineNo { get; set; }
        public string MobileCode { get; set; }
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
     
        //----contactdetails finished----//

        //----add new branch----//
        public int EmpName { get; set; }   

    }
}
