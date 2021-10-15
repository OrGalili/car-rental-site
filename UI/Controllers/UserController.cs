using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BL;
using Model;
using System.Web.Security;

namespace MvcUI.nadav.Controllers
{
    /// <summary>
    /// Controller to user actions.
    /// </summary>
    [Authorize(Roles = "User")]
    public class UserController : Controller
    {
        private CarsManager c_manager;
        private CarRentalManager cr_manager;
        private UsersManager u_manager;

        /// <summary>
        /// Constructor.
        /// </summary>
        public UserController()
        {
            c_manager = new CarsManager();
            cr_manager = new CarRentalManager();
            u_manager = new UsersManager();
        }

        /// <summary>
        /// An invitaion page for a specific car.
        /// </summary>
        /// <returns>car invitation view with car details from the Db, if not found return page with value null</returns>
        public ActionResult CarInvitation()
        {
            HttpCookie acceptedCook = Request.Cookies["carNumber"];
            string carNumber;
            if (acceptedCook != null)
            {
                carNumber = acceptedCook.Value;
                Car carInDb = c_manager.FindCar(int.Parse(carNumber));
                if (carInDb != null)
                {
                    return View(carInDb);
                }
            }
            return View();
        }

        /// <summary>
        /// Create a new rental row in Db by the car rental manager.
        /// </summary>
        /// <param name="startDate">the start date of the rent</param>
        /// <param name="endDate">the end date of the rent</param>
        /// <param name="carNumber">the car number of the car to rent</param>
        /// <returns>redirecting to home page</returns>
        [HttpPost]
        public ActionResult setOrderInDb(DateTime startDate, DateTime endDate, string carNumber)
        {
            string username = User.Identity.Name;
            cr_manager.AddRent(username, Convert.ToInt32(carNumber), startDate, endDate);
            return Redirect("/Home/Index");
        }

        /// <summary>
        /// View page of the orders history of the user.
        /// </summary>
        /// <returns>a view with a list of rentals history</returns>
        public ActionResult userOrdersHistory()
        {
            string username = User.Identity.Name;
            return View(cr_manager.userRentalsHistory(username));
        }


        
    }
}