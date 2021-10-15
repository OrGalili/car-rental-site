using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AutoMapper;
using BL;
using BL.SearchModels;
using Model;
using MvcUI.nadav.Models;
using System.IO;

namespace MvcUI.nadav.Controllers
{
    public class HomeController : Controller
    {
        private UsersManager manager;
        private CarsManager c_manager;
        private CarTypeManager ct_manager;

        /// <summary>
        /// Constructor.
        /// </summary>
        public HomeController()
        {
            manager = new UsersManager();
            c_manager = new CarsManager();
        }
        
        /// <summary>
        /// Going to home page.
        /// </summary>
        /// <param name="username"></param>
        /// <returns>view of the index page</returns>
        public ActionResult Index(/*string username*/)
        {
            return View();
        }
        
        /// <summary>
        /// The details about the company.
        /// </summary>
        /// <returns>view of the about page</returns>
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        
        /// <summary>
        /// Details to contact to the company
        /// </summary>
        /// <returns>view of the contact page</returns>
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        /// <summary>
        /// The register user page.
        /// </summary>
        /// <returns>view of the register page</returns>
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// Post to register action with the user details and his image.
        /// </summary>
        /// <param name="user">the user details</param>
        /// <param name="userImage">the image of the user</param>
        /// <returns>redirecting to login page if registration completed, else false with error masseges</returns>
        [HttpPost]
        public ActionResult Register(User user ,HttpPostedFileBase userImage)
        {
            if(manager.userIdExist(user.UserId,manager.GetAllUsers()))
            {
                ModelState.AddModelError("","תעודת זהות זו הינה קיימת במערכת");
            }
            if(manager.usernameExist(user.Username,manager.GetAllUsers()))
            {
                ModelState.AddModelError("","שם משתמש זה קיים במערכת");
            }
            if (userImage != null && userImage.ContentType.ToLower() != "image/jpeg")
            {
                ModelState.AddModelError("", "התמונה ששלחת אינה מסוג jpg");
            }

            if (!ModelState.IsValid)
            {
                return View(user);
            }
            else
            {
                if (userImage != null && userImage.ContentType.ToLower() == "image/jpeg")
                {
                    BinaryReader br = new BinaryReader(userImage.InputStream);
                    user.Image = br.ReadBytes(userImage.ContentLength);
                }
                manager.RegisterNewUser(user);
                return RedirectToAction("Login");
            }
        }

        /// <summary>
        /// The login page.
        /// </summary>
        /// <returns>view of the login page</returns>
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// Post of the action login.
        /// </summary>
        /// <param name="viewModel">login view model with user name and password fields</param>
        /// <param name="ReturnUrl">the last url that the user went</param>
        /// <returns>if authentication succeded it redirecting to the last url, else error message</returns>
        [HttpPost]
        public ActionResult Login(LoginVm viewModel,string ReturnUrl)
        {
            UsersManager um = new UsersManager();
            if (ModelState.IsValid && um.IsAuthenticated(viewModel.Username, viewModel.Password))
            {
                FormsAuthentication.SetAuthCookie(viewModel.Username, false);
                if (ReturnUrl != null)
                    return Redirect(ReturnUrl);

                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "שם משתמש או סיסמא אינם נכונים");
            return View(viewModel);
        }

        /// <summary>
        /// signing out the user.
        /// </summary>
        /// <returns>redirecting to the home page</returns>
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return Redirect("/Home/Index");
        }

        /// <summary>
        /// The car search engine
        /// </summary>
        /// <returns>the view of the carsearch page</returns>
        [HttpGet]
        public ActionResult CarSearch()
        {
            return View();
        }

        /// <summary>
        /// Post of the car search.
        /// </summary>
        /// <param name="viewModel">the details of the requseted seacrh</param>
        /// <returns>list of cars that matched to the details to the view</returns>
        [HttpPost]
        public ActionResult CarSearch(CarSearchVm viewModel)
        {
           var carList = c_manager.GetCarsBySearch(viewModel);
            return View(carList);
        }

        /// <summary>
        /// Calculating view of a rental.
        /// </summary>
        /// <param name="carNumber">the car number of the car to rent</param>
        /// <returns>the car details to the view if exist, else null</returns>
        [HttpPost]
        public ActionResult RentalCalculator(string carNumber)
        {
            if(ModelState.IsValid)
            {
                Car carInDb = c_manager.FindCar(int.Parse(carNumber));
                if(carInDb !=null)
                {
                    return View(carInDb);
                }
            }
            return View();
        }

        /// <summary>
        /// The calculate page.
        /// </summary>
        /// <returns>view of the rental calculator page</returns>
        [HttpGet]
        public ActionResult RentalCalculator()
        {
            return View();
        }
    }
}