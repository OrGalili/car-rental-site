using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BL;
using Model;
using MvcUI.nadav.Models;

namespace MvcUI.nadav.Controllers
{
    /// <summary>
    /// A controller to the rentals for Create Read Update and delete actions.
    /// </summary>
    public class CRUDRentalsController : Controller
    {
        private CarRentalManager cr_manager;

        /// <summary>
        /// Constructor.
        /// </summary>
        public CRUDRentalsController()
        {
            cr_manager = new CarRentalManager();
        }

        /// <summary>
        /// Creating a new rental by the car rental manager. 
        /// if the validation of the entity is wrong there will not be a creation.
        /// </summary>
        /// <param name="rental">the rental to create</param>
        /// <returns>Json with ture value and the car rental entity if the creation succeded, else false with error messages</returns>
        public ActionResult CreateRental(CarRentalVM rental)
        {
            if (cr_manager.IsAvailableForRent(rental.CarNumber, rental.StartDate, rental.ReturnDate))
            {
                cr_manager.AddRent(rental.Username, rental.CarNumber, rental.StartDate, rental.ReturnDate);
                return Json(new { success = true, rental = rental }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, error = "לא ניתן להשכיר מפני שהרכב תפוס בזמן זה" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Bring from car rental manager all the rentals in Db.
        /// </summary>
        /// <returns>Json with all rentals that in Db</returns>
        public ActionResult GetRentals()
        {
            return Json(cr_manager.allRentalsInDbAsVM(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Updating a rental by the car rental manager.
        /// </summary>
        /// <param name="rental">the new rental data</param>
        /// <returns>Json with ture value if the update succeded, else false with error messages</returns>
        public ActionResult UpdateRental(CarRentalVM rental)
        {
            if (cr_manager.UpdateRent(rental.createCarRentalObject()))
            {
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, error = "לא ניתן להשכיר מפני שהרכב תפוס בזמן זה" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Deleting a rental by the car rental manager.
        /// </summary>
        /// <param name="id">the id of the rental to delete</param>
        /// <returns>true if the delete has succeded, else false</returns>
        public ActionResult DeleteRental(int Id)
        {
            if (cr_manager.DeleteRent(Id))
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);

            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                cr_manager.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}