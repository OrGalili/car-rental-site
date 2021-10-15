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
    /// A controller to the cars for Create Read Update and delete actions.
    /// </summary>
    public class CRUDCarsController : Controller
    {
        private CarsManager c_manager;

        /// <summary>
        /// Constructor.
        /// </summary>
        public CRUDCarsController ()
        {
            c_manager = new CarsManager();
        }

        /// <summary>
        /// Creating a new car by the car manager. 
        /// if the validation of the entity is wrong there will not be a creation.
        /// </summary>
        /// <param name="car">the car to create</param>
        /// <returns>Json with ture value and the car entity if the creation succeded, else false with error messages</returns>
        public ActionResult CreateCar(CarVM car)
        {
            Car carObject = car.CreateCarObject();
            string errorMsgs = ErrorMessages();
            if(errorMsgs !="")
            {
                return Json(new { success = false, errorMessage = errorMsgs }, JsonRequestBehavior.AllowGet);
            }

                
            else if(c_manager.CreateNewCar(carObject))
            {
                car.Id = carObject.Id;
                return Json(new { success = true, car = car });
            }
                
            else
            {
                return Json(new { success = false, errorMessage = "מספר הרכב קיים במערכת" });
            }

        }

        /// <summary>
        /// Bring from cars manager all the cars in Db.
        /// </summary>
        /// <returns>Json with all cars that in Db</returns>
        public ActionResult GetCars()
        {
            return Json(c_manager.CarsInDbAsVM(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Updating a car by the car manager.
        /// </summary>
        /// <param name="car">the new car data</param>
        /// <returns>Json with ture value if the update succeded, else false with error messages</returns>
        public ActionResult UpdateCar(CarVM car)
        {
            string errorMsgs = ErrorMessages();
            if (errorMsgs != "")
            {
                return Json(new { success = false, erorrMessage = errorMsgs }, JsonRequestBehavior.AllowGet);
            }

            else if (c_manager.UpdateCar(car.CreateCarObject()))
            {
                return Json(new { success = true });
            }

            else
            {
                return Json(new { success = false, errorMessage = "מספר הרכב קיים במערכת" });
            }
        }

        /// <summary>
        /// Deleting a car by the cars manager.
        /// </summary>
        /// <param name="id">the id of the car to delete</param>
        /// <returns>true if the delete has succeded, else false with an error message</returns>
        public ActionResult DeleteCar(int id)
        {
            if (!c_manager.carHaveRentals(id))
            {
                if (c_manager.DeleteCar(id))
                {
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { success = false , errorMessage = "לרכב זה קיים השכרות ולכן לא ניתן למחוק אותו" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Check the model state is valid or not.
        /// </summary>
        /// <returns>if not valid it returns a string with error massages, else get an empty string</returns>
        private string ErrorMessages()
        {
            string errorMsgs = "";

            if(!ModelState.IsValid)
            {
                foreach (var prop in ModelState)
                {
                    if(prop.Value.Errors.Any())
                    {
                        foreach (var error in prop.Value.Errors)
                        {
                            errorMsgs += error.ErrorMessage + " ";
                        }
                    }
                }
            }
            return errorMsgs;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                c_manager.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}