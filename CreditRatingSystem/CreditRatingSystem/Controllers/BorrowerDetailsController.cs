using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CreditRatingModel.Model;


namespace CreditRating.Controllers
{
    public class BorrowerDetailsController : Controller
    {
        creaditratingEntities db = new creaditratingEntities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult BorrowerList()
        {
            using (creaditratingEntities db = new creaditratingEntities())
            {
                var strSql = (from BCM in db.BorrowerCompanyMasters
                              join BCTM in db.BorrowerCompanyTypeMasters on BCM.Id equals BCTM.CompanyId 
                              join BCBM in db.BorrowerCompanyBranchMasters on BCTM.Id equals BCBM.CompanyTypeId 
                              join BDM in db.BorrowerDetailsMasters on BCBM.Id equals BDM.BranchMasterId 
                      
                                 select new
                                 {                                     
                                     BCM.CompanyName,
                                     BCTM.CompanyType,
                                     BCBM.BranchName,
                                     BCM.IncorporationDate,
                                     BCM.CommencementDate,
                                     BCBM.Id
                                 }
                            ).ToList();
                var L = (from ICM in db.IndividualCustomerMasters
                         join NM in db.NationalityMasters on ICM.NationalityId equals NM.Id
                         join NTM in db.NameTitleMasters on ICM.TitleId equals NTM.Id
                         select new
                         {
                             ICM.Id,
                             ICM.FirstName,
                             ICM.MiddleName,
                             ICM.LastName,
                             ICM.Sex,
                             ICM.Pan,
                             ICM.DateOfBirth,
                             NM.Nationality,
                             NTM.Name
                         }).ToList();

                ViewBag.List = L;
                //ViewData["MyProduct"] = L;

                return View("BorrowerList",strSql);
            }
        }

        public ActionResult BorrowerViewList(int Id)
        {
            using (creaditratingEntities db = new creaditratingEntities())
            {
                var strSql = (from BCM in db.BorrowerCompanyMasters
                              join BCTM in db.BorrowerCompanyTypeMasters on BCM.Id equals BCTM.CompanyId
                              join BCBM in db.BorrowerCompanyBranchMasters on BCTM.Id equals BCBM.CompanyTypeId
                              join BDM in db.BorrowerDetailsMasters on BCBM.Id equals BDM.BranchMasterId
                              where BCBM.Id == Id
                              select new BorrowerDetails   
                              {
                                  CompanyName=BCM.CompanyName,
                                  CompanyType = BCTM.CompanyType,
                                  BranchName=BCBM.BranchName,
                                  IncorporationDate = BCM.IncorporationDate.Value,
                                  CommencementDate = BCM.CommencementDate,
                                  id = Id                                  
                              }).FirstOrDefault();
            
                return View(strSql);
            }            
        }

        [HttpPost]
        public ActionResult EditAction(BorrowerDetails.BorrowGrid BD)
        {
            ViewBag.intUserId = Session["UserId"];

            var tblBorrowerCompanyBranchMaster = db.BorrowerCompanyBranchMasters.Where(x => x.Id == BD.ID).FirstOrDefault();

            decimal decCompTypeId = (from BCTM in db.BorrowerCompanyTypeMasters
                                     join BCBM in db.BorrowerCompanyBranchMasters on BCTM.Id equals BCBM.CompanyTypeId
                                     where BCBM.Id == BD.ID
                                     select BCTM.Id
                                  ).FirstOrDefault();


            var tblBorrowerCompanyTypeMaster = db.BorrowerCompanyTypeMasters.Where(x => x.Id == decCompTypeId).FirstOrDefault();

            decimal decCompId = (from BCM in db.BorrowerCompanyMasters
                                 join BCTM in db.BorrowerCompanyTypeMasters on BCM.Id equals BCTM.CompanyId
                                 join BCBM in db.BorrowerCompanyBranchMasters on BCTM.Id equals BCBM.CompanyTypeId
                                 where BCBM.Id == BD.ID
                                 select BCM.Id
                                 ).FirstOrDefault();

            var tblBorrowerCompanyMaster = db.BorrowerCompanyMasters.Where(x => x.Id == decCompId).FirstOrDefault();
            if (ModelState.IsValid)
            {
                try
                {
                    tblBorrowerCompanyBranchMaster.BranchName = BD.BranchName;
                    tblBorrowerCompanyBranchMaster.EditDate = DateTime.UtcNow.Date;
                    tblBorrowerCompanyBranchMaster.EditId = ViewBag.intUserId;

                    tblBorrowerCompanyTypeMaster.CompanyType = BD.CompanyType;
                    tblBorrowerCompanyTypeMaster.EditDate = DateTime.UtcNow.Date;
                    tblBorrowerCompanyTypeMaster.EditId = ViewBag.intUserId;

                    tblBorrowerCompanyMaster.CompanyName = BD.CompanyName;
                    tblBorrowerCompanyMaster.IncorporationDate = BD.IncorporationDate;
                    tblBorrowerCompanyMaster.CommencementDate = BD.CommencementDate;
                    tblBorrowerCompanyMaster.EditDate = DateTime.UtcNow.Date;
                    tblBorrowerCompanyMaster.EditId = ViewBag.intUserId;

                    db.SaveChanges();
                    //TempData["Message"] = "Record Update Successfully.";                    

                   return RedirectToAction("BorrowerList");
                }
                catch (Exception )
                {
                    return View("BorrowerViewList");
                }
            }
            return View("BorrowerViewList");

        }

        //****************

        //------------Darshana-----------------
        public ActionResult Details(int id)
        {
            using (creaditratingEntities db = new creaditratingEntities())
            {
                var L = (from ICM in db.IndividualCustomerMasters
                         join NM in db.NationalityMasters on ICM.NationalityId equals NM.Id
                         join NTM in db.NameTitleMasters on ICM.TitleId equals NTM.Id
                         join CDM in db.ContactDetailsMasters on ICM.Id equals CDM.IndividualCustomerId
                         join SM in db.StateMasters on CDM.StateId equals SM.Id
                         join CM in db.CountryMasters on CDM.CountryId equals CM.Id
                         where ICM.Id == id
                         select new BorrowerDetailscs
                         {
                             Id = id,
                             FirstName = ICM.FirstName,
                             MiddleName = ICM.MiddleName,
                             LastName = ICM.LastName,
                             Sex = ICM.Sex,
                             Pan = ICM.Pan,
                             DateOfBirth = ICM.DateOfBirth.Value,
                             Nationality = NM.Nationality,
                             Name = NTM.Name,
                             Nationality1 = NM.Id,
                             Name1 = NTM.Id,
                             Address1 = CDM.Address1,
                             Address2 = CDM.Address2,
                             Address3 = CDM.Address3,
                             Country = CM.Id,
                             State1 = SM.Id,
                             //City = CDM.City,
                             Pincode = CDM.Pincode,
                             LandlineCode = CDM.LandlineCode,
                             LandlineNo = CDM.LandlineNo,
                             MobileCode = CDM.MobileCode,
                             MobileNo = CDM.MobileNo,
                             EmailId1 = CDM.EmailId1,
                             EmailId2 = CDM.EmailId2
                         }).FirstOrDefault();

                decimal c = (from CDM in db.ContactDetailsMasters
                             where CDM.IndividualCustomerId == id
                             select CDM.CountryId).FirstOrDefault() ?? 0;

                decimal s = (from CDM in db.ContactDetailsMasters
                             where CDM.IndividualCustomerId == id
                             select CDM.StateId).FirstOrDefault() ?? 0;

                decimal n = (from ICM in db.IndividualCustomerMasters
                             where ICM.Id == id
                             select ICM.NationalityId).FirstOrDefault() ?? 0;

                decimal T = (from ICM in db.IndividualCustomerMasters
                             where ICM.Id == id
                             select ICM.TitleId).FirstOrDefault() ?? 0;

                ViewBag.Name = new SelectList(db.NameTitleMasters.ToList(), "Id", "Name", T);
                ViewBag.CountryList = new SelectList(db.CountryMasters.ToList(), "Id", "Country", c);
                ViewBag.StateList1 = new SelectList(db.StateMasters.ToList(), "Id", "State", s);
                ViewBag.Nationality = new SelectList(db.NationalityMasters.ToList(), "Id", "Nationality", n);

                return View(L);
            }
        }
                
        private List<SelectListItem> GetCountry()
        {
            List<SelectListItem> country = new List<SelectListItem>();

            try
            {
                var countries = (from c in db.CountryMasters
                                 select c).ToList();

                if (countries != null)
                {
                    country.Add(new SelectListItem { Text = "Select", Value = "0" });

                    foreach (var item in countries)
                    {
                        country.Add(new SelectListItem { Text = item.Country, Value = item.Id.ToString() });
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            return country;
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
        [HttpPost]
        public ActionResult CountryCode(string CountryId)
        {
            int id = Convert.ToInt32(CountryId);

            var CountryCode = (from s in db.CountryMasters
                               where s.Id == id
                               select s.CountryCode);
            return Json(CountryCode);
            //return Json(CountryCode, JsonRequestBehavior.AllowGet);
        }

        public JsonResult doesPanExist(string Pan)
        {
            FinancialInstituteModel i = new FinancialInstituteModel();
            //var Name = ContactIDValue;
            var result = true;
            var user = db.IndividualCustomerMasters.Where(x => x.Pan == Pan).FirstOrDefault();
            if (user != null)
            {
                result = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Edit(BorrowerDetailscs BD)
        {
            if (ModelState.IsValid)
            {
                //string selectedState1 = Request.Form["Name"];
                creaditratingEntities db = new creaditratingEntities();
                var tblProd = db.IndividualCustomerMasters.Where(x => x.Id == BD.Id).FirstOrDefault();
                var tblProd1 = db.ContactDetailsMasters.Where(x => x.IndividualCustomerId == BD.Id).FirstOrDefault();


                //var tblProd1 =( from ICM in db.IndividualCustomerMasters
                //         join NM in db.NationalityMasters on ICM.NationalityId equals NM.Id
                //         join NTM in db.NameTitleMasters on ICM.TitleId equals NTM.Id
                //         join CDM in db.ContactDetailsMasters on ICM.Id equals CDM.IndividualCustomerId
                //         join SM in db.StateMasters on CDM.StateId equals SM.Id
                //         join CM in db.CountryMasters on CDM.CountryId equals CM.Id
                //               where (ICM.Id == BD.Id).FirstOrDefault();

                //int Nationality1 = Int32.Parse(BD.Nationality1);
                //int Name1 = Int32.Parse(BD.Name1);

                tblProd.FirstName = BD.FirstName;
                tblProd.MiddleName = BD.MiddleName;
                tblProd.LastName = BD.LastName;
                tblProd.Sex = BD.Sex;
                tblProd.Pan = BD.Pan;
                tblProd.DateOfBirth = BD.DateOfBirth;
                tblProd.NationalityId = BD.Nationality1;
                tblProd.TitleId = BD.Name1;
                tblProd1.PAN = BD.Pan;
                tblProd1.Address1 = BD.Address1;
                tblProd1.Address2 = BD.Address2;
                tblProd1.Address3 = BD.Address3;
                //tblProd1.City = BD.City;
                tblProd1.CountryId = BD.CountryId;
                tblProd1.StateId = BD.State1;
                tblProd1.Pincode = BD.Pincode;
                tblProd1.LandlineCode = BD.LandlineCode;
                tblProd1.LandlineNo = BD.LandlineNo;
                tblProd1.MobileCode = BD.MobileCode;
                tblProd1.MobileNo = BD.MobileNo;
                tblProd1.EmailId1 = BD.EmailId1;
                tblProd1.EmailId2 = BD.EmailId2;

                db.SaveChanges();
                //return Json(tblProd, JsonRequestBehavior.AllowGet);
                return RedirectToAction("GridBIDetails");
            }
            return RedirectToAction("Details", new { Id = BD.Id });
        }


        public ActionResult NameChanged()
        {

            return View();
        }
        public JsonResult NameChanged1(string Pan)
        {
            var result = (from ICM in db.IndividualCustomerMasters
                          where ICM.Pan == Pan
                          select new BorrowerDetailscs
                          {
                              Pan = ICM.Pan,
                              FirstName = ICM.FirstName,
                              MiddleName = ICM.MiddleName,
                              LastName = ICM.LastName
                          }).FirstOrDefault();

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult NameChangedEdit(BorrowerDetailscs BD)
        {
            //if (ModelState.IsValid)
            //{
            creaditratingEntities db = new creaditratingEntities();
            var tblProd = db.IndividualCustomerMasters.Where(x => x.Pan == BD.Pan).FirstOrDefault();
            //var tblProd1 = db.IndividualCustomerLogs.FirstOrDefault();

            tblProd.FirstName = BD.FirstName1;
            tblProd.MiddleName = BD.MiddleName1;
            tblProd.LastName = BD.LastName1;
            tblProd.IsNameChange = true;
            db.SaveChanges();
            ViewBag.ICMID = tblProd.Id;
            InsertInfo1(BD.FirstName, BD.MiddleName, BD.LastName, BD.FirstName1, BD.MiddleName1, BD.LastName1);


            return RedirectToAction("NameChanged");
        }

        private void InsertInfo1(string FirstName, string MiddleName, string LastName, string FirstName1, string MiddleName1, string LastName1)
        {
            //FinancialInstituteModel i = new FinancialInstituteModel();
            Add(FirstName, MiddleName, LastName, FirstName1, MiddleName1, LastName1);
        }

        public void Add(string FirstName, string MiddleName, string LastName, string FirstName1, string MiddleName1, string LastName1)
        {
            IndividualCustomerLog ICL = new IndividualCustomerLog();
            ICL.IndividualCustomerId = ViewBag.ICMID;
            ICL.OldFirstName = FirstName;
            ICL.OldMiddleName = MiddleName;
            ICL.OldLastName = LastName;
            ICL.NewFirstName = FirstName1;
            ICL.NewMiddleName = MiddleName1;
            ICL.NewLastName = LastName1;
            ICL.CreateDate = DateTime.UtcNow.Date;
            db.AddToIndividualCustomerLogs(ICL);
            db.SaveChanges();

        }
        //-------------------------------------------
    }
}
