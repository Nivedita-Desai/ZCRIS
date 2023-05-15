using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CreditRatingModel.Model
{
    public class FinancialInstitutionMaster_Metadata
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Financial Institute Name Required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Person Full Name Required")]
        public string PersonFullName { get; set; }

        [Required(ErrorMessage = "Person Full Name Required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Person Full Name Required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Registration No Required")]
        public string RegistrationNo { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime EditDate { get; set; }
        public int CreateId { get; set; }
        public int EditId { get; set; }
        
    }
}
