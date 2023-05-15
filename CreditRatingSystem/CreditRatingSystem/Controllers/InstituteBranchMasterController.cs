using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CreditRatingModel.Model;

namespace CreditRating.Controllers
{
    public class InstituteBranchMasterController : Controller
    {
        //
        // GET: /InstituteBranchMaster/

        private creaditratingEntities db = new creaditratingEntities();

        [Authorize]
        [HttpGet]
        [ActionName("Index")]
        public ActionResult Index_Get()
        {
            FinancialInstitutionContactMaster_Metadata objFICM = new FinancialInstitutionContactMaster_Metadata();
            //CategoryName(objFICM);
            //BindCreditType(objFICM);
            //BindCountry(objFICM);
          
            dropList(objFICM);
            return View(objFICM);
        }


        [HttpPost]
        [ActionName("Index")]
        public ActionResult Index_Post(FinancialInstitutionContactMaster_Metadata FICM)
        {
           // FinancialInstitutionContactMaster_Metadata objFICM = new FinancialInstitutionContactMaster_Metadata();
            if (ModelState.IsValid)
            {
                btnAddClick(FICM);
            }
            dropList(FICM);
            return View(FICM);
        }

        public void dropList(FinancialInstitutionContactMaster_Metadata FICM)
        {
            ViewBag.Category = new SelectList(db.FinancialInstitutionMasters.ToList(), "Id", "Name");
            ViewBag.Country = new SelectList(db.CountryMasters.ToList(), "Id", "Country");
            ViewBag.CreditType = new SelectList(db.FinancialInstitutionCreditTypeMasters.ToList(), "Id", "CreditType");
        }
        //public List<SelectListItem> CategoryName(FinancialInstitutionContactMaster_Metadata objFICM)
        //{
        //    return objFICM.CategoryItem = GetCategory();
        //}

        //private List<SelectListItem> GetCategory()
        //{
        //    List<SelectListItem> category = new List<SelectListItem>();

        //    try
        //    {
        //        var categories = (from c in db.FinancialInstitutionMasters
        //                          select c).ToList();

        //        if (categories != null)
        //        {
        //            category.Add(new SelectListItem { Text = "Select", Value = "0" });
        //            foreach (var item in categories)
        //            {
        //                category.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.Message.ToString();
        //    }
        //    return category;
        //}

        //public List<SelectListItem> BindCreditType(FinancialInstitutionContactMaster_Metadata objFICM)
        //{
        //    return objFICM.CreditType = GetCreditType();
        //}

        //private List<SelectListItem> GetCreditType()
        //{
        //    List<SelectListItem> CreditType = new List<SelectListItem>();

        //    try
        //    {
        //        var Types = (from t in db.FinancialInstitutionCreditTypeMasters
        //                     select t).ToList();

        //        if (Types != null)
        //        {
        //            CreditType.Add(new SelectListItem { Text = "Select", Value = "0" });

        //            foreach (var item in Types)
        //            {
        //                CreditType.Add(new SelectListItem { Text = item.CreditType, Value = item.Id.ToString() });
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //        ex.Message.ToString();
        //    }

        //    return CreditType;
        //}

        //public List<SelectListItem> BindCountry(FinancialInstitutionContactMaster_Metadata objFICM)
        //{
        //    return objFICM.country = GetCountry();
        //}

        //private List<SelectListItem> GetCountry()
        //{
        //    List<SelectListItem> country = new List<SelectListItem>();

        //    try
        //    {
        //        var countries = (from c in db.CountryMasters
        //                         select c).ToList();

        //        if (countries != null)
        //        {
        //            country.Add(new SelectListItem { Text = "Select", Value = "0" });

        //            foreach (var item in countries)
        //            {
        //                country.Add(new SelectListItem { Text = item.Country, Value = item.Id.ToString() });
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.Message.ToString();
        //    }
        //    return country;
        //}


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

            return Json(new SelectList(states.ToArray(), "Id", "State"), JsonRequestBehavior.AllowGet);
        }

        
        public string AssignCountryCode(string CountryId)
        {
            int id = Convert.ToInt32(CountryId);
            string strCountryCode="";

            var C = db.CountryMasters.Where(c=>c.Id.Equals(id)).FirstOrDefault();
                               //where s.Id == id
                               //select s.CountryCode);

            if (C != null)
            {
                strCountryCode = C.CountryCode;
            }
            return strCountryCode;
            //return Json(CountryCode, JsonRequestBehavior.AllowGet);
        }

        private void InsertInfo(string BranchName, int CreateId, int FinancialInstituteId, string Address1, string Address2, string Address3, string FinancialInstituteContact, int CountryId, int StateId, string city, string pincode, string code, string FinancialInstituteEmailId1, string ContactPerson1, string ContactPerson1Mobile, string ContactPerson2, string ContactPerson2Mobile, string ContactPerson1EmailId, string ContactPerson2EmailId,List<int> CreditType,string BrCode)
        {
            InstituteBranch_CRUD_Operation objInstituteBranch_CRUD_Operation = new InstituteBranch_CRUD_Operation();

            //objInstituteBranch_CRUD_Operation.ADD_DETAILS(BranchName, CreateId, FinancialInstituteId, Address1, Address2, Address3, FinancialInstituteContact, CountryId, StateId, city, pincode, code, FinancialInstituteEmailId1, ContactPerson1, ContactPerson1Mobile, ContactPerson2, ContactPerson2Mobile, ContactPerson1EmailId, ContactPerson2EmailId,CreditType,BrCode);
          
        }
        [HttpPost]
        public ActionResult btnAddClick(FinancialInstitutionContactMaster_Metadata FICM)
        {
            //InsertInfo(FICM.FIBM.BranchName, 2, FICM.FIM.Id, FICM.Address1, FICM.Address2, FICM.Address3, FICM.ContactNo, FICM.CountryId, stateId, FICM.City, FICM.Pincode, FICM.Code, FICM.FinancialInstituteEmailId1, FICM.ContactPerson1, FICM.ContactPerson1Mobile, FICM.ContactPerson2, FICM.ContactPerson2Mobile, FICM.ContactPerson1EmailId, FICM.ContactPerson2EmailId);
            if (ModelState.IsValid)
            {
                int CreateUserId = Convert.ToInt32(Session["UserId"]);
            string selectedState = Request.Form["selState"];
                int stateId1=Convert.ToInt32(selectedState);
                InsertInfo(FICM.BranchName, CreateUserId, FICM.FinancialInstituteId, FICM.Address1, FICM.Address2, FICM.Address3, FICM.ContactNo, FICM.CountryId, stateId1, FICM.City, FICM.Pincode, FICM.Code, FICM.FinancialInstituteEmailId1, FICM.ContactPerson1, FICM.ContactPerson1Mobile, FICM.ContactPerson2, FICM.ContactPerson2Mobile, FICM.ContactPerson1EmailId, FICM.ContactPerson2EmailId, FICM.FinancialInstituteCreditTypeId, FICM.BranchCode);
                ViewBag.Message = "Data Saved";
                ModelState.Clear();
               // dropList(FICM);
              //  return RedirectToAction("Index");
            //}
           // else
            //{

            }
            dropList(FICM);
            return View("Index");  
            
        }

        //private void InsertInfo(string p1, int p2, int p3, string p4, string p5, string p6, string p7, decimal p8, int stateId, string p9, string p10, string p11, string p12, string p13, string p14, string p15, string p16, string p17, string p18)
        //{
        //    throw new NotImplementedException();
        //}

    }
}
