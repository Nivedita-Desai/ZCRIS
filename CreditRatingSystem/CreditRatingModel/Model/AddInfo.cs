using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc.Ajax;
using System.Web;
namespace CreditRatingModel.Model
{
   public class AddInfo
   {
       public string USERNAME { get; set; }
       //[Required(ErrorMessage = "Address1 is required field.")]
       //[DataType(DataType.Text)]
       //[StringLength(100)]
       public string Address1 { get; set; }
       //[Required(ErrorMessage = "Address2 is required field.")]
       //[DataType(DataType.Text)]
       //[StringLength(100)]
       public string Address2 { get; set; }
       //[Required(ErrorMessage = "Address3 is required field.")]
       //[DataType(DataType.Text)]
       //[StringLength(100)]
       public string Address3 { get; set; }


       public List<SelectListItem> AddressType { get; set; }
       public int Addressid { get; set; }

       //[Required(ErrorMessage = "Country is required field.")]
       public int CountryId { get; set; }

       //[Required(ErrorMessage = "Select State ")]
       public int StateId { get; set; }

       //[Required(ErrorMessage = "Enter city ")]
       public string City { get; set; }

       public string Pincode { get; set; }


       public string LandlineCode { get; set; }


       public string LandlineNo { get; set; }

       public string MobileCode { get; set; }

       //[Required(ErrorMessage = " Person Contact Number is required field.")]
       //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Enter valid Contact number")]
       public string MobileNo { get; set; }

       //[RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Invalid email")]
       //[Required(ErrorMessage = "Email Id is required field.")]
       public string EmailId1 { get; set; }

       //[RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Invalid email")]
       //[Required(ErrorMessage = "Email Id is required field.")]
       public string EmailId2 { get; set; }

       creaditratingEntities CRE = new creaditratingEntities();
          public void Addinfo(string USERNAME, string Address1, string Address2, string Address3,int Addressid, int CountryId, int StateId, string City, string Pincode, string LandlineCode, string LandlineNo, string MobileCode, string MobileNo, string EmailId1, string EmailId2)
          { 
              
              InsertInfo i =new InsertInfo();
           
              ContactDetailsMaster CD = new ContactDetailsMaster();
              CD.Address1 = Address1;
              CD.Address2 = Address2;
              CD.Address3 = Address3;
              CD.CountryId = CountryId;
              CD.PAN = InsertInfo.pan;
              CD.StateId = StateId;
              //CD.City = City;
              CD.Pincode = Pincode;
              CD.LandlineCode = LandlineCode;
              CD.LandlineNo = LandlineNo;
              CD.MobileCode = MobileCode;
              CD.MobileNo = MobileNo;
              CD.EmailId1 = EmailId1;
              CD.EmailId2 = EmailId2;
              CD.CreateDate = DateTime.UtcNow.Date;
              var N = (from p in CRE.User_Details
                       where p.USERNAME == USERNAME
                       select
                       p.ID
            ).FirstOrDefault();
              CD.CreateId = N;
              if (InsertInfo.companyName == null)
              {
                  CD.IndividualCustomerId = InsertInfo.individual;
                  CD.BorrowerBranchId = null;
              }
              else {
                  CD.IndividualCustomerId = null;
                  CD.BorrowerBranchId = InsertInfo.branch;
              }
              CRE.AddToContactDetailsMasters(CD);
              CRE.SaveChanges();

          

              ContactRelation CR = new ContactRelation();
              if (InsertInfo.companyName == null)
              {

                  CR.IndividualCustomerId = InsertInfo.individual;
                  CR.BorrowerId = null;
              }
              else
              {
                  CR.IndividualCustomerId = null;
                  CR.BorrowerId = InsertInfo.borrowerid;
              }
              CR.AddressTypeId = Addressid;
              
           
              CR.ContactDetailsId=CD.Id;
              CR.CreateDate = DateTime.UtcNow.Date;
                var M = (from p in CRE.User_Details
                          where p.USERNAME == USERNAME
                          select
                          p.ID
              ).FirstOrDefault();
                CR.CreateId = M;
                CRE.AddToContactRelations(CR);
                CRE.SaveChanges();

          }

    }
}
