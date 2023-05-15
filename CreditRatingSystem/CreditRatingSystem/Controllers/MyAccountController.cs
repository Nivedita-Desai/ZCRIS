using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CreditRatingModel;
using CreditRatingModel.Model;
using System.Web.Security; 

namespace CreditRating.Controllers
{
    public class MyAccountController : Controller
    {
        //
        // GET: /MyAccount/
        creaditratingEntities db = new creaditratingEntities();

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(login l,string ReturnUrl = "")
        {
            if (ModelState.IsValid)
            {
                string username = l.USERNAME.ToString();
                string passwd = l.PASSWORD.ToString();
                if (username.Trim().Length == 0 && passwd.Trim().Length == 0)
                {
                    ViewBag.LoginError = "Username and Password is required to Login";
                    return View();
                }

                var user = db.User_Details.Where(u => u.USERNAME.Equals(l.USERNAME) && u.PASSWORD.Equals(l.PASSWORD)).FirstOrDefault();

                if (user != null)
                {
                    Session["UserId"] = user.ID;
                    Session["Username"] = user.NAME;
                    FormsAuthentication.SetAuthCookie(user.USERNAME, false);
                    var userType = db.UserTypeMasters.Where(ut => ut.Id.Equals(user.USER_TYPE_ID)).FirstOrDefault();
                    if (userType != null)
                    {
                        Session["UserType"] = userType.UserTypeShortName;
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
                        return RedirectToAction("Company", "BorrowerCompany");
                    }
                }
                else
                {
                    ViewBag.LoginError = "The user name or password provided is incorrect";
                }
            }
            ModelState.Remove("PASSWORD");
            return View();
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

        //public void LoginAttemptEntry(bool Flag, int ID)
        //{

        //}

    }
}
