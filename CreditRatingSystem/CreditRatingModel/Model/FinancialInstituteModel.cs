using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CreditRatingModel.Model
{
    [MetadataType(typeof(FinancialInstituteModel))]
    public partial class FinancialInstitute
    {

    }
    public class FinancialInstituteModel
    {
        creaditratingEntities db = new creaditratingEntities();
        //financial institute master parameters
        public int Id { get; set; }
        public decimal Id1 { get; set; }
        public List<SelectListItem> CategoryItem { get; set; }
        public List<SelectListItem> CountryItem { get; set; }


        public string Name { get; set; }

       public string RegistrationNo { get; set; }

          //public string PersonFullName { get; set; }

           //public string UserName { get; set; }

       //public string Password { get; set; }

                public string BankCode { get; set; }

        //financial institute category relation parameters
        public int FinancialInstituteCategoryId { get; set; }
        public int FinancialInstituteId { get; set; }
        public decimal FinancialInstituteId1 { get; set; }
        public int FinancialCategoryRelationId { get; set; }
        public string  InstituteCategory { get; set; }

        //financial institute branch master parameters
        public string BranchName { get; set; }

         public string BranchCode { get; set; }

         public string SwiftCode { get; set; }
         public string IfscCode { get; set; }

        public int InstituteBranchId { get; set; }

        //financial institute category master parameters
     
        public string Category { get; set; }
          public int CategoryId { get; set; }
          public decimal CategoryId2 { get; set; }

        //financial institute contact master parameters
        public int BranchId { get; set; }
        public string Address1 { get; set; }

           public string Address2 { get; set; }

        public string Address3 { get; set; }
          public string City { get; set; }
        public decimal cityid { get; set; }
           public string Pincode { get; set; }

        public int InstituteContactMasterId { get; set; }
          public decimal CountryId { get; set; }

          public decimal StateId { get; set; }
   
        public string FinancialInstituteContact { get; set; }

        public string Code { get; set; }
        public string Code1 { get; set; }
        public string Code2 { get; set; }

             public string ContactNo { get; set; }
      
          public string FinancialInstituteEmailId1 { get; set; }

             public string ContactPerson1 { get; set; }

        public string ContactPerson2 { get; set; }
   public string ContactPerson1Mobile { get; set; }

          public string ContactPerson2Mobile { get; set; }
   public string ContactPerson1EmailId { get; set; }

           public string ContactPerson2EmailId { get; set; }

          public List<int> FinancialInstituteCreditTypeId { get; set; }

        public FinancialInstitutionCategoryMaster objcategory { get; set; }
        public CountryMaster CountryMaster { get; set; }
        public StateMaster StateMaster { get; set; }

        //common parameters
        [DataType(DataType.DateTime)]
        public DateTime CreateDate { get; set; }
        public int CreateId { get; set; }

        public int SessionId = 1;
        public int AreaId { get; set; }

        FinancialInstitutionCreditTypeMaster FICTM = new FinancialInstitutionCreditTypeMaster();
        FinancialInstitutionCategoryMaster FICM = new FinancialInstitutionCategoryMaster();
        //CountryMaster CM = new CountryMaster();
        //StateMaster SM = new StateMaster();

        //public void Add(int CategoryId, string Name, string PersonFullName, string UserName, string Password, string RegistrationNo, string BranchName,int CreditTypeId, string FinancialInstituteContact, string Address1, string Address2, string Address3, int CountryId, int StateId, string City, string Pincode, string Code, string ContactNo, string FinancialInstituteEmailId1, string ContactPerson1, string ContactPerson2, string ContactPerson1Mobile, string ContactPerson2Mobile, string ContactPerson1EmailId, string ContactPerson2EmailId)
        //{

        //    FinancialInstitutionMaster FIM = new FinancialInstitutionMaster();
        //    FIM.Name = Name;
        //    FIM.PersonFullName = PersonFullName;
        //    FIM.UserName = UserName;
        //    FIM.Password = Password;
        //    FIM.RegistrationNo = RegistrationNo;
        //    FIM.CreateDate = DateTime.UtcNow.Date;
        //    FIM.FinancialInstituteCategoryId = CategoryId;

        //    FinancialInstitutionBranchMaster FIBM = new FinancialInstitutionBranchMaster();
        //    FIBM.BranchName = BranchName;
        //    FIBM.CreateDate = DateTime.UtcNow.Date;
        //    FIBM.FinancialInstituteId=FIM.Id;

        //    FinancialInstitutionContactMaster FICOM = new FinancialInstitutionContactMaster();
        //    FICOM.FinancialInstituteBranchId = FIBM.Id;
        //    FICOM.FinancialInstituteContact = FinancialInstituteContact;
        //    FICOM.Address1 = Address1;
        //    FICOM.Address2 = Address2;
        //    FICOM.Address3 = Address3;
        //    FICOM.CountryId = CountryId;
        //    FICOM.StateId = StateId; 
        //    FICOM.City = City;
        //    FICOM.Pincode = Pincode;
        //    FICOM.Code = Code;
        //    FICOM.ContactNo = ContactNo;
        //    FICOM.FinancialInstituteEmailId1 = FinancialInstituteEmailId1;
        //    FICOM.ContactPerson1 = ContactPerson1;
        //    FICOM.ContactPerson2 = ContactPerson2;
        //    FICOM.ContactPerson1Mobile = ContactPerson1Mobile;
        //    FICOM.ContactPerson2Mobile = ContactPerson2Mobile;
        //    FICOM.ContactPerson1EmailId = ContactPerson1EmailId;
        //    FICOM.ContactPerson2EmailId = ContactPerson2EmailId;
        //    FICOM.CreateDate = DateTime.UtcNow.Date;
        //    FICOM.FinancialInstitutionId = FIM.Id;



        //    FinancialInstitutionCreditTypeRelation FICTR = new FinancialInstitutionCreditTypeRelation();
        //    FICTR.FinancialInstituteId = FIM.Id;
        //    FICTR.FinancialInstituteBranchId = FIBM.Id;
        //    FICTR.FinancialInstituteCreditTypeId = CreditTypeId;

        //    db.AddToFinancialInstitutionMasters(FIM);
        //    //db.AddToFinancialInstitutionCategoryMasters(FICM);
        //    db.AddToFinancialInstitutionBranchMasters(FIBM);
        //    db.AddToFinancialInstitutionContactMasters(FICOM);
        //    db.AddToFinancialInstitutionCreditTypeRelations(FICTR);
        //    db.SaveChanges();
        //}
    }
}
