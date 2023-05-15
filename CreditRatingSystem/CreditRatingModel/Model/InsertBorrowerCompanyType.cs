using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreditRatingModel.Model
{
   public class InsertBorrowerCompanyType
    {
       public static decimal decCompTypeId { get; set; }
       creaditratingEntities CRE = new creaditratingEntities();

        public void AddBorrCompType(string UserName, string CompanyType, int CompanyId)
        {
            BorrowerCompanyTypeMaster BCTM = new BorrowerCompanyTypeMaster();
            BCTM.CompanyType = CompanyType;
            BCTM.CompanyId = CompanyId;
            BCTM.CreateDate = DateTime.UtcNow.Date;

            var intUserId = (from p in CRE.User_Details
                             where p.USERNAME == UserName
                     select
                     p.ID
                   ).FirstOrDefault();
            BCTM.CreateId = intUserId;
            CRE.AddToBorrowerCompanyTypeMasters(BCTM);
            CRE.SaveChanges();
           
            decCompTypeId=Convert.ToInt32( BCTM.Id);

        }        
    }    
}
