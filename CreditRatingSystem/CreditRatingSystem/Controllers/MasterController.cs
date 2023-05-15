using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CreditRatingModel.Model;
using System.Transactions;


namespace CreditRatingSystem.Controllers
{
    public class MasterController : Controller
    {
        //
        // GET: /Master/
        private creaditratingEntities db = new creaditratingEntities();
        Master M = new Master();

        public ActionResult Index()
        {
            return View();
        }
        public void dropList(Master M)
        {
            ViewBag.CreditType = new SelectList(db.CreditTypeMasters.ToList(), "Id", "CreditType");
            ViewBag.Country = new SelectList(db.CountryMasters.ToList(), "Id", "Country");
            ViewBag.State = new SelectList(db.StateMasters.ToList(), "Id", "State");
        }
        //-----------------Loan Type----------------//

        public ActionResult AddLoanType()
        {
            Master M = new Master();
            dropList(M);
            return View(M);
        }
        public ActionResult EditLoanType(int id)
        {
            var LoanTypeDetails = (from LTM in db.LoanTypesMasters
                                   from CTM in db.CreditTypeMasters
                                   where LTM.Id == id
                                   select new Master
                                   {
                                       Id = LTM.Id,
                                       LoanType = LTM.Type,
                                       CreditTypeId = LTM.LoanTypeId.Value,
                                       Category = LTM.Category
                                   }).FirstOrDefault();
            dropList(M);
            return View(LoanTypeDetails);
        }
        [HttpPost]
        public ActionResult LoanType(Master M)
        {
            int CreateId = Convert.ToInt32(Session["UserId"]);
            using (TransactionScope transaction = new TransactionScope())
            {
                try
                {
                    if (M.Id == 0)
                    {
                        //InsertInfo1(M.LoanType, M.Category, M.CreditTypeId, CreateId);
                        LoanTypesMaster LTM = new LoanTypesMaster();
                        LTM.LoanTypeId = M.CreditTypeId;
                        LTM.Type = M.LoanType;
                        LTM.Category = M.Category;
                        LTM.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                        LTM.CreateId = CreateId;
                        db.AddToLoanTypesMasters(LTM);
                        db.SaveChanges();
                        transaction.Complete();
                        return Content(("<script language='javascript' type='text/javascript'> alert('Record save successfully!'); window.location.href ='/Master/AddLoanType';  </script>"));
                    }
                    else
                    {
                        var LTM = db.LoanTypesMasters.Where(x => x.Id == M.Id).FirstOrDefault();

                        LTM.LoanTypeId = M.CreditTypeId;
                        LTM.Type = M.LoanType;
                        LTM.Category = M.Category;
                        LTM.EditDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                        LTM.EditId = CreateId;
                        db.SaveChanges();
                        transaction.Complete();
                        return Content(("<script language='javascript' type='text/javascript'> alert('Record save successfully!'); window.location.href ='/Master/ViewLoanType';  </script>"));
                    }
                }
                catch (Exception e)
                {
                    transaction.Dispose();
                    string message = e.Message;
                }
            }
            ModelState.Clear();
            dropList(M);
            return View();
        }
        public JsonResult doesLoanType(string LoanType)
        {
            var result = true;
            var user = db.LoanTypesMasters.Where(x => x.Type == LoanType).FirstOrDefault();
            if (user != null)
            {
                result = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ViewLoanType()
        {
            var List = (from LTM in db.LoanTypesMasters
                        join CTM in db.CreditTypeMasters on LTM.LoanTypeId equals CTM.Id
                        orderby LTM.Type.Trim()
                        select new
                        {
                            LTM.Id,
                            LTM.Type,
                            CTM.CreditType,
                            category = LTM.Category.Equals("C") ? "Credit Card" : "Loan"
                        }
                ).ToList();

            return View(List);

        }

        //-----------CountryMaster---------------//

        public ActionResult AddCountry()
        {
            dropList(M);
            return View(M);
        }
        public ActionResult EditCountry(int id)
        {
            var CountryDetails = (from CM in db.CountryMasters
                                  where CM.Id == id
                                  select new Master
                                  {
                                      CountryName = CM.Country,
                                      CountryCode = CM.CountryCode,
                                      Id1 = CM.Id
                                  }
                ).FirstOrDefault();
            dropList(M);
            return View(CountryDetails);
        }
        [HttpPost]
        public ActionResult CountryMaster(Master M)
        {
            int CreateId = Convert.ToInt32(Session["UserId"]);
            using (TransactionScope transaction = new TransactionScope())
            {
                try
                {
                    if (M.Id1 == 0)
                    {
                        CountryMaster CM = new CountryMaster();
                        CM.Country = M.CountryName;
                        CM.CountryCode = M.CountryCode;
                        CM.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                        CM.CreateId = CreateId;
                        db.AddToCountryMasters(CM);
                        db.SaveChanges();
                        transaction.Complete();
                        return Content(("<script language='javascript' type='text/javascript'> alert('Record save successfully!'); window.location.href ='/Master/AddCountry';  </script>"));
                    }
                    else
                    {
                        var CM = db.CountryMasters.Where(x => x.Id == M.Id1).FirstOrDefault();

                        CM.Country = M.CountryName;
                        CM.CountryCode = M.CountryCode;
                        CM.EditDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                        CM.EditId = CreateId;
                        db.SaveChanges();
                        transaction.Complete();
                        return Content(("<script language='javascript' type='text/javascript'> alert('Record save successfully!'); window.location.href ='/Master/ViewCountry';  </script>"));
                    }
                }
                catch (Exception e)
                {
                    transaction.Dispose();
                    string message = e.Message;
                }
            }
            ModelState.Clear();
            dropList(M);
            return View();
        }
        public JsonResult doesCountry(string CountryName)
        {
            var result = true;
            var user = db.CountryMasters.Where(x => x.Country == CountryName).FirstOrDefault();
            if (user != null)
            {
                result = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult doesCountryCode(string CountryCode, int Id)
        {
            var result = true;
            if (Id == 0)
            {
                var user = db.CountryMasters.Where(x => x.CountryCode == CountryCode).FirstOrDefault();
                if (user != null)
                {
                    result = false;
                }
            }
            else
            {
                var user = db.CountryMasters.Where(x => x.CountryCode == CountryCode && x.Id != Id).FirstOrDefault();
                if (user != null)
                {
                    result = false;
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ViewCountry()
        {
            var List = (from CM in db.CountryMasters
                        orderby CM.Country.Trim()
                        select new
                        {
                            CM.Country,
                            CM.CountryCode,
                            CM.Id
                        }
                ).ToList();

            return View(List);

        }


        //--------------StateMaster-----------------------//

        public ActionResult AddProvince()
        {
            dropList(M);
            return View(M);
        }
        public ActionResult EditProvince(int id)
        {
            var List = (from SM in db.StateMasters
                        join CM in db.CountryMasters on SM.CountryId equals CM.Id
                        where SM.Id == id
                        select new Master
                        {
                            StateName = SM.State,
                            CountryId1 = SM.CountryId.Value,
                            Id1 = SM.Id
                        }
             ).FirstOrDefault();
            dropList(M);
            return View(List);
        }
        [HttpPost]
        public ActionResult AddProvince(Master M)
        {
            int CreateId = Convert.ToInt32(Session["UserId"]);
            using (TransactionScope transaction = new TransactionScope())
            {
                try
                {
                    if (M.Id1 == 0)
                    {
                        var province = (from b in db.StateMasters
                                        where b.State == M.StateName
                                        select b.State).FirstOrDefault();



                        if (province != null)
                        {
                            ViewBag.e = "e";
                            dropList(M);
                            return View("AddProvince", M);

                        }
                        else
                        {
                            StateMaster SM = new StateMaster();

                            SM.CountryId = M.CountryId;
                            SM.State = M.StateName;
                            SM.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                            SM.CreateId = CreateId;
                            db.AddToStateMasters(SM);
                            db.SaveChanges();
                            transaction.Complete();
                            ModelState.Clear();
                            return Content(("<script language='javascript' type='text/javascript'> alert('Record save successfully!'); window.location.href ='/Master/AddProvince';  </script>"));
                        }
                    }
                    else
                    {
                        var province = (from b in db.StateMasters
                                        where b.State == M.StateName && b.CountryId == M.CountryId1
                                        select b.State).FirstOrDefault();



                        if (province != null)
                        {
                            ViewBag.e = "e";
                            dropList(M);
                            return View("AddProvince", M);

                        }
                        else
                        {
                            var SM = db.StateMasters.Where(x => x.Id == M.Id1).FirstOrDefault();

                            SM.CountryId = M.CountryId1;
                            SM.State = M.StateName;
                            SM.EditDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                            SM.EditId = CreateId;
                            db.SaveChanges();
                            transaction.Complete();
                            return Content(("<script language='javascript' type='text/javascript'> alert('Record save successfully!'); window.location.href ='/Master/ViewProvince';  </script>"));
                        }
                    }
                }

                catch (Exception e)
                {
                    transaction.Dispose();
                    string message = e.Message;
                }
            }

            dropList(M);
            return View("AddProvince");
        }
        public JsonResult doesState(string StateName, int CountryId, int Id)
        {
            var result = true;
            if (Id == 0)
            {
                var user = db.StateMasters.Where(x => x.State == StateName && x.CountryId == CountryId).FirstOrDefault();
                if (user != null)
                {
                    result = false;
                }
            }
            else
            {
                var user = db.StateMasters.Where(x => x.State == StateName && x.CountryId == CountryId && x.Id != Id).FirstOrDefault();
                if (user != null)
                {
                    result = false;
                }
            }
            //var user = Strsql;

            //if (user != null)
            //{
            //    result = false;
            //}
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ViewProvince()
        {
            var List = (from SM in db.StateMasters
                        join CM in db.CountryMasters on SM.CountryId equals CM.Id
                        orderby SM.State.Trim(), CM.Country.Trim()
                        select new
                        {
                            SM.State,
                            CM.Country,
                            SM.Id
                        }
                ).ToList();

            return View(List);

        }

        //-----------CityMaster---------------//


        public ActionResult AddCity()
        {
            dropList(M);
            return View(M);
        }
        public ActionResult EditCIty(int id)
        {
            var CItyDetails = (from CM in db.CityMasters
                               join SM in db.StateMasters on CM.StateId equals SM.Id
                               where CM.Id == id
                               select new Master
                               {
                                   CityName = CM.City,
                                   CityCode = CM.CityCode,
                                   StateId1 = CM.StateId.Value,
                                   CountryId1 = SM.CountryId.Value,
                                   Id1 = CM.Id
                               }
                ).FirstOrDefault();
            dropList(M);
            var StateName = (from S in db.StateMasters where S.CountryId == CItyDetails.CountryId1 select new { S.Id, S.State });
            ViewBag.State = new SelectList(StateName.ToList(), "Id", "State", CItyDetails.StateId1);
            return View(CItyDetails);
        }
        [HttpPost]
        public ActionResult CityMaster(Master M)
        {
            string selectedState1 = Request.Form["selState"];
            int selectedState = Convert.ToInt32(selectedState1);
            int CreateId = Convert.ToInt32(Session["UserId"]);
            using (TransactionScope transaction = new TransactionScope())
            {
                try
                {
                    if (M.Id1 == 0)
                    {
                        CityMaster CM = new CityMaster();
                        CM.City = M.CityName;
                        CM.CityCode = M.CityCode;
                        CM.StateId = selectedState;
                        CM.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                        CM.CreateId = CreateId;
                        db.AddToCityMasters(CM);
                        db.SaveChanges();
                        transaction.Complete();
                        return Content(("<script language='javascript' type='text/javascript'> alert('Record save successfully!'); window.location.href ='/Master/AddCity';  </script>"));
                    }
                    else
                    {
                        var CM = db.CityMasters.Where(x => x.Id == M.Id1).FirstOrDefault();

                        CM.City = M.CityName;
                        CM.CityCode = M.CityCode;
                        CM.StateId = M.StateId1;
                        CM.EditDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                        CM.EditId = CreateId;
                        db.SaveChanges();
                        transaction.Complete();
                        return Content(("<script language='javascript' type='text/javascript'> alert('Record save successfully!'); window.location.href ='/Master/ViewCity';  </script>"));
                    }
                }
                catch (Exception e)
                {
                    transaction.Dispose();
                    string message = e.Message;
                }
            }
            ModelState.Clear();
            dropList(M);
            return View();
        }
        public JsonResult doesCity(string CityName)
        {
            var result = true;
            var user = db.CityMasters.Where(x => x.City == CityName).FirstOrDefault();
            if (user != null)
            {
                result = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult doesCityCode(string CityCode)
        {
            var result = true;
            var user = db.CityMasters.Where(x => x.CityCode == CityCode).FirstOrDefault();
            if (user != null)
            {
                result = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult BindStateList(string CountryId)
        {
            int id = Convert.ToInt32(CountryId);
            var states = (from s in db.StateMasters
                          where s.CountryId == id
                          select s);
            return Json(new SelectList(states.ToArray(), "Id", "State"), JsonRequestBehavior.AllowGet);
        }
        public ActionResult ViewCity()
        {
            var List = (from CM in db.CityMasters
                        join SM in db.StateMasters on CM.StateId equals SM.Id
                        orderby CM.City.Trim()
                        select new
                        {
                            CM.City,
                            CM.CityCode,
                            SM.State,
                            CM.Id
                        }
                ).ToList();

            return View(List);

        }

        //-------------------Nationality---------------//

        public ActionResult AddNationality(Master m)
        {
            if (m.Id == 0)
            {
                return View();
            }
            else
            {
                var r = (from R in db.NationalityMasters
                         where R.Id == m.Id
                         select new Master

                         {

                             nationality = R.Nationality
                         }).FirstOrDefault();
                return View(r);
            }
        }
        public ActionResult Nationality(Master m)
        {
            int intUserId = Convert.ToInt32(Session["UserId"]);
            using (TransactionScope transaction = new TransactionScope())
            {
                try
                {
                    if (m.Id == 0)
                    {
                        var s = (from r in db.NationalityMasters
                                 where r.Nationality == m.nationality
                                 select r.Nationality).FirstOrDefault();

                        if (s != null)
                        {
                            return Content(("<script language='javascript' type='text/javascript'> alert('Nationality Already Exist!');window.location.href ='/Master/AddNationality';</script>"));


                        }
                        else
                        {
                            NationalityMaster CM = new NationalityMaster();
                            CM.Nationality = m.nationality;

                            CM.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                            CM.CreateId = intUserId;

                            db.AddToNationalityMasters(CM);
                            db.SaveChanges();
                            transaction.Complete();
                            return Content(("<script language='javascript' type='text/javascript'> alert('Data saved successfully!'); window.location.href ='/Master/AddNationality';  </script>"));
                        }
                    }
                    else
                    {

                        var CM = db.NationalityMasters.Where(x => x.Id == m.Id).FirstOrDefault();


                        CM.Nationality = m.nationality;

                        CM.EditDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                        CM.EditId = intUserId;


                        db.SaveChanges();
                        transaction.Complete();
                        return Content(("<script language='javascript' type='text/javascript'> alert('Data saved successfully!'); window.location.href ='/Master/ViewNationality';  </script>"));

                    }
                }
                catch (Exception e)
                {
                    transaction.Dispose();
                    string message = e.Message;

                }
            }
            ModelState.Clear();
            return View();
        }
        public ActionResult ViewNationality()
        {
            var List = (from NM in db.NationalityMasters
                        orderby NM.Nationality.Trim()
                        select new
                        {
                            nationality = NM.Nationality,
                            id = NM.Id
                        }
                ).ToList();

            return View(List);

        }

        //-----------DesignationMaster---------------//

        public ActionResult AddDesignation()
        {

            return View();
        }
        public ActionResult EditDesignation(int id)
        {
            var DesignationDetails = (from DM in db.DesignationMasters
                                      where DM.Id == id
                                      select new Master
                                      {
                                          DesignationName = DM.Designation,
                                          DesignationType = DM.DesignationType,
                                          Id1 = DM.Id
                                      }
              ).FirstOrDefault();
            //dropList(M);
            return View(DesignationDetails);
        }
        [HttpPost]
        public ActionResult AddDesignation(Master M)
        {
            int CreateId = Convert.ToInt32(Session["UserId"]);
            using (TransactionScope transaction = new TransactionScope())
            {
                try
                {
                    if (M.Id1 == 0)
                    {
                        //var s = (from r in db.DesignationMasters
                        //         where r.Designation == M.DesignationName
                        //         select r.Designation).FirstOrDefault();
                        //if (s != null)
                        //{
                        //    return Content(("<script language='javascript' type='text/javascript'> alert('Designation Already Exist!');window.location.href ='/Master/AddDesignation';</script>"));


                        //}
                        //else
                        //{
                        DesignationMaster DM = new DesignationMaster();
                        DM.Designation = M.DesignationName;
                        DM.DesignationType = M.DesignationType;
                        DM.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                        DM.CreateId = CreateId;
                        db.AddToDesignationMasters(DM);
                        db.SaveChanges();
                        transaction.Complete();
                        return Content(("<script language='javascript' type='text/javascript'> alert('Record save successfully!'); window.location.href ='/Master/AddDesignation';  </script>"));
                        // }
                    }
                    else
                    {
                        //var s = (from r in db.DesignationMasters
                        //         where r.Designation == M.DesignationName
                        //         select r.Designation).FirstOrDefault();
                        //if (s != null)
                        //{
                        //    return Content(("<script language='javascript' type='text/javascript'> alert('Designation Already Exist!');window.location.href ='/Master/AddDesignation';</script>"));


                        //}
                        //else
                        // {
                        var DM = db.DesignationMasters.Where(x => x.Id == M.Id1).FirstOrDefault();

                        DM.Designation = M.DesignationName;
                        DM.DesignationType = M.DesignationType;
                        DM.EditDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                        DM.EditId = CreateId;
                        db.SaveChanges();
                        transaction.Complete();
                        return Content(("<script language='javascript' type='text/javascript'> alert('Record save successfully!'); window.location.href ='/Master/ViewDesignation';  </script>"));
                        //}
                    }
                }
                catch (Exception e)
                {
                    transaction.Dispose();
                    string message = e.Message;
                }
            }
            ModelState.Clear();
            return View();
        }
        public JsonResult doesDesignation(int Id, string DesignationName)
        {
            var result = true;
            // var user = db.DesignationMasters.Where(x => x.Designation == DesignationName).FirstOrDefault();
            //if (user != null)
            //{
            //    result = false;
            //}
            //return Json(result, JsonRequestBehavior.AllowGet);

            var sqlstr = (from CC in db.DesignationMasters
                          where CC.Designation == DesignationName
                          select new
                          {
                              CC.Designation,
                              CC.Id

                          }

               ).FirstOrDefault();

            if (sqlstr == null)
            {
                string str = "";
                return Json(str, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (Id != sqlstr.Id)
                {
                    string str = "N";
                    return Json(str, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(sqlstr, JsonRequestBehavior.AllowGet);
                }
            }
        }
        public ActionResult ViewDesignation()
        {
            var List = (from DM in db.DesignationMasters
                        orderby DM.Designation.Trim()
                        select new
                        {
                            DM.Designation,
                            DesignationType = DM.DesignationType.Equals("E") ? "Employee" : "College",
                            DM.Id
                        }
                ).ToList();

            return View(List);

        }

        //-----------DepartmentMaster-----------------//

        public ActionResult AddDepartment()
        {
            return View();
        }
        public ActionResult EditDepartment(int id)
        {
            var DepartmentDetails = (from DM in db.DepartmentMasters
                                     where DM.Id == id
                                     select new Master
                                     {
                                         DepartmentName = DM.Department,
                                         Id1 = DM.Id
                                     }
                           ).FirstOrDefault();

            return View(DepartmentDetails);
        }
        [HttpPost]
        public ActionResult AddDepartment(Master M)
        {
            int CreateId = Convert.ToInt32(Session["UserId"]);
            using (TransactionScope transaction = new TransactionScope())
            {
                try
                {
                    if (M.Id1 == 0)
                    {
                        var s = (from r in db.DepartmentMasters
                                 where r.Department == M.DepartmentName
                                 select r.Department).FirstOrDefault();
                        if (s != null)
                        {
                            return Content(("<script language='javascript' type='text/javascript'> alert('Department Already Exist!');window.location.href ='/Master/AddDepartment';</script>"));


                        }
                        else
                        {
                            DepartmentMaster DeM = new DepartmentMaster();
                            DeM.Department = M.DepartmentName;
                            DeM.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                            DeM.CreateId = CreateId;
                            db.AddToDepartmentMasters(DeM);
                            db.SaveChanges();
                            transaction.Complete();
                            return Content(("<script language='javascript' type='text/javascript'> alert('Record save successfully!'); window.location.href ='/Master/AddDepartment';  </script>"));
                        }
                    }
                    else
                    {
                        var s = (from r in db.DepartmentMasters
                                 where r.Department == M.DepartmentName
                                 select r.Department).FirstOrDefault();
                        if (s != null)
                        {
                            return Content(("<script language='javascript' type='text/javascript'> alert('Department Already Exist!');window.location.href ='/Master/AddDepartment';</script>"));


                        }
                        else
                        {
                            var DeM = db.DepartmentMasters.Where(x => x.Id == M.Id1).FirstOrDefault();

                            DeM.Department = M.DepartmentName;
                            DeM.EditDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                            DeM.EditId = CreateId;
                            db.SaveChanges();
                            transaction.Complete();
                            return Content(("<script language='javascript' type='text/javascript'> alert('Record save successfully!'); window.location.href ='/Master/ViewDepartment';  </script>"));
                        }
                    }
                }
                catch (Exception e)
                {
                    transaction.Dispose();
                    string message = e.Message;
                }
            }
            ModelState.Clear();
            return View();
        }
        public JsonResult doesDepartment(string DepartmentName)
        {
            var result = true;
            var user = db.DepartmentMasters.Where(x => x.Department == DepartmentName).FirstOrDefault();
            if (user != null)
            {
                result = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ViewDepartment()
        {
            var List = (from DM in db.DepartmentMasters

                        orderby DM.Department.Trim()
                        select new
                        {
                            DM.Department,
                            DM.Id
                        }
                ).ToList();

            return View(List);

        }

        //-------------------StreamMaster------------------------//

        public ActionResult AddStream()
        {
            return View();
        }
        public ActionResult EditStream(int id)
        {
            var StreamDetails = (from SM in db.StreamMasters
                                 where SM.Id == id
                                 select new Master
                                 {
                                     StreamName = SM.Stream,
                                     Id1 = SM.Id
                                 }
                          ).FirstOrDefault();
            return View(StreamDetails);
        }
        [HttpPost]
        public ActionResult AddStream(Master M)
        {
            int CreateId = Convert.ToInt32(Session["UserId"]);
            using (TransactionScope transaction = new TransactionScope())
            {
                try
                {
                    if (M.Id1 == 0)
                    {
                        var fields = (from b in db.StreamMasters
                                      where b.Stream == M.StreamName
                                      select b.Stream).FirstOrDefault();



                        if (fields != null)
                        {
                            ViewBag.e = "e";
                            dropList(M);
                            return View("AddStream", M);

                        }
                        else
                        {
                            StreamMaster StM = new StreamMaster();
                            StM.Stream = M.StreamName;
                            StM.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                            StM.CreateId = CreateId;
                            db.AddToStreamMasters(StM);
                            db.SaveChanges();
                            transaction.Complete();
                            ModelState.Clear();
                            return Content(("<script language='javascript' type='text/javascript'> alert('Data save successfully!'); window.location.href ='/Master/AddStream';  </script>"));
                        }
                    }
                    else
                    {
                        var fields = (from b in db.StreamMasters
                                      where b.Stream == M.StreamName
                                      select b.Stream).FirstOrDefault();



                        if (fields != null)
                        {
                            ViewBag.e = "e";
                            dropList(M);
                            return View("AddStream", M);

                        }
                        else
                        {
                            var StM = db.StreamMasters.Where(x => x.Id == M.Id1).FirstOrDefault();

                            StM.Stream = M.StreamName;
                            StM.EditDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                            StM.EditId = CreateId;
                            db.SaveChanges();
                            transaction.Complete();
                            return Content(("<script language='javascript' type='text/javascript'> alert('Data save successfully!'); window.location.href ='/Master/ViewStream';  </script>"));
                        }
                    }
                }
                catch (Exception e)
                {
                    transaction.Dispose();
                    string message = e.Message;
                }
            }
            ModelState.Clear();

            return View();
        }

        public JsonResult doesStream(int Id, string StreamName)
        {
            //var result = true;
            var sqlstr = db.StreamMasters.Where(x => x.Stream == StreamName).FirstOrDefault();
            //if (user != null)
            //{
            //    result = false;
            //}
            //return Json(result, JsonRequestBehavior.AllowGet);

            if (sqlstr == null)
            {
                string str = "";
                return Json(str, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (Id != sqlstr.Id)
                {
                    string str = "N";
                    return Json(str, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(sqlstr, JsonRequestBehavior.AllowGet);
                }
            }

        }
        public ActionResult ViewStream()
        {
            var List = (from SM in db.StreamMasters
                        orderby SM.Stream.Trim()
                        select new
                        {
                            SM.Stream,
                            SM.Id
                        }
                ).ToList();

            return View(List);

        }

        //--------------CourseCategory---------------------------//

        public ActionResult AddCourseCategory(Master MCC)
        {
            if (MCC.Id == 0)
            {
                return View();
            }
            else
            {
                var sqlstr = (from cc in db.CourseCategories
                              where cc.Id == MCC.Id
                              select new Master
                              {
                                  CourseCategory = cc.CourseCategory_Description,
                                  Id = cc.Id
                              }
                              ).FirstOrDefault();
                return View(sqlstr);
            }

        }

        public string checkCourseCategory(int catid, string catname)
        {



            var sqlstr = (from CC in db.CourseCategories
                          where CC.CourseCategory_Description == catname
                          select new
                            {
                                CC.CourseCategory_Description,
                                CC.Id

                            }

                ).FirstOrDefault();

            if (sqlstr == null)
            {
                string str;
                return str = "";
            }
            else
            {
                if (catid != sqlstr.Id)
                {
                    string str;
                    return str = "N";
                }
                else
                {
                    return sqlstr.ToString();
                }
            }


        }
        [HttpPost]
        public ActionResult CourseCategory1(int CourseCategoryid, string CourseCategoryName)
        {
            var strSql = checkCourseCategory(CourseCategoryid, CourseCategoryName);
            var result = false;
            if (strSql == "N")
            {
                result = false;
            }
            else
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    int userID = Convert.ToInt32(Session["UserId"]);
                    try
                    {

                        if (CourseCategoryid == 0)
                        {
                            CourseCategory CC = new CourseCategory();
                            CC.CourseCategory_Description = CourseCategoryName;
                            CC.CreateId = userID;
                            CC.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                            db.AddToCourseCategories(CC);

                        }
                        else
                        {
                            var courseCatId = db.CourseCategories.Where(x => x.Id == CourseCategoryid).FirstOrDefault();
                            if (TryUpdateModel(courseCatId))
                            {
                                courseCatId.CourseCategory_Description = CourseCategoryName;
                                courseCatId.EditDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                                courseCatId.EditId = userID;

                            }


                        }
                        db.SaveChanges();
                        transaction.Complete();
                        var output = new { result = true, msg = "Data Saved Successfully." };
                        return Json(output, JsonRequestBehavior.AllowGet);



                    }
                    catch (Exception e)
                    {
                        transaction.Dispose();
                        return Json(new { msg = "Data Saved Faild." + e.ToString() }, JsonRequestBehavior.AllowGet);
                        //string message = e.Message;

                    }
                }
            }
            ModelState.Clear();
            //return View();
            return Json(result, JsonRequestBehavior.AllowGet);


        }


        public ActionResult ViewCourseCategory()
        {
            var sqlstr = (from cc in db.CourseCategories
                          orderby cc.CourseCategory_Description
                          select new
                          {
                              cc.CourseCategory_Description,
                              cc.Id
                          }
                ).ToList();

            return View(sqlstr);

        }


        //------------------- Academic Year ---------------------//

        public ActionResult AddAcademicYear()
        {
            dropList(M);
            return View();
        }

        public string CheckRecordExists(int intCountryId, string strAyId, int id)
        {
            var result = (from AY in db.AcademicYears
                          where AY.CountryId == intCountryId && AY.AyId == strAyId
                          select new
                          {
                              AY.CountryId,
                              AY.AyId,
                              AY.Id
                          }).FirstOrDefault();
            if (result == null)
            {
                string RecordExists = "";
                return RecordExists;

            }
            else
            {
                if (id != result.Id)
                {
                    string RecordExists = "N";
                    return RecordExists;
                }
                else
                {
                    return result.ToString();
                }
            }
        }

        [HttpPost]
        public ActionResult AddAcademicYear(Master M)
        {
            int CreateId = Convert.ToInt32(Session["UserId"]);
            int id = 0;
            var strSql = CheckRecordExists(M.CountryId, M.AyId, id);

            if (strSql == "N")
            {
                return Content(("<script language='javascript' type='text/javascript'> alert('Record already exists!');window.location.href ='/Master/AddAcademicYear';</script>"));
            }
            else
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    try
                    {
                        AcademicYear ADY = new AcademicYear();

                        ADY.CountryId = M.CountryId;
                        ADY.AyFromDate = M.AyFromDate;
                        ADY.AyToDate = M.AyToDate;
                        ADY.AyId = M.AyId;

                        ADY.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                        ADY.CreateId = CreateId;

                        db.AddToAcademicYears(ADY);
                        db.SaveChanges();
                        transaction.Complete();
                        return Content(("<script language='javascript' type='text/javascript'> alert('Data saved successfully!');window.location.href ='/Master/AddAcademicYear';</script>"));
                    }
                    catch (Exception e)
                    {
                        transaction.Dispose();
                        string message = e.Message;
                    }
                }
                ModelState.Clear();
            }

            return View();
        }


        public ActionResult ViewAcademicYear()
        {

            var strsql = (from ay in db.AcademicYears
                          join cm in db.CountryMasters on ay.CountryId equals cm.Id
                          orderby ay.AyId, cm.Country
                          select new
                          {
                              cm.Country,
                              ay.AyFromDate,
                              ay.AyToDate,
                              ay.AyId,
                              ay.Id


                          }).ToList();
            return View(strsql);
        }

        public ActionResult EditAcademicYear(int id)
        {
            dropList(M);
            var AcademicYearDetails = (from AY in db.AcademicYears


                                       where AY.Id == id
                                       select new Master
                                       {
                                           CountryId1 = AY.CountryId.Value,
                                           AyFromDate = AY.AyFromDate.Value,
                                           AyToDate = AY.AyToDate.Value,
                                           AyId = AY.AyId,
                                           Id1 = AY.Id
                                       }
                           ).FirstOrDefault();
            return View(AcademicYearDetails);
        }

        [HttpPost]
        public ActionResult EditAcademicYearPost(int Id, int CountryId, DateTime fromdate, DateTime todate, string year)
        {
            int EditId = Convert.ToInt32(Session["UserId"]);
            var result = false;
            var strSql = CheckRecordExists(CountryId, year, Id);

            if (strSql == "N")
            {
                result = false;
                //return Content(("<script language='javascript' type='text/javascript'> alert('Record already exists!');window.location.href ='/Master/AcademicYear';</script>"));
            }
            else
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    try
                    {
                        var acdemicYearId = db.AcademicYears.Where(x => x.Id == Id).FirstOrDefault();
                        if (TryUpdateModel(acdemicYearId))
                        {


                            acdemicYearId.CountryId = CountryId;
                            acdemicYearId.AyFromDate = fromdate;
                            acdemicYearId.AyToDate = todate;
                            acdemicYearId.AyId = year;

                            acdemicYearId.EditDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                            acdemicYearId.EditId = EditId;
                            db.SaveChanges();
                            transaction.Complete();
                            result = true;
                        }
                        //   return Content(("<script language='javascript' type='text/javascript'> alert('Record saved successfully!');window.location.href ='/Master/AcademicYearList';</script>"));
                    }
                    catch (Exception e)
                    {
                        transaction.Dispose();
                        string message = e.Message;
                    }
                }
            }
            ModelState.Clear();
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        //-----Relationship master---//

        public ActionResult AddRelationship(Master m)
        {
            if (m.Id == 0)
            {
                return View();
            }
            else
            {
                var r = (from R in db.Relationships
                         where R.Id == m.Id
                         select new Master

                         {
                             //    Id = R.Id,

                             relation = R.Relation
                         }).FirstOrDefault();
                return View(r);
            }
        }



        public JsonResult ChkRelation(int Id, string Relation)
        {

            var sqlstr = (from s in db.Relationships
                          where s.Relation == Relation
                          select new
                          {
                              s.Relation,
                              s.Id

                          }

             ).FirstOrDefault();

            if (sqlstr == null)
            {
                string str = "";
                //return str = "";
                return Json(str, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (Id != sqlstr.Id)
                {
                    string str = "N";
                    //return str = "N";
                    return Json(str, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //return sqlstr.ToString();
                    return Json(sqlstr, JsonRequestBehavior.AllowGet);
                }
            }

        }
        [HttpPost]
        public ActionResult RelationshipMaster(Master m)
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                try
                {
                    if (m.Id == 0)
                    {
                        //var s = (from r in db.Relationships
                        //         where r.Relation == m.relation
                        //         select r.Relation).FirstOrDefault();
                        //if (s != null)
                        //{
                        //    return Content(("<script language='javascript' type='text/javascript'> alert('Relationship Already Exist!');window.location.href ='/Master/AddRelationship';</script>"));


                        //}
                        //else
                        //{
                        int CreateId = Convert.ToInt32(Session["UserId"]);

                        Relationship rm = new Relationship();
                        rm.Relation = m.relation;
                        rm.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                        rm.CreateId = CreateId;
                        db.AddToRelationships(rm);
                        db.SaveChanges();
                        transaction.Complete();
                        return Content(("<script language='javascript' type='text/javascript'> alert('Data saved successfully!');window.location.href ='/Master/AddRelationship';</script>"));
                        //}
                    }
                    else
                    {
                        //var s = (from r in db.Relationships
                        //         where r.Relation == m.relation
                        //         select r.Relation).FirstOrDefault();
                        //if (s != null)
                        //{
                        //    return Content(("<script language='javascript' type='text/javascript'> alert('Relationship Already Exist!');window.location.href ='/Master/AddRelationship';</script>"));


                        //}
                        //else
                        //{
                        int editId = Convert.ToInt32(Session["UserId"]);

                        var rm = db.Relationships.Where(x => x.Id == m.Id).FirstOrDefault();

                        rm.Relation = m.relation;
                        rm.EditDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                        rm.EditId = editId;
                        db.SaveChanges();
                        transaction.Complete();
                        return Content(("<script language='javascript' type='text/javascript'> alert('Data saved successfully!');window.location.href ='/Master/ViewRelationship';</script>"));
                        //}
                    }
                }
                catch (Exception e)
                {
                    transaction.Dispose();
                    string message = e.Message;
                }
            }

            return View();
        }

        public ActionResult ViewRelationship()
        {
            var r = (from R in db.Relationships
                     orderby R.Relation.Trim()
                     select new
          {
              id = R.Id,
              relation = R.Relation

          }).ToList();


            return View(r);

        }
        //public JsonResult existRElationship(string relation) 
        //{


        //    var s = (from r in db.Relationships
        //             where r.Relation == relation
        //             select r.Relation).FirstOrDefault();

        //    return Json(s, JsonRequestBehavior.AllowGet);
        //}


        //-----------------------Slab Master--------------------//
        public ActionResult AddSlab(Master m)
        {
            if (m.Id == 0)
            {

                //ViewBag.slab = "s";
                return View();
            }
            else
            {

                // ViewBag.e = "e";
                var slab = (from s in db.SlabMasters
                            where s.Id == m.Id
                            select new Master
                            {
                                Id = m.Id,
                                Rangefrom = s.RangeFrom.Value,
                                RangeTo = s.RangeTo.Value,
                                SlabPercentage = s.SlabPercentage.Value
                            }).FirstOrDefault();
                return View(slab);
            }
        }

        [HttpPost]
        public ActionResult Slab(Master m)
        {

            using (TransactionScope transaction = new TransactionScope())
            {
                try
                {
                    if (m.Id == 0)
                    {
                        decimal rf = m.Rangefrom;
                        decimal rt = m.RangeTo;
                        decimal s = m.SlabPercentage;
                        if (rf != 0 && rt != 0 && s != 0)
                        {
                            //var slabEx = (from b in db.SlabMasters
                            //              where b.SlabPercentage == m.SlabPercentage
                            //              select b.SlabPercentage).FirstOrDefault();



                            //if (slabEx != null)
                            // {

                            //return View("AddSlab", m);

                            //  }
                            // else
                            //  {

                            int CreateId = Convert.ToInt32(Session["UserId"]);

                            SlabMaster sm = new SlabMaster();
                            sm.RangeFrom = m.Rangefrom;
                            sm.RangeTo = m.RangeTo;
                            sm.SlabPercentage = m.SlabPercentage;
                            sm.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                            sm.CreateId = CreateId;
                            db.AddToSlabMasters(sm);

                            db.SaveChanges();
                            transaction.Complete();
                        }

                        return Content(("<script language='javascript' type='text/javascript'> alert('Data saved successfully!');window.location.href ='/Master/AddSlab';</script>"));


                        // }

                    }
                    //  else
                    //  {
                    //     return View("AddSlab");

                       // }
                    //  }
                    // else {
                    //var slabEx = (from b in db.SlabMasters
                    //            where b.SlabPercentage == m.SlabPercentage
                    //            select b.SlabPercentage).FirstOrDefault();



                      //  if (slabEx != null)
                    //{

                        //    return View("AddSlab", m);
                    //
                    // }
                    else
                    {
                        int CreateId = Convert.ToInt32(Session["UserId"]);
                        var slabs = db.SlabMasters.Where(x => x.Id == m.Id).FirstOrDefault();
                        if (TryUpdateModel(slabs))
                        {
                            slabs.RangeFrom = m.Rangefrom;
                            slabs.RangeTo = m.RangeTo;
                            slabs.SlabPercentage = m.SlabPercentage;
                            slabs.EditDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                            slabs.EditId = CreateId;
                            db.SaveChanges();
                            transaction.Complete();
                        }


                        return Content(("<script language='javascript' type='text/javascript'> alert('Data saved successfully!');window.location.href ='/Master/ViewSlab';</script>"));
                    }

                }
                catch (Exception e)
                {
                    transaction.Dispose();
                    string message = e.Message;
                }
            }
            return View();
        }
        // }

        public ActionResult ViewSlab()
        {
            var slab = (from s in db.SlabMasters
                        orderby s.RangeFrom
                        select new
                        {
                            id = s.Id,
                            RangeFrom = s.RangeFrom,
                            RangeTo = s.RangeTo,
                            SlabPercentage = s.SlabPercentage
                        }).ToList();

            return View(slab);
        }


        public JsonResult slabChkFrom(int Id, int RangeFrom)
        {

            var sqlstr = (from s in db.SlabMasters
                          where s.RangeFrom == RangeFrom
                          select new
                          {
                              s.RangeFrom,
                              s.Id

                          }

             ).FirstOrDefault();

            if (sqlstr == null)
            {
                string str = "";
                //return str = "";
                return Json(str, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (Id != sqlstr.Id)
                {
                    string str = "N";
                    //return str = "N";
                    return Json(str, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //return sqlstr.ToString();
                    return Json(sqlstr, JsonRequestBehavior.AllowGet);
                }
            }

        }


        public JsonResult slabFromexist(int RangeFrom)
        {



            var s = (from r in db.SlabMasters
                     orderby r.RangeTo descending
                     select r.RangeTo).FirstOrDefault();




            return Json(s, JsonRequestBehavior.AllowGet);
        }




        public JsonResult PercentageExits(int Id, int SlabPercentage)
        {


            var sqlstr = (from s in db.SlabMasters
                          where s.SlabPercentage == SlabPercentage
                          select new
                          {
                              s.SlabPercentage,
                              s.Id

                          }

             ).FirstOrDefault();

            if (sqlstr == null)
            {
                string str = "";
                //return str = "";
                return Json(str, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (Id != sqlstr.Id)
                {
                    string str = "N";
                    //return str = "N";
                    return Json(str, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //return sqlstr.ToString();
                    return Json(sqlstr, JsonRequestBehavior.AllowGet);
                }
            }

        }

        public JsonResult slabgreaterRate(int Rangefrom, int RangeTo)
        {

            var result = true;
            //var Rangef = (from b in db.SlabMasters
            //              where b.RangeFrom == Rangefrom
            //              select b.RangeFrom).FirstOrDefault();
            if (RangeTo <= Rangefrom)
            {
                result = false;
            }



            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult SlabsmallerRate(int Rangefrom, int RangeTo)
        {

            var result = true;
            //var Rangef = (from b in db.SlabMasters
            //              where b.RangeFrom == Rangefrom
            //              select b.RangeFrom).FirstOrDefault();
            if (Rangefrom >= RangeTo)
            {
                result = false;
            }



            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ChkToValue(int Id, int RangeTo)
        {
            string str = "";
            var strsql = (db.sp_chkToVal(Id, RangeTo)).ToList();



            if (strsql != null)
            {
                str = "N";
            }
            return Json(str, JsonRequestBehavior.AllowGet);
        }




        //public JsonResult RangeFromexist(int Rangefrom)
        //{



        //    var s = (from r in db.SlabMasters
        //             orderby r.RangeTo descending
        //             select r.RangeTo).FirstOrDefault();




        //    return Json(s, JsonRequestBehavior.AllowGet);
        //}











        //public ActionResult rangetoexist(int RangeTo)
        //{
        //    var s = (from S in db.SlabMasters
        //             where S.RangeTo == RangeTo
        //             select S.RangeTo).FirstOrDefault();


        //    return Json(s, JsonRequestBehavior.AllowGet);
        //}
        //public JsonResult greaterSlab(decimal Rangefrom, decimal RangeTo)
        //{

        //    var result=true;
        //    //var Rangef = (from b in db.SlabMasters
        //    //              where b.RangeFrom == Rangefrom
        //    //              select b.RangeFrom).FirstOrDefault();
        //    if (RangeTo <= Rangefrom)
        //    {
        //        result = false;
        //    }



        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}


        //------------------------Threshold---------------------//


        public ActionResult AddThreshold()
        {
            //dropList(M);
            return View();
        }
        public ActionResult EditThreshold(int id)
        {
            var ThresholdDetails = (from ETM in db.EduThresholdMasters
                                    where ETM.Id == id
                                    select new Master
                                    {
                                        AyFromDate = ETM.FromDate.Value,
                                        AyToDate = ETM.ToDate.Value,
                                        AyId = ETM.FinancialYear,
                                        ThresholdAmt = ETM.ThresholdValue.Value,
                                        Id1 = ETM.Id
                                    }
                ).FirstOrDefault();

            return View(ThresholdDetails);
        }
        [HttpPost]
        public ActionResult ThresholdMaster(Master M)
        {
            int CreateId = Convert.ToInt32(Session["UserId"]);
            using (TransactionScope transaction = new TransactionScope())
            {

                try
                {
                    if (M.Id1 == 0)
                    {
                        EduThresholdMaster ETM = new EduThresholdMaster();
                        ETM.ThresholdValue = M.ThresholdAmt;
                        ETM.FromDate = M.AyFromDate;
                        ETM.ToDate = M.AyToDate;
                        ETM.FinancialYear = M.AyId;
                        ETM.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                        ETM.CreateId = CreateId;
                        db.AddToEduThresholdMasters(ETM);
                        db.SaveChanges();
                        transaction.Complete();
                        return Content(("<script language='javascript' type='text/javascript'> alert('Data save successfully!'); window.location.href ='/Master/AddThreshold';  </script>"));
                    }
                    else
                    {
                        var ETM = db.EduThresholdMasters.Where(x => x.Id == M.Id1).FirstOrDefault();

                        ETM.ThresholdValue = M.ThresholdAmt;
                        ETM.FromDate = M.AyFromDate;
                        ETM.ToDate = M.AyToDate;
                        ETM.FinancialYear = M.AyId;
                        ETM.EditDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                        ETM.EditId = CreateId;
                        db.SaveChanges();
                        transaction.Complete();
                        return Content(("<script language='javascript' type='text/javascript'> alert('Data save successfully!'); window.location.href ='/Master/ViewThreshold';  </script>"));
                    }
                }
                catch (Exception e)
                {
                    transaction.Dispose();
                    string message = e.Message;
                }
            }
            ModelState.Clear();

            return View();
        }
        public JsonResult doesExitsFinancialYear(int Id, string FinancialYear)
        {
            var result = true;
            //var sqlstr = (from s in db.EduThresholdMasters.Where(x => x.FinancialYear == FinancialYear).FirstOrDefault();
            //if (user != null)
            //{
            //    result = false;
            //}
            //return Json(result, JsonRequestBehavior.AllowGet);
            var sqlstr = (from s in db.EduThresholdMasters
                          where s.FinancialYear == FinancialYear
                          select new
                          {
                              s.FinancialYear,
                              s.Id

                          }

           ).FirstOrDefault();

            if (sqlstr == null)
            {
                string str = "";
                //return str = "";
                return Json(str, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (Id != sqlstr.Id)
                {
                    string str = "N";
                    //return str = "N";
                    return Json(str, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //return sqlstr.ToString();
                    return Json(sqlstr, JsonRequestBehavior.AllowGet);
                }
            }

        }
        public ActionResult ViewThreshold()
        {
            var Threshold = (from s in db.EduThresholdMasters
                             orderby s.Id descending
                             select new
                             {
                                 s.Id,
                                 s.FromDate,
                                 s.ToDate,
                                 s.ThresholdValue,
                                 s.FinancialYear
                             }).ToList();

            return View(Threshold);
        }


        //-------------DISTRIBUTIONTYPEMASTER----------------------------//

        public ActionResult AddDistributionType(Master M)
        {

            if (M.Id == 0)
            {
                return View();
            }
            else
            {
                var query = (from s in db.DistributionTypeMasters
                             orderby s.Name
                             where s.Id == M.Id
                             select new Master
                             {
                                 Id = s.Id,
                                 Name = s.Name,
                                 ShortName = s.ShortName,
                                 CoPartner = s.CoPartner
                             }).FirstOrDefault();
                return View(query);
            }

        }

        [HttpPost]
        public ActionResult AddDistributionType1(int Id, string Name, string ShortName, string CoPartner)
        {

            //var strSql = Nameexist(Name);
            var result = false;
            //if (strSql == "N")
            //{
            //    result = false;
            //}
            int userID = Convert.ToInt32(Session["UserId"]);
            using (TransactionScope transaction = new TransactionScope())
            {
                try
                {
                    if (Id == 0)
                    {

                        DistributionTypeMaster DTP = new DistributionTypeMaster();
                        DTP.Name = Name;
                        DTP.ShortName = ShortName;
                        DTP.CoPartner = CoPartner;
                        DTP.CreateId = userID;
                        DTP.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                        db.AddToDistributionTypeMasters(DTP);
                        db.SaveChanges();
                        transaction.Complete();
                        //ModelState.Clear();
                        // return Content(("<script language='javascript' type='text/javascript'> alert('Data save successfully!'); window.location.href ='/Master/AddDistributionType';  </script>"));                          
                    }



                    else
                    {
                        var DTP = db.DistributionTypeMasters.Where(x => x.Id == Id).FirstOrDefault();
                        DTP.Name = Name;
                        DTP.ShortName = ShortName;
                        DTP.CoPartner = CoPartner;
                        DTP.EditId = userID;
                        DTP.EditDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                        db.SaveChanges();
                        transaction.Complete();
                        //return Content(("<script language='javascript' type='text/javascript'> alert('Data save successfully!'); window.location.href ='/Master/AddDistributionType';  </script>"));
                    }
                    result = true;
                }
                catch (Exception e)
                {
                    transaction.Dispose();
                    string message = e.Message;
                }
            }
            ModelState.Clear();
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public string Nameexist(int Id, string Name)
        {
            var result = false;
            var names = (from s in db.DistributionTypeMasters
                         where s.Name == Name
                         select new
                             {
                                 s.Name,
                                 s.Id
                             }).FirstOrDefault();
            if (names == null)
            {
                string DT = "";
                return DT;
            }
            else
            {
                if (Id != names.Id)
                {

                    string DT = "N";
                    return DT;
                }
                else
                {
                    return names.ToString();
                }

            }

            // return Content(("<script language='javascript' type='text/javascript'> alert('Record saved successfully!');window.location.href ='/Master/AcademicYearList';</script>"));

        }


        public string ShortNameexist(int Id, string ShortName)
        {
            var result = false;
            var ShortNames = (from s in db.DistributionTypeMasters
                              where s.ShortName == ShortName
                              select new
                              {
                                  s.Id,
                                  s.ShortName
                              }).FirstOrDefault();
            if (ShortNames == null)
            {
                string SN = "";
                return SN;
            }
            else
            {
                if (Id != ShortNames.Id)
                {

                    string SN = "N";
                    return SN;
                }
                else
                {
                    return ShortNames.ToString();
                }

            }

        }
        public string CoPartnerexist(int Id, string CoPartner)
        {
            var result = false;
            var CoPartners = (from s in db.DistributionTypeMasters
                              where s.CoPartner == CoPartner
                              select new
                              {
                                  s.Id,
                                  s.CoPartner
                              }).FirstOrDefault();
            if (CoPartners == null)
            {
                string CP = "";
                return CP;
            }
            else
            {
                if (Id != CoPartners.Id)
                {

                    string CP = "N";
                    return CP;
                }
                else
                {
                    return CoPartners.ToString();
                }

            }
        }


        public ActionResult ViewDistributionType()
        {
            var query = (from s in db.DistributionTypeMasters
                         orderby s.Name

                         select new
                         {
                             Id = s.Id,
                             Name = s.Name,
                             ShortName = s.ShortName,
                             CoPartner = s.CoPartner
                         }).ToList();

            return View(query);
        }



        //------------DISAGREEMENTYEAR------------------//

        public ActionResult AddDisAgreementYear(Master M)
        {
            if (M.Id == 0)
            {
                ViewBag.Start = "";

                return View();
            }
            else
            {
                ViewBag.Start = "startDate";
                var sqlstr = (from s in db.DisAgreementYears
                              where s.Id == M.Id
                              select new Master
                              {
                                  EffectiveDate = s.EffectiveDate,
                                  Naration = s.Naration,
                                  Id = s.Id
                              }).FirstOrDefault();
                return View(sqlstr);
            }

        }

        [HttpPost]
        public ActionResult AddDisAgreementYear1(int Id, DateTime EffectiveDate, string Naration)
        {
            var result = false;
            int UserId = Convert.ToInt32(Session["UserId"]);
            using (TransactionScope transaction = new TransactionScope())
            {
                try
                {

                    if (Id == 0)
                    {


                        DisAgreementYear DAY = new DisAgreementYear();


                        DAY.EffectiveDate = EffectiveDate;
                        DAY.Naration = Naration;
                        DAY.CreateId = UserId;
                        DAY.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                        db.AddToDisAgreementYears(DAY);
                        db.SaveChanges();
                        transaction.Complete();

                    }
                    else
                    {
                        var DisId = db.DisAgreementYears.Where(x => x.Id == Id).FirstOrDefault();
                        if (TryUpdateModel(DisId))
                        {
                            DisId.EffectiveDate = EffectiveDate;
                            DisId.Naration = Naration;
                            DisId.EditId = UserId;
                            DisId.EditDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                            db.SaveChanges();
                            transaction.Complete();
                        }
                    }
                    result = true;
                }
                catch (Exception e)
                {
                    transaction.Dispose();
                    string message = e.Message;
                }
            }
            ModelState.Clear();
            return Json(result, JsonRequestBehavior.AllowGet);
            // return View();
        }

        public ActionResult ViewDisAgreementYear()
        {
            var query = (from s in db.DisAgreementYears
                         orderby s.Id

                         select new
                         {
                             Id = s.Id,
                             EffectiveDate = s.EffectiveDate,
                             Naration = s.Naration,

                         }).ToList();

            return View(query);

        }

        public string DateValidation(int Id, DateTime EffectiveDate, int year)
        {
            var result = false;




            var date = (from s in db.DisAgreementYears
                        where s.EffectiveDate == EffectiveDate
                        select
                            s.EffectiveDate
                            ).FirstOrDefault();

            var effdate = EffectiveDate;

            //var strSql = (db.SP_DATE_VALIDATION(EffectiveDate,year)).FirstOrDefault();
            var strSql = ("").FirstOrDefault();
            string SN = "";
            if (strSql == 1)
            {
                if (Id == 0)
                {
                    SN = "N";

                }
                else
                {
                    if (date != EffectiveDate)
                    {
                        SN = "N";

                    }
                    //else
                    //{
                    //    return SN.ToString();
                    //}


                }


            }
            return SN;
            //else
            //{





            //}


        }

        //----------FinancialInstitutionCategoryMaster----------//

        public ActionResult AddFinancialInstitutionCategory(Master M)
        {
            if (M.Id == 0)
            {
                return View();
            }
            else
            {
                var sqlstr = (from s in db.FinancialInstitutionCategoryMasters
                              where s.Id == M.Id
                              select new Master
                              {
                                  Category = s.Category,
                                  Id = s.Id

                              }).FirstOrDefault();
                return View(sqlstr);
            }


        }


        public string ChkFinancialCategory(int Id, string Category)
        {

            var sqlstr = (from s in db.FinancialInstitutionCategoryMasters
                          where s.Category == Category
                          select new
                          {
                              s.Category,
                              s.Id

                          }

             ).FirstOrDefault();

            if (sqlstr == null)
            {
                string str;
                return str = "";
            }
            else
            {
                if (Id != sqlstr.Id)
                {
                    string str;
                    return str = "N";
                }
                else
                {
                    return sqlstr.ToString();
                }
            }

        }

        [HttpPost]
        public ActionResult AddFinancialInstitutionCategory1(int Id, string Category)
        {
            var sqlstr = ChkFinancialCategory(Id, Category);
            var result = false;
            if (sqlstr == "N")
            {
                result = false;
            }
            else
            {
                int UserId = Convert.ToInt32(Session["UserId"]);
                using (TransactionScope transaction = new TransactionScope())
                {
                    try
                    {
                        if (Id == 0)
                        {
                            FinancialInstitutionCategoryMaster FICA = new FinancialInstitutionCategoryMaster();
                            FICA.Category = Category;
                            FICA.CreateId = UserId;
                            FICA.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                            db.AddToFinancialInstitutionCategoryMasters(FICA);
                            db.SaveChanges();
                            transaction.Complete();

                        }
                        else
                        {
                            var CaId = db.FinancialInstitutionCategoryMasters.Where(x => x.Id == Id).FirstOrDefault();
                            if (TryUpdateModel(CaId))
                            {
                                CaId.Category = Category;
                                CaId.EditDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                                CaId.EditId = UserId;
                                db.SaveChanges();
                                transaction.Complete();

                            }


                        }
                        result = true;
                    }
                    catch (Exception e)
                    {
                        transaction.Dispose();
                        string message = e.Message;

                    }
                }
            }
            ModelState.Clear();
            //return View();
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public ActionResult ViewFinancialInstitutionCategory()
        {
            var sqlstr = (from ac in db.FinancialInstitutionCategoryMasters
                          orderby ac.Category
                          select new
                          {
                              ac.Category,
                              ac.Id
                          }
            ).ToList();

            return View(sqlstr);
        }

        //--------AccountTypeMaster-----------//

        public ActionResult AddAccountType(Master M)
        {
            if (M.Id == 0)
            {
                return View();
            }
            else
            {
                var sqlstr = (from s in db.AccountTypeMasters
                              where s.Id == M.Id
                              select new Master
                              {
                                  AccountType = s.AccountType,
                                  Id = s.Id

                              }).FirstOrDefault();
                return View(sqlstr);
            }


        }


        public string ChkAccountType(int Id, string AccountType)
        {

            var sqlstr = (from s in db.AccountTypeMasters
                          where s.AccountType == AccountType
                          select new
                          {
                              s.AccountType,
                              s.Id

                          }

             ).FirstOrDefault();

            if (sqlstr == null)
            {
                string str;
                return str = "";
            }
            else
            {
                if (Id != sqlstr.Id)
                {
                    string str;
                    return str = "N";
                }
                else
                {
                    return sqlstr.ToString();
                }
            }

        }

        [HttpPost]
        public ActionResult AddAccountType1(int Id, string AccountType)
        {
            var sqlstr = ChkAccountType(Id, AccountType);
            var result = false;
            if (sqlstr == "N")
            {
                result = false;
            }
            else
            {
                int UserId = Convert.ToInt32(Session["UserId"]);
                using (TransactionScope transaction = new TransactionScope())
                {
                    try
                    {
                        if (Id == 0)
                        {
                            AccountTypeMaster AC = new AccountTypeMaster();
                            AC.AccountType = AccountType;
                            AC.CreateId = UserId;
                            AC.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                            db.AddToAccountTypeMasters(AC);
                            // db.SaveChanges();

                        }
                        else
                        {
                            var AccId = db.AccountTypeMasters.Where(x => x.Id == Id).FirstOrDefault();
                            if (TryUpdateModel(AccId))
                            {
                                AccId.AccountType = AccountType;
                                AccId.EditDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                                AccId.EditId = UserId;
                                // db.SaveChanges();

                            }


                        }
                        db.SaveChanges();
                        transaction.Complete();
                        var output = new { result = true, msg = "Data Saved Successfully." };
                        return Json(output, JsonRequestBehavior.AllowGet);
                        //return Json(new { msg = "Data Saved Successfully." }, JsonRequestBehavior.AllowGet);     

                    }
                    catch (Exception e)
                    {
                        transaction.Dispose();
                        return Json(new { msg = "Data Saved Faild." + e.ToString() }, JsonRequestBehavior.AllowGet);
                        //string message = e.Message;

                    }
                }
            }
            ModelState.Clear();
            //return View();
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public ActionResult ViewAccountType()
        {
            var sqlstr = (from ac in db.AccountTypeMasters
                          orderby ac.AccountType
                          select new
                          {
                              ac.AccountType,
                              ac.Id
                          }
            ).ToList();

            return View(sqlstr);
        }

        //----------------DisAgreementDetails-----------//

        public ActionResult AddDisAgreementDetails(Master M)
        {
            ViewBag.DistributionTypeList = new SelectList(db.SP_BIND_YEAR().ToList(), "Id", "EffectiveDate");
            if (M.Id == 0)
            {
                return View();
            }
            else
            {


                var sqlstr = (from D1 in db.DisAgreementDetails
                              join d2 in db.DisAgreementYears on D1.DisAgreementYearId equals d2.Id
                              where D1.DisAgreementYearId == M.Id
                              select new
                              {
                                  D1.DisAgreementYearId,
                              }).FirstOrDefault();
                return View(sqlstr);




            }
        }

        public JsonResult GetDistributionInfo()
        {

            var strSql = (from s in db.DistributionTypeMasters select new { s.Id, s.Name }).ToList();

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

        public JsonResult GetEditDistributionInfo(Master M)
        {

            decimal DisAgreementYearId = 0;
            if (M.Id != 0)
            {
                DisAgreementYearId = M.Id;
            }


            var strSql = (db.spGetAgreementDetailsInfo(DisAgreementYearId)).ToList();

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

        public JsonResult SaveData(SaveDistributionData sd)
        {
            using (creaditratingEntities db = new creaditratingEntities())
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    try
                    {
                        var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

                        string Data1 = sd.SaveData;

                        IEnumerable<DisAgreementDetail> det = serializer.Deserialize<IEnumerable<DisAgreementDetail>>(Data1);

                        int UserId = Convert.ToInt32(Session["UserId"]);

                        if (det != null)
                        {
                            int count = det.Count();
                            foreach (var item in det)
                            {
                                if (count > 0)
                                {
                                    if (item.Id == 0)
                                    {
                                        DisAgreementDetail DA = new DisAgreementDetail();      //FeesDetailsHistory name of the table
                                        DA.CreateId = UserId;
                                        DA.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                                        DA.DistributionTypeId = item.DistributionTypeId;
                                        DA.DisAgreementYearId = item.DisAgreementYearId;
                                        DA.Percentage = item.Percentage;
                                        db.AddToDisAgreementDetails(DA);
                                        db.SaveChanges();
                                        transaction.Complete();
                                    }
                                    else
                                    {
                                        var DisAgreementId = db.DisAgreementDetails.Where(x => x.Id == item.Id).FirstOrDefault();
                                        if (TryUpdateModel(DisAgreementId))
                                        {
                                            DisAgreementId.EditId = UserId;
                                            DisAgreementId.EditDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                                            DisAgreementId.DistributionTypeId = item.DistributionTypeId;
                                            DisAgreementId.DisAgreementYearId = item.DisAgreementYearId;
                                            DisAgreementId.Percentage = item.Percentage;
                                            db.SaveChanges();
                                            transaction.Complete();
                                        }
                                    }


                                }


                            }

                        }
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

        public ActionResult ViewDisAgreementDetails()
        {

            var sqlstr = (db.sp_GetDisAgreementDetails()).ToList();

            return View(sqlstr);
        }

        public ActionResult EditDisAgreementDetails(Master M)
        {
            decimal DisId = 0;
            if (M.Id != 0)
            {
                DisId = M.Id;
                var count = (db.spCheckMonthAndYear(DisId)).FirstOrDefault();
                if (count == 0)
                {
                    ViewBag.YearId = DisId;
                    return View();
                }
                else
                {
                    return Content(("<script language='javascript' type='text/javascript'> alert('It is already Applicable.'); window.location.href ='/Master/ViewDisAgreementDetails'   </script>"));
                }



            }

            return View();


        }


        //----------------------TransactionMaster------------------------//


        public ActionResult AddTransaction(Master M)
        {

            if (M.Id == 0)
            {
                return View();
            }
            else
            {
                var sqlstr = (from s in db.TransactionMasters
                              where s.Id == M.Id
                              select new Master
                              {
                                  TransactionType = s.TransactionType,
                                  points = s.points,
                                  Id = s.Id

                              }).FirstOrDefault();
                return View(sqlstr);
            }
        }

        public string ChkTransactionType(int Id, string TransactionType)
        {

            var sqlstr = (from s in db.TransactionMasters
                          where s.TransactionType == TransactionType
                          select new
                          {
                              s.TransactionType,
                              s.Id

                          }

             ).FirstOrDefault();

            if (sqlstr == null)
            {
                string str;
                return str = "";
            }
            else
            {
                if (Id != sqlstr.Id)
                {
                    string str;
                    return str = "N";
                }
                else
                {
                    return sqlstr.ToString();
                }
            }

        }

        [HttpPost]
        public ActionResult AddTransaction1(int Id, string TransactionType, string points)
        {
            var sqlstr = ChkTransactionType(Id, TransactionType);
            var result = false;
            if (sqlstr == "N")
            {
                result = false;
            }
            else
            {
                int UserId = Convert.ToInt32(Session["UserId"]);
                using (TransactionScope transaction = new TransactionScope())
                {
                    try
                    {
                        if (Id == 0)
                        {
                            TransactionMaster TM = new TransactionMaster();
                            TM.TransactionType = TransactionType;
                            TM.points = points.ToString();
                            TM.CreateId = UserId;
                            TM.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                            db.AddToTransactionMasters(TM);
                            db.SaveChanges();
                            transaction.Complete();
                        }
                        else
                        {
                            var TranId = db.TransactionMasters.Where(x => x.Id == Id).FirstOrDefault();
                            if (TryUpdateModel(TranId))
                            {
                                TranId.TransactionType = TransactionType;
                                TranId.points = points.ToString();
                                TranId.EditDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                                TranId.EditId = UserId;
                                db.SaveChanges();
                                transaction.Complete();
                            }


                        }
                        result = true;
                    }
                    catch (Exception e)
                    {
                        transaction.Dispose();
                        string message = e.Message;

                    }
                }
            }
            ModelState.Clear();
            //return View();
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ViewTransaction()
        {
            var sqlstr = (from TM in db.TransactionMasters
                          orderby TM.Id
                          select new
                          {
                              TM.TransactionType,
                              TM.points,
                              TM.Id
                          }
            ).ToList();

            return View(sqlstr);
        }



        //---------------------RatingRangeMaster---------------------//

        public ActionResult AddRatingRange(Master m)
        {
            if (m.Id == 0)
            {

                //ViewBag.slab = "s";
                return View();
            }
            else
            {
                //ViewBag.e = "e";
                var slab = (from s in db.RatingRangeMasters
                            where s.ID == m.Id
                            select new Master
                            {
                                Id = m.Id,
                                FROM_VALUE = s.FROM_VALUE,
                                TO_VALUE = s.TO_VALUE,
                                RATING_STATUS = s.RATING_STATUS
                            }).FirstOrDefault();
                return View(slab);
            }
        }
        [HttpPost]
        public ActionResult RatingRange(Master m)
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                try
                {
                    if (m.Id == 0)
                    {
                        decimal rf = m.FROM_VALUE;
                        decimal rt = m.TO_VALUE;
                        string s = m.RATING_STATUS;
                        if (rf != 0 && rt != 0 && s != "")
                        {
                            //var RatingEx = (from b in db.RatingRangeMasters
                            //              where b.RATING_STATUS == m.RATING_STATUS
                            //              select b.RATING_STATUS).FirstOrDefault();



                            //if (RatingEx != null)
                            //{

                            //    return View("AddRatingRange", m);

                            //}
                            //else
                            //{

                            int CreateId = Convert.ToInt32(Session["UserId"]);

                            RatingRangeMaster RM = new RatingRangeMaster();
                            RM.FROM_VALUE = m.FROM_VALUE;
                            RM.TO_VALUE = m.TO_VALUE;
                            RM.RATING_STATUS = m.RATING_STATUS;
                            RM.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                            RM.CreateId = CreateId;
                            db.AddToRatingRangeMasters(RM);

                            db.SaveChanges();
                            transaction.Complete();


                        }
                        return Content(("<script language='javascript' type='text/javascript'> alert('Data saved successfully!');window.location.href ='/Master/AddRatingRange';</script>"));
                    }
                    //    else
                    //    {
                    //        return View("AddRatingRange");

                    //    }
                    //}
                    //else
                    //{
                    //var RatingEx = (from b in db.RatingRangeMasters
                    //                where b.RATING_STATUS == m.RATING_STATUS
                    //                select b.RATING_STATUS).FirstOrDefault();



                        //if (RatingEx != null)
                    //{

                        //    return View("AddRatingRange", m);

                        //}
                    else
                    {
                        int CreateId = Convert.ToInt32(Session["UserId"]);
                        var RatingRange = db.RatingRangeMasters.Where(x => x.ID == m.Id).FirstOrDefault();

                        if (TryUpdateModel(RatingRange))
                        {
                            RatingRange.FROM_VALUE = m.FROM_VALUE;
                            RatingRange.TO_VALUE = m.TO_VALUE;
                            RatingRange.RATING_STATUS = m.RATING_STATUS;
                            RatingRange.EditDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                            RatingRange.EditId = CreateId;
                            db.SaveChanges();
                            transaction.Complete();
                        }


                        return Content(("<script language='javascript' type='text/javascript'> alert('Data saved successfully!');window.location.href ='/Master/ViewRatingRange';</script>"));
                    }

                }
                catch (Exception e)
                {
                    transaction.Dispose();
                    string message = e.Message;
                }
            }
            //  }
            // }
            return View();
        }

        public ActionResult ViewRatingRange()
        {
            var Range = (from s in db.RatingRangeMasters
                         orderby s.FROM_VALUE
                         select new
                         {
                             ID = s.ID,
                             FROM_VALUE = s.FROM_VALUE,
                             TO_VALUE = s.TO_VALUE,
                             RATING_STATUS = s.RATING_STATUS
                         }).ToList();

            return View(Range);
        }



        public JsonResult ChkFrom(int Id, int FROM_VALUE)
        {

            var sqlstr = (from s in db.RatingRangeMasters
                          where s.FROM_VALUE == FROM_VALUE
                          select new
                          {
                              s.FROM_VALUE,
                              s.ID

                          }

             ).FirstOrDefault();

            if (sqlstr == null)
            {
                string str = "";
                //return str = "";
                return Json(str, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (Id != sqlstr.ID)
                {
                    string str = "N";
                    //return str = "N";
                    return Json(str, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //return sqlstr.ToString();
                    return Json(sqlstr, JsonRequestBehavior.AllowGet);
                }
            }

        }

        public JsonResult Fromexist(int FROM_VALUE)
        {



            var s = (from r in db.RatingRangeMasters
                     orderby r.TO_VALUE descending
                     select r.TO_VALUE).FirstOrDefault();




            return Json(s, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Statusexist(string RATING_STATUS)
        {


            var STATUS = (from b in db.RatingRangeMasters
                          where b.RATING_STATUS == RATING_STATUS
                          select b.RATING_STATUS).FirstOrDefault();




            return Json(STATUS, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Toexist(int TO_VALUE)
        {
            var s = (from S in db.RatingRangeMasters
                     where S.TO_VALUE == TO_VALUE
                     select S.TO_VALUE).FirstOrDefault();


            return Json(s, JsonRequestBehavior.AllowGet);
        }
        public JsonResult greaterRate(int FROM_VALUE, int TO_VALUE)
        {

            var result = true;
            //var Rangef = (from b in db.SlabMasters
            //              where b.RangeFrom == Rangefrom
            //              select b.RangeFrom).FirstOrDefault();
            if (TO_VALUE <= FROM_VALUE)
            {
                result = false;
            }



            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult smallerRate(int FROM_VALUE, int TO_VALUE)
        {

            var result = true;
            //var Rangef = (from b in db.SlabMasters
            //              where b.RangeFrom == Rangefrom
            //              select b.RangeFrom).FirstOrDefault();
            if (FROM_VALUE >= TO_VALUE)
            {
                result = false;
            }



            return Json(result, JsonRequestBehavior.AllowGet);
        }


        //----------------------------------AreaMaster--------------------------------//


        public ActionResult AddArea(Master M)
        {
            ViewBag.CityList = new SelectList(db.CityMasters.ToList(), "Id", "City");

            if (M.Id == 0)
            {
                return View();
            }
            else
            {
                var sqlstr = (from s in db.AreaMasters
                              where s.Id == M.Id
                              select new Master
                              {
                                  CityId = s.CityId.Value,
                                  Name = s.Name,
                                  Id = s.Id

                              }).FirstOrDefault();
                return View(sqlstr);
            }
        }


        public string ChkAreaName(int Id, decimal CityId, string AreaName)
        {

            var sqlstr = (from s in db.AreaMasters
                          where s.Name == AreaName && s.CityId == CityId
                          select new
                          {
                              s.Name,
                              s.Id

                          }

             ).FirstOrDefault();

            if (sqlstr == null)
            {
                string str;
                return str = "";
            }
            else
            {
                if (Id != sqlstr.Id)
                {
                    string str;
                    return str = "N";
                }
                else
                {
                    return sqlstr.ToString();
                }
            }

        }




        [HttpPost]
        public ActionResult AddArea1(int Id, decimal CityId, string Name)
        {
            var sqlstr = ChkAreaName(Id, CityId, Name);
            var result = false;
            if (sqlstr == "N")
            {
                result = false;
            }
            else
            {


                int UserId = Convert.ToInt32(Session["UserId"]);
                using (TransactionScope transaction = new TransactionScope())
                {
                    try
                    {
                        if (Id == 0)
                        {
                            AreaMaster AM = new AreaMaster();
                            AM.CityId = CityId;
                            AM.Name = Name;
                            AM.CreateId = UserId;
                            AM.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                            db.AddToAreaMasters(AM);
                            db.SaveChanges();
                            transaction.Complete();
                        }
                        else
                        {
                            var AreaId = db.AreaMasters.Where(x => x.Id == Id).FirstOrDefault();
                            if (TryUpdateModel(AreaId))
                            {
                                AreaId.CityId = CityId;
                                AreaId.Name = Name;
                                AreaId.EditDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                                AreaId.EditId = UserId;
                                db.SaveChanges();
                                transaction.Complete();
                            }


                        }
                        result = true;
                    }
                    catch (Exception e)
                    {
                        transaction.Dispose();
                        string message = e.Message;

                    }
                }
            }
            ModelState.Clear();
            //return View();
            return Json(result, JsonRequestBehavior.AllowGet);

        }


        public ActionResult ViewArea()
        {
            var sqlstr = (from AM in db.AreaMasters
                          join CM in db.CityMasters on AM.CityId equals CM.Id
                          orderby AM.Id
                          select new
                          {
                              CM.City,
                              AM.Name,
                              AM.Id
                          }
            ).ToList();

            return View(sqlstr);
        }

        //-----------UserTypeMaster------------//

        public ActionResult AddUserType(Master M)
        {


            if (M.Id == 0)
            {
                return View();
            }
            else
            {
                var sqlstr = (from s in db.UserTypeMasters
                              where s.Id == M.Id
                              select new Master
                              {
                                  UserType = s.UserType,
                                  UserTypeShortName = s.UserTypeShortName

                              }).FirstOrDefault();
                return View(sqlstr);
            }
        }


        public JsonResult ChkUserType(int Id, string UserType)
        {

            var sqlstr = (from s in db.UserTypeMasters
                          where s.UserType == UserType
                          select new
                          {
                              s.UserType,
                              s.Id

                          }

             ).FirstOrDefault();

            if (sqlstr == null)
            {
                string str = "";
                //return str = "";
                return Json(str, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (Id != sqlstr.Id)
                {
                    string str = "N";
                    //return str = "N";
                    return Json(str, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //return sqlstr.ToString();
                    return Json(sqlstr, JsonRequestBehavior.AllowGet);
                }
            }

        }

        public JsonResult ChkUserTypeShortName(int Id, string UserTypeShortName)
        {

            var sqlstr = (from s in db.UserTypeMasters
                          where s.UserTypeShortName == UserTypeShortName
                          select new
                          {
                              s.UserTypeShortName,
                              s.Id

                          }

             ).FirstOrDefault();

            if (sqlstr == null)
            {
                string str = "";
                //return str = "";
                return Json(str, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (Id != sqlstr.Id)
                {
                    string str = "N";
                    //return str = "N";
                    return Json(str, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //return sqlstr.ToString();
                    return Json(sqlstr, JsonRequestBehavior.AllowGet);
                }
            }

        }




        [HttpPost]
        public ActionResult AddUserType1(int Id, string UserType, string UserTypeShortName)
        {
            //var sqlstr = ChkAreaName(Id, CityId, Name);
            var result = false;
            // if (sqlstr == "N")
            // {
            //  result = false;
            //}
            //else
            // {
            int UserId = Convert.ToInt32(Session["UserId"]);
            using (TransactionScope transaction = new TransactionScope())
            {
                try
                {
                    if (Id == 0)
                    {
                        UserTypeMaster utm = new UserTypeMaster();
                        utm.UserType = UserType;
                        utm.UserTypeShortName = UserTypeShortName;
                        utm.CreateId = UserId;
                        utm.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                        db.AddToUserTypeMasters(utm);
                        db.SaveChanges();
                        transaction.Complete();
                    }
                    else
                    {
                        var UserTypeId = db.UserTypeMasters.Where(x => x.Id == Id).FirstOrDefault();
                        if (TryUpdateModel(UserTypeId))
                        {
                            UserTypeId.UserType = UserType;
                            UserTypeId.UserTypeShortName = UserTypeShortName;
                            UserTypeId.EditDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                            UserTypeId.EditId = UserId;
                            db.SaveChanges();
                            transaction.Complete();
                        }


                    }
                    result = true;
                }
                catch (Exception e)
                {
                    transaction.Dispose();
                    string message = e.Message;

                }
            }

            ModelState.Clear();
            //return View();
            return Json(result, JsonRequestBehavior.AllowGet);

        }


        public ActionResult ViewUserType()
        {
            var sqlstr = (from UTM in db.UserTypeMasters
                          orderby UTM.Id
                          select new
                          {
                              UTM.UserType,
                              UTM.UserTypeShortName,
                              UTM.Id
                          }
            ).ToList();

            return View(sqlstr);
        }

        //-------------Role------------------//


        public ActionResult AddRole(Master M)
        {


            if (M.Id == 0)
            {
                return View();
            }
            else
            {
                var sqlstr = (from s in db.Roles
                              where s.Id == M.Id
                              select new Master
                              {
                                  Role = s.RoleName,
                                  RoleDescription = s.RoleDescription

                              }).FirstOrDefault();
                return View(sqlstr);
            }
        }

        public JsonResult ChkRole(int Id, string Role)
        {

            var sqlstr = (from s in db.Roles
                          where s.RoleName == Role
                          select new
                          {
                              s.RoleName,
                              s.Id

                          }

             ).FirstOrDefault();

            if (sqlstr == null)
            {
                string str = "";
                //return str = "";
                return Json(str, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (Id != sqlstr.Id)
                {
                    string str = "N";
                    //return str = "N";
                    return Json(str, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //return sqlstr.ToString();
                    return Json(sqlstr, JsonRequestBehavior.AllowGet);
                }
            }

        }

        public JsonResult ChkRoleDescription(int Id, string RoleDescription)
        {

            var sqlstr = (from s in db.Roles
                          where s.RoleDescription == RoleDescription
                          select new
                          {
                              s.RoleDescription,
                              s.Id

                          }

             ).FirstOrDefault();

            if (sqlstr == null)
            {
                string str = "";
                //return str = "";
                return Json(str, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (Id != sqlstr.Id)
                {
                    string str = "N";
                    //return str = "N";
                    return Json(str, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //return sqlstr.ToString();
                    return Json(sqlstr, JsonRequestBehavior.AllowGet);
                }
            }

        }

        [HttpPost]
        public ActionResult AddRole1(int Id, string Role, string RoleDescription)
        {
            //var sqlstr = ChkAreaName(Id, CityId, Name);
            var result = false;
            // if (sqlstr == "N")
            // {
            //  result = false;
            //}
            //else
            // {
            int UserId = Convert.ToInt32(Session["UserId"]);
            using (TransactionScope transaction = new TransactionScope())
            {
                try
                {
                    if (Id == 0)
                    {
                        Role RM = new Role();
                        RM.RoleName = Role;
                        RM.RoleDescription = RoleDescription;
                        RM.CreateId = UserId;
                        RM.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                        db.AddToRoles(RM);
                        db.SaveChanges();
                        transaction.Complete();
                    }
                    else
                    {
                        var RoleId = db.Roles.Where(x => x.Id == Id).FirstOrDefault();
                        if (TryUpdateModel(RoleId))
                        {
                            RoleId.RoleName = Role;
                            RoleId.RoleDescription = RoleDescription;
                            RoleId.EditDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                            RoleId.EditId = UserId;
                            db.SaveChanges();
                            transaction.Complete();
                        }


                    }
                    result = true;
                }
                catch (Exception e)
                {
                    transaction.Dispose();
                    string message = e.Message;

                }

            }
            ModelState.Clear();
            //return View();
            return Json(result, JsonRequestBehavior.AllowGet);

        }


        public ActionResult ViewRole()
        {
            var sqlstr = (from R in db.Roles
                          orderby R.Id
                          select new
                          {
                              R.RoleName,
                              R.RoleDescription,
                              R.Id
                          }
            ).ToList();

            return View(sqlstr);
        }


        //--------------NameTitleMaster----------------//

        public ActionResult AddNameTitle(Master M)
        {
            if (M.Id == 0)
            {
                return View();
            }
            else
            {
                var sqlstr = (from s in db.NameTitleMasters
                              where s.Id == M.Id
                              select new Master
                              {
                                  TitleName = s.Name,
                                  Id = s.Id
                              }).FirstOrDefault();
                return View(sqlstr);
            }

        }

        [HttpPost]
        public ActionResult AddNameTitle1(int Id, string TitleName)
        {
            var result = false;
            int UserId = Convert.ToInt32(Session["UserId"]);
            using (TransactionScope transaction = new TransactionScope())
            {
                try
                {
                    if (Id == 0)
                    {
                        NameTitleMaster ntm = new NameTitleMaster();
                        ntm.Name = TitleName;
                        ntm.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                        ntm.CreateId = UserId;
                        db.AddToNameTitleMasters(ntm);
                        db.SaveChanges();
                        transaction.Complete();
                    }
                    else
                    {
                        var TitleId = db.NameTitleMasters.Where(x => x.Id == Id).FirstOrDefault();
                        if (TryUpdateModel(TitleId))
                        {
                            TitleId.Name = TitleName;
                            TitleId.EditDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                            TitleId.EditId = UserId;
                            db.SaveChanges();
                            transaction.Complete();
                        }
                    }
                }
                catch (Exception e)
                {
                    transaction.Dispose();
                    string message = e.Message;
                }
            }
            result = true;

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public JsonResult ChkNameTitle(int Id, string TitleName)
        {
            var result = false;
            var sqlstr = (from s in db.NameTitleMasters
                          where s.Name == TitleName
                          select new
                          {
                              s.Name,
                              s.Id
                          }).FirstOrDefault();

            if (sqlstr == null)
            {
                string str = "";
                //return str = "";
                return Json(str, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (Id != sqlstr.Id)
                {
                    string str = "N";
                    //return str = "N";
                    return Json(str, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //return sqlstr.ToString();
                    return Json(sqlstr, JsonRequestBehavior.AllowGet);
                }
            }
        }


        public ActionResult ViewNameTitle()
        {
            var strsqlView = (from s in db.NameTitleMasters
                              orderby s.Id
                              select new
                              {
                                  s.Name,
                                  s.Id
                              }).ToList();

            return View(strsqlView);
        }
        //----------------------------------------------PackageMaster-----------------------------------------------------------------//

        public ActionResult AddPackage()
        {
            return View();

        }
        [HttpPost]
        public ActionResult SavePackage(string packagename, string packagecode, string packagecredit, string packageamt)
        {
            var result = false;
            using (TransactionScope transaction = new TransactionScope())
            {

                int UserId = Convert.ToInt32(Session["UserId"]);
                try
                {
                    PackageMaster PM = new PackageMaster();
                    PM.PackageName = packagename;
                    PM.PackageCode = packagecode;
                    PM.PackageCredits = Convert.ToInt32(packagecredit);
                    PM.PackageAmt = Convert.ToDecimal(packageamt);
                    PM.Active = Convert.ToBoolean(1);
                    PM.CreditId = UserId;
                    PM.CreditDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                    db.AddToPackageMasters(PM);
                    db.SaveChanges();
                    transaction.Complete();

                    result = true;
                }
                catch (Exception e)
                {
                    transaction.Dispose();
                    string message = e.Message;
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPkgName(string packagename)
        {
            var sqlquery = (db.SP_GetPackageName(packagename)).FirstOrDefault();
            if (sqlquery == null)
            {
                sqlquery = "false";

            }
            return Json(sqlquery, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewPackage()
        {
            bool active = Convert.ToBoolean(1);
            var sqlstr = (from PkgM in db.PackageMasters
                          where PkgM.Active == active
                          orderby PkgM.Id
                          select new
                          {
                              PkgM.PackageName,
                              PkgM.PackageAmt,
                              PkgM.PackageCredits,
                              PkgM.Id
                          }
            ).ToList();

            return View(sqlstr);
        }


        //-----------CardTypeMaster------------//
        public ActionResult AddCardType(int Id = 0)
        {
            int BI_ID = Convert.ToInt32(Session["EmployerName"]);
            if (BI_ID == 0)
            {
                //return View("_Home"); 
                return Content(("<script language='javascript' type='text/javascript'> alert('You are not authorised person to access this module') ; window.location.href ='/Home/_Home'; </script>"));

            }
            if (Id == 0)
            {
                ViewBag.BankList = new SelectList((from FIM in db.FinancialInstitutionMasters
                                               where FIM.FinancialInstituteCategoryId == 1
                                               orderby FIM.Name
                                               select FIM).ToList(), "Id", "Name",Convert.ToInt32(Session["EmployerName"]));
            }
                else
	            {
                    ViewBag.BankList = new SelectList((from FIM in db.FinancialInstitutionMasters
                                                       where FIM.FinancialInstituteCategoryId == 1
                                                       orderby FIM.Name
                                                       select FIM).ToList(), "Id", "Name");
                    getRetriveData(Id);
	            }
            
            
            return View();
        }

        public ActionResult SaveCardType(int FinancialInstituteId, string CardType, int DueDay, int gracePeriod, decimal CreditLimit, decimal CashLimit, int intid)
        {
            var result = false;
            using (TransactionScope transaction = new TransactionScope())
            {

                int UserId = Convert.ToInt32(Session["UserId"]);
                try
                {
                    CardTypeMaster PM = new CardTypeMaster();
                    PM.CardType = CardType;
                    PM.FinancialInstituteId = FinancialInstituteId;
                    PM.DueDays = DueDay;
                    PM.GracePeriod = gracePeriod;
                    PM.CreditLimit = CreditLimit;
                    PM.CashLimit = CashLimit;
                    PM.CreateId = UserId;
                    PM.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                    db.AddToCardTypeMasters(PM);
                    db.SaveChanges();
                    transaction.Complete();

                    result = true;
                }
                catch (Exception e)
                {

                    transaction.Dispose();
                    string message = e.Message;

                }
            }
            //return Content(("<script language='javascript' type='text/javascript'> alert('Data saved successfully!'); window.location.href ='/Master/CardTypeList';</script>"));
            return Json(result, JsonRequestBehavior.AllowGet);
            //return View("AddCardType");
        }
        public ActionResult CardTypeList()
        {
            int BI_ID = Convert.ToInt32(Session["EmployerName"]);
            if (BI_ID == 0)
            {
                //return View("_Home"); 
                return Content(("<script language='javascript' type='text/javascript'> alert('You are not authorised person to access this module') ; window.location.href ='/Home/_Home'; </script>"));

            }
            int FI_ID = Convert.ToInt32(Session["EmployerName"]);

            var strSql = (from CTL in db.sp_CardTypeList(FI_ID) select CTL).ToList();
            //var strSql = (from AppLoan in db.sp_LoanApplicationStatusList(FI_ID, FI_Br_ID, strLoanStatus) select AppLoan).ToList();
            //return View(strSql);
            return View(strSql);
        }
        public ActionResult getRetriveData(int intid)
        {
            //var result = false;
            using (TransactionScope transaction = new TransactionScope())
            {

                //int UserId = Convert.ToInt32(Session["UserId"]);
                try
                {
                    CardTypeMaster PM = new CardTypeMaster();
                    var strSql2 = (from CTM in db.sp_GetCardTypeDetails(intid)
                                   select new Master
                                   {
                                       CardType = CTM.CardType,
                                       FinantialInstituteId = CTM.FinancialInstituteId,
                                       Graceperiod = CTM.GracePeriod.Value,
                                       DueDays = CTM.DueDays,
                                       CreditLimit = CTM.CreditLimit.Value,
                                       CashLimit = CTM.CashLimit.Value,
                                       Id = intid
                                   }).FirstOrDefault();

                    return View(strSql2);
                }
                catch (Exception e)
                {

                    transaction.Dispose();
                    string message = e.Message;

                }
                return View();
                
            }
        }
        public ActionResult EditAction(int FinancialInstituteId,string CardType, int DueDay, int gracePeriod, decimal CreditLimit, decimal CashLimit,int intid)
        {
            var result = false;
            int intUserId = Convert.ToInt32(Session["UserId"]);
            var tblcardType = db.CardTypeMasters.Where(x => x.Id == intid).FirstOrDefault();
            using (TransactionScope transaction = new TransactionScope())
                try
                {
                    tblcardType.CardType = CardType;
                    //tblcardType.FinancialInstituteId = FinancialInstituteId;
                    tblcardType.DueDays = DueDay;
                    tblcardType.GracePeriod = gracePeriod;
                    tblcardType.CreditLimit = CreditLimit;
                    tblcardType.CashLimit = CashLimit;
                    tblcardType.EditDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                    tblcardType.EditId = intUserId;
                    db.SaveChanges();
                    transaction.Complete();

                    result = true;
                    //return Content(("<script language='javascript' type='text/javascript'> alert('Data saved successfully!'); window.location.href ='/Master/CardTypeList';</script>"));

                }
                catch (Exception e)
                {

                    transaction.Dispose();
                    string message = e.Message;
                }                                            
            //return Content(("<script language='javascript' type='text/javascript'> alert('Data saved successfully!'); window.location.href ='/Master/CardTypeList';</script>"));
            //return View();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}

