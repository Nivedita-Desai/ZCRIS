using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CreditRatingModel.Model;
using System.Transactions;

namespace CreditRating.Controllers
{
    public class FinancialInstituteController : Controller
    {
        //
        // GET: /FinancialInstitute/
        private creaditratingEntities db = new creaditratingEntities();
        FinancialInstituteModel BD = new FinancialInstituteModel();

        public void DropList()
        {
            ViewBag.InstituteName = new SelectList(db.FinancialInstitutionMasters.ToList(), "Id", "Name");
            ViewBag.Category = new SelectList(db.FinancialInstitutionCategoryMasters.ToList(), "Id", "Category");
            ViewBag.Country = new SelectList(db.CountryMasters.ToList(), "Id", "Country");
            ViewBag.CreditType = new SelectList(db.FinancialInstitutionCreditTypeMasters.ToList(), "Id", "CreditType");
            ViewBag.StateList1 = new SelectList(db.StateMasters.ToList(), "Id", "State");
            ViewBag.CityList1 = new SelectList(db.CityMasters.ToList(), "Id", "City");
            ViewBag.AreaList = new SelectList(db.AreaMasters.ToList(), "Id", "Name");
        }

        public ActionResult Index()
        {
            ViewBag.CreditType = new SelectList(db.FinancialInstitutionCreditTypeMasters, "Id", "CreditType");
            return View();
        }
           [Authorize]
        public ActionResult Create()
        {
            FinancialInstituteModel BD = new FinancialInstituteModel();
            DropList();
          
            return View(BD);
        }

        [Authorize]
           public ActionResult CreateInstitute()
           {
               FinancialInstituteModel BD = new FinancialInstituteModel();
               DropList();
               
               return View(BD);
           }

        [Authorize]
        [HttpPost]
        public ActionResult CreateInstitute(FinancialInstituteModel objFinancialInstituteModel)
        {
            string msg;
            using (TransactionScope transaction = new TransactionScope())
            {
                try
                {
                    AddInstitute(objFinancialInstituteModel);
                    transaction.Complete();
                    msg = "Data saved successfully!";
                }
                catch (Exception)
                {
                    transaction.Dispose();
                    msg = "Data saved Failed!";
                }
            }
            
           // FinancialInstituteModel BD = new FinancialInstituteModel();
            
            DropList();
            return Content(("<script language='javascript' type='text/javascript'> alert('" + msg + "'); window.location.href ='/FinancialInstitute/CreateInstitute';  </script>")); 
            //return View(objFinancialInstituteModel);
        }
        

        
        
        public IList<StateMaster> BindState(int countryid)
        {
            ////return objcategory.state = GetState(countryid);

            return db.StateMasters.Where(s => s.CountryId == countryid).ToList();

        }

        public JsonResult StateList(string CountryId)
        {
            int id = Convert.ToInt32(CountryId);

            var states = (from s in db.StateMasters
                          where s.CountryId == id
                          select s);
            //return Json(new SelectList(states.ToArray(), objCategory.Id.ToString(), objCategory.State), JsonRequestBehavior.AllowGet);
            return Json(new SelectList(states.ToArray(), "Id", "State"), JsonRequestBehavior.AllowGet);

        }

        //[HttpPost]
        //public ActionResult CountryCode(string CountryId)
        //{
        //    int id = Convert.ToInt32(CountryId);

        //    var CountryCode = (from s in db.CountryMasters
        //                       where s.Id == id
        //                       select s.CountryCode);
        //    return Json(CountryCode);
        //    //return Json(CountryCode, JsonRequestBehavior.AllowGet);
        //}
   //[Authorize]
   //[HttpPost]
        //public ActionResult btnAddInstituteClick(FinancialInstituteModel BD)
        //{
            
            //var CreateUserId1 = Session["UserId"];
            
                    //if (ModelState.IsValid)
                    //{
                    //    try
                    //    {
                    //        string selectedState1 = Request.Form["selState"];
                    //        int selectedState = Convert.ToInt32(selectedState1);
                    //        string selectedCity1 = Request.Form["selCity"];
                    //        int selectedcity1 = Convert.ToInt32(selectedCity1);
                    //        //InsertInfo1(BD.CategoryId, BD.Name, BD.BankCode, BD.RegistrationNo, BD.BranchName, BD.BranchCode, BD.SwiftCode, BD.FinancialInstituteContact, BD.Address1, BD.Address2, BD.Address3, BD.CountryId, selectedState, selectedcity1, BD.Pincode, BD.Code, BD.ContactNo, BD.FinancialInstituteEmailId1, BD.ContactPerson1, BD.ContactPerson2, BD.ContactPerson1Mobile, BD.ContactPerson2Mobile, BD.ContactPerson1EmailId, BD.ContactPerson2EmailId, BD.FinancialInstituteCreditTypeId, CreateUserId);
                    //    }
                    //    catch (Exception e)
                    //    {
                    //        string Message = e.Message;
                    //    }
    //                        string selectedCreditType1 = Request.Form["CreditType1"];                  
    //                        //int selectedCreditType = Convert.ToInt32(selectedCreditType1);
    //                        int[] ints = selectedCreditType1.Split(',').Select(int.Parse).ToArray();
    //                        for (int i = 0; i < ints.Length; ++i)   
    //{
    //    int selectedCreditType = Convert.ToInt32(ints[i]);
    //    InsertInfo2(selectedCreditType);
    //  }                     
                //ViewBag.Message = "Data Saved";
                //ModelState.Clear();
                //DropList(BD);
                //return View("Create");
                    //}
                   // DropList(BD);
                    

                  //  return View("");          
        //}

   //private void InsertInfo1(int CategoryId, string Name, string BankCode, string RegistrationNo, string BranchName, string BranchCode, string SwiftCode, string FinancialInstituteContact, string Address1, string Address2, string Address3, decimal CountryId, decimal StateId, decimal CityID ,   string Pincode, string Code, string ContactNo, string FinancialInstituteEmailId1, string ContactPerson1, string ContactPerson2, string ContactPerson1Mobile, string ContactPerson2Mobile, string ContactPerson1EmailId, string ContactPerson2EmailId, List<int> CreditType, int CreateId)
   //     {
   //         //FinancialInstituteModel i = new FinancialInstituteModel();
   //         Add(CategoryId, Name, BankCode, RegistrationNo, BranchName, BranchCode, SwiftCode, FinancialInstituteContact, Address1, Address2, Address3, CountryId, StateId, CityID, Pincode, Code, ContactNo, FinancialInstituteEmailId1, ContactPerson1, ContactPerson2, ContactPerson1Mobile, ContactPerson2Mobile, ContactPerson1EmailId, ContactPerson2EmailId, CreditType, CreateId);
   //     }
        //private void InsertInfo2(int CreditTypeId)
        //{
        //    Add1(CreditTypeId);
        //}
        //public void Add1(int CreditTypeId)
        //{
        //    FinancialInstitutionCreditTypeRelation FICTR = new FinancialInstitutionCreditTypeRelation();
        //    FICTR.FinancialInstituteId = ViewBag.FIMId;
        //    FICTR.FinancialInstituteBranchId = ViewBag.FIBMId;
        //    FICTR.FinancialInstituteCreditTypeId = CreditTypeId;

        //    db.AddToFinancialInstitutionCreditTypeRelations(FICTR);

        //    db.SaveChanges();
        //}
        public ActionResult AddInstitute(FinancialInstituteModel BD)
        {
            try
            {
                string selectedState1 = Request.Form["selState"];
                string selectedCity1 = Request.Form["selCity"];
                string selectedArea = Request.Form["selArea"];

                int selectedState = Convert.ToInt32(selectedState1);
                int selectedcity1 = Convert.ToInt32(selectedCity1);
                int AreaId = Convert.ToInt32(selectedArea);
                int CreateUserId = Convert.ToInt32(Session["UserId"]);

                FinancialInstitutionMaster FIM = new FinancialInstitutionMaster();

                FIM.FinancialInstituteCategoryId = BD.CategoryId;
                FIM.Name = BD.Name;
                FIM.RegistrationNo = BD.RegistrationNo;
                FIM.BankCode = BD.BankCode;
                FIM.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                FIM.CreateId = CreateUserId;

                db.AddToFinancialInstitutionMasters(FIM);
                db.SaveChanges();

                FinancialInstitutionBranchMaster FIBM = new FinancialInstitutionBranchMaster();
                FIBM.FinancialInstituteId = FIM.Id;
                FIBM.BranchName = BD.BranchName;
                FIBM.BranchCode = BD.BranchCode;
                FIBM.SwiftCode = BD.SwiftCode;
                FIBM.AreaId = AreaId;
                FIBM.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                FIBM.CreateId = CreateUserId;

                db.AddToFinancialInstitutionBranchMasters(FIBM);
                db.SaveChanges();

                FinancialInstitutionContactMaster FICOM = new FinancialInstitutionContactMaster();
                FICOM.FinancialInstitutionId = FIM.Id;
                FICOM.FinancialInstituteBranchId = FIBM.Id;
                FICOM.FinancialInstituteContact = BD.ContactNo;
                FICOM.Address1 = BD.Address1;

                if (BD.Address2 != null)
                {
                    FICOM.Address2 = BD.Address2;
                }

                if (BD.Address3 != null)
                {
                    FICOM.Address3 = BD.Address3;
                }

                FICOM.CountryId = BD.CountryId;
                FICOM.StateId = selectedState;
                FICOM.CityId = selectedcity1;
                FICOM.Pincode = BD.Pincode;
                FICOM.Code = BD.Code;
                FICOM.ContactNo = BD.ContactNo;
                FICOM.FinancialInstituteEmailId1 = BD.FinancialInstituteEmailId1;
                FICOM.ContactPerson1 = BD.ContactPerson1;
                FICOM.ContactPerson1Mobile = BD.ContactPerson1Mobile;
                FICOM.ContactPerson1EmailId = BD.ContactPerson1EmailId;

                if (BD.ContactPerson2 != null)
                {
                    FICOM.ContactPerson2 = BD.ContactPerson2;
                }
                if (BD.ContactPerson2EmailId != null)
                {
                    FICOM.ContactPerson2EmailId = BD.ContactPerson2EmailId;
                }
                if (BD.ContactPerson2Mobile != null)
                {
                    FICOM.ContactPerson2Mobile = BD.ContactPerson2Mobile;
                }

                FICOM.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                FICOM.CreateId = CreateUserId;

                db.AddToFinancialInstitutionContactMasters(FICOM);
                db.SaveChanges();

                foreach (var item in BD.FinancialInstituteCreditTypeId)
                {
                    FinancialInstitutionCreditTypeRelation FICTR = new FinancialInstitutionCreditTypeRelation();
                    FICTR.FinancialInstituteId = FIM.Id;
                    FICTR.FinancialInstituteBranchId = FIBM.Id;
                    FICTR.FinancialInstituteCreditTypeId = item;
                    FICTR.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                    FICTR.CreateId = CreateUserId;
                    db.AddToFinancialInstitutionCreditTypeRelations(FICTR);
                    db.SaveChanges();
                }

                ViewBag.FIMId = FIM.Id;
                ViewBag.FIBMId = FIBM.Id;
            }
            catch (Exception)
            {
                
                throw;
            }
            
            return View("CreateInstitute");
        }
        public JsonResult doesNameExist(string Name)
        {
            FinancialInstituteModel i = new FinancialInstituteModel();
            //var Name = ContactIDValue;
            var result = true;
            var user = db.FinancialInstitutionMasters.Where(x => x.Name == Name).FirstOrDefault();
            if (user != null)
            {
                result = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult doesRegistrationNoExist(string RegistrationNo)
        {
            FinancialInstituteModel i = new FinancialInstituteModel();
            //var Name = ContactIDValue;
            var result = true;
            var user = db.FinancialInstitutionMasters.Where(x => x.RegistrationNo == RegistrationNo).FirstOrDefault();
            if (user != null)
            {
                result = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult doesUserNameExist(string UserName)
        {
            FinancialInstituteModel i = new FinancialInstituteModel();
            //var Name = ContactIDValue;
            var result = true;
            var user = db.FinancialInstitutionMasters.Where(x => x.UserName == UserName).FirstOrDefault();
            if (user != null)
            {
                result = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult doesBankCodeExist(string BankCode)
        {
            FinancialInstituteModel i = new FinancialInstituteModel();
            //var Name = ContactIDValue;
            var result = true;
            var user = db.FinancialInstitutionMasters.Where(x => x.BankCode == BankCode).FirstOrDefault();
            if (user != null)
            {
                result = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult doesBranchCodeExist(string BranchCode)
        {
            FinancialInstituteModel i = new FinancialInstituteModel();
            //var Name = ContactIDValue;
            var result = true;
            var user = db.FinancialInstitutionBranchMasters.Where(x => x.BranchCode == BranchCode).FirstOrDefault();
            if (user != null)
            {
                result = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult doesSwiftCodeExist(string SwiftCode)
        {
            FinancialInstituteModel i = new FinancialInstituteModel();
            //var Name = ContactIDValue;
            var result = true;
            var user = db.FinancialInstitutionBranchMasters.Where(x => x.SwiftCode == SwiftCode).FirstOrDefault();
            if (user != null)
            {
                result = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult ViewInstitute()
        {
            using (creaditratingEntities db = new creaditratingEntities())
            {
                var CreateUserId1 = Session["UserId"];
                int CreateUserId = Convert.ToInt32(CreateUserId1);

                var L = (from FIBM in db.FinancialInstitutionBranchMasters
                         join FIM in db.FinancialInstitutionMasters on FIBM.FinancialInstituteId equals FIM.Id
                         join FICM in db.FinancialInstitutionCategoryMasters on FIM.FinancialInstituteCategoryId equals FICM.Id
                          orderby  FIM.Name,FIBM.BranchName
                         //where FIBM.Id == 1
                         select new
                         {

                             FIBM.Id,
                             FIBM.BranchName,
                             FIM.Name,
                             FIM.RegistrationNo,
                             FIM.BankCode,
                             FIBM.BranchCode,
                             FIBM.SwiftCode,
                             FICM.Category
                         }).ToList();

                ViewData["a"] = L;

                var c = (from FICT in db.FinancialInstitutionCreditTypeRelations
                         select new
                         {

                             FICT.FinancialInstituteCreditTypeId
                         });
                ViewBag.see = c.ToList();
                //int Count1 = Convert.ToInt32(L.Count());
                //for (int i = 0; i < Count1; ++i)
                //{

                //}       

                return View(L);

            }
        }

        [Authorize]
        public ActionResult ViewDetails(int id)
        {
            using (creaditratingEntities db = new creaditratingEntities())
            {

                var L = (from FIBM in db.FinancialInstitutionBranchMasters
                         join FIM in db.FinancialInstitutionMasters on FIBM.FinancialInstituteId equals FIM.Id
                         join FICM in db.FinancialInstitutionCategoryMasters on FIM.FinancialInstituteCategoryId equals FICM.Id
                         join FICOM in db.FinancialInstitutionContactMasters on FIBM.Id equals FICOM.FinancialInstituteBranchId
                        
                         join SM in db.StateMasters on FICOM.StateId equals SM.Id
                         join CM in db.CountryMasters on FICOM.CountryId equals CM.Id
                         where FIBM.Id == id
                         select new FinancialInstituteModel
                         {
                             Id = id,
                             Name = FIM.Name,
                             RegistrationNo = FIM.RegistrationNo,
                             CategoryId2 = FIM.FinancialInstituteCategoryId.Value,
                             BankCode = FIM.BankCode,
                             BranchName = FIBM.BranchName,
                             BranchCode = FIBM.BranchCode,
                             SwiftCode = FIBM.SwiftCode,
                             IfscCode=FIBM.IfscCode,
                             CreateDate = FIBM.CreateDate ?? DateTime.Now,
                             Address1 = FICOM.Address1,
                             Address2 = FICOM.Address2,
                             Address3 = FICOM.Address3,
                             CountryId = FICOM.CountryId.Value,
                             AreaId = FIBM.AreaId.Value,
                             StateId = FICOM.StateId.Value,
                             cityid = FICOM.CityId.Value,
                             Pincode = FICOM.Pincode,
                             Code = FICOM.Code,
                             ContactNo = FICOM.ContactNo,
                             //UserName=FIM.UserName,
                             //Password=FIM.Password,
                             //PersonFullName=FIM.PersonFullName,
                             FinancialInstituteEmailId1 = FICOM.FinancialInstituteEmailId1,
                             FinancialInstituteContact = FICOM.FinancialInstituteContact,
                             ContactPerson1 = FICOM.ContactPerson1,
                             ContactPerson2 = FICOM.ContactPerson2,
                             ContactPerson2Mobile = FICOM.ContactPerson2Mobile,
                             Code1=FICOM.Code,
             
                             ContactPerson1Mobile = FICOM.ContactPerson1Mobile,
                             ContactPerson1EmailId = FICOM.ContactPerson1EmailId,
                             ContactPerson2EmailId = FICOM.ContactPerson2EmailId
                         }).FirstOrDefault(); ;

                //decimal c = (from FICOM in db.FinancialInstitutionContactMasters
                //             where FICOM.FinancialInstituteBranchId == id
                //             select FICOM.CountryId).FirstOrDefault() ?? 0;

                //decimal s = (from FICOM in db.FinancialInstitutionContactMasters
                //             where FICOM.FinancialInstituteBranchId == id
                //             select FICOM.StateId).FirstOrDefault() ?? 0;

                //decimal FI = (from F in db.FinancialInstitutionBranchMasters
                //              where F.Id == id
                //              select F.FinancialInstituteId).FirstOrDefault() ?? 0;

                //decimal CA = (from FIM in db.FinancialInstitutionMasters
                //              where FIM.Id == FI
                //              select FIM.FinancialInstituteCategoryId).FirstOrDefault() ?? 0;

                ////var c = db.FinancialInstitutionContactMasters.Where(a => a.CountryId == id).Select(a=>a.CountryId);
                ////int f = c;
                //ViewBag.CountryList = new SelectList(db.CountryMasters.ToList(), "Id", "Country", c);
                //ViewBag.StateList1 = new SelectList(db.StateMasters.ToList(), "Id", "State", s);
                //ViewBag.Category = new SelectList(db.FinancialInstitutionCategoryMasters.ToList(), "Id", "Category", CA);

                DropList();
                return View(L);
            }
        }
        
        [Authorize]
        public ActionResult Edit(FinancialInstituteModel BD)
        {
            var CreateUserId1 = Session["UserId"];
            int CreateUserId = Convert.ToInt32(CreateUserId1);

            creaditratingEntities db = new creaditratingEntities();
            try
            {
                var tblProd = db.FinancialInstitutionBranchMasters.Where(x => x.Id == BD.Id).FirstOrDefault();
                var tblProd1 = db.FinancialInstitutionContactMasters.Where(x => x.FinancialInstituteBranchId == BD.Id).FirstOrDefault();
                decimal b = (from s in db.FinancialInstitutionContactMasters
                             where s.FinancialInstituteBranchId == BD.Id
                             select s.FinancialInstitutionId).FirstOrDefault() ?? 0;

                var tblProd2 = db.FinancialInstitutionMasters.Where(x => x.Id == b).FirstOrDefault();


               

                tblProd.BranchName = BD.BranchName;
                tblProd.BranchCode = BD.BranchCode;
                tblProd.AreaId = BD.AreaId;
                tblProd.EditDate = DateTime.UtcNow.Date;
                tblProd.EditId = CreateUserId;
                tblProd1.Address1 = BD.Address1;
                tblProd1.Address2 = BD.Address2;
                tblProd1.Address3 = BD.Address3;
                tblProd1.CityId =BD.cityid;
                tblProd1.CountryId = BD.CountryId;
                tblProd1.StateId = BD.StateId;
                tblProd1.Pincode = BD.Pincode;
                tblProd1.Code = BD.Code;
                tblProd1.ContactNo = BD.ContactNo;
                tblProd1.FinancialInstituteEmailId1 = BD.FinancialInstituteEmailId1;
                tblProd1.ContactPerson1 = BD.ContactPerson1;
                tblProd1.ContactPerson2 = BD.ContactPerson2;
                tblProd1.ContactPerson1Mobile = BD.ContactPerson1Mobile;
                tblProd1.ContactPerson2Mobile = BD.ContactPerson2Mobile;
                tblProd1.ContactPerson1EmailId = BD.ContactPerson1EmailId;
                tblProd1.ContactPerson2EmailId = BD.ContactPerson2EmailId;
                tblProd1.EditDate = DateTime.UtcNow.Date;
                tblProd1.EditId = CreateUserId;
                tblProd2.Name = BD.Name;
                //tblProd2.PersonFullName = BD.PersonFullName;
                //tblProd2.UserName = BD.UserName;
                //tblProd2.Password = BD.Password;
                tblProd2.RegistrationNo = BD.RegistrationNo;
                tblProd2.BankCode = BD.BankCode;
                tblProd.SwiftCode = BD.SwiftCode;
                tblProd.IfscCode = BD.IfscCode;
                tblProd2.EditDate = DateTime.UtcNow.Date;
                tblProd2.EditId = CreateUserId;

                db.SaveChanges();
            }
            catch(Exception e)
            {
                string Message = e.Message;
            }
            //return Json(tblProd, JsonRequestBehavior.AllowGet);
            return Content(("<script language='javascript' type='text/javascript'> alert('Data saved successfully!'); window.location.href ='/FinancialInstitute/ViewInstitute';  </script>"));

           // return RedirectToAction("");
        }

        //---akshata 08-10-2015---//
        public IList<CityMaster> BindCity(int selstate)
        {
            return db.CityMasters.Where(cm => cm.StateId == selstate).ToList();
        }


        public JsonResult CityList(string selstate)
        {

            int id = Convert.ToInt32(selstate);

            var states = (from s in db.CityMasters
                          where s.StateId == id
                          select s);

            return Json(new SelectList(states.ToArray(), "Id", "City"), JsonRequestBehavior.AllowGet);
        }

        public JsonResult AreaList(int selcity)
        {

            int id = Convert.ToInt32(selcity);

            var area = (from s in db.AreaMasters
                          where s.CityId == id
                          select s);

            return Json(new SelectList(area.ToArray(), "Id", "Name"), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string CityCode(string selCity)
        {


            int id = Convert.ToInt32(selCity);
            string strCountryCode = "";

            var C = db.CountryMasters.Where(c => c.Id.Equals(id)).FirstOrDefault();

            if (C != null)
            {
                strCountryCode = C.CountryCode;
            }
            return strCountryCode;
        }
        public ActionResult CountryCode(string countryid)
        {
            int id = Convert.ToInt32(countryid);

            var countryId = (from s in db.CountryMasters
                             where s.Id == id
                             select s.CountryCode);
            return Json(countryId);

        }
        public ActionResult viewBranch()
        {
            var branch = (from FIBM in db.FinancialInstitutionBranchMasters
                          join im in db.FinancialInstitutionMasters on FIBM.FinancialInstituteId equals im.Id
                     orderby FIBM.BranchName,im.Name
                          select new
                          {
                              FIBM.Id,
                              FIBM.BranchName,
                              FIBM.BranchCode,
                              im.Name

                          }).ToList();


            return View(branch);
        }

        public ActionResult editBranch(int id)
        {

            var branch = (from FIBM in db.FinancialInstitutionBranchMasters
                          join im in db.FinancialInstitutionMasters on FIBM.FinancialInstituteId equals im.Id
                          join cm in db.FinancialInstitutionContactMasters on FIBM.Id equals cm.FinancialInstituteBranchId

                          where FIBM.Id == id
                          select new FinancialInstituteModel 
                          {

                           Id1= FIBM.Id,
                       
                          BranchName=FIBM.BranchName,
                           FinancialInstituteId1 = im.Id,
                           BranchCode=FIBM.BranchCode,
                            Name=  im.Name,
                            IfscCode=FIBM.IfscCode,
                            SwiftCode=FIBM.SwiftCode,
                            Address1 = cm.Address1,
                           Address2 =  cm.Address2,
                           Address3 =  cm.Address3,
                            cityid=  cm.CityId.Value,
                         CountryId=     cm.CountryId.Value,
                         AreaId=FIBM.AreaId.Value,
                         StateId=     cm.StateId.Value,
                            Pincode=  cm.Pincode,
                           Code=   cm.Code,
                           ContactNo=   cm.ContactNo,
                           FinancialInstituteEmailId1=   cm.FinancialInstituteEmailId1,
                           ContactPerson1=   cm.ContactPerson1,
                          ContactPerson2=    cm.ContactPerson2,
                        ContactPerson1Mobile=      cm.ContactPerson1Mobile,
                             ContactPerson2Mobile= cm.ContactPerson2Mobile,
                          ContactPerson1EmailId=    cm.ContactPerson1EmailId,
                        ContactPerson2EmailId=      cm.ContactPerson2EmailId


                          }).FirstOrDefault();

            ViewBag.CountryList = new SelectList(db.CountryMasters.ToList(), "Id", "Country", branch.CountryId);

            var stateList=(from e in db.StateMasters where e.CountryId == branch.CountryId select new {e.Id,e.State});
            ViewBag.StateList1 = new SelectList(stateList.ToList(), "Id", "State", branch.StateId);

            var CityList = (from e in db.CityMasters where e.StateId == branch.StateId select new { e.Id, e.City });
            ViewBag.CityList1 = new SelectList(CityList.ToList(), "Id", "City", branch.cityid);

            var Area = (from e in db.AreaMasters where e.CityId == branch.cityid select new { e.Id, e.Name });
            ViewBag.AreaList = new SelectList(Area.ToList(), "Id", "Name", branch.AreaId);

            var creditTypelist = (from e in db.FinancialInstitutionCreditTypeRelations 
                              join f in db.FinancialInstitutionCreditTypeMasters on e.FinancialInstituteCreditTypeId equals f.Id
                                  where e.FinancialInstituteId == branch.FinancialInstituteId1 && e.FinancialInstituteBranchId == branch.Id1
                                  select new {f.Id}).ToList();

           // var AllCreditType=(from e in db.FinancialInstitutionCreditTypeMasters select new )
            ViewBag.CreditType = new MultiSelectList(db.FinancialInstitutionCreditTypeMasters.ToList(), "Id", "CreditType", new [] {creditTypelist});
            return View(branch);
        }


        public ActionResult SaveEditBranch(FinancialInstituteModel FIM) 
        {
            string msg;
            using (TransactionScope transaction = new TransactionScope())
            {
                try
                {
                    var bb = db.FinancialInstitutionBranchMasters.Where(x => x.Id == FIM.Id1).FirstOrDefault();
                    var b = (from ib in db.FinancialInstitutionBranchMasters
                             join cm in db.FinancialInstitutionContactMasters on ib.Id equals cm.FinancialInstituteBranchId
                             where ib.Id == FIM.Id1
                             select cm.Id).FirstOrDefault();

                    var c = db.FinancialInstitutionContactMasters.Where(x => x.Id == b).FirstOrDefault();
                    bb.AreaId = FIM.AreaId;
                    bb.IfscCode = FIM.IfscCode;
                    bb.SwiftCode = FIM.SwiftCode;
                    c.Address1 = FIM.Address1;
                    c.Address2 = FIM.Address2;
                    c.Address3 = FIM.Address3;
                    c.CountryId = FIM.CountryId;
                    c.CityId = FIM.cityid;
                    c.StateId = FIM.StateId;
                    c.Pincode = FIM.Pincode;
                    c.Code = FIM.Code;
                    c.ContactNo = FIM.ContactNo;
                    c.FinancialInstituteEmailId1 = FIM.FinancialInstituteEmailId1;
                    c.ContactPerson1 = FIM.ContactPerson1;

                    c.ContactPerson2 = FIM.ContactPerson2;
                    c.ContactPerson1EmailId = FIM.FinancialInstituteEmailId1;
                    c.ContactPerson2EmailId = FIM.ContactPerson2EmailId;
                    c.ContactPerson1Mobile = FIM.ContactPerson1Mobile;
                    c.ContactPerson2Mobile = FIM.ContactPerson2Mobile;

                    db.SaveChanges();
                    transaction.Complete();
                    msg = "Data saved successfully!";
                }
                catch (Exception)
                {
                    transaction.Dispose();
                    msg = "Data saved Failed!";
                }
            }
                      

            return Content(("<script language='javascript' type='text/javascript'> alert('" + msg + "'); window.location.href ='/FinancialInstitute/viewBranch';  </script>"));

        
        }

        //-----------------------Nivedita--------------------
        [Authorize]
        [HttpGet]
        public ActionResult CreateBranch()
        {
            FinancialInstitutionContactMaster_Metadata objFICM = new FinancialInstitutionContactMaster_Metadata();

            FinancialInstituteModel BD = new FinancialInstituteModel();
            DropList();

            return View(objFICM);
        }

        [HttpPost]
        public ActionResult CreateBranch(FinancialInstitutionContactMaster_Metadata FICM)
        {
            //FinancialInstituteModel BD = new FinancialInstituteModel();


            string msg;
            using (TransactionScope transaction = new TransactionScope())
            {
                try
                {
                    AddBranch(FICM);
                    transaction.Complete();
                    msg = "Data saved successfully!";
                }
                catch (Exception)
                {
                    transaction.Dispose();
                    msg = "Data saved Failed!";
                }
            }

            DropList();
            return Content(("<script language='javascript' type='text/javascript'> alert('" + msg + "'); window.location.href ='/FinancialInstitute/CreateBranch';  </script>"));

        }

        [HttpPost]
        public ActionResult AddBranch(FinancialInstitutionContactMaster_Metadata FCM)
        {
            //InsertInfo(FICM.FIBM.BranchName, 2, FICM.FIM.Id, FICM.Address1, FICM.Address2, FICM.Address3, FICM.ContactNo, FICM.CountryId, stateId, FICM.City, FICM.Pincode, FICM.Code, FICM.FinancialInstituteEmailId1, FICM.ContactPerson1, FICM.ContactPerson1Mobile, FICM.ContactPerson2, FICM.ContactPerson2Mobile, FICM.ContactPerson1EmailId, FICM.ContactPerson2EmailId);
            FinancialInstituteModel BD = new FinancialInstituteModel();

            
                int CreateUserId = Convert.ToInt32(Session["UserId"]);
                string selectedState = Request.Form["selState"];
                int stateId1 = Convert.ToInt32(selectedState);
                string selectedCity = Request.Form["selCity"];
                int selectedCitys = Convert.ToInt32(selectedCity);
                string area = Request.Form["selArea"];
                int selectedArea = Convert.ToInt32(area);

                FinancialInstitutionBranchMaster FIBM = new FinancialInstitutionBranchMaster();
                FIBM.BranchName = FCM.BranchName;
                FIBM.BranchCode = FCM.BranchCode;
                FIBM.AreaId = selectedArea;
                FIBM.IfscCode = FCM.IfscCode;
                FIBM.SwiftCode = FCM.SwiftCode;
                FIBM.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                    
                FIBM.CreateId = CreateUserId;
                FIBM.FinancialInstituteId = FCM.FinancialInstituteId;

                db.AddToFinancialInstitutionBranchMasters(FIBM);
                db.SaveChanges();

                FinancialInstitutionContactMaster FICM = new FinancialInstitutionContactMaster();
                FICM.FinancialInstituteBranchId = FIBM.Id;
                FICM.Address1 = FCM.Address1;
                FICM.Address2 = FCM.Address2;
                FICM.Address3 = FCM.Address3;
           
                FICM.CountryId = FCM.CountryId;
                FICM.StateId = stateId1;
                FICM.ContactNo = FCM.ContactNo;
                FICM.CityId = selectedCitys;
                FICM.Pincode = FCM.Pincode;
                FICM.Code = FCM.Code;
                FICM.FinancialInstituteEmailId1 = FCM.FinancialInstituteEmailId1;
                FICM.ContactPerson1 = FCM.ContactPerson1;
                FICM.ContactPerson1Mobile = FCM.ContactPerson1Mobile;
                FICM.ContactPerson1EmailId = FCM.ContactPerson1EmailId;
                FICM.ContactPerson2 = FCM.ContactPerson2;
                FICM.ContactPerson2Mobile = FCM.ContactPerson2Mobile;
                FICM.ContactPerson2EmailId = FCM.ContactPerson2EmailId;
                FICM.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);

                FICM.CreateId = CreateUserId;
                FICM.FinancialInstitutionId = FCM.FinancialInstituteId;

                db.AddToFinancialInstitutionContactMasters(FICM);
                db.SaveChanges();


         

                foreach (var item in FCM.FinancialInstituteCreditTypeId)
                {
                    FinancialInstitutionCreditTypeRelation FICTR = new FinancialInstitutionCreditTypeRelation();
                    FICTR.FinancialInstituteId = FCM.FinancialInstituteId;
                    FICTR.FinancialInstituteBranchId = FIBM.Id;
                    FICTR.FinancialInstituteCreditTypeId = item;
                    FICTR.CreateId = CreateUserId;
                    FICTR.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                    db.AddToFinancialInstitutionCreditTypeRelations(FICTR);
                    db.SaveChanges();
                }

           

        
               //InsertInfo(FICM.BranchName, CreateUserId, FICM.FinancialInstituteId, FICM.Address1, FICM.Address2, FICM.Address3, FICM.ContactNo, FICM.CountryId, stateId1, selectedCitys, FICM.Pincode, FICM.Code, FICM.FinancialInstituteEmailId1, FICM.ContactPerson1, FICM.ContactPerson1Mobile, FICM.ContactPerson2, FICM.ContactPerson2Mobile, FICM.ContactPerson1EmailId, FICM.ContactPerson2EmailId, FICM.FinancialInstituteCreditTypeId, FICM.BranchCode);
                ViewBag.Message = "Data Saved";
                ModelState.Clear();

                return View("CreateBranch");

            }
           
            //return Content(("<script language='javascript' type='text/javascript'> alert('Data saved successfully!'); window.location.href ='/FinancialInstitute/CreateBranch';  </script>"));

         //   return View("");
        

        //private void InsertInfo(string BranchName, int CreateId, int FinancialInstituteId, string Address1, string Address2, string Address3, string FinancialInstituteContact, decimal CountryId, decimal StateId, decimal cityid, string pincode, string code, string FinancialInstituteEmailId1, string ContactPerson1, string ContactPerson1Mobile, string ContactPerson2, string ContactPerson2Mobile, string ContactPerson1EmailId, string ContactPerson2EmailId, List<int> CreditType, string BrCode)
        //{
        //    //InstituteBranch_CRUD_Operation objInstituteBranch_CRUD_Operation = new InstituteBranch_CRUD_Operation();

        //    //objInstituteBranch_CRUD_Operation.ADD_DETAILS(BranchName, CreateId, FinancialInstituteId, Address1, Address2, Address3, FinancialInstituteContact, CountryId, StateId, cityid, pincode, code, FinancialInstituteEmailId1, ContactPerson1, ContactPerson1Mobile, ContactPerson2, ContactPerson2Mobile, ContactPerson1EmailId, ContactPerson2EmailId, CreditType, BrCode);


        //    //---------------------------------------------------
        //}
    }
}
