using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;


namespace CreditRatingModel.Model
{
    public class BorrowerCompanyBranchMasters
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(1000)]
        public string BranchNameCompBranch { get; set; }

        public int CompanyTypeId { get; set; }
        
        public int CreateId { get; set; }
        public int EditId { get; set; }

        public Nullable<DateTime> CreateDate { get; set; }
        public Nullable<DateTime> EditDate { get; set; }

        public int CompanyId { get; set; }
        
    }
}
