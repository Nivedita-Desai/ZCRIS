using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CreditRatingModel.Model
{
   public class user
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Select Role")]
        public int RoleId { get; set; }

        [Required(ErrorMessage = "Select Menu.")]
        public int UserId { get; set; }

        public string Add_UpdateUser(user U, long sessionid)
        {
            creaditratingEntities db = new creaditratingEntities();

            List<UserRoleRelation> UserRoleRelation = db.UserRoleRelations.Where(m => m.UserId == U.UserId).ToList();

            for (int j = 0; j < UserRoleRelation.Count; j++)
                {

                    db.UserRoleRelations.DeleteObject(UserRoleRelation[j]);
                }

                db.SaveChanges();

                UserRoleRelation UserRoleRelations = new UserRoleRelation();
                UserRoleRelations.RoleId = U.RoleId;
                    UserRoleRelations.UserId = U.UserId;
                    UserRoleRelations.CreateDate = DateTime.UtcNow.Date;
                    UserRoleRelations.CreateId = sessionid;
                    db.AddToUserRoleRelations(UserRoleRelations);

                db.SaveChanges();

                return "Role has been updated";

            }        
        
    }
}
