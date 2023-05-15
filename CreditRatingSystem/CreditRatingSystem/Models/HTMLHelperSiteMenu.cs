using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CreditRating.Models
{
    public static class HTMLHelperSiteMenu
    {
      [Authorize]
        public static MvcHtmlString SiteMenu(this HtmlHelper helper, int UsrID)
        {

            Menu menu = new Menu();

            return MvcHtmlString.Create(menu.MenuString(UsrID));

        }
    }
}