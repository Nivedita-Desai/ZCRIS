using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using CreditRatingSystem;
using CreditRatingModel;
using CreditRatingModel.Model;
using CreditRatingSystem.Models;

namespace CreditRatingSystem.Controllers
{
    public class HomeController : Controller
    {
        private creaditratingEntities db = new creaditratingEntities();
        CommonModel AF = new CommonModel();
        // GET: /Home/
        [HttpGet]
        public ActionResult Login()
        {

            //login l = new login();
            DropList();
            //ViewBag.LoginError = "";
            return View();
        }

        public void DropList()
        {
           // ViewBag.UserTypes = new SelectList(db.UserTypeMasters.ToList(), "Id", "UserType");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]

        public ActionResult Login(login l, string ReturnUrl = "")
        {
            

            if (ModelState.IsValid)
            {
                string username = l.USERNAME.ToString();
                //string passwd = AF.Encrypt(l.PASSWORD.ToString()); // l.PASSWORD.ToString(); 
                //Nivedita 18-01-2022
                string passwd = l.PASSWORD.ToString(); 

                //if (username.Trim().Length == 0 && passwd.Trim().Length == 0)
                //{
                //    ViewBag.LoginError = "Username and Password is required to Login";
                //    return View();
                //}
                
                var user = db.User_Details.Where(u => u.USERNAME.Equals(l.USERNAME) && u.PASSWORD.Equals(passwd) && u.Active==true).FirstOrDefault();

                if (user != null)
                {
                    int UserTypeId = Convert.ToInt32(user.USER_TYPE_ID);
                    Session["UserTypeId"] = user.USER_TYPE_ID;
                    Session["UserId"] = user.ID;
                    Session["Username"] = user.NAME;
                    Session["UserLoginName"] = user.USERNAME;
                    Session["EmployerName"] = user.EmployerId;
                    Session["EmployerDivision"]=user.EmployerDivisionId;
                    Session["IsFirstLogin"] = user.IsFirstLogin;
                    Session["EmployerDivisionBranch"] = user.EmployerBranchId;
                    Session["DistributionType"] = user.DistributionTypeId;
                    //string encryptPass = AF.Encrypt(user.PASSWORD);
                    //string decryptPass = AF.Decrypt(encryptPass);
                    FormsAuthentication.SetAuthCookie(user.USERNAME, false);
                    var userType = db.UserTypeMasters.Where(ut => ut.Id.Equals(UserTypeId)).FirstOrDefault();
                    if (userType != null)
                    {
                        Session["UserType"] = userType.UserTypeShortName.Trim();
                    }

                    var authTicket = new FormsAuthenticationTicket(1, user.USERNAME,
                                       DateTime.Now, DateTime.Now.AddMinutes(3), true, "");
                    //FormsAuthentication.SetAuthCookie(UserName, false);
                    string cookieContents = FormsAuthentication.Encrypt(authTicket);
                    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, cookieContents)
                    {
                        Expires = authTicket.Expiration,
                        Path = FormsAuthentication.FormsCookiePath
                    };
                    // LoginAttemptEntry(true, Convert.ToInt32(user.ID));

                    if (Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        //return RedirectToAction("Company", "BorrowerCompany");
                        if (user.IsFirstLogin == true)
                        {
                            return RedirectToAction("userchangePassword", "UserRegistration");
                        }
                        else
                        {
                            return View("_Home");
                        }
                        
                    }
                }
                else
                {
                    ViewBag.LoginError = "The user name or password provided is incorrect";
                }
            }
            else
            {
                ViewBag.LoginError = "Username and Password is required to Login";
            }
            ModelState.Remove("PASSWORD");

            DropList();
            return View();
        }

        //[HttpPost]
        public string LoginValidation(string username, string passwd)
        {
            //CommonModel AF = new CommonModel();

            if (passwd != "")
            {
                // string username = username.ToString();
                //string passwd = passwd.ToString();
                //Nivedita 18-01-2022
                //string passwd1 = AF.Encrypt(passwd.ToString());
                string passwd1 = passwd.ToString();
                var user = db.User_Details.Where(u => u.USERNAME.Equals(username) && u.PASSWORD.Equals(passwd1) && u.Active==true).FirstOrDefault();
                if (user != null)
                { }
                else
                {
                    ViewBag.LoginError = "The user name or password provided is incorrect";
                }
            }
          
            return ViewBag.LoginError;
        }

        public ActionResult LogOut()
        {
            var Username = Session["Username"];
            if (string.IsNullOrEmpty(Convert.ToString(Username)))
                return View("Index");
            //userloginlogs_trn ObjUserLonginLog = new userloginlogs_trn();

            //ObjUserLonginLog.UserLoginLogId = Convert.ToInt64(Session["loginlogId"]);
            //ObjUserLonginLog.UserId = Convert.ToInt64(Session["userid"]);
            // ObjUserLonginLog.LogoutDateTime = DateTime.UtcNow;
            //var UserLastlogin = db.userloginlogs_trn.Single(u => u.UserLoginLogId == ObjUserLonginLog.UserLoginLogId);
            //UserLastlogin.LogoutDateTime = ObjUserLonginLog.LogoutDateTime;
            //db.SaveChanges();

            FormsAuthentication.SignOut();
            HttpContext.Session.RemoveAll();
            HttpContext.Session.Abandon();
            return RedirectToAction("Login", "Home");
        }

        public ActionResult afterlogin()
        {
            return View();
        }
        public ActionResult LoginFailed()
        {
            return View();
        }

        public ActionResult Registration(login l)
        {
            using (creaditratingEntities db = new creaditratingEntities())
            {
                if (ModelState.IsValid)
                {
                    InsertInfo(l.Name,l.RegUsername, l.RegPassword, 3);
                    return Content(("<script language='javascript' type='text/javascript'> alert('Record save successfully!'); window.location.href ='/Home/Login';  </script>"));
                }
                
                ModelState.Clear();
                DropList();

                //return RedirectToAction("Registration");
            }
            
            return View("Login");
        }
        private void InsertInfo(string Name, string Username, string Password, int UserType)
        {
           // if ((doesUserNameExist(Username))!="")
            //{
                Add(Name, Username, Password, UserType);
           // }
        }
        public void Add(string Name, string Username, string Password, int UserType)
        {
            //UserAntiForgery AF = new UserAntiForgery();
            var CreateUserId1 = Session["UserId"];
            int CreateUserId = Convert.ToInt32(CreateUserId1);
            User_Details UD = new User_Details();
            UD.NAME = Name;
            UD.USERNAME = Username;
            //UD.PASSWORD = AF.Encrypt(Password);
            //Nivedita 18-01-2022
            UD.PASSWORD = Password;
            UD.USER_TYPE_ID = UserType;
            UD.CreateDate = DateTime.UtcNow.Date;
            UD.CreateId = CreateUserId;
            UD.IsFirstLogin = false;
            UD.Active = true;
            db.AddToUser_Details(UD);
            db.SaveChanges();

            IndividualCustomerMaster ICM = new IndividualCustomerMaster();
            ICM.Pan = Username;
            db.AddToIndividualCustomerMasters(ICM);
            db.SaveChanges();
            ViewBag.LoginError = "Data Saved";
        }

        
        public string doesUserNameExist(string UserName)
        {
            //UserName = UserName.ToUpper();
            var user = db.User_Details.Where(u => u.USERNAME.Equals(UserName) && u.Active==true).FirstOrDefault();
            if (user != null)
            {
                UserName ="" ;
                //ViewBag.LoginError = "Username already Exist";
                UserName = "Username already Exist";
            }
            else
            {
               
            }
            return UserName;// UserName;
        }
        public ActionResult _Home()
        {
            return View("_Home");
        }
        //public ActionResult Index()
        //{
        //    ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

        //    return View();
        //}

        //public ActionResult About()
        //{
        //    ViewBag.Message = "Your app description page.";

        //    return View();
        //}

        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Your contact page.";

        //    return View();
        //}
    }
}
