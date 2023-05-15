using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreditRatingModel.Model
{
    public class InsertBorrowerCompanyBranch
    {
        creaditratingEntities CRE = new creaditratingEntities();

        public void AddBorrBranch(string UserName, string BranchName, int CompanyTypeId, int CompanyId, string Address1, string Address2, string Address3, int CountryId, int StateId, string City, string Pincode, string LandlineCode, string LandlineNo, string MobileNo, string EmailId1, string EmailId2, int AddressTypeId)
        {            
            BorrowerCompanyBranchMaster BrMst = new BorrowerCompanyBranchMaster();
            BrMst.BranchName = BranchName;
            BrMst.CompanyTypeId = CompanyTypeId;
            BrMst.CompanyId = CompanyId;
            BrMst.CreateDate = DateTime.UtcNow.Date;

            var intUserId = (from p in CRE.User_Details
                             where p.USERNAME == UserName
                             select
                             p.ID
                   ).FirstOrDefault();
            BrMst.CreateId = intUserId;
            BrMst.CompanyTypeId =InsertBorrowerCompanyType.decCompTypeId;

            CRE.AddToBorrowerCompanyBranchMasters(BrMst);
            CRE.SaveChanges();

            ContactDetailsMaster ContMst = new ContactDetailsMaster();
            ContMst.PAN = null;            
            ContMst.Address1 = Address1;
            ContMst.Address2 = Address2;
            ContMst.Address3 = Address3;
            ContMst.CountryId = CountryId;
            ContMst.StateId = StateId;
            //ContMst.City = City;
            ContMst.Pincode = Pincode;
            ContMst.LandlineCode = LandlineCode;
            ContMst.LandlineNo = LandlineNo;
            ContMst.MobileNo = MobileNo;
            ContMst.EmailId1 = EmailId1;
            ContMst.EmailId2 = EmailId2;
            ContMst.BorrowerBranchId = BrMst.Id;
            ContMst.MobileCode = null;

            ContMst.CreateDate = DateTime.UtcNow.Date;
            ContMst.CreateId = intUserId;

            CRE.AddToContactDetailsMasters(ContMst);
            CRE.SaveChanges();

            ContactRelation ContRel = new ContactRelation();
            ContRel.AddressTypeId = AddressTypeId;
            ContRel.BorrowerId = BrMst.Id;           
            ContRel.ContactDetailsId = ContMst.Id;
            ContRel.CreateDate = DateTime.UtcNow.Date;
            ContRel.CreateId = intUserId;

            CRE.AddToContactRelations(ContRel);
            CRE.SaveChanges();
            
        }

    }
}
