using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using  System.ComponentModel.DataAnnotations;
//using System.Web.Mvc;


namespace CreditRatingModel.Model
{
        public class login
        {
            public int ID { get; set; }
            //[Required(ErrorMessage="Please enter username ",AllowEmptyStrings=false)]
           // [Remote("doesUserNameExist", "Home",HttpMethod="POST", ErrorMessage = "Username already exist")]
            public string USERNAME { get; set; }

           // [Required(ErrorMessage = "Please enter password ",AllowEmptyStrings=false)]
           // [DataType(DataType.Password)]
            public string PASSWORD { get; set; }

            //[DataType(DataType.Password)]
            //[Compare("PASSWORD", ErrorMessage = "Password and Confirm password not same")]
            public string ConfirmPasswd { get; set; }

              public string UserTypeShortName { get; set; }

             // public int Id { get; set; }
             public string RegUsername { get; set; }
              public string RegPassword { get; set; }
              public int UserTypeID { get; set; }
              public string Name { get; set; }
        }
    }

