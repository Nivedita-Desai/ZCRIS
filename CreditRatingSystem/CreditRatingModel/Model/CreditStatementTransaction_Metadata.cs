using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CreditRatingModel.Model
{
    public class CreditStatementTransaction_Metadata
    {
        public int Id { get; set; }

        public string TransactionId { get; set; }
        public int InstCounter { get; set; }
        public decimal InstAmount { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime InstDate { get; set; }
        public int LoanDisbursedId { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime BillingCycleStartDate { get; set; }
        public decimal MinDueAmt { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime InstDueDate { get; set; }

        ////public string CRAccNo1 { get; set; }
        ////public string CRAccNo2 { get; set; }
        ////public string CRAccNo3 { get; set; }
        ////public string CRAccNo4 { get; set; }
        public string CrAccNo { get; set; }
        public int gracePeriod { get; set; }
        public string CardType { get; set; }
        public decimal CreditLimit { get; set; }
        public decimal CashLimit { get; set; }
        public string FinantialInstituteName { get; set; }


        
    }
}
