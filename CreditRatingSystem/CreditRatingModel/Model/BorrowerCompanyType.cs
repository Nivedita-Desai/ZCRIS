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
  public  class BorrowerCompanyType
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Sub Division Name is compulsory.")]
        [MaxLength (1000)]
        public string CompanyType { get; set; }
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please Select Company Name.")] 
        public int CompanyId { get; set; }
     
        public string CompanyName { get; set; }

        public int CreateId { get; set; }
        public int EditId { get; set; }

        public Nullable<DateTime> CreateDate { get; set; }            
        public Nullable<DateTime> EditDate { get; set; }

        public class BorrowerCompanyMaster
        { 
            public int CompanyId { get; set; }            
            public string CompanyName { get; set; }
        }
        public BorrowerCompanyBranchMasters BCBM;
        public ContactDetailsMasters CDM;
    }
}
