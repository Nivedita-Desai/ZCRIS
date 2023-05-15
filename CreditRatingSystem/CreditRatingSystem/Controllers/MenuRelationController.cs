using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CreditRatingModel.Model;
using System.Data.Entity;

namespace CreditRating.Controllers
{
    public class MenuRelationController : Controller
    {
        //
        // GET: /MenuRelation/
        creaditratingEntities db = new creaditratingEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult RoleMenuRelation( long Role_Id = 0)
        {
           
            ViewBag.Role = new SelectList(db.Roles.ToList(), "Id", "RoleDescription");
            MenuRelation menu = new MenuRelation();
            menu.lstMainMenu = menu.GetMenulist();
            //test t = new test();
            //t.lstMainMenu = t.GetMenulist();

            if (Role_Id != 0)
            {
                menu = menu.BindEditMenu(menu, Role_Id);
                //t = t.BindEditMenu(t, Role_Id);
            }
            else
            {
                Role Role_Master = new Role();
                Role_Master.Id = 0;
                menu.role = Role_Master;
                //t.role = Role_Master;
            }
            return View(menu);
        }
        [HttpPost]
      //  [Authorize]
        public ActionResult RoleMenuClick(MenuRelation menu)
        {
            creaditratingEntities db = new creaditratingEntities();
            int CreateUserId = Convert.ToInt32(Session["UserId"]);
            if (ModelState.IsValid)
            {
                if (menu.menuids != null)
                {
                    try
                    {

                        //                      for (int i = 0; i < menu.menuids.Length; i++)
                        //         {
                        //             RoleMenuRelation RoleMenuRelation = new RoleMenuRelation();
                        //             RoleMenuRelation.RoleId = menu.RoleId;
                        //             RoleMenuRelation.MenuId = menu.menuids[i];
                        //             RoleMenuRelation.CreateDate = DateTime.UtcNow.Date;
                        //             RoleMenuRelation.CreateId = CreateUserId;
                        //             db.AddToRoleMenuRelations(RoleMenuRelation);

                        //         }

                        //         db.SaveChanges();
                        //                     ModelState.Clear();
                        //}

                       // InsertInfo1(menu.RoleId, menu.menuids, CreateUserId);
                            

                        if (menu.RoleId == 0)
                        {
                            ViewBag.Message = menu.Add_UpdateMennu(menu, Convert.ToInt32(Session["userid"]));

                        }
                        else
                        {
                            var prvRoleMaster = db.Roles.Where(x => x.Id == menu.RoleId).FirstOrDefault();
                           // prvRoleMaster. = Convert.ToInt32(Session["userid"]);
                           // prvRoleMaster.UpdatedDate = DateTime.UtcNow;

                            if (TryUpdateModel(prvRoleMaster, "rl_mst"))
                            {
                                db.SaveChanges();
                            }
                            ViewBag.Message = menu.Add_UpdateMennu(menu, Convert.ToInt32(Session["userid"]));
                            return RedirectToAction("RoleMenuRelation");
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                else
                {
                    ModelState.AddModelError("err", "Select atleast one menu.");
                    //ViewBag.Message = "Select atleast one menu."; 
                }
            }
            menu.lstMainMenu = menu.GetMenulist();
            if (menu.role.Id != 0)
            {
                menu = menu.BindEditMenu(menu, menu.role.Id);
            }
            return RedirectToAction("RoleMenuRelation");
        
        }
        private void InsertInfo1( int RoleId, int UserId, int CreateUserId)
        {
            Add1( RoleId, UserId, CreateUserId);
        }
        public void Add1( int RoleId, int menuids, int CreateUserId)
        {
            MenuRelation menu=new MenuRelation();
                 for (int i = 0; i < menu.menuids.Length; i++)
                 {
                     RoleMenuRelation RoleMenuRelation = new RoleMenuRelation();
                     RoleMenuRelation.RoleId = RoleId;
                     RoleMenuRelation.MenuId = menu.menuids[i];
                     db.AddToRoleMenuRelations(RoleMenuRelation);

                 }

                 db.SaveChanges();
        }



        public void DropList(user U)
        {
            ViewBag.Role = new SelectList(db.Roles.ToList(), "Id", "RoleDescription");
            ViewBag.User = new SelectList(db.User_Details.ToList(), "Id", "NAME");
        }
        public ActionResult UserRoleRelation(user U)
        {     
            DropList(U);
            return View();
        }

        [HttpPost]
        public ActionResult btnAddClick(user U)
        {
            creaditratingEntities db = new creaditratingEntities();
            int CreateUserId = Convert.ToInt32(Session["UserId"]);
                if (ModelState.IsValid)
                {
                    try
                    {
                        ViewBag.Message = U.Add_UpdateUser(U, Convert.ToInt32(Session["userid"]));
                      //  InsertInfo(U.RoleId, U.UserId, CreateUserId);
                        ModelState.Clear();
                    }
                    catch(Exception e)
                    {
                        string Message = e.Message;
                    }
                }
                DropList(U);
                return View("UserRoleRelation");
        }
        private void InsertInfo(int RoleId, int UserId, int CreateUserId)
        {
            Add(RoleId, UserId, CreateUserId);
        }
        public void Add(int RoleId, int UserId, int CreateUserId)
        {
            UserRoleRelation URR = new UserRoleRelation();
            URR.RoleId = RoleId;
            URR.UserId = UserId;
            URR.CreateDate = DateTime.UtcNow.Date;
            URR.CreateId = CreateUserId;
            db.AddToUserRoleRelations(URR);
            db.SaveChanges();
        }

        public JsonResult EditRoleMenuRelation(long Role_Id)
        {
            //var result="";
            MenuRelation menu = new MenuRelation();
            menu.lstMainMenu = menu.GetMenulist();
            //test t = new test();
            //t.lstMainMenu = t.GetMenulist();

            if (Role_Id != 0)
            {
                menu = menu.BindEditMenu(menu, Role_Id);
                //t = t.BindEditMenu(t, Role_Id);
            }
            return Json(menu,JsonRequestBehavior.AllowGet);
        }

    }
}
