using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using CreditRatingModel.Model;

namespace CreditRatingSystem.Controllers
{
    public class FeeDetailsController : Controller
    {
        
        private creaditratingEntities db = new creaditratingEntities();
        
        public ActionResult Index()
        {            
            return View();
        }

        public void FillData()
        {
            ViewBag.Country = new SelectList(db.CountryMasters.ToList(), "Id", "Country");
            ViewBag.Course = new SelectList(db.CourseMasters.ToList(), "Id", "CourseName");                        
        }

        public JsonResult FillAcademic(int CountryId)
        {   
            var AcYr = (from s in db.AcademicYears
                          where s.CountryId == CountryId
                          select s);
            return Json(new SelectList(AcYr.ToArray(), "Id", "AyId"), JsonRequestBehavior.AllowGet);
        }

        public JsonResult FillCollege(int CountryId, int CourseId)
        {

             var CollMst = (from CLGM in db.CollegeMasters
                               join CM in db.CountryMasters on CLGM.CountryId equals CM.Id
                               join CCR in db.CourseCollegeRelations on CLGM.Id equals CCR.CollegeId
                               join CRM in db.CourseMasters on CCR.CourseId equals CRM.Id
                                where CCR.Active == true && CRM.Id == CourseId && CM.Id == CountryId
                               select new { CLGM.Id, CLGM.CollegeName });
        
            return Json(new SelectList(CollMst.ToArray(), "Id", "CollegeName"), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCourseInfo(int CourseId, int AYId, int collegeid)
        {
            var strSql = (db.spGetCourseFeesInformation(CourseId, AYId, collegeid)).ToList();

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

        public ActionResult AddFeeDetails()
        {
            FillData();
            return View();
        }
        
        public JsonResult SaveData(SaveCollageFeesData sd)
        {             
            using (TransactionScope transaction = new TransactionScope())
            {           
                 try
                 {
                     var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                     string Data1 = sd.SaveFeesData;
                     
                     IEnumerable<FeeDetails> det = serializer.Deserialize<IEnumerable<FeeDetails>>(Data1);
                     
                     int intUserId = Convert.ToInt32(Session["UserId"]);
                     
                     if (det != null)
                     {
                         foreach (var item in det)
                         {
                             if (item.Id > 0)
                             {
                                 FeesDetailsHistory fh = new FeesDetailsHistory();      //FeesDetailsHistory name of the table
                                 fh.AcademicYearId = item.AcademicYearId;                                 
                                 fh.Fees = item.OldFees;                                 
                                 fh.CourseId = item.CourseId;
                                 fh.CollegeId = item.CollegeId;
                                 fh.Year = item.Year;    
                                 fh.FeesDetailId = item.Id;

                                 fh.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                                 fh.CreateId = intUserId;

                                 db.AddToFeesDetailsHistories(fh);
                                 
                                 //For delete
                                 var delSql = (from f in db.FeesDetails
                                               where f.Id == item.Id
                                               select f
                                            ).FirstOrDefault();

                                 db.DeleteObject(delSql);
                             }

                             //For Add     
                             FeesDetail fd = new FeesDetail();

                             fd.AcademicYearId = item.AcademicYearId;
                             fd.Fees = item.Fees;
                             fd.Year = item.Year;
                             fd.CourseId = item.CourseId;
                             fd.CollegeId = item.CollegeId;
                             fd.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                             fd.CreateId = intUserId;

                             db.AddToFeesDetails(fd);
                             
                         }
                         db.SaveChanges();                         
                     }
                     transaction.Complete();
                     return Json(new { msg = "Data Saved Successfully." }, JsonRequestBehavior.AllowGet);             
                 }
                 catch (Exception ex)
                 {
                     transaction.Dispose();
                     return Json(new { msg = "Data Saved Faild." + ex.ToString() }, JsonRequestBehavior.AllowGet);                   
                 }             
            }
        }
    }
}
