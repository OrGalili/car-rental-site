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
    /// A controller to the car types for Create Read Update and delete actions.
    /// </summary>
    public class CRUDCarTypesController : Controller
    {
        private CarTypeManager ct_manager;

        /// <summary>
        /// Constructor.
        /// </summary>
        public CRUDCarTypesController()
        {
            ct_manager = new CarTypeManager();
        }

        /// <summary>
        /// Creating a new carType by the carType manager. 
        /// if the validation of the entity is wrong there will not be a creation.
        /// </summary>
        /// <param name="carType">the car type to create</param>
        /// <returns>Json with ture value and the car type entity if the creation succeded, else false with error messages</returns>
        public ActionResult CreateCarType(CarType carType)
        {
            string errorMsgs = errorMessages();
            if (errorMsgs != "")
            {
                return Json(new { success = false, errorMessage = errorMsgs });
            }

            else if (ct_manager.CreateNewCarType(carType))
            {
                return Json(new { success = true, cartype = carType }, JsonRequestBehavior.AllowGet);
            }

            else
            {
                return Json(new { success = false, errorMessage = "טיפוס מכונית זה קיים במערכת" });
            }
        }

        /// <summary>
        /// Bring from car type manager all the car types in Db.
        /// </summary>
        /// <returns>Json with all car types that in Db</returns>
        public ActionResult GetCarTypes()
        {
            return Json(ct_manager.CarTypesInDb(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Updating a car type by the car type manager.
        /// </summary>
        /// <param name="carType">the new car type data</param>
        /// <returns>Json with ture value if the update succeded, else false with error messages</returns>
        public ActionResult UpdateCarType(CarType carType)
        {
            string errorMsgs = errorMessages();
            if (errorMsgs != "")
            {
                return Json(new { success = false, errorMessage = errorMsgs });
            }

            else if (ct_manager.UpdateCarType(carType))
            {
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            else
            {
                return Json(new { success = false, errorMessage = "טיפוס מכונית זה קיים במערכת" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Deleting a car type by the car type manager.
        /// </summary>
        /// <param name="id">the id of the car type to delete</param>
        /// <returns>true if the delete has succeded, else false</returns>
        public ActionResult DeleteCarType(int id)
        {
            if (!ct_manager.CarTypeRelatedToCars(id))
            {
                if (ct_manager.DeleteCarType(id))
                {
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Check the model state is valid or not.
        /// </summary>
        /// <returns>if not valid it returns a string with error massages, else get an empty string</returns>
        private string errorMessages()
        {
            string errorMsgs = "";

            if (!ModelState.IsValid)
            {
                foreach (var prop in ModelState)
                {
                    if (prop.Value.Errors.Any())
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
    }
}