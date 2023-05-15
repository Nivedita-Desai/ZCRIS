using CreditRatingModel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CreditRating.Controllers
{
    public class ChequeBounceController : Controller
    {
        //
        // GET: /ChequeBounce/
        private creaditratingEntities db = new creaditratingEntities();
        int FInstBankId, FInstBrId;
  
        [Authorize]
        public ActionResult AddCheqeBounce()
        {
            ChequeBounce_Metadata objChequeBounce_Metadata = new ChequeBounce_Metadata();
            dropList(objChequeBounce_Metadata);
            return View(objChequeBounce_Metadata);
        }

        [HttpPost]
        public ActionResult Index(ChequeBounce_Metadata CBM)
        {
           // ChequeBounce_Metadata objChequeBounce_Metadata = new ChequeBounce_Metadata();

            //int a = CBM.ReasonId;

            //if (a == 0)
            //{
            //    //ReasonId = 0;
            //}

            if (ModelState.IsValid)
            {
                btnAddClick(CBM);
            }
            dropList(CBM);
            ModelState.Clear();
            return View(CBM);
        }
        public void dropList(ChequeBounce_Metadata objChequeBounce_Metadata)
        {
            ViewBag.CheckTypeList = new SelectList(db.ChequeTypeMasters.ToList(), "Id", "ChequeType");
            //ViewBag.ReasonList = new SelectList(db.ReasonMasters.ToList(), "Id", "Reason");

            List<SelectListItem> ReasonListfromDB = new List<SelectListItem>();
            try
            {
                var ReasonList1 = (from c in db.ReasonMasters
                                  select c).ToList();

                if (ReasonList1 != null)
                {
                    //ReasonListfromDB.Add(new SelectListItem { Text = "Select Reason", Value = "0" });
                    foreach (var item in ReasonList1)
                    {
                        ReasonListfromDB.Add(new SelectListItem { Text = item.Reason, Value = item.Id.ToString() });
                    }
                }
                ReasonListfromDB.Add(new SelectListItem { Text = "Others", Value = (ReasonList1.Count + 1).ToString() });
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            ViewBag.ReasonList = new SelectList (ReasonListfromDB.ToList(),"Value","Text");
            
        }

        public string RetrieveBankCode(string strBankCode)
        {
            //string strBankCode = strBankCode;
            //bool result;
            //result = false;
            string BankName="";
            var BankExist = db.FinancialInstitutionMasters.Where(FI => FI.BankCode.Equals(strBankCode)).FirstOrDefault();
            if (BankExist != null)
            {
                BankName = BankExist.Name;
                FInstBankId = Convert.ToInt32(BankExist.Id);
                //ViewBag.BankId = FInstBankId;
                //result = true;
            }
            //else
            //{
            //    result = false;
            //}
           
            return BankName;
        }

        public string RetrieveBrCode(string strBrCode)
        {
            //string strBankCode = strBankCode;
            //bool result;
            //result = false;
            string BrName = "";
            var BrExist = db.FinancialInstitutionBranchMasters.Where(FIB => FIB.BranchCode.Equals(strBrCode)).FirstOrDefault();
            if (BrExist != null)
            {
                BrName = BrExist.BranchName;
                FInstBrId= Convert.ToInt32(BrExist.Id);
                //ViewBag.BrId = FInstBrId;
                //result = true;
            }
            //else
            //{
            //    result = false;
            //}

            //return result;
            return BrName;
        }

        private void InsertInfo(string Reason, int CreateId, int RId, DateTime TrDt, string pan, int FInstId, int FInstBrId, string chNo, DateTime chDt, decimal chAmt, string FoName, int chTypeId, string FAccNo, string TAccNo)
        {
            InstituteBranch_CRUD_Operation objInstituteBranch_CRUD_Operation = new InstituteBranch_CRUD_Operation();

            objInstituteBranch_CRUD_Operation.AddDetailsChequeBounce(Reason,CreateId,RId,TrDt,pan,FInstId,FInstBrId,chNo,chDt,chAmt,FoName,chTypeId,FAccNo,TAccNo);

        }
        [HttpPost]
        public ActionResult btnAddClick(ChequeBounce_Metadata CBM)
        {
           
            if (ModelState.IsValid)
            {
                int CreateUserId = Convert.ToInt32(Session["UserId"]);

                string selectedState = Request.Form["selState"];
                int stateId1 = Convert.ToInt32(selectedState);
                var BankId1 =( from s in db.FinancialInstitutionMasters
                          where s.BankCode == CBM.BankCode
                          select s.Id).FirstOrDefault();
                  
                var  BrId1  = (from s in db.FinancialInstitutionBranchMasters
                          where s.BranchCode == CBM.BranchCode
                          select s.Id).FirstOrDefault();

                int BankId =Convert.ToInt32(BankId1);
                int BrId = Convert.ToInt32(BrId1);

                InsertInfo(CBM.Reason, CreateUserId, CBM.ReasonId, CBM.TransDate, CBM.Pancard, BankId, BrId, CBM.ChequeNo, CBM.ChequeDate, CBM.ChequeAmount, CBM.FavourOfName, CBM.ChTypeId, CBM.FavourAccountNo, CBM.FrAccNo);
                ViewBag.Message = "Data Saved";
                ModelState.Clear();
                // dropList(FICM);
                //  return RedirectToAction("Index");
                //}
                // else
                //{

            }
           // dropList(FICM);
            return View("Index");

        }

        //-----------------------Darshana-------------------

        public ActionResult ViewChequeBounce()
        {
            var strSql = (from CBT in db.ChequeBounceTransactions
                          join FM in db.FinancialInstitutionMasters on CBT.FinancialInstituteId equals FM.Id
                          join FBM in db.FinancialInstitutionBranchMasters on CBT.FinancialInstituteBranchId equals FBM.Id
                          join RM in db.ReasonMasters on CBT.ReasonId equals RM.Id
                          join CTM in db.ChequeTypeMasters on CBT.ChequeTypeId equals CTM.Id
                          select new
                          {
                              CBT.Id,
                              CBT.Pan,
                              FM.Name,
                              FM.BankCode,
                              FBM.BranchName,
                              FBM.BranchCode,
                              CBT.ChequeNo,
                              CBT.ChequeDate,
                              CBT.ChequeAmount,
                              CBT.InFavourOf,
                              CBT.FromAccountNo,
                              CBT.ToAccountNo,
                              RM.Reason,
                              CTM.ChequeType,
                              CBT.TransactionDate
                          }
                            ).ToList();

            return View(strSql);
        }
        //--------------------------------------------------

    }
}
