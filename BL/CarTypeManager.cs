using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Model;
using System.Data.Entity;

namespace BL
{
    /// <summary>
    /// Manage the carType entities that in Db.
    /// </summary>
    public class CarTypeManager
    {
        private CarsRentalContext context;

        /// <summary>
        /// constructor.
        /// </summary>
        public CarTypeManager()
        {
            context = new CarsRentalContext();
            context.Configuration.ProxyCreationEnabled = false;
            context.Configuration.LazyLoadingEnabled = false;
        }

        /// <summary>
        /// Deleting a car type entity that in the Db.
        /// </summary>
        /// <param name="id">the id of the car to delete</param>
        /// <returns>true if the delete succeded, else false</returns>
        public bool DeleteCarType(int id)
        {
            CarType carType = context.CarTypes.Find(id);
            if (carType != null)
            {
                context.CarTypes.Remove(carType);
                context.SaveChanges();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Updating an existing car type that in the Db according to the data of the parameter.
        /// </summary>
        /// <param name="cartype">the car type with the updates</param>
        /// <returns>true if the car type updated, else false</returns>
        public bool UpdateCarType(CarType cartype)
        {
            IEnumerable<CarType> carTypesInDb = context.CarTypes.Where(ct => ct.Id != cartype.Id).ToArray();
            bool Exist = carTypesInDb.Any(ct => ct.Manufacturer == cartype.Manufacturer && ct.Model == cartype.Model);

            if(!Exist)
            {
                context.Entry(cartype).State = EntityState.Modified;
                context.SaveChanges();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Inserting new car type row to the Db.
        /// </summary>
        /// <param name="cartype">the car type entity to insert in the Db</param>
        /// <returns>true if the insert succeded, else false</returns>
        public bool CreateNewCarType(CarType cartype)
        {
            bool carTypeIsExist = context.CarTypes.Any(c => c.Manufacturer == cartype.Manufacturer && c.Model == cartype.Model);
            if (!carTypeIsExist)
            {
                context.CarTypes.Attach(cartype);
                //context.Entry(cartype).State = EntityState.Detached;
                context.Entry(cartype).State = EntityState.Added;
                context.SaveChanges();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Get from the Db all the manufactor names.
        /// </summary>
        /// <returns>string list of all manufactor names that in Db</returns>
        public IQueryable<string> GetAllTypes()
        {
            return context.CarTypes.Select(t => t.Manufacturer).Distinct();
        }

        /// <summary>
        /// Get the models of the car type with a specific manufactor name.
        /// </summary>
        /// <param name="manufacturer">the manfactor name</param>
        /// <returns>the model names of the manufactor in list</returns>
        public IQueryable<string> GetModelsForType(string manufacturer)
        {
            return context.CarTypes.Where(t => t.Manufacturer == manufacturer).Select(ct => ct.Model);
        }

        /// <summary>
        /// Get all car types that in Db.
        /// </summary>
        /// <returns>list of all car type etities in Db</returns>
        public IEnumerable<CarType> CarTypesInDb()
        {
            return context.CarTypes.ToArray().Reverse();
        }

        /// <summary>
        /// Indicates if a carType have cars.
        /// </summary>
        /// <param name="id">the id of the car type</param>
        /// <returns>true if have cars, else false</returns>
        public bool CarTypeRelatedToCars(int id)
        {
            return context.Cars.Any(c => c.CarType.Id == id);
        }

        /// <summary>
        /// Getting a car type according to manufactor name and model name.
        /// </summary>
        /// <param name="manufacturer">the manufcator name to search</param>
        /// <param name="model">the model name to search</param>
        /// <returns>car type entity</returns>
        public CarType FindCarType(string manufacturer ,string model)
        {
            return context.CarTypes.Where(cr => cr.Manufacturer == manufacturer && cr.Model == model).SingleOrDefault();
        }
    }
}
