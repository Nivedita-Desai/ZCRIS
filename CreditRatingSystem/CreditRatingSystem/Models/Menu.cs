using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using CreditRatingModel.Model;

namespace CreditRating.Models
{
    public class Menu
    {
        creaditratingEntities db = new creaditratingEntities();

        //public string MenuString(int UserID)
        //{

        //    StringBuilder strbldg = new StringBuilder();

        //    strbldg.Append("<ul class='sidebar-menu'>");


        //    var menuRsult = (from URR in db.UserRoleRelations
        //                     join RMR in db.RoleMenuRelations on URR.RoleId equals RMR.RoleId
        //                     join M in db.Menus on RMR.MenuId equals M.Id
        //                     where (M.ParentId == null && URR.UserId == UserID)
        //                     select new { M.Id, M.MenuUrl, M.MenuName,
        //                                  IconClass = M.IconClass == null ? "" : M.IconClass
        //                          }).ToList();

        //    if (menuRsult.Count <= 0)
        //    {
        //        return strbldg.Append("").ToString();
        //    }


        //    foreach (var item in menuRsult)
        //    {

        //        // Reust 1,2,8,4

        //        var subItemResult = (from M in db.Menus
        //                             where M.ParentId == item.Id
        //                             select new {
        //                                 M.MenuName,
        //                               MenuUrl= M.MenuUrl==null ? "" : M.MenuUrl ,
        //                                 M.Id }).ToList();
               
        //        if (subItemResult.Count > 0)
        //        {

        //            strbldg.Append("<li id='" + item.MenuName.Trim() + "' class='treeview'><a href='#' id='" + item.MenuName.Trim() + "'><i class='" + item.IconClass.Trim() + "'></i><span>" + item.MenuName.Trim() + "</span><i class='fa fa-angle-left pull-right'></i></a>");
        //            strbldg.Append("<ul class='treeview-menu'>");

        //            foreach (var subitem in subItemResult)
        //            {
        //                //if (subitem.MenuUrl == null)
        //                //{
        //                //    subitem.MenuUrl = "a";
        //                //}

        //                var subMenuItemResult = (from S in db.Menus
        //                                         where S.ParentId == subitem.Id
        //                                         select new
        //                                         {
        //                                             S.MenuName,
        //                                             MenuUrl = S.MenuUrl == null ? "" : S.MenuUrl,
        //                                             S.Id
        //                                         }).ToList();
                        
        //                if (subMenuItemResult.Count > 0)
        //                {
        //                    strbldg.Append("<li id='" + subitem.MenuName.Trim() + "' class='treeview'><a href='#' id='" + subitem.MenuName.Trim() + "'><i class='fa fa-circle-o'></i><span>" + subitem.MenuName.Trim() + "</span><i class='fa fa-angle-left pull-right'></i></a>");

        //                    strbldg.Append("<ul class='treeview-menu'>");

        //                    foreach (var menuitem in subMenuItemResult)
        //                    {
        //                        strbldg.Append("<li id='" + menuitem.MenuName.Trim() + "'><a href='" + menuitem.MenuUrl.Trim() + "' id='" + menuitem.MenuUrl.Trim() + "'><i class='fa fa-circle-o'></i>" + menuitem.MenuName.Trim() + "</a></li>");
        //                    }

        //                    strbldg.Append("</ul>");
        //                }
        //                else
        //                {
        //                    strbldg.Append("<li id='" + subitem.MenuName.Trim() + "'><a href='" + subitem.MenuUrl.Trim() + "' id='" + subitem.MenuName.Trim() + "'><i class='fa fa-circle-o'></i>" + subitem.MenuName.Trim() + "</a>");
        //                }

        //                strbldg.Append("</li>");
        //            }

        //            strbldg.Append("</ul>");
        //        }
        //        else
        //        {
        //            //strbldg.Append("<li id='li" + item.MenuText.Trim() + "' ><a href='" + item.Url.Trim() + "' id='a" + item.MenuText.Trim() + "' class='active'><i class='fa fa-home'></i>" + item.MenuText.Trim() + "</a></li>");
        //            strbldg.Append("<li id='" + item.MenuName.Trim() + "' class='treeview'><a href='#' id='" + item.MenuName.Trim() + "'><i class='" + item.IconClass.Trim() + "'></i><span>" + item.MenuName.Trim() + "</span><i class='fa fa-angle-left pull-right'></i></a>");
        //        }

        //        strbldg.Append("</li>");
        //    }


        //    strbldg.Append("</ul>");

        //    return strbldg.ToString();
        //}

        public string MenuString(int UserID)
        {

            StringBuilder strbldg = new StringBuilder();

            strbldg.Append("<ul class='sidebar-menu'>");


            var menuRsult = (from URR in db.UserRoleRelations
                             join RMR in db.RoleMenuRelations on URR.RoleId equals RMR.RoleId
                             join M in db.Menus on RMR.MenuId equals M.Id
                             where (M.ParentId == null && URR.UserId == UserID)
                             select new
                             {
                                 M.Id,
                                 M.MenuUrl,
                                 M.MenuName,
                                 IconClass = M.IconClass == null ? "" : M.IconClass
                             }).ToList();

            if (menuRsult.Count <= 0)
            {
                return strbldg.Append("").ToString();
            }


            foreach (var item in menuRsult)
            {

                // Reust 1,2,8,4

                var subItemResult = (from URR in db.UserRoleRelations
                                     join RMR in db.RoleMenuRelations on URR.RoleId equals RMR.RoleId
                                     join M in db.Menus on RMR.MenuId equals M.Id
                                     where (M.ParentId == item.Id && URR.UserId == UserID)
                                     select new
                                     {
                                         M.MenuName,
                                         MenuUrl = M.MenuUrl == null ? "" : M.MenuUrl,
                                         M.Id
                                     }).ToList();

                if (subItemResult.Count > 0)
                {

                    strbldg.Append("<li id='" + item.MenuName.Trim() + "' class='treeview'><a href='" + item.MenuUrl.Trim() + "' id='" + item.MenuName.Trim() + "'><i class='" + item.IconClass.Trim() + "'></i><span>" + item.MenuName.Trim() + "</span><i class='fa fa-angle-left pull-right'></i></a>");
                    strbldg.Append("<ul class='treeview-menu'>");

                    foreach (var subitem in subItemResult)
                    {
                        //if (subitem.MenuUrl == null)
                        //{
                        //    subitem.MenuUrl = "a";
                        //}

                        var subMenuItemResult = (from URR in db.UserRoleRelations
                                                 join RMR in db.RoleMenuRelations on URR.RoleId equals RMR.RoleId
                                                 join S in db.Menus on RMR.MenuId equals S.Id
                                                 where (S.ParentId == subitem.Id && URR.UserId == UserID)
                                                 select new
                                                 {
                                                     S.MenuName,
                                                     MenuUrl = S.MenuUrl == null ? "" : S.MenuUrl,
                                                     S.Id
                                                 }).ToList();

                        if (subMenuItemResult.Count > 0)
                        {
                            strbldg.Append("<li id='" + subitem.MenuName.Trim() + "' class='treeview'><a href='" + subitem.MenuUrl.Trim() + "' id='" + subitem.MenuName.Trim() + "'><i class='fa fa-circle-o'></i><span>" + subitem.MenuName.Trim() + "</span><i class='fa fa-angle-left pull-right'></i></a>");

                            strbldg.Append("<ul class='treeview-menu'>");

                            foreach (var menuitem in subMenuItemResult)
                            {
                                strbldg.Append("<li id='" + menuitem.MenuName.Trim() + "'><a href='" + menuitem.MenuUrl.Trim() + "' id='" + menuitem.MenuUrl.Trim() + "'><i class='fa fa-circle-o'></i>" + menuitem.MenuName.Trim() + "</a></li>");
                            }

                            strbldg.Append("</ul>");
                        }
                        else
                        {
                            strbldg.Append("<li id='" + subitem.MenuName.Trim() + "'><a href='" + subitem.MenuUrl.Trim() + "' id='" + subitem.MenuName.Trim() + "'><i class='fa fa-circle-o'></i>" + subitem.MenuName.Trim() + "</a>");
                        }

                        strbldg.Append("</li>");
                    }

                    strbldg.Append("</ul>");
                }
                else
                {
                    //strbldg.Append("<li id='li" + item.MenuText.Trim() + "' ><a href='" + item.Url.Trim() + "' id='a" + item.MenuText.Trim() + "' class='active'><i class='fa fa-home'></i>" + item.MenuText.Trim() + "</a></li>");
                    strbldg.Append("<li id='" + item.MenuName.Trim() + "' class='treeview'><a href='" + item.MenuUrl.Trim() + "' id='" + item.MenuName.Trim() + "'><i class='" + item.IconClass.Trim() + "'></i><span>" + item.MenuName.Trim() + "</span><i class='fa fa-angle-left pull-right'></i></a>");
                }

                strbldg.Append("</li>");
            }


            strbldg.Append("</ul>");

            return strbldg.ToString();
        }
    }
}