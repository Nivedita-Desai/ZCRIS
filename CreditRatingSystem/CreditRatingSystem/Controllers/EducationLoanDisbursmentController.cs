using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CreditRatingModel.Model;
using System.Transactions;

namespace CreditRatingSystem.Controllers
{
    public class EducationLoanDisbursmentController : Controller
    {
        //


        //aarti
        // GET: /EducationLoanDisbursment/
        public string n;
        private creaditratingEntities db = new creaditratingEntities();

        [Authorize]
        [HttpGet]
        public ActionResult AddEducationLoanDisbursement()
        {
            //EducationLoanDisbursment_Metadata model = new EducationLoanDisbursment_Metadata();
            //model.issuedDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
            dropList();
            return View();
        }

        public ActionResult NIDDetails(string NID)
        {
            var B = (from ELD in db.EducationLoanDisbursments
                     join CD in db.ContactDetails on ELD.NId equals CD.NID
                     join ICM in db.IndividualCustomerMasters on ELD.NId equals ICM.Pan
                     join SM in db.StateMasters on CD.StateId equals SM.Id
                     where ELD.NId == NID
                     orderby CD.Address1 descending
                     select new EducationLoanDisbursment_Metadata
                     {
                         NId = ELD.NId,
                      

                         TitleId1 = ICM.TitleId.Value,
                         DateOfBirth = ICM.DateOfBirth.Value,
                         Sex = ICM.Sex,
                         MarritalStatus = ICM.MaritalStatusId.Value,
                         FirstName = ICM.FirstName,
                         MiddleName = ICM.MiddleName,
                         LastName = ICM.LastName,
                         NationalityId = ICM.NationalityId.Value,

                         Address1 = CD.Address1,
                         Address2 = CD.Address2,
                         Address3 = CD.Address3,
                         CountryId1 = CD.CountryId.Value,
                         StateId1 = CD.StateId.Value,
                         CityId1 = CD.CityId.Value,
                         Pincode = CD.Pincode,
                         State = SM.State,
                         StudentContactNo = ELD.StudentContactNo,
                         StudentEmailId=ELD.StudentEmailId,
                         GarNID = ELD.GuardianNID,
                         GarName=ELD.GuardianName,
                       RId = ELD.RelationshipId.Value,
                         GarContact = ELD.GuardianContactNo,
                   

                     }).FirstOrDefault();
            //string Country  = B.CountryId1.ToString();
            //EBankDetails(Country);
            //return View(B);
            if (B == null)
            {
                var b = "null";
                return Json(b, JsonRequestBehavior.AllowGet);
            }
            return Json(B, JsonRequestBehavior.AllowGet);
        }

        public string GenerateLoanACNo()
        {
            string AutoGenrateNumber = "";

            DateTime date = DateTime.UtcNow.Date;

            int Day = DateTime.UtcNow.Day;
            int Month = DateTime.UtcNow.Month;
            int Year = DateTime.UtcNow.Year;
            string d = Day + "" + Month + "" + Year;

            AutoGenrateNumber = "EDU" + d + "0001";

        b:
            var c = (from s in db.EducationLoanDisbursments
                     where s.LoanAccountNo == AutoGenrateNumber
                     select
                     s.LoanAccountNo).FirstOrDefault();

            if (c == null)
            {
                // AutoGenrateNumber = AutoGenrateNumber;
            }
            else
            {
             
                var text = String.Format(AutoGenrateNumber.Substring(AutoGenrateNumber.Length - 4));
                int x = Convert.ToInt32(text);
                int x1 = x + 1;
                string t = x1.ToString();
                if (x1 > 999)
                    n = t;
                else if (x1 > 99)
                    n = "0" + x1;
                else if (x1 > 9)
                    n = "00" + x1;
                else
                    n = "000" + x1;

                AutoGenrateNumber = "EDU" + d + n;

                goto b;
            }

            return AutoGenrateNumber;
        }

        [HttpPost]
        public ActionResult ADD(SaveCollectionData SCD)
        {
           
            var result = false;
         
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string Data1 = SCD.childData;
            EducationLoanDisbursment_Metadata ChildField = serializer.Deserialize<EducationLoanDisbursment_Metadata>(Data1);
            int CreateUserId = Convert.ToInt32(Session["UserId"]);

            ////////////////////LoanA/C No////////////

            string AutoGenrateNumber = GenerateLoanACNo();

            using (TransactionScope transaction = new TransactionScope())
            {
                try
                {
                    EducationLoanDisbursment ELD = new EducationLoanDisbursment();
                    ELD.NId = ChildField.NId;
                    ELD.StudentContactNo = ChildField.code + '-' + ChildField.StudentContactNo;
                    ELD.StudentEmailId = ChildField.StudentEmailId;
                    ELD.CourseId = ChildField.CourseId;
                    ELD.CollegeId = ChildField.CollegeId;
                    ELD.DisburseAmount = ChildField.DFees;
              
                 ELD.DisburseDate = ChildField.LoanAccountDate;
                    ELD.LoanAmount = ChildField.LoanAmount;
                    ELD.LoanAccountNo = ChildField.AutoGenrateNumber;
                    ELD.LoanAccountDate = ChildField.LoanAccountDate;
                    ELD.AccademicYearId = ChildField.AcaYId;
                    ELD.BalanceAmount = ChildField.DFees;
                    ELD.Remark = ChildField.Remark;
                    ELD.AccountStatus = "A";
                    ELD.RelationshipId = ChildField.RelationId;
                    ELD.GuardianName = ChildField.GarName;
                    ELD.GuardianNID = ChildField.GarNID;
                    ELD.GuardianContactNo = ChildField.code1 + '-' + ChildField.GarContact;
                    ELD.CreateId = CreateUserId;
                    ELD.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                    ELD.SactionAmount = ChildField.LoanAmount;
                    db.AddToEducationLoanDisbursments(ELD);
                    db.SaveChanges();

                    int ELDID = ELD.Id;

                    var Con = (from CD in db.ContactDetails
                               where CD.NID == ChildField.NId
                               select CD.NID).FirstOrDefault();

                    if (ChildField.AddressChangedCount == 1 || Con == null)
                    {
                        ContactDetail CD = new ContactDetail();
                        CD.NID = ChildField.NId;
                        CD.Address1 = ChildField.Address1;
                        CD.Address2 = ChildField.Address2;
                        CD.Address3 = ChildField.Address3;
                        CD.CountryId = ChildField.CountryId1;
                        CD.StateId = ChildField.StateId1;
                        CD.CityId = ChildField.CityId1;
                        CD.Pincode = ChildField.Pincode;
                        CD.CreateId = CreateUserId;
                        CD.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                        db.AddToContactDetails(CD);
                                          }
                    var a = (from ICM1 in db.IndividualCustomerMasters
                             where ICM1.Pan == ChildField.NId
                             select ICM1).FirstOrDefault();
                    if (a == null)
                    {
                        IndividualCustomerMaster ICM = new IndividualCustomerMaster();
                        ICM.Pan = ChildField.NId;
                        ICM.FirstName = ChildField.FirstName;
                        ICM.MiddleName = ChildField.MiddleName;
                        ICM.LastName = ChildField.LastName;
                        ICM.Sex = ChildField.Sex;
                      //  ICM.MarritalStatus = ChildField.MarriedS;
                        ICM.MaritalStatusId = Convert.ToInt32(ChildField.MarriedS);
                        ICM.DateOfBirth = ChildField.DateOfBirth;
                        ICM.TitleId = ChildField.TitleId1;
                        ICM.NationalityId = ChildField.NationalityId;
                        ICM.CreateId = CreateUserId;

                        ICM.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                        db.AddToIndividualCustomerMasters(ICM);
                     
                    }




                    EduLoanDisbusementChild EDC = new EduLoanDisbusementChild();
                    EDC.EducationLoanDisburseId = ELDID;
                    EDC.AcademicYearId = ChildField.AcademicYeaId; //ELDM.AcademicYeaId;
                    EDC.Year = ChildField.courseyear; //ELDM.courseyear;
                    EDC.DisbursementAmount = ChildField.DFees;  // ELDM.DFees;
                    EDC.PaymentTypeId = Convert.ToInt32(ChildField.PayModeID); //ELDM.PayMode;
                    EDC.collageId = Convert.ToInt32(ChildField.NewcollageID); //ELD.collageId


                    if (ChildField.PayModeID == "1")
                    {
                        EDC.TranNo = ChildField.TranNo; //ELDM.TranNo;
                        EDC.TranDate = ChildField.TranDate;//ELDM.TranDate;
                        EDC.FinancialInstitutionId = Convert.ToInt32(ChildField.BName);//ELDM.BName;
                        EDC.FinancialInstitutionBranchId = Convert.ToInt32(ChildField.BrName); // ELDM.BrName;
                    }
                    else if (ChildField.PayModeID == "2")
                    {
                      //  EDC.FinancialInstitutionId = Convert.ToInt32(ChildField.BName);//ELDM.BName;
                        //EDC.FinancialInstitutionBranchId = Convert.ToInt32(ChildField.BrName); // ELDM.BrName;
                        EDC.TranNo = ChildField.TranNo; //ELDM.TranNo;
                        EDC.TranDate = ChildField.TranDate;//ELDM.TranDate;
                  
                    }

                    EDC.IssuedBy = ChildField.issuedBy;
                    EDC.IssuedDate = ChildField.issuedDate;
                    EDC.CreateId = CreateUserId;
                    EDC.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                    db.AddToEduLoanDisbusementChilds(EDC);
                    db.SaveChanges();

                    transaction.Complete();
                    result = true;
                    return Json(result, JsonRequestBehavior.AllowGet);

                }

                catch (Exception e)
                {
                    transaction.Dispose();
                    string m = e.Message;
                }
            }

            dropList();
            return View();
        }

        public void dropList()
        {
            ViewBag.Nationality = new SelectList((from NM in db.NationalityMasters orderby NM.Nationality select NM).ToList(), "Id", "Nationality");
            ViewBag.LoanType = new SelectList((from LTM in db.LoanTypesMasters where LTM.Id == 2 select LTM).ToList(), "Id", "Type");
            ViewBag.Country = new SelectList((from CM in db.CountryMasters orderby CM.Country select CM).ToList(), "Id", "Country");
            ViewBag.Province = new SelectList((from SM in db.StateMasters orderby SM.State select SM).ToList(), "Id", "State");
            ViewBag.City = new SelectList((from CM in db.CityMasters orderby CM.City select CM).ToList(), "Id", "City");
            ViewBag.College = new SelectList((from COllM in db.CollegeMasters orderby COllM.CollegeName select COllM).ToList(), "Id", "CollegeName");
            ViewBag.Course = new SelectList((from COM in db.CourseMasters orderby COM.CourseName select COM).ToList(), "Id", "CourseName");
            ViewBag.TitleId = new SelectList((from NTM in db.NameTitleMasters orderby NTM.Name select NTM).ToList(), "Id", "Name");
            ViewBag.Relationship = new SelectList((from R in db.Relationships orderby R.Relation select R).ToList(), "Id", "Relation");
           
            ViewBag.AcademicYear = new SelectList((from AY in db.AcademicYears orderby AY.AyId select AY).ToList(), "Id", "AyId");
            ViewBag.BankName = new SelectList((from FIM in db.FinancialInstitutionMasters
                                               join FICM in db.FinancialInstitutionCategoryMasters on FIM.FinancialInstituteCategoryId equals FICM.Id
                                               where FICM.Id == 1
                                               orderby FIM.Name
                                               select FIM).ToList(), "Id", "Name");

            ViewBag.PayMode=new SelectList(db.SP_BIND_PAYMENT_MODE().ToList(), "ID", "Name");
            ViewBag.MarritalStatusId = new SelectList((from MS in db.MaritalStatusMasters orderby MS.MaritalStatus select MS).ToList(), "Id", "MaritalStatus");
            ViewBag.issuedDate1 = DateTime.UtcNow.AddHours(5).AddMinutes(30);
        }

        public void EditdropList()
        {
            ViewBag.TitleId = new SelectList((from NTM in db.NameTitleMasters orderby NTM.Name select NTM).ToList(), "Id", "Name");
            ViewBag.Nationality = new SelectList((from NM in db.NationalityMasters orderby NM.Nationality select NM).ToList(), "Id", "Nationality");
            ViewBag.Country = new SelectList((from CM in db.CountryMasters orderby CM.Country select CM).ToList(), "Id", "Country");
            ViewBag.Relationship = new SelectList((from R in db.Relationships orderby R.Relation select R).ToList(), "Id", "Relation");
            ViewBag.LoanType = new SelectList((from LTM in db.LoanTypesMasters where LTM.Id == 2 select LTM).ToList(), "Id", "Type");
            //ViewBag.BankName = new SelectList((from FIM in db.FinancialInstitutionMasters
            //                                   join FICM in db.FinancialInstitutionCategoryMasters on FIM.FinancialInstituteCategoryId equals FICM.Id
            //                                   where FICM.Id == 1
            //                                   orderby FIM.Name
            //                                   select FIM).ToList(), "Id", "Name");

            ViewBag.PayMode = new SelectList(db.SP_BIND_PAYMENT_MODE().ToList(), "ID", "Name");
            ViewBag.MarritalStatusId = new SelectList((from MS in db.MaritalStatusMasters orderby MS.MaritalStatus select MS).ToList(), "Id", "MaritalStatus");
           
          

            
          


        }
        //forEdit

        public JsonResult BankDetails(string CountryId)
        {
            int Country = Convert.ToInt32(CountryId);
            var bank = (db.SP_BankDetails(Country)).ToList();
          return Json(new SelectList(bank.ToArray(), "Id", "Name"), JsonRequestBehavior.AllowGet);
        }
        //forEdit
        public void EBankDetails(string CountryId)
        {

            ViewBag.BankName = "";
            int Country = Convert.ToInt32(CountryId);
            ViewBag.BankName = new SelectList((db.SP_BankDetails(Country)).ToList(), "Id", "Name");

        }
    
        public JsonResult StateList(string CountryId)
        {
            if (CountryId != "")
            {
                int id = Convert.ToInt32(CountryId);

                var states = (from s in db.StateMasters
                              where s.CountryId == id
                              select s).ToList();
                return Json(new SelectList(states.ToArray(), "Id", "State"), JsonRequestBehavior.AllowGet);

            }
            else
            {
                int id = 0;
                var states = (from s in db.StateMasters
                              where s.CountryId == id
                              select s).ToList();
                return Json(new SelectList(states.ToArray(), "Id", "State"), JsonRequestBehavior.AllowGet);
            }
        }
        public void EStateList(string Country)
        {
            int id = Convert.ToInt32(Country);

            ViewBag.Province = new SelectList((from s in db.StateMasters
                                               where s.CountryId == id
                                               select new 
                                               {
                                               s.Id,
                                               s.State
                                               }).ToList(), "Id", "State");

                    
        }

        public JsonResult AcademicYearList(string CountryId)
        {

            int id = Convert.ToInt32(CountryId);

            var AcademicYear = (from s in db.AcademicYears
                                where s.CountryId == id
                                select s).ToList();

            return Json(new SelectList(AcademicYear.ToArray(), "Id", "AyId"), JsonRequestBehavior.AllowGet);
        }

        
        public void EAcademicYearList(string CountryId)
        {

            int id = Convert.ToInt32(CountryId);

            ViewBag.AcademicYear = new SelectList((from s in db.AcademicYears
                                                   where s.CountryId == id
                                                   select s).ToList(), "Id", "AyId");
         
        
        }

      
    
        public JsonResult AcademicYearListForDisbursment(string AcdemicYearDisbursementid, string countryid,string ELD)
        {
            int Cid = Convert.ToInt32(countryid);
            int Educationid = Convert.ToInt32(ELD);
            return Json(new SelectList(db.SP_AcdemicYearDisbursement(AcdemicYearDisbursementid, Cid, Educationid).ToList(), "Id", "AyId"), JsonRequestBehavior.AllowGet);
        }

        public JsonResult CityList(string Stateid)
        {
            if (Stateid != "")
            {
                int id = Convert.ToInt32(Stateid);
                var citys = (from s in db.CityMasters
                             where s.StateId == id
                             select s).ToList();
                return Json(new SelectList(citys.ToArray(), "Id", "City"), JsonRequestBehavior.AllowGet);
            }
            else
            {
                int id = 0;
                var citys = (from s in db.CityMasters
                             where s.StateId == id
                             select s).ToList();
                return Json(new SelectList(citys.ToArray(), "Id", "City"), JsonRequestBehavior.AllowGet);
            }
        }

        public void ECityList(string Stateid)
        {
            int id = Convert.ToInt32(Stateid);
            ViewBag.City = new SelectList((from s in db.CityMasters
                         where s.StateId == id
                                           select s).ToList(),"Id", "City");
       
        }

        public JsonResult CourseList()
        {

            var courseList =(from CM in db.CourseMasters
                              orderby CM.CourseName
                              select new
                              {
                                  CM.Id,
                                  CM.CourseName
                              }).ToList();

            return Json(new SelectList(courseList.ToArray(), "Id", "CourseName"), JsonRequestBehavior.AllowGet);
        }


        public void ECourseList()
        {

            ViewBag.Course = new SelectList((from CM in db.CourseMasters
                              orderby CM.CourseName
                              select new
                              {
                                  CM.Id,
                                  CM.CourseName
                              }).ToList(), "Id", "CourseName");


        }
        public JsonResult CollegeList(string CourseId, string CountryID)
        {

            int id = Convert.ToInt32(CourseId);
            int Countryid = Convert.ToInt32(CountryID);

            var collegeList = (from CM in db.CollegeMasters
                               join CCR in db.CourseCollegeRelations on CM.Id equals CCR.CollegeId
                               join COM in db.CourseMasters on CCR.CourseId equals COM.Id
                               join COUM in db.CountryMasters on CM.CountryId equals COUM.Id
                               where COM.Id == id &&  CM.CountryId == Countryid
                               orderby CM.CollegeName
                               select new { CM.Id, CM.CollegeName }).ToList();

            return Json(new SelectList(collegeList.ToArray(), "Id", "CollegeName"), JsonRequestBehavior.AllowGet);
        }

            public void ECollegeList(string CourseId, string CountryID)
        {

            int id = Convert.ToInt32(CourseId);
            int Countryid = Convert.ToInt32(CountryID);

            ViewBag.College = new SelectList((from CM in db.CollegeMasters
                               join CCR in db.CourseCollegeRelations on CM.Id equals CCR.CollegeId
                               join COM in db.CourseMasters on CCR.CourseId equals COM.Id
                               join COUM in db.CountryMasters on CM.CountryId equals COUM.Id
                               where COM.Id == id &&  CM.CountryId == Countryid
                               orderby CM.CollegeName
                                              select new { CM.Id, CM.CollegeName }).ToList(), "Id", "CollegeName");
            ViewBag.NewCollege = new SelectList((from CM in db.CollegeMasters
                                              join CCR in db.CourseCollegeRelations on CM.Id equals CCR.CollegeId
                                              join COM in db.CourseMasters on CCR.CourseId equals COM.Id
                                              join COUM in db.CountryMasters on CM.CountryId equals COUM.Id
                                              where COM.Id == id && CM.CountryId == Countryid
                                              orderby CM.CollegeName
                                              select new { CM.Id, CM.CollegeName }).ToList(), "Id", "CollegeName");


        }

        public JsonResult Fee(string CollegeId, string CourseId, string AcademicYearId1)
        {
            int COid = Convert.ToInt32(CourseId);
            int Cid = Convert.ToInt32(CollegeId);
            int AYid = Convert.ToInt32(AcademicYearId1);
         
            var Fee = (from FD in db.FeesDetails
                       join CM in db.CollegeMasters on FD.CollegeId equals CM.Id
                       join COM in db.CourseMasters on FD.CourseId equals COM.Id
                       join AY in db.AcademicYears on FD.AcademicYearId equals AY.Id
                       where COM.Id == COid && AY.Id == AYid && CM.Id == Cid
                       select FD.Fees).Sum();
            return Json(Fee, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CourseYear(string AcademicYearId,string CollegeId,string CourseId,string EducationID)
        {
            int AYid = Convert.ToInt32(AcademicYearId);
            int CollId = Convert.ToInt32(CollegeId);
            int CourId = Convert.ToInt32(CourseId);
            int ELDID = Convert.ToInt32(EducationID);
            var CourseYear = (db.SP_GetCourseYear(CollId, AYid, CourId, ELDID)).ToList();


      return Json(new SelectList(CourseYear.ToArray(), "Id", "Year"), JsonRequestBehavior.AllowGet);
        }

        public JsonResult BranchMaster(string BankId)
        {
            int Bankid = Convert.ToInt32(BankId);
            var Branch = (from FIBM in db.FinancialInstitutionBranchMasters
                          join FIM in db.FinancialInstitutionMasters on FIBM.FinancialInstituteId equals FIM.Id
                          where FIM.Id == Bankid
                          orderby FIBM.BranchName
                          select new { FIBM.Id, FIBM.BranchName }).ToList();


            return Json(new SelectList(Branch.ToArray(), "Id", "BranchName"), JsonRequestBehavior.AllowGet);
        }

        public string AssignCountryCode(string CountryId)
        {
            int id = Convert.ToInt32(CountryId);
            string strCountryCode = "";

            var C = db.CountryMasters.Where(c => c.Id.Equals(id)).FirstOrDefault();

            if (C != null)
            {
                strCountryCode = C.CountryCode;
            }
            return strCountryCode;

        }

            [Authorize]
        public ActionResult ViewEducationLoanDisbursement()
        {
            var EduLoanDis = (from ELD in db.EducationLoanDisbursments
                              join CM in db.CourseMasters on ELD.CourseId equals CM.Id
                              join CLGM in db.CollegeMasters on ELD.CollegeId equals CLGM.Id
                              join ICM in db.IndividualCustomerMasters on ELD.NId equals ICM.Pan
                              orderby ELD.Id descending
                              select new
                              {
                                  ELD.Id,
                                  ELD.NId,
                                 ELD.StudentContactNo,
                                  ELD.StudentEmailId,
                                  ELD.LoanAmount,
                                  ELD.LoanAccountDate,
                                  ELD.DisburseAmount,
                                  ELD.DisburseDate,
                                  CM.CourseName,
                                  CLGM.CollegeName,
                                  ELD.LoanAccountNo,
                                  Name = (ICM.FirstName + " " + ICM.MiddleName + " " + ICM.LastName),
                                  ICM.DateOfBirth
                              }
                   ).ToList();

            return View(EduLoanDis);
        }


        [Authorize]
        public ActionResult EditEducationLoanDisbursement(int id)
        {
            //dropList();
  
            // var EduLoanDis = "";
            EditdropList();

            var EduLoanDis = (from ELD in db.EducationLoanDisbursments
                              join CD in db.ContactDetails on ELD.NId equals CD.NID
                              join ICM in db.IndividualCustomerMasters on ELD.NId equals ICM.Pan
                              join AY in db.AcademicYears on ELD.AccademicYearId equals AY.Id
                              join CS in db.CourseMasters on ELD.CourseId equals CS.Id
                              join C in db.CollegeMasters on ELD.CollegeId equals C.Id
                              join SM in db.StateMasters on CD.StateId equals SM.Id
                              join CM in db.CountryMasters on CD.CountryId equals CM.Id
                              where ELD.Id == id
                              orderby CD.Address1 descending
                              select new EducationLoanDisbursment_Metadata
                           {
                               Id = ELD.Id,
                               NId = ELD.NId,
                               StudentContactNo = ELD.StudentContactNo,
                               StudentEmailId = ELD.StudentEmailId,
                               LoanAccountNo = ELD.LoanAccountNo,
                               LoanAmount = ELD.LoanAmount.Value,
                               LoanAccountDate = ELD.LoanAccountDate.Value,
                               DisburseAmount = ELD.DisburseAmount.Value,
                               DisburseDate = ELD.DisburseDate.Value,
                               CourseId = ELD.CourseId.Value,
                               CollegeId = ELD.CollegeId.Value,
                               GarName = ELD.GuardianName,
                               GarNID = ELD.GuardianNID,
                               RelationShipId = ELD.RelationshipId.Value,
                               GarContact = ELD.GuardianContactNo,
                               AcademicYeaId1 = ELD.AccademicYearId.Value,
                               Fee = ELD.LoanAmount.Value,
                               BalanceAmount = ELD.BalanceAmount.Value,
                               Remark = ELD.Remark,

                               Sex = ICM.Sex,
                               MarritalStatus = ICM.MaritalStatusId.Value,
                               TitleId1 = ICM.TitleId.Value,
                               FirstName = ICM.FirstName,
                               MiddleName = ICM.MiddleName,
                               LastName = ICM.LastName,
                               DateOfBirth = ICM.DateOfBirth.Value,
                               NationalityId = ICM.NationalityId.Value,

                               Address1 = CD.Address1,
                               Address2 = CD.Address2,
                               Address3 = CD.Address3,
                               CountryId1 = CD.CountryId.Value,
                               StateId1 = CD.StateId.Value,
                               Pincode = CD.Pincode,
                               CityId1 = CD.CityId.Value,

                           }).Distinct().FirstOrDefault();

                       var B = (from ELD in db.EducationLoanDisbursments
                     where ELD.Id == id
                     select
                         ELD.CourseId
                     ).FirstOrDefault();
            var CC = (from ELD in db.CourseMasters
                      where ELD.Id == B
                      select
                          ELD.CourseName
                     ).FirstOrDefault();
            ViewBag.CourseName = CC;
           

            char delimiter = '-';
            string StudentContactNo = EduLoanDis.StudentContactNo;
            string[] phrases = StudentContactNo.Split(delimiter);
            ViewBag.MobileCode = phrases[0];
            ViewBag.MobileNo = phrases[1];

            string GardianeContactNo = EduLoanDis.GarContact;
            string[] phrases1 = GardianeContactNo.Split(delimiter);
            ViewBag.MobileCode1 = phrases1[0];
            ViewBag.MobileNo1 = phrases1[1];


            ECourseList();
         
            string Country= EduLoanDis.CountryId1.ToString();
            EStateList(Country);

            EAcademicYearList(Country);


               string Province= EduLoanDis.StateId1.ToString();
               ECityList(Province);

               string CourseId = EduLoanDis.CourseId.ToString();
               ECollegeList(CourseId, Country);
               EBankDetails(Country);


               //int MarritalStatus = EduLoanDis.MarritalStatusId;
               //EMarritalStatus(MarritalStatus);

            return View(EduLoanDis);

        }

        //public void EMarritalStatus(int MarriedId)
        //{

        //    ViewBag.MarritalStatusId = new SelectList((from MS in db.MaritalStatusMasters
        //                                   where MS.Id == MarriedId
        //                                   orderby MS.MaritalStatus
        //                                   select MS).ToList(), "Id", "MaritalStatus");

           

        //}



        [Authorize]
        [HttpPost]
        public ActionResult EditEduLoanDis(SaveCollectionData SCD)
        {
            var result = false;
            decimal DisburseAmount =0;
            decimal BalanceAmount = 0; 
            int intUserId = Convert.ToInt32(Session["UserId"]);

            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string EditData = SCD.childData;
            string EditChildDataData = SCD.EduLoanchildData;
            using (TransactionScope transaction = new TransactionScope())
                {
                   
        try
            {
               if (EditData != null)
                    {
                        EducationLoanDisbursment_Metadata ChildField = serializer.Deserialize<EducationLoanDisbursment_Metadata>(EditData);
                    var EM = db.EducationLoanDisbursments.Where(x => x.Id == ChildField.Id).FirstOrDefault();

                    EM.NId = ChildField.NId;
                    EM.StudentContactNo = ChildField.code + "-" + ChildField.StudentContactNo;
                    EM.StudentEmailId = ChildField.StudentEmailId;
                    EM.CourseId = ChildField.CourseId;
                    EM.CollegeId = ChildField.CollegeId;
                    EM.DisburseAmount = ChildField.DisburseAmount;
                    EM.LoanAccountDate = ChildField.LoanAccountDate;
                    EM.LoanAmount = ChildField.LoanAmount;
                    EM.GuardianNID = ChildField.GarNID;
                    EM.GuardianName = ChildField.GarName;
                    EM.RelationshipId = ChildField.RelationId;
                    EM.AccademicYearId = ChildField.AcaYId;
                    //  EM.BalanceAmount = ChildField.BalanceAmount;
                    EM.BalanceAmount = ChildField.DisburseAmount;
                    EM.Remark = ChildField.Remark;
                    EM.GuardianContactNo = ChildField.code1 + "-" + ChildField.GarContact;
                    EM.EditDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                    EM.EditId = intUserId;
                    EM.SactionAmount = ChildField.LoanAmount;
                

                    var CD = db.ContactDetails.Where(x => x.NID == EM.NId).OrderByDescending(x => x.Address1).FirstOrDefault();
                    CD.NID = ChildField.NId;
                    CD.Address1 = ChildField.Address1;
                    CD.Address2 = ChildField.Address2;
                    CD.Address3 = ChildField.Address3;
                    CD.CityId = ChildField.CityId1;
                    CD.StateId = ChildField.StateId1;
                    CD.CountryId = ChildField.CountryId1;
                    CD.Pincode = ChildField.Pincode;
                    CD.EditDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                    CD.EditId = intUserId;
             

                    var ICM = db.IndividualCustomerMasters.Where(x => x.Pan == EM.NId).FirstOrDefault();
                    ICM.Sex = ChildField.Sex;
                    ICM.MaritalStatusId = ChildField.MarriedS;
                    ICM.TitleId = ChildField.TitleId1;
                    ICM.FirstName = ChildField.FirstName;
                    ICM.MiddleName = ChildField.MiddleName;
                    ICM.LastName = ChildField.LastName;
                    ICM.DateOfBirth = ChildField.DateOfBirth;
                    ICM.NationalityId = ChildField.NationalityId;
                    ICM.EditDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                    ICM.EditId = intUserId;
                    

                    IEnumerable<EducationLoanDisbursment_Metadata> EducationChildField = serializer.Deserialize <IEnumerable<EducationLoanDisbursment_Metadata>>(EditChildDataData);
                   
                        if (EducationChildField != null)
                        {


                            foreach (var item in EducationChildField)
                            {
                                db.SP_DeleteEduChild(ChildField.Id); 
                                 if (item != null)
                                {
                                    
                            
                                EduLoanDisbusementChild EDC = new EduLoanDisbusementChild();
                                EDC.EducationLoanDisburseId = item.DisbursementId;
                                EDC.AcademicYearId = item.AcaYId; //ELDM.AcademicYeaId;
                                EDC.Year = item.courseyear; //ELDM.courseyear;
                                EDC.DisbursementAmount = item.DFees;  // ELDM.DFees;
                                EDC.PaymentTypeId = Convert.ToInt32(item.PayMode); //ELDM.PayMode;
                                EDC.collageId = Convert.ToInt32(item.CollageID); //ELDM.Collage;

                                if (item.PayMode == "1")
                                {
                                    EDC.TranNo = item.TranNo; //ELDM.TranNo;
                                    EDC.TranDate = item.TranDate;//ELDM.TranDate;
                                    EDC.FinancialInstitutionId = Convert.ToInt32(item.Bid);//ELDM.BName;
                                    EDC.FinancialInstitutionBranchId =Convert.ToInt32(item.Brid); // ELDM.BrName;
                                }
                                else  if (item.PayMode == "2")
                                {
                                    EDC.TranNo = item.TranNo; //ELDM.TranNo;
                                    EDC.TranDate = item.TranDate;//ELDM.TranDate;
                                }
                    


                              
                                EDC.IssuedBy = item.issuedBy;
                                EDC.IssuedDate = item.issuedDate;
                                EDC.CreateId = intUserId;
                                EDC.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                                db.AddToEduLoanDisbusementChilds(EDC);

                                 

                                     DisburseAmount = DisburseAmount + item.DFees;
                                     BalanceAmount = BalanceAmount + item.DFees;


                                }

                            }
                          
                        }

                        var UpdateEM = db.EducationLoanDisbursments.Where(x => x.Id == ChildField.Id).FirstOrDefault();
                        UpdateEM.DisburseAmount = Convert.ToDecimal(DisburseAmount);
                        UpdateEM.BalanceAmount =Convert.ToDecimal(BalanceAmount);





                    }




                                       db.SaveChanges();
                    transaction.Complete();
                    result = true;
                

                }
        catch (Exception e)
        {
            transaction.Dispose();
            var a = e.Message;
        }

        return Json(result, JsonRequestBehavior.AllowGet);

            }
          
           
        }


        public ActionResult EditDisbursementDetails(int EducationId)
        {
            var strSql = (db.spGetDisbursementDetails(EducationId)).ToList();

            if (strSql != null)
            {

                return Json(strSql, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var strDisable = "true";
                return Json(strDisable);
            }
        }

        public ActionResult getDisbursementAmt(int EducationId)
        {
            var strSql = (db.spGetDisbursementAmt(EducationId)).FirstOrDefault();

            if (strSql != null)
            {

                return Json(strSql, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var strDisable = "true";
                return Json(strDisable);
            }
        }

        public ActionResult checkRepeatLoan(int CountryId, int CourseId, int Acdemicyear, string NID)
        {
            var strSql = (db.SP_checkRepeatLoan(CountryId, CourseId, Acdemicyear, NID)).FirstOrDefault();

            if (strSql != null)
            {

                return Json(strSql, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var strDisable = "";
                return Json(strDisable);
            }
        }


        public JsonResult getcourse(int CourseId)
        {

            var year=(from cm in db.CourseMasters
                      where cm.Id == CourseId
                      select cm.Duration).FirstOrDefault();
            return Json(year, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getDefaultFees(string AYID, string course, string college, string CourseYear)
        {
            int Acdemicyear=Convert.ToInt32(AYID);
            int CourseId=Convert.ToInt32(course);
                int CollegeID=Convert.ToInt32(college);
            int CourseYearId=Convert.ToInt32(CourseYear);

            var DefaultFees = (db.SP_GetDefaultFees(Acdemicyear,CourseId,CollegeID,CourseYearId).FirstOrDefault());

            if (DefaultFees != null)
            {

                return Json(DefaultFees, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var strDisable = "0.00";
                return Json(strDisable);
            }
        }

        //For Edit DisbursementDetails
        public JsonResult EditAcademicYearListForDisbursment(string AcdemicYearDisbursementid, string countryid, string ELD, int selAYID)
        {
            int Cid = Convert.ToInt32(countryid);
            int Educationid = Convert.ToInt32(ELD);
            return Json(new SelectList(db.SP_EditAcdemicYearDisbursement(AcdemicYearDisbursementid, Cid, Educationid, selAYID).ToList(), "Id", "AyId"), JsonRequestBehavior.AllowGet);
        }

        public JsonResult EditGetCourseYear(string AcademicYearId, string CollegeId, string CourseId, string EducationID, int CYr)
        {
            int AYid = Convert.ToInt32(AcademicYearId);
            int CollId = Convert.ToInt32(CollegeId);
            int CourId = Convert.ToInt32(CourseId);
            int ELDID = Convert.ToInt32(EducationID);
            var CourseYear = (db.SP_EditGetCourseYear(CollId, AYid, CourId, ELDID, CYr)).ToList();


            return Json(new SelectList(CourseYear.ToArray(), "Id", "Year"), JsonRequestBehavior.AllowGet);
        }
        public JsonResult CheckRepaymentEntry(string LoanAccountNo)
        {

            var cnt = (db.SP_CheckRepaimentCount(LoanAccountNo));


            return Json(cnt, JsonRequestBehavior.AllowGet);
        }


    }
}
