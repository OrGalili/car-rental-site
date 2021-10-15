using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using BL;


namespace MvcUI.nadav.Controllers
{
    /// <summary>
    /// A controller to an employee.
    /// </summary>
    [Authorize(Roles = "Employee")]
    public class EmployeeController : Controller
    {
        private CarRentalManager cr_manager;
        private CarsManager c_manager;

        /// <summary>
        /// Constructor.
        /// </summary>
        public EmployeeController ()
        {
            cr_manager = new CarRentalManager();
            c_manager = new CarsManager();
        }

        /// <summary>
        /// Action of ReturnCar page.
        /// </summary>
        /// <returns>view of the page</returns>
        public ActionResult ReturnCar()
        {
            return View();
        }

        /// <summary>
        /// Brings all car rentals that in Db by the car rental manager.
        /// </summary>
        /// <param name="carNumber">the car number of the car with the rentals</param>
        /// <returns>Json with true value and car rental entity if found rentals to the car, else false and a message</returns>
        public ActionResult GetCarRentals(int carNumber)
        {
            if(c_manager.FindCar(carNumber)!=null)
            {
                ICollection<CarRental> carRentals = cr_manager.GetCarRentalsByCarNumber(carNumber);
                if (carRentals.Count > 0)
                {
                    return Json(new { success = true, carRentals = carRentals }, JsonRequestBehavior.AllowGet);
                }

                else
                {
                    return Json(new { success = false, error = "אין השכרות לרכב מספר " + carNumber }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { success = false, error = "מספר הרכב "+carNumber+" לא קיים במערכת" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Change to a specific car rental his actual return date to now.
        /// deleting the rent from the Db.
        /// </summary>
        /// <param name="id">the id of the car rental</param>
        /// <returns>if update succeded return true and the cost of the rent, else false</returns>
        public ActionResult ChangeActualReturnDate(int id)
        {
            if(cr_manager.UpdataActualReturnDateRentalToNow(id))
            {
                decimal sumToPay = cr_manager.SumRental(id);
                cr_manager.DeleteRent(id);
                return Json(new { success = true, paymentCost = sumToPay }, JsonRequestBehavior.AllowGet);
            }

            else
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}