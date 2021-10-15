using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BL;
using Model;
using MvcUI.nadav.Models;
using Microsoft.Ajax.Utilities;
using System.IO;

namespace MvcUI.nadav.Controllers
{
    /// <summary>
    /// Controller to admin.
    /// </summary>
    [Authorize(Roles="Admin")]
    public class AdminController : Controller
    {
        private UsersManager u_manager;
        private CarRentalManager cr_manager;
        private CarsManager c_manager;

        /// <summary>
        /// Constructor.
        /// </summary>
        public AdminController()
        {
            u_manager = new UsersManager();
            cr_manager = new CarRentalManager();
            c_manager = new CarsManager();
        }

        /// <summary>
        /// Action of CRUDRentals page.
        /// </summary>
        /// <returns>view of the page</returns>
        public ActionResult CRUDRentals()
        {
            return View();
        }

        /// <summary>
        /// Action of CRUDCarTypes page.
        /// </summary>
        /// <returns>view of the page</returns>
        public ActionResult CRUDCarTypes()
        {
            return View();
        }

        /// <summary>
        /// Action of CRUDCars page.
        /// </summary>
        /// <returns>view of the page</returns>
        public ActionResult CRUDCars()
        {
            return View();
        }

        /// <summary>
        /// Brings from the car manager an image of a specific car.
        /// </summary>
        /// <param name="id">the id of the specific car</param>
        /// <returns>partial view of an image of the car</returns>
        [AllowAnonymous]
        public ActionResult GetImageForCar(int id)
        {
            ViewBag.id = id;
            return PartialView(c_manager.GetImageById(id));
        }

        /// <summary>
        /// Show car image.
        /// </summary>
        /// <param name="id">the id of the car</param>
        /// <returns>an image if exist ,else null</returns>
        [AllowAnonymous]
        public ActionResult CarImage(int id)
        {
            byte[] image = c_manager.GetImageById(id);
            if (image != null)
            {
                return File(image, "image/jpeg");
            }
            return null;
        }

        /// <summary>
        /// Insert car image to a specific car.
        /// </summary>
        /// <param name="id">the id of the car</param>
        /// <param name="carImage">the car image to insert</param>
        /// <returns>redirecting to the crudcars page</returns>
        public ActionResult InsertCarImage(int id, HttpPostedFileBase carImage)
        {
            if (carImage != null && carImage.ContentType.ToLower() == "image/jpeg")
            {
                BinaryReader br = new BinaryReader(carImage.InputStream);
                c_manager.InsertImageToCar(id, br.ReadBytes(carImage.ContentLength));

            }
            return RedirectToAction("CRUDCars");
        }

        /// <summary>
        /// Action of CRUDUsers page.
        /// </summary>
        /// <returns>view of the page</returns>
        public ActionResult CRUDUsers()
        {
            return View();
        }

        /// <summary>
        /// brings from the user manager an image of a specific user.
        /// </summary>
        /// <param name="id">the id of the specific user</param>
        /// <returns>partial view of an image of the user</returns>
        public ActionResult GetImageForUser(int id)
        {
            ViewBag.id = id;
            return PartialView(u_manager.GetImageById(id));
        }

        /// <summary>
        /// Show user image
        /// </summary>
        /// <param name="id">the id of the user</param>
        /// <returns>an image if exist ,else null</returns>
        public ActionResult UserImage(int id)
        {
            byte[] image = u_manager.GetImageById(id);
            if (image != null)
            {
                return File(image, "image/jpeg");
            }
            return null;
        }

        /// <summary>
        /// Insert car image to a specific user.
        /// </summary>
        /// <param name="id">the id of the user</param>
        /// <param name="carImage">the user image to insert</param>
        /// <returns>redirecting to the crudusers page</returns>
        public ActionResult InsertUserImage(int id , HttpPostedFileBase userImage)
        {
            if (userImage != null && userImage.ContentType.ToLower() == "image/jpeg")
            {
                BinaryReader br = new BinaryReader(userImage.InputStream);
                u_manager.insertImageToUser(id, br.ReadBytes(userImage.ContentLength));
                
            }
            return RedirectToAction("CRUDUsers");
        }
    }
}