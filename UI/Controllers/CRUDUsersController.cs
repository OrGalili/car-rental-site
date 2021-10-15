using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BL;
using Model;

namespace MvcUI.nadav.Controllers
{
    /// <summary>
    /// A controller to the users for Create Read Update and delete actions.
    /// </summary>
    public class CRUDUsersController : Controller
    {
        private UsersManager u_manager;
        private CarRentalManager cr_manager;

        /// <summary>
        /// Constructor.
        /// </summary>
        public CRUDUsersController()
        {
            u_manager = new UsersManager();
            cr_manager = new CarRentalManager();
        }

        /// <summary>
        /// Bring from users manager all the users in Db.
        /// </summary>
        /// <returns>Json with all users that in Db</returns>
        public ActionResult GetUsers()
        {
            return Json(u_manager.getAllUsersAsJSON(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Deleting a user by the users manager.
        /// </summary>
        /// <param name="user">the user to delete</param>
        /// <returns>true if the delete has succeded, else false with an error message</returns>
        public ActionResult DeleteUser(User user)
        {
            //המנהל יוכל למחוק את המשתמש רק כאשר הוא מחק את היסטוריית ההשכרה של אותו משתמש
                if (cr_manager.UserNotHaveRentalsHistory(user.Id))
                {
                    if (u_manager.deleteUser(user.Id))
                        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }

            return Json(new { success = false , errorMessage = "לא ניתן למחוק את המשתמש מפני שקיימות לו עדיין השכרות במערכת"},JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Creating a new user by the user manager.
        /// if the validation of the entity is wrong there will not be a creation.
        /// </summary>
        /// <param name="user">the user to create</param>
        /// <param name="roles">the roles of the user</param>
        /// <returns>Json with ture value and the user entity if the creation succeded, else false with error messages</returns>
        public ActionResult CreateUser(User user, string[] roles)
        {
            string errMessage = errorMassages(user, roles);
            if (u_manager.usernameExist(user.Username,u_manager.GetAllUsers()))
            {
                errMessage += "* כינוי המשתמש קיים כבר במערכת";
            }
            if (u_manager.userIdExist(user.UserId, u_manager.GetAllUsers()))
            {
                errMessage += "* תעודת זהות זו קיימת במערכת";
            }

            if (errMessage != "")
            {
                return Json(new { success = false, errorMessages = errMessage });
            }
            else
            {
                u_manager.addUser(user,roles);
                return Json(new { success = true, user = u_manager.getUserAsJSON(user) });
            }

        }

        /// <summary>
        /// Updating a user by the user manager.
        /// </summary>
        /// <param name="user">the new user data</param>
        /// <param name="roles">the roles of the user to update</param>
        /// <returns>Json with ture value if the update succeded, else false with error messages</returns>
        public ActionResult UpdateUser(User user,string[] roles)
        {
            string errMessage = errorMassages(user, roles);
            if (u_manager.usernameExist(user.Username, u_manager.GetUsersWithoutUser(user.Id)))
            {
                errMessage += "* כינוי המשתמש קיים כבר במערכת";
            }
            if (u_manager.userIdExist(user.UserId, u_manager.GetUsersWithoutUser(user.Id)))
            {
                errMessage += "* תעודת זהות זו קיימת במערכת";
            }

            if (errMessage != "")
            {
                return Json(new { success = false, errorMessages = errMessage });
            }
            else
            {
                u_manager.updateUser(user,roles);
                return Json(new { success = true });
            }

        }

       
        /// <summary>
        /// Check the model state is valid or not.
        /// </summary>
        /// <param name="user">the user to check</param>
        /// <param name="roles">the roles of the user</param>
        /// <returns>if not valid it returns a string with error massages, else get an empty string</returns>
        private string errorMassages(User user,string[] roles)
        {
            string errMessage = "";
            if (roles == null)
            {
                errMessage += "* המשתמש חייב לקבל לפחות תפקיד אחד";
            }
            if (!ModelState.IsValid)
            {
                foreach (var prop in ModelState)
                {
                    if (prop.Value.Errors.Any())
                    {
                        foreach (var error in prop.Value.Errors)
                        {
                            errMessage += "*" + error.ErrorMessage + "\n";
                        }
                    }
                }
            }
            return errMessage;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                u_manager.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}