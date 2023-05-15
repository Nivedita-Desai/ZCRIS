using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CreditRatingSystem.Models;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.Web;
using CreditRatingModel.Model;


namespace CreditRating.Controllers
{
    public class userController : Controller
    {
        //
        // GET: /user/

        

        grid m = new grid();
        creaditratingEntities db = new creaditratingEntities();
        public ActionResult Index()
        {
            
            return View();
        }
    //CrystalReportViewer CrystalReportViewer1 = new CrystalReportViewer();
        BorrowerDetails BD = new BorrowerDetails();
        //public void CrystalPrintData(string user_id)
        //{
        //    //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["creaditratingEntities"].ConnectionString);
        //    using (creaditratingEntities db = new creaditratingEntities())
        //    {
        //        try
        //        {
        //            DataSet ds = new DataSet();
        //            DataTable dt = new DataTable();

        //            var res = db.spPersonalInfo(Convert.ToInt32(user_id)).ToList();
        //            var Presult = Newtonsoft.Json.JsonConvert.SerializeObject(res);
        //            dt = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(Presult);
        //            dt.TableName = "CUSTOMER_DATA";
        //            ds.Tables.Add(dt);
        //            //IEnumerable<GetCustomerData> results = Context.Database.SqlQuery<GetPadikaData>("PADIKA_PRINT_GET_DATA_SP @LOT_DTL_ID = {0}", Para1).ToList();

        //            using (ReportClass rptH = new ReportClass())
        //            {
        //                //rptH.FileName = "E:/tanmay/ProjectsNew/CRS/CreditRatingSystem/CreditRatingSystem/CrystalReport1.rpt";
        //                rptH.FileName = Server.MapPath("~\\Report\\CrystalReport1.rpt");

        //                //ParameterField paramField = new ParameterField();
        //                //ParameterFields paramFields = new ParameterFields();
        //                //ParameterDiscreteValue paramDiscreteValue = new ParameterDiscreteValue();
        //                //paramField.Name = "@Id";                        
        //                //paramDiscreteValue.Value = 1;

        //                //paramField.CurrentValues.Add(paramDiscreteValue);
        //                //paramFields.Add(paramField);
        //                ////rptH.ParameterFieldInfo = paramFields;
        //                ////rptH.ParameterFields.Add(paramFields);
        //                //rptH.SetParameterValue("@Id", 1);

        //                rptH.Load();
        //                rptH.Database.Tables[0].SetDataSource(ds.Tables["CUSTOMER_DATA"]);
        //                rptH.Refresh();
        //                rptH.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");

        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            //Json(new { msg = "Error : " + ex.Message }, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //}
        public ActionResult List(string SearchBy, string Search)
        {
           
            var users = new List<FinancialInstitutionCategoryMaster>();
            //here  MyDatabaseEntities is the dbcontext
            using (creaditratingEntities dc = new creaditratingEntities())
            {
                users = dc.FinancialInstitutionCategoryMasters.ToList();
            }

            //if (SearchBy == "Name")
            //{
            //    return View(db.FinancialInstitutionCategoryMasters.Where(x => x.Category == Search || Search == null).ToList());
            //}
            //else
            //{
            //    return View(db.FinancialInstitutionCategoryMasters.Where(x => x.Category.StartsWith(Search) || Search == null).ToList());
            //}
            return View(users);
        }


         [Authorize]
        public ActionResult List1()
        {

            using (creaditratingEntities db = new creaditratingEntities())
            {
                var CreateUserId1 = Session["UserId"];
                int CreateUserId = Convert.ToInt32(CreateUserId1);

                var L = (from FIBM in db.FinancialInstitutionBranchMasters
                         join FIM in db.FinancialInstitutionMasters on  FIBM.FinancialInstituteId equals FIM.Id
                         join FICM in db.FinancialInstitutionCategoryMasters on FIM.FinancialInstituteCategoryId equals FICM.Id 

                         //where FIBM.Id == 1
                         select new
                         {

                             FIBM.Id,
                             FIBM.BranchName,
                             FIM.Name,
                             FIM.RegistrationNo,
                             FIM.BankCode,
                             FIBM.BranchCode,
                             FIM.SwiftCode,
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
        //[HttpPost]

         [Authorize]
        public ActionResult ViewDetails1(int id)
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
                         select new grid
                         {
                             Id = id,
                             Name = FIM.Name,
                             RegistrationNumber = FIM.RegistrationNo,
                             Category = FICM.Category,
                             BankCode=FIM.BankCode,
                             BranchName = FIBM.BranchName,
                             BranchCode = FIBM.BranchCode,
                             SwiftCode=FIM.SwiftCode,
                             CreateDate = FIBM.CreateDate ?? DateTime.Now,
                             Address1 = FICOM.Address1,
                             Address2 = FICOM.Address2,
                             Address3 = FICOM.Address3,
                             Country1 = CM.Id,
                             State1 = SM.Id,
                             //City = FICOM.City,
                             Pincode = FICOM.Pincode,
                             Code = FICOM.Code,
                             ContactNo = FICOM.ContactNo,
                             FinancialInstituteEmailId1 = FICOM.FinancialInstituteEmailId1,
                             FinancialInstituteContact = FICOM.FinancialInstituteContact,
                             ContactPerson1 = FICOM.ContactPerson1,
                             ContactPerson2 = FICOM.ContactPerson2,
                             ContactPerson2Mobile = FICOM.ContactPerson2Mobile,
                             ContactPerson1Mobile = FICOM.ContactPerson1Mobile,
                             ContactPerson1EmailId = FICOM.ContactPerson1EmailId,
                             ContactPerson2EmailId = FICOM.ContactPerson2EmailId
                         }).FirstOrDefault(); ;

                decimal c = (from FICOM in db.FinancialInstitutionContactMasters
                             where FICOM.FinancialInstituteBranchId == id
                             select FICOM.CountryId).FirstOrDefault() ?? 0;

                decimal s = (from FICOM in db.FinancialInstitutionContactMasters
                             where FICOM.FinancialInstituteBranchId == id
                             select FICOM.StateId).FirstOrDefault() ?? 0;

                decimal FI = (from F in db.FinancialInstitutionBranchMasters
                                   where F.Id == id
                              select F.FinancialInstituteId).FirstOrDefault() ?? 0;

                decimal CA = (from FIM in db.FinancialInstitutionMasters
                              where FIM.Id == FI
                              select FIM.FinancialInstituteCategoryId).FirstOrDefault() ?? 0;

                //var c = db.FinancialInstitutionContactMasters.Where(a => a.CountryId == id).Select(a=>a.CountryId);
                //int f = c;
                ViewBag.CountryList = new SelectList(db.CountryMasters.ToList(), "Id", "Country", c);
                ViewBag.StateList1 = new SelectList(db.StateMasters.ToList(), "Id", "State", s);
                ViewBag.Category = new SelectList(db.FinancialInstitutionCategoryMasters.ToList(), "Id", "Category",CA);
                return View(L);
            }
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
         [Authorize]
        public ActionResult Edit(grid BD)
        {
            var CreateUserId1 = Session["UserId"];
            int CreateUserId = Convert.ToInt32(CreateUserId1);

            creaditratingEntities db = new creaditratingEntities();
           
             var tblProd = db.FinancialInstitutionBranchMasters.Where(x => x.Id == BD.Id).FirstOrDefault();
             var tblProd1 = db.FinancialInstitutionContactMasters.Where(x => x.FinancialInstituteBranchId == BD.Id).FirstOrDefault();
             decimal b = (from s in db.FinancialInstitutionContactMasters
                          where s.FinancialInstituteBranchId == BD.Id
                          select s.FinancialInstitutionId).FirstOrDefault() ?? 0;

            var tblProd2 = db.FinancialInstitutionMasters.Where(x => x.Id == b).FirstOrDefault();
           

            //int Nationality1 = Int32.Parse(BD.Nationality);

            tblProd.BranchName = BD.BranchName;
            tblProd.BranchCode = BD.BranchCode;
            tblProd.EditDate = DateTime.UtcNow.Date;
            tblProd.EditId = CreateUserId;
            tblProd1.Address1 = BD.Address1;
            tblProd1.Address2 = BD.Address2;
            tblProd1.Address3 = BD.Address3;
            //tblProd1.City = BD.City;
            tblProd1.CountryId = BD.CountryId;
            tblProd1.StateId = BD.State1;
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
            tblProd2.RegistrationNo = BD.RegistrationNumber;
            tblProd2.BankCode = BD.BankCode;
            tblProd2.SwiftCode = BD.SwiftCode;
            tblProd2.EditDate = DateTime.UtcNow.Date;
            tblProd2.EditId = CreateUserId;
         
             db.SaveChanges();
            //return Json(tblProd, JsonRequestBehavior.AllowGet);
            return RedirectToAction("List1");
        }

        //public IList<CountryMaster> BindCountryCode(int countryid)
        //{
        //    return db.CountryMasters.Where(s => s.Id == countryid).ToList;
        //}
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
    }
}
