using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using CreditRatingModel.Model;
using System.Transactions;

namespace CreditRatingSystem.Controllers
{
    public class EmployeeController : Controller
    {
        creaditratingEntities db = new creaditratingEntities();
        EmployeeMaster EMP=new EmployeeMaster();
        public string NId;
        //
        // GET: /Employee/
        public void DropList()
        {
            int intUserId = Convert.ToInt32(Session["EmployerName"]);
            int intDivId = Convert.ToInt32(Session["EmployerDivision"]);

            ViewBag.EmployerId = new SelectList(db.EmployerMasters.ToList(), "Id", "EmployerName");
            ViewBag.BranchList = new SelectList((from BM in db.EmployerBranchMasters where BM.EmployerId == intUserId && BM.EmployerDivisionId == intDivId select new { BM.Id, BM.BranchName }).ToList(), "Id", "BranchName");
            ViewBag.Country = new SelectList(db.CountryMasters.ToList(), "Id", "Country");
            ViewBag.Department = new SelectList(db.DepartmentMasters.ToList(), "Id", "Department");
            ViewBag.Designation = new SelectList((from DM in db.DesignationMasters where DM.DesignationType == "E" select new { DM.Id,DM.Designation}).ToList(), "Id", "Designation");
            ViewBag.Name = new SelectList(db.NameTitleMasters.ToList(), "Id", "Name");
            ViewBag.State = new SelectList(db.StateMasters.ToList(), "Id", "State");
            //ViewBag.College = new SelectList(db.CollegeMasters.ToList(), "Id", "CollegeName");
            //ViewBag.Course = new SelectList(db.CourseMasters.ToList(), "Id", "CourseName");
            ViewBag.MaritalStatusList = new SelectList(db.MaritalStatusMasters.ToList(), "Id", "MaritalStatus");
            ViewBag.TitleId = new SelectList(db.NameTitleMasters.ToList(), "Id", "Name");
            ViewBag.City = new SelectList(db.CityMasters.ToList(), "Id", "City");
        }
       
        //public ActionResult Index()
        //{
        //    DropList(EMP);
        //    return View();
       
        //}
        [HttpGet]
        public ActionResult AddEmployee()
        {
            int intUserId = Convert.ToInt32(Session["EmployerName"]);
            int intDivId = Convert.ToInt32(Session["EmployerDivision"]);
            int EmpBranchId = Convert.ToInt32(Session["EmployerDivisionBranch"]);
            var EMID = (from UD in db.EmployerMasters
                        where UD.Id == intUserId
                        select UD.EmployerName
                ).FirstOrDefault();
            var EMDIVID = (from UD in db.EmployerDivisionMasters
                           where UD.Id == intDivId
                           select UD.EmployerDivision
                ).FirstOrDefault();

            var EMBranchID = (from UD in db.EmployerBranchMasters
                              where UD.Id == EmpBranchId
                              select UD.BranchName
                ).FirstOrDefault();
            ViewBag.EMName = EMID;
            ViewBag.EMDIVName = EMDIVID;
            ViewBag.EMBranchName = EMBranchID;
            DropList();
            return View();
        }

         [Authorize]
        [HttpPost]
        public ActionResult AddEmployee(EmployeeMaster EMP)
        {
            string msg;
            using (TransactionScope transaction = new TransactionScope())
            {
                try
                {
                    EmployeeAdd(EMP);
                    transaction.Complete();
                    msg = "Data saved successfully!";
                }
                catch (Exception)
                {
                    transaction.Dispose();
                    msg = "Data saved Failed!";
                }
            }

            return Content(("<script language='javascript' type='text/javascript'> alert('" + msg + "'); window.location.href ='/Employee/AddEmployee';  </script>"));
            
  
        }
        [HttpPost]
        public ActionResult CountryCode(string CountryId)
        {
            int id = Convert.ToInt32(CountryId);
            var CountryCode = (from s in db.CountryMasters
                               where s.Id == id
                               select s.CountryCode);
            return Json(CountryCode);
        }

        public IList<StateMaster> BindState(int countryid)
        {
            return db.StateMasters.Where(s => s.CountryId == countryid).ToList();
        }

        public JsonResult StateList(string CountryId)
        {
            int id = Convert.ToInt32(CountryId);

            var states = (from s in db.StateMasters
                          where s.CountryId == id
                          select s);
            return Json(new SelectList(states.ToArray(), "Id", "State"), JsonRequestBehavior.AllowGet);
        }
        public JsonResult EmployerTypeList(int EmployerId)
        {
            //int empName = Convert.ToInt32(EmployerTypeList);

            var B = (from s in db.EmployerDivisionMasters
                     where s.EmployerId == EmployerId
                     select s);

            return Json(new SelectList(B.ToArray(), "Id", "EmployerDivision"), JsonRequestBehavior.AllowGet);
        }
        public JsonResult EmployerBranchList(int EmployerId, int EmployerTypeId)
        {
            //int empName = Convert.ToInt32(EmployerTypeList);

            var B = (from s in db.EmployerBranchMasters
                     where s.EmployerId == EmployerId && s.EmployerDivisionId == EmployerTypeId
                     select s);

            return Json(new SelectList(B.ToArray(), "Id", "Branchname"), JsonRequestBehavior.AllowGet);
        }
        [Authorize]
    [HttpPost]
        public ActionResult EmployeeAdd(EmployeeMaster EMP)
        {
            int EmployerId = Convert.ToInt32(Session["EmployerName"]);
            int DivId = Convert.ToInt32(Session["EmployerDivision"]);
            int BranchId = Convert.ToInt32(Session["EmployerDivisionBranch"]);
            int intUserId = Convert.ToInt32(Session["UserId"]);

            string selectedState1 = Request.Form["selState"];
            int selectedState = Convert.ToInt32(selectedState1);

            var EduLoan = (from ELD in db.EducationLoanDisbursments
                           where ELD.NId == EMP.NID
                           select new EmployeeMaster
                            {
                                Id = ELD.Id,
                                LoanAmount=ELD.LoanAmount.Value
                            });
      
    
        var B = (from ICM in db.IndividualCustomerMasters
                 join NTM in db.NameTitleMasters on ICM.TitleId equals NTM.Id
                 where ICM.Pan == EMP.NID
                 select new EmployeeMaster
                 {                    
                     DateOfBirth = ICM.DateOfBirth.Value,
                     Sex=ICM.Sex,
                     TitleId1=ICM.TitleId.Value,
                     Title=NTM.Name,
                     FirstName=ICM.FirstName,
                     MiddleName=ICM.MiddleName,
                     LastName=ICM.LastName
                 //    Id=ICM.Id
                 }).FirstOrDefault();


            var CheckEmployeeStatus=(from a in db.EmployeeDetails where a.NId==EMP.NID && a.ActiveEmployee==true select a).FirstOrDefault();

            if (CheckEmployeeStatus != null)
            {
                CheckEmployeeStatus.ActiveEmployee = false;
                db.SaveChanges();
            }

            try
            {
                EmployeeDetail EM = new EmployeeDetail();
                EM.NId = EMP.NID;
                EM.Name = B.Title + " " + B.FirstName + " " + B.MiddleName + " " + B.LastName;           
                ////////////////////EM.EducationLoanDisburseId = EduLoan.Id;
                EM.EmployerId = EmployerId;
                EM.DateOfJoining = EMP.DateOfJoining;
                EM.DesignationId = EMP.DesignationId;
                EM.DepartmentId = EMP.DepartmentId;
                EM.CurrentDepartmentId = EMP.DepartmentId;
                EM.CurrentDesignationId = EMP.DesignationId;
                EM.CompanyEmployeeId = EMP.EmployeeID;
                EM.Salary = EMP.Salary;
                EM.ContactNo = EMP.MobileCode + "-" + EMP.MobileNo;
                EM.EmailId = EMP.EmailId;
                EM.DivisionId = DivId;
                EM.EmployerTypeId = DivId;
                EM.EmployerBranchId = BranchId;
                EM.ActiveEmployee = true;
                //EM.TotalLoanAmount = EMP.TotalLoanAmount;
                //EM.TotalBalanceAmount = EMP.TotalBalanceAmount;
                EM.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                EM.CreateId = intUserId;
                db.AddToEmployeeDetails(EM);
                db.SaveChanges();
              
                foreach (var item in EduLoan)
                {
                    EmployeeLoanDetail ELD = new EmployeeLoanDetail();
                    ELD.EmployeeId = EM.Id;
                    ELD.SlabId = Convert.ToInt32(EMP.SlabId);
                    ELD.EducationLoanDisburseId =item.Id;
                    ELD.EMIAmount = EMP.MonthlyEMI;
                    ELD.EffectiveDate = EMP.SalEffectiveDate;
                    ELD.Remark = EMP.Remark;
                    ELD.Status = EMP.Status;
                    ELD.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                    ELD.CreateId = intUserId;
                    db.AddToEmployeeLoanDetails(ELD);
                }
                db.SaveChanges();

                //if (EMP.AddressChangedCount == 1)
                //{
                    ContactDetail CD = new ContactDetail();
                    CD.NID = EMP.NID;
                    CD.Address1 = EMP.Address1;
                    CD.Address2 = EMP.Address2;
                    CD.Address3 = EMP.Address3;
                    CD.CityId = EMP.CityId;
                    CD.StateId = EMP.StateId;
                    CD.CountryId = EMP.CountryId;
                    CD.Pincode = EMP.Pincode;
                    CD.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                    CD.CreateId = intUserId;
                    db.AddToContactDetails(CD);
                    db.SaveChanges();
                //}
                EmployeeHistoryDetail EHD = new EmployeeHistoryDetail();
                EHD.EmployeeId = EM.Id;
                EHD.EmployerId = EmployerId;
                EHD.EmployerDivisionid = DivId;
                EHD.EmployerBranchId = BranchId;
                EHD.NID = EMP.NID;
                EHD.CompanyEmployeeId = EMP.EmployeeID;
                EHD.DateOfJoining = EMP.DateOfJoining;
                EHD.SalaryEffectiveDate = EMP.SalEffectiveDate;
                 EHD.Salary=EMP.Salary;
                EHD.SlabId= Convert.ToInt32(EMP.SlabId);
               EHD.EMIAmount= EMP.MonthlyEMI;
                    EHD.DesignationId= EMP.DesignationId;
              EHD.DepartmentId=EMP.DepartmentId;
                EHD.CreateId = intUserId;
                 EHD.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                 db.AddToEmployeeHistoryDetails(EHD);
                 db.SaveChanges();
            }
        catch(Exception e)
            {
                var Message = e.Message;
        }
            ModelState.Clear();
            DropList();
            return View("AddEmployee");
        }

        public ActionResult NIDCheck(string NID)
        {
            var result = true;
           // var user = db.EmployeeDetails.Where(x => x.NId == NID).FirstOrDefault();
            var user= (from b in db.EmployeeDetails where b.NId==NID && b.ActiveEmployee == true select b).FirstOrDefault();
            if (user != null)
            {
                var empLoanDetail = db.EmployeeLoanDetails.Where(y => y.EmployeeId == user.Id).FirstOrDefault();
                if (empLoanDetail.Status == "A" || empLoanDetail.Status == "L")
                {
                    result = false; 
                }

                if (empLoanDetail.Status == "LC")
                {
                    result = true;
                }
               
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ConfirmLastWorkingDt(string NID)
        {
            var result = (from a in db.EmployeeDetails where a.NId == NID select a.LastWorkingDate).Max();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult NIDDetails(string NID)
        {
   
            var B = (from ELD in db.EducationLoanDisbursments
                     join CD in db.ContactDetails on ELD.NId equals CD.NID
                     join ICM in db.IndividualCustomerMasters on ELD.NId equals ICM.Pan
                     where ELD.NId==NID
                     //group emp by ELD.NID into ELD
                     orderby CD.Address1 descending
                     select new EmployeeMaster
                     {
                     NID=ELD.NId,                 
                     MobileNo=ELD.StudentContactNo,
                     EmailId=ELD.StudentEmailId,
                     Status=ELD.AccountStatus,

                     CourseId=ELD.CourseId.Value,
                     CollegeId=ELD.CollegeId.Value,
                     LoanAccountNo=ELD.LoanAccountNo,
                     LoanAmount=ELD.LoanAmount.Value,
                     LoanAccountDate=ELD.LoanAccountDate.Value,
                     DisburseAmount=ELD.DisburseAmount.Value,
                     DisburseDate=ELD.DisburseDate.Value,

                     //TotalLoanAmount = ELD.Sum(b => b.LoanAmount.Value),
                     MaritalId=ICM.MaritalStatusId.Value,
                     TitleId1=ICM.TitleId.Value,
                     FirstName=ICM.FirstName,
                     MiddleName=ICM.MiddleName,
                     LastName=ICM.LastName,
                     Sex=ICM.Sex,
                     DateOfBirth=ICM.DateOfBirth.Value,
                     //lastWorkingDate=
                     Address1 = CD.Address1,
                     Address2 = CD.Address2,
                     Address3 = CD.Address3,
                     CountryId = CD.CountryId.Value,
                     StateId = CD.StateId.Value,
                     CityId = CD.CityId.Value,
                     Pincode = CD.Pincode
                     }).FirstOrDefault();

           
            if (B == null)
            {
                var b = "null";
                return Json(b, JsonRequestBehavior.AllowGet);
            }
          
            return Json(B, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CityList(string Stateid)
        {
            int id = Convert.ToInt32(Stateid);
            var citys = (from s in db.CityMasters
                         where s.StateId == id
                         select s);
            return Json(new SelectList(citys.ToArray(), "Id", "City"), JsonRequestBehavior.AllowGet);
        }
        public ActionResult CourseName(int CourseId)
        {
            var B = (from CM in db.CourseMasters
                     where CM.Id==CourseId
                     select new EmployeeMaster
                     {
                         CourseName=CM.CourseName
                     }).FirstOrDefault();
            return Json(B, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SalEMI(string Salary)
        {
            var Threshold = (from TM in db.EduThresholdMasters
                             orderby TM.Id descending
                             select TM.ThresholdValue).FirstOrDefault();
            decimal Sal = Convert.ToDecimal(Salary);
            //decimal BalanceSal = Sal - Convert.ToDecimal(Threshold);


            decimal BalanceSal = Sal - Threshold.Value;

            var B = (from SM in db.SlabMasters
                     where SM.RangeFrom <= Sal && SM.RangeTo >= Sal
                     select new EmployeeMaster
                     {
                     SlabId=SM.Id,
                     MonthlyEMI = (BalanceSal * (SM.SlabPercentage.Value) / 100)
                     }).FirstOrDefault();

            if (B == null)
            {
                string msg;
                if (BalanceSal > 0)
                {
                    msg = "Slab not defined for this Salary";
                }
                else
                {
                    msg = "Salary criteria not met";
                }
                 return Json(msg, JsonRequestBehavior.AllowGet);
            }
            return Json(B, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EmpEduGrid(string NID)
        {
            var B = (from ELD in db.EducationLoanDisbursments
                     join CLGM in db.CollegeMasters on ELD.CollegeId equals CLGM.Id
                     join CM in db.CourseMasters on ELD.CourseId equals CM.Id
                     where ELD.NId == NID
                     select new
                     {
                         ELD.NId,
                         ELD.Id,
                         ELD.StudentContactNo,
                         ELD.StudentEmailId,
                         CM.CourseName,
                         CLGM.CollegeName,
                         ELD.DisburseAmount,
                         ELD.DisburseDate,
                         ELD.LoanAccountDate,
                         ELD.LoanAccountNo,
                         ELD.LoanAmount,
                         ELD.BalanceAmount,
                         AccountStatus=ELD.AccountStatus.Equals("A") ? "Active" : ELD.AccountStatus.Equals("C") ? "Close" : ELD.AccountStatus.Equals("S") ? "Salary" : ELD.AccountStatus,
                         //AccountStatus = ELD.AccountStatus.Equals("A") ? "Active":ELD.AccountStatus.Equals("L") ? "Leave" : "Left Company",
                     }).ToList();

            return Json(B, JsonRequestBehavior.AllowGet);
        }
      
        [Authorize]
        public ActionResult ViewEmployee()
        { 
            int EmployerId = Convert.ToInt32(Session["EmployerName"]);
            int DivId = Convert.ToInt32(Session["EmployerDivision"]);
            int BranchId = Convert.ToInt32(Session["EmployerDivisionBranch"]);

            var emp = (from ED in db.EmployeeDetails
                       //join EM in db.EmployeeLoanDetails on ED.Id equals EM.EmployeeId
                       //join UD in db.User_Details on ED.EmployerId equals UD.ID
                       join Emp in db.EmployerMasters on ED.EmployerId equals Emp.Id 
                       join DEM in db.DesignationMasters on ED.DesignationId equals DEM.Id
                       join DM in db.DepartmentMasters on ED.DepartmentId equals DM.Id
                       where ED.EmployerId == EmployerId && ED.DivisionId == DivId && ED.EmployerBranchId == BranchId && ED.ActiveEmployee == true
                       orderby ED.Id descending
                       select new
                       {
                           ED.Id,
                           ED.Name,
                           DM.Department,
                           ED.NId,
                           ED.DateOfJoining,
                           DEM.Designation,
                           ED.Salary,
                           EmpolyerName = Emp.EmployerName
                           //ED.ContactNo,
                           //ED.EmailId
                       }
                   ).ToList();

            return View(emp);
        }
   
        [Authorize]
    public ActionResult EmployeeDetails(int id)
    {
       
        var totalDisAmount = (from ED in db.EmployeeDetails
                              join ELD1 in db.EmployeeLoanDetails on ED.Id equals ELD1.EmployeeId
                              join ELD in db.EducationLoanDisbursments on ELD1.EducationLoanDisburseId equals ELD.Id
                              where ED.Id == id && ELD.AccountStatus != "C" 
                           //group ED by new { ED.NId, ED.DisburseAmount, ED.BalanceAmount } into g
                               select ELD.DisburseAmount).Sum();

        var totalBalAmount = (from ED in db.EmployeeDetails
                              join ELD1 in db.EmployeeLoanDetails on ED.Id equals ELD1.EmployeeId
                              join ELD in db.EducationLoanDisbursments on ELD1.EducationLoanDisburseId equals ELD.Id
                              where ED.Id == id && ELD.AccountStatus != "C"
                           //group ED by new { ED.NId, ED.DisburseAmount, ED.BalanceAmount } into g
                           select ELD.BalanceAmount).Sum();
                               //a = ED.Sum(x => x.DisburseAmount.Value),
                               //b = g.Sum(x => x.BalanceAmount.Value)
                           //    a = ED.DisburseAmount,
                           //    b = ED.BalanceAmount
                           //}).Sum();
           
        var emp = (from ED in db.EmployeeDetails
                   join ELD1 in db.EmployeeLoanDetails on ED.Id equals ELD1.EmployeeId
                   join CD in db.ContactDetails on ED.NId equals CD.NID
                   join ELD in db.EducationLoanDisbursments on ELD1.EducationLoanDisburseId equals ELD.Id
                   join ICM in db.IndividualCustomerMasters on ED.NId equals ICM.Pan
                   join CM in db.CountryMasters on CD.CountryId equals CM.Id
                   join DEM in db.DesignationMasters on ED.DesignationId equals DEM.Id
                   join DM in db.DepartmentMasters on ED.DepartmentId equals DM.Id
                   join Emp in db.EmployerMasters on ED.EmployerId equals Emp.Id
                   join DIV in db.EmployerDivisionMasters on ED.DivisionId equals DIV.Id 
                   join BR in db.EmployerBranchMasters on ED.EmployerBranchId equals BR.Id 
                   //join UD in db.User_Details on ED.EmployerId equals UD.ID
                  // join TA in totalAmount on ELD.NId equals TA.
                   where ED.Id == id && ELD.AccountStatus != "C"
                   orderby CD.Address1 descending
                   select new EmployeeMaster
                   {
                       EmployerName = Emp.EmployerName,
                       EmpDivisionName=DIV.EmployerDivision,
                       EmployerBranchId=ED.EmployerBranchId.Value,
                       BranchName=BR.BranchName,
                       Id = ED.Id,
                       NID = ED.NId,
                       DepartmentId = ED.DepartmentId.Value,
                       CurrentDepartmentId = ED.CurrentDepartmentId.Value,
                       CurrentDesignationId = ED.CurrentDesignationId.Value,
                       DateOfJoining = ED.DateOfJoining.Value,
                       DesignationId = ED.DesignationId.Value,
                       Salary = ED.Salary.Value,
                       MobileNo = ED.ContactNo,
                       EmailId = ED.EmailId,
                       EmployeeID = ED.CompanyEmployeeId,
                       TotalLoanAmount=totalDisAmount.Value,
                       TotalBalanceAmount=totalBalAmount.Value,
                       MaritalId=ICM.MaritalStatusId.Value,
                      // TotalLoanAmount = g.Sum(x => x.DisburseAmount.Value),//ELD.DisburseAmount.Value,
                       //TotalBalanceAmount = ELD.BalanceAmount.Value,
                       lastWorkingDate = ED.LastWorkingDate ?? DateTime.Now,
                       SlabId = ELD1.SlabId.Value,
                       MonthlyEMI = ELD1.EMIAmount.Value,
                       SalEffectiveDate = ELD1.EffectiveDate.Value,
                       Remark = ELD1.Remark,
                       Status = ELD1.Status,

                       TitleId1 = ICM.TitleId.Value,
                       FirstName = ICM.FirstName,
                       MiddleName = ICM.MiddleName,
                       LastName = ICM.LastName,
                       Sex = ICM.Sex,
                       DateOfBirth = ICM.DateOfBirth.Value,

                       Address1 = CD.Address1,
                       Address2 = CD.Address2,
                       Address3 = CD.Address3,
                       CountryId = CD.CountryId.Value,
                       StateId = CD.StateId.Value,
                       CityId = CD.CityId.Value,
                       Pincode = CD.Pincode
                   }).FirstOrDefault();


           

            char delimiter = '-';
            string MobileNo = emp.MobileNo;
            string[] phrases = MobileNo.Split(delimiter);
            ViewBag.MobileCode = phrases[0];
            ViewBag.MobileNo = phrases[1];
            ViewBag.stateId = emp.StateId;
            string NID = emp.NID;
            ViewBag.Status = emp.Status;

          

            var B = (from ELD in db.EducationLoanDisbursments
                     join CLGM in db.CollegeMasters on ELD.CollegeId equals CLGM.Id
                     join CM in db.CourseMasters on ELD.CourseId equals CM.Id
                     where ELD.NId == NID && ELD.AccountStatus != "C"
                     select new
                     {
                         ELD.NId,
                         ELD.Id,
                         ELD.StudentContactNo,
                         ELD.StudentEmailId,
                         CM.CourseName,
                         CLGM.CollegeName,
                         ELD.DisburseAmount,
                         ELD.DisburseDate,
                         ELD.LoanAccountDate,
                         ELD.LoanAccountNo,
                         ELD.LoanAmount,
                         ELD.BalanceAmount,
                         ELD.AccountStatus
                     }).ToList();


            ViewBag.LoanList = B;
    
            DropList(); 
        return View(emp);
    }

        [Authorize]
    [HttpPost]
    public ActionResult EditEmployee(EmployeeMaster EMP)
    {
        int intUserId = Convert.ToInt32(Session["UserId"]);      
        try
        {
           

            var EM = db.EmployeeDetails.Where(x => x.Id == EMP.Id).FirstOrDefault();
            //EM.NId =EMP.NID1;
            EM.Name = EMP.FirstName + " " + EMP.MiddleName + " " + EMP.LastName;
            EM.CurrentDepartmentId = EMP.CurrentDepartmentId;
            EM.CurrentDesignationId = EMP.CurrentDesignationId;
            EM.Salary = EMP.Salary;
            EM.ContactNo = EMP.MobileCode + "-" + EMP.MobileNo;
            EM.EmailId = EMP.EmailId;
            EM.EditDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
            EM.EditId = intUserId;
            if (EMP.Status == "LC")
            {
                EM.LastWorkingDate = EMP.lastWorkingDate;
                var UserLogin = (from e in db.User_Details where e.USERNAME == EMP.NID && e.Active == true select e).FirstOrDefault();
                if (UserLogin != null)
                {
                    UserLogin.Active = false;
                    UserLogin.EditId = intUserId;
                    UserLogin.EditDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                    db.SaveChanges();
                }
                    //db.User_Details.Where(x => x.USERNAME == EMP.NID).FirstOrDefault();
            }
            db.SaveChanges();
           
          
          var ELD1 = db.EmployeeLoanDetails.Where(x => x.EmployeeId == EMP.Id).ToList();
       foreach(var item in ELD1)
          {
              var ELD = db.EmployeeLoanDetails.Where(x => x.Id ==item.Id).FirstOrDefault();
              ELD.SlabId = Convert.ToInt32(EMP.SlabId);
              ELD.EMIAmount = EMP.MonthlyEMI;
              //ELD.EffectiveDate = EMP.SalEffectiveDate;
              ELD.Remark = EMP.Remark;
              ELD.Status = EMP.Status;
              ELD.EditDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
              ELD.EditId = intUserId;
          }
            db.SaveChanges();

            var NID = (from ed in db.EmployeeDetails where ed.Id == EMP.Id select ed.NId).FirstOrDefault();
            var ICM = db.IndividualCustomerMasters.Where(x => x.Pan == NID).FirstOrDefault();
            ICM.FirstName = EMP.FirstName;
            ICM.MiddleName = EMP.MiddleName;
            ICM.LastName = EMP.LastName;
            ICM.EditId = intUserId;
            //ICM.MaritalStatusId = EMP.MaritalId;
            ICM.EditDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);

            var CD = db.ContactDetails.Where(x => x.NID == NID).OrderByDescending(x => x.Address1).FirstOrDefault();
            //CD.NID = EMP.NID1;
            CD.Address1 = EMP.Address1;
            CD.Address2 = EMP.Address2;
            CD.Address3 = EMP.Address3;
            CD.CityId = EMP.CityId;
            CD.StateId = EMP.StateId;
            CD.CountryId = EMP.CountryId;
            CD.Pincode = EMP.Pincode;
            CD.EditDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
            CD.EditId = intUserId;
            db.SaveChanges();

            var EmployerId = (from ed in db.EmployeeDetails where ed.Id == EMP.Id select ed.EmployerId).FirstOrDefault();

            var EmpDivisionName = (from ed in db.EmployeeDetails where ed.Id == EMP.Id select ed.DivisionId).FirstOrDefault();

            EmployeeHistoryDetail EHD = new EmployeeHistoryDetail();
            EHD.EmployeeId = EM.Id;
            EHD.EmployerId =  Convert.ToInt16(EmployerId);
            EHD.EmployerDivisionid = Convert.ToInt16(EmpDivisionName);
            EHD.EmployerBranchId = EMP.EmployerBranchId;
            EHD.NID = NID;
            EHD.CompanyEmployeeId = EMP.EmployeeID;
            EHD.DateOfJoining = EMP.DateOfJoining;
            EHD.SalaryEffectiveDate = EMP.SalEffectiveDate;
            EHD.Salary = EMP.Salary;
            EHD.SlabId = Convert.ToInt32(EMP.SlabId);
            EHD.EMIAmount = EMP.MonthlyEMI;
            EHD.DesignationId = EMP.CurrentDesignationId;
            EHD.DepartmentId = EMP.CurrentDepartmentId;
            EHD.EditId = intUserId;
            EHD.EditDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
            db.AddToEmployeeHistoryDetails(EHD);
            db.SaveChanges();



            return Content(("<script language='javascript' type='text/javascript'> alert('Data saved successfully!'); window.location.href ='/Employee/VIewEmployee';  </script>"));


        }
        catch (Exception e)
        {
            var a = e.Message;
        }
        return RedirectToAction("VIewEmployee");
    }
        public JsonResult doesEmployeeIDExist(string EmployeeID)
        {
            var result = true;
            int CompanyId = Convert.ToInt32(Session["EmployerName"]);
            int DivId = Convert.ToInt32(Session["EmployerDivision"]);
            var user = db.EmployeeDetails.Where(x => x.CompanyEmployeeId == EmployeeID && x.EmployerId == CompanyId && x.DivisionId == DivId).FirstOrDefault();
            if (user != null)
            {
                result = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
