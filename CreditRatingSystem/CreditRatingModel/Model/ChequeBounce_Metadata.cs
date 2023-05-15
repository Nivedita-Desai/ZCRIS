using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CreditRatingModel.Model
{
    public class ChequeBounce_Metadata
    {
       // public List<SelectListItem> ChequeType { get; set; }
        //public List<SelectListItem> ReasonList { get; set; }


        [Required(ErrorMessage = "Enter Bank Code")]
        [MaxLength(11, ErrorMessage = "Bank Code Minimum Length Should Be 11 Char")]
        public string BankCode { get; set; }


        [Required(ErrorMessage = "Enter Branch Code")]
        [MaxLength(11, ErrorMessage = "Branch Code Minimum Length Should Be 11 Char")]
        public string BranchCode { get; set; }

        [Required(ErrorMessage = "Bank Name Required")]
        public string BankName { get; set; }

        [Required(ErrorMessage = "Branch Name Required")]
        public string BranchName { get; set; }

        [Required(ErrorMessage = "Pancard Required")]
        public string Pancard { get; set; }

        [Required(ErrorMessage = "Account No Required")]
        public string FavourAccountNo { get; set; }

        [Required(ErrorMessage = "Name Required")]
        public string FavourOfName { get; set; }

        [Required(ErrorMessage = "Cheque No Required")]
        public string ChequeNo { get; set; }

        [Required(ErrorMessage = "Enter Cheque Date.")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]

        public DateTime ChequeDate { get; set; }

        [Required(ErrorMessage = "Cheque Amount Required")]
        public decimal ChequeAmount { get; set; }

        [Required(ErrorMessage = "Enter Transaction Date.")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime TransDate { get; set; }

        [Required(ErrorMessage = "Account No Required")]
        public string FrAccNo { get; set; }

        [Required(ErrorMessage = "Account Name Required")]
        public string FrAccName { get; set; }

        [Required(ErrorMessage = "Select Check Type Required")]
        public int ChTypeId { get; set; }

        [Required(ErrorMessage = "Select Reason Required")]
        public int ReasonId { get; set; }

        public string Reason { get; set; }

        //public int FInstId { get; set; }
    }
}
