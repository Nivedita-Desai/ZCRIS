using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using CreditRatingModel.Model;
using System.Data.Objects;

namespace CreditRatingSystem.Controllers
{
    public class EmployeeMonthlySalaryController : Controller
    {
        private creaditratingEntities db = new creaditratingEntities();

        public ActionResult Index()
        {
            return View();
        }

        public void GetFormInfo()
        {       
            if ( Request.QueryString["Name"]!=null)
            {
                TempData["strFormName"] = Request.QueryString["Name"];  //Form call from Sal Ded or App..    
                TempData.Keep("strFormName");
            }
                var strTempData = TempData["strFormName"] ;

            if (strTempData.ToString() == "SD")
            {
                ViewBag.strFrmCaption = "Salary Deduction";            //Salary Deduction
            }
            else if (strTempData.ToString() == "SDApp")
            {
                ViewBag.strFrmCaption = "Salary Deduction Approval";
            }
            else if (strTempData.ToString() == "CRS")
            {
                ViewBag.strFrmCaption = "Salary Deduction Approval - CRS";
            }
        }

        [Authorize]
        public ActionResult AddEmployeeMonthlySalary(int? Id, int? MID, string Name) 
        {
            if (Id==null)
            {
                Id = 0;                
            }
            if (Session["UserId"] ==null)
            {
                return Content("<script language='javascript' type='text/javascript'> ; window.location.href ='/Home/LogOut'; </script>");
            }
            ViewBag.Uid = Session["UserId"];
            ViewBag.EMName = Session["Username"];

            if (Session["EmployerName"] != null)
            {
                ViewBag.Employer_Id = Session["EmployerName"];
                ViewBag.Employer_Div_Id = Session["EmployerDivision"];
                ViewBag.Employer_Branch_Id = Session["EmployerDivisionBranch"];

                ViewBag.strEmployerName = GetEmployerInfo("E", ViewBag.Employer_Id);   //For Employer Name
                ViewBag.strEmployerDivName = GetEmployerInfo("D", ViewBag.Employer_Div_Id);   //For Employer Division Name
                ViewBag.strEmployerBrName = GetEmployerInfo("B", ViewBag.Employer_Branch_Id);   //For Employer Branch Name                
            }
            else 
            {
                var strSql = (db.spFillEmployerIdDetails(Id)).FirstOrDefault();

                if (strSql==null)
                {
                    return Content(("<script language='javascript' type='text/javascript'> alert('You are not authorised person to access this module'); window.location.href ='/Home/_Home'; </script>")); 
                }

                ViewBag.Employer_Id = strSql.EmployerId;
                ViewBag.Employer_Div_Id = strSql.EmpDivisionid;
                ViewBag.Employer_Branch_Id = strSql.EmpBranchId;

                ViewBag.strEmployerName = GetEmployerInfo("E", ViewBag.Employer_Id);   //For Employer Name
                ViewBag.strEmployerDivName = GetEmployerInfo("D", ViewBag.Employer_Div_Id);   //For Employer Division Name
                ViewBag.strEmployerBrName = GetEmployerInfo("B", ViewBag.Employer_Branch_Id);   //For Employer Branch Name

            }

            if (Id != 0)  //CompMonthlyDistributionHead Table Primary Key
            {
                ViewBag.CompMonDistId = Id;                
            }

            if (MID != 0)  //MonthlySalaryHead Table Primary Key
            {
                ViewBag.MonEmpSalId = MID;
            }

            GetFormInfo();
            return View();
        }

        public string GetEmployerInfo(string strType, int Id)
        {
            var strSql = (db.spFillEmployerDetails(strType,Id)).FirstOrDefault();

            return strSql.ToString();
        }

        private void FillAccName()
        {
            ViewBag.AccInfo = new SelectList(db.EmployerAccountMasters.ToList(), "Id", "AccountName");
        }

        public JsonResult FillData(string strType, int  ArId, decimal BId, decimal BrId, int DistTypeId)
        {
            var strSql = (db.spFillDistributionData(strType, ArId, BId, BrId, DistTypeId)).ToList();
           
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

        public JsonResult GetAccountInfo(int EmpAccId)
        {
            var strSql = (db.spGetAccountInformation(EmpAccId)).FirstOrDefault();
            
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

        public JsonResult GetEmpMonthlySalary(int user_id, int Employer_Id, int Employer_Type_Id, int EmployerBranch_Id, DateTime Salary_Month)
        {        
           var strSql = (db.spGetEmpMonthlySalary(user_id, Employer_Id, Employer_Type_Id, EmployerBranch_Id, Salary_Month)).ToList();
     
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

        public JsonResult GetDistEmpMonthlySalary(int user_id, int Employer_Id, int Employer_Type_Id, int EmployerBranch_Id, DateTime Salary_Month, int intid,string Type)
        {
            var strSql = (db.spDistributionEmpMonSalary(user_id, Employer_Id, Employer_Type_Id, EmployerBranch_Id, Salary_Month, intid, Type)).ToList(); //"A"
            
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

        public JsonResult GetCompMonthlySalary(int CompDistId)
        {
            var intDistType =Convert.ToInt32(Session["DistributionType"]);
            var strSql = (db.spGetCompMonthlySalary(CompDistId, intDistType)).ToList();

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

        public JsonResult CheckRecordExists(int UserId, int EmployerId, int EmployerTypeId, int EmployerBranchId, DateTime SalaryMonth)
        {
            var result = false;
            var strSql = (db.spCheckEmpMonSalExists(UserId, EmployerId, EmployerTypeId, EmployerBranchId, SalaryMonth)).FirstOrDefault();

            if (strSql > 0)
            {
                result = true;
            }            
            return Json(result, JsonRequestBehavior.AllowGet);                      
        }

        public JsonResult SaveApprovalData(string strAppData)   //SaveEmpMonSalData sd
        {
            //using (TransactionScope transaction = new TransactionScope())
            //{
            try
                {
                    var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    //string Data1 = sd.SaveAppSalData;
                    string Data1 = strAppData;

                    IEnumerable<EmployeeMonthlySalary> dist = serializer.Deserialize<IEnumerable<EmployeeMonthlySalary>>(Data1);
         
                    int intUserId = Convert.ToInt32(Session["UserId"]);

                    if (dist != null)
                    {
                        foreach (var i in dist)
                        {
                            var editSql = db.MonthlySalaryTransactions.Where(x => x.Id == i.Id).FirstOrDefault();
                            editSql.Status = "A";
                            editSql.EditDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                            editSql.EditId = intUserId;
                            db.SaveChanges();
                        }
                    }
                      //transaction.Complete();
                    return Json(new { msg = "Data Approved." }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    //transaction.Dispose();
                    return Json(new { msg = "Data Approved Failed." + ex.ToString() }, JsonRequestBehavior.AllowGet);
                }
        //}
        }

        public JsonResult SaveDistData(SaveEmpMonSalData sd)
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                    try
                    {
                        string AppData = sd.SaveAppSalData;
                        SaveApprovalData(AppData);

                        var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                        string HeadData = sd.SaveDistSalHeadData;
                        string ChildData = sd.SaveDistSalChildData;
                        
                        
                        IEnumerable<EmployeeMonthlySalary> Mst = serializer.Deserialize<IEnumerable<EmployeeMonthlySalary>>(HeadData);
                        IEnumerable<EmployeeMonthlySalary> det = serializer.Deserialize<IEnumerable<EmployeeMonthlySalary>>(ChildData);

                        int intUserId = Convert.ToInt32(Session["UserId"]);
                        
                        if (det != null)
                        {
                            CompMonthlyDistributionHead CMD = new CompMonthlyDistributionHead();

                            if (Mst != null)
                            {
                                foreach (var h in Mst)
                                {                                   
                                    CMD.EmployerId = h.EmployerId;
                                    CMD.EmpDivisionid= h.EmpDivisionid;
                                    CMD.EmpBranchId = h.EmpBranchId;
                                    CMD.DistMonth = h.DistMonth;
                                    CMD.Status = "P";

                                    CMD.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                                    CMD.CreateId = intUserId;

                                    db.AddToCompMonthlyDistributionHeads(CMD);
                                    db.SaveChanges();
                                }
                            }
                            foreach (var item in det)
                            {                         
                                if (item != null)
                                    {                                
                                        CompMonthlyDistributionChild CDC = new CompMonthlyDistributionChild();
                                        CDC.CompMondistId = CMD.Id;
                                        CDC.DistributionTypeId = item.DistTypeId;
                                        CDC.EmployerAccountId = item.EmpAccId;
                                        CDC.Percentage = item.Percentage;
                                        CDC.Amount = item.Amount;
                                        CDC.Status = "P";
                                        CDC.DistributionAccountId = item.AccId;
                                        //CDC.AreaId = item.AreaId;
                                        CDC.FinancialInstitutionId = item.BankId;
                                        CDC.FinancialInstitutionBranchId = item.BranchId ;
                                
                                        CDC.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                                        CDC.CreateId = intUserId;

                                        db.AddToCompMonthlyDistributionChilds(CDC);
                            }
                            }
                            db.SaveChanges();
                        }

                        transaction.Complete();
                        return Json(new { msg = "Data Saved Successfully." }, JsonRequestBehavior.AllowGet);
                    }
                    catch (Exception ex)
                    {
                        transaction.Dispose();
                        return Json(new { msg = "Data Saved Failed." + ex.ToString() }, JsonRequestBehavior.AllowGet);
                    }            
            }
        }
        
        public JsonResult SaveData(SaveEmpMonSalData sd)
        {
            using (TransactionScope transaction = new TransactionScope())
            {
            using (creaditratingEntities db = new creaditratingEntities())
            {
                try
                {
                    var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    string Data1 = sd.SaveMonSalData;
                    string HeadData1 = sd.SaveHeadSalData;
                    
                    IEnumerable<EmployeeMonthlySalary> det = serializer.Deserialize<IEnumerable<EmployeeMonthlySalary>>(Data1);
                    IEnumerable<EmployeeMonthlySalary> Mst = serializer.Deserialize<IEnumerable<EmployeeMonthlySalary>>(HeadData1);
                    
                    int intUserId = Convert.ToInt32(Session["UserId"]);

                    if (det != null)
                    {
                        MonthlySalaryHead MSH = new MonthlySalaryHead();
                        if (Mst != null)                        
                        {
                            foreach (var h in Mst)
                            {                                
                                MSH.UserId = h.UserId;
                                MSH.EmployerId = h.EmployerId;
                                MSH.EmployerTypeId = h.EmployerTypeId;
                                MSH.EmployerBranchId = h.EmployerBranchId;
                                MSH.SalaryMonth = h.SalaryMonth;

                                MSH.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                                MSH.CreateId = intUserId;

                                db.AddToMonthlySalaryHeads(MSH);
                                db.SaveChanges();
                            }
                        }
                        foreach (var item in det)
                        {   
                            MonthlySalaryTransaction MST = new MonthlySalaryTransaction();
                            MST.MonSalHeadId = MSH.Id;
                            MST.NID = item.NID;
                            MST.EmployeeId = item.EmployeeId;
                            MST.DepartmentId = item.DepartmentId;
                            MST.Salary = item.Salary;
                            MST.SlabId = item.SlabId;
                            MST.EMIAmount = item.EMIAmount;
                            MST.TotalLoanAmount = item.TotalLoanAmount;
                            MST.TotalBalanceAmount = item.TotalBalanceAmount;
                            MST.Status = "P";

                            MST.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                            MST.CreateId = intUserId;

                            db.AddToMonthlySalaryTransactions(MST);
                        }

                        db.SaveChanges();
                    }

                    transaction.Complete();              

                    return Json(new { msg = "Data Saved Successfully." }, JsonRequestBehavior.AllowGet);

                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    return Json(new { msg = "Data Saved Failed." + ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        }
        public JsonResult SaveCRSAppData(SaveEmpMonSalData sd) 
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                try
                {                    
                    var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    string CRSData = sd.SaveCRSAppData;
                    
                    IEnumerable<EmployeeMonthlySalary> CRSDet = serializer.Deserialize<IEnumerable<EmployeeMonthlySalary>>(CRSData);
                    
                    int intUserId = Convert.ToInt32(Session["UserId"]);                    
                    DateTime dtpCreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                                     
                    if (CRSDet != null)
                    {
                        foreach (var item in CRSDet)
                        { 
                             var strSql1 = (from CompMonthlyDistributionChild_ in db.CompMonthlyDistributionChilds
                                            where CompMonthlyDistributionChild_.DistributionTypeId == 1 && CompMonthlyDistributionChild_.CompMondistId == item.CompMonId
                                            select CompMonthlyDistributionChild_.Status).FirstOrDefault();

                             if (strSql1 == "P" && item.DistTypeId == 2)
                             {
                                 return Json(new { msg = "Kindly Submit Public Amount" }, JsonRequestBehavior.AllowGet);
                             }
                             else if (strSql1 == "P" && item.DistTypeId == 1)
                             {
                                 //var tblCompMonthlyDistributionChild = db.CompMonthlyDistributionChilds.Where(x => x.Id == item.CompMonDistChildId).FirstOrDefault();

                                 //try
                                 //{
                                 //    tblCompMonthlyDistributionChild.Status = "A";
                                 //    tblCompMonthlyDistributionChild.EditDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                                 //    tblCompMonthlyDistributionChild.EditId = intUserId;

                                 //    db.SaveChanges();                                     
                                 //}
                                 //catch (Exception ex)
                                 //{
                                 //    transaction.Dispose();
                                 //    return Json(new { msg = "Data Saved Failed." + ex.Message }, JsonRequestBehavior.AllowGet);
                                 //}
                             }
                             else
                             {
                                 //if (item.DistTypeId == 2)
                                 //{
                                 //    var strSql = (db.spApprovedfromCRS(item.NID, item.EMIAmount, item.CompMonId, intUserId, dtpCreateDate, item.CompMonDistChildId)).ToList();
                                 //}
                             }
                        }
                            
                            //  var strSql = (db.spApprovedfromCRS(item.NID, item.EMIAmount, item.CompMonId, intUserId, dtpCreateDate, item.CompMonDistChildId));
                            // string Per1 = "OutPara";
                            // ObjectParameter PerCol1 = new ObjectParameter(Per1, typeof(string));
                            //var strSql = (db.spApprovedfromCRS(item.NID, item.EMIAmount, item.CompMonId, intUserId, dtpCreateDate, item.CompMonDistChildId, PerCol1));
 //                            var strSql = (db.spApprovedfromCRS(item.NID, item.EMIAmount, item.CompMonId, intUserId, DateTime.UtcNow.AddHours(5).AddMinutes(30), item.CompMonDistChildId, PerCol1));

                        //}
                    }
                   
                    transaction.Complete();
                    return Json(new { msg = "Data Saved Successfully." }, JsonRequestBehavior.AllowGet);
                    //var msg = "Data Saved Successfully.";
                    //return Json(msg, JsonRequestBehavior.AllowGet);
                    //return new JsonResult
                    //{
                    //    Data = new { msg = "Data Save Sucessfully", Success = true },
                    //    ContentEncoding = System.Text.Encoding.UTF8,
                    //    JsonRequestBehavior = JsonRequestBehavior.DenyGet
                    //};
                }
                catch (Exception ex)
                {
                  transaction.Dispose();
                  return Json(new { msg = "Data Saved Failed." + ex.Message }, JsonRequestBehavior.AllowGet);

                  //var msg = "Data Saved Failed.";
                  //return Json(msg, JsonRequestBehavior.AllowGet);
                  //return new JsonResult
                  //{
                  //    Data = new { msg = "Data Saved Failed.", Success = true },
                  //    ContentEncoding = System.Text.Encoding.UTF8,
                  //    JsonRequestBehavior = JsonRequestBehavior.DenyGet
                  //};
                }
            }             
        }
        //********************From Nikita
        public ActionResult ViewCRSAdminApproval()
        {

            int Employer_Id=0, Employer_Div_Id=0, Employer_Branch_Id=0;
            if (Session["EmployerName"] != null)
            {
                Employer_Id = Convert.ToInt32(Session["EmployerName"]);
                Employer_Div_Id = Convert.ToInt32(Session["EmployerDivision"]);
                Employer_Branch_Id =Convert.ToInt32(Session["EmployerDivisionBranch"]);
            }

            string Per1 = "first";
            string Per2 = "Second";
            

            ObjectParameter PerCol1 = new ObjectParameter(Per1, typeof(string));
            ObjectParameter PerCol2 = new ObjectParameter(Per2, typeof(string));
            var UserType = Session["UserTypeId"];
            var decUserType = Convert.ToDecimal(UserType);
            ViewBag.UserType = Session["UserTypeId"];
            ////////////////////// from Aarti///////////////////Added Date Parameter in Store Procedure////////////
            DateTime TodayDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
            //var strSql = (db.sp_getpercentage(Employer_Id, Employer_Div_Id, Employer_Branch_Id, decUserType, PerCol1, PerCol2)); 
            var strSql = "";
            ////////////////////////////////////////////////////////

               
            ViewBag.PercentageCol1 = PerCol1.Value.ToString();
            ViewBag.PercentageCol2 = PerCol2.Value.ToString();


            var sqlquery = db.CountResults.ToList();

           
            if (strSql != null && sqlquery != null)
            {
                return View(sqlquery);
            }
            else
            {
                return Content(("<script language='javascript' type='text/javascript'> alert('No Record Found!'); window.location.href ='/Home/_Home'; </script>"));
            }

           
        }

    }
}
