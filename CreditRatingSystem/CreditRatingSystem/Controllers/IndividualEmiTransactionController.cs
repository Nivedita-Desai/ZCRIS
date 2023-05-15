using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CreditRatingModel.Model;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Transactions;
using System.Data.Objects;

namespace CreditRatingSystem.Controllers
{
   
    public class IndividualEmiTransactionController : Controller
    {
        private creaditratingEntities db = new creaditratingEntities();

        public string n;
        public string AutoGenrateNumber;
        //
        // GET: /IndividualEmiTransaction/
        [HttpGet]
        public ActionResult AddIndividualEmiTransaction()
        {
            BindGrid();
            return View();
        }
        [HttpPost]
        public ActionResult AddIndividualEmiTransaction(IndividualEmiTransaction_Metadata objIndEmiTran)
        {
            string msg;
            using (TransactionScope transaction = new TransactionScope())
            {
                try
                {
                    SaveIndividualEmiTransaction(objIndEmiTran);
                    transaction.Complete();
                    msg = "Data Saved Successfully!";
                }
                catch (Exception)
                {
                    transaction.Dispose();
                    msg = "Data saved Failed!";
                }
            }

            BindGrid();
            ViewBag.msg = msg;
            //return Content("<script language='javascript' type='text/javascript'> alert('" + msg + "'); window.location.href ='/IndividualEmiTransaction/AddIndividualEmiTransaction';</script>");
            return View();
        }

       
        public ActionResult AddIndividualEmiTransactionOnline()
        {

            ModelState.Clear();
            BindGrid();
            return View();
        }

        [HttpGet]
        public ActionResult ReverseIndividualEmiTransaction()
        {
            BindGrid();
            return View();
        }

        [HttpPost]
        public ActionResult ReverseIndividualEmiTransaction(IndividualEmiTransaction_Metadata objIndEmiTran)
        {
            string msg2;
            int UserId = Convert.ToInt32(Session["UserId"]);
            using (TransactionScope transaction = new TransactionScope())
            {
                try
                {
                    string msg1 = "msg";
                    //string strUserType = "";

                    ObjectParameter objParam = new ObjectParameter(msg1, typeof(string));


                  //  var j = db.SP_Reverse_Loan_Installment_Transaction(objIndEmiTran.TranId,UserId,objParam);
                    var j = db.SP_Reverse_LoanDisburse_Transaction(objIndEmiTran.TranId, UserId, objParam).FirstOrDefault();
                    msg2 = objParam.Value.ToString();

                    transaction.Complete();
                    //msg2 = "Data Saved Successfully!";
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    msg2 = "Transaction reversed Failed!";
                }
            }

            BindGrid();
            ViewBag.msg = msg2;
            return Content("<script language='javascript' type='text/javascript'> alert('" + msg2 + "'); window.location.href ='/IndividualEmiTransaction/ReverseIndividualEmiTransaction';</script>");
            //return View();
        }
        public void BindGrid()
        {
            IndividualEmiTransaction_Metadata objIndEmiTran = new IndividualEmiTransaction_Metadata();
            //ViewBag.LoanNoList = new SelectList((from ELD in db.EducationLoanDisbursments where ELD.NId = objIndEmiTran.NationalId select ELD).ToList(),"","");
            //ViewBag.LoanNoList = new SelectList(db.EducationLoanDisbursments, "Id", "LoanAccountNo");
            var NationalizeBank = (from e in db.FinancialInstitutionMasters where e.FinancialInstituteCategoryId == 1 select e);
            ViewBag.BankNameList = new SelectList(NationalizeBank, "Id", "Name");
            //ViewBag.BranchNameList = new SelectList(db.FinancialInstitutionBranchMasters, "Id", "BranchName");
        }

        
        public JsonResult LoanAccountList(string NationalId)
        {

            string NID = NationalId;

            var states = (from s in db.EducationLoanDisbursments
                          where s.NId == NID && s.AccountStatus=="A"
                          select s);

            return Json(new SelectList(states.ToArray(), "Id", "LoanAccountNo"), JsonRequestBehavior.AllowGet);
        }

        public JsonResult EmiTransactionList(string TransactionId)
        {

            string TrID = TransactionId;

            var Transaction = (from d in db.SP_GetEmiTransactionList(TrID)
                               select new IndividualEmiTransaction_Metadata 
                               {
                                   LoanAccountNo=d.LoanAccountNo,
                                   FullName=d.FullName,
                                   Gender=d.gender,
                                   LoanAmt=d.LoanAmount.Value,
                                   DisAmt=d.DisburseAmount.Value,
                                   BalAmt=d.BalanceAmount.Value,
                                   RepayMode=d.RepayMode,
                                   RepayAmt=d.RepaymentAmount.Value,
                                   TranNo=d.TransactionNo,
                                   ChqDt=d.TransactionDate.Value,
                                   BankName=d.BankName,
                                   BranchName=d.BranchName,
                                   Remark=d.Remark
                               }).FirstOrDefault();
            //var Transaction = (
            //                   from d in db.EducationLoanTransactions
            //                   join t in db.IndividualCustomerMasters on d.NId equals t.Pan
            //                   join s in db.EducationLoanDisbursments on d.LoanAccountNo equals s.LoanAccountNo
            //                   join F in db.FinancialInstitutionMasters on d.FinancialInstituteId equals F.Id
                                
            //                   where d.TransactionId == TrID && d.NId == s.NId select d
            //                   //select new IndividualEmiTransaction_Metadata
            //                   //{
            //                   //    LoanAccountNo=d.LoanAccountNo,
            //                   //    FullName = t.FirstName + " " + t.MiddleName + " " + t.LastName,
            //                   //    Gender = t.Sex,
            //                   //    LoanAmt = s.LoanAmount.Value,
            //                   //    DisAmt = s.DisburseAmount.Value,
            //                   //    BalAmt = s.BalanceAmount.Value,
            //                   //    RepayMode=d.RepayMode,
            //                   //    RepayAmt=d.RepaymentAmount.Value,
            //                   //    TranNo=d.TransactionNo,
            //                   //    ChqDt=d.TransactionDate.Value,
            //                   //    BankName= F.Name,
            //                   //    BranchName = ,
            //                   //    Remark=d.Remark
            //                   //    // BalAmt = Convert.ToDecimal(s.BalanceAmount.Value)
            //                   //}
            //                       ).FirstOrDefault();

            return Json(Transaction, JsonRequestBehavior.AllowGet);
            
        }
        public JsonResult BranchList(int bankId)
        {

            int BankID = bankId;

            var lstBranchList = (from s in db.FinancialInstitutionBranchMasters
                          where s.FinancialInstituteId == BankID
                          select s);

            return Json(new SelectList(lstBranchList.ToArray(), "Id", "BranchName"), JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoanDetails(string LoanAccNo)
        {

            string LAccNo = LoanAccNo;

            var LoanDetails = (from s in db.EducationLoanDisbursments
                          join t in db.IndividualCustomerMasters on s.NId equals t.Pan 
                          where s.LoanAccountNo == LAccNo
                          select new IndividualEmiTransaction_Metadata 
                          { 
                              FullName=t.FirstName + " " + t.MiddleName + " "  + t.LastName,
                              Gender=t.Sex,
                              LoanAmt=s.LoanAmount.Value,
                              DisAmt=s.DisburseAmount.Value ,
                              BalAmt = s.BalanceAmount.Value
                             // BalAmt = Convert.ToDecimal(s.BalanceAmount.Value)

                          }).FirstOrDefault();

            return Json(LoanDetails, JsonRequestBehavior.AllowGet);
        }

        public JsonResult greaterAmount(decimal Rangefrom, decimal RangeTo)
        {

            var result = true;
            //var Rangef = (from b in db.SlabMasters
            //              where b.RangeFrom == Rangefrom
            //              select b.RangeFrom).FirstOrDefault();
            if (RangeTo > Rangefrom)
            {
                result = false;
            }



            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public string GenerateTransactionNo()
        {
            string Day = Convert.ToString(DateTime.UtcNow.Day);

            if (Day.Length==1)
            {
                Day = 0 + Day;
            }

            int Month = DateTime.UtcNow.Month;
            int Year = DateTime.UtcNow.Year;
            string d = Day + "" + Month + "" + Year;
            
            AutoGenrateNumber = "TR" + d + "00001";

        b:
            var c = (from s in db.EducationLoanTransactions
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

        public ActionResult onRepaymentModeSelection()
        {
            IndividualEmiTransaction_Metadata IETM = new IndividualEmiTransaction_Metadata();
            //IETM.TranId = GenerateTransactionNo();
            IETM.ChqDt = DateTime.UtcNow.AddHours(5).AddMinutes(30);
            return Json(IETM, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveIndividualEmiTransaction(IndividualEmiTransaction_Metadata IETM)
        {
            
            string selectedLoanNo = Request.Form["selLoanNo"];
            string selectedBranch = Request.Form["selBranch"];
           

            int BranchId = Convert.ToInt32(selectedBranch);
            

            string TransactionNo=GenerateTransactionNo();

            if (selectedLoanNo != "")
            {
                try
            {
                EducationLoanTransaction ELT = new EducationLoanTransaction();

                //ELT.TransactionId = IETM.TranId;
                ELT.TransactionId = TransactionNo;
                ELT.NId = IETM.NationalId;
                ELT.LoanAccountNo = selectedLoanNo;
                ELT.RepayMode = IETM.RepayMode;
                ELT.RepaymentAmount = IETM.RepayAmt;
                ELT.Remark = IETM.Remark;
                ELT.TransactionDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);

                if (IETM.RepayMode == "C")
                {
                    //ELT.TransactionNo = IETM.TranNo;
                    ELT.TransactionNo = TransactionNo;
                    ELT.ChequeClear = "C";
                    ELT.AvailableBalance = IETM.BalAmt - IETM.RepayAmt;
                    ELT.LedgerBalance = IETM.BalAmt - IETM.RepayAmt;
                    ELT.RepaymentDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                }

                if (IETM.RepayMode == "H")
                {
                    ELT.TransactionNo = IETM.TranNo;
                    ELT.ChequeClear = "P";
                    ELT.FinancialInstituteId = IETM.BankId;
                    ELT.FinancialInstituteBranchId = BranchId;
                    ELT.RepaymentDate = IETM.ChqDt;
                    ELT.AvailableBalance = IETM.BalAmt - IETM.RepayAmt;
                    ELT.LedgerBalance = IETM.BalAmt;
                }


                ELT.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                ELT.CreateId = Convert.ToInt32(Session["UserId"]);
                db.AddToEducationLoanTransactions(ELT);
                db.SaveChanges();

                if ((IETM.BalAmt - IETM.RepayAmt) <= 0)
                {
                    if (IETM.RepayMode == "C")
                    {
                        //EducationLoanDisbursment ELD = new EducationLoanDisbursment();
                        var EM = db.EducationLoanDisbursments.Where(x => x.LoanAccountNo == selectedLoanNo).FirstOrDefault();
                        EM.AccountStatus = "C";
                        EM.ClosureDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                        EM.BalanceAmount = 0;
                        db.SaveChanges();
                    }
                    
                }
                else
                {
                    if (IETM.RepayMode == "C")
                    {
                        var EM = db.EducationLoanDisbursments.Where(x => x.LoanAccountNo == selectedLoanNo).FirstOrDefault();
                        EM.BalanceAmount = (IETM.BalAmt - IETM.RepayAmt);
                        db.SaveChanges();
                    }
                   
                }

               
                // NIDDetails(IETM.NationalId, selectedLoanNo);

                var B = (from c in db.EducationLoanTransactions
                         join d in db.EducationLoanDisbursments on c.LoanAccountNo equals d.LoanAccountNo
                         where c.NId == IETM.NationalId && c.LoanAccountNo == selectedLoanNo
                         orderby c.Id descending
                         select new IndividualEmiTransaction_Metadata
                         {
                             TranId = c.TransactionId,
                             ChqDt = c.TransactionDate.Value,
                             RepayMode = c.RepayMode.Equals("C") ? "Cash" : c.RepayMode.Equals("H") ? "Cheque" : "Salary",
                             RepayAmt = c.RepaymentAmount.Value,
                             status = c.ChequeClear.Equals("P") ? "Pending" : "Close",
                             Remark = c.Remark,
                             LoanNoList = c.LoanAccountNo,

                         }).ToList();
                ViewBag.PaidTransactionList = B;

                var e = (from c in db.EducationLoanTransactions
                         join d in db.EducationLoanDisbursments on c.LoanAccountNo equals d.LoanAccountNo
                         where c.NId == IETM.NationalId && c.LoanAccountNo == selectedLoanNo
                         select new IndividualEmiTransaction_Metadata
                         {
                             TotalLoanAmount = d.DisburseAmount.Value,
                             TotalBalanceAmount = d.BalanceAmount.Value
                         }).FirstOrDefault();
                ViewBag.TotalAmt = e.TotalLoanAmount;
                ViewBag.TotalBalAmt = e.TotalBalanceAmount;
                ViewBag.LoanAccNo = selectedLoanNo;
                if (B == null)
                {
                    var b = "null";
                    return Json(b, JsonRequestBehavior.AllowGet);
                }
               // return Content("<script language='javascript' type='text/javascript'> alert('Data Saved Successfully!');</script>");
                //return Json(B, JsonRequestBehavior.AllowGet);
            }
                catch (Exception e)
                {
                    string m = e.Message;
                }
            }
            

                
            BindGrid();
            return View("AddIndividualEmiTransaction");

        }

        //public ActionResult NIDDetails(string NID, string LoanAccNo)
        //{
        //    var B = (from c in db.EducationLoanTransactions
        //             where c.NId == NID && c.LoanAccountNo == LoanAccNo
        //             select new IndividualEmiTransaction_Metadata
        //             {
        //                 TranId=c.TransactionId,
        //                 ChqDt=c.TransactionDate.Value,
        //                 RepayMode=c.RepayMode,
        //                 RepayAmt=c.RepaymentAmount.Value,
        //                 status=c.ChequeClear,
        //                 Remark=c.Remark
        //             }).FirstOrDefault();

        //    if (B == null)
        //    {
        //        var b = "null";
        //        return Json(b, JsonRequestBehavior.AllowGet);
        //    }
        //    return Json(B, JsonRequestBehavior.AllowGet);
        //}
        public ActionResult RepaymentReceipt(string id)
        {
            using (creaditratingEntities db = new creaditratingEntities())
            {
                try
                {
                    DataSet ds = new DataSet();
                    DataTable dt = new DataTable();
                    SqlParameter parameters1 = new SqlParameter();
                    SqlParameter parameters2 = new SqlParameter();

                    parameters1.ParameterName = "@NID";
                    parameters1.Value = id;


                    var connectionString = System.Configuration.ConfigurationManager.AppSettings["direct_connection_string"];
                    var ds1 = new DataSet();

                    using (var conn = new SqlConnection(connectionString))
                    {
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = "spPersonalInfo";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(parameters1);

                            using (var adapter = new SqlDataAdapter(cmd))
                            {
                                adapter.Fill(ds);
                            }
                        }
                    }
                    if (ds.Tables[0].Rows.Count < 1)
                    {
                        return Content("<script language='javascript' type='text/javascript'> alert('No Record Found!'); window.location.href ='/IndividualEmiTransaction/AddIndividualEmiTransaction';  </script>");
                    }
                    else
                    {
                        using (ReportClass rptH = new ReportClass())
                        {
                            //rptH.FileName = "E:/tanmay/ProjectsNew/CRS/CreditRatingSystem/CreditRatingSystem/CrystalReport1.rpt";
                            rptH.FileName = Server.MapPath("~/Report/Student_Info.rpt");


                            rptH.Load();
                            rptH.Database.Tables[0].SetDataSource(ds.Tables[0]);
                            rptH.Refresh();
                            rptH.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                            return View();
                        }

                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                    //Json(new { msg = "Error : " + ex.Message }, JsonRequestBehavior.AllowGet);
                }

            }

        }
       

    }
}
