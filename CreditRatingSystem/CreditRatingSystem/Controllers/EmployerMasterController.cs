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


        public ActionResult rview()
        {
            return View();
        }
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
             //ViewBag.StateList1 = new SelectList(CRE.StateMasters.ToList(), "Id", "State");
             return View();
         }
         
        [HttpPost]
        public ActionResult Employer(EmployerMaster_Metadata EMM)
        {
            try
            {

               
                var UID = Session["UserId"];
              
                EmployerContactDetail ECD = new EmployerContactDetail();
                
                
                var employerName= (from pn in CRE.EmployerMasters where pn.EmployerName == EMM.EmployerName select new { pn.EmployerName}).FirstOrDefault();


                if (employerName != null)
                {
                    var i = (from p in CRE.EmployerMasters where p.EmployerName == EMM.EmployerName select p.Id).FirstOrDefault();

                    int intUserId = Convert.ToInt32(Session["UserId"]);

                    string selectedState1 = Request.Form["selState"];
                    int selectedState = Convert.ToInt32(selectedState1);

                    string selectedCity = Request.Form["selCity"];
                    int selectedCity1 = Convert.ToInt32(selectedCity);

                    //foreach (var item in EMM.EmployerTypes)
                    //{
                        EmployerDivisionMaster etm = new EmployerDivisionMaster();

                        etm.EmployerId = Convert.ToInt32(i);
                        etm.EmployerDivision = EMM.EmployerType;
                        etm.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                        etm.CreateId = Convert.ToDecimal(intUserId);
                        CRE.AddToEmployerDivisionMasters(etm);
                        CRE.SaveChanges();
                        //foreach (var Item in EMM.EmployerBranches)
                        //{
                            EmployerBranchMaster ebm = new EmployerBranchMaster();
                            ebm.EmployerId = Convert.ToInt32(i);
                            ebm.EmployerDivisionId = etm.Id;
                            ebm.BranchName = EMM.EmployerBranch;
                            ebm.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                            ebm.CreateId = Convert.ToDecimal(intUserId);
                            CRE.AddToEmployerBranchMasters(ebm);
                            CRE.SaveChanges();

                            ECD.EmployerId = Convert.ToInt32(i);
                            ECD.EmployerDivisionId = etm.Id;
                            ECD.EmployerBranchId = ebm.Id;
                            ECD.Address1 = EMM.Address1;
                            ECD.Address2 = EMM.Address2;
                            ECD.Address3 = EMM.Address3;
                            ECD.CountryId = EMM.CountryId;
                            ECD.StateId = selectedState;
                            ECD.CityId = selectedCity1;
                            ECD.Pincode = EMM.Pincode;
                            ECD.LandlineCode = EMM.LandlineCode;
                            ECD.LandlineNo = EMM.LandlineNo;
                            ECD.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                            ECD.CreateId = Convert.ToDecimal(intUserId);
                            ECD.EmailId = EMM.EmailId;
                            CRE.AddToEmployerContactDetails(ECD);
                            CRE.SaveChanges();


                        //}

                   
                    //}







                    //ECD.EmployerId = Convert.ToInt32(i);
                    //ECD.EmployerDivisionId = ETM.Id;
                    //ECD.EmployerBranchId = EBM.Id;
                    //ECD.Address1 = EMM.Address1;
                    //ECD.Address2 = EMM.Address2;
                    //ECD.Address3 = EMM.Address3;
                    //ECD.CountryId = EMM.CountryId;
                    ////ECD.StateId = selectedState;
                    ////ECD.CityId = selectedcity;
                    //ECD.Pincode = EMM.Pincode;
                    //ECD.LandlineCode = EMM.LandlineCode;
                    //ECD.LandlineNo = EMM.LandlineNo;
                    //ECD.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                    //ECD.CreateId = Convert.ToDecimal(intUserId);
                    //ECD.EmailId = EMM.EmailId;
                    //CRE.AddToEmployerContactDetails(ECD);
                    //CRE.SaveChanges();

                }
                else
                {
                    int intUserId = Convert.ToInt32(Session["UserId"]);
                    string selectedState1 = Request.Form["selState"];
                    int selectedState = Convert.ToInt32(selectedState1);
                    string selectedCity = Request.Form["selCity"];
                    int selectedcity = Convert.ToInt32(selectedCity);
                    EmployerMaster EM = new EmployerMaster();

                    
                   // EM.PanNo = EMM.PanNo;
                    EM.EmployerName = EMM.EmployerName;
                    EM.IncorporationDate = EMM.IncorporationDt;
                    EM.CommencementDate = EMM.CommencementDt;
                    EM.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                    EM.CreateId = Convert.ToDecimal(intUserId);
                    CRE.AddToEmployerMasters(EM);
                    CRE.SaveChanges();

                    //var d = EMM.EmployerTypes;
                    //var b = EMM.EmployerBranches;
                    //  var y= d.FirstOrDefault();

                    //  foreach (var item in EMM.EmployerTypes)
                    //  {
                    //  //for (int i = 0; i <= d.Count; i++)
                    //  //{



                    EmployerDivisionMaster EDM = new EmployerDivisionMaster();

                    EDM.EmployerId = EM.Id;
                    EDM.EmployerDivision = EMM.EmployerType;
                    EDM.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                    EDM.CreateId = Convert.ToDecimal(intUserId);
                    CRE.AddToEmployerDivisionMasters(EDM);
                    CRE.SaveChanges();
                      

                    //}
                
                        //foreach (var itemS in EMM.EmployerBranches )
                        //{
                    EmployerBranchMaster EBM =  new EmployerBranchMaster();
                    EBM.EmployerId = EM.Id;
                    EBM.EmployerDivisionId = EDM.Id;
                    EBM.BranchName = EMM.EmployerBranch;
                    EBM.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                    EBM.CreateId = Convert.ToDecimal(intUserId);
                    CRE.AddToEmployerBranchMasters(EBM);
                    CRE.SaveChanges();

                            ECD.EmployerId = EM.Id;
                           ECD.EmployerDivisionId = EDM.Id;
                            ECD.EmployerBranchId = EBM.Id;
                            ECD.Address1 = EMM.Address1;
                            ECD.Address2 = EMM.Address2;
                            ECD.Address3 = EMM.Address3;
                            ECD.CountryId = EMM.CountryId;
                            ECD.StateId = selectedState;
                            ECD.CityId = selectedcity;
                            ECD.Pincode = EMM.Pincode;
                            ECD.LandlineCode = EMM.LandlineCode;
                            ECD.LandlineNo = EMM.LandlineNo;
                            ECD.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                            ECD.CreateId = Convert.ToDecimal(intUserId);
                            ECD.EmailId = EMM.EmailId;
                            CRE.AddToEmployerContactDetails(ECD);
                            CRE.SaveChanges();
                            //goto v;
                        //}

                    

                    //}

                

                 
                }
            }
            catch(Exception )
            {
                //string message = e.Message;
             
            }
            ModelState.Clear();
            ViewBag.Country = new SelectList(CRE.CountryMasters.ToList(), "Id", "Country");
            return Content(("<script language='javascript' type='text/javascript'> alert('Data saved successfully!'); window.location.href ='/EmployerMaster/AddEmployer';  </script>"));
            //return RedirectToAction("AddEmployer");
        }
        [HttpPost]
        public string CityCode(string selCity)
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
        public ActionResult CountryCode(string countryid)
        {
            int id = Convert.ToInt32(countryid);

            var countryId = (from s in CRE.CountryMasters
                      where s.Id == id
                       select s.CountryCode);
            return Json(countryId);
     


        } 
        //int id = Convert.ToInt32(selCity);

            //var selsCity = (from s in CRE.CityMasters
            //                where s.Id == id
            //                select s.CityCode);
            //return Json(selCity);
     
        //public JsonResult EmployerName(string EmployerName )
        //{

        //    var Employer = (from e in CRE.EmployerMasters
        //                    where e.EmployerName == EmployerName
        //                    select e.EmployerName);
        //    return Json(Employer);

        //}
        public JsonResult EmployerType(string EmployerType, string EmployerName)
        {

            var result = true;
         
            var Employer1 = (from e in CRE.EmployerDivisionMasters
                            join Em in CRE.EmployerMasters on e.EmployerId equals Em.Id
                           
                             where e.EmployerDivision == EmployerType && Em.EmployerName == EmployerName

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
        //----add emploer finished ----//
        //----employer list----//
        [Authorize]
        public ActionResult ViewEmployer()
        {
            var emp = (from emps in CRE.EmployerContactDetails
                       join em in CRE.EmployerMasters on emps.EmployerId equals em.Id
                       join empdiv in CRE.EmployerDivisionMasters on emps.EmployerDivisionId equals empdiv.Id
                       join empbranch in CRE.EmployerBranchMasters on emps.EmployerBranchId equals empbranch.Id
                       orderby em.EmployerName, empdiv.EmployerDivision, empbranch.BranchName
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
                           id =  ecd.Id,

                           EmployerName = em.EmployerName,
                           EmployerType = empdiv.EmployerDivision,
                           EmployerBranch = empbranch.BranchName,
                           IncorporationDt = (em.IncorporationDate).Value,
                           CommencementDt = (em.CommencementDate).Value,
                           PanNo = em.PanNo,
                           Address1=ecd.Address1,
                           Address2 = ecd.Address2,
                           Address3 = ecd.Address3,
                           CountryId=CM.Id,
                           StateId=SM.Id,
                           Cityid = ecd.CityId.Value,
                           Pincode=ecd.Pincode,
                           LandlineCode=ecd.LandlineCode,
                           LandlineNo=ecd.LandlineNo,
                           EmailId = ecd.EmailId
                       }).FirstOrDefault();

            decimal c = (from ecd in CRE.EmployerContactDetails
                         where ecd.Id == id
                         select ecd.CountryId).FirstOrDefault() ?? 0;

            decimal s = (from ecd in CRE.EmployerContactDetails
                         where ecd.Id == id && ecd.CountryId== c
                         select ecd.StateId).FirstOrDefault() ?? 0;
           
             
             //decimal CC = (from ecd in CRE.EmployerMasters
             //             where ecd.Id == id && ecd.co == c && ecd.StateId == s
             //             select ecd.CityId).FirstOrDefault() ?? 0;


            ViewBag.CountryList = new SelectList(CRE.CountryMasters.ToList(), "Id", "Country", c);
            ViewBag.StateList1 = new SelectList((from sm in CRE.StateMasters where sm.CountryId == c select new { sm.Id, sm.State }).ToList(), "Id", "State");
            ViewBag.CityList1 = new SelectList((from cm in CRE.CityMasters where cm.StateId == s select new { cm.Id, cm.City }).ToList(), "Id", "City");


            return View(emp);
        }
  
        public ActionResult editemp(EmployerMaster_Metadata EMM)
        {
            int intUserId = Convert.ToInt32(Session["UserId"]);
            //--employermaster---//
            var ecd = CRE.EmployerContactDetails.Where(x => x.Id == EMM.id).FirstOrDefault();

            var E =(from ECD in CRE.EmployerContactDetails
                       join em in CRE.EmployerMasters on ECD.EmployerId equals em.Id
                       where ECD.Id == EMM.id 
                        select   ECD.EmployerId).FirstOrDefault();

            var Em = Convert.ToInt32(E);
            var ems = CRE.EmployerMasters.Where(x => x.Id == Em ).FirstOrDefault();

            //---employerDivsion---//
            var D = (from ECD in CRE.EmployerContactDetails
                     join em in CRE.EmployerDivisionMasters on ECD.EmployerDivisionId equals em.Id
                     where ECD.Id == EMM.id
                     select ECD.EmployerDivisionId).FirstOrDefault();

            var d = Convert.ToInt32(D);
            var Dm = CRE.EmployerDivisionMasters.Where(x => x.Id == d).FirstOrDefault();

            //----employerBranch----//
            var B = (from ECD in CRE.EmployerContactDetails
                     join ee in CRE.EmployerBranchMasters on ECD.EmployerBranchId equals ee.Id
                     where ECD.Id == EMM.id
                     select ECD.EmployerBranchId).FirstOrDefault();

            var b = Convert.ToInt32(B);
            var bm = CRE.EmployerBranchMasters.Where(x => x.Id == b).FirstOrDefault();

         //   var es = (from e in CRE.EmployerMasters where e.Id == b select new { e.EmployerName,e.CommencementDate,e.IncorporationDate}).FirstOrDefault();
         //if (EMM.IncorporationDt == es.IncorporationDate && es.CommencementDate==EMM.CommencementDt && es.EmployerName==EMM.EmployerName)
         //{
         //}
         //else
         //{    
             ems.EmployerName = EMM.EmployerName;
             ems.IncorporationDate = EMM.IncorporationDt;
             ems.CommencementDate = EMM.CommencementDt;
             ems.EditDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
             ems.EditId = Convert.ToDecimal(intUserId);
             CRE.SaveChanges();
            //}
             Dm.EmployerDivision = EMM.EmployerType;
             Dm.EditDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
             Dm.EditId = Convert.ToDecimal(intUserId);
             CRE.SaveChanges();
             //}
             bm.BranchName = EMM.EmployerBranch;
             bm.EditDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
             bm.EditId = Convert.ToDecimal(intUserId);
             CRE.SaveChanges();

            ecd.Address1=EMM.Address1;
            ecd.Address2=EMM.Address2;
            ecd.Address3 = EMM.Address3;
            ecd.CountryId = EMM.CountryId;
            ecd.StateId = EMM.StateId;
            ecd.CityId = EMM.Cityid;
            ecd.Pincode = EMM.Pincode;
            ecd.LandlineCode = EMM.LandlineCode;
            ecd.LandlineNo = EMM.LandlineNo;
            ecd.EmailId = EMM.EmailId;
            ecd.EditDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
            ecd.EditId = Convert.ToDecimal(intUserId);

            CRE.SaveChanges();
            return Content(("<script language='javascript' type='text/javascript'> alert('Data saved successfully!'); window.location.href ='/EmployerMaster/ViewEmployer';  </script>"));
            //return RedirectToAction("ViewEmployer");
        }
        //----edit employer finished----//

    //----if branch change----//
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

        public ActionResult AddBranchDetails(EmployerMaster_Metadata EMM)
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
            EBM.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
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
            ECD.CityId = divsions;
            ECD.Pincode = EMM.Pincode;
            ECD.LandlineCode = EMM.LandlineCode;
            ECD.LandlineNo = EMM.LandlineNo;
            ECD.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
            ECD.CreateId = Convert.ToDecimal(intUserId);
            ECD.EmailId = EMM.EmailId;
            CRE.AddToEmployerContactDetails(ECD);
            CRE.SaveChanges();
            return Content(("<script language='javascript' type='text/javascript'> alert('Data saved successfully!'); window.location.href ='/EmployerMaster/AddBranch';  </script>"));
            //return RedirectToAction("AddBranch");
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

        public ActionResult ViewBranch()
        
        {
            var b = (from br in CRE.EmployerBranchMasters
                     join ed in CRE.EmployerDivisionMasters on br.EmployerDivisionId equals ed.Id
                     join en in CRE.EmployerMasters on br.EmployerId equals en.Id
                     orderby en.EmployerName, br.BranchName, ed.EmployerDivision
                     select new 

              {    id=br.Id,
                   EmployerName=en.EmployerName,
                     Branch = br.BranchName,
                     Division=ed.EmployerDivision
              }).ToList();




            return View(b);
        
        }
        public ActionResult BranchDetails(int id)
        {

            var b = (from br in CRE.EmployerBranchMasters
                     join ed in CRE.EmployerDivisionMasters on br.EmployerDivisionId equals ed.Id
                     join en in CRE.EmployerMasters on br.EmployerId equals en.Id
                     join cd in CRE.EmployerContactDetails on br.Id equals cd.EmployerBranchId
                     join SM in CRE.StateMasters on cd.StateId equals SM.Id
                     join CM in CRE.CountryMasters on cd.CountryId equals CM.Id
                     where br.Id==id
                     select new EmployerMaster_Metadata

                     {
                        id=br.Id,

                         EmployerName = en.EmployerName,
                         EmployerBranch = br.BranchName,
                         EmployerType = ed.EmployerDivision,
                         Address1 = cd.Address1,
                        Address2 = cd.Address2,
                        Address3 = cd.Address3,
                         CountryId = CM.Id,
                         StateId = SM.Id,
                         Cityid = cd.CityId.Value,
                         Pincode = cd.Pincode,
                         LandlineCode = cd.LandlineCode,
                         LandlineNo = cd.LandlineNo,
                         EmailId = cd.EmailId


                     }).FirstOrDefault();



            decimal c = (from ecd in CRE.EmployerContactDetails
                         where ecd.EmployerBranchId==id
                         select ecd.CountryId).FirstOrDefault() ?? 0;

            decimal s = (from ecd in CRE.EmployerContactDetails
                         where ecd.EmployerBranchId == id && ecd.CountryId == c
                         select ecd.StateId).FirstOrDefault() ?? 0;


            //decimal CC = (from ecd in CRE.CollegeMasters
            //              where ecd.Id == id && ecd.CountryId == c && ecd.StateId == s
            //              select ecd.CityId).FirstOrDefault() ?? 0;


            ViewBag.CountryList = new SelectList(CRE.CountryMasters.ToList(), "Id", "Country", c);
            ViewBag.StateList1 = new SelectList((from sm in CRE.StateMasters where sm.CountryId == c select new { sm.Id, sm.State }).ToList(), "Id", "State");
            ViewBag.CityList1 = new SelectList((from cm in CRE.CityMasters where cm.StateId == s select new { cm.Id, cm.City }).ToList(), "Id", "City");
            return View(b);
        }
        public ActionResult EditBranch(EmployerMaster_Metadata EMM)
        {

            var ecd = CRE.EmployerContactDetails.Where(x => x.EmployerBranchId == EMM.id).FirstOrDefault();

            int intUserId = Convert.ToInt32(Session["UserId"]);

       
            ecd.Address1 = EMM.Address1;
            ecd.Address2 = EMM.Address2;
            ecd.Address3 = EMM.Address3;
            ecd.CountryId = EMM.CountryId;
            ecd.StateId = EMM.StateId;
            ecd.CityId = EMM.Cityid;
            ecd.Pincode = EMM.Pincode;
            ecd.LandlineCode = EMM.LandlineCode;
            ecd.LandlineNo = EMM.LandlineNo;
            ecd.EmailId = EMM.EmailId;
            ecd.EditDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
            ecd.EditId = Convert.ToDecimal(intUserId);

            CRE.SaveChanges();
           // return View();
            return Content(("<script language='javascript' type='text/javascript'> alert('Data saved successfully!'); window.location.href ='/EmployerMaster/ViewBranch';  </script>"));
       

        }
        ////////////////////////ADD DIVISION/////////////////////////////
        [Authorize]
        public ActionResult AddEmployerDivision()
        {
            ViewBag.Employer = new SelectList(CRE.EmployerMasters.ToList(), "Id", "EmployerName");
           
            return View();
        }
        [HttpPost]
        public ActionResult AddDivision(EmployerMaster_Metadata emm) 
         {
              int intUserId = Convert.ToInt32(Session["UserId"]);

             EmployerDivisionMaster Edm = new EmployerDivisionMaster();
             Edm.EmployerId =emm.EmployerID;
             Edm.EmployerDivision = emm.EmployerType;
             Edm.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
             Edm.CreateId = intUserId;
             CRE.AddToEmployerDivisionMasters(Edm);
             CRE.SaveChanges();

             return RedirectToAction("AddEmployerDivision");
        }




        [Authorize]
        public ActionResult EditEmployerDivision(int id)
        {
            var Division = (from Div in CRE.EmployerDivisionMasters
                            where Div.Id == id
                            select new EmployerMaster_Metadata
                            {
                                EmployerID = Div.EmployerId.Value,
                                EmployerType = Div.EmployerDivision

                            }).FirstOrDefault();
            ViewBag.div = Division.EmployerID;
            ViewBag.EmployerId = new SelectList(CRE.EmployerMasters.ToList(), "Id", "EmployerName");
            return View(Division);
        }

          [HttpPost]
        public ActionResult EditEmployerDivision(EmployerMaster_Metadata em)
        {
            int intUserId = Convert.ToInt32(Session["UserId"]);

              var D= CRE.EmployerDivisionMasters.Where (x=>x.Id==em.id).FirstOrDefault();
              D.EmployerDivision = em.EmployerType;
              D.EditDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
              D.EditId = intUserId;
              CRE.SaveChanges();
              return Content(("<script language='javascript' type='text/javascript'> alert('Data saved successfully!'); window.location.href ='/EmployerMaster/ViewDivision';  </script>"));
        }



          public ActionResult doesEmployerTypeExist(string EmployerType, int EmpName)
        {
            var result = true;
              //var user = CRE.EmployerDivisionMasters.Where(x => x.EmployerDivision == EmployerType.Trim()).FirstOrDefault();

            var user = (from ed in CRE.EmployerDivisionMasters where ed.EmployerDivision == EmployerType.Trim() && ed.EmployerId == EmpName select ed.EmployerDivision).FirstOrDefault();
              if (user != null)
            {
                result = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }



        public ActionResult ViewDivision()
        {
            var Division = (from Div in CRE.EmployerDivisionMasters
                            join em in CRE.EmployerMasters on Div.EmployerId equals em.Id
                            orderby Div.EmployerDivision ,  em.EmployerName
                            select new 
                            {
                                id=Div.Id,
                         EmployerDivsion=   Div.EmployerDivision,
                        EmployerName=    em.EmployerName
                            
                            }).ToList();
            return View(Division);
        
        }
    }

    }

