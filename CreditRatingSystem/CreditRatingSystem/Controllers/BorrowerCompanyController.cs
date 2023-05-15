using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CreditRatingModel.Model;

namespace CreditRating.Controllers
{
    public class BorrowerCompanyController : Controller
    {
        //
        // GET: /BorrowerCompany/

        public ActionResult Index()
        {
            return View();
        }

         [Authorize]
        public ActionResult Company()
        {
            using (creaditratingEntities db = new creaditratingEntities())
            {
              var g = (from BCBM in db.BorrowerCompanyBranchMasters
                         join BCTM in db.BorrowerCompanyTypeMasters on BCBM.CompanyTypeId equals BCTM.Id
                         join BCM in db.BorrowerCompanyMasters on BCTM.CompanyId equals BCM.Id
                         //where BCBM.Id == 
                         select new
                         {
                             Id = BCBM.Id,
                             CompanyName = BCM.CompanyName,
                             CompanyType = BCTM.CompanyType,
                             BranchName = BCBM.BranchName
                         }).ToList();
                return View(g);
            }
        }

 [Authorize]
        public ActionResult Borrower(int id)
        {
            using (creaditratingEntities db = new creaditratingEntities())
            {

                var g = (from BCBM in db.BorrowerCompanyBranchMasters
                         join BCTM in db.BorrowerCompanyTypeMasters on BCBM.CompanyTypeId equals BCTM.Id
                         join BCM in db.BorrowerCompanyMasters on BCTM.CompanyId equals BCM.Id
                         where BCBM.Id == id
                         select new BorrowerDetails
                         {
                             //Id = id,
                             CompanyName = BCM.CompanyName,
                             CompanyType = BCTM.CompanyType,
                             BranchName = BCBM.BranchName
                         }).ToList();
                ViewBag.Company = g;
                return View(g.ToList());
            }        
 }
    }
}
