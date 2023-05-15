using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CreditRatingModel.Model;
using System.Transactions;
using System.Text.RegularExpressions;

namespace CreditRating.Controllers
{
    public class LoanDisburseController : Controller
    {
        //
        // GET: /LoanDisburseTransaction/
        private creaditratingEntities db = new creaditratingEntities();
        public string n;        
        public string AutoGenrateNumber;


        [HttpGet]
        public ActionResult AddLoanDisburse(string intid = "")
        {
            int F_Id = Convert.ToInt32(Session["EmployerName"]);
            if (F_Id == 0)
            {
                return Content(("<script language='javascript' type='text/javascript'> alert('You are not authorised person to access this module') ; window.location.href ='/Home/_Home'; </script>"));
                //return View("_Home");
            }
            //if (intid == "")
            //{
                int userTypeId = Convert.ToInt32(Session["UserTypeId"]);
                if (userTypeId == 2)
                {
                    int FI_ID = Convert.ToInt32(Session["EmployerName"]);
                    int FI_Br_ID = Convert.ToInt32(Session["EmployerDivision"]);

                    var FI_Name = (from F in db.FinancialInstitutionMasters
                                   where F.Id == FI_ID
                                   select F.Name
                                   ).FirstOrDefault();

                    var FI_BR_Name = (from B in db.FinancialInstitutionBranchMasters
                                      where B.Id == FI_Br_ID
                                      select B.BranchName
                                   ).FirstOrDefault();

                    ViewBag.FinancialInstituteName = FI_Name;
                    ViewBag.FinancialInstituteBranchName = FI_BR_Name;
                    ViewBag.accno = intid;
                }
            //}
            //else
            //{
               

            //}
        
            dropList();
            return View();
        }

        [HttpPost]
        public ActionResult AddLoanDisburse(LoanDisburseTransaction_Metadata objLoanDisburseTransaction_Metadata)
        {
            string msg;
            using (TransactionScope transaction = new TransactionScope())
            {
                try
                {
                    addLoanDisburseDetails(objLoanDisburseTransaction_Metadata);
                    transaction.Complete();
                    msg = "Data saved successfully!";
                }
                catch (Exception)
                {
                    transaction.Dispose();
                    msg = "Data saved Failed!";
                }
            }

            return Content(("<script language='javascript' type='text/javascript'> alert('" + msg + "'); window.location.href ='/LoanDisburse/LoanApplicationApproveList';  </script>"));
            
        }

        [HttpGet]
        public ActionResult AddLoanDisburseInstallment()
        {
            int F_Id = Convert.ToInt32(Session["EmployerName"]);
            if (F_Id == 0)
            {
                return Content(("<script language='javascript' type='text/javascript'> alert('You are not authorised person to access this module') ; window.location.href ='/Home/_Home'; </script>"));
                //return View("_Home");
            }
            dropList();
            return View();
        }
        [HttpPost]
        public ActionResult AddLoanDisburseInstallment(LoanDisburseTransaction_Metadata objLoanDisburseTransaction_Metadata)
        {
            string msg;
            using (TransactionScope transaction = new TransactionScope())
            {
                try
                {
                    addLoanDisburseInstallmentDetails(objLoanDisburseTransaction_Metadata);
                    transaction.Complete();
                    msg = "Data saved successfully!";
                }
                catch (Exception)
                {
                    transaction.Dispose();
                    msg = "Data saved Failed!";
                }
            }

            return Content(("<script language='javascript' type='text/javascript'> alert('" + msg + "'); window.location.href ='/LoanDisburse/AddLoanDisburseInstallment';  </script>"));
            
        }

        [HttpGet]
        public ActionResult PaidInstallments(string loanacno = "")
        {
            int F_Id = Convert.ToInt32(Session["EmployerName"]);
            if (F_Id == 0)
            {
                return Content(("<script language='javascript' type='text/javascript'> alert('You are not authorised person to access this module') ; window.location.href ='/Home/_Home'; </script>"));
                //return View("_Home");
            }
            dropList();
            ViewBag.loanacno = loanacno;
            //getUnpaidInstDetails(loanacno);
            return View();
        }

        [HttpPost]
        public ActionResult PaidInstallments(LoanDisburseTransaction_Metadata objLoanDisburseTransaction_Metadata)
        {
            string msg;
            using (TransactionScope transaction = new TransactionScope())
            {
                try
                {
                    UpdateUnpaidInst(objLoanDisburseTransaction_Metadata);
                    transaction.Complete();
                    msg = "Data saved successfully!";
                }
                catch (Exception)
                {
                    transaction.Dispose();
                    msg = "Data saved Failed!";
                }
            }

            return Content(("<script language='javascript' type='text/javascript'> alert('" + msg + "'); window.location.href ='/LoanDisburse/LoanDisburseUnpaidList';  </script>"));
            
        }

        public ActionResult getUnpaidInstDetails(string LoanAccNO)
        {
            //var Result = (from a in db.SP_GetUnpaidInstallmentDetails(LoanAccNO) select new LoanDisburseTransaction_Metadata 
            //{ 
            //    LoanAccNo=a.LoanAccountNo,
            //    PaidAmount=a.TotalPaidInst,
            //    BalanceAmount=a.TotalBalInst.Value,
            //    InstCounter=a.InstCounter,
            //    InstDate=a.InstDate,
            //    InstAmount=a.InstAmt,
            //    DelayedDays=a.DelayDays.Value,
            //    Id=a.Id
            //}).FirstOrDefault();
            var Result = (from a in db.SP_GetUnpaidInstallmentDetails(LoanAccNO)
                          select a).FirstOrDefault();
            return Json(Result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult UpdateUnpaidInst(LoanDisburseTransaction_Metadata objLoanDisburseTransaction_Metadata)
        {
            int EditUserId = Convert.ToInt32(Session["UserId"]);
            string selectedBranch = Request.Form["selBranch"];
            decimal selectedBranchId = Convert.ToInt32(selectedBranch);
            var objLoanInst = db.LoanDisbursedInstallments.Where(x => x.Id == objLoanDisburseTransaction_Metadata.Id).FirstOrDefault();
            objLoanInst.PaidStatus = true;
            objLoanInst.PaidDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
            objLoanInst.PaymentModeId = objLoanDisburseTransaction_Metadata.PaymentModeId;
            if (objLoanDisburseTransaction_Metadata.PaymentModeId == 1)
            {
                objLoanInst.CHQ_No = objLoanDisburseTransaction_Metadata.ChqNo;
                objLoanInst.CHQ_Date = objLoanDisburseTransaction_Metadata.ChqDate;
                objLoanInst.BankName = objLoanDisburseTransaction_Metadata.BankId;
                objLoanInst.BranchName = selectedBranchId;
            }
            if (objLoanDisburseTransaction_Metadata.PaymentModeId == 2)
            {
                //objLoanInst.CHQ_No = objLoanDisburseTransaction_Metadata.TransactionId;
                objLoanInst.CHQ_No = objLoanInst.TransactionId;
                objLoanInst.CHQ_Date = DateTime.UtcNow.AddHours(5).AddMinutes(30);
            }
            objLoanInst.DelayedDays = objLoanDisburseTransaction_Metadata.DelayedDays;
            objLoanInst.LatePayChargedAmt = objLoanDisburseTransaction_Metadata.LatePayCharges;
            objLoanInst.InterestAmt = objLoanDisburseTransaction_Metadata.InterestAmt;
            objLoanInst.BounceCharge = objLoanDisburseTransaction_Metadata.BounceCharges;
            objLoanInst.TotalAmt = objLoanDisburseTransaction_Metadata.TotalAmount;
            objLoanInst.EditId = EditUserId;
            objLoanInst.EditDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
            db.SaveChanges();
            return View();
        }

        public ActionResult addLoanDisburseInstallmentDetails(LoanDisburseTransaction_Metadata objLoanDisburseTransaction_Metadata)
        {
            LoanDisbursedInstallment objLoanDisburseInstallment = new LoanDisbursedInstallment();
            string selectedBranch = Request.Form["selBranch"];
            decimal selectedBranchId =  Convert.ToInt32(selectedBranch);
            int CreateUserId =  Convert.ToInt32(Session["UserId"]);
            objLoanDisburseInstallment.InstCounter = objLoanDisburseTransaction_Metadata.InstCounter;
            objLoanDisburseInstallment.InstAmt = objLoanDisburseTransaction_Metadata.InstAmount;
            objLoanDisburseInstallment.InstDate = objLoanDisburseTransaction_Metadata.InstDate;
            objLoanDisburseInstallment.LoanDisbursedId = objLoanDisburseTransaction_Metadata.LoanDisbursedId;
            objLoanDisburseInstallment.PaidStatus = true;
            objLoanDisburseInstallment.PaidDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
            objLoanDisburseInstallment.PaymentModeId = objLoanDisburseTransaction_Metadata.PaymentModeId;

            if (objLoanDisburseTransaction_Metadata.PaymentModeId == 1)
            {
                objLoanDisburseInstallment.CHQ_No = objLoanDisburseTransaction_Metadata.ChqNo;
                objLoanDisburseInstallment.CHQ_Date = objLoanDisburseTransaction_Metadata.ChqDate;
                objLoanDisburseInstallment.BankName = objLoanDisburseTransaction_Metadata.BankId;
                objLoanDisburseInstallment.BranchName = selectedBranchId;
            }
            
           
            objLoanDisburseInstallment.DelayedDays = objLoanDisburseTransaction_Metadata.DelayedDays;
            objLoanDisburseInstallment.LatePayChargedAmt = objLoanDisburseTransaction_Metadata.LatePayCharges;
            objLoanDisburseInstallment.InterestAmt = objLoanDisburseTransaction_Metadata.InterestAmt;
            objLoanDisburseInstallment.BounceCharge = objLoanDisburseTransaction_Metadata.BounceCharges;
            objLoanDisburseInstallment.TotalAmt = objLoanDisburseTransaction_Metadata.TotalAmount;
            objLoanDisburseTransaction_Metadata.TransactionId = GenerateTransactionNo();
            objLoanDisburseInstallment.TransactionId = objLoanDisburseTransaction_Metadata.TransactionId;
            if (objLoanDisburseTransaction_Metadata.PaymentModeId == 2)
            {
                objLoanDisburseInstallment.CHQ_No = objLoanDisburseTransaction_Metadata.TransactionId;
                objLoanDisburseInstallment.CHQ_Date = DateTime.UtcNow.AddHours(5).AddMinutes(30);
            }
            objLoanDisburseInstallment.CreateId = CreateUserId;
            objLoanDisburseInstallment.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
            
            var CheckDouble = db.LoanDisbursedInstallments.Where(x => x.LoanDisbursedId == objLoanDisburseTransaction_Metadata.LoanDisbursedId && x.InstCounter==objLoanDisburseTransaction_Metadata.InstCounter).FirstOrDefault();
            if (CheckDouble == null)
            {
                db.AddToLoanDisbursedInstallments(objLoanDisburseInstallment);
                db.SaveChanges();
            }
            

            if ((objLoanDisburseTransaction_Metadata.BalanceAmount - objLoanDisburseTransaction_Metadata.InstAmount) <= 0)
            {
                var EL = db.LoanDisburseTransactions.Where(x => x.Id == objLoanDisburseTransaction_Metadata.LoanDisbursedId).FirstOrDefault();
                EL.AccountStatus = "C";
                EL.ClosingDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                db.SaveChanges();
            }
            return View("AddLoanDisburseInstallment");
        }
        public string GenerateTransactionNo()
        {
            string Day = Convert.ToString(DateTime.UtcNow.Day);

            if (Day.Length == 1)
            {
                Day = 0 + Day;
            }

            string Month = Convert.ToString(DateTime.UtcNow.Month);
            if (Month.Length == 1)
            {
                Month = 0 + Month;
            }
            int Year = DateTime.UtcNow.Year;
            string d = Day + "" + Month + "" + Year;

            AutoGenrateNumber = "TR" + d + "00001";

        b:
            var c = (from s in db.LoanDisbursedInstallments
                     where s.TransactionId == AutoGenrateNumber
                     select s.TransactionId).FirstOrDefault();

            if (c == null)
            {
                // AutoGenrateNumber = AutoGenrateNumber;
            }
            else
            {
                var text = String.Format(AutoGenrateNumber.Substring(AutoGenrateNumber.Length - 5));
                int x = Convert.ToInt32(text);
                int x1 = x + 1;
                string t = x1.ToString();

                if (x1 > 9999)
                    n = t;
                else if (x1 > 999)
                    n = "0" + x1;
                else if (x1 > 99)
                    n = "00" + x1;
                else if (x1 > 9)
                    n = "000" + x1;
                else
                    n = "0000" + x1;


                AutoGenrateNumber = "TR" + d + n;

                goto b;
            }

            return AutoGenrateNumber;
        }
        public ActionResult AddLoanDisburseold()
        {
            dropList();
            return View();
        }
        public ActionResult addLoanDisburseDetails(LoanDisburseTransaction_Metadata objLoanDisburseTransaction_Metadata)
        {
            int FI_ID = Convert.ToInt32(Session["EmployerName"]);
            int FI_Br_ID = Convert.ToInt32(Session["EmployerDivision"]);
            int intUserId = Convert.ToInt32(Session["UserId"]);
            var strLoanNo = db.LoanApplicationTransactions.Where(x => x.LoanApplicationNo == objLoanDisburseTransaction_Metadata.LoanNo && x.FinancialInstitutionId == FI_ID && x.FinancialInstitutionBranchId == FI_Br_ID && x.LoanStatus == "A").FirstOrDefault();
            LoanDisburseTransaction objLDisTran = new LoanDisburseTransaction();

            objLDisTran.FinancialInstitutionId = FI_ID;
            objLDisTran.FinancialInstitutionBranchId = FI_Br_ID;
           
            objLDisTran.LoanApplicationId = strLoanNo.Id;
            
            
            objLDisTran.LoanDisburseDate = objLoanDisburseTransaction_Metadata.LoanDisburseDate;
            
            
            if (objLoanDisburseTransaction_Metadata.Category == "L")
            {
                objLDisTran.DisburseAmount = objLoanDisburseTransaction_Metadata.DisburseAmount;
                objLDisTran.LoanAccountNo = objLoanDisburseTransaction_Metadata.LoanAccNo;
                objLDisTran.LoanAccountDate = objLoanDisburseTransaction_Metadata.LoanAccCreateDate;
                objLDisTran.FirstInstallment = objLoanDisburseTransaction_Metadata.FirstInstallment;
                objLDisTran.FirstInstallmentDate = objLoanDisburseTransaction_Metadata.FirstInstallmentDate;
                objLDisTran.Tenure = objLoanDisburseTransaction_Metadata.NoOfMonths;
                objLDisTran.LastInstallmentDate = objLoanDisburseTransaction_Metadata.LastInstallmentDate;
                objLDisTran.InterestTypeId = objLoanDisburseTransaction_Metadata.InterestTypeId;
                objLDisTran.RateOfInterest = objLoanDisburseTransaction_Metadata.RateOfInterest;
                if (objLoanDisburseTransaction_Metadata.gracePeriod == 0)
                {
                    //objLDisTran.InstallmentDueDate = objLoanDisburseTransaction_Metadata.InstallmentDueDate;
                }
                else
                {
                    objLDisTran.InstallmentDueDate = objLoanDisburseTransaction_Metadata.InstallmentDueDate;
                }
                objLDisTran.GracePeriod = objLoanDisburseTransaction_Metadata.gracePeriod;
                objLDisTran.Downpayment = objLoanDisburseTransaction_Metadata.Downpayment;
            }

            if (objLoanDisburseTransaction_Metadata.Category == "C")
            {
                //objLDisTran.LoanAccountNo = objLoanDisburseTransaction_Metadata.CRAccNo1 + objLoanDisburseTransaction_Metadata.CRAccNo2 + objLoanDisburseTransaction_Metadata.CRAccNo3 + objLoanDisburseTransaction_Metadata.CRAccNo4;

                objLDisTran.LoanAccountNo =  Regex.Replace(objLoanDisburseTransaction_Metadata.CRAccNo," ","");
                objLDisTran.IssueDate = objLoanDisburseTransaction_Metadata.CreditCardIssueDate;
                objLDisTran.CreditCardExpiryDate = objLoanDisburseTransaction_Metadata.CreditCardExpiryDate;
                objLDisTran.CreditLimit = objLoanDisburseTransaction_Metadata.CreditLimit;
                objLDisTran.CashLimit = objLoanDisburseTransaction_Metadata.CashLimit;
                objLDisTran.BillingCycleStartDate = objLoanDisburseTransaction_Metadata.BilCycleStartDt;
                //objLDisTran.BillingCycleEndDate = objLoanDisburseTransaction_Metadata.BilCycleEndDt;
            }

            objLDisTran.AccountStatus = "A";
           
            objLDisTran.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
            objLDisTran.CreateId = intUserId;
            db.AddToLoanDisburseTransactions(objLDisTran);
            db.SaveChanges();
            return View("AddLoanDisburse");
        }

        [HttpPost]
        public ActionResult Index(LoanDisburseTransaction_Metadata LDT)
        {
            //if (ModelState.IsValid)
            //{
               // btnAddClick(LDT);
            //}
            dropList();
          //  return View(LDT);
            return Content(("<script language='javascript' type='text/javascript'> alert('Data saved successfully!'); window.location.href ='/LoanDisburse/AddLoanDisburse'; </script>"));

        }

        public void dropList()
        {
            ViewBag.InterestTypeLst = new SelectList(db.InterestTypeMasters.ToList(), "Id", "InterestType");
            ViewBag.LoanTypeLst = new SelectList(db.LoanTypesMasters.ToList(), "Id", "Type");
            ViewBag.PaymentModeList = new SelectList(db.PaymentModes.ToList(), "Id", "PaymentType");
            ViewBag.BankList = new SelectList((from FIM in db.FinancialInstitutionMasters
                                               where FIM.FinancialInstituteCategoryId == 1
                                               orderby FIM.Name
                                               select FIM).ToList(), "Id", "Name");
            ViewBag.BranchList = new SelectList((from FIB in db.FinancialInstitutionBranchMasters select FIB).ToList(), "Id", "BranchName");
        }

        public JsonResult BranchList(string BankId)
        {
            int Bankid = Convert.ToInt32(BankId);
            var Branch = (from FIBM in db.FinancialInstitutionBranchMasters
                          join FIM in db.FinancialInstitutionMasters on FIBM.FinancialInstituteId equals FIM.Id
                          where FIM.Id == Bankid
                          orderby FIBM.BranchName
                          select new { FIBM.Id, FIBM.BranchName }).ToList();


            return Json(new SelectList(Branch.ToArray(), "Id", "BranchName"), JsonRequestBehavior.AllowGet);
        }
        public JsonResult doesLoanNoExist(string LoanNo)
        {
            var result = true;
            int FI_ID = Convert.ToInt32(Session["EmployerName"]);
            int FI_Br_ID = Convert.ToInt32(Session["EmployerDivision"]);

            var user = db.LoanApplicationTransactions.Where(x => x.LoanApplicationNo == LoanNo && x.FinancialInstitutionId == FI_ID && x.FinancialInstitutionBranchId == FI_Br_ID ).FirstOrDefault();
            if (user != null)
            {
                    result = false;
            }
            return Json(result,JsonRequestBehavior.AllowGet);
        }
        public JsonResult CheckLoanAccountNoExists(string ParamLoanNo)
        {
            var result = true;
            int FI_ID = Convert.ToInt32(Session["EmployerName"]);
            int FI_Br_ID = Convert.ToInt32(Session["EmployerDivision"]);

            var user = db.LoanDisburseTransactions.Where(x => x.LoanAccountNo == ParamLoanNo && x.FinancialInstitutionId == FI_ID && x.FinancialInstitutionBranchId == FI_Br_ID).FirstOrDefault();
            if (user != null)
            {
                result = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckLoanNoExists(string LoanNo)
        {
            var result = false;
            int FI_ID = Convert.ToInt32(Session["EmployerName"]);
            int FI_Br_ID = Convert.ToInt32(Session["EmployerDivision"]);

            var strLoanNo = db.LoanApplicationTransactions.Where(x => x.LoanApplicationNo == LoanNo && x.FinancialInstitutionId == FI_ID && x.FinancialInstitutionBranchId == FI_Br_ID).FirstOrDefault();
            if (strLoanNo != null)
            {
                var strLoanID = db.LoanDisburseTransactions.Where(x => x.LoanApplicationId == strLoanNo.Id).FirstOrDefault();
                if (strLoanID != null)
                {
                    result = true;
                }  
            }
            
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RetrieveLoanDetails(string strLoanNo)
        {
            
            //DateTime LoanDate;
            //string PanNo;

            //var LoanNoExist = db.LoanApplicationTransactions.Where(LT => LT.LoanNo.Equals(strLoanNo)).FirstOrDefault();
            //if (LoanNoExist != null)
            //{
            //    LoanDate = Convert.ToDateTime(LoanNoExist.LoanDate);
            //}

            int FI_ID = Convert.ToInt32(Session["EmployerName"]);
            int FI_Br_ID = Convert.ToInt32(Session["EmployerDivision"]);

            //var result = (from LT in db.LoanApplicationTransactions
            //              join LIR in db.LoanIndividualRelations on LT.Id equals LIR.LoanApplicationId
            //              join Ind in db.IndividualCustomerMasters on LIR.IndividualCustomerId equals Ind.Id
            //              join LTM in db.LoanTypesMasters on LT.LoanTypesId equals LTM.Id
            //              join CTM in db.CardTypeMasters on LT.CardTypeId equals CTM.Id
            //              where LT.LoanApplicationNo == strLoanNo && LT.FinancialInstitutionId==FI_ID && LT.FinancialInstitutionBranchId==FI_Br_ID  && LT.LoanStatus=="A"
            //              select new LoanDisburseTransaction_Metadata
            //              {
            //                  LoanDate = LT.LoanApplicationDate.Value,
            //                  Pancard = LT.Pan,
            //                  LoanType = LTM.Type,
            //                  Category = LTM.Category,
            //                  DisburseAmount=LT.LoanAmount.Value,
            //                  CardType = CTM.CardType,
            //                  CreditLimit = CTM.CreditLimit.Value,
            //                  CashLimit = CTM.CashLimit.Value  
            //              }).FirstOrDefault();
            var result=(from LT in db.sp_GetLoanDisburseData(strLoanNo,FI_ID,FI_Br_ID,"A")
                        select new LoanDisburseTransaction_Metadata
                          {
                              LoanDate = LT.LoanApplicationDate.Value,
                              Pancard = LT.Pan,
                              LoanType = LT.Type,
                              Category = LT.Category,
                              DisburseAmount = LT.LoanAmount.Value,
                              CardType = LT.CardType,
                              CreditLimit = LT.CreditLimit,
                              CashLimit = LT.CashLimit
                          }).FirstOrDefault();


            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RetrieveLoanPaymentDetails(string strLoanAccountNo)
        {
            int FI_ID =  Convert.ToInt32(Session["EmployerName"]);
            int FI_Br_ID = Convert.ToInt32(Session["EmployerDivision"]);
            var result = (from LPD in db.LoanPaymentDetails(FI_ID, FI_Br_ID, Regex.Replace(strLoanAccountNo," ","")) select LPD).FirstOrDefault();
            //var result = (from LDT in db.LoanDisburseTransactions
            //              where LDT.LoanAccountNo == strLoanAccountNo && LDT.AccountStatus == "A" && LDT.FinancialInstitutionId == FI_ID && LDT.FinancialInstitutionBranchId == FI_Br_ID
            //              select new LoanDisburseTransaction_Metadata 
            //              { 
            //                 LoanDisbursedId = LDT.Id,
            //                 DisburseAmount = LDT.DisburseAmount.Value,
            //              }).FirstOrDefault();
            ////if (result != null)
            ////{
            ////    var SumPaidInst = (from LDI in db.LoanDisbursedInstallments
            ////                       where LDI.LoanDisbursedId == result.Id
            ////                       select (decimal?)LDI.InstAmt).Sum() ?? 0;

            ////    //var SumPaidInst = (from LDI in db.LoanDisbursedInstallments
            ////    //                   where LDI.LoanDisbursedId == result.Id
            ////    //                   select new LoanDisburseTransaction_Metadata
            ////    //                       {
            ////    //                           PaidAmount=LDI.InstAmt
            ////    //                       }).Sum(x=>x.PaidAmount);
               
            ////    TempData["InstAmt"] = SumPaidInst;
            ////    TempData["BalAmt"] = result.DisburseAmount - SumPaidInst;
            ////    //ViewBag.InstAmt = SumPaidInst;
            ////    //ViewBag.BalAmt = result.DisburseAmount;
            ////}
            return Json(result,JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckForInstalmentLoanAcNo(string strLoanAccountNo)
        {
            var result = false;
            result = false;
            int FI_ID = Convert.ToInt32(Session["EmployerName"]);
            int FI_Br_ID = Convert.ToInt32(Session["EmployerDivision"]);

            var LoanDetails = (from LDT in db.LoanDisburseTransactions
                          where LDT.LoanAccountNo == strLoanAccountNo && LDT.AccountStatus == "A" && LDT.FinancialInstitutionId == FI_ID && LDT.FinancialInstitutionBranchId == FI_Br_ID
                          select LDT).FirstOrDefault();
            if (LoanDetails != null)
            {
                result = true;
            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }
       
        //----------------------Darshana------------------
        public ActionResult ViewLoanDisbursed()
        { //join ICM in db.IndividualCustomerMasters on LDT.IndividualCustomerId equals ICM.Id
            var strSql = (from LDT in db.LoanDisburseTransactions
                          join LAT in db.LoanApplicationTransactions on LDT.LoanApplicationId equals LAT.Id
                          join LTM in db.LoanTypesMasters on LAT.LoanTypesId equals LTM.Id
                          join ITM in db.InterestTypeMasters on LDT.InterestTypeId equals ITM.Id
                          where LTM.Category == "L"
                          select new
                          {
                              LDT.Id,
                              //ICM.Pan,
                              LDT.CashLimit,
                              LDT.CreditCardExpiryDate,
                              LDT.CreditLimit,
                              LDT.DisburseAmount,
                              //LDT.EMI,
                              LDT.ExpiryDate,
                              LDT.FirstInstallment,
                              LDT.LoanAccountDate,
                              //LDT.LoanAccountNo1,
                              //LDT.LoanAccountNo2,
                              //LDT.LoanAccountNo3,
                              //LDT.LoanAccountNo4,
                              LDT.LoanDisburseDate,
                              //LDT.LoanApplicationNo,
                              //LDT.LoanApplicationDate,
                              //LDT.NoOfMonths,
                              LDT.RateOfInterest,
                              ITM.InterestType,
                              LTM.Type,
                              LTM.Category,
                              //LTM.Pan
                          }
                            ).ToList();
            var L = (from LDT in db.LoanDisburseTransactions
                     //join ICM in db.IndividualCustomerMasters on LDT.IndividualCustomerId equals ICM.Id
                     join LAT in db.LoanApplicationTransactions on LDT.LoanApplicationId equals LAT.Id
                     join LTM in db.LoanTypesMasters on LAT.LoanTypesId equals LTM.Id                     
                     join ITM in db.InterestTypeMasters on LDT.InterestTypeId equals ITM.Id
                     where LAT.LoanTypesId == 'C'
                     select new
                     {
                         LDT.Id,
                         LAT.Pan,
                         LDT.CashLimit,
                         LDT.CreditCardExpiryDate,
                         LDT.CreditLimit,
                         LDT.DisburseAmount,
                         LDT.Downpayment,
                         LDT.ExpiryDate,
                         LDT.FirstInstallment,
                         LDT.LoanAccountDate,
                         //LDT.LoanAccountNo1,
                         //LDT.LoanAccountNo2,
                         //LDT.LoanAccountNo3,
                         //LDT.LoanAccountNo4,
                         LDT.LoanDisburseDate,
                         //LDT.LoanApplicationNo,
                         //LDT.NoOfMonths,
                         LDT.RateOfInterest,                         
                         ITM.InterestType,
                         LTM.Type,
                         LTM.Category
                     }).ToList();
            ViewBag.List = L;
            return View(strSql);
        }
        //------------------------------------------------

        //-------------------------Rutuja Edit Part----------------------------

        public ActionResult EditLoanDisburse(int intId,string strCategory)
        {
            LoanDisburseTransaction_Metadata LDT_MD = new LoanDisburseTransaction_Metadata();
            dropList();

            if (strCategory == "L")
            {
                var strSql = (from LDT in db.LoanDisburseTransactions
                              join LAT in db.LoanApplicationTransactions on LDT.LoanApplicationId equals LAT.Id
                              join LTM in db.LoanTypesMasters on LAT.LoanTypesId equals LTM.Id
                              where LDT.Id == intId && LTM.Category == "L"
                              select new LoanDisburseTransaction_Metadata
                              {
                                  Id = LDT.Id,
                                  LoanNo = LAT.LoanApplicationNo,
                                  LoanDate = LAT.LoanApplicationDate.Value,
                                  Pancard = LAT.Pan,
                                  LoanType = LTM.Type,
                                  LoanTypeId = LAT.LoanTypesId.Value,
                                  Category = LTM.Category,

                                  LoanDisburseDate = LDT.LoanDisburseDate.Value,
                                  DisburseAmount = LDT.DisburseAmount.Value,
                                  FirstInstallment = LDT.FirstInstallment.Value,
                                  NoOfMonths = LDT.Tenure.Value,
                                  ExpiryDate = LDT.ExpiryDate.Value,
                                  EMI = LDT.Downpayment.Value,
                                  RateOfInterest = LDT.RateOfInterest.Value,
                                  InterestTypeId = LDT.InterestTypeId.Value,
                                  LoanAccCreateDate = LDT.LoanAccountDate.Value,
                                  FirstInstallmentDate = LDT.FirstInstallmentDate.Value,

                                  LoanAccNo = LDT.LoanAccountNo,    //LoanAccountNo
                              }).FirstOrDefault();

                return View(strSql);
            }
            else
            {
                var strSql = (from LDT in db.LoanDisburseTransactions
                               join LAT in db.LoanApplicationTransactions on LDT.LoanApplicationId equals LAT.Id
                               join LTM in db.LoanTypesMasters on LAT.LoanTypesId equals LTM.Id
                               where LDT.Id == intId && LTM.Category == "C"
                               select new LoanDisburseTransaction_Metadata
                               {
                                   Id = LDT.Id,                                   
                                   LoanNo = LAT.LoanApplicationNo,
                                   LoanDate = LAT.LoanApplicationDate.Value,
                                   Pancard = LAT.Pan,
                                   LoanType = LTM.Type,
                                   LoanTypeId = LAT.LoanTypesId.Value,
                                   Category = LTM.Category,
                                   LoanDisburseDate = LDT.LoanDisburseDate.Value,
                                   DisburseAmount = LDT.DisburseAmount.Value,
                                   CreditCardExpiryDate = LDT.CreditCardExpiryDate.Value,
                                   CreditCardIssueDate = LDT.IssueDate.Value,
                                   CashLimit = LDT.CashLimit.Value,
                                   CreditLimit = LDT.CreditLimit.Value,
                                   //CRAccNo1 = LDT.LoanAccountNo1,    
                                   //CRAccNo2 = LDT.LoanAccountNo2,
                                   //CRAccNo3 = LDT.LoanAccountNo3,
                                   //CRAccNo4 = LDT.LoanAccountNo4                                   
                               }).FirstOrDefault();

                return View(strSql);
            }                 
        }

        [HttpPost]
        public ActionResult EditAction(LoanDisburseTransaction_Metadata LDT_MD)
        {   
            int intUserId = Convert.ToInt32(Session["UserId"]);

            var tblLoanDisburseTransaction = db.LoanDisburseTransactions.Where(x => x.Id == LDT_MD.Id).FirstOrDefault();

            var tblEMITransaction = db.EMITransactions.Where(x => x.LoanDisburseId ==LDT_MD.Id).FirstOrDefault(); 
                try
                {
                    tblLoanDisburseTransaction.LoanDisburseDate = LDT_MD.LoanDisburseDate; 
                    tblLoanDisburseTransaction.DisburseAmount = LDT_MD.DisburseAmount;
                    
                    if (LDT_MD.Category == "L")
                    {
                        tblLoanDisburseTransaction.FirstInstallment = LDT_MD.FirstInstallment;
                        tblLoanDisburseTransaction.Tenure = LDT_MD.NoOfMonths;
                        tblLoanDisburseTransaction.ExpiryDate = LDT_MD.ExpiryDate; 
                        //tblLoanDisburseTransaction.EMI = LDT_MD.EMI;
                        tblLoanDisburseTransaction.RateOfInterest = LDT_MD.RateOfInterest;
                        tblLoanDisburseTransaction.InterestTypeId = LDT_MD.InterestTypeId;
                        tblLoanDisburseTransaction.FirstInstallmentDate = LDT_MD.FirstInstallmentDate;
                        tblLoanDisburseTransaction.LoanAccountDate = LDT_MD.LoanAccCreateDate;
                        tblLoanDisburseTransaction.LoanAccountNo = LDT_MD.LoanAccNo;
                    }
                    else
                    {                        
                        tblLoanDisburseTransaction.CashLimit = LDT_MD.CashLimit;                        
                        tblLoanDisburseTransaction.CreditLimit = LDT_MD.CreditLimit;
                        //tblLoanDisburseTransaction.LoanAccountNo1 = LDT_MD.CRAccNo1;
                        //tblLoanDisburseTransaction.LoanAccountNo2 = LDT_MD.CRAccNo2;
                        //tblLoanDisburseTransaction.LoanAccountNo3 = LDT_MD.CRAccNo3;
                        //tblLoanDisburseTransaction.LoanAccountNo4 = LDT_MD.CRAccNo4;
                        tblLoanDisburseTransaction.CreditCardExpiryDate = LDT_MD.CreditCardExpiryDate;
                        tblLoanDisburseTransaction.IssueDate = LDT_MD.CreditCardIssueDate;                      
                    }

                    tblLoanDisburseTransaction.EditDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                    tblLoanDisburseTransaction.EditId = intUserId;

                    db.SaveChanges();
                    dropList();
                    //TempData["Message"] = "Record Update Successfully.";                    
                    return Content(("<script language='javascript' type='text/javascript'> alert('Data saved successfully!'); window.location.href ='/LoanDisburse/ViewLoanDisbursed';</script>"));

                    
                    //return RedirectToAction("ViewLoanDisbursed");
                }
                catch (Exception e)
                {
                    string strErrMsg = e.Message;
                    dropList();
                    return View("EditLoanDisburse");
                }           

        }
        [HttpGet]
        public ActionResult LoanApplicationApproveList()
        {
            int F_Id = Convert.ToInt32(Session["EmployerName"]);
            if (F_Id == 0)
            {
                return Content(("<script language='javascript' type='text/javascript'> alert('You are not authorised person to access this module') ; window.location.href ='/Home/_Home'; </script>"));
                //return View("_Home");
            }
            int FI_ID =  Convert.ToInt32(Session["EmployerName"]);
            int FI_Br_ID = Convert.ToInt32(Session["EmployerDivision"]);
            string strLoanStatus ="A";

            var strSql = (from AppLoan in db.sp_LoanApplicationStatusList(FI_ID, FI_Br_ID, strLoanStatus) select AppLoan).ToList();
            return View(strSql);
        }
        [HttpPost]
        public ActionResult LoanApplicationApproveList(LoanDisburseTransaction_Metadata objLoanDis)
        {
            return View();
        }
        [HttpGet]
        public ActionResult LoanDisburseUnpaidList()
        {
            int F_Id = Convert.ToInt32(Session["EmployerName"]);
            if (F_Id == 0)
            {
                return Content(("<script language='javascript' type='text/javascript'> alert('You are not authorised person to access this module') ; window.location.href ='/Home/_Home'; </script>"));
                //return View("_Home");
            }
            int FI_ID = Convert.ToInt32(Session["EmployerName"]);
            int FI_Br_ID = Convert.ToInt32(Session["EmployerDivision"]);
            var strSql2 = (from ViewList in db.sp_ViewLoanDisburseList(FI_ID, FI_Br_ID) select ViewList).ToList();
            return View(strSql2);
        }
        
        

        
        //----------------------------------------------------------------------


    }
}
