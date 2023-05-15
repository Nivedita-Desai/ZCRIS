using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CreditRatingModel.Model;
using System.Transactions;

namespace CreditRatingSystem.Controllers
{
    public class EmployerAccountMasterController : Controller
    {
        //
        // GET: /EmployerAccountMaster/
        private creaditratingEntities db = new creaditratingEntities();

        [HttpGet]
        public ActionResult AddEmployerAccountDetails()
        {
            //if (id!=0)
            //{
            //    var sqlstr = (from s in db.EmployerAccountMasters 
            //                  where s.Id ==id
            //                  select new EmployerAccountMaster_Metadata
            //                  {
            //                      EmployerId = s.EmployerId.Value,
            //                      EmployerDivisionId = s.EmployerDivisionId.Value,
            //                      DistributionTypeId = s.DistributionTypeId.Value,
            //                      AccountName = s.AccountName,
            //                      AccountShortName = s.AccountShortName,
            //                      AccountType = s.AccountType.Value,
            //                      FinancialInstitutionId =s.FinancialInstitutionId.Value,
            //                      FinancialInstitutionBranchId = s.FinancialInstitutionBranchId.Value,
            //                      AccountNo = s.AccountNo,
            //                      IFSCCode = s.IFSCCode,
            //                      Narration = s.Narration,
            //                      status = s.Status.Value
            //                  }).FirstOrDefault();

            //    //decimal FI = (from F in db.FinancialInstitutionBranchMasters
            //    //              where F.FinancialInstituteId == sqlstr.FinancialInstitutionId
            //    //              select F).FirstOrDefault() ?? 0;

            //    BindCombo();
            //    return View(sqlstr);
            //}
            BindCombo();
            return View();
        }

        [HttpPost]
        public ActionResult AddEmployerAccountDetails(EmployerAccountMaster_Metadata EAM)
        {
            string msg;
            using (TransactionScope transaction = new TransactionScope())
            {
                try
                {
                    SaveEmployeeAccountDetails(EAM);
                    transaction.Complete();
                    msg = "Data saved successfully!";
                }
                catch (Exception)
                {
                    transaction.Dispose();
                    msg = "Data saved Failed!";
                }
            }
		        
            return Content("<script language='javascript' type='text/javascript'> alert('" + msg + "'); window.location.href ='/EmployerAccountMaster/AddEmployerAccountDetails'; </script>");
	      
           
            
            //BindCombo();
            //return View();
        }


        public void BindCombo()
        {
            IndividualEmiTransaction_Metadata objIndEmiTran = new IndividualEmiTransaction_Metadata();
            //ViewBag.LoanNoList = new SelectList((from ELD in db.EducationLoanDisbursments where ELD.NId = objIndEmiTran.NationalId select ELD).ToList(),"","");
            ViewBag.EmployerList = new SelectList(db.EmployerMasters, "Id", "EmployerName");
            ViewBag.AccountTypeList = new SelectList(db.AccountTypeMasters, "Id", "AccountType");
            //ViewBag.DistributionTypeList = new SelectList(db.DistributionTypeMasters, "Id", "ShortName");
            var NationalizeBank = (from e in db.FinancialInstitutionMasters where e.FinancialInstituteCategoryId == 1 select e);
            ViewBag.BankNameList = new SelectList(NationalizeBank, "Id", "Name");
            //ViewBag.BranchNameList = new SelectList(db.FinancialInstitutionBranchMasters, "Id", "BranchName");
        }

        public JsonResult BranchList(int bankId)
        {

            int BankID = bankId;

            var lstBranchList = (from s in db.FinancialInstitutionBranchMasters
                                 where s.FinancialInstituteId == BankID
                                 select s);

            return Json(new SelectList(lstBranchList.ToArray(), "Id", "BranchName"), JsonRequestBehavior.AllowGet);
        }

        public JsonResult DivisionList(int employerId)
        {

            int EmployerId = employerId;

            var lstEmployerDivisionList = (from s in db.EmployerDivisionMasters
                                            where s.EmployerId == EmployerId
                                            select s);

            return Json(new SelectList(lstEmployerDivisionList.ToArray(), "Id", "EmployerDivision"), JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveEmployeeAccountDetails(EmployerAccountMaster_Metadata EAMM)
        {
            string selectedBranch = Request.Form["selBranch"];
            int BranchId = Convert.ToInt32(selectedBranch);

            string selectedDivision = Request.Form["selEmployerDivision"];
            int DivisionId = Convert.ToInt32(selectedDivision);

            EmployerAccountMaster EAM = new EmployerAccountMaster();

            EAM.AccountName = EAMM.AccountName;
            EAM.AccountShortName = EAMM.AccountShortName;
            EAM.AccountType = EAMM.AccountType;
            //EAM.DistributionTypeId = EAMM.DistributionTypeId;
            EAM.EmployerId = EAMM.EmployerId;
            EAM.EmployerDivisionId = DivisionId;
            EAM.FinancialInstitutionId = EAMM.FinancialInstitutionId;
            EAM.FinancialInstitutionBranchId = BranchId;
            EAM.IFSCCode = EAMM.IFSCCode;
            EAM.Narration = EAMM.Narration;
            EAM.Status = true;
            EAM.AccountNo = EAMM.AccountNo;
            EAM.CreateId = Convert.ToInt32(Session["UserId"]);
            EAM.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
            db.AddToEmployerAccountMasters(EAM);
            db.SaveChanges();
            ModelState.Clear();
            return View("AddEmployerAccountDetails");
        }


        public string CheckAccNO(string accountNo)
        {
            string error="";

            var IsAccNoExist = db.EmployerAccountMasters.Where(u => u.AccountNo.Equals(accountNo)).FirstOrDefault();
            if (IsAccNoExist != null)
            {
                error = "Same account number already exist";
            }
            return error;
        }

        public string CheckAccName(string accountName)
        {
            string error = "";

            var IsAccNameExist = db.EmployerAccountMasters.Where(u => u.AccountName.Equals(accountName)).FirstOrDefault();
            if (IsAccNameExist != null)
            {
                error = "Same account name already exist";
            }
            return error;
        }

        public string CheckAccShortName(string accountShortName)
        {
            string error = "";

            var IsAccShortNameExist = db.EmployerAccountMasters.Where(u => u.AccountShortName.Equals(accountShortName)).FirstOrDefault();
            if (IsAccShortNameExist != null)
            {
                error = "Same short name for account already exist";
            }
            return error;
        }

        public ActionResult ViewEmployerAccount()
        {
            var EmployerAccount = (from EACC in db.EmployerAccountMasters
                                   join FI in db.FinancialInstitutionMasters on EACC.FinancialInstitutionId equals FI.Id 
                                   join FIBR in db.FinancialInstitutionBranchMasters on EACC.FinancialInstitutionBranchId equals FIBR.Id 
                                   select new EmployerAccountMaster_Metadata
                                   {
                                       AccountName=EACC.AccountName,
                                       AccountShortName=EACC.AccountShortName,
                                       AccountNo=EACC.AccountNo,
                                       BankName=FI.Name,
                                       BranchName=FIBR.BranchName,
                                       IFSCCode = EACC.IFSCCode,
                                       Id=EACC.Id
                                   }).ToList();
            return View(EmployerAccount);
        }

        [Authorize]
        public ActionResult EditEmployerAccount(int id)
        {

            var sqlstr = (from s in db.EmployerAccountMasters
                          where s.Id == id
                          select new EmployerAccountMaster_Metadata
                          {
                              EmployerId = s.EmployerId.Value,
                              EmployerDivisionId = s.EmployerDivisionId.Value,
                              //DistributionTypeId = s.DistributionTypeId.Value,
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

            var EmployerDivList = (from e in db.EmployerDivisionMasters where e.EmployerId == sqlstr.EmployerId select e);
            ViewBag.EmployerDivisionList = new SelectList(EmployerDivList, "Id", "EmployerDivision");
            //ViewBag.BranchNameList = (from e in db.FinancialInstitutionBranchMasters where e.FinancialInstituteId == sqlstr.FinancialInstitutionBranchId select e);
            BindCombo();
            return View(sqlstr);
        }

    }
}
