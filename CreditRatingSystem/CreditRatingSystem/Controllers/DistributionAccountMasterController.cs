using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CreditRatingModel.Model;
using System.Transactions;

namespace CreditRatingSystem.Controllers
{
    public class DistributionAccountMasterController : Controller
    {
        //
        // GET: /DistributionAccountMaster/
        private creaditratingEntities db = new creaditratingEntities();

        [HttpGet]
        public ActionResult AddDistributionAccountDetails()
        {  
            BindCombo();
            return View();
        }

        [HttpPost]
        public ActionResult AddDistributionAccountDetails(DistributionAccountMaster_Metadata EAM)
        {
             string msg;
             using (TransactionScope transaction = new TransactionScope())
             {
                 try
                 {
                     SaveAccountDetails(EAM);
                     transaction.Complete();
                     msg = "Data saved successfully!";
                 }
                 catch (Exception)
                 {
                     transaction.Dispose();
                     msg = "Data saved Failed!";
                 }
             }
            
           
            return Content("<script language='javascript' type='text/javascript'> alert('" + msg + "'); window.location.href ='/DistributionAccountMaster/AddDistributionAccountDetails'; </script>");

            //BindCombo();
            //return View();
        }

        public void BindCombo()
        {
            IndividualEmiTransaction_Metadata objIndEmiTran = new IndividualEmiTransaction_Metadata();
            //ViewBag.LoanNoList = new SelectList((from ELD in db.EducationLoanDisbursments where ELD.NId = objIndEmiTran.NationalId select ELD).ToList(),"","");
            ViewBag.EmployerList = new SelectList(db.EmployerMasters, "Id", "EmployerName");
            ViewBag.AccountTypeList = new SelectList(db.AccountTypeMasters, "Id", "AccountType");
            ViewBag.DistributionTypeList = new SelectList(db.DistributionTypeMasters, "Id", "Name");
            var NationalizeBank = (from e in db.FinancialInstitutionMasters where e.FinancialInstituteCategoryId == 1 select e);
            ViewBag.BankNameList = new SelectList(NationalizeBank, "Id", "Name");
            //ViewBag.BranchNameList = new SelectList(db.FinancialInstitutionBranchMasters, "Id", "BranchName");
        }
        //public void BindComboEdit()
        //{
        //    IndividualEmiTransaction_Metadata objIndEmiTran = new IndividualEmiTransaction_Metadata();
        //    //ViewBag.LoanNoList = new SelectList((from ELD in db.EducationLoanDisbursments where ELD.NId = objIndEmiTran.NationalId select ELD).ToList(),"","");
        //    ViewBag.EmployerList = new SelectList(db.EmployerMasters, "Id", "EmployerName");
        //    ViewBag.AccountTypeList = new SelectList(db.AccountTypeMasters, "Id", "AccountType");
        //    ViewBag.DistributionTypeList = new SelectList(db.DistributionTypeMasters, "Id", "ShortName");
        //    var NationalizeBank = (from e in db.FinancialInstitutionMasters where e.FinancialInstituteCategoryId == 1 select e);
        //    ViewBag.BankNameList = new SelectList(NationalizeBank, "Id", "Name");
        //    ViewBag.BranchNameList = new SelectList(db.FinancialInstitutionBranchMasters, "Id", "BranchName");
        //}
        public JsonResult BranchList(int bankId)
        {

            int BankID = bankId;

            var lstBranchList = (from s in db.FinancialInstitutionBranchMasters
                                 where s.FinancialInstituteId == BankID
                                 select s);

            return Json(new SelectList(lstBranchList.ToArray(), "Id", "BranchName"), JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveAccountDetails(DistributionAccountMaster_Metadata EAMM)
        {
            string selectedBranch = Request.Form["selBranch"];
            int BranchId = Convert.ToInt32(selectedBranch);
            DistributionAccountMaster EAM = new DistributionAccountMaster();

            EAM.AccountName = EAMM.AccountName;
            EAM.AccountShortName = EAMM.AccountShortName;
            EAM.AccountType = EAMM.AccountType;
            EAM.DistributionTypeId = EAMM.DistributionTypeId;
            //EAM.EmployerId = EAMM.EmployerId;
            EAM.FinancialInstitutionId = EAMM.FinancialInstitutionId;
            EAM.FinancialInstitutionBranchId = BranchId;
            EAM.IFSCCode = EAMM.IFSCCode;
            EAM.Narration = EAMM.Narration;
            EAM.Status = true;
            EAM.AccountNo = EAMM.AccountNo;
            EAM.CreateId = Convert.ToInt32(Session["UserId"]);
            EAM.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
            db.AddToDistributionAccountMasters(EAM);
            db.SaveChanges();
            ModelState.Clear();
            return View("AddDistributionAccountDetails");
        }

        public string CheckAccNO(string accountNo)
        {
            string error = "";

            var IsAccNoExist = db.DistributionAccountMasters.Where(u => u.AccountNo.Equals(accountNo)).FirstOrDefault();
            if (IsAccNoExist != null)
            {
                error = "Same account number already exist";
            }
            return error;
        }

        public string CheckAccName(string accountName)
        {
            string error = "";

            var IsAccNameExist = db.DistributionAccountMasters.Where(u => u.AccountName.Equals(accountName)).FirstOrDefault();
            if (IsAccNameExist != null)
            {
                error = "Same account name already exist";
            }
            return error;
        }

        public string CheckAccShortName(string accountShortName)
        {
            string error = "";

            var IsAccShortNameExist = db.DistributionAccountMasters.Where(u => u.AccountShortName.Equals(accountShortName)).FirstOrDefault();
            if (IsAccShortNameExist != null)
            {
                error = "Same short name for account already exist";
            }
            return error;
        }

        public ActionResult ViewDistributionAccount()
        {
            var DistributionAccount = (from EACC in db.DistributionAccountMasters
                                   join FI in db.FinancialInstitutionMasters on EACC.FinancialInstitutionId equals FI.Id
                                   join FIBR in db.FinancialInstitutionBranchMasters on EACC.FinancialInstitutionBranchId equals FIBR.Id
                                   select new DistributionAccountMaster_Metadata
                                   {
                                       AccountName = EACC.AccountName,
                                       AccountShortName = EACC.AccountShortName,
                                       AccountNo = EACC.AccountNo,
                                       BankName = FI.Name,
                                       BranchName = FIBR.BranchName,
                                       IFSCCode = EACC.IFSCCode,
                                       Id = EACC.Id
                                   }).ToList();
            return View(DistributionAccount);
        }

        [Authorize]
        public ActionResult EditDistributionAccount(int id)
        {

            var sqlstr = (from s in db.DistributionAccountMasters
                              where s.Id == id
                              select new DistributionAccountMaster_Metadata
                              {
                                  //EmployerId = s.EmployerId.Value,
                                  DistributionTypeId = s.DistributionTypeId.Value,
                                  AccountName = s.AccountName,
                                  AccountShortName = s.AccountShortName,
                                  AccountType = s.AccountType.Value,
                                  FinancialInstitutionId = s.FinancialInstitutionId.Value,
                                  FinancialInstitutionBranchId = s.FinancialInstitutionBranchId.Value,
                                  AccountNo = s.AccountNo,
                                  IFSCCode = s.IFSCCode,
                                  Narration = s.Narration,
                                  status = s.Status.Value
                              }).FirstOrDefault();

                var BranchList = (from e in db.FinancialInstitutionBranchMasters where e.FinancialInstituteId == sqlstr.FinancialInstitutionBranchId select e);
                ViewBag.BranchNameList = new SelectList(BranchList, "Id", "BranchName");

                //ViewBag.BranchNameList = (from e in db.FinancialInstitutionBranchMasters where e.FinancialInstituteId == sqlstr.FinancialInstitutionBranchId select e);
                BindCombo();
                return View(sqlstr);
        }

        //[HttpPost]
        //public ActionResult EditSaveAccountDetails(DistributionAccountMaster_Metadata DAM)
        //{
        //    int intUserId = Convert.ToInt32(Session["UserId"]);
        //    try
        //    {
        //        var DM = db.DistributionAccountMasters.Where(x => x.Id == DAM.Id).FirstOrDefault();
        //        DM.AccountName = DAM.AccountName;
        //        DM.AccountShortName = DAM.AccountShortName;
        //        DM.AccountType = DAM.AccountType;
        //        DM.DistributionTypeId = DAM.DistributionTypeId;
        //        //EAM.EmployerId = EAMM.EmployerId;
        //        DM.FinancialInstitutionId = DAM.FinancialInstitutionId;
        //        DM.FinancialInstitutionBranchId = DM.FinancialInstitutionBranchId;
        //        DM.IFSCCode = DAM.IFSCCode;
        //        DM.Narration = DAM.Narration;
        //        DM.Status = DAM.status;
        //        DM.AccountNo = DAM.AccountNo;
        //        db.SaveChanges();
        //    }
        //    catch (Exception e)
        //    {
        //        return Content("<script language='javascript' type='text/javascript'> alert('" + e + "'); </script>");
        //    }
        //    return Content("<script language='javascript' type='text/javascript'> alert('Data Saved Successfully!'); window.location.href ='/DistributionAccountMaster/ViewDistributionAccount'; </script>");
        //}

    }
}
