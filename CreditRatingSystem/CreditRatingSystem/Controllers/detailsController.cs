using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CreditRatingModel.Model;
using System.IO;


namespace CreditRating.Controllers
{
    public class detailsController : Controller
    {
        public string detail;
        public string demo1;
        creaditratingEntities CRE = new creaditratingEntities();

        public ActionResult demo()
        {
            return View();
        }

        public ActionResult Index()
        {



            return View();
        }
        public void DropList(BorrowerDetails BD)
        {
            ViewBag.Country = new SelectList(CRE.CountryMasters.ToList(), "Id", "Country");
            ViewBag.Name = new SelectList(CRE.NameTitleMasters.ToList(), "Id", "Name");
            ViewBag.Nationality = new SelectList(CRE.NationalityMasters.ToList(), "Id", "Nationality");
            ViewBag.Companyid = new SelectList(CRE.BorrowerCompanyMasters.ToList(), "Id", "CompanyName");
            ViewBag.AddressidInv = BindAddresstypeInv(BD);
            ViewBag.DOC = BindDoc(BD);
        }

        public ActionResult Bdetails()
        {

            var UsrType = Session["UserType"];


            var e = (from m in CRE.User_Details
                     join c in CRE.UserTypeMasters on m.USER_TYPE_ID equals c.Id
                     where m.USER_TYPE_ID == 1
                     select new
                     {
                         c.UserTypeShortName

                     }).FirstOrDefault();
            TempData["details"] = UsrType.ToString().Trim(); ;
            TempData["M"] = e.UserTypeShortName.ToString().Trim();

            var f = TempData["details"];
            var o = TempData["M"];
            TempData.Keep("M");
            TempData.Keep("details");



            ViewBag.TabYN = "N";
            ViewBag.TabYN1 = "Y";

            BorrowerDetails BD = new BorrowerDetails();
            AddInfo ai = new AddInfo();

            ViewBag.Address = BindAddresstype(BD);

            DropList(BD);
            return View(BD);

        }
        private void InsertInfo1(int NID, int NationalityId, string USERNAME, int id, string PAN, string FirstName, string MiddleName, string LastName, string Sex, Nullable<DateTime> DateOfBirth, string CompanyName, Nullable<DateTime> IncorporationDate, Nullable<DateTime> CommencementDate, string CompanyType, string BranchName)
        {
            InsertInfo i = new InsertInfo();
        //    i.Add(NID, NationalityId, USERNAME, id, PAN, FirstName, MiddleName, LastName, Sex, DateOfBirth, CompanyName, IncorporationDate, CommencementDate, CompanyType, BranchName);


        }
        private void InsertInfo2(string USERNAME, string Address1, string Address2, string Address3, int Addressid, int CountryId, int StateId, string City, string Pincode, string LandlineCode, string LandlineNo, string MobileCode, string MobileNo, string EmailId1, string EmailId2)
        {
            InsertInfo i2 = new InsertInfo();
            //i2.Addinfo(USERNAME, Address1, Address2, Address3, Addressid, CountryId, StateId, City, Pincode, LandlineCode, LandlineNo, MobileCode, MobileNo, EmailId1, EmailId2);
        }



        [HttpPost]
        public ActionResult btnAddClick(BorrowerDetails BD)
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
            InsertInfo1(BD.NID, BD.NationalityId, BD.USERNAME, BD.id, BD.PAN, BD.FirstName, BD.MiddleName, BD.LastName, BD.Sex, BD.DateOfBirth, BD.CompanyName, BD.IncorporationDate, BD.CommencementDate, BD.CompanyType, BD.BranchName);


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
            ViewBag.Address = BindAddresstype(BD);

            return View("Bdetails", BD);




        }


        [HttpPost]
        public ActionResult btnAddClick1(BorrowerDetails BD)
        {

            if (ModelState.IsValid)
            {

                ViewBag.A = Session["USERNAME"];
                BD.USERNAME = ViewBag.A;
                string selectedState1 = Request.Form["selState"];
                int selectedState = Convert.ToInt32(selectedState1);
                InsertInfo2(BD.USERNAME, BD.Address1, BD.Address2, BD.Address3, BD.Addressid, BD.CountryId, selectedState, BD.City, BD.Pincode, BD.LandlineCode, BD.LandlineNo, BD.MobileCode, BD.MobileNo, BD.EmailId1, BD.EmailId2);
            }




            ViewBag.DOC = BindDoc(BD);
            DropList(BD);
            ViewBag.Address = BindAddresstype(BD);




            ViewBag.TabYN1 = "N";
            TempData["M"] = Session["UserType"];

            TempData["details"] = (from m in CRE.User_Details
                                   join c in CRE.UserTypeMasters on m.USER_TYPE_ID equals c.Id
                                   where m.USER_TYPE_ID == 1
                                   select new
                                   {
                                       c.UserTypeShortName

                                   }).FirstOrDefault();

            TempData.Keep("M");
            TempData.Keep("details");
            return View("Bdetails", BD);



        }

        private void InsertInfo3(int DOC, String docid, Nullable<DateTime> IssueDate, Nullable<DateTime> Expirydate, string FilePath)
        {
            InsertInfo i3 = new InsertInfo();
            //i3.AddUploads(DOC, docid, IssueDate, Expirydate, FilePath);
        }

        [HttpPost]
        public ActionResult upload(BorrowerDetails BD, HttpPostedFileBase file)
        {

            if (ModelState.IsValid)
            {

                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/files"), fileName);
                file.SaveAs(path);
                BD.FilePath = path;
                InsertInfo3(BD.DOC, BD.docid, BD.IssueDate, BD.Expirydate, BD.FilePath);
            }
            return View();
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

        private List<SelectListItem> GetCountry()
        {
            List<SelectListItem> country = new List<SelectListItem>();

            try
            {
                var countries = (from c in CRE.CountryMasters
                                 select c).ToList();

                if (countries != null)
                {
                    country.Add(new SelectListItem { Text = "----Select----", Value = "0" });

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
        public List<SelectListItem> BindName(BorrowerDetails BD)
        {
            return BD.Name = GetDoc();
        }

        private List<SelectListItem> GetName()
        {
            List<SelectListItem> Name = new List<SelectListItem>();

            try
            {
                var name = (from c in CRE.NameTitleMasters
                            select c).ToList();

                if (name != null)
                {
                    Name.Add(new SelectListItem { Text = "Select", Value = "0" });

                    foreach (var item in name)
                    {
                        Name.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });

                    }
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            return Name;
        }
        public List<SelectListItem> BindNation(BorrowerDetails BD)
        {
            return BD.Nationality = GetNation();
        }

        private List<SelectListItem> GetNation()
        {
            List<SelectListItem> nation = new List<SelectListItem>();

            try
            {
                var Nation = (from c in CRE.NationalityMasters
                              select c).ToList();

                if (Nation != null)
                {
                    nation.Add(new SelectListItem { Text = "Select", Value = "0" });

                    foreach (var item in Nation)
                    {
                        nation.Add(new SelectListItem { Text = item.Nationality, Value = item.Id.ToString() });

                    }
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            return nation;
        }



        public List<SelectListItem> BindAddresstype(BorrowerDetails BD)
        {

            return BD.AddressType = GetAddressType();
        }

        private List<SelectListItem> GetAddressType()
        {
            List<SelectListItem> AddressType = new List<SelectListItem>();
            var UsrType = Session["UserType"];


            var e = (from m in CRE.User_Details
                     join c in CRE.UserTypeMasters on m.USER_TYPE_ID equals c.Id
                     where m.USER_TYPE_ID == 1
                     select new
                     {
                         c.UserTypeShortName

                     }).FirstOrDefault();
            TempData["details"] = UsrType.ToString().Trim(); ;
            TempData["M"] = e.UserTypeShortName.ToString().Trim();

            var f = TempData["details"];
            var o = TempData["M"];
            TempData.Keep("M");
            TempData.Keep("details");


            if (f.ToString().Trim() == o.ToString().Trim())
            {
                var AddType = (from c in CRE.AddressTypeMasters
                               where c.Id == 1
                               select c).ToList();
                if (AddType != null)
                {
                    AddressType.Add(new SelectListItem { Text = "Select", Value = "0" });

                    foreach (var item in AddType)

                        AddressType.Add(new SelectListItem { Text = item.AddressType, Value = item.Id.ToString() });
                }


            }
            else
            {
                var AddType = (from c in CRE.AddressTypeMasters
                               select c).ToList();
                if (AddType != null)
                {
                    AddressType.Add(new SelectListItem { Text = "Select", Value = "0" });

                    foreach (var item in AddType)

                        AddressType.Add(new SelectListItem { Text = item.AddressType, Value = item.Id.ToString() });
                }


            }

            return AddressType;
        }

  //--------Add Board Of director------////    
        public ActionResult BOD(BorrowerDetails BD)
        {




            ViewData["a"] = null;
            ViewBag.Addressid = BindAddresstype(BD);
            DropList(BD);
            return View();

        }

        public List<SelectListItem> BindAddresstypeInv(BorrowerDetails BD)
        {

            return BD.AddressType = GetAddressTypeInv();
        }

        private List<SelectListItem> GetAddressTypeInv()
        {
            List<SelectListItem> AddressType = new List<SelectListItem>();





            var AddType = (from c in CRE.AddressTypeMasters

                           select c).ToList();
            if (AddType != null)
            {
                AddressType.Add(new SelectListItem { Text = "Select", Value = "0" });

                foreach (var item in AddType)

                    AddressType.Add(new SelectListItem { Text = item.AddressType, Value = item.Id.ToString() });
            }




            return AddressType;
        }

        public JsonResult BODStateList(string CountryId)
        {
            int id = Convert.ToInt32(CountryId);

            var states = (from s in CRE.StateMasters
                          where s.CountryId == id
                          select s);
            return Json(new SelectList(states.ToArray(), "Id", "State"), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult addBOD(BorrowerDetails BD, HttpPostedFileBase file)
        {

            var pan = (from ICM in CRE.IndividualCustomerMasters
                       where ICM.Pan == BD.PAN
                       select ICM.Pan).FirstOrDefault();
        
            if (pan != null)
            {
                CompanyDirectorRelation CDR = new CompanyDirectorRelation();

                int CreateUserId = Convert.ToInt32(Session["UserId"]);
                CDR.CreateDate = DateTime.UtcNow.Date;

                CDR.CreateId = CreateUserId;
                CDR.CompanyId = BD.Companyid;
                CDR.DateOfJoining = BD.DOJ;
                var ICMid = (from ICM in CRE.IndividualCustomerMasters
                             where ICM.Pan == BD.PAN
                             select ICM.Id).FirstOrDefault();
                decimal paninv = Convert.ToInt32(ICMid);
                var cust = (from bdm in CRE.BorrowerDetailsMasters
                            where bdm.IndividualCustomerId == paninv
                            select bdm.Id).FirstOrDefault();

                CDR.IndividualCustomerId = paninv;
                TempData["customerid"] = cust;
                TempData.Keep("customerid");

                var ConId = (from ICM in CRE.IndividualCustomerMasters
                             join CD in CRE.ContactDetailsMasters on ICM.Id equals CD.IndividualCustomerId
                             where ICM.Pan == BD.PAN
                             select CD.Id).FirstOrDefault();
                CDR.CompanyId = Convert.ToInt32(ConId);

                CRE.AddToCompanyDirectorRelations(CDR);
                CRE.SaveChanges();


            }
            else
            {
                string selectedState1 = Request.Form["selState"];
                int selectedState = Convert.ToInt32(selectedState1);
                ViewBag.c = Session["USERNAME"];
                var U = (from p in CRE.User_Details
                         where p.USERNAME == BD.USERNAME
                         select
                         p.ID
            ).FirstOrDefault();
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
                ICM.CreateDate = DateTime.UtcNow.Date;
                var i2 = (from p in CRE.User_Details
                          where p.USERNAME == BD.USERNAME
                          select
                          p.ID
              ).FirstOrDefault();
                ICM.CreateId = Convert.ToInt32(i2);
                ICM.DateOfBirth = Convert.ToDateTime(BD.DateOfBirth);
                CRE.AddToIndividualCustomerMasters(ICM);
                CRE.SaveChanges();

                BorrowerDetailsMaster BDM = new BorrowerDetailsMaster();
                BDM.IndividualCustomerId = ICM.Id;
                BDM.CreateDate = DateTime.UtcNow.Date;
              //  var U = (from p in CRE.User_Details
              //           where p.USERNAME == BD.USERNAME
              //           select
              //           p.ID
              //).FirstOrDefault();
                BDM.CreateId = U;
                CRE.AddToBorrowerDetailsMasters(BDM);
                CRE.SaveChanges();
                TempData["customerid"] = BDM.IndividualCustomerId;
                TempData.Keep("customerid");

                CompanyDirectorRelation CDR = new CompanyDirectorRelation();
                CDR.CompanyId = Convert.ToInt32(BD.Companyid);
                CDR.DateOfJoining = Convert.ToDateTime(BD.DOJ);
                CDR.IndividualCustomerId = Convert.ToInt32(ICM.Id);
                CDR.CreateDate = DateTime.UtcNow.Date;
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
                CD.CreateDate = DateTime.UtcNow.Date;
                var N = (from p in CRE.User_Details
                         where p.USERNAME == BD.USERNAME
                         select
                         p.ID
              ).FirstOrDefault();
                CD.CreateId = N;
                CD.IndividualCustomerId = ICM.Id;
                CD.BorrowerBranchId = null;

                CRE.AddToContactDetailsMasters(CD);
                CRE.SaveChanges();



                ContactRelation CR = new ContactRelation();
                CR.IndividualCustomerId = ICM.Id;
                CR.BorrowerId = null;

                CR.AddressTypeId = BD.Addressid;


                CR.ContactDetailsId = CD.Id;
                CR.CreateDate = DateTime.UtcNow.Date;
                var M = (from p in CRE.User_Details
                         where p.USERNAME == BD.USERNAME
                         select
                         p.ID
              ).FirstOrDefault();
                CR.CreateId = M;
                CRE.AddToContactRelations(CR);
                ViewBag.A = Session["USERNAME"];
                BD.USERNAME = ViewBag.A;




                var folder = Path.Combine(Server.MapPath("~/files/" + TempData["customerid"]));
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
               // DCR.CompanyTypeId = null;
                DCR.CreateDate = DateTime.UtcNow.Date;
              //  var U = (from p in CRE.User_Details
              //           where p.USERNAME == BD.USERNAME
              //           select
              //           p.ID
              //).FirstOrDefault();
                DCR.CreateId = U;
                DCR.DocumentId = BD.DOC;
                DCR.DocumentNumber = BD.docid;
                DCR.IssueDate = BD.IssueDate;
                DCR.ExpiryDate = BD.Expirydate;
                DCR.FilePath = BD.FilePath;
                CRE.AddToDocumentCustomerRelations(DCR);
                CRE.SaveChanges();

            }

            ViewBag.Addressid = BindAddresstype(BD);
            DropList(BD);
            ModelState.Clear();
       return View("BOD");

       
            
        }


        public ActionResult Companyid(int Companyid)
        {
           
            var C = (from BCM in CRE.BorrowerCompanyMasters
                     where BCM.Id == Companyid
                     select new { BCM.PAN}

                       ).FirstOrDefault();
            return Json( C, JsonRequestBehavior.AllowGet);
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
            ViewBag.Address = BindAddresstype(BD);
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

                return Json(strSql, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var strDisable = "true";
                return Json(strDisable);
            }
        }
    }

}










