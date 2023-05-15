using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CreditRatingModel.Model;
using System.Data.Objects;
using System.Data.SqlClient;


using System.Net;
using System.Net.Mail;
using CreditRatingSystem.Models;

namespace CreditRating.Controllers
{
    public class UserRegistrationController : Controller
    {
        //
        // GET: /Reg/
        private creaditratingEntities db = new creaditratingEntities();
        CommonModel AF = new CommonModel();
        public void DropList(Reg R)
        {
            var UserType = (from e in db.UserTypeMasters where e.UserTypeShortName != "CU" select e);
            ViewBag.UserType = new SelectList(UserType.ToList(), "Id", "UserType");
            ViewBag.EmployerList = new SelectList(db.EmployerMasters.ToList(), "Id", "EmployerName");
            ViewBag.DivisionList = new SelectList(db.EmployerDivisionMasters.ToList(), "Id", "EmployerDivision");
            ViewBag.CollegeList = new SelectList(db.CollegeMasters.ToList(), "Id", "CollegeName");
            ViewBag.FinantialInstituteList = new SelectList(db.FinancialInstitutionMasters.ToList(), "Id", "Name");
            ViewBag.FinantialInstituteBranchList = new SelectList(db.FinancialInstitutionBranchMasters.ToList(), "Id", "BranchName");
            ViewBag.AdminTypeList = new SelectList(db.DistributionTypeMasters.ToList(), "Id", "Name");
        }
        public ActionResult Index()
        {
            return View();
        }
         
        [Authorize]
        [HttpGet]
        public ActionResult Registration()
        {
            Reg R=new Reg();
            DropList(R);
            return View();
        }

        [HttpPost]
        public ActionResult Registration(Reg R)
        {
            string msg = "";
            try
            {
                AddRegistration(R);
                msg = "Data Saved Successfully!";
            }
            catch (Exception)
            {
                msg = "Data Saved Failed!";
                //throw;
            }
            
            return Content("<script language='javascript' type='text/javascript'> alert('" + msg + "'); window.location.href ='/UserRegistration/Registration'; </script>");
        }

         public JsonResult EmployerDivisionList(int EmpId)
         {

             int EmployerId = EmpId;

             var lstDivisionList = (from s in db.EmployerDivisionMasters
                                  where s.EmployerId == EmployerId
                                  select s);

             return Json(new SelectList(lstDivisionList.ToArray(), "Id", "EmployerDivision"), JsonRequestBehavior.AllowGet);
         }

         public JsonResult EmployerDivisionBranchList(int EmpId, int divId)
         {

             int EmployerId = EmpId;
             int EmployerDivId = divId;

             var lstDivisionBranchList = (from s in db.EmployerBranchMasters
                                    where s.EmployerId == EmployerId && s.EmployerDivisionId==divId
                                    select s);

             return Json(new SelectList(lstDivisionBranchList.ToArray(), "Id", "BranchName"), JsonRequestBehavior.AllowGet);
         }

         public JsonResult FinantialInstituteBranchList(int EmpId)
         {

             int EmployerId = EmpId;

             var lstDivisionList = (from s in db.FinancialInstitutionBranchMasters
                                    where s.FinancialInstituteId == EmployerId
                                    select s);

             return Json(new SelectList(lstDivisionList.ToArray(), "Id", "BranchName"), JsonRequestBehavior.AllowGet);
         }

        //[HttpPost]
        //public ActionResult btnAddClick(Reg R)
        //{
            
        //}

         public string CheckNid(string Nid)
         {
             string error = "";

             try
             {
                 var IsNidExist = db.User_Details.Where(u => u.USERNAME.Equals(Nid) && u.Active == true).FirstOrDefault();
                 if (IsNidExist != null)
                 {
                     error = "Same NID already exist";
                 }
             }
             catch (Exception)
             {
                 error = "Same NID already exist";
                 //throw;
             }
            
             return error;
         }
       
        public ActionResult AddRegistration(Reg R)
        {
            
            //UserAntiForgery AF = new UserAntiForgery();
            string selectedFinantialBranch = Request.Form["selFinantialInstituteBranch"];
            int BranchId = Convert.ToInt32(selectedFinantialBranch);

            string selectedEmployerDivision = Request.Form["selDivisionList"];
            int DivisionId = Convert.ToInt32(selectedEmployerDivision);

            string selectedDivBranchList = Request.Form["selDivisionBranchList"];
            int DivBranchId = Convert.ToInt32(selectedDivBranchList);

            string Password = "password1";
            //string strUserType = "";

            ObjectParameter objParam = new ObjectParameter(Password, typeof(string));

            var i = db.usp_RandomPassword(objParam);


            var pass = objParam.Value.ToString();
            

            var CreateUserId1 = Session["UserId"];
            int CreateUserId = Convert.ToInt32(CreateUserId1);
            User_Details UD = new User_Details();
            UD.USERNAME = R.Username;
            //Nivedita 18-01-2022
            UD.PASSWORD = pass;
            //UD.PASSWORD = AF.Encrypt(pass);
            UD.NAME = R.Name;
            UD.USER_TYPE_ID = R.UserType;

            if (R.UserType == 1)
            {
                if (R.DistributionTypeId != null)
                {
                    UD.DistributionTypeId = R.DistributionTypeId;
                }
            }
            if (R.UserType==2)
            {
                //strUserType = "Financial Institute";
                UD.EmployerId = Convert.ToInt32(R.FinantialInstituteId);
                UD.EmployerDivisionId = BranchId; // Convert.ToInt32(R.FinantialInstituteBranchId);
            }
            else if (R.UserType==4)
            {
                //strUserType = "Employer";
                UD.EmployerId = R.employerId;
                if (DivisionId != 0)//(R.employerDivisionId != 0)
                {
                    UD.EmployerDivisionId = DivisionId; // R.employerDivisionId;
                }
                if (DivBranchId != 0)//(R.employerDivisionId != 0)
                {
                    UD.EmployerBranchId = DivBranchId;
                }
                
            }
            else if (R.UserType == 5)
            {
                //strUserType = "College";
                UD.EmployerId = R.collegeId;
            }
            //else if (R.UserType == 6)
            //{
            //    //strUserType = "College";
            //    //UD.EmployerId = R.collegeId;
            //}
            UD.Active = true;
            UD.CountryCode = R.countryCode;
            UD.ContactNo = R.ContactNo;
            UD.EmailId = R.EmailId;
            UD.IsFirstLogin = true;
            UD.CreateDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
            UD.CreateId = CreateUserId;

            db.AddToUser_Details(UD);
            db.SaveChanges();
            //mailSendForRegistration(R);
            string body = "<html><body><br/><div>Dear " + R.Name + ",<br/><br/> </div><div>Welcome to CRSS System and thanks for registering with us, below are your credentials. You are requested to change your password on first login.</div><br/><div>Login ID/Name:<b> " + R.Username + " </b></div><div>Password (One time):<b> " + pass + " </b></div><br/>Thanks & Regards,<br/>CRS Support Team.</body></html>";

            SendMail(R.EmailId, "Registration Confirmation", body, "", "", "");
            return View("Registration");
        }

       

        public void SendMail(string recipient, string subject, string body, string AttachmentFiles = null, string cc = "", string bcc = "")
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com");

            client.Port = 587;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("crs.softlabsgroup@gmail.com", "M1th1le$h");
           // System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["NetworkCredential_UserId"].ToString(), ConfigurationManager.AppSettings["NetworkCredential_Password"].ToString());
            client.EnableSsl = true;
            client.Credentials = credentials;

            try
            {
                var mail = new MailMessage();
                string from = "";

                from = "crs.softlabsgroup@gmail.com"; //ConfigurationManager.AppSettings["NetworkCredential_UserId"].ToString();

                if (!string.IsNullOrEmpty(from.Trim()))
                {
                    mail.From = new MailAddress(from);
                }

                if (!string.IsNullOrEmpty(recipient.Trim()))
                {
                    mail.To.Add(recipient);
                }

                if (!string.IsNullOrEmpty(cc.Trim()))
                {
                    mail.CC.Add(cc);
                }
                if (!string.IsNullOrEmpty(bcc.Trim()))
                {
                    mail.Bcc.Add(bcc);
                }


                mail.Subject = subject;
                mail.IsBodyHtml = true;
                mail.Body = body;

                if (!string.IsNullOrEmpty(AttachmentFiles))
                {
                    foreach (string a in AttachmentFiles.Split(new char[] { ',' }))
                    {
                        if (!string.IsNullOrEmpty(a))
                        {
                            Attachment att = new Attachment(a);
                            mail.Attachments.Add(att);
                        }
                    }
                }

                client.Send(mail);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
        }

        [HttpGet]
        public ActionResult userchangePassword()
        {

            return View();
        }

        [HttpPost]
        public ActionResult userchangePassword(Reg R)
        {
            //UserAntiForgery AF = new UserAntiForgery();
            var userId = Session["UserId"];

            int CreateUserId = Convert.ToInt32(userId);
            var user = db.User_Details.Where(u => u.ID == CreateUserId && u.Active == true).FirstOrDefault();
            if (user != null)
            {
                //User_Details UD = new User_Details();
                //Nivedita 18-01-2022
                //user.PASSWORD = AF.Encrypt(R.newPassword);
                user.PASSWORD = R.newPassword;
                user.IsFirstLogin = false;
                user.EditId = CreateUserId;
                user.EditDate = DateTime.UtcNow.AddHours(5).AddMinutes(30);
                db.SaveChanges();
            }
            return Content("<script language='javascript' type='text/javascript'> alert('Password Changed Successfully!'); window.location.href ='/Home/LogOut'; </script>");
        }

        public string CheckValidPass(string Pass)
        {
            string error = "";
            //UserAntiForgery AF = new UserAntiForgery();
            string username = Session["UserLoginName"].ToString();
            //Nivedita 18-01-2022
            //string pass1 = AF.Encrypt(Pass);
            string pass1 = Pass;
            
            //var IsNidExist = db.User_Details.Where(u => u.USERNAME.Equals(Nid)).FirstOrDefault();
            var user = db.User_Details.Where(u => u.USERNAME.Equals(username) && u.PASSWORD.Equals(pass1) && u.Active == true).FirstOrDefault();
            if (user == null)
            {
                error = "Invalid Password";
            }
            return error;
        }


    }
}
