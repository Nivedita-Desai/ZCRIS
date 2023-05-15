using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CreditRatingModel.Model;
using System.Transactions;
using System.Text.RegularExpressions;

namespace CreditRatingSystem.Controllers
{
    public class CreditStatementTransactionController : Controller
    {
        //
        // GET: /CreditStatementTransaction/
        private creaditratingEntities db = new creaditratingEntities();
        public string n;
        public string AutoGenrateNumber;

        [HttpGet]
        public ActionResult AddCreditStatementDetails()
        {
            int FI_ID = Convert.ToInt32(Session["EmployerName"]);
            var FI_Name = (from F in db.FinancialInstitutionMasters
                           where F.Id == FI_ID
                           select F.Name
                               ).FirstOrDefault();
            ViewBag.FinancialInstituteName = FI_Name;
            return View();
        }

        [HttpPost]
        public ActionResult AddCreditStatementDetails(CreditStatementTransaction_Metadata objCreditStatementTransaction_Metadata)
        {
            string msg;
            using (TransactionScope transaction = new TransactionScope())
            {
                try
                {
                    addCreditCardStatement(objCreditStatementTransaction_Metadata);
                    transaction.Complete();
                    msg = "Data saved successfully!";
                }
                catch (Exception)
                {
                    transaction.Dispose();
                    msg = "Data saved Failed!";
                }
            }

            return Content(("<script language='javascript' type='text/javascript'> alert('" + msg + "'); window.location.href ='/CreditStatementTransaction/AddCreditStatementDetails';  </script>"));
            

        }

        public void dropList()
        {
            ViewBag.BankList = new SelectList((from FIM in db.FinancialInstitutionMasters
                                               where FIM.FinancialInstituteCategoryId == 1
                                               orderby FIM.Name
                                               select FIM).ToList(), "Id", "Name");
            //ViewBag.BranchList = new SelectList((from FIB in db.FinancialInstitutionBranchMasters select FIB).ToList(), "Id", "BranchName");
        }

        public JsonResult RetrieveLoanDetails(string strLoanNo)
        {
            //var Result=(from A in db.LoanDisburseTransactions
            //                join LoanApp in db.LoanApplicationTransactions on A.LoanApplicationId equals LoanApp.Id
            //                join CrType in db.CardTypeMasters on LoanApp.CardTypeId equals CrType.Id
            //                where A.LoanAccountNo==strLoanNo
            //                select CrType);
            var Result = (from ld in db.SP_GET_CARD_DETAILS(Regex.Replace(strLoanNo," ",""), "C", "A") select ld).FirstOrDefault();
            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult addCreditCardStatement(CreditStatementTransaction_Metadata objCreditStatementTransaction_Metadata)
        {
            int intUserId = Convert.ToInt32(Session["UserId"]);
            LoanDisbursedInstallment objLoanDisInst = new LoanDisbursedInstallment();
            objLoanDisInst.TransactionId = GenerateTransactionNo();
            objLoanDisInst.InstCounter = objCreditStatementTransaction_Metadata.InstCounter;
            objLoanDisInst.InstAmt = objCreditStatementTransaction_Metadata.InstAmount;
            objLoanDisInst.InstDate = objCreditStatementTransaction_Metadata.InstDate;
            objLoanDisInst.LoanDisbursedId = objCreditStatementTransaction_Metadata.LoanDisbursedId;
            objLoanDisInst.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30); 
            objLoanDisInst.CreateId = intUserId;
            objLoanDisInst.BillingCycleStartDate = objCreditStatementTransaction_Metadata.BillingCycleStartDate;
            objLoanDisInst.MinDueAmt = objCreditStatementTransaction_Metadata.MinDueAmt;
            objLoanDisInst.InstDueDate = objCreditStatementTransaction_Metadata.InstDueDate;
            db.AddToLoanDisbursedInstallments(objLoanDisInst);
            db.SaveChanges();

            return View("AddCreditStatementDetails");
        }

        public string GenerateTransactionNo()
        {
            string Day = Convert.ToString(DateTime.UtcNow.Day);

            if (Day.Length == 1)
            {
                Day = 0 + Day;
            }

            string Month = Convert.ToString(DateTime.UtcNow.Month);
            if (Month.Length==1)
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


    }
}
