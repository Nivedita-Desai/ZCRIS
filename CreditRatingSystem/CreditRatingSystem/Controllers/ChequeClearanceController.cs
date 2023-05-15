using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CreditRatingModel.Model;

namespace CreditRatingSystem.Controllers
{
    public class ChequeClearanceController : Controller
    {
        //
        // GET: /CheckClearance/
        creaditratingEntities db = new creaditratingEntities();
        ChequeClearance CC = new ChequeClearance();

        public ActionResult ChequeClearance()
        {
            return View();
        }
        public JsonResult GetChequeClrList(string NRC,string ChequeNo)
        {
           
            if (NRC == "0" && ChequeNo=="0")
            {
                var strSql1 = (from ELT in db.EducationLoanTransactions
                              join FIM in db.FinancialInstitutionMasters on ELT.FinancialInstituteId equals FIM.Id
                              join FBM in db.FinancialInstitutionBranchMasters on ELT.FinancialInstituteBranchId equals FBM.Id
                              join ICM in db.IndividualCustomerMasters on ELT.NId equals ICM.Pan
                              where ELT.RepayMode == "H" && ELT.ChequeClear == "P"
                              select new
                              {
                                  ELT.NId,
                                  ELT.Id,
                                  StudentName = ICM.FirstName + " " + ICM.MiddleName + " " + ICM.LastName,
                                  ELT.LoanAccountNo,
                                  FIM.Name,
                                  FBM.BranchName,
                                  ELT.TransactionNo,
                                  ELT.TransactionDate,
                                  ELT.RepaymentAmount,
                                  ChequeClearanceDate = DateTime.UtcNow
                              }).ToList();
                ViewBag.a = strSql1;
            }
            else
            {
                var strSql2 = (from ELT in db.EducationLoanTransactions
                              join FIM in db.FinancialInstitutionMasters on ELT.FinancialInstituteId equals FIM.Id
                              join FBM in db.FinancialInstitutionBranchMasters on ELT.FinancialInstituteBranchId equals FBM.Id
                              join ICM in db.IndividualCustomerMasters on ELT.NId equals ICM.Pan
                              where ELT.RepayMode == "H" && ELT.ChequeClear == "P" && ELT.NId == NRC ||ELT.TransactionNo==ChequeNo
                              select new
                              {
                                  ELT.NId,
                                  ELT.Id,
                                  StudentName = ICM.FirstName + " " + ICM.MiddleName + " " + ICM.LastName,
                                  ELT.LoanAccountNo,
                                  FIM.Name,
                                  FBM.BranchName,
                                  ELT.TransactionNo,
                                  ELT.TransactionDate,
                                  ELT.RepaymentAmount,
                                  ChequeClearanceDate = DateTime.UtcNow
                              }).ToList();
                ViewBag.a = strSql2;
            }
            var strSql = ViewBag.a;
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
        public JsonResult SaveData(ChequeClearance cc)
        {
            using (creaditratingEntities db = new creaditratingEntities())
            {
                try
                {
                    var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    string Data1 = cc.SaveChequeClearance;


                    IEnumerable<ChequeClearance> det = serializer.Deserialize<IEnumerable<ChequeClearance>>(Data1);

                    int intUserId = Convert.ToInt32(Session["UserId"]);

                    if (det != null)
                    {
                        foreach (var item in det)
                        {
                            var ELD = db.EducationLoanTransactions.Where(x => x.Id == item.CheckId).FirstOrDefault();
                            var LN = ELD.LoanAccountNo;
                            var CA = ELD.RepaymentAmount;
                            ELD.ChequeClearanceDate = item.CheckClearanceDate;
                            ELD.LedgerBalance = ELD.AvailableBalance;
                            ELD.ChequeClear = "C";
                            ELD.EditId = intUserId;
                            ELD.EditDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);

                            var ELD1 = db.EducationLoanDisbursments.Where(x => x.LoanAccountNo == LN).FirstOrDefault();
                            var BA = ELD1.BalanceAmount;
                            ELD1.BalanceAmount = BA - CA;
                            if (ELD1.BalanceAmount==0)
                            {
                                ELD1.ClosureDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                                ELD1.AccountStatus = "C";
                                ELD.EditId = intUserId;
                                ELD.EditDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                            }
                        }
                        db.SaveChanges();
                    }

                    return Json(new { msg = "Data Saved Successfully." }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { msg = "Data Saved Faild." + ex.ToString() }, JsonRequestBehavior.AllowGet);
                }
            }
        }
    }
}
