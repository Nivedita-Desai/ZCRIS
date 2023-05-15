using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;


namespace CreditRatingModel.Model
{
    public class grid
    {
          [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

          [Required(ErrorMessage = "Category is required.")]
        public string Category { get; set; }

          [Required(ErrorMessage = "Bank Code is required.")]
        public string BankCode { get; set; }

          [Required(ErrorMessage = "Branch Code is required.")]
        public string BranchCode { get; set; }

[Required(ErrorMessage = "Swift Code  is required.")]
          public string SwiftCode { get; set; }

          [Required(ErrorMessage = "Branch Name is required.")]
        public string BranchName { get; set; }

        public int Id { get; set; }
        public DateTime CreateDate { get; set; }

        public string FinancialInstituteContact { get; set; }

          [Required(ErrorMessage = "Address is required.")]
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }

          [Required(ErrorMessage = "Country is required.")]
        public int CountryId { get; set; }
        public int StateId { get; set; }

          [Required(ErrorMessage = "City is required.")]
        public string City { get; set; }

          [Required(ErrorMessage = "Pincode is required.")]
        public string Pincode { get; set; }

          [Required(ErrorMessage = "Code is required.")]
        public string Code { get; set; }

          [Required(ErrorMessage = "Contact Number is required.")]
          [MinLength(10, ErrorMessage = "Contact No Minimum Length Should Be 10 Char")]
          [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Enter valid Contact number")] 
        public string ContactNo { get; set; }

          [Required(ErrorMessage = "Institute EmailId is required.")]
          [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Invalid email")]
        public string FinancialInstituteEmailId1 { get; set; }

          [Required(ErrorMessage = "Contact Person Name is required.")]
        public string ContactPerson1 { get; set; }
        public string ContactPerson2 { get; set; }

          [Required(ErrorMessage = "Mobile Number is required.")]
        [MinLength(10, ErrorMessage = "Contact No Minimum Length Should Be 10 Char")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Enter valid Contact number")] 
        public string ContactPerson2Mobile { get; set; }

          [MinLength(10, ErrorMessage = "Contact No Minimum Length Should Be 10 Char")]
          [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Enter valid Contact number")] 
        public string ContactPerson1Mobile { get; set; }

          [Required(ErrorMessage = "Email Id  is required.")]
          [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Invalid email")]
        public string ContactPerson1EmailId { get; set; }

              [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Invalid email")]
        public string ContactPerson2EmailId { get; set; }

          [Required(ErrorMessage = "State is required.")]
        public decimal State1 { get; set; }

          [Required(ErrorMessage = "Registration Number is required.")]
        public string RegistrationNumber { get; set; }

        public decimal Country1 { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string PersonFullName { get; set; }

        public List<SelectListItem> state { get; set; }
        //public List<SelectListItem> CategoryItem { get; set; }
        public List<SelectListItem> country { get; set; }
        //public FinancialInstitutionCategoryMaster FinancialInstituteCategoryMatser { get; set; }
        public CountryMaster CountryList { get; set; }
        public StateMaster StateList { get; set; }



        public class FinancialInstituteBranchMaster
        {
            public int Id { get; set; }
            public int FinancialInstituteCategoryRelationId { get; set; }
            public int FinancialInstituteId { get; set; }
            public string BranchName { get; set; }
            public DateTime CreateDate { get; set; }
            public virtual FinancialInstituteCategoryRelation FinancialInstituteCategoryRelation { get; set; }
        }

        public class FinancialInstituteCategoryMatser
        {
            public int Id { get; set; }
            public string Category { get; set; }
            public DateTime CreateDate { get; set; }
            public DateTime EditDate { get; set; }
            public int CreateId { get; set; }
            public int EditId { get; set; }
        }

        public class FinancialInstituteCategoryRelation
        {
            public int Id { get; set; }
            public int FinancialInstituteId { get; set; }
            public int FinancialInstituteCategoryId { get; set; }
            public DateTime CreateDate { get; set; }
            public DateTime EditDate { get; set; }
            public int CreateId { get; set; }
            public int EditId { get; set; }
            public virtual FinancialInstitutionMasters FinancialInstitutionMasters { get; set; }
            public virtual FinancialInstituteCategoryMatser FinancialInstituteCategoryMatser { get; set; }
        }
        public class FinancialInstitutionMasters
        {
            public int FinancialInstituteCategoryId { get; set; }
            public string Name { get; set; }
            public DateTime CreateDate { get; set; }
            public DateTime EditDate { get; set; }
            public int CreateId { get; set; }
            public int EditId { get; set; }

        }
        public class FinancialInstitutionContactMasters
        {
            public int Id { get; set; }
            public int FinancialInstituteBranchId { get; set; }
            public int FinancialInstituteContact { get; set; }
            public string  Address1 { get; set; }
            public string Address2 { get; set; }
            public int Address3 { get; set; }
            public string CountryId { get; set; }
            public string StateId { get; set; }
            public string City { get; set; }
            public string Pincode { get; set; }
            public string Code { get; set; }
            public string ContactNo { get; set; }
            public string FinancialInstituteEmailId1 { get; set; }
            public string ContactPerson1 { get; set; }
            public string ContactPerson2 { get; set; }                            
            public string ContactPerson2Mobile { get; set; }
            public string ContactPerson1Mobile { get; set; }
            public string ContactPerson1EmailId { get; set; }
            public string ContactPerson2EmailId { get; set; }
            public virtual FinancialInstituteBranchMaster FinancialInstituteBranchMaster { get; set; }
    
        }
        public class StateMaster
        {
            public int Id { get; set; }
            public int State { get; set; }
            public int CountryId { get; set; }
            public DateTime CreateDate { get; set; }
            public DateTime EditDate { get; set; }
            public virtual CountryMaster CountryMaster { get; set; }
        }

        public class CountryMaster
        {
            public int Id { get; set; }
            public string Country { get; set; }
            public DateTime CreateDate { get; set; }
            public DateTime EditDate { get; set; }
        }
    }
}
    

