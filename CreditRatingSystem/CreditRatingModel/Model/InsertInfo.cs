using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreditRatingModel.Model
{
    public class InsertInfo
    {
        public static int borrowerid1 { get; set; }   
        public static int borrowerid { get; set; }
        public static int individual { get; set; }
        public static int CompanyId { get; set; }
        public static int BranchMasterId { get; set; }
        public static string pan { get; set; }
        public static int username { get; set; }
        public static string companyName { get; set; }
        public static int branch { get; set; }
        public static int CompanyTypeId { get; set; }
        public static int StateId { get; set; }


        creaditratingEntities CRE = new creaditratingEntities();
        public void Add(int intUserId, int Title, int MaritalStatusId, int NationalityId, int id, string PAN, string CIN, string FirstName, string MiddleName, string LastName, string Sex, Nullable<DateTime> DateOfBirth, string CompanyName, Nullable<DateTime> IncorporationDate, Nullable<DateTime> CommencementDate, string CompanyType, string BranchName, string Address1, string Address2, string Address3, int Addressid, int CountryId, int StateId, int City, string Pincode, string LandlineCode, string LandlineNo, string MobileCode, string MobileNo, string EmailId1, string EmailId2, string Designation, string borrowerType)
        {


            BorrowerCompanyMaster BCM = new BorrowerCompanyMaster();
            BorrowerCompanyTypeMaster BCTM = new BorrowerCompanyTypeMaster();
            BorrowerCompanyBranchMaster BCBM = new BorrowerCompanyBranchMaster();
            BorrowerDetailsMaster BDM = new BorrowerDetailsMaster();
            IndividualCustomerMaster ICM = new IndividualCustomerMaster();

            //------020915------//
            if (CompanyName == null)
            {

                var pan1 = (from pn in CRE.IndividualCustomerMasters where pn.Pan == PAN select new { pn.Pan }).FirstOrDefault();



                if (pan1 != null)
                {
                    var i = (from p in CRE.IndividualCustomerMasters where p.Pan == PAN select p.Id).FirstOrDefault();

                    BorrowerDetailsMaster bdm = new BorrowerDetailsMaster();
                    bdm.BorrowerType = borrowerType;
                    bdm.IndividualCustomerId = Convert.ToInt32(i);
                    bdm.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                    bdm.CreateId = Convert.ToInt32(intUserId);
                    CRE.AddToBorrowerDetailsMasters(bdm);
                    CRE.SaveChanges();
                    borrowerid1 = Convert.ToInt32(bdm.Id);
                    individual = Convert.ToInt32(bdm.IndividualCustomerId);
                    Addinfo(intUserId, Address1, Address2, Address3, Addressid, CountryId, StateId, City, Pincode, LandlineCode, LandlineNo, MobileCode, MobileNo, EmailId1, EmailId2, CompanyName);

                }
                else
                {
                    ICM.FirstName = FirstName;
                    ICM.MiddleName = MiddleName;
                    ICM.LastName = LastName;
                    ICM.TitleId = Convert.ToInt32(Title);
                    ICM.MaritalStatusId = MaritalStatusId;
                    ICM.NationalityId = Convert.ToInt32(NationalityId);
                    ICM.Pan = PAN;
                    pan = ICM.Pan;

                    ICM.Sex = Sex;
                    ICM.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                  //  var i2 = (from p in CRE.User_Details
                  //            where p.USERNAME == USERNAME
                  //            select
                  //            p.ID
                  //).FirstOrDefault();
                    ICM.CreateId = Convert.ToInt32(intUserId);

                    ICM.DateOfBirth = Convert.ToDateTime(DateOfBirth);
                    CRE.AddToIndividualCustomerMasters(ICM);
                    CRE.SaveChanges();

                    BDM.BorrowerType = borrowerType;
                    BDM.CreateId = Convert.ToInt32(intUserId);
                    BDM.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                    BDM.IndividualCustomerId = ICM.Id;
                    CRE.AddToBorrowerDetailsMasters(BDM);
                    CRE.SaveChanges();

                    borrowerid1 = Convert.ToInt32(BDM.Id);
                    individual = Convert.ToInt32(ICM.Id);
                    Addinfo(intUserId, Address1, Address2, Address3, Addressid, CountryId, StateId, City, Pincode, LandlineCode, LandlineNo, MobileCode, MobileNo, EmailId1, EmailId2, CompanyName);

                }
            }
            else
            {
                ICM.FirstName = null;
                ICM.MiddleName = null;
                ICM.LastName = null;
                ICM.TitleId = null;
                ICM.NationalityId = null;

                ICM.Pan = null;
                ICM.Sex = null;
                ICM.CreateDate = null;





                var q = (from w in CRE.BorrowerCompanyMasters
                         where w.CompanyName == CompanyName
                         select w.Id).FirstOrDefault();


                if (Convert.ToInt32(q) != 0)
                {

                    var m = (from w in CRE.BorrowerCompanyTypeMasters
                             where w.CompanyType == CompanyType && w.CompanyId == q
                             select w.Id).FirstOrDefault();

                    if (Convert.ToInt32(m) != 0)
                    {
                        var n = (from w in CRE.BorrowerCompanyBranchMasters
                                 where w.BranchName == BranchName && w.CompanyId == q && w.CompanyTypeId == m
                                 select w.Id).FirstOrDefault();

                        if (Convert.ToInt32(n) != 0)
                        {

                            BDM.BranchMasterId = Convert.ToInt32(n);
                            branch = Convert.ToInt32(BDM.BranchMasterId);
                            BDM.CompanyTypeId = Convert.ToInt32(m);
                            BDM.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                            BDM.BorrowerType = borrowerType;
                            //var i4 = (from p in CRE.User_Details
                            //          where p.USERNAME == USERNAME
                            //          select
                            //          p.ID
                            //     ).FirstOrDefault();
                            BDM.CreateId = Convert.ToInt32(intUserId);
                            CRE.AddToBorrowerDetailsMasters(BDM);
                            CRE.SaveChanges();
                            companyName = CompanyName;
                            CompanyTypeId = Convert.ToInt32(BDM.CompanyTypeId);
                            borrowerid = Convert.ToInt32(BDM.Id);
                            Addinfo(intUserId, Address1, Address2, Address3, Addressid, CountryId, StateId, City, Pincode, LandlineCode, LandlineNo, MobileCode, MobileNo, EmailId1, EmailId2, CompanyName);


                        }
                        else
                        {
                            BCBM.BranchName = BranchName;
                            BCBM.CompanyId = Convert.ToInt32(q);
                            BCBM.CompanyTypeId = Convert.ToInt32(m);
                            BCBM.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                            //var i4 = (from p in CRE.User_Details
                            //          where p.USERNAME == USERNAME
                            //          select
                            //          p.ID
                            //     ).FirstOrDefault();
                            BCBM.CreateId = Convert.ToInt32(intUserId);
                            CRE.AddToBorrowerCompanyBranchMasters(BCBM);
                            CRE.SaveChanges();

                            BDM.BranchMasterId = BCBM.Id;
                            branch = Convert.ToInt32(BDM.BranchMasterId);

                            BDM.CompanyTypeId = Convert.ToInt32(m);
                            BDM.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                            BDM.BorrowerType = borrowerType;
                            //var i5 = (from p in CRE.User_Details
                            //          where p.USERNAME == USERNAME
                            //          select
                            //          p.ID
                            //     ).FirstOrDefault();
                            BDM.CreateId =  Convert.ToInt32(intUserId); ;
                            CRE.AddToBorrowerDetailsMasters(BDM);
                            CRE.SaveChanges();
                            CompanyTypeId = Convert.ToInt32(BDM.CompanyTypeId);
                            companyName = CompanyName;
                            borrowerid = Convert.ToInt32(BDM.Id);
                            Addinfo(intUserId, Address1, Address2, Address3, Addressid, CountryId, StateId, City, Pincode, LandlineCode, LandlineNo, MobileCode, MobileNo, EmailId1, EmailId2, CompanyName);


                        }


                    }
                    else
                    {
                        BCTM.CompanyType = CompanyType;
                        BCTM.CompanyId = Convert.ToInt32(q);

                        BCTM.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                        //var i4 = (from p in CRE.User_Details
                        //          where p.USERNAME == USERNAME
                        //          select
                        //          p.ID
                        //     ).FirstOrDefault();
                        BCTM.CreateId = Convert.ToInt32(intUserId);
                        CRE.AddToBorrowerCompanyTypeMasters(BCTM);
                        CRE.SaveChanges();

                        BCBM.CompanyTypeId = BCTM.Id;
                        BCBM.BranchName = BranchName;


                        BCBM.CompanyId = Convert.ToInt32(q);
                       
                        BCBM.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                        //var i5 = (from p in CRE.User_Details
                        //          where p.USERNAME == USERNAME
                        //          select
                        //          p.ID
                        //     ).FirstOrDefault();
                        BCBM.CreateId =  Convert.ToInt32(intUserId); 
                        CRE.AddToBorrowerCompanyBranchMasters(BCBM);
                        CRE.SaveChanges();
                        branch = Convert.ToInt32(BCBM.Id);

                        BDM.BranchMasterId = BCBM.Id;


                        BDM.CompanyTypeId = BCTM.Id;
                        BDM.CreateDate = DateTime.UtcNow.Date;
                        BDM.BorrowerType = borrowerType;
                        //var i6 = (from p in CRE.User_Details
                        //          where p.USERNAME == USERNAME
                        //          select
                        //          p.ID
                        //     ).FirstOrDefault();
                        BDM.CreateId = Convert.ToInt32(intUserId); 
                        CRE.AddToBorrowerDetailsMasters(BDM);
                        branch = Convert.ToInt32(BDM.BranchMasterId);
                        CRE.SaveChanges();
                        CompanyTypeId = Convert.ToInt32(BDM.CompanyTypeId);
                        companyName = CompanyName;
                        borrowerid = Convert.ToInt32(BDM.Id);
                        Addinfo(intUserId, Address1, Address2, Address3, Addressid, CountryId, StateId, City, Pincode, LandlineCode, LandlineNo, MobileCode, MobileNo, EmailId1, EmailId2, CompanyName);



                    }



                }
                else
                {


                    BCM.CompanyName = CompanyName;
                    companyName = BCM.CompanyName;
                   
                    BCM.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                    //var i = (from p in CRE.User_Details
                    //         where p.USERNAME == USERNAME
                    //         select
                    //         p.ID
                    //       ).FirstOrDefault();
                    BCM.CreateId = intUserId;
                    //BCM.Designation = Designation;
                    BCM.IncorporationDate = IncorporationDate;
                    BCM.CommencementDate = CommencementDate;
                    BCM.PAN = CIN;
                    pan = CIN;

                    CRE.AddToBorrowerCompanyMasters(BCM);
                    CRE.SaveChanges();



                    BCTM.CompanyType = CompanyType;
                    BCTM.CompanyId = BCM.Id;
                    CompanyId = Convert.ToInt16(BCTM.CompanyId);
                    BCTM.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                    //var i4 = (from p in CRE.User_Details
                    //          where p.USERNAME == USERNAME
                    //          select
                    //           p.ID
                    //     ).FirstOrDefault();
                    BCTM.CreateId = intUserId;
                    CRE.AddToBorrowerCompanyTypeMasters(BCTM);
                    CRE.SaveChanges();

                    BCBM.CompanyTypeId = BCTM.Id;
                    BCBM.BranchName = BranchName;

                    BCBM.CompanyId = BCM.Id;
                    BCBM.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                    //var i5 = (from p in CRE.User_Details
                    //          where p.USERNAME == USERNAME
                    //          select
                    //          p.ID
                    //    ).FirstOrDefault();
                    BCBM.CreateId = intUserId;
                    CRE.AddToBorrowerCompanyBranchMasters(BCBM);
                    CRE.SaveChanges();

                    BDM.BorrowerType = borrowerType;
                    BDM.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                    //var i6 = (from p in CRE.User_Details
                    //          where p.USERNAME == USERNAME
                    //          select
                    //          p.ID
                    //     ).FirstOrDefault();
                    BDM.CreateId = intUserId;
                    BDM.BranchMasterId = BCBM.Id;
                    BDM.CompanyTypeId = BCTM.Id;
                    CRE.AddToBorrowerDetailsMasters(BDM);
                    CRE.SaveChanges();
                    branch = Convert.ToInt32(BDM.BranchMasterId);
                    CompanyTypeId = Convert.ToInt32(BDM.CompanyTypeId);
                    companyName = CompanyName;
                    borrowerid = Convert.ToInt32(BDM.Id);
                    Addinfo(intUserId, Address1, Address2, Address3, Addressid, CountryId, StateId, City, Pincode, LandlineCode, LandlineNo, MobileCode, MobileNo, EmailId1, EmailId2, CompanyName);



                }
            }
            //BCTM.CompanyType = CompanyType;
            //BCTM.CompanyId = BCM.Id;
            //BCTM.CreateDate = DateTime.UtcNow.Date;
            //var i1 = (from p in CRE.User_Details
            //          where p.USERNAME == USERNAME
            //          select
            //          p.ID
            //                ).FirstOrDefault();
            //BCTM.CreateId = i1;
            //CRE.AddToBorrowerCompanyTypeMasters(BCTM);
            //CRE.SaveChanges();

            //BCBM.BranchName = BranchName;
            //BCBM.CreateDate = DateTime.UtcNow.Date;
            //var i4 = (from p in CRE.User_Details
            //          where p.USERNAME == USERNAME
            //          select
            //          p.ID
            //     ).FirstOrDefault();
            //BCBM.CreateId = i4;



            //var d = (CRE.BorrowerCompanyMasters.Where(a => a.CompanyName.Equals(CompanyName))).FirstOrDefault();

            //if (d != null)
            //{
            //    var q = (from u in CRE.BorrowerCompanyMasters
            //             where u.CompanyName == CompanyName
            //             select u.Id
            //             ).FirstOrDefault();


            //    BCTM.CompanyId = Convert.ToDecimal(q);
            //    BCBM.CompanyId = BCTM.CompanyId;
            //    BCBM.CompanyTypeId = BCTM.Id;

            //}
            //else
            //{

            //    BCTM.CompanyId = BCM.Id;
            //    BCBM.CompanyTypeId = BCTM.Id;
            //    BCBM.CompanyId = BCTM.CompanyId;

            //}
            //CRE.AddToBorrowerCompanyBranchMasters(BCBM);
            //CRE.SaveChanges();




            //BDM.Id = id;




            //BDM.CreateDate = DateTime.UtcNow.Date;
            //var i3 = (from p in CRE.User_Details
            //          where p.USERNAME == USERNAME
            //          select
            //          p.ID
            //       ).FirstOrDefault();
            //BDM.CreateId = i;


            //if (BCM.CompanyName == null)
            //{
            //    BDM.IndividualCustomerId = ICM.Id;
            //    BDM.CompanyTypeId = null;



            //}
            //else
            //{
            //    BDM.CompanyTypeId = BCTM.Id;
            //    BDM.IndividualCustomerId = null;

            //    BDM.BranchMasterId = BCBM.Id;
            //}
            //CRE.AddToBorrowerDetailsMasters(BDM);
            //CRE.SaveChanges();

            pan = PAN;

            username = intUserId;

        }

        //-------030915----contact info ----//
        public void Addinfo(int intUserId, string Address1, string Address2, string Address3, int Addressid, int CountryId, int StateId, int City, string Pincode, string LandlineCode, string LandlineNo, string MobileCode, string MobileNo, string EmailId1, string EmailId2, string CompanyName)
        {



            ContactDetailsMaster CD = new ContactDetailsMaster();
            CD.Address1 = Address1;
            CD.Address2 = Address2;
            CD.Address3 = Address3;
            CD.CountryId = CountryId;
            CD.PAN = pan;
            CD.StateId = StateId;
            CD.CityId  = City;
            CD.Pincode = Pincode;
            CD.LandlineCode = LandlineCode;
            CD.LandlineNo = LandlineNo;
            CD.MobileCode = MobileCode;
            CD.MobileNo = MobileNo;
            CD.EmailId1 = EmailId1;
            CD.EmailId2 = EmailId2;
            CD.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
        
            CD.CreateId = intUserId;
            if (CompanyName == null)
            {
                CD.IndividualCustomerId = InsertInfo.individual;
                CD.BorrowerBranchId = null;
            }
            else
            {
                CD.IndividualCustomerId = null;
                CD.BorrowerBranchId = InsertInfo.branch;
            }
            CRE.AddToContactDetailsMasters(CD);
            CRE.SaveChanges();



            ContactRelation CR = new ContactRelation();
            if (CompanyName == null)
            {

                CR.IndividualCustomerId = InsertInfo.individual;
                CR.BorrowerId = InsertInfo.borrowerid1;
            }
            else
            {
                CR.IndividualCustomerId = null;
                CR.BorrowerId = InsertInfo.borrowerid;
            }
            CR.AddressTypeId = Addressid;


            CR.ContactDetailsId = CD.Id;
            CR.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
        
            CR.CreateId = Convert.ToInt32(intUserId);
            CRE.AddToContactRelations(CR);
            CRE.SaveChanges();


        }
 



        //-----------file upload--------030915-----//
        public void AddUploads(int intUserId, int DOC, string docid, Nullable<DateTime> IssueDate, Nullable<DateTime> Expirydate, string FilePath)
        {

            DocumentCustomerRelation DCR = new DocumentCustomerRelation();
            if (InsertInfo.companyName == null)
            {

                DCR.BorrowerId = InsertInfo.borrowerid1;
                DCR.IndividualCustomerId = InsertInfo.individual;
                DCR.CompanyTypeId = null;
            }
            else
            {

                DCR.IndividualCustomerId = null;
                DCR.BorrowerId = InsertInfo.borrowerid;
                DCR.CompanyTypeId = InsertInfo.CompanyTypeId;
            }
            DCR.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
          //  var j = (from p in CRE.User_Details
          //           where p.USERNAME == USERNAME
          //           select
          //           p.ID
          //).FirstOrDefault();
            DCR.CreateId = intUserId;
            DCR.DocumentId = DOC;
            DCR.DocumentNumber = docid;
            DCR.IssueDate = IssueDate;
            DCR.ExpiryDate = Expirydate;
            DCR.FilePath = FilePath;
            CRE.AddToDocumentCustomerRelations(DCR);
            CRE.SaveChanges();



        }






    }
            
}
