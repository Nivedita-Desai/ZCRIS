using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CreditRatingModel.Model
{
   public class ChequeClearance
    {
       public int Id { get; set; }
       public string NRC { get; set; }
       public string ChequeNo { get; set; }
       public string SaveChequeClearance { get; set; }
       public int CheckId { get; set; }
       public DateTime CheckClearanceDate { get; set; }
    }
}
