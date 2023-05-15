using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CreditRatingModel.Model
{
    public class IndividualEmiTransaction_Metadata
    {
        public string NationalId { get; set; }
        public string LoanNoList { get; set; }
        public string FullName { get; set; }
        public decimal LoanAmt { get; set; }
        public string RepayMode { get; set; }
        public DateTime ChqDt { get; set; }
        public decimal DisAmt { get; set; }
        public decimal RepayAmt { get; set; }
        public decimal BalAmt { get; set; }
        public string Remark { get; set; }
        public string TranNo { get; set; }
        public string TranId { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }


        public string Gender { get; set; }

        public int BankId { get; set; }
        public int BranchId { get; set; }

        public decimal TotalLoanAmount { get; set; }
        public decimal TotalBalanceAmount { get; set; }

        public string status { get; set; }

        //Additional For Reverse Transaction
        public string LoanAccountNo { get; set; }
        public DateTime RepaymentDate { get; set; }
        public decimal AvailableBalance { get; set; }
        public decimal LedgerBalance { get; set; }
        public string ChequeClear { get; set; }
        public DateTime ChequeClearanceDt { get; set; }

        public decimal FinanceInstituteId { get; set; }
        public decimal FinanceInstituteBranchId { get; set; }
    }
}
