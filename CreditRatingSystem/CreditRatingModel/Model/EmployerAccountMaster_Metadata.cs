using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CreditRatingModel.Model
{
    public class EmployerAccountMaster_Metadata
    {
        public int Id { get; set; }
        public string AccountName { get; set; }
        public string AccountShortName { get; set; }
        public int AccountType { get; set; }
        public int DistributionTypeId { get; set; }
        public int EmployerId { get; set; }
        public int EmployerDivisionId { get; set; }
        public decimal FinancialInstitutionId { get; set; }
        public decimal FinancialInstitutionBranchId { get; set; }
       
        public string IFSCCode { get; set; }
        public string AccountNo { get; set; }
        public string Narration { get; set; }
        public bool status { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
    }
}
