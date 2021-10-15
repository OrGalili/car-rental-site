using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BL;
using Model;

namespace MvcUI.nadav.Controllers
{
    /// <summary>
    /// Controller for all kinds of data from the Db.
    /// </summary>
    public class LookupController : ApiController
    {
        private CarTypeManager ct_manager;
        private CarsManager c_manager;
        private CarRentalManager cr_manager;
        private UsersManager u_manager;
        private BranchManager b_manager;

        /// <summary>
        /// Constructor.
        /// </summary>
        public LookupController()
        {
            ct_manager = new CarTypeManager();
            c_manager = new CarsManager();
            cr_manager = new CarRentalManager();
            u_manager = new UsersManager();
            b_manager = new BranchManager();
        }

        /// <summary>
        /// Bring a list of car types
        /// </summary>
        /// <returns>list of car types</returns>
        [HttpGet]
        [Route("lookup/types")]
        public List<string> GetCarTypes()
        {
            var allCarTypes = ct_manager.GetAllTypes();
            return new List<string>(allCarTypes);
        }

        /// <summary>
        /// Bring from Db all the model names.
        /// </summary>
        /// <param name="manu">the manufactor name of the models</param>
        /// <returns>list of models that related to the manufactor name</returns>
        [HttpGet]
        [Route("lookup/models/{manu}")]
        public List<string> GetModelForType(string manu)
        {
            var models = ct_manager.GetModelsForType(manu).ToList();
            return models;
        }

        /// <summary>
        /// Bring Details of a cpecific car.
        /// </summary>
        /// <param name="carNumber">the car number of the car to get details</param>
        /// <returns>car entity</returns>
        [HttpGet]
        [Route("lookup/carDetails/{carNumber}")]
        public Car GetCarDetails(string carNumber)
        {
            Car theCarDetails = c_manager.FindCar(int.Parse(carNumber));
            return theCarDetails;
        }

        /// <summary>
        /// Checking if a specific car is available for rent in a given date.
        /// </summary>
        /// <param name="carNumber">the car number of the car to check</param>
        /// <param name="startDate">start date rent</param>
        /// <param name="endDate">end date of the rent</param>
        /// <returns>true if the car is available in that time , else false</returns>
        [HttpGet]
        [Route("lookup/isAvailableTime/")]
        public bool IsAvailableForRent(string carNumber,DateTime startDate, DateTime endDate)
        {
            return cr_manager.IsAvailableForRent(int.Parse(carNumber), startDate, endDate);
        }

        /// <summary>
        /// Bring all car numbers that in Db.
        /// </summary>
        /// <returns>int array of car numbers that in Db</returns>
        [HttpGet]
        [Route("lookup/carNumbers")]
        public int[] GetCarNumbers()
        {
            return c_manager.CarNumbersInDb();
        }

        /// <summary>
        /// Bring all user names that in Db.
        /// </summary>
        /// <returns>string array of user names that in Db</returns>
        [HttpGet]
        [Route("lookup/usernames")]
        public string[] GetUserNames()
        {
            return u_manager.UsernamesInDb();
        }

        /// <summary>
        /// Bring the branch names that in Db.
        /// </summary>
        /// <returns>string array of Branch names that in Db</returns>
        [HttpGet]
        [Route("lookup/branchNames")]
        public string[] GetBranchNames()
        {
            return b_manager.BranchNamesInDb();
        }
    }
}
