using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CreditRatingModel.Model;
using System.Data.Entity;
using System.Transactions;

namespace CreditRating.Controllers
{
    public class LoanApplicationController : Controller
    {
        creaditratingEntities db = new creaditratingEntities();

        public int intLoanId = 0;

        private void FillData()
        {
            ViewBag.LoanCounName = new SelectList(db.CountryMasters.ToList(), "Id", "Country");
            ViewBag.LoanType = new SelectList(db.LoanTypesMasters.ToList(), "Id", "Type");
            ViewBag.NmTitle = new SelectList(db.NameTitleMasters.ToList(), "Id", "Name");
            ViewBag.Nationality = new SelectList(db.NationalityMasters.ToList(), "Id", "Nationality");
            ViewBag.AddressType = new SelectList(db.AddressTypeMasters.ToList(), "Id", "AddressType");
            ViewBag.CompName = new SelectList(db.BorrowerCompanyMasters.ToList(), "Id", "CompanyName");           
            ViewBag.MaritalStatus = new SelectList(db.MaritalStatusMasters.ToList(), "Id", "MaritalStatus");
            ViewBag.Reason = new SelectList(db.ReasonMasters.ToList(), "Id", "Reason");
            ViewBag.BorrowerType = new SelectList(db.BorrowerTypeMasters.ToList(), "Id", "BorrowerType");
            ViewBag.Financial_Id = Convert.ToInt32(Session["EmployerName"]);
            ViewBag.FinancialBr_Id = Convert.ToInt32(Session["EmployerDivision"]);
            ViewBag.FinancialInstituteName = GetFinanceInfo("F", ViewBag.Financial_Id);
            ViewBag.FinancialInstituteBranchName = GetFinanceInfo("B", ViewBag.FinancialBr_Id);

            int intFinId = ViewBag.Financial_Id;
            ViewBag.CardType = new SelectList((from C in db.CardTypeMasters
                                               where C.FinancialInstituteId == intFinId
                                               orderby C.CardType
                                               select C).ToList(), "Id", "CardType");                  

        }

        [HttpPost]
        public ActionResult GetAppName(string Pan, int intBorTypeId)
        {
            var strSql = (db.SP_GetName(intBorTypeId, Pan)).FirstOrDefault();

            ViewBag.GetAppName = strSql;
            var strAppName = ViewBag.GetAppName;
            return Json(strAppName);
        }
                 
        public ActionResult GetCreditCardList(int intLoanTypeId)
        {
            var strSql = (db.spGetLoanCategory(intLoanTypeId)).FirstOrDefault();

            var strCreditCard = strSql;
            return Json(strCreditCard);
        }

        public ActionResult GetCardAmt(int intCardTypeId)
        {
            var strSql = (db.spGetCreditCardAmt(intCardTypeId)).FirstOrDefault();

            var dblCardAmt = strSql;
            return Json(dblCardAmt);
        }

        public JsonResult GetGuarantorInfo(string Pan)
        {
            FillData();

            var strSql = (from IndividualCustomerMaster_ in db.IndividualCustomerMasters
                          join ContactDetailsMaster_ in db.ContactDetailsMasters on IndividualCustomerMaster_.Id equals ContactDetailsMaster_.IndividualCustomerId
                          join ContactRelation_ in db.ContactRelations on ContactDetailsMaster_.Id equals ContactRelation_.ContactDetailsId
                          join StateMaster_ in db.StateMasters on ContactDetailsMaster_.StateId equals StateMaster_.Id
                          join CityMaster_ in db.CityMasters on ContactDetailsMaster_.CityId equals CityMaster_.Id
                          join MaritalStatusMasters_ in db.MaritalStatusMasters on IndividualCustomerMaster_.MaritalStatusId equals MaritalStatusMasters_.Id
                          where IndividualCustomerMaster_.Pan == Pan
                          select new
                          {
                              IndividualCustomerMaster_.TitleId,
                              IndividualCustomerMaster_.NationalityId,
                              IndividualCustomerMaster_.FirstName,
                              IndividualCustomerMaster_.MiddleName,
                              IndividualCustomerMaster_.LastName,
                              IndividualCustomerMaster_.Sex,
                              DateOfBirth = IndividualCustomerMaster_.DateOfBirth.Value,
                              ContactRelation_.AddressTypeId,
                              ContactDetailsMaster_.Address1,
                              ContactDetailsMaster_.Address2,
                              ContactDetailsMaster_.Address3,
                              ContactDetailsMaster_.CountryId,
                              ContactDetailsMaster_.StateId,
                              ContactDetailsMaster_.Pincode,
                              ContactDetailsMaster_.LandlineCode,
                              ContactDetailsMaster_.LandlineNo,
                              ContactDetailsMaster_.MobileCode,
                              ContactDetailsMaster_.MobileNo,
                              ContactDetailsMaster_.EmailId1,
                              ContactDetailsMaster_.EmailId2,
                              StateMaster_.State,
                              IndividualCustomerMaster_.Id,
                              IndividualCustomerMaster_.MaritalStatusId,
                              ContactDetailsMaster_.CityId,
                              CityMaster_.City
                          }
                          ).FirstOrDefault();
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

        public JsonResult GetCompanyDirectorInfo(string Pan)
        {
            var strSql = (from CDR in db.CompanyDirectorRelations
                          join ICM in db.IndividualCustomerMasters on CDR.IndividualCustomerId equals ICM.Id
                          where ICM.Pan == Pan
                          orderby CDR.DateOfJoining descending
                          select new
                          {
                              CDR.CompanyId,
                              CDR.DateOfJoining,
                              CDR.Id
                          }
                        ).FirstOrDefault();

            return Json(strSql, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEditCompanyDirectorInfo(int Id)
        {
            var strSql = (from CDR in db.CompanyDirectorRelations
                          join LIR in db.LoanIndividualRelations on CDR.Id equals LIR.CompanyDirectorRelationId
                          where LIR.Id == Id
                          orderby CDR.DateOfJoining descending
                          select new
                          {
                              CDR.CompanyId,
                              CDR.DateOfJoining,
                              CDR.Id
                          }
                        ).FirstOrDefault();

            return Json(strSql, JsonRequestBehavior.AllowGet);
        }

        public JsonResult BindState(int countryid)
        {
            var strSql = db.StateMasters.Where(s => s.CountryId == countryid).FirstOrDefault();
            return Json(strSql);
        }

        public JsonResult LoanStateList(string CountryId)
        {
            int id = Convert.ToInt32(CountryId);

            var states = (from s in db.StateMasters
                          where s.CountryId == id
                          select s);
            return Json(new SelectList(states.ToArray(), "Id", "State"), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCityList(string StateId)
        {
            int id = Convert.ToInt32(StateId);

            var city = (from C in db.CityMasters
                        where C.StateId == id
                        select C);
            return Json(new SelectList(city.ToArray(), "Id", "City"), JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoanGuarantor()
        {
            FillData();
            return View("LoanGuarantor");
        }
        [HttpGet]
        public ActionResult LoanApplication()
        {
            int userTypeId = Convert.ToInt32(Session["UserTypeId"]);
            if (userTypeId == 2)
            {
                FillData();
                
                if (ViewBag.CardType == null)
                {
                       return Content("<script language='javascript' type='text/javascript'> ; window.location.href ='/Home/LogOut'; </script>");
                }
                
                return View("LoanApplication");     //for add mode            
            }
            else
            {
                return Content(("<script language='javascript' type='text/javascript'> alert('You are not authorised person to access this module') ; window.location.href ='/Home/_Home'; </script>"));
            }           
        }

        public string GetFinanceInfo(string strType, int Id)
        {
            var strSql = (db.spFillFinanceDetails(strType, Id)).FirstOrDefault();

            return strSql.ToString();
        }

        [HttpPost]
        public ActionResult LoanApplication(LoanApplication lA)
        {            
            FillData();
            return View("LoanApplication");
        }

        public int CheckIndividualId(int intId, int intCompId, DateTime dtpJoiningDate)
        {
            var strSql = (from CDR in db.CompanyDirectorRelations
                          where CDR.IndividualCustomerId == intId
                          && CDR.CompanyId == intCompId
                          && CDR.DateOfJoining == dtpJoiningDate
                          select
                               CDR.Id
            ).FirstOrDefault();
            return strSql;
        }

        public JsonResult CheckGuarantorCount(string Pan, int Id)
        {
            if (Id == 0)
            {
                var strSql = (from LIR in db.LoanIndividualRelations
                              join ICM in db.IndividualCustomerMasters on LIR.IndividualCustomerId equals ICM.Id
                              where (ICM.Pan == Pan)
                              group ICM by ICM.Id into ICMg
                              select new
                              {
                                  CntId = ICMg.Key,
                                  Count = ICMg.Count()
                              }
                          ).FirstOrDefault();
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
            else
            {
                var strSql = (from LIR in db.LoanIndividualRelations
                              join ICM in db.IndividualCustomerMasters on LIR.IndividualCustomerId equals ICM.Id
                              where (ICM.Pan == Pan && LIR.Id != Id)
                              group ICM by ICM.Id into ICMg
                              select new
                              {
                                  CntId = ICMg.Key,
                                  Count = ICMg.Count()
                              }
                        ).FirstOrDefault();
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
        }

        public JsonResult CheckPanExists(string Pan, int intBorTypeId)
        {
            var result = true;
            var strPan = (db.spCheckPanExists(Pan, intBorTypeId)).FirstOrDefault();

            if (strPan != null)
            {
                result = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckLoanNoExists(string LoanApplicationNo, int Id, decimal FinIntId, decimal FinIntBId)
        {
            var result = false;
            Id = 0;
            if (Id == 0)
            {
                var strLoanNo = db.LoanApplicationTransactions.Where(x => x.LoanApplicationNo == LoanApplicationNo && x.FinancialInstitutionId == FinIntId && x.FinancialInstitutionBranchId == FinIntBId).FirstOrDefault();
                if (strLoanNo != null)
                {
                    result = true;
                }
            }
            else
            {
                var strLoanNo = db.LoanApplicationTransactions.Where(x => x.LoanApplicationNo == LoanApplicationNo && x.FinancialInstitutionId == FinIntId && x.FinancialInstitutionBranchId == FinIntBId && x.Id != Id).FirstOrDefault();
                if (strLoanNo != null)
                {
                    result = true;
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckSameGuarantorExistsForApplicant(string Pan, string ApplicationNo, int Id)
        {
            var result = false;

            if (Id == 0)
            {
                var strsql = (from LI in db.LoanIndividualRelations
                              join LA in db.LoanApplicationTransactions on LI.LoanApplicationId equals LA.Id
                              join IM in db.IndividualCustomerMasters on LI.IndividualCustomerId equals IM.Id
                              where LA.LoanApplicationNo == ApplicationNo
                                 && IM.Pan == Pan
                              select
                                  LI.IndividualCustomerId
                                ).FirstOrDefault();
                if (strsql != null)
                {
                    result = true;
                }
            }
            else
            {
                var strsql = (from LI in db.LoanIndividualRelations
                              join LA in db.LoanApplicationTransactions on LI.LoanApplicationId equals LA.Id
                              join IM in db.IndividualCustomerMasters on LI.IndividualCustomerId equals IM.Id
                              where LA.LoanApplicationNo == ApplicationNo
                                 && IM.Pan == Pan && LI.Id != Id
                              select
                                  LI.IndividualCustomerId
                                ).FirstOrDefault();
                if (strsql != null)
                {
                    result = true;
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CallClearFields()
        {
            var result = true;
            return Json(result);
        }
        
        public ActionResult CountryCode(string CountryId)
        {
            int id = Convert.ToInt32(CountryId);

            var CountryCode = (from CM in db.CountryMasters
                               where CM.Id == id
                               select CM.CountryCode);
            return Json(CountryCode);
        }

        public ActionResult CityCode(string CityId)
        {
            int id = Convert.ToInt32(CityId);

            var CityCode = (from CM in db.CityMasters
                            where CM.Id == id
                            select CM.CityCode);
            return Json(CityCode);
        }

        public void InsertLoanDetails(int intUserId, string strLoanNo, DateTime dtpLoanDt, int intLoanTypeId, decimal dcLoanAmount, string strPan, decimal FinancialIntituteId, decimal FinancialIntituteBranchId, int intBorrowerTypeId, int intCardTypeId)
        {
            var strSql = (from LA in db.LoanApplicationTransactions
                          where LA.LoanApplicationNo == strLoanNo && LA.Pan == strPan
                          && LA.FinancialInstitutionId == FinancialIntituteId && LA.FinancialInstitutionBranchId == FinancialIntituteBranchId
                          select
                               LA.Id
                ).FirstOrDefault();

            intLoanId = strSql;
             
            if (intLoanId == 0)
            {
                LoanApplicationTransaction LAT = new LoanApplicationTransaction();

                LAT.LoanApplicationNo = strLoanNo;
                LAT.LoanApplicationDate = dtpLoanDt;
                LAT.LoanTypesId = intLoanTypeId;
                if (intCardTypeId!=0)
                {
                    LAT.CardTypeId = intCardTypeId;    
                }                

                LAT.LoanAmount = dcLoanAmount;
                LAT.Pan = strPan;
                LAT.FinancialInstitutionId = FinancialIntituteId;
                LAT.FinancialInstitutionBranchId = FinancialIntituteBranchId;
                LAT.BorrowerTypeId = intBorrowerTypeId;
                LAT.LoanStatus="";
                LAT.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);

                LAT.CreateId = intUserId;
                db.AddToLoanApplicationTransactions(LAT);
                db.SaveChanges();

                intLoanId = LAT.Id;
            }
        }

        public void InsertGuarantorDetails(int intIndId, decimal dclGuarIndiId, int intUserId, string strPan, decimal intTitleId, decimal intNationalityId, string strFirstName, string strMiddleName, string strLastName, string strSex, DateTime dtpDateOfBirth, decimal intAddType, string strAddress1, string strAddress2, string strAddress3, decimal intCountryId, decimal intStateId, decimal intCityId, string strPincode, string strLandlineCode, string strLandlineNo, string strMobileCode, string strMobileNo, string strEmailId1, string strEmailId2, string strType, int intLId, decimal dclCompanyId, DateTime dtpDateOfJoining, int intMaritalStatusId)
        {
            if (dclGuarIndiId == 0)
            {
                IndividualCustomerMaster ICMI = new IndividualCustomerMaster();

                ICMI.FirstName = strFirstName;
                ICMI.MiddleName = strMiddleName;
                ICMI.LastName = strLastName;
                ICMI.Pan = strPan;
                ICMI.Sex = strSex;
                ICMI.DateOfBirth = dtpDateOfBirth;
                ICMI.TitleId = intTitleId;
                ICMI.NationalityId = intNationalityId;
                ICMI.MaritalStatusId = intMaritalStatusId;

                ICMI.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);

                ICMI.CreateId = intUserId;
                db.AddToIndividualCustomerMasters(ICMI);
                db.SaveChanges();

                // ** Save ContactDetailsMaster
                ContactDetailsMaster CDMI = new ContactDetailsMaster();

                CDMI.PAN = strPan;
                CDMI.IndividualCustomerId = ICMI.Id;
                CDMI.Address1 = strAddress1;
                CDMI.Address2 = strAddress2;
                CDMI.Address3 = strAddress3;
                CDMI.CountryId = intCountryId;
                CDMI.StateId = intStateId;
                CDMI.CityId = intCityId;
                CDMI.Pincode = strPincode;
                CDMI.LandlineCode = strLandlineCode;
                CDMI.LandlineNo = strLandlineNo;
                CDMI.MobileCode = strMobileCode;
                CDMI.MobileNo = strMobileNo;
                CDMI.EmailId1 = strEmailId1;
                CDMI.EmailId2 = strEmailId2;
                CDMI.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30); ;
                CDMI.CreateId = intUserId;

                db.AddToContactDetailsMasters(CDMI);
                db.SaveChanges();

                //  **** Save ContactRelation 
                ContactRelation CR = new ContactRelation();

                CR.AddressTypeId = intAddType;
                CR.IndividualCustomerId = ICMI.Id;
                CR.ContactDetailsId = CDMI.Id;
                CR.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                CR.CreateId = intUserId;

                db.AddToContactRelations(CR);
                db.SaveChanges();

                dclGuarIndiId = ICMI.Id;
            }

            if (intIndId == 0)
            {
                if (strType == "D")
                {
                    CompanyDirectorRelation CDR = new CompanyDirectorRelation();

                    CDR.CompanyId = dclCompanyId;
                    CDR.IndividualCustomerId = dclGuarIndiId;
                    CDR.DateOfJoining = dtpDateOfJoining;

                    CDR.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                    CDR.CreateId = intUserId;

                    db.AddToCompanyDirectorRelations(CDR);
                    db.SaveChanges();

                    intIndId = CDR.Id;
                }
            }

            LoanIndividualRelation LIR = new LoanIndividualRelation();

            LIR.IndividualCustomerId = dclGuarIndiId;
            LIR.Type = strType;
            LIR.LoanApplicationId = intLId;
            if ((intIndId != 0) && (strType == "D"))
            {
                LIR.CompanyDirectorRelationId = intIndId;
            }

            LIR.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
            LIR.CreateId = intUserId;
            db.AddToLoanIndividualRelations(LIR);
            db.SaveChanges();
        }

        public string GetIndividualId(string strPan)
        {
            var strSql1 = (from ICM in db.IndividualCustomerMasters
                           where ICM.Pan == strPan
                           select
                                ICM.Id
            ).FirstOrDefault();

            return strSql1.ToString();
        }

        [HttpPost]
        public ActionResult btnSave(LoanApplication LA)
        {
            int intUserId = Convert.ToInt32(Session["UserId"]);

            if (LA.PanL == null)
            {
                LA.PanL = LA.TPin;
            }
            decimal dclGuarIndiId = Convert.ToInt32(GetIndividualId(LA.ICM.Pan));   //For Loan Applicant Guarantor Id
            int intIndId = CheckIndividualId(Convert.ToInt32(dclGuarIndiId), Convert.ToInt32(LA.CDR.CompanyId), LA.CDR.DateOfJoining);

            string selectedState1 = Request.Form["selState"];
            int selectedState = Convert.ToInt32(selectedState1);

            string selectedCity1 = Request.Form["selCity"];
            int selectedCity = Convert.ToInt32(selectedCity1);

            string selectedBranch = Request.Form["drpfinIntBranch"];
            decimal decBranchId = Convert.ToInt32(selectedBranch);

            ViewBag.Financial_Id = Convert.ToInt32(Session["EmployerName"]);
            ViewBag.FinancialBr_Id = Convert.ToInt32(Session["EmployerDivision"]);
            
            decimal decFinancial_Id = ViewBag.Financial_Id;
            decimal decFinancialBr_Id = ViewBag.FinancialBr_Id;

            using (TransactionScope transaction = new TransactionScope())
            {
                try
                {
                    InsertLoanDetails(intUserId, LA.LoanApplicationNo, LA.LoanApplicationDate, LA.LoanTypesId, LA.LoanAmount, LA.PanL, decFinancial_Id, decFinancialBr_Id, LA.BorrowerTypeId, LA.CardTypeId);

                    if (LA.Type == null)
                    {
                        LA.Type = "G";
                    }
                    InsertGuarantorDetails(intIndId, dclGuarIndiId, intUserId, LA.ICM.Pan, LA.ICM.TitleId, LA.ICM.NationalityId, LA.ICM.FirstName, LA.ICM.MiddleName, LA.ICM.LastName, LA.ICM.Sex, LA.ICM.DateOfBirth, LA.CDM.AddressTypeId, LA.CDM.Address1Cont, LA.CDM.Address2Cont, LA.CDM.Address3Cont, LA.CDM.ContCountryId, selectedState, selectedCity, LA.CDM.Pincode, LA.CDM.LandlineCode, LA.CDM.LandlineNo, LA.CDM.MobileCode, LA.CDM.MobileNo, LA.CDM.EmailId1, LA.CDM.EmailId2, LA.Type, intLoanId, LA.CDR.CompanyId, LA.CDR.DateOfJoining, LA.ICM.MaritalStatusId);
                    
                    FillData();
                 
                    transaction.Complete();

                    CallClearFields();
                                        
                    return View("LoanApplication");
                }
                catch (Exception e)
                {
                    transaction.Dispose();
                    string errmsg = e.Message;
                }
            }

            FillData();            

            return View("LoanApplication", LA);
        }

        public ActionResult btnEdit(LoanApplication LA)
        {
            return View();
        }

        public ActionResult CheckLoanApproved(int Id,string StrType)
        {
            var strSql = (db.spCheckLoanApproved(Id, StrType)).FirstOrDefault();

            if (strSql != null)
            {
                return Json(strSql,JsonRequestBehavior.AllowGet);
            }
            else
            {
                var strDisable = "true";
                return Json(strDisable);
            }
        }

        public ActionResult EditLoanApplication(int intId)
        {
                FillData();
                                
                LoanApplication LA = new LoanApplication();
                IndividualCustomerMasters ICM = new IndividualCustomerMasters();
                ContactDetailsMasters CDM = new ContactDetailsMasters();

                var strSql = (db.spEditLoanApplication(intId)).ToList();

                foreach (var i in strSql)
                {
                    LA.PanL = i.PanL;
                    LA.TPin = i.PanL;
                    LA.ApplicantName = i.ApplicantName;
                    LA.LoanApplicationNo = i.LoanApplicationNo;
                    LA.LoanApplicationDate = i.LoanApplicationDate.Value;
                    LA.LoanTypesId = i.LoanTypesId.Value;
                    LA.CardTypeId = i.CardTypeId;
                    LA.LoanAmount = i.LoanAmount.Value;
                    LA.Type = i.Type;
                    LA.FinancialInstitutionId = i.FinancialInstitutionId.Value;
                    LA.FinancialInstitutionBranchId = i.FinancialInstitutionBranchId.Value;
                    LA.BorrowerTypeId = i.BorrowerTypeId.Value;
                    ICM.Pan = i.Pan;
                    ICM.TitleId = i.TitleId.Value;
                    ICM.FirstName = i.FirstName;
                    ICM.MiddleName = i.MiddleName;
                    ICM.LastName = i.LastName;
                    ICM.Sex = i.Sex;
                    ICM.DateOfBirth = i.DateOfBirth.Value;
                    ICM.NationalityId = i.NationalityId.Value;
                    ICM.Id = i.GIndCusId.Value;
                    ICM.MaritalStatusId = i.MaritalStatusId.Value;
                    CDM.AddressTypeId = i.AddressTypeId.Value;
                    CDM.Address1Cont = i.Address1Cont;
                    CDM.Address2Cont = i.Address2Cont;
                    CDM.Address3Cont = i.Address3Cont;
                    CDM.ContCountryId = i.ContCountryId.Value;
                    CDM.StateId = i.StateId.Value;
                    CDM.Pincode = i.Pincode;
                    CDM.LandlineCode = i.LandlineCode;
                    CDM.LandlineNo = i.LandlineNo;
                    CDM.MobileCode = i.MobileCode;
                    CDM.MobileNo = i.MobileNo;
                    CDM.EmailId1 = i.EmailId1;
                    CDM.EmailId2 = i.EmailId2;
                    CDM.Id = Convert.ToDecimal(i.CDMId);
                    CDM.ContRelId = Convert.ToDecimal(i.ContRelId);
                    LA.Id = i.Id;
                    LA.LIRId = i.LIRId;

                    LA.ICM = ICM;
                    LA.CDM = CDM;
                }
                return View(LA);
        }


        [HttpPost]
        public ActionResult EditAction(LoanApplication LA)
        {
            int intUserId = Convert.ToInt32(Session["UserId"]);

            decimal dclGuarIndiId = Convert.ToInt32(GetIndividualId(LA.ICM.Pan));   //For Loan Applicant Guarantor Id

            var strSql = (from LIR in db.LoanIndividualRelations
                          where LIR.LoanApplicationId == LA.Id
                          && LIR.IndividualCustomerId == dclGuarIndiId
                          select
                               LIR.Id
            ).FirstOrDefault();

            int LIRIndId = Convert.ToInt32(strSql);

            string selectedState1 = Request.Form["selState"];
            int selectedState = Convert.ToInt32(selectedState1);

            string selectedCity1 = Request.Form["selCity"];
            int selectedCity = Convert.ToInt32(selectedCity1);

            string selectedBranch = Request.Form["drpfinIntBranch"];
            decimal decBranchId = Convert.ToInt32(selectedBranch);

            if (LIRIndId == 0)  //If Guarantor added then new entries for all the tables except LoanApplicationTransactions
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    try
                    {
                        int intIndId = CheckIndividualId(Convert.ToInt32(dclGuarIndiId), Convert.ToInt32(LA.CDR.CompanyId), LA.CDR.DateOfJoining);

                        InsertGuarantorDetails(intIndId, dclGuarIndiId, intUserId, LA.ICM.Pan, LA.ICM.TitleId, LA.ICM.NationalityId, LA.ICM.FirstName, LA.ICM.MiddleName, LA.ICM.LastName, LA.ICM.Sex, LA.ICM.DateOfBirth, LA.CDM.AddressTypeId, LA.CDM.Address1Cont, LA.CDM.Address2Cont, LA.CDM.Address3Cont, LA.CDM.ContCountryId, selectedState, selectedState, LA.CDM.Pincode, LA.CDM.LandlineCode, LA.CDM.LandlineNo, LA.CDM.MobileCode, LA.CDM.MobileNo, LA.CDM.EmailId1, LA.CDM.EmailId2, LA.Type, LA.Id, LA.CDR.CompanyId, LA.CDR.DateOfJoining, LA.ICM.MaritalStatusId);

                        FillData();
                        transaction.Complete();
                        return Content(("<script language='javascript' type='text/javascript'> alert('Data Saved Successfully!'); window.location.href ='/LoanApplication/ViewLoanApplication';</script>"));
                    }
                    catch (Exception e)
                    {
                        string strErrMsg = e.Message;
                        transaction.Dispose();
                        return View("EditLoanApplication");
                    }
                }
            }
            else
            {
                var tblLoanApplicationTransactions = db.LoanApplicationTransactions.Where(x => x.Id == LA.Id).FirstOrDefault();

                var tblLoanIndividualRelation = db.LoanIndividualRelations.Where(x => x.Id == LA.LIRId).FirstOrDefault();

                var tblIndividualCustomerMaster = db.IndividualCustomerMasters.Where(x => x.Id == LA.ICM.Id).FirstOrDefault();

                var tblContactDetailsMaster = db.ContactDetailsMasters.Where(x => x.Id == LA.CDM.Id).FirstOrDefault();

                var tblContactRelation = db.ContactRelations.Where(x => x.Id == LA.CDM.ContRelId).FirstOrDefault();

                var tblCompanyDirectorRelation = db.CompanyDirectorRelations.Where(x => x.Id == LA.CDR.Id).FirstOrDefault();

                using (TransactionScope transaction = new TransactionScope())
                {
                    try
                    {
                        tblLoanIndividualRelation.Type = LA.Type;
                        tblLoanIndividualRelation.EditDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                        tblLoanIndividualRelation.EditId = intUserId;

                        tblLoanApplicationTransactions.LoanApplicationNo = LA.LoanApplicationNo;
                        tblLoanApplicationTransactions.LoanApplicationDate = LA.LoanApplicationDate;
                        tblLoanApplicationTransactions.LoanTypesId = LA.LoanTypesId;
                        tblLoanApplicationTransactions.CardTypeId = LA.CardTypeId;
                        tblLoanApplicationTransactions.LoanAmount = LA.LoanAmount;
                        tblLoanApplicationTransactions.EditDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                        tblLoanApplicationTransactions.EditId = intUserId;

                        tblContactRelation.AddressTypeId = LA.CDM.AddressTypeId;
                        tblContactRelation.EditDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                        tblContactRelation.EditId = intUserId;

                        tblContactDetailsMaster.PAN = LA.ICM.Pan;
                        tblContactDetailsMaster.Address1 = LA.CDM.Address1Cont;
                        tblContactDetailsMaster.Address2 = LA.CDM.Address2Cont;
                        tblContactDetailsMaster.Address3 = LA.CDM.Address3Cont;
                        tblContactDetailsMaster.CountryId = LA.CDM.ContCountryId;
                        tblContactDetailsMaster.StateId = selectedState;
                        tblContactDetailsMaster.CityId = selectedCity;
                        tblContactDetailsMaster.Pincode = LA.CDM.Pincode;
                        tblContactDetailsMaster.LandlineCode = LA.CDM.LandlineCode;
                        tblContactDetailsMaster.LandlineNo = LA.CDM.LandlineNo;
                        tblContactDetailsMaster.MobileCode = LA.CDM.MobileCode;
                        tblContactDetailsMaster.MobileNo = LA.CDM.MobileNo;
                        tblContactDetailsMaster.EmailId1 = LA.CDM.EmailId1;
                        tblContactDetailsMaster.EmailId2 = LA.CDM.EmailId2;
                        tblContactDetailsMaster.EditDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                        tblContactDetailsMaster.EditId = intUserId;

                        tblIndividualCustomerMaster.Pan = LA.ICM.Pan;
                        tblIndividualCustomerMaster.FirstName = LA.ICM.FirstName;
                        tblIndividualCustomerMaster.MiddleName = LA.ICM.MiddleName;
                        tblIndividualCustomerMaster.LastName = LA.ICM.LastName;
                        tblIndividualCustomerMaster.Sex = LA.ICM.Sex;
                        tblIndividualCustomerMaster.DateOfBirth = LA.ICM.DateOfBirth;
                        tblIndividualCustomerMaster.TitleId = LA.ICM.TitleId;
                        tblIndividualCustomerMaster.NationalityId = LA.ICM.NationalityId;
                        tblIndividualCustomerMaster.MaritalStatusId = LA.ICM.MaritalStatusId;
                        tblIndividualCustomerMaster.EditDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                        tblIndividualCustomerMaster.EditId = intUserId;

                        if (LA.Type == "D")
                        {
                            tblCompanyDirectorRelation.CompanyId = LA.CDR.CompanyId;
                            tblCompanyDirectorRelation.DateOfJoining = LA.CDR.DateOfJoining;
                            tblCompanyDirectorRelation.EditDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                            tblCompanyDirectorRelation.EditId = intUserId;
                        }

                        db.SaveChanges();
                        
                        FillData();
                        transaction.Complete();
                        return Content(("<script language='javascript' type='text/javascript'> alert('Data Saved Successfully!'); window.location.href ='/LoanApplication/ViewLoanApplication';</script>"));
                                                
                    }
                    catch (Exception e)
                    {
                        string strErrMsg = e.Message;

                        FillData();
                        transaction.Dispose();
                        return View("EditLoanApplication");
                    }
                }
            }            
        }


        //---------------------Darshana-----------------
        public ActionResult ViewLoanApplication()
        {
            ViewBag.Financial_Id = Convert.ToInt32(Session["EmployerName"]);
            ViewBag.FinancialBr_Id = Convert.ToInt32(Session["EmployerDivision"]);

            ViewBag.FinancialInstituteName = GetFinanceInfo("F", ViewBag.Financial_Id);
            ViewBag.FinancialInstituteBranchName = GetFinanceInfo("B", ViewBag.Financial_Id);

            decimal Financial_Id = ViewBag.Financial_Id;
            decimal FinancialBr_Id = ViewBag.FinancialBr_Id;

            var strSql = (db.spViewLoanApplication(Financial_Id, FinancialBr_Id)).ToList();
            ViewBag.List = strSql;
            return View(strSql);
        }
        //----------------------------------------------

        // ****************************** Loan Approval Start *************************************//
        public ActionResult ViewLoanApproval()
        {
            ViewBag.Financial_Id = Convert.ToInt32(Session["EmployerName"]);
            ViewBag.FinancialBr_Id = Convert.ToInt32(Session["EmployerDivision"]);

            ViewBag.FinancialInstituteName = GetFinanceInfo("F", ViewBag.Financial_Id);
            ViewBag.FinancialInstituteBranchName = GetFinanceInfo("B", ViewBag.FinancialBr_Id);

            decimal Financial_Id = ViewBag.Financial_Id;
            decimal FinancialBr_Id = ViewBag.FinancialBr_Id;

            var strSql = (db.spGetLoanApprovalDetails(0, Financial_Id, FinancialBr_Id)).ToList();
            ViewBag.List = strSql;   
            return View(strSql);
        }

        public ActionResult LoanApproval(int Id)
        {
            FillData();

            LoanApplication LA = new LoanApplication();
            
            decimal Financial_Id = ViewBag.Financial_Id;
            decimal FinancialBr_Id = ViewBag.FinancialBr_Id;

            var strSql = (db.spGetLoanApprovalDetails(Id, Financial_Id, FinancialBr_Id)).ToList();
            
            foreach (var i in strSql)
            {
                LA.PanL = i.PanL;
                LA.TPin = i.PanL;
                LA.ApplicantName = i.ApplicantName;
                LA.FinancialInstitutionId = i.FinancialInstitutionId.Value;
                LA.FinancialInstitutionBranchId = i.FinancialInstitutionBranchId.Value;
                LA.LoanApplicationNo = i.LoanApplicationNo;
                LA.LoanApplicationDate = i.LoanApplicationDate.Value;
                LA.LoanTypesId = i.LoanTypesId.Value;
                LA.CardTypeId = i.CardTypeId.Value;
                LA.LoanAmount = i.LoanAmount.Value;
                LA.LoanStatus = i.LoanStatus;           
                LA.ReasonId = i.ReasonId.Value;
                LA.BorrowerTypeId = i.BorrowerTypeId.Value;
            }
                                           
            return View(LA);
        }

        [HttpPost]
        public ActionResult LoanApprovalUpdate(LoanApplication LA)
        {
            int intUserId = Convert.ToInt32(Session["UserId"]);
            var tblLoanApplicationTransactions = db.LoanApplicationTransactions.Where(x => x.Id == LA.Id).FirstOrDefault();

            using (TransactionScope transaction = new TransactionScope())
            {
                try
                {
                    tblLoanApplicationTransactions.LoanStatus = LA.LoanStatus;
                    tblLoanApplicationTransactions.EditDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                    tblLoanApplicationTransactions.EditId = intUserId;
                    if (LA.LoanStatus == "R")
                    {
                        tblLoanApplicationTransactions.ReasonId = LA.ReasonId;
                    }
                    else 
                    {
                        tblLoanApplicationTransactions.ReasonId = 0;
                    }

                    db.SaveChanges();

                    if (LA.PanL==null)
                    {
                        LA.PanL = LA.TPin;
                    }

                    //****** To give CRS Rating points in case of Approval or Rejection
                    var strSql = (db.spLoanApplDeductionPts(LA.PanL, LA.LoanApplicationNo, LA.LoanTypesId, LA.LoanStatus, intUserId, DateTime.UtcNow.AddHours(5).AddMinutes(30))).ToString();  
                    //*****
                    
                    FillData();

                    transaction.Complete();
                    return Content(("<script language='javascript' type='text/javascript'> alert('Data Saved Successfully!'); window.location.href ='/LoanApplication/ViewLoanApproval';</script>"));
                }
                catch (Exception e)
                {
                    FillData();
                    string strErrMsg = e.Message;
                    transaction.Dispose();
                    return View("LoanApproval");
                }
            }
        }

        // ****************************** Loan Approval End *************************************//

    }
}
