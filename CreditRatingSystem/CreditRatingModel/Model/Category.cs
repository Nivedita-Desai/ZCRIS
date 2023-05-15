using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CreditRatingModel.Model
{
    public class Category
    {                
        public List<SelectListItem> CategoryItem { get; set; }
        public List<SelectListItem> country { get; set; }

        //public int Id { get; set; }
        //public string CountryId { get; set; }        
        //public string StateName { get; set; }

        ////-----------------Parameters---------------------

        ////financial institute master parameters
        //public int InstituteId { get; set; }
        //public string Name { get; set; }
        //public string RegistrationNo { get; set; }
        //public string PersonFullName { get; set; }
        //public string UserName { get; set; }
        //public string Password { get; set; }

        ////financial institute category relation parameters
        //public int FinancialInstituteCategoryId { get; set; }
        //public int FinancialInstituteId { get; set; }
        //public int FinancialCategoryRelationId { get; set; }
        ////public string InstituteCategory { get; set; }

        ////financial institute branch master parameters
        //public string BranchName { get; set; }
        //public int InstituteBranchId { get; set; }

        ////financial institute contact master parameters
        //public int BranchId { get; set; }
        //public string Address1 { get; set; }
        //public string Address2 { get; set; }
        //public string Address3 { get; set; }
        //public int CountryId { get; set; }
        //public int StateId { get; set; }
        //public string City { get; set; }
        //public string Pincode { get; set; }
        //public int InstituteContactMasterId { get; set; }
        //public string State { get; set; }

        ////common parameters
        //public DateTime CreateDate { get; set; }
        //public int CreateId { get; set; }

        ////-------------------------------------------------

        //public List<SelectListItem> state { get; set; }
        public FinancialInstitutionCategoryMaster objcategory { get; set; }
        public CountryMaster Country { get; set; }
        //public StateMaster State { get; set; }
        public NationalityMaster Nationality { get; set; }

        public FinancialInstitutionMaster Finance_Institute { get; set; }
        public FinancialInstitutionCategoryMaster Finance_Category { get; set; }
        //public FinancialInstitutionCategoryRelation Category_Relation { get; set; }
        public FinancialInstitutionBranchMaster Finance_Branch { get; set; }
        public FinancialInstitutionContactMaster Finance_Contact { get; set; }

        public FinancialInstituteModel finance_model { get; set; }
        
        public int SessionId = 1;

    }
}
