using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CreditRatingModel.Model;
using System.IO;

namespace CreditRatingSystem.Controllers
{
    public class BorrowerController : Controller
    {
        //
        // GET: /Borrower/

        creaditratingEntities CRE = new creaditratingEntities();
        public string detail;
        public string demo1;
     

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewBorrower()
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

                return View("ViewBorrower", strSql);
            }
        }

        public ActionResult EditBorrowerCompany(int Id)
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
                                  CompanyName = BCM.CompanyName,
                                  CompanyType = BCTM.CompanyType,
                                  BranchName = BCBM.BranchName,
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

            var tblBorrowerCompanyBranchMaster = CRE.BorrowerCompanyBranchMasters.Where(x => x.Id == BD.ID).FirstOrDefault();

            decimal decCompTypeId = (from BCTM in CRE.BorrowerCompanyTypeMasters
                                     join BCBM in CRE.BorrowerCompanyBranchMasters on BCTM.Id equals BCBM.CompanyTypeId
                                     where BCBM.Id == BD.ID
                                     select BCTM.Id
                                  ).FirstOrDefault();


            var tblBorrowerCompanyTypeMaster = CRE.BorrowerCompanyTypeMasters.Where(x => x.Id == decCompTypeId).FirstOrDefault();

            decimal decCompId = (from BCM in CRE.BorrowerCompanyMasters
                                 join BCTM in CRE.BorrowerCompanyTypeMasters on BCM.Id equals BCTM.CompanyId
                                 join BCBM in CRE.BorrowerCompanyBranchMasters on BCTM.Id equals BCBM.CompanyTypeId
                                 where BCBM.Id == BD.ID
                                 select BCM.Id
                                 ).FirstOrDefault();

            var tblBorrowerCompanyMaster = CRE.BorrowerCompanyMasters.Where(x => x.Id == decCompId).FirstOrDefault();
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

                    CRE.SaveChanges();
                    //TempData["Message"] = "Record Update Successfully.";                    

                    return RedirectToAction("ViewBorrower");
                }
                catch (Exception )
                {
                    return View("EditBorrowerCompany");
                }
            }
            return View("EditBorrowerCompany");

        }

        //****************

        //------------Darshana-----------------
        public ActionResult EditBorrowerCustomer(int id)
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
                var countries = (from c in CRE.CountryMasters
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

            return CRE.StateMasters.Where(s => s.CountryId == countryid).ToList();

        }

        public JsonResult StateList(string CountryId)
        {
            int id = Convert.ToInt32(CountryId);

            var states = (from s in CRE.StateMasters
                          where s.CountryId == id
                          select s);
            //return Json(new SelectList(states.ToArray(), objCategory.Id.ToString(), objCategory.State), JsonRequestBehavior.AllowGet);
            return Json(new SelectList(states.ToArray(), "Id", "State"), JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult CountryCode(string CountryId)
        {
            int id = Convert.ToInt32(CountryId);

            var CountryCode = (from s in CRE.CountryMasters
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
            var user = CRE.IndividualCustomerMasters.Where(x => x.Pan == Pan).FirstOrDefault();
            if (user != null)
            {
                result = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GridBIDetails()
        {
            var L = (from ICM in CRE.IndividualCustomerMasters
                     join NM in CRE.NationalityMasters on ICM.NationalityId equals NM.Id
                     join NTM in CRE.NameTitleMasters on ICM.TitleId equals NTM.Id
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

            ViewData["a"] = L;

            return View(L);
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
            return RedirectToAction("EditBorrowerCustomer", new { Id = BD.Id });
        }

        public ActionResult ChangeName()
        {
            return View();
        }

        public JsonResult ChangeName1(string Pan)
        {
            var result = (from ICM in CRE.IndividualCustomerMasters
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

        public ActionResult EditChangeName(BorrowerDetailscs BD)
        {
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


            return RedirectToAction("ChangeName");
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
            ICL.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
  
            CRE.AddToIndividualCustomerLogs(ICL);
            CRE.SaveChanges();

        }
        //-------------------------------------------

        //----------------Akshata--------------------
        public void DropList(BorrowerDetails BD)
        {
            ViewBag.Country = new SelectList(CRE.CountryMasters.ToList(), "Id", "Country");
            ViewBag.Name = new SelectList(CRE.NameTitleMasters.ToList(), "Id", "Name");
            ViewBag.Nationality = new SelectList(CRE.NationalityMasters.ToList(), "Id", "Nationality");
            ViewBag.Companyid = new SelectList(CRE.BorrowerCompanyMasters.ToList(), "Id", "CompanyName");
            ViewBag.Addressid = new SelectList(CRE.AddressTypeMasters.ToList(), "Id", "AddressType"); 
            ViewBag.DOC = BindDoc(BD);

        }

        [Authorize]
        public ActionResult AddBorrower(BorrowerDetails BD)
        {
            //var UsrType = Session["UserType"];
            //var e = (from m in CRE.User_Details
            //         join c in CRE.UserTypeMasters on m.USER_TYPE_ID equals c.Id
            //         where m.USER_TYPE_ID == 1
            //         select new
            //         {
            //             c.UserTypeShortName
            //         }).FirstOrDefault();
            //TempData["details"] = UsrType.ToString().Trim(); ;
            //TempData["M"] = e.UserTypeShortName.ToString().Trim();
            //var f = TempData["details"];
            //var o = TempData["M"];
            //TempData.Keep("M");
            //TempData.Keep("details");
           //ViewBag.TabYN = "N";
            //ViewBag.TabYN1 = "Y";
            //ViewBag.Address = BindAddresstype(BD);

            DropList(BD);
            return View(BD);
        }
        [HttpPost]
          public ActionResult AddBorrower(BorrowerDetails BD,HttpPostedFileBase file)
        {

            btnAddClick(BD,file);
            return View("AddBorrower",BD);
         
          }
        //private void InsertInfo1(int NID, int NationalityId, string USERNAME, int id, string PAN, string FirstName, string MiddleName, string LastName, string Sex, Nullable<DateTime> DateOfBirth, string CompanyName, Nullable<DateTime> IncorporationDate, Nullable<DateTime> CommencementDate, string CompanyType, string BranchName)
        //{
        //    InsertInfo i = new InsertInfo();
        //    i.Add(NID, NationalityId, USERNAME, id, PAN, FirstName, MiddleName, LastName, Sex, DateOfBirth, CompanyName, IncorporationDate, CommencementDate, CompanyType, BranchName);


        //}
        //private void InsertInfo2(string USERNAME, string Address1, string Address2, string Address3, int Addressid, int CountryId, int StateId, string City, string Pincode, string LandlineCode, string LandlineNo, string MobileCode, string MobileNo, string EmailId1, string EmailId2)
        //{
        //    InsertInfo i2 = new InsertInfo();
        //    i2.Addinfo(USERNAME, Address1, Address2, Address3, Addressid, CountryId, StateId, City, Pincode, LandlineCode, LandlineNo, MobileCode, MobileNo, EmailId1, EmailId2);
        //}

      
        public ActionResult btnAddClick(BorrowerDetails BD, HttpPostedFileBase file)
        {


            ViewBag.c = Session["USERNAME"];
            BD.USERNAME = ViewBag.c;
            TempData["pan"] = BD.PAN;
            TempData["firstname"] = BD.FirstName;
            TempData["middlename"] = BD.MiddleName;
            TempData["lastname"] = BD.LastName;
            TempData["Company"] = BD.CompanyName;
            TempData["CompanyType"] = BD.CompanyType;

            ViewBag.pan = BD.PAN;
            ViewBag.COM = BD.CompanyName;

            string selectedState1 = Request.Form["selState"];
            int selectedState = Convert.ToInt32(selectedState1);

            string Addressid1 = Request.Form["selComtype"];
            int Addressid = Convert.ToInt32(Addressid1);

            int intUserId = Convert.ToInt32(Session["UserId"]);
           
            //InsertInfo1(BD.NID, BD.NationalityId, BD.USERNAME, BD.id, BD.PAN, BD.FirstName, BD.MiddleName, BD.LastName, BD.Sex, BD.DateOfBirth, BD.CompanyName, BD.IncorporationDate, BD.CommencementDate, BD.CompanyType, BD.BranchName);
            InsertInfo i = new InsertInfo();
            //i.Add(intUserId, BD.NID, BD.NationalityId, BD.id, BD.PAN, BD.FirstName, BD.MiddleName, BD.LastName, BD.Sex, BD.DateOfBirth, BD.CompanyName, BD.IncorporationDate, BD.CommencementDate, BD.CompanyType, BD.BranchName, BD.Address1, BD.Address2, BD.Address3, Addressid, BD.CountryId, selectedState, BD.City, BD.Pincode, BD.LandlineCode, BD.LandlineNo, BD.MobileCode, BD.MobileNo, BD.EmailId1, BD.EmailId2, BD.Designation);

            //----for fileupload 15092015---///
            if (BD.CompanyName == null)
            {
                int customer = InsertInfo.individual;
                TempData["customerid"] = customer;

                TempData.Keep("customerid");

            }
            else
            {

                int customer = InsertInfo.CompanyTypeId;
                TempData["customerid"] = customer;
                TempData.Keep("customerid");

            }
            var fileName = Path.GetFileName(file.FileName);

            var e = (from f in CRE.DocumentMasters where f.Id == BD.DOC select new { f.ShortName }).FirstOrDefault();

            fileName = (e.ShortName.ToString().Trim()) + "--" + fileName;

              string borrowerType = BD.borrowerTypeId;
              if (borrowerType == "C")
              {
                  var folder = Path.Combine(Server.MapPath("~/files/Company/" + TempData["customerid"]));
                  if (!Directory.Exists(folder))
                  {
                      Directory.CreateDirectory(folder);
                  }
               
                  var finalpath = folder + "/" + fileName;
                  file.SaveAs(finalpath);
                  BD.FilePath = finalpath;
              }
              else {
                  var folder = Path.Combine(Server.MapPath("~/files/Individual/" + TempData["customerid"]));
                  if (!Directory.Exists(folder))
                  {
                      Directory.CreateDirectory(folder);
                  }
                  var finalpath = folder + "/" + fileName;
                  file.SaveAs(finalpath);
                  BD.FilePath = finalpath;
              
              }
         

            i.AddUploads(intUserId, BD.DOC, BD.docid, BD.IssueDate, BD.Expirydate, BD.FilePath);
            ///---------

            TempData.Keep("lastname");
            TempData.Keep("middlename");

            TempData.Keep("firstname");
            TempData.Keep("Company");
            TempData.Keep("CompanyType");
            TempData.Keep("pan");
            BindDoc(BD);
            DropList(BD);

            ViewBag.TabYN = "Y";
            ViewBag.TabYN1 = "N";
            AddInfo ai = new AddInfo();
            string company = InsertInfo.companyName;
            //ViewBag.Address = BindAddresstype(BD);
           // string borrowerType = BD.borrowerTypeId;
            if (borrowerType == "C")
            {
                ViewBag.SUCEESS = "S";
                ViewBag.Company = "Company";
                string states = (from s in CRE.StateMasters
                                 where s.Id == selectedState
                                 select s.State).FirstOrDefault();
                decimal statesId = (from s in CRE.StateMasters
                                    where s.Id == selectedState
                                    select s.Id).FirstOrDefault();
                string AddType = (from s in CRE.AddressTypeMasters
                                  where s.Id == Addressid
                                  select s.AddressType).FirstOrDefault();
                BD.State = states;
                BD.Addtype = AddType;
                ViewBag.State = states;
                ViewBag.stateId = statesId;
                var fileName1 = Path.GetFileName(file.FileName);
                ViewBag.path = fileName1;
            }
            else
            {
                ViewBag.SUCEESS = "I";
                ModelState.Clear();
            }
            return View("AddBorrower", BD);
        }

        //[HttpPost]
        //public ActionResult btnAddClick1(BorrowerDetails BD)
        //{
        //        ViewBag.A = Session["USERNAME"];
        //        BD.USERNAME = ViewBag.A;
        //        string selectedState1 = Request.Form["selState"];
        //        int selectedState = Convert.ToInt32(selectedState1);
        //        //InsertInfo2(BD.USERNAME, BD.Address1, BD.Address2, BD.Address3, BD.Addressid, BD.CountryId, selectedState, BD.City, BD.Pincode, BD.LandlineCode, BD.LandlineNo, BD.MobileCode, BD.MobileNo, BD.EmailId1, BD.EmailId2);

        //        InsertInfo i = new InsertInfo();
        //        i.Addinfo(BD.USERNAME, BD.Address1, BD.Address2, BD.Address3, BD.Addressid, BD.CountryId, BD.StateId, BD.City, BD.Pincode, BD.LandlineCode, BD.LandlineNo, BD.MobileCode, BD.MobileNo, BD.EmailId1, BD.EmailId2);
        //    ViewBag.DOC = BindDoc(BD);
        //    DropList(BD);
        //    ViewBag.Address = BindAddresstype(BD);
         
        //    ViewBag.TabYN1 = "N";
        //    return View("AddBorrower", BD);
        //    //TempData["M"] = Session["UserType"];

        //    //TempData["details"] = (from m in CRE.User_Details
        //    //                       join c in CRE.UserTypeMasters on m.USER_TYPE_ID equals c.Id
        //    //                       where m.USER_TYPE_ID == 1
        //    //                       select new
        //    //                       {
        //    //                           c.UserTypeShortName

        //    //                       }).FirstOrDefault();

        //    //TempData.Keep("M");
        //    //TempData.Keep("details");
         



        //}

        //private void InsertInfo3(int DOC, String docid, Nullable<DateTime> IssueDate, Nullable<DateTime> Expirydate, string FilePath)
        //{
        //    InsertInfo i3 = new InsertInfo();
        //    i3.AddUploads(DOC, docid, IssueDate, Expirydate, FilePath);
        //}

        //[HttpPost]
        //public ActionResult upload(BorrowerDetails BD, HttpPostedFileBase file)
        //{  
        //    //-----15092015----//
        //    ViewBag.A = Session["USERNAME"];
        //    BD.USERNAME = ViewBag.A;


        //    InsertInfo i = new InsertInfo();

        //    var folder = Path.Combine(Server.MapPath("~/files/" + TempData["customerid"]));
        //    if (!Directory.Exists(folder))
        //    {
        //        Directory.CreateDirectory(folder);
        //    }




        //    var filesave = folder;

        //    var fileName = Path.GetFileName(file.FileName);

        //    var e = (from f in CRE.DocumentMasters where f.Id == BD.DOC select new { f.ShortName }).FirstOrDefault();

        //    fileName = (e.ShortName.ToString().Trim()) + "--" + fileName;

        //    var finalpath = folder + "/" + fileName;
        //    file.SaveAs(finalpath);
        //    BD.FilePath = finalpath;
           
        //    i.AddUploads(BD.USERNAME, BD.DOC, BD.docid, BD.IssueDate, BD.Expirydate, BD.FilePath);

        //    ViewBag.Addressid = BindAddresstype(BD);

        //    DropList(BD);

        //    ViewBag.TabYN1 = "N";

        //    ModelState.Clear();
        //    return View("AddBorrower", BD);

            //if (ModelState.IsValid)
            //{

            //    var fileName = Path.GetFileName(file.FileName);
            //    var path = Path.Combine(Server.MapPath("~/files"), fileName);
            //    file.SaveAs(path);
            //    BD.FilePath = path;
            //    InsertInfo3(BD.DOC, BD.docid, BD.IssueDate, BD.Expirydate, BD.FilePath);
            //}
            //return View();
        
        
        [HttpPost]
        public ActionResult FullName()
        {
            ViewBag.fullname = TempData["firstname"] + " " + TempData["middlename"] + " " + TempData["lastname"];
            string FULLNAME = ViewBag.fullname;

            return Json(FULLNAME);

        }
        [HttpPost]
        public ActionResult ComName()
        {

            ViewBag.ComName = TempData["Company"] + " " + TempData["CompanyType"];
            string comname = ViewBag.ComName;

            return Json(comname);

        }


        public List<SelectListItem> BindCountry(BorrowerDetails BD)
        {
            return BD.country = GetCountry();
        }

                
        public List<SelectListItem> BindDoc(BorrowerDetails BD)
        {
            return BD.document = GetDoc();
        }

        private List<SelectListItem> GetDoc()
        {
            List<SelectListItem> docs = new List<SelectListItem>();

            try
            {
                var Docs = (from c in CRE.DocumentMasters
                            select c).ToList();
                
                if (Docs != null)
                {
                    docs.Add(new SelectListItem { Text = "Select", Value = "0" });

                    foreach (var item in Docs)
                    {
                        docs.Add(new SelectListItem { Text = item.Document + " - " + item.ShortName, Value = item.Id.ToString() });

                    }
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            return docs;

        }
        public JsonResult AddressTypeList(string Comtype)
        {
            // int id = Convert.ToInt32(Comtype);

            if (Comtype == "C")
            {

                var AddType = (from c in CRE.AddressTypeMasters
                               where c.Id == 1
                               select c);
                return Json(new SelectList(AddType.ToArray(), "Id", "AddressType"), JsonRequestBehavior.AllowGet);
            }
            else
            {
                var AddType = (from c in CRE.AddressTypeMasters
                               // where c.Id != 1
                               select c);
                return Json(new SelectList(AddType.ToArray(), "Id", "AddressType"), JsonRequestBehavior.AllowGet);
            }
            //  return Json(id,JsonRequestBehavior.AllowGet);

        }



        //public List<SelectListItem> BindName(BorrowerDetails BD)
        //{
        //    return BD.Name = GetDoc();
        //}

        //private List<SelectListItem> GetName()
        //{
        //    List<SelectListItem> Name = new List<SelectListItem>();

        //    try
        //    {
        //        var name = (from c in CRE.NameTitleMasters
        //                    select c).ToList();

        //        if (name != null)
        //        {
        //            Name.Add(new SelectListItem { Text = "Select", Value = "0" });

        //            foreach (var item in name)
        //            {
        //                Name.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.Message.ToString();
        //    }
        //    return Name;
        //}
        //public List<SelectListItem> BindNation(BorrowerDetails BD)
        //{
        //    return BD.Nationality = GetNation();
        //}

        //private List<SelectListItem> GetNation()
        //{
        //    List<SelectListItem> nation = new List<SelectListItem>();

        //    try
        //    {
        //        var Nation = (from c in CRE.NationalityMasters
        //                      select c).ToList();

        //        if (Nation != null)
        //        {
        //            nation.Add(new SelectListItem { Text = "Select", Value = "0" });

        //            foreach (var item in Nation)
        //            {
        //                nation.Add(new SelectListItem { Text = item.Nationality, Value = item.Id.ToString() });

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.Message.ToString();
        //    }
        //    return nation;
        //}

        //public List<SelectListItem> BindAddresstype(BorrowerDetails BD)
        //{

        //    return BD.AddressType = GetAddressType();
        //}

        //private List<SelectListItem> GetAddressType()
        //{
        //    List<SelectListItem> AddressType = new List<SelectListItem>();
        //    var UsrType = Session["UserType"];


        //    var e = (from m in CRE.User_Details
        //             join c in CRE.UserTypeMasters on m.USER_TYPE_ID equals c.Id
        //             where m.USER_TYPE_ID == 1
        //             select new
        //             {
        //                 c.UserTypeShortName

        //             }).FirstOrDefault();
        //    TempData["details"] = UsrType.ToString().Trim(); ;
        //    TempData["M"] = e.UserTypeShortName.ToString().Trim();

        //    var f = TempData["details"];
        //    var o = TempData["M"];
        //    TempData.Keep("M");
        //    TempData.Keep("details");


        //    if (f.ToString().Trim() == o.ToString().Trim())
        //    {
        //        var AddType = (from c in CRE.AddressTypeMasters
        //                       where c.Id == 1
        //                       select c).ToList();
        //        if (AddType != null)
        //        {
        //            AddressType.Add(new SelectListItem { Text = "Select", Value = "0" });

        //            foreach (var item in AddType)

        //                AddressType.Add(new SelectListItem { Text = item.AddressType, Value = item.Id.ToString() });
        //        }


        //    }
        //    else
        //    {
        //        var AddType = (from c in CRE.AddressTypeMasters
        //                       select c).ToList();
        //        if (AddType != null)
        //        {
        //            AddressType.Add(new SelectListItem { Text = "Select", Value = "0" });

        //            foreach (var item in AddType)

        //                AddressType.Add(new SelectListItem { Text = item.AddressType, Value = item.Id.ToString() });
        //        }


        //    }

        //    return AddressType;
        //}
        //-------------------------------------------
        //--------Add Board Of director------////  
        public ActionResult Adddirector(BorrowerDetails BD)
        {
            ViewData["a"] = null;
            decimal ComId = (from BCM in CRE.BorrowerCompanyMasters
                             where BCM.CompanyName == BD.CompanyName1
                             select BCM.Id).FirstOrDefault();
            ViewBag.ComId = ComId;
            ViewBag.ComName = BD.CompanyName1;
            ViewBag.ComPan = BD.ComPAN;
            DropList(BD);
            return View("Adddirector");
        }
        [HttpPost]
        public ActionResult Adddirectors(BorrowerDetails BD, HttpPostedFileBase file)
        {

            BODAdd(BD, file);
            return View();
        }
        //public ActionResult AddBOD(BorrowerDetails BD)
        //{
        //    ViewData["a"] = null;
        //    decimal ComId = (from BCM in CRE.BorrowerCompanyMasters
        //                     where BCM.CompanyName == BD.CompanyName1
        //                     select BCM.Id).FirstOrDefault();
        //    ViewBag.ComId = ComId;
        //    ViewBag.ComName = BD.CompanyName1;
        //    ViewBag.ComPan = BD.ComPAN;
        //    DropList(BD);
        //    return View("AddBOD");
        //}

        ////public List<SelectListItem> BindAddresstypeInv(BorrowerDetails BD)
        //{

        //    return BD.AddressType = GetAddressTypeInv();
        //}

        //private List<SelectListItem> GetAddressTypeInv()
        //{
        //    List<SelectListItem> AddressType = new List<SelectListItem>();





        //    var AddType = (from c in CRE.AddressTypeMasters

        //                   select c).ToList();
        //    if (AddType != null)
        //    {
        //        AddressType.Add(new SelectListItem { Text = "Select", Value = "0" });

        //        foreach (var item in AddType)

        //            AddressType.Add(new SelectListItem { Text = item.AddressType, Value = item.Id.ToString() });
        //    }




        //    return AddressType;
        //}

        public JsonResult BODStateList(string CountryId)
        {
            int id = Convert.ToInt32(CountryId);

            var states = (from s in CRE.StateMasters
                          where s.CountryId == id
                          select s);
            return Json(new SelectList(states.ToArray(), "Id", "State"), JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult BODAdd(BorrowerDetails BD, HttpPostedFileBase file)
        {
            decimal ComId = (from BCM in CRE.BorrowerCompanyMasters
                             where BCM.CompanyName == BD.CompanyName1
                             select BCM.Id).FirstOrDefault();
            BD.Companyid = Convert.ToInt32(ComId);

            var pan = (from ICM in CRE.IndividualCustomerMasters
                       where ICM.Pan == BD.PAN
                       select ICM.Pan).FirstOrDefault();

            if (pan != null)
            {
                CompanyDirectorRelation CDR = new CompanyDirectorRelation();

                int CreateUserId = Convert.ToInt32(Session["UserId"]);
                CDR.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                ViewBag.c = Session["USERNAME"];
                BD.USERNAME = ViewBag.c;
           //     var I = (from p in CRE.User_Details
           //              where p.USERNAME == BD.USERNAME
           //              select
           //              p.ID
           //).FirstOrDefault();


                CDR.CreateId = CreateUserId;
                CDR.CompanyId = BD.Companyid;
                CDR.DateOfJoining = BD.DOJ;
                var ICMid = (from ICM in CRE.IndividualCustomerMasters
                             where ICM.Pan == BD.PAN
                             select ICM.Id).FirstOrDefault();
                decimal paninv = Convert.ToInt32(ICMid);
                CDR.IndividualCustomerId = paninv;
                CRE.AddToCompanyDirectorRelations(CDR);
                CRE.SaveChanges();

                var cust = (from bdm in CRE.BorrowerDetailsMasters
                            where bdm.IndividualCustomerId == paninv
                            select bdm.Id).FirstOrDefault();


                TempData["customerid"] = cust;
                TempData.Keep("customerid");

                //var ConId = (from ICM in CRE.IndividualCustomerMasters
                //             join CD in CRE.ContactDetailsMasters on ICM.Id equals CD.IndividualCustomerId
                //             where ICM.Pan == BD.PAN
                //             select CD.Id).FirstOrDefault();
                //CDR.CompanyId = Convert.ToInt32(ConId);



            }
            else
            {
                string selectedState1 = Request.Form["selState"];
                int selectedState = Convert.ToInt32(selectedState1);
            //    ViewBag.c = Session["USERNAME"];
            //    var U = (from p in CRE.User_Details
            //             where p.USERNAME == BD.USERNAME
            //             select
            //             p.ID
            //).FirstOrDefault();
                int CreateUserId = Convert.ToInt32(Session["UserId"]);
                BD.USERNAME = ViewBag.c;


                IndividualCustomerMaster ICM = new IndividualCustomerMaster();
                ICM.FirstName = BD.FirstName;
                ICM.MiddleName = BD.MiddleName;
                ICM.LastName = BD.LastName;
                ICM.TitleId = Convert.ToInt32(BD.NID);
                ICM.NationalityId = Convert.ToInt32(BD.NationalityId);
                ICM.Pan = BD.PAN;


                ICM.Sex = BD.Sex;
                ICM.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
              //  var i2 = (from p in CRE.User_Details
              //            where p.USERNAME == BD.USERNAME
              //            select
              //            p.ID
              //).FirstOrDefault();
                ICM.CreateId = CreateUserId;
                ICM.DateOfBirth = Convert.ToDateTime(BD.DateOfBirth);
                CRE.AddToIndividualCustomerMasters(ICM);
                CRE.SaveChanges();

                BorrowerDetailsMaster BDM = new BorrowerDetailsMaster();
                BDM.IndividualCustomerId = ICM.Id;
                BDM.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                //  var U = (from p in CRE.User_Details
                //           where p.USERNAME == BD.USERNAME
                //           select
                //           p.ID
                //).FirstOrDefault();
                BDM.CreateId = CreateUserId;
                CRE.AddToBorrowerDetailsMasters(BDM);
                CRE.SaveChanges();
                TempData["customerid"] = BDM.IndividualCustomerId;
                TempData.Keep("customerid");

                CompanyDirectorRelation CDR = new CompanyDirectorRelation();
                CDR.CompanyId = Convert.ToInt32(BD.Companyid);
                CDR.DateOfJoining = Convert.ToDateTime(BD.DOJ);
                CDR.IndividualCustomerId = Convert.ToInt32(ICM.Id);
                CDR.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                CDR.CreateId = CreateUserId;
                CRE.AddToCompanyDirectorRelations(CDR);
                CRE.SaveChanges();

                ContactDetailsMaster CD = new ContactDetailsMaster();
                CD.Address1 = BD.Address1;
                CD.Address2 = BD.Address2;
                CD.Address3 = BD.Address3;
                CD.CountryId = BD.CountryId;
                CD.PAN = BD.PAN;
                CD.StateId = selectedState;
                //CD.City = BD.City;
                CD.Pincode = BD.Pincode;
                CD.LandlineCode = BD.LandlineCode;
                CD.LandlineNo = BD.LandlineNo;
                CD.MobileCode = BD.MobileCode;
                CD.MobileNo = BD.MobileNo;
                CD.EmailId1 = BD.EmailId1;
                CD.EmailId2 = BD.EmailId2;
                CD.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
              //  var N = (from p in CRE.User_Details
              //           where p.USERNAME == BD.USERNAME
              //           select
              //           p.ID
              //).FirstOrDefault();
                CD.CreateId = CreateUserId;
                CD.IndividualCustomerId = ICM.Id;
                CD.BorrowerBranchId = null;

                CRE.AddToContactDetailsMasters(CD);
                CRE.SaveChanges();



                ContactRelation CR = new ContactRelation();
                CR.IndividualCustomerId = ICM.Id;
                CR.BorrowerId = null;

                CR.AddressTypeId = BD.Addressid;


                CR.ContactDetailsId = CD.Id;
                CR.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
              //  var M = (from p in CRE.User_Details
              //           where p.USERNAME == BD.USERNAME
              //           select
              //           p.ID
              //).FirstOrDefault();
                CR.CreateId = CreateUserId;
                CRE.AddToContactRelations(CR);
                ViewBag.A = Session["USERNAME"];
                BD.USERNAME = ViewBag.A;




                var folder = Path.Combine(Server.MapPath("~/files/Individual/" + TempData["customerid"]));
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                var filesave = folder;

                var fileName = Path.GetFileName(file.FileName);

                var e = (from f in CRE.DocumentMasters where f.Id == BD.DOC select new { f.ShortName }).FirstOrDefault();

                fileName = (e.ShortName.ToString().Trim()) + "--" + fileName;

                var finalpath = folder + "/" + fileName;
                file.SaveAs(finalpath);
                BD.FilePath = finalpath;
                DocumentCustomerRelation DCR = new DocumentCustomerRelation();
                DCR.BorrowerId = BDM.Id;
                DCR.IndividualCustomerId = ICM.Id;
                DCR.CompanyTypeId = null;
                DCR.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                //  var U = (from p in CRE.User_Details
                //           where p.USERNAME == BD.USERNAME
                //           select
                //           p.ID
                //).FirstOrDefault();
                DCR.CreateId = CreateUserId;
                DCR.DocumentId = BD.DOC;
                DCR.DocumentNumber = BD.docid;
                DCR.IssueDate = BD.IssueDate;
                DCR.ExpiryDate = BD.Expirydate;
                DCR.FilePath = BD.FilePath;
                CRE.AddToDocumentCustomerRelations(DCR);
                CRE.SaveChanges();

            }
            ModelState.Clear();
            DropList(BD);

            if (BD.Designation == "PP")
            {
                return View("AddBorrower");
            }
            else
            {
                //BD.Addtype = AddType;
                ViewBag.ComId = ComId;
                ViewBag.ComName = BD.CompanyName1;
                ViewBag.ComPan = BD.ComPAN;
                return View("Adddirector");
            }
            //  ViewBag.Addressid = BindAddresstype(BD);             
        }
        //public ActionResult addBOD(BorrowerDetails BD, HttpPostedFileBase file)
        //{

        //    var pan = (from ICM in CRE.IndividualCustomerMasters
        //               where ICM.Pan == BD.PAN
        //               select ICM.Pan).FirstOrDefault();

        //    if (pan != null)
        //    {
        //        CompanyDirectorRelation CDR = new CompanyDirectorRelation();

        //        int CreateUserId = Convert.ToInt32(Session["UserId"]);
        //        CDR.CreateDate = DateTime.UtcNow.Date;

        //        CDR.CreateId = CreateUserId;
        //        CDR.CompanyId = BD.Companyid;
        //        CDR.DateOfJoining = BD.DOJ;
        //        var ICMid = (from ICM in CRE.IndividualCustomerMasters
        //                     where ICM.Pan == BD.PAN
        //                     select ICM.Id).FirstOrDefault();
        //        decimal paninv = Convert.ToInt32(ICMid);
        //        var cust = (from bdm in CRE.BorrowerDetailsMasters
        //                    where bdm.IndividualCustomerId == paninv
        //                    select bdm.Id).FirstOrDefault();

        //        CDR.IndividualCustomerId = paninv;
        //        TempData["customerid"] = cust;
        //        TempData.Keep("customerid");

        //        var ConId = (from ICM in CRE.IndividualCustomerMasters
        //                     join CD in CRE.ContactDetailsMasters on ICM.Id equals CD.IndividualCustomerId
        //                     where ICM.Pan == BD.PAN
        //                     select CD.Id).FirstOrDefault();
        //        CDR.CompanyId = Convert.ToInt32(ConId);

        //        CRE.AddToCompanyDirectorRelations(CDR);
        //        CRE.SaveChanges();


        //    }
        //    else
        //    {
        //        string selectedState1 = Request.Form["selState"];
        //        int selectedState = Convert.ToInt32(selectedState1);
        //        ViewBag.c = Session["USERNAME"];
        //        var U = (from p in CRE.User_Details
        //                 where p.USERNAME == BD.USERNAME
        //                 select
        //                 p.ID
        //    ).FirstOrDefault();
        //        int CreateUserId = Convert.ToInt32(Session["UserId"]);
        //        BD.USERNAME = ViewBag.c;


        //        IndividualCustomerMaster ICM = new IndividualCustomerMaster();
        //        ICM.FirstName = BD.FirstName;
        //        ICM.MiddleName = BD.MiddleName;
        //        ICM.LastName = BD.LastName;
        //        ICM.TitleId = Convert.ToInt32(BD.NID);
        //        ICM.NationalityId = Convert.ToInt32(BD.NationalityId);
        //        ICM.Pan = BD.PAN;


        //        ICM.Sex = BD.Sex;
        //        ICM.CreateDate = DateTime.UtcNow.Date;
        //        var i2 = (from p in CRE.User_Details
        //                  where p.USERNAME == BD.USERNAME
        //                  select
        //                  p.ID
        //      ).FirstOrDefault();
        //        ICM.CreateId = Convert.ToInt32(i2);
        //        ICM.DateOfBirth = Convert.ToDateTime(BD.DateOfBirth);
        //        CRE.AddToIndividualCustomerMasters(ICM);
        //        CRE.SaveChanges();

        //        BorrowerDetailsMaster BDM = new BorrowerDetailsMaster();
        //        BDM.IndividualCustomerId = ICM.Id;
        //        BDM.CreateDate = DateTime.UtcNow.Date;
        //        //  var U = (from p in CRE.User_Details
        //        //           where p.USERNAME == BD.USERNAME
        //        //           select
        //        //           p.ID
        //        //).FirstOrDefault();
        //        BDM.CreateId = U;
        //        CRE.AddToBorrowerDetailsMasters(BDM);
        //        CRE.SaveChanges();
        //        TempData["customerid"] = BDM.IndividualCustomerId;
        //        TempData.Keep("customerid");

        //        CompanyDirectorRelation CDR = new CompanyDirectorRelation();
        //        CDR.CompanyId = Convert.ToInt32(BD.Companyid);
        //        CDR.DateOfJoining = Convert.ToDateTime(BD.DOJ);
        //        CDR.IndividualCustomerId = Convert.ToInt32(ICM.Id);
        //        CDR.CreateDate = DateTime.UtcNow.Date;
        //        CDR.CreateId = CreateUserId;
        //        CRE.AddToCompanyDirectorRelations(CDR);
        //        CRE.SaveChanges();

        //        ContactDetailsMaster CD = new ContactDetailsMaster();
        //        CD.Address1 = BD.Address1;
        //        CD.Address2 = BD.Address2;
        //        CD.Address3 = BD.Address3;
        //        CD.CountryId = BD.CountryId;
        //        CD.PAN = BD.PAN;
        //        CD.StateId = selectedState;
        //        CD.City = BD.City;
        //        CD.Pincode = BD.Pincode;
        //        CD.LandlineCode = BD.LandlineCode;
        //        CD.LandlineNo = BD.LandlineNo;
        //        CD.MobileCode = BD.MobileCode;
        //        CD.MobileNo = BD.MobileNo;
        //        CD.EmailId1 = BD.EmailId1;
        //        CD.EmailId2 = BD.EmailId2;
        //        CD.CreateDate = DateTime.UtcNow.Date;
        //        var N = (from p in CRE.User_Details
        //                 where p.USERNAME == BD.USERNAME
        //                 select
        //                 p.ID
        //      ).FirstOrDefault();
        //        CD.CreateId = N;
        //        CD.IndividualCustomerId = ICM.Id;
        //        CD.BorrowerBranchId = null;

        //        CRE.AddToContactDetailsMasters(CD);
        //        CRE.SaveChanges();



        //        ContactRelation CR = new ContactRelation();
        //        CR.IndividualCustomerId = ICM.Id;
        //        CR.BorrowerId = null;

        //        CR.AddressTypeId = BD.Addressid;


        //        CR.ContactDetailsId = CD.Id;
        //        CR.CreateDate = DateTime.UtcNow.Date;
        //        var M = (from p in CRE.User_Details
        //                 where p.USERNAME == BD.USERNAME
        //                 select
        //                 p.ID
        //      ).FirstOrDefault();
        //        CR.CreateId = M;
        //        CRE.AddToContactRelations(CR);
        //        ViewBag.A = Session["USERNAME"];
        //        BD.USERNAME = ViewBag.A;




        //        var folder = Path.Combine(Server.MapPath("~/files/" + TempData["customerid"]));
        //        if (!Directory.Exists(folder))
        //        {
        //            Directory.CreateDirectory(folder);
        //        }




        //        var filesave = folder;

        //        var fileName = Path.GetFileName(file.FileName);

        //        var e = (from f in CRE.DocumentMasters where f.Id == BD.DOC select new { f.ShortName }).FirstOrDefault();

        //        fileName = (e.ShortName.ToString().Trim()) + "--" + fileName;

        //        var finalpath = folder + "/" + fileName;
        //        file.SaveAs(finalpath);
        //        BD.FilePath = finalpath;
        //        DocumentCustomerRelation DCR = new DocumentCustomerRelation();
        //        DCR.BorrowerId = BDM.Id;
        //        DCR.IndividualCustomerId = ICM.Id;
        //        // DCR.CompanyTypeId = null;
        //        DCR.CreateDate = DateTime.UtcNow.Date;
        //        //  var U = (from p in CRE.User_Details
        //        //           where p.USERNAME == BD.USERNAME
        //        //           select
        //        //           p.ID
        //        //).FirstOrDefault();
        //        DCR.CreateId = U;
        //        DCR.DocumentId = BD.DOC;
        //        DCR.DocumentNumber = BD.docid;
        //        DCR.IssueDate = BD.IssueDate;
        //        DCR.ExpiryDate = BD.Expirydate;
        //        DCR.FilePath = BD.FilePath;
        //        CRE.AddToDocumentCustomerRelations(DCR);
        //        CRE.SaveChanges();

        //    }

        //    ViewBag.Addressid = BindAddresstype(BD);
        //    DropList(BD);
        //    ModelState.Clear();
        //    return View("BOD");



        //}


        public ActionResult Companyid(int Companyid)
        {

            var C = (from BCM in CRE.BorrowerCompanyMasters
                     where BCM.Id == Companyid
                     select new { BCM.PAN }

                       ).FirstOrDefault();
            return Json(C, JsonRequestBehavior.AllowGet);
        }


        public JsonResult grid(string Pan)
        {

            ViewData["a"] = null;
            BorrowerDetails BD = new BorrowerDetails();
            var g = (from DCR in CRE.DocumentCustomerRelations
                     join dm in CRE.DocumentMasters on DCR.DocumentId equals dm.Id
                     join im in CRE.IndividualCustomerMasters on DCR.IndividualCustomerId equals im.Id
                     where im.Pan == Pan
                     select new
                     {

                         dm.Document,
                         DCR.DocumentNumber,
                         fullnames = im.FirstName + " " + im.MiddleName + " " + im.LastName,
                         DCR.IssueDate,
                         DCR.ExpiryDate,



                     }
                          ).FirstOrDefault();
            ViewBag.g = g;
          //  ViewBag.Address = BindAddresstype(BD);
            DropList(BD);
            return Json(g, JsonRequestBehavior.AllowGet);
        }
        public JsonResult boardofDir(string Pan)
        {



            var strSql = (from icm in CRE.IndividualCustomerMasters
                          join CD in CRE.ContactDetailsMasters on icm.Id equals CD.IndividualCustomerId
                          join CR in CRE.ContactRelations on CD.Id equals CR.ContactDetailsId
                          //join CDR in CRE.CompanyDirectorRelations on icm.Id equals CDR.IndividualCustomerId
                          join SM in CRE.StateMasters on CD.StateId equals SM.Id
                          where icm.Pan == Pan
                          select new
                          {
                              icm.FirstName,
                              icm.MiddleName,
                              icm.LastName,
                              icm.TitleId,
                              icm.NationalityId,
                              icm.DateOfBirth,
                              icm.Sex,
                              CD.Address1,
                              CD.Address2,
                              CD.Address3,
                              SM.State,
                              CD.CountryId,
                              CD.StateId,
                              //CD.City,
                              CD.Pincode,
                              CD.LandlineCode,
                              CD.LandlineNo,
                              CD.MobileCode,
                              CD.MobileNo,
                              CD.EmailId1,
                              CD.EmailId2,
                              CR.AddressTypeId,
                              //CDR.DateOfJoining

                          }
                          ).FirstOrDefault();
            if (strSql != null)
            {
                ViewBag.SUCEESS = "Y";
                return Json(strSql, JsonRequestBehavior.AllowGet);
            }
            else
            {
                ViewBag.SUCEESS = "N";
                var strDisable = "true";
                return Json(strDisable);
            }
        }




        //--------Add board of director---15092015----//
        //public ActionResult Adddirector(BorrowerDetails BD)
        //{
        //    ViewBag.Addressid = BindAddresstype(BD);
        //    DropList(BD);
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult  Adddirectordetails(BorrowerDetails BD, HttpPostedFileBase file)
        //{

        //    var pan = (from ICM in CRE.IndividualCustomerMasters
        //               where ICM.Pan == BD.PAN
        //               select ICM.Pan).FirstOrDefault();

        //    if (pan != null)
        //    {
        //        CompanyDirectorRelation CDR = new CompanyDirectorRelation();


        //        CDR.CompanyId = BD.Companyid;
        //        CDR.DateOfJoining = BD.DOJ;
        //        var ICMid = (from ICM in CRE.IndividualCustomerMasters
        //                     where ICM.Pan == BD.PAN
        //                     select ICM.Id).FirstOrDefault();
        //        decimal paninv = Convert.ToInt32(ICMid);
        //        var cust = (from bdm in CRE.BorrowerDetailsMasters
        //                    where bdm.IndividualCustomerId == paninv
        //                    select bdm.Id).FirstOrDefault();

        //        CDR.IndividualCustomerId = paninv;
        //        TempData["customerid"] = cust;
        //        TempData.Keep("customerid");

        //        var ConId = (from ICM in CRE.IndividualCustomerMasters
        //                     join CD in CRE.ContactDetailsMasters on ICM.Id equals CD.IndividualCustomerId
        //                     where ICM.Pan == BD.PAN
        //                     select CD.Id).FirstOrDefault();
        //        CDR.CompanyId = Convert.ToInt32(ConId);

        //        CRE.AddToCompanyDirectorRelations(CDR);
        //        CRE.SaveChanges();


        //    }
        //    else
        //    {
        //        string selectedState1 = Request.Form["selState"];
        //        int selectedState = Convert.ToInt32(selectedState1);
        //        ViewBag.c = Session["USERNAME"];
        //        BD.USERNAME = ViewBag.c;

        //        IndividualCustomerMaster ICM = new IndividualCustomerMaster();
        //        ICM.FirstName = BD.FirstName;
        //        ICM.MiddleName = BD.MiddleName;
        //        ICM.LastName = BD.LastName;
        //        ICM.TitleId = Convert.ToInt32(BD.NID);
        //        ICM.NationalityId = Convert.ToInt32(BD.NationalityId);
        //        ICM.Pan = BD.PAN;


        //        ICM.Sex = BD.Sex;
        //        ICM.CreateDate = DateTime.UtcNow.Date;
        //        var i2 = (from p in CRE.User_Details
        //                  where p.USERNAME == BD.USERNAME
        //                  select
        //                  p.ID
        //      ).FirstOrDefault();
        //        ICM.CreateId = Convert.ToInt32(i2);
        //        ICM.DateOfBirth = Convert.ToDateTime(BD.DateOfBirth);
        //        CRE.AddToIndividualCustomerMasters(ICM);
        //        CRE.SaveChanges();

        //        BorrowerDetailsMaster BDM = new BorrowerDetailsMaster();
        //        BDM.IndividualCustomerId = ICM.Id;
        //        CRE.AddToBorrowerDetailsMasters(BDM);
        //        CRE.SaveChanges();
        //        TempData["customerid"] = BDM.IndividualCustomerId;
        //        TempData.Keep("customerid");

        //        CompanyDirectorRelation CDR = new CompanyDirectorRelation();
        //        CDR.CompanyId = Convert.ToInt32(BD.Companyid);
        //        CDR.DateOfJoining = Convert.ToDateTime(BD.DOJ);
        //        CDR.IndividualCustomerId = Convert.ToInt32(ICM.Id);
        //        CRE.AddToCompanyDirectorRelations(CDR);
        //        CRE.SaveChanges();

        //        ContactDetailsMaster CD = new ContactDetailsMaster();
        //        CD.Address1 = BD.Address1;
        //        CD.Address2 = BD.Address2;
        //        CD.Address3 = BD.Address3;
        //        CD.CountryId = BD.CountryId;
        //        CD.PAN = BD.PAN;
        //        CD.StateId = selectedState;
        //        CD.City = BD.City;
        //        CD.Pincode = BD.Pincode;
        //        CD.LandlineCode = BD.LandlineCode;
        //        CD.LandlineNo = BD.LandlineNo;
        //        CD.MobileCode = BD.MobileCode;
        //        CD.MobileNo = BD.MobileNo;
        //        CD.EmailId1 = BD.EmailId1;
        //        CD.EmailId2 = BD.EmailId2;
        //        CD.CreateDate = DateTime.UtcNow.Date;
        //        var N = (from p in CRE.User_Details
        //                 where p.USERNAME == BD.USERNAME
        //                 select
        //                 p.ID
        //      ).FirstOrDefault();
        //        CD.CreateId = N;
        //        CD.IndividualCustomerId = ICM.Id;
        //        CD.BorrowerBranchId = null;

        //        CRE.AddToContactDetailsMasters(CD);
        //        CRE.SaveChanges();



        //        ContactRelation CR = new ContactRelation();
        //        CR.IndividualCustomerId = ICM.Id;
        //        CR.BorrowerId = null;
        //        CR.AddressTypeId = BD.Addressid;
        //        CR.ContactDetailsId = CD.Id;
        //        CR.CreateDate = DateTime.UtcNow.Date;
        //        var M = (from p in CRE.User_Details
        //                 where p.USERNAME == BD.USERNAME
        //                 select
        //                 p.ID
        //      ).FirstOrDefault();
        //        CR.CreateId = M;
        //        CRE.AddToContactRelations(CR);
        //        ViewBag.A = Session["USERNAME"];
        //        BD.USERNAME = ViewBag.A;

        //        var folder = Path.Combine(Server.MapPath("~/files/" + TempData["customerid"]));
        //        if (!Directory.Exists(folder))
        //        {
        //            Directory.CreateDirectory(folder);
        //        }


        //        var filesave = folder;

        //        var fileName = Path.GetFileName(file.FileName);

        //        var e = (from f in CRE.DocumentMasters where f.Id == BD.DOC select new { f.ShortName }).FirstOrDefault();

        //        fileName = (e.ShortName.ToString().Trim()) + "--" + fileName;

        //        var finalpath = folder + "/" + fileName;
        //        file.SaveAs(finalpath);
        //        BD.FilePath = finalpath;

        //        DocumentCustomerRelation DCR = new DocumentCustomerRelation();
        //        DCR.BorrowerId = BDM.Id;
        //        DCR.IndividualCustomerId = ICM.Id;
        //        DCR.CompanyTypeId = null;
        //        DCR.CreateDate = DateTime.UtcNow.Date;
        //        var U = (from p in CRE.User_Details
        //                 where p.USERNAME == BD.USERNAME
        //                 select
        //                 p.ID
        //      ).FirstOrDefault();
        //        DCR.CreateId = U;
        //        DCR.DocumentId = BD.DOC;
        //        DCR.DocumentNumber = BD.docid;
        //        DCR.IssueDate = BD.IssueDate;
        //        DCR.ExpiryDate = BD.Expirydate;
        //        DCR.FilePath = BD.FilePath;
        //        CRE.AddToDocumentCustomerRelations(DCR);
        //        CRE.SaveChanges();

        //    }

        //    ViewBag.Addressid = BindAddresstype(BD);
        //    DropList(BD);
        //    ModelState.Clear();
        //    return View("BOD");

        //}


        //public JsonResult grid(string Pan)
        //{
        //    BorrowerDetails BD = new BorrowerDetails();
        //    var g = (from DCR in CRE.DocumentCustomerRelations
        //             join dm in CRE.DocumentMasters on DCR.DocumentId equals dm.Id
        //             join im in CRE.IndividualCustomerMasters on DCR.IndividualCustomerId equals im.Id
        //             where im.Pan == Pan
        //             select new
        //             {

        //                 dm.Document,
        //                 DCR.DocumentNumber,
        //                 fullnames = im.FirstName + " " + im.MiddleName + " " + im.LastName,
        //                 DCR.IssueDate,
        //                 DCR.ExpiryDate,



        //             }
        //                  ).FirstOrDefault();
        //    ViewBag.g = g;
        //    ViewBag.Address = BindAddresstype(BD);
        //    DropList(BD);
        //    return Json(g, JsonRequestBehavior.AllowGet);
        //}
        //public JsonResult BoardOfDir(string Pan)
        //{
        //    var strSql = (from icm in CRE.IndividualCustomerMasters
        //                  join CD in CRE.ContactDetailsMasters on icm.Id equals CD.IndividualCustomerId
        //                  join CR in CRE.ContactRelations on CD.Id equals CR.ContactDetailsId
        //                  join SM in CRE.StateMasters on CD.StateId equals SM.Id
        //                  where icm.Pan == Pan
        //                  select new
        //                  {
        //                      icm.FirstName,
        //                      icm.MiddleName,
        //                      icm.LastName,
        //                      icm.TitleId,
        //                      icm.NationalityId,
        //                      icm.DateOfBirth,
        //                      icm.Sex,
        //                      CD.Address1,
        //                      CD.Address2,
        //                      CD.Address3,
        //                      SM.State,
        //                      CD.CountryId,
        //                      CD.StateId,
        //                      CD.City,
        //                      CD.Pincode,
        //                      CD.LandlineCode,
        //                      CD.LandlineNo,
        //                      CD.MobileCode,
        //                      CD.MobileNo,
        //                      CD.EmailId1,
        //                      CD.EmailId2,
        //                      CR.AddressTypeId,



        //                  }
        //                  ).FirstOrDefault();
        //    if (strSql != null)
        //    {

        //        return Json(strSql, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        var strDisable = "true";
        //        return Json(strDisable);
        //    }
        //}

        public JsonResult Designation(int ComId)
        {
            var g = (from BCM in CRE.BorrowerCompanyMasters
                     where BCM.Id == ComId
                     select new
                     {
                         BCM.Designation
                     }
                          ).FirstOrDefault();
            return Json(g, JsonRequestBehavior.AllowGet);
        }
    }
}
