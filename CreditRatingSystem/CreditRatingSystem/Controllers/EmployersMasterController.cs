using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CreditRatingModel.Model;
namespace CreditRating.Controllers
{
    public class EmployerMasterController : Controller
    {
        creaditratingEntities CRE = new creaditratingEntities();



        // GET: /EmployerMaster/
        //----Add Emploer----//
        
        //public ActionResult Index()
        //{
        //    ViewBag.Country = new SelectList(CRE.CountryMasters.ToList(), "Id", "Country");
        //    return View();
        //}
        [Authorize]
        public ActionResult AddEmployer()
        {
            ViewBag.Country = new SelectList(CRE.CountryMasters.ToList(), "Id", "Country");
            return View();
        }

        [HttpPost]
        public ActionResult Employer(EmployerMaster_Metadata EMM)
        {
            try
            {

                string selectedState1 = Request.Form["selState"];
                int selectedState = Convert.ToInt32(selectedState1);
                var UID = Session["UserId"];
                EmployerMaster EM = new EmployerMaster();
                EmployerDivisionMaster ETM = new EmployerDivisionMaster();
                EmployerBranchMaster EBM = new EmployerBranchMaster();
                EmployerContactDetail ECD = new EmployerContactDetail();


                var employerName = (from pn in CRE.EmployerMasters where pn.EmployerName == EMM.EmployerName select new { pn.EmployerName }).FirstOrDefault();


                if (employerName != null)
                {
                    var i = (from p in CRE.EmployerMasters where p.EmployerName == EMM.EmployerName select p.Id).FirstOrDefault();

                    int intUserId = Convert.ToInt32(Session["UserId"]);



                    ETM.EmployerId = Convert.ToInt32(i);
                    ETM.EmployerDivision = EMM.EmployerType;
                    ETM.CreateDate = DateTime.UtcNow.Date;
                    ETM.CreateId = Convert.ToDecimal(intUserId);
                    CRE.AddToEmployerDivisionMasters(ETM);
                    CRE.SaveChanges();

                    EBM.EmployerId = Convert.ToInt32(i);
                    EBM.EmployerDivisionId = ETM.Id;
                    EBM.BranchName = EMM.EmployerBranch;
                    EBM.CreateDate = DateTime.UtcNow.Date;
                    EBM.CreateId = Convert.ToDecimal(intUserId);
                    CRE.AddToEmployerBranchMasters(EBM);
                    CRE.SaveChanges();

                    ECD.EmployerId = Convert.ToInt32(i);
                    ECD.EmployerDivisionId = ETM.Id;
                    ECD.EmployerBranchId = EBM.Id;
                    ECD.Address1 = EMM.Address1;
                    ECD.Address2 = EMM.Address2;
                    ECD.Address3 = EMM.Address3;
                    ECD.CountryId = EMM.CountryId;
                    ECD.StateId = selectedState;
                    ECD.City = EMM.City;
                    ECD.Pincode = EMM.Pincode;
                    ECD.LandlineCode = EMM.LandlineCode;
                    ECD.LandlineNo = EMM.LandlineNo;
                    ECD.CreateDate = DateTime.UtcNow.Date;
                    ECD.CreateId = Convert.ToDecimal(intUserId);
                    ECD.EmailId = EMM.EmailId;
                    CRE.AddToEmployerContactDetails(ECD);
                    CRE.SaveChanges();

                }
                else
                {
                    int intUserId = Convert.ToInt32(Session["UserId"]);
                    EM.PanNo = EMM.PanNo;
                    EM.EmployerName = EMM.EmployerName;
                    EM.IncorporationDate = EMM.IncorporationDt;
                    EM.CommencementDate = EMM.CommencementDt;
                    EM.CreateDate = DateTime.UtcNow.Date;
                    EM.CreateId = Convert.ToDecimal(intUserId);
                    CRE.AddToEmployerMasters(EM);
                    CRE.SaveChanges();

                    ETM.EmployerId = EM.Id;
                    ETM.EmployerDivision = EMM.EmployerType;
                    ETM.CreateDate = DateTime.UtcNow.Date;
                    ETM.CreateId = Convert.ToDecimal(intUserId);
                    CRE.AddToEmployerDivisionMasters(ETM);
                    CRE.SaveChanges();

                    EBM.EmployerId = EM.Id;
                    EBM.EmployerDivisionId = ETM.Id;
                    EBM.BranchName = EMM.EmployerBranch;
                    EBM.CreateDate = DateTime.UtcNow.Date;
                    EBM.CreateId = Convert.ToDecimal(intUserId);
                    CRE.AddToEmployerBranchMasters(EBM);
                    CRE.SaveChanges();

                    ECD.EmployerId = EM.Id;
                    ECD.EmployerDivisionId = ETM.Id;
                    ECD.EmployerBranchId = EBM.Id;
                    ECD.Address1 = EMM.Address1;
                    ECD.Address2 = EMM.Address2;
                    ECD.Address3 = EMM.Address3;
                    ECD.CountryId = EMM.CountryId;
                    ECD.StateId = selectedState;
                    ECD.City = EMM.City;
                    ECD.Pincode = EMM.Pincode;
                    ECD.LandlineCode = EMM.LandlineCode;
                    ECD.LandlineNo = EMM.LandlineNo;
                    ECD.CreateDate = DateTime.UtcNow.Date;
                    ECD.CreateId = Convert.ToDecimal(intUserId);
                    ECD.EmailId = EMM.EmailId;
                    CRE.AddToEmployerContactDetails(ECD);
                    CRE.SaveChanges();
                }
            }
            catch (Exception e)
            {


            }
            ModelState.Clear();
            ViewBag.Country = new SelectList(CRE.CountryMasters.ToList(), "Id", "Country");
            return RedirectToAction("AddEmployer");
        }
        [HttpPost]
        public ActionResult CountryCode(string CountryId)
        {
            int id = Convert.ToInt32(CountryId);

            var CountryCode = (from s in CRE.CountryMasters
                               where s.Id == id
                               select s.CountryCode);
            return Json(CountryCode);

        }
     
        //public JsonResult EmployerName(string EmployerName )
        //{

        //    var Employer = (from e in CRE.EmployerMasters
        //                    where e.EmployerName == EmployerName
        //                    select e.EmployerName);
        //    return Json(Employer);

        //}
           [HttpPost]
        public JsonResult EmployerType(string EmployerType, string EmployerName)
        {

            var result = true;
            int empname = (from em in CRE.EmployerMasters where em.EmployerName == EmployerName select em.Id).FirstOrDefault();

            var Employer1 = (from e in CRE.EmployerDivisionMasters


                             where e.EmployerDivision == EmployerType && e.EmployerId == empname

                             select e.EmployerDivision).FirstOrDefault();
            if (Employer1 != null)
            {
                result = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);





        }


        public IList<StateMaster> BindState(int countryid)
        {

            return CRE.StateMasters.Where(s => s.CountryId == countryid).ToList();

        }


        public JsonResult StateList(string CountryId)
        {
            int id = Convert.ToInt32(CountryId);

            var states = (from s in CRE.StateMasters
                          where s.CountryId == id
                          select s);

            return Json(new SelectList(states.ToArray(), "Id", "State"), JsonRequestBehavior.AllowGet);

        }
        //----add emploer finished ----//
        //----employer list----//
        [Authorize]
        public ActionResult ViewEmployer()
        {
            var emp = (from emps in CRE.EmployerContactDetails
                       join em in CRE.EmployerMasters on emps.EmployerId equals em.Id
                       join empdiv in CRE.EmployerDivisionMasters on emps.EmployerDivisionId equals empdiv.Id
                       join empbranch in CRE.EmployerBranchMasters on emps.EmployerBranchId equals empbranch.Id
                       select new
                       {

                           id = emps.Id,

                           EmployerName = em.EmployerName,
                           EmployerDivision = empdiv.EmployerDivision,
                           BranchName = empbranch.BranchName
                       }
                 ).ToList();

            return View(emp);
        }
        //public ActionResult EmpList()
        //{

        //    var emp = (from emps in CRE.EmployerContactDetails
        //               join em in CRE.EmployerMasters on emps.EmployerId equals em.Id
        //               join empdiv in CRE.EmployerDivisionMasters on emps.EmployerDivisionId equals empdiv.Id
        //               join empbranch in CRE.EmployerBranchMasters on emps.EmployerBranchId equals empbranch.Id
        //               select new { 

        //                 id=emps.Id,

        //                 EmployerName = em.EmployerName,
        //             EmployerDivision=  empdiv.EmployerDivision,
        //           BranchName= empbranch.BranchName
        //               }
        //           ).ToList();

        //    return View(emp);
        //}
        //----employer list finished----//
        //----edit employer----//
        [Authorize]

        public ActionResult EmployerDetails(int id)
        {

            var emp = (from ecd in CRE.EmployerContactDetails
                       join em in CRE.EmployerMasters on ecd.EmployerId equals em.Id
                       join empdiv in CRE.EmployerDivisionMasters on ecd.EmployerDivisionId equals empdiv.Id
                       join empbranch in CRE.EmployerBranchMasters on ecd.EmployerBranchId equals empbranch.Id


                       join SM in CRE.StateMasters on ecd.StateId equals SM.Id
                       join CM in CRE.CountryMasters on ecd.CountryId equals CM.Id
                       where ecd.Id == id
                       select new EmployerMaster_Metadata
                       {
                           id = ecd.Id,

                           EmployerName = em.EmployerName,
                           EmployerType = empdiv.EmployerDivision,
                           EmployerBranch = empbranch.BranchName,
                           IncorporationDt = (em.IncorporationDate).Value,
                           CommencementDt = (em.CommencementDate).Value,
                           PanNo = em.PanNo,
                           Address1 = ecd.Address1,
                           Address2 = ecd.Address2,
                           Address3 = ecd.Address3,
                           CountryId = CM.Id,
                           StateId = SM.Id,
                           City = ecd.City,
                           Pincode = ecd.Pincode,
                           LandlineCode = ecd.LandlineCode,
                           LandlineNo = ecd.LandlineNo,
                           EmailId = ecd.EmailId
                       }).FirstOrDefault();

            decimal c = (from ecd in CRE.EmployerContactDetails
                         where ecd.Id == id
                         select ecd.CountryId).FirstOrDefault() ?? 0;

            decimal s = (from ecd in CRE.EmployerContactDetails
                         where ecd.Id == id
                         select ecd.StateId).FirstOrDefault() ?? 0;

            ViewBag.CountryList = new SelectList(CRE.CountryMasters.ToList(), "Id", "Country", c);
            ViewBag.StateList1 = new SelectList(CRE.StateMasters.ToList(), "Id", "State", s);

            return View(emp);
        }

        public ActionResult editemp(EmployerMaster_Metadata EMM)
        {
            var ecd = CRE.EmployerContactDetails.Where(x => x.Id == EMM.id).FirstOrDefault();

            int intUserId = Convert.ToInt32(Session["UserId"]);

            var a = (from ECD in CRE.EmployerContactDetails
                     join em in CRE.EmployerMasters on ECD.EmployerId equals em.Id
                     where ECD.Id == EMM.id
                     select ECD.EmployerId).FirstOrDefault();

            var b = Convert.ToInt32(a);

            var ems = CRE.EmployerMasters.Where(x => x.Id == b).FirstOrDefault();
            var es = (from e in CRE.EmployerMasters where e.Id == b select new { e.IncorporationDate, e.CommencementDate }).FirstOrDefault();
            if (EMM.IncorporationDt == es.IncorporationDate && es.CommencementDate == EMM.CommencementDt)
            {
            }
            else
            {
                ems.IncorporationDate = EMM.IncorporationDt;
                ems.CommencementDate = EMM.CommencementDt;
                ems.EditDate = DateTime.UtcNow.Date;
                ems.EditId = Convert.ToDecimal(intUserId);
            }



            ecd.Address1 = EMM.Address1;
            ecd.Address2 = EMM.Address2;
            ecd.Address3 = EMM.Address3;
            ecd.CountryId = EMM.CountryId;
            ecd.StateId = EMM.StateId;
            ecd.City = EMM.City;
            ecd.Pincode = EMM.Pincode;
            ecd.LandlineCode = EMM.LandlineCode;
            ecd.LandlineNo = EMM.LandlineNo;
            ecd.EmailId = EMM.EmailId;
            ecd.EditDate = DateTime.UtcNow.Date;
            ecd.EditId = Convert.ToDecimal(intUserId);





            CRE.SaveChanges();
            return RedirectToAction("ViewEmployer");
        }
        //----edit employer finished----//

        //----branch change----//
        [Authorize]
        public ActionResult AddBranch()
        {
            ViewBag.Country = new SelectList(CRE.CountryMasters.ToList(), "Id", "Country");
            ViewBag.EmpName = new SelectList(CRE.EmployerMasters.ToList(), "Id", "EmployerName");

            return View();
        }
        //public ActionResult DivisionChange()
        //{
        //    ViewBag.Country = new SelectList(CRE.CountryMasters.ToList(), "Id", "Country");
        //    ViewBag.EmpName = new SelectList(CRE.EmployerMasters.ToList(), "Id", "EmployerName");

        //    return View();
        //}
        public IList<EmployerDivisionMaster> BindDiv(int EmployerName)
        {


            return CRE.EmployerDivisionMasters.Where(s => s.EmployerId == EmployerName).ToList();

        }


        public JsonResult DivList(string EmployerName)
        {
            int empName = Convert.ToInt32(EmployerName);

            var B = (from s in CRE.EmployerDivisionMasters
                     where s.EmployerId == empName
                     select s);

            return Json(new SelectList(B.ToArray(), "Id", "EmployerDivision"), JsonRequestBehavior.AllowGet);
        }

        public ActionResult BranchDetails(EmployerMaster_Metadata EMM)
        {
            int intUserId = Convert.ToInt32(Session["UserId"]);
            string selectedState1 = Request.Form["selState"];
            int selectedState = Convert.ToInt32(selectedState1);
            var UID = Session["UserId"];
            string division = Request.Form["divsion"];
            int divsions = Convert.ToInt32(division);

            EmployerDivisionMaster ETM = new EmployerDivisionMaster();
            EmployerBranchMaster EBM = new EmployerBranchMaster();
            EmployerContactDetail ECD = new EmployerContactDetail();

            EBM.EmployerId = EMM.EmpName;
            EBM.EmployerDivisionId = divsions;
            EBM.BranchName = EMM.EmployerBranch;
            EBM.CreateDate = DateTime.UtcNow.Date;
            EBM.CreateId = Convert.ToDecimal(intUserId);
            CRE.AddToEmployerBranchMasters(EBM);
            CRE.SaveChanges();

            ECD.EmployerId = EMM.EmpName;
            ECD.EmployerDivisionId = divsions;
            ECD.EmployerBranchId = EBM.Id;
            ECD.Address1 = EMM.Address1;
            ECD.Address2 = EMM.Address2;
            ECD.Address3 = EMM.Address3;
            ECD.CountryId = EMM.CountryId;
            ECD.StateId = selectedState;
            ECD.City = EMM.City;
            ECD.Pincode = EMM.Pincode;
            ECD.LandlineCode = EMM.LandlineCode;
            ECD.LandlineNo = EMM.LandlineNo;
            ECD.CreateDate = DateTime.UtcNow.Date;
            EBM.CreateId = Convert.ToDecimal(intUserId);
            ECD.EmailId = EMM.EmailId;
            CRE.AddToEmployerContactDetails(ECD);
            CRE.SaveChanges();
            return RedirectToAction("AddBranch");
        }

        public JsonResult branchexist(int EmployerName, int divsion, string branch)
        {
            var result = true;
            var BRANCH = (from s in CRE.EmployerBranchMasters
                          where s.EmployerId == EmployerName && s.EmployerDivisionId == divsion && s.BranchName == branch
                          select s.BranchName).FirstOrDefault();
            if (BRANCH != null)
            {
                result = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);



        }
        //----branch change finished----//
    }
}

