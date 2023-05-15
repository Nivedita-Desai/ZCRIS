using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CreditRatingModel.Model;


namespace CreditRatingSystem.Controllers
{
    public class CollegeMasterController : Controller
    {
        //
        // GET: /CollegeMaster/
        private creaditratingEntities CRE = new creaditratingEntities();

        [Authorize]
        [HttpGet]
        public ActionResult AddCollege()
        {
            dropList();
            return View();
        }

        [HttpPost]
        public ActionResult AddCollege(CollegeMaster_Metadata CMM)
        {
            if (ModelState.IsValid)
            {
                btnAddClick(CMM);
            }
            dropList();
            return Content(("<script language='javascript' type='text/javascript'> alert('Data saved successfully!'); window.location.href ='/CollegeMaster/AddCollege';  </script>"));
        }
        public void dropList()
        {
            ViewBag.Designation = new SelectList((from de in CRE.DesignationMasters where de.DesignationType == "C" select new { de.Id, de.Designation }).ToList(), "Id", "Designation");
            ViewBag.Country = new SelectList(CRE.CountryMasters.ToList(), "Id", "Country", "Id");
            //ViewBag.Course = new SelectList((from CO in CRE.CourseMasters where CO.Active == true select new { CO.Id, CO.CourseName }).ToList(), "Id", "CourseName");
        }

        public IList<StateMaster> BindState(int countryid)
        {
            return CRE.StateMasters.Where(s => s.CountryId == countryid).ToList();
        }


        public JsonResult StateList(string CountryId)
        {
            if (CountryId != "")
            {
                int id = Convert.ToInt32(CountryId);

                var states = (from s in CRE.StateMasters
                              where s.CountryId == id
                              select s);

                return Json(new SelectList(states.ToArray(), "Id", "State"), JsonRequestBehavior.AllowGet);
            }
            return Json(null);
        }
        //--city bind--//
        public IList<CityMaster> BindCity(int selstate)
        {
            return CRE.CityMasters.Where(cm => cm.StateId == selstate).ToList();
        }


        public JsonResult CityList(string selstate)
        {

            int id = Convert.ToInt32(selstate);

            var states = (from s in CRE.CityMasters
                          where s.StateId == id
                          select s);

            return Json(new SelectList(states.ToArray(), "Id", "City"), JsonRequestBehavior.AllowGet);
        }
        //----
        //private SelectList CourseList()
        //{   
        //    //var couses = (from s in db.CourseMasters
        //    //              select s);

        //    //return new SelectList(db.CourseMasters.ToList(), "Id", "CourseName", "CourseCategory");
        //}

        public string AssignCountryCode(string selCity)
        {
            int id = Convert.ToInt32(selCity);
            string strCountryCode = "";

            var C = CRE.CityMasters.Where(c => c.Id.Equals(id)).FirstOrDefault();

            if (C != null)
            {
                strCountryCode = C.CityCode;
            }
            return strCountryCode;

        }

        private void InsertInfo(string ColName, string Address1, string Address2, string Address3, decimal CountryId, decimal StateId, decimal cityID, string pincode, string code, string contactNo, string EmailId, string Colwebsite, string ContPerName, string ContPerMob, string ContPerEmail, int CreateID, string Designation)
        {
            InstituteBranch_CRUD_Operation objInstituteBranch_CRUD_Operation = new InstituteBranch_CRUD_Operation();

            objInstituteBranch_CRUD_Operation.AddNewCollege_Master(ColName, Address1, Address2, Address3, CountryId, StateId, cityID, pincode, code, contactNo, EmailId, Colwebsite, ContPerName, ContPerMob, ContPerEmail, CreateID, Designation);

        }

        [HttpPost]
        public ActionResult btnAddClick(CollegeMaster_Metadata CMM)
        {
            var c = (from clg in CRE.CollegeMasters
                     where clg.CollegeName == CMM.CourseName
                     select clg.CollegeName).FirstOrDefault();
            if (c != null)
            {
                return Content(("<script language='javascript' type='text/javascript'> alert('College Name Already Exist!');window.location.href ='/CollegeMaster/AddCollege';</script>"));
  
            }
            else
            {
                if (ModelState.IsValid)
                {
                    int CreateUserId = Convert.ToInt32(Session["UserId"]);
                    string selectedState = Request.Form["selState"];
                    int stateId1 = Convert.ToInt32(selectedState);
                    string selectedCity = Request.Form["selCity"];
                    int CityId1 = Convert.ToInt32(selectedCity);
                    //var BankId1 = (from s in db.FinancialInstitutionMasters
                    //               where s.BankCode == CBM.BankCode
                    //               select s.Id).FirstOrDefault();

                    //var BrId1 = (from s in db.FinancialInstitutionBranchMasters
                    //             where s.BranchCode == CBM.BranchCode
                    //             select s.Id).FirstOrDefault();

                    //int BankId = Convert.ToInt32(BankId1);
                    //int BrId = Convert.ToInt32(BrId1);

           InsertInfo(CMM.CollegeName, CMM.Address1, CMM.Address2, CMM.Address3, CMM.CountryId, stateId1, CityId1, CMM.pincode, CMM.code, CMM.ContactNo, CMM.CEmailId, CMM.website, CMM.ContactPersonName, CMM.ContactPersonMobile, CMM.ContactPersonEmail, CreateUserId, CMM.ConPersonDesignation);
                    ModelState.Clear();
                    // dropList(FICM);
                    //  return RedirectToAction("Index");
                    //}
                    // else
                    //{

                }
                // dropList(FICM);
                return Content(("<script language='javascript' type='text/javascript'> alert('Data Saved Successfully!');window.location.href ='/CollegeMaster/AddCollege';</script>"));
            }

           // return View("AddCollege");

        }
        [HttpPost]
        public ActionResult CountryCode(string CountryId)
        {
            if (CountryId != "")
            {
                int id = Convert.ToInt32(CountryId);

                var CountryCode = (from s in CRE.CountryMasters
                                   where s.Id == id
                                   select s.CountryCode);
                return Json(CountryCode);
            }
            return Json(null);
            //return Json(CountryCode, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewCollege()
        {
            var clg = (from c in CRE.CollegeMasters
                       orderby c.CollegeName.Trim()
                       select new
                       {

                           id = c.Id,

                           CollegeName = c.CollegeName,
                           Address = c.Address1,
                           Website = c.WebSite
                       }
                 ).ToList();
            return View(clg);
        }
        public ActionResult CollegeDetails(int id,CollegeMaster_Metadata Cm)
        {

            var clg = (from c in CRE.CollegeMasters
                       join Cc in CRE.CourseCollegeRelations on c.Id equals id
                       where c.Id == id
                       select new CollegeMaster_Metadata
                       {

                           id = c.Id,

                           CollegeName = c.CollegeName,
                           Address1 = c.Address1,
                           Address2 = c.Address2,
                           Address3 = c.Address3,
                           website = c.WebSite,
                           CountryId = c.CountryId.Value,
                           stateId = c.StateId.Value,
                           cityID = c.CityId.Value,
                           pincode = c.Pincode,
                           ContactNo = c.ContactNo,
                           CEmailId = c.EmailId,
                           ContactPersonName = c.ContactPersonName,
                           ContactPersonMobile = c.ContactPersonMobile,
                           ContactPersonEmail = c.ContactPersonEmailId,
                           ConPersonDesignation=c.Designation,

                       }
                 ).FirstOrDefault();
            char delimiter = '-';

            string clgContactNo = clg.ContactNo;
            string[] phrases = clgContactNo.Split(delimiter);
            ViewBag.ContactCode = phrases[0];
            ViewBag.ContactNo = phrases[1];

            string ContactPersonMobile = clg.ContactPersonMobile;
            string[] phrasess = ContactPersonMobile.Split(delimiter);
            ViewBag.Mobilecode = phrasess[0];
            ViewBag.Mobileno = phrasess[1];

            decimal C = (from ecd in CRE.CollegeMasters
                         where ecd.Id == id
                         select ecd.CountryId).FirstOrDefault() ?? 0;

            decimal s = (from ecd in CRE.CollegeMasters
                         where ecd.Id == id && ecd.CountryId == C
                         select ecd.StateId).FirstOrDefault() ?? 0;

            decimal CC = (from ecd in CRE.CollegeMasters
                          where ecd.Id == id && ecd.CountryId == C && ecd.StateId == s
                          select ecd.CityId).FirstOrDefault() ?? 0;

            var cc = (from c in CRE.CourseMasters
                      join Cc in CRE.CourseCollegeRelations
                      on c.Id equals Cc.CourseId
                      where Cc.CollegeId == id
                      select c.CourseName).ToList();


            ViewBag.desig = new SelectList((from de in CRE.DesignationMasters where de.DesignationType == "C" select new { de.Id, de.Designation }).ToList(), "Id", "Designation",clg.ConPersonDesignation);
           
            ViewBag.CountryList = new SelectList(CRE.CountryMasters.ToList(), "Id", "Country", C);
            ViewBag.StateList1 = new SelectList((from sm in CRE.StateMasters where sm.CountryId == C select new { sm.Id, sm.State }).ToList(), "Id", "State");
            ViewBag.CityList1 = new SelectList((from cm in CRE.CityMasters where cm.StateId == s select new { cm.Id, cm.City }).ToList(), "Id", "City");

            //ViewBag.Courses = new MultiSelectList(cc);

            ////var w =( 
            ////       from CM in CRE.CourseMasters
            ////       where  !
            ////       (from CCR in CRE.CourseCollegeRelations where CCR.CollegeId == id  select CCR.CourseId).Contains(CM.Id)
            ////       select CM.CourseName).ToList();
            //ViewBag.Course1 = new SelectList(
            //       (from CM in CRE.CourseMasters
            //       where !
            //       (from CCR in CRE.CourseCollegeRelations where CCR.CollegeId == id && CM.Active== Convert.ToBoolean('1') select CCR.CourseId).Contains(CM.Id)
            //    select new {CM.Id,CM.CourseName}).ToList(),"Id","CourseName");


            return View(clg);
        }
        public ActionResult editclg(CollegeMaster_Metadata CMM)
        {

            int intUserId = Convert.ToInt32(Session["UserId"]);
            var CM = CRE.CollegeMasters.Where(x => x.Id == CMM.id).FirstOrDefault();

            CM.CollegeName = CMM.CollegeName;
            CM.Address1 = CMM.Address1;
            CM.Address2 = CMM.Address2;
            CM.Address3 = CMM.Address3;
            CM.WebSite = CMM.website;
            CM.CountryId = CMM.CountryId;
            CM.StateId = CMM.stateId;
            CM.CityId = CMM.cityID;
            CM.Pincode = CMM.pincode;
            CM.ContactNo =CMM.code+"-"+ CMM.ContactNo;
            CM.EmailId = CMM.CEmailId;
            CM.ContactPersonName = CMM.ContactPersonName;
            CM.Designation = CMM.ConPersonDesignation;
            CM.ContactPersonMobile =CMM.code+"-"+ CMM.ContactPersonMobile;
            CM.ContactPersonEmailId = CMM.ContactPersonEmail;
            CM.EditDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
            CM.EditId = Convert.ToDecimal(intUserId);
            CRE.SaveChanges();


            //if (CMM.CourseListId == null)
            //{
            //}
            //else{
            //foreach (var item in CMM.CourseListId)
            //{
            //    CourseCollegeRelation cm = new CourseCollegeRelation();
            //    cm.CollegeId = CMM.id;
            //    cm.CourseId = item;
            //    cm.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);

            //    cm.CreateId = Convert.ToDecimal(intUserId);
            //    CRE.AddToCourseCollegeRelations(cm);
            //    CRE.SaveChanges();

            //}

            //}
            return RedirectToAction("ViewCollege");
        }
        public JsonResult Clgexist( string CollegeName)
        {

          

            var s = (from r in CRE.CollegeMasters
                     where r.CollegeName==CollegeName
                     select r.CollegeName).FirstOrDefault();

            return Json(s, JsonRequestBehavior.AllowGet);
        }
//------CourseMaster----//
        public ActionResult AddCourse(CollegeMaster_Metadata cmm)
        {
            if (cmm.id == 0)
            {
                ViewBag.Start = "";
                ViewBag.StreamFields = new SelectList(CRE.StreamMasters.ToList(), "Id", "Stream");
                ViewBag.category = new SelectList(CRE.CourseCategories.ToList(), "Id", "CourseCategory_Description");
                return View();
            }
            else
            {
                ViewBag.Start = "startDate";
                var course = (from cm in CRE.CourseMasters
                              join ct in CRE.CourseCategories on cm.CourseCategoryId equals ct.Id
                              join sm in CRE.StreamMasters on cm.StreamId equals sm.Id
                              where cm.Id == cmm.id

                              select new CollegeMaster_Metadata
                             {
                                 id = cm.Id,
                                 CourseName = cm.CourseName,
                                 CategoryId = cm.CourseCategoryId.Value,
                                 StreamField = cm.StreamId.Value,
                              
                         month=cm.InMonth,


                                 duration=cm.Duration.Value,
                               
                                 durationperiod=cm.DurationPeriod,
                                 CourseType = cm.CourseType,
                                 Startdate = (cm.CourseStartDate).Value,
                                 status = cm.Active.Value
                             





                             }

                          ).FirstOrDefault();

              var C= course.month;
              if (C == "Y") { 
              
              ViewBag.C=true;
              
              }

                //var S = (from ecd in CRE.CourseMasters
                //             join st in CRE.StreamMasters on ecd.StreamId equals st.Id
                //             where ecd.Id == cmm.id
                //             select new { st.Id, st.Stream }).ToList();
                //decimal S = (from ecd in CRE.CourseMasters
                //             where ecd.Id == cmm.id
                //             select ecd.StreamId).FirstOrDefault() ?? 0;
                //decimal Cats = (from ecd in CRE.CourseMasters
                //             where ecd.Id == cmm.id
                //             select ecd.CourseCategoryId).FirstOrDefault() ?? 0;


                ViewBag.StreamFields = new SelectList(CRE.StreamMasters.ToList(), "Id", "Stream", course.StreamField);
                ViewBag.CategoryId = new SelectList(CRE.CourseCategories.ToList(), "Id", "CourseCategory_Description", course.CategoryId);
                //ViewBag.StreamId1 = new SelectList(CRE.StreamMasters.ToList(), "Id", "Stream", course.StreamId1);
                  return View(course);
            }



        }

        public JsonResult Courseexist(int field, string course)
        {

            int id = Convert.ToInt32(field);

            var s = (from r in CRE.CourseMasters
                     where r.StreamId == field && r.CourseName == course
                     select r.CourseName).FirstOrDefault();

            return Json(s, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Course(CollegeMaster_Metadata cmm)
        {
            int intUserId = Convert.ToInt32(Session["UserId"]);
            if (cmm.id == 0)
            {
                //var s = (from r in CRE.CourseMasters
                //         where r.StreamId == cmm.StreamField && r.CourseName == cmm.CourseName
                //         select r.CourseName).FirstOrDefault();

                //if (s != null)
                //{
                //    return Content(("<script language='javascript' type='text/javascript'> alert('Course Name Already Exist!');window.location.href ='/CollegeMaster/AddRelationship';</script>"));


                //}
               // else
               // {
                    CourseMaster CM = new CourseMaster();
                    CM.CourseName = cmm.CourseName;
                    CM.CourseCategoryId = Convert.ToInt32(cmm.CategoryId);
                    CM.StreamId = cmm.StreamField;
                    CM.CourseType = cmm.CourseType;
                    CM.CourseStartDate = cmm.Startdate;
                    CM.Duration = cmm.duration;
                 
                    if (cmm.Morethan1yr == true)
                    {
                        CM.InMonth = "Y";
                        CM.DurationPeriod = cmm.durationperiod;
                    }
                    else {
                        CM.InMonth = "N";
                        CM.DurationPeriod = null;
                    }
                    CM.Active = cmm.status;
                    CM.DurationPeriod = cmm.durationperiod;
                    CM.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                    CM.CreateId = intUserId;

                    CRE.AddToCourseMasters(CM);
                    CRE.SaveChanges();

                    return Content(("<script language='javascript' type='text/javascript'> alert('Data saved successfully!'); window.location.href ='/CollegeMaster/AddCourse';  </script>"));
                }
           // }
            else
            {

                var CM = CRE.CourseMasters.Where(x => x.Id == cmm.id).FirstOrDefault();


                CM.CourseName = cmm.CourseName;
                CM.CourseCategoryId = Convert.ToInt32(cmm.CategoryId);
                CM.StreamId = cmm.StreamField;
                CM.CourseType = cmm.CourseType;
                CM.CourseStartDate = cmm.Startdate;
                CM.Duration = cmm.duration;
              
                if (cmm.Morethan1yr == true)
                {
                    CM.InMonth = "Y";
                    CM.DurationPeriod = cmm.durationperiod;
                }
                else
                {
                    CM.InMonth = "N";
                    CM.DurationPeriod = "";

                }
                CM.Active = cmm.status;
                CM.DurationPeriod = cmm.durationperiod;
                CM.EditDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                CM.EditId = intUserId;


                CRE.SaveChanges();

                return Content(("<script language='javascript' type='text/javascript'> alert('Data saved successfully!'); window.location.href ='/CollegeMaster/ViewCourse';  </script>"));

            }
        }
        public ActionResult ViewCourse()
        {
            var course = (from cm in CRE.CourseMasters
                          join ct in CRE.CourseCategories
                          on cm.CourseCategoryId equals ct.Id
                          join sm in CRE.StreamMasters on cm.StreamId equals sm.Id
                          select new
                         {
                             id = cm.Id,
                             courseName = cm.CourseName,
                             courseCategory = ct.CourseCategory_Description,
                             stream = sm.Stream,
                             duration =cm.Duration,
                             durationperiod=cm.DurationPeriod

                         }

                          ).ToList();


            return View(course);
        }


      
    }
}