using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CreditRatingModel.Model
{
    public class EmployeeMonthlySalary
    {
        //table MonthlySalaryHead
        public int Id { get; set; }
        public int UserId { get; set; } 
        public int EmployerId { get; set; } 
        public int EmployerTypeId { get; set; } 
        public int EmployerBranchId { get; set; }
        public string CEmployeeId { get; set; }
        public int MonId { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MMM-yyyy}")]
        public DateTime SalaryMonth{ get; set; }

        //table MonthlySalaryTransaction
        public int MonSalTranId { get; set; } 
        public string NID { get; set; } 
        public int EmployeeId { get; set; }        
        public int DepartmentId { get; set; }
        public decimal Salary { get; set; } 
        public int SlabId { get; set; }
        public decimal EMIAmount { get; set; }
        public decimal TotEMIAmount { get; set; }
        public decimal TotalLoanAmount { get; set; }
        public decimal TotalBalanceAmount { get; set; }
        public string Status { get; set; }

        public string EmpName { get; set; }
        public string DeptName { get; set; }
        public string Slab { get; set; } 

        public DateTime CreateDate { get; set; }
        public int CreateId { get; set; }
        public DateTime EditDate { get; set; }
        public int EditId { get; set; }

        //company wise monthly salary
        public int CompMonId { get; set; }
        public int EmpDivisionid { get; set; }    
        public int EmpBranchId { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MMM-yyyy}")]
        public DateTime DistMonth { get; set; }    

        public string DistType { get; set; }    
        public int DistTypeId { get; set; }
        public int Percentage { get; set; }
        public decimal Amount { get; set; }
        public string AreaName { get; set; }
        public int AreaId { get; set; }
        public string BranchName { get; set; }
        public int BranchId { get; set; }
        public string BankName { get; set; }
        public int BankId { get; set; }
        public string AccName { get; set; }
        public int AccId { get; set; }
        public string AccountNo { get; set; }
        public int EmpAccId { get; set; }
        public string EmpAccName { get; set; }
        public string PercentageCol1 { get; set; }
        public string PercentageCol2 { get; set; }
    }

    public class SaveEmpMonSalData
    {
        public string SaveMonSalData { get; set; }        
        public string SaveHeadSalData { get; set; }

        public string SaveAppSalData { get; set; }

        public string SaveDistSalHeadData { get; set; }
        public string SaveDistSalChildData { get; set; }

        public string SaveCRSAppData { get; set; }        
    }
}
