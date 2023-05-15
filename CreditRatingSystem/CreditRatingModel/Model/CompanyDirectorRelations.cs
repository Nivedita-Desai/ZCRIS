using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;


namespace CreditRatingModel.Model
{
    public class CompanyDirectorRelations
    {       
        public int Id { get; set; }

        //[Required(ErrorMessage = "Select Company Name.")]  
        public decimal CompanyId { get; set; }
        public decimal IndividualCustomerId { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        //[Required(ErrorMessage = "Enter Joining Date.")]
        public DateTime DateOfJoining { get; set; }

        public int CreateId { get; set; }
        public int EditId { get; set; }

        public Nullable<DateTime> CreateDate { get; set; }
        public Nullable<DateTime> EditDate { get; set; }

    }
}
