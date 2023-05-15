using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CreditRatingModel.Model
{
   public class MenuRelation
    {
        public int Id { get; set; }
      public  Role role { get; set; }
      public int[] menuids { get; set; }

        [Required(ErrorMessage = "Select Role")]
        public int RoleId { get; set; }

     
        public string MenuId { get; set; }




        public List<MainMenu> lstMainMenu = new List<MainMenu>();


        //public List<MainMenu> GetMenulist()
        //{

        //    creaditratingEntities dbnew = new creaditratingEntities();

        //    List<MainMenu> Menu = new List<MainMenu>();

        //    var Result = (from M in dbnew.Menus
        //                  where (M.ParentId == null)
        //                  select new { M.MenuName, M.Id, M.MenuUrl }).ToList();


        //    foreach (var item in Result)
        //    {

        //        var SubItemResult = (from M in dbnew.Menus
        //                             where (M.ParentId == item.Id)
        //                             select new { M.MenuName, M.Id }).ToList();


        //        if (SubItemResult.Count > 0)
        //        {

        //            List<SubMenu> objSubMenulst = new List<SubMenu>();

        //            foreach (var SubItem in SubItemResult)
        //            {

        //                SubMenu objSubMenu = new SubMenu();

        //                objSubMenu.MenuId = SubItem.Id;
        //                objSubMenu.Text = SubItem.MenuName;
        //                objSubMenulst.Add(objSubMenu);

        //            }

        //            MainMenu objMM_with_chield = new MainMenu { MenuId = item.Id, Text = item.MenuName, SubMenu = objSubMenulst };


        //            Menu.Add(objMM_with_chield);
        //        }
        //        else
        //        {
        //            MainMenu objMM_without_chield = new MainMenu { MenuId = item.Id, Text = item.MenuName };

        //            Menu.Add(objMM_without_chield);
        //        }

        //    }

        //    return Menu;

        //}


       //-------------301015 Menu Binding----------------------------
        public List<MainMenu> GetMenulist()
        {

            creaditratingEntities dbnew = new creaditratingEntities();

            List<MainMenu> Menu = new List<MainMenu>();

            var Result = (from M in dbnew.Menus
                          where (M.ParentId == null)
                          select new { M.MenuName, M.Id, M.MenuUrl }).ToList();


            foreach (var item in Result)
            {

                var SubItemResult = (from M in dbnew.Menus
                                     where (M.ParentId == item.Id)
                                     select new { M.MenuName, M.Id }).ToList();


                if (SubItemResult.Count > 0)
                {

                    List<SubMenu> objSubMenulst = new List<SubMenu>();


                    foreach (var SubItem in SubItemResult)
                    {



                        var subMenuItemResult = (from S in dbnew.Menus
                                                 where (S.ParentId == SubItem.Id)
                                                 select new { S.MenuName, S.Id }).ToList();

                        if (subMenuItemResult.Count > 0)
                        {
                            List<SubMenuItem> objSubMenuItemlst = new List<SubMenuItem>();

                            foreach (var subMenuItem in subMenuItemResult)
                            {
                                SubMenuItem objSubMenuItem = new SubMenuItem();

                                objSubMenuItem.MenuId = subMenuItem.Id;
                                objSubMenuItem.Text = subMenuItem.MenuName;
                                objSubMenuItemlst.Add(objSubMenuItem);

                            }

                            SubMenu objSubMenu = new SubMenu();

                            objSubMenu.MenuId = SubItem.Id;
                            objSubMenu.Text = SubItem.MenuName;
                            objSubMenu.SubMenuItem = objSubMenuItemlst;
                            objSubMenulst.Add(objSubMenu);
                        }

                        else
                        {
                            SubMenu objSubMenu = new SubMenu();

                            objSubMenu.MenuId = SubItem.Id;
                            objSubMenu.Text = SubItem.MenuName;
                            //objSubMenu.SubMenuItem = objSubMenuItemlst;
                            objSubMenulst.Add(objSubMenu);
                        }

                    }

                    MainMenu objMM_with_chield = new MainMenu { MenuId = item.Id, Text = item.MenuName, SubMenu = objSubMenulst };


                    Menu.Add(objMM_with_chield);
                }
                else
                {
                    MainMenu objMM_without_chield = new MainMenu { MenuId = item.Id, Text = item.MenuName };

                    Menu.Add(objMM_without_chield);
                }

            }

            return Menu;

        }
        public MenuRelation BindEditMenu(MenuRelation menu, long Role_Id)
        {
            creaditratingEntities db = new creaditratingEntities();


            Role roleMaster = db.Roles.Where(X => X.Id == Role_Id).FirstOrDefault();
            menu.role = roleMaster;

            var roleMenuRelation = db.RoleMenuRelations.Where(Y => Y.RoleId == Role_Id).ToList();

            foreach (var item in menu.lstMainMenu)
            {
                foreach (var subItem in item.SubMenu)
                {
                    foreach (var submnuItem in subItem.SubMenuItem)
                    {
                        for (int i = 0; i < roleMenuRelation.Count; i++)
                        {

                            if (submnuItem.MenuId == roleMenuRelation[i].MenuId)
                            {
                                submnuItem.Checked = true;
                            }
                        }
                    }

                    for (int i = 0; i < roleMenuRelation.Count; i++)
                    {

                        if (subItem.MenuId == roleMenuRelation[i].MenuId)
                        {
                            subItem.Checked = true;
                        }
                    }                    
                }

                for (int j = 0; j < roleMenuRelation.Count; j++)
                {
                    if (item.MenuId == roleMenuRelation[j].MenuId)
                    {
                        item.Checked = true;
                    }
                }

            }

            return menu;

        }
        public string Add_UpdateMennu(MenuRelation menu, long sessionid)
        {
            creaditratingEntities db = new creaditratingEntities();
            if (menu.RoleId == 0)
            {
                //Role r = new Role();
                //r.Role1 = menu.role.RoleDescription;
                //r.RoleDescription = menu.rl_mst.RoleDescription;
                //r.RoleDescription = sessionid;
                //r.CreatedDate = DateTime.UtcNow;
                //db.AddToRoles(r);
                //db.SaveChanges();
                // int Role_Id = r.Id;

                for (int i = 0; i < menu.menuids.Length; i++)
                {
                    RoleMenuRelation RoleMenuRelation = new RoleMenuRelation();
                    RoleMenuRelation.RoleId = menu.RoleId;
                    RoleMenuRelation.MenuId = menu.menuids[i];
                    db.AddToRoleMenuRelations(RoleMenuRelation);

                }

                db.SaveChanges();

                return "A New Role has been created.";

            }
            else
            {

                List<RoleMenuRelation> roleMenuRelation = db.RoleMenuRelations.Where(m => m.RoleId == menu.RoleId).ToList();

                for (int j = 0; j < roleMenuRelation.Count; j++)
                {

                    db.RoleMenuRelations.DeleteObject(roleMenuRelation[j]);
                }

                db.SaveChanges();

                for (int i = 0; i < menu.menuids.Length; i++)
                {
                    RoleMenuRelation RoleMenuRelation = new RoleMenuRelation();
                    RoleMenuRelation.RoleId = menu.RoleId;
                    RoleMenuRelation.MenuId = menu.menuids[i];
                    RoleMenuRelation.CreateDate=DateTime.UtcNow.Date;
                    RoleMenuRelation.CreateId = sessionid;
                    db.AddToRoleMenuRelations(RoleMenuRelation);

                }

                db.SaveChanges();

                return "Role has been updated";

            }

            //  return "";

        }


    }
   public class MainMenu
   {
       public long MenuId { get; set; }
       public bool Checked { get; set; }
       public string Text { get; set; }
       public IList<SubMenu> SubMenu { get; set; }

       

       public MainMenu()
       {
           SubMenu = new List<SubMenu>();
           
       }

   }

   public class SubMenu
   {
       public long MenuId { get; set; }
       public string Text { get; set; }
       public bool Checked { get; set; }

       public IList<SubMenuItem> SubMenuItem { get; set; }

       public SubMenu()
       {
           SubMenuItem = new List<SubMenuItem>();
       }
   }

   public class SubMenuItem
   {
       public long MenuId { get; set; }
       public string Text { get; set; }
       public bool Checked { get; set; }

   }
         
}
