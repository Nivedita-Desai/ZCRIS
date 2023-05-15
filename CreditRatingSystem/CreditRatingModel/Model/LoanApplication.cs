using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace CreditRatingModel.Model
{
    public class LoanApplication
    {
        public int Id { get; set; }
        public string LoanApplicationNo { get; set; }
        public string ApplicantName { get; set; }
               
        [DataType(DataType.Date)]        
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime LoanApplicationDate { get; set; }
                
        public int LoanTypesId { get; set; }
        public int CardTypeId { get; set; }       
        public decimal LoanAmount { get; set; }
        public int BorrowerId { get; set; }
        public string PanL { get; set; }
        public string TPin { get; set; }

        public DateTime CreateDate { get; set; }
        public int CreateId { get; set; }
        public DateTime EditDate { get; set; }
        public int EditId { get; set; }

        public string Type { get; set; }
        public int LIRId { get; set; }

        public int IndividualCustomerId { get; set; }

        public decimal FinancialInstitutionId { get; set; }
        public decimal FinancialInstitutionBranchId { get; set; }

        public int BorrowerTypeId { get; set; }
                
        //for Loan Approval
        public string LoanStatus { get; set; }
        public int ReasonId { get; set; }

        public ContactDetailsMasters CDM {get;set;}
        public IndividualCustomerMasters ICM {get;set;}
        public CompanyDirectorRelations CDR { get; set; }
         
    }
}
