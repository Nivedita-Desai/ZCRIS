using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CreditRatingModel.Model
{
    public class FinancialInstitutionCategoryRelation_Metadata
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Financial Institute Required")]
        public int FinancialInstituteId { get; set; }

        [Required(ErrorMessage = "Financial Institute Category Required")]
        public int FinancialInstituteCategoryId { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime EditDate { get; set; }
        public int CreateId { get; set; }
        public int EditId { get; set; }
       
    }
}
