using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CreditRatingModel.Model
{
    public class FinancialInstitutionContactMaster_Metadata
    {
        public List<SelectListItem> CategoryItem { get; set; }
        public List<SelectListItem> country { get; set; }
        public List<SelectListItem> CreditType { get; set; }
        [Key]
        public int Id { get; set; }

        //[Required(ErrorMessage = "Financial Institute Branch Required")]
        public int FinancialInstituteId { get; set; }

        //[Required(ErrorMessage = "Financial Institute Branch Required")]
        public string BranchName { get; set; }

        //[Required(ErrorMessage = "Financial Institute Contact Required")]
        //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Enter valid Contact number")]
        public string FinancialInstituteContact { get; set; }

        //[Required(ErrorMessage = "Address1 Required")]
        public string Address1 { get; set; }

        //[Required(ErrorMessage = "Address2 Required")]
        public string Address2 { get; set; }

        public string Address3 { get; set; }

        //[Required(ErrorMessage = "Country Required")]
        public int CountryId { get; set; }

        public int ContactNoLength { get; set; }

        //[Required(ErrorMessage="Select Credit Type")]
        public List<int> FinancialInstituteCreditTypeId { get; set; }

        //[Required(ErrorMessage = "State Required")]
        public int StateId { get; set; }

        //[Required(ErrorMessage = "City Required")]
        public string City { get; set; }

        //[Required(ErrorMessage = "Pincode Required")]
        //[MinLength(6, ErrorMessage = "Pincode Minimum Length Should Be 6 Char")]
        public string Pincode { get; set; }

        //[Required(ErrorMessage = "Country Code Required")]
        //[MinLength(2, ErrorMessage = "Country Code Minimum Length Should Be 2 Char")]
        public string Code { get; set; }

        //[Required(ErrorMessage = "Contact No Required")]
        //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Enter valid Contact number")]
        //[MinLength(9, ErrorMessage = "Contact No Minimum Length Should Be 10 Char")]
        public string ContactNo { get; set; }

        //[Required(ErrorMessage = "Email Id Required")]
        //[DataType(DataType.EmailAddress)]
        public string FinancialInstituteEmailId1 { get; set; }

        //[Required(ErrorMessage = "Contact Person1 Name Required")]
        //[DataType(DataType.Text)]
        public string ContactPerson1 { get; set; }

        //[Required(ErrorMessage = "Contact Person2 Name Required")]
        ////[DataType(DataType.Text)]
        public string ContactPerson2 { get; set; }

        //[Required(ErrorMessage = "Person 1 Contact No Required")]
        //[MinLength(9, ErrorMessage = "Contact No Minimum Length Should Be 10 Char")]
        //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Enter valid Contact number")]
        public string ContactPerson1Mobile { get; set; }

        //[Required(ErrorMessage = "Person 2 Contact No Required")]
        //[MinLength(9, ErrorMessage = "Contact No Minimum Length Should Be 10 Char")]
        //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Enter valid Contact number")]
        public string ContactPerson2Mobile { get; set; }

        //[Required(ErrorMessage = "Person1 Email Id Required")]
        //[DataType(DataType.EmailAddress)]
        public string ContactPerson1EmailId { get; set; }

        //[Required(ErrorMessage = "Person2 Email Id Required")]
        //[DataType(DataType.EmailAddress)]
        public string ContactPerson2EmailId { get; set; }

        //[Required(ErrorMessage = "Financial Institute Required")]
        public int FinancialInstitutionId { get; set; }

        //[Required(ErrorMessage ="Enter Branch Code")]
        //[MaxLength(11, ErrorMessage = "Branch Code Max Length Should Be 11 Char")]
        public string BranchCode { get; set; }

        //[DataType(DataType.DateTime)]
        public DateTime CreateDate { get; set; }
        public DateTime EditDate { get; set; }
        public int CreateId { get; set; }
        public int EditId { get; set; }
        public string SwiftCode { get; set; }
        public string IfscCode { get; set; }

        //public FinancialInstitutionBranchMaster_Metadata FIBM;
        //public FinancialInstitutionCategoryRelation_Metadata FICR;
        //public FinancialInstitutionMaster_Metadata FIM;
        //public CountryMaster CM;

    }
}
