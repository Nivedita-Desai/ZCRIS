using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc.Ajax;



using System.Xml;
using System.Configuration;
using System.Web;


namespace CreditRatingModel.Model
{
   public class installment
   {
       creaditratingEntities CRE = new creaditratingEntities();

       public static int LoanDisburseId { get; set; }
       public static string tempid { get; set; }
       public static int PaymentAmount { get; set; }
       public static int ReceivedAmount { get; set; }
       public static DateTime InstallmentDate { get; set; }

        public int eId { get; set; }
        public int paymentamt { get; set; }
        public int receivedamt { get; set; }
        public Nullable<DateTime> installmentdate { get; set; }
    
    
        public void upload()
        {

            InstallmentTransaction IT = new InstallmentTransaction();
            IT.PaidAmount = PaymentAmount;
            IT.ReceivedAmount = ReceivedAmount;
            IT.InstallmentDate = InstallmentDate;
            CRE.AddToInstallmentTransactions(IT);
            CRE.SaveChanges();
             
         
            EMITransaction et = new EMITransaction();
            List<SelectListItem> update = new List<SelectListItem>();
              
        var   update1 = (from u in CRE.EMITransactions where u.LoanDisburseId == LoanDisburseId select new { u.Id }).ToList();
         foreach (var item in update1)
         {
  

         var emi = CRE.EMITransactions.Where(x => x.Id == item.Id).FirstOrDefault();
         
           emi.InstallmentId = IT.Id;
             CRE.SaveChanges();
            
         }
       
   
            var m = (from p in CRE.TempInstallmentTransactions
                     where p.TempSessionId == tempid
                     select p

                       ).FirstOrDefault();
           CRE.DeleteObject(m);
            CRE.SaveChanges();

        }
            
    }
}
