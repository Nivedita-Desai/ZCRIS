using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CreditRatingModel.Model
{
    public class LoanDisburseTransaction_Metadata
    {
        public int Id { get; set; }
       
        public string Pancard { get; set; }

        public string disbursedBankName { get; set; }

        public string disbursedBranchName { get; set; }

        public int IndividualCustomerId { get; set; }
       
        public string LoanNo { get; set; }
     
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime LoanDate { get; set; }
       
        public int LoanTypeId { get; set; }

        public string LoanType { get; set; }
        public string Category { get; set; }
      
         [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime LoanDisburseDate { get; set; }
       
        public decimal DisburseAmount { get; set; }
       
        public decimal CashLimit { get; set; }

        public decimal CreditLimit { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime CreditCardIssueDate { get; set; }
       
         [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime CreditCardExpiryDate { get; set; }

       // public string CRAccNo1 { get; set; }
       
       // public string CRAccNo2 { get; set; }
       
       // public string CRAccNo3 { get; set; }
      
//public string CRAccNo4 { get; set; }
         public string CRAccNo { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime LoanAccCreateDate { get; set; }
        
        public decimal InterestTypeId { get; set; }
      
        public decimal FirstInstallment { get; set; }

         [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime FirstInstallmentDate { get; set; }
         [DataType(DataType.Date)]
         [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
         public DateTime LastInstallmentDate { get; set; }

         [DataType(DataType.Date)]
         [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
         public DateTime InstallmentDueDate { get; set; }

        public int NoOfMonths { get; set; }
        
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime ExpiryDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime BilCycleStartDt { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime BilCycleEndDt { get; set; }

       
        public decimal EMI { get; set; }
      
        public decimal RateOfInterest { get; set; }
        
        public string LoanAccNo { get; set; }

        public int gracePeriod { get; set; }

        public decimal Downpayment { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime IssueDate { get; set; }



        public string TransactionId { get; set; }
        public string LoanAccountNo { get; set; }
        public int InstCounter { get; set; }
        public decimal InstAmount { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime InstDate { get; set; }
        public int LoanDisbursedId { get; set; }
        public bool PaidStatus { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime PaidDate { get; set; }
        public int PaymentModeId { get; set; }
        public string ChqNo { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime ChqDate { get; set; }
        //public string BankName { get; set; }
        //public string BranchName { get; set; }
        public int DelayedDays { get; set; }
        public decimal LatePayCharges { get; set; }
        public decimal InterestAmt { get; set; }
        public decimal BounceCharges { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal BalanceAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public int BankId { get; set; }
        public int BranchId { get; set; }
        public string CardType { get; set; }
    }
}
