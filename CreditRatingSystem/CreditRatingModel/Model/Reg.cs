using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreditRatingModel.Model
{
  public  class Reg
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int UserType { get; set; }
        public string Name { get; set; }
        public string EmailId { get; set; }
        public int employerId { get; set; }
        public int employerDivisionId { get; set; }
        public int DistributionTypeId { get; set; }
        public decimal FinantialInstituteId { get; set; }
        public decimal FinantialInstituteBranchId { get; set; }
        public int collegeId { get; set; }
        public string ContactNo { get; set; }
        public string countryCode { get; set; }
        public bool Active { get; set; }

        public string newPassword { get; set; }
        public string confirmPassword { get; set; }
    }
}
