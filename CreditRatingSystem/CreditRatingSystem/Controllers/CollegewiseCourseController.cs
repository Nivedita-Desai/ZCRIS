using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CreditRatingModel.Model;

namespace CreditRatingSystem.Controllers
{
    public class CollegewiseCourseController : Controller
    {
        private creaditratingEntities db = new creaditratingEntities();

        public ActionResult Index()
        {
            return View();
        }

        public void FillData()
        {
            ViewBag.College = new SelectList(db.CollegeMasters.ToList(), "Id", "CollegeName");
            ViewBag.Stream = new SelectList(db.StreamMasters.ToList(), "Id", "Stream");
            ViewBag.Category = new SelectList(db.CourseCategories.ToList(), "Id", "CourseCategory_Description");            
        }

        public JsonResult GetCourseName(int streamid, int coursecategoryid, int collegeId)
        {
            var strSql = (db.spGetCourseInformation(streamid,coursecategoryid,collegeId)).ToList();

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

        public ActionResult AddCollegewiseCourse()
        {
            FillData();
            return View();
        }

        public JsonResult SaveData(SaveCollegewiseCourse sc)
        {
            using (creaditratingEntities db = new creaditratingEntities())
            {
                try
                {
                    var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    string Data1 = sc.SaveCollegewiseCourseData;
                    string EditData1 = sc.SavePrimaryKeyData;
                                        
                    IEnumerable<CollegewiseCourse> det = serializer.Deserialize<IEnumerable<CollegewiseCourse>>(Data1);
                    IEnumerable<CollegewiseCourse> editdet = serializer.Deserialize<IEnumerable<CollegewiseCourse>>(EditData1);
                 
                    int intUserId = Convert.ToInt32(Session["UserId"]);
                  
                    if (editdet != null)
                    {
                        foreach (var i in editdet)
                        {
                            var editSql = db.CourseCollegeRelations.Where(x => x.Id == i.Id).FirstOrDefault();
                           
                            editSql.Active = i.Active;
                            editSql.EditDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                            editSql.EditId = intUserId;
                            db.SaveChanges();                                                                           
                        }
                    }
                    if (det != null)
                    {

                        foreach (var item in det)
                        {                           
                            CourseCollegeRelation cc = new CourseCollegeRelation();
                            cc.CollegeId = item.CollegeId;
                            cc.CourseId= item.CourseId;
                            cc.Active = item.Active;
                            
                            cc.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                            cc.CreateId = intUserId;

                            db.AddToCourseCollegeRelations(cc);

                        }
                        db.SaveChanges();
                    }

                    return Json(new { msg = "Data Saved Successfully." }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { msg = "Data Saved Faild." + ex.ToString() }, JsonRequestBehavior.AllowGet);
                }
            }
        }

    }
}
