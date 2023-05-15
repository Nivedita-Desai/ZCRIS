using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreditRatingModel.Model
{
    public class InstituteBranch_CRUD_Operation
    {
        creaditratingEntities CRE = new creaditratingEntities();

        public void ADD_DETAILS(string BranchName, int CreateId, int FinancialInstituteId, string Address1, string Address2, string Address3, string FinancialInstituteContact, decimal CountryId, decimal StateId, decimal cityid, string pincode, string code, string FinancialInstituteEmailId1, string ContactPerson1, string ContactPerson1Mobile, string ContactPerson2, string ContactPerson2Mobile, string ContactPerson1EmailId, string ContactPerson2EmailId, List<int> CreditType, string BrCode)
        {
            FinancialInstitutionBranchMaster FIBM = new FinancialInstitutionBranchMaster();
            FIBM.BranchName = BranchName;
            FIBM.BranchCode = BrCode;
            FIBM.CreateDate =  DateTime.UtcNow.AddHours(5).AddMinutes(30);
            
            FIBM.CreateId = CreateId;
            FIBM.FinancialInstituteId = FinancialInstituteId;
            
            CRE.AddToFinancialInstitutionBranchMasters(FIBM);
            CRE.SaveChanges();

            FinancialInstitutionContactMaster FICM = new FinancialInstitutionContactMaster();
            FICM.FinancialInstituteBranchId = FIBM.Id;
            FICM.Address1 = Address1;
            FICM.Address2 = Address2;
            FICM.Address3 = Address3;
            //FICM.FinancialInstituteContact = FinancialInstituteContact; Institution Name
            FICM.CountryId = CountryId;
            FICM.StateId = StateId;
            FICM.ContactNo = FinancialInstituteContact;
            FICM.CityId = cityid;
            FICM.Pincode = pincode;
            FICM.Code = code;
            FICM.FinancialInstituteEmailId1 = FinancialInstituteEmailId1;
            FICM.ContactPerson1 = ContactPerson1;
            FICM.ContactPerson1Mobile = ContactPerson1Mobile;
            FICM.ContactPerson1EmailId = ContactPerson1EmailId;
            FICM.ContactPerson2 = ContactPerson2;
            FICM.ContactPerson2Mobile = ContactPerson2Mobile;
            FICM.ContactPerson2EmailId = ContactPerson2EmailId;
            FICM.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
  
            FICM.CreateId = CreateId;
            FICM.FinancialInstitutionId = FinancialInstituteId;

            CRE.AddToFinancialInstitutionContactMasters(FICM);
            CRE.SaveChanges();

            
            //int[] ints = CreditType.Split(',').Select(int.Parse).ToArray();
            
            //For Each item As ListItem In lbdsgn.Items
            //    If item.Selected Then
            //        message += "'" + item.Value + "'"
            //        message += ","
            //        i += 1
            //    End If
            //Next

            foreach (var item in CreditType)
            {
                FinancialInstitutionCreditTypeRelation FICTR = new FinancialInstitutionCreditTypeRelation();
                FICTR.FinancialInstituteId = FinancialInstituteId;
                FICTR.FinancialInstituteBranchId = FIBM.Id;
                FICTR.FinancialInstituteCreditTypeId = item;
                CRE.AddToFinancialInstitutionCreditTypeRelations(FICTR);
                CRE.SaveChanges();
            }

            //for (int i = 0; i < CreditType.Count; ++i)
            //{
            //    FICTR.FinancialInstituteId = FinancialInstituteId;
            //    FICTR.FinancialInstituteBranchId = FIBM.Id;
            //    FICTR.FinancialInstituteCreditTypeId = CreditType.IndexOf(i);
            //    CRE.AddToFinancialInstitutionCreditTypeRelations(FICTR);
            //}
            
            
        }

        public void AddDetailsChequeBounce(string Reason,int CreateId,int RId, DateTime TrDt, string pan,int FInstId, int FInstBrId, string chNo, DateTime chDt,decimal chAmt,string FoName,int chTypeId, string FAccNo,string TAccNo)
        {
            int ReasonId;
           
            if (Reason != null)
            {
                ReasonMaster RM = new ReasonMaster();
                RM.Reason = Reason;
                RM.CreateId = CreateId;
                RM.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
  
                CRE.AddToReasonMasters(RM);
                CRE.SaveChanges();
                ReasonId = RM.Id;
            }
            else
            {
                ReasonId = RId;
            }
            ChequeBounceTransaction CBT = new ChequeBounceTransaction();
            CBT.TransactionDate = TrDt;
            CBT.Pan = pan;
            CBT.FinancialInstituteId = FInstId;
            CBT.FinancialInstituteBranchId = FInstBrId;
            CBT.ChequeNo =chNo; 
            CBT.ChequeDate = chDt;
            CBT.ChequeAmount = chAmt;
            CBT.InFavourOf =FoName;
            CBT.ReasonId = ReasonId;
            CBT.ChequeTypeId = chTypeId;
            CBT.CreateId = CreateId;
            CBT.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
  
            CBT.FromAccountNo = FAccNo;
            CBT.ToAccountNo = TAccNo;
           // CBT.IndividualCustomerId = IndCustId;
            CRE.AddToChequeBounceTransactions(CBT);
            CRE.SaveChanges();
        }

        public void AddDetailsLoanDisbursedTransaction(int IndCustId, int LoanId, string LoanNo, DateTime LoanDt, DateTime LoanDisDt, decimal DisAmt, string LoanTypeId, string LoanAccNo, decimal FInstAmt, int NMonth, DateTime ExpDt, decimal EMI, decimal ROI, decimal IntTypeId, DateTime CCExpDT, decimal CashLimit, decimal CreditLimit, DateTime LAccDt, int CreateId, string LoanAccNo1, string LoanAccNo2, string LoanAccNo3, string LoanAccNo4, string Pan,DateTime FirstInstallmentDate, DateTime IssueDate)
        {
            LoanDisburseTransaction LDT = new LoanDisburseTransaction();
            //LDT.IndividualCustomerId = IndCustId;
            //LDT.LoanApplicationNo = LoanNo;
            //LDT.LoanApplicationDate = LoanDt;
            LDT.LoanDisburseDate = LoanDisDt;
            LDT.DisburseAmount = DisAmt;

            if (LoanTypeId == "C")
            {
                LDT.CreditCardExpiryDate = CCExpDT;
                LDT.CashLimit = CashLimit;
                LDT.CreditLimit = CreditLimit;
                LDT.IssueDate = IssueDate;

                //if (LoanAccNo1 != null)
                //{
                //    LDT.LoanAccountNo1 = LoanAccNo1;
                //}

                //if (LoanAccNo2 != null)
                //{
                //    LDT.LoanAccountNo2 = LoanAccNo2;
                //}
                //if (LoanAccNo3 != null)
                //{
                //    LDT.LoanAccountNo3 = LoanAccNo3;
                //}
                //if (LoanAccNo4 != null)
                //{
                //    LDT.LoanAccountNo4 = LoanAccNo4;
                //}
            }
            if (LoanTypeId =="L")
            {
                LDT.LoanAccountDate = LAccDt;
                LDT.LoanAccountNo = LoanAccNo;
                LDT.FirstInstallment = FInstAmt;
                LDT.Tenure = NMonth;
                LDT.ExpiryDate = ExpDt;
                LDT.Downpayment = EMI;
                LDT.RateOfInterest = ROI;
                LDT.InterestTypeId = IntTypeId;
                LDT.FirstInstallmentDate = FirstInstallmentDate; 
            }


            LDT.CreateId = CreateId;
            LDT.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);


            CRE.AddToLoanDisburseTransactions(LDT);
            CRE.SaveChanges();


            for (int i = 1; i <= NMonth; i++)
            {
                EMITransaction EMITrans = new EMITransaction();
                EMITrans.NoOfEMI = i;
                EMITrans.EMIAmount = EMI;
                EMITrans.Pan = Pan;
                EMITrans.LoanApplicationId = LoanId;
                EMITrans.LoanDisburseId = LDT.Id;
                //EMITrans.LoanDate = LoanDt;
                EMITrans.CreateId = CreateId;
                EMITrans.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
  
                CRE.AddToEMITransactions(EMITrans);
                CRE.SaveChanges();
            }

        }


        public void AddNewCollege_Master(string ColName, string Address1, string Address2, string Address3, decimal CountryId, decimal StateId, decimal cityID, string pincode, string code, string contactNo, string EmailId, string Colwebsite, string ContPerName, string ContPerMob, string ContPerEmail, int CreateID, string Designation)
        {
            CollegeMaster CM = new CollegeMaster();
            CM.CollegeName = ColName;
            CM.Address1 = Address1;
            CM.Address2 = Address2;
            CM.Address3 = Address3;
            CM.CountryId = CountryId;
            CM.StateId = StateId;
            CM.CityId = cityID;
            CM.Pincode = pincode;
            CM.ContactNo = code+"-"+ contactNo;
            CM.EmailId = EmailId;
            CM.WebSite = Colwebsite;
            CM.ContactPersonName = ContPerName;
            CM.ContactPersonMobile = code+"-"+ContPerMob;
            CM.ContactPersonEmailId = ContPerEmail;
            CM.Designation = Designation;
            CM.CreateId = CreateID;
            CM.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
  
            CRE.AddToCollegeMasters(CM);
            CRE.SaveChanges();

            //foreach (var item in CourseList)
            //{
            //    CourseCollegeRelation CCR = new CourseCollegeRelation();
            //    CCR.CollegeId = CM.Id;
            //    CCR.CourseId = item;
            //    CCR.CreateId = CreateID;
            //    CCR.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
  
            //    CRE.AddToCourseCollegeRelations(CCR);
            //    CRE.SaveChanges();
            //}
            
        }

        //public void AddEducationLoanDisbursedment(long NID, string SName, string SAdd, int CountryId, int StateId, string City, string pincode, DateTime DOB, string sex, string Code,string SContactNo, string SEmailId, int CourseID, int CollegeID, decimal DisbursedAmt, DateTime DisbursedDt, string LoanAccNo, DateTime LoanAccDt, int CreateId)
        //{
        //    EducationLoanDisbursment ELD = new EducationLoanDisbursment();

        //   // ELD.NId = NID;
        //    ELD.StudentName = SName;
        //    ELD.StudentAddress = SAdd;
        //    ELD.CountryId = CountryId;
        //    ELD.StateId = StateId;
        //    ELD.City = City;
        //    ELD.Pincode = pincode;
        //    ELD.DateOfBirth = DOB;
        //    ELD.Sex = sex;
        //    ELD.StudentContactNo =Code+'-'+SContactNo;
        //    ELD.StudentEmailId = SEmailId;
        //    ELD.CourseId = CourseID;
        //    ELD.CollegeId = CollegeID;
        //    ELD.CourseId = CourseID;
        //    ELD.DisburseAmount = DisbursedAmt;
        //    ELD.DisburseDate = DisbursedDt;
        //    ELD.LoanAccountNo = LoanAccNo;
        //    ELD.LoanAccountDate = LoanAccDt;
        //    ELD.CreateId = CreateId;
        //    ELD.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
  

        //    CRE.AddToEducationLoanDisbursments(ELD);
        //    CRE.SaveChanges();
        //}
    }
}
