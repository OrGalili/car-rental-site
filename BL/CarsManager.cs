using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.SearchModels;
using DAL;
using Model;
using System.Data.Entity.Validation;

namespace BL
{
    /// <summary>
    /// Manage the car entities that in Db.
    /// </summary>
    public class CarsManager: IDisposable
    {
        CarsRentalContext context = new CarsRentalContext();

        /// <summary>
        /// Add a new car entity in the Db.
        /// </summary>
        /// <param name="car">the car entity to add</param>
        /// <returns>true if the add succeded, else false</returns>
        public bool CreateNewCar(Car car)
        {
            bool CarExist = context.Cars.Any(c => c.CarNumber == car.CarNumber);
            CarType carType = context.CarTypes.Find(car.CarType.Id);
            Branch branch = context.Branchs.Find(car.Branch.Id);


            if (!CarExist && carType != null && branch != null)
            {
                Car newCar = new Car
                {
                    CarNumber = car.CarNumber,
                    CarType = carType,
                    Branch = branch,
                    CurrentMileage = car.CurrentMileage,
                    Image = car.Image,
                    RightToRent = car.RightToRent,
                    CostOfDayDelay = car.CostOfDayDelay
                };

                context.Cars.Add(newCar);
                context.SaveChanges();
                car.Id = newCar.Id;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Delete a car entity that in Db.
        /// </summary>
        /// <param name="id">the id of the entity to delete</param>
        /// <returns>true if delete succeded, else false</returns>
        public bool DeleteCar(int id)
        {
            Car car = context.Cars.Find(id);
            if (car != null)
            {
                context.Entry(car).State = EntityState.Deleted;
                context.SaveChanges();
                return true;
            }

            return false;

        }

        /// <summary>
        /// Get all car entites that in Db.
        /// </summary>
        /// <returns>a list of car entities</returns>
        public IEnumerable<Car> CarsInDb()
        {
            return context.Cars.ToArray();
        }

        /// <summary>
        /// Get all car entities in Db and translating it to CarVM objects.
        /// </summary>
        /// <returns>a list of CarVM object</returns>
        public IEnumerable<object> CarsInDbAsVM()
        {
            IEnumerable<Car> cars = context.Cars.ToArray();

            var carsVM = from car in cars
                         select new
                         {
                             car.Id,
                             car.CurrentMileage,
                             car.RightToRent,
                             car.CarNumber,
                             car.CostOfDayDelay,
                             car.CarType.Manufacturer,
                             car.CarType.Model,
                             BranchName = car.Branch.Name
                         };

            return carsVM.Reverse();
        }

        /// <summary>
        /// Updating the fields of an existing entity of Car According to the data of the parameter.
        /// </summary>
        /// <param name="car">the entity with the updates</param>
        /// <returns>true if the update succede, else false</returns>
        public bool UpdateCar(Car car)
        {
            IEnumerable<Car> cars = context.Cars.Where(c => c.Id != car.Id).ToArray();
            bool exist = cars.Any(c => c.CarNumber == car.CarNumber);
            
            CarType carType = context.CarTypes.Find(car.CarType.Id);
            Branch branch = context.Branchs.Find(car.Branch.Id);
            ICollection<CarRental> carRentals = context.CarsRentals.Where(cr => cr.Car.Id == car.Id).ToArray();

            if (!exist && carType != null && branch != null)
            {

                Car carInDb = context.Cars.Find(car.Id);
                context.Entry(carInDb).State = EntityState.Modified;
                
                carInDb.CarNumber = car.CarNumber;
                carInDb.RightToRent = car.RightToRent;
                carInDb.Branch = branch;
                carInDb.CarType = carType;
                carInDb.CostOfDayDelay = car.CostOfDayDelay;
                carInDb.CurrentMileage = car.CurrentMileage;
                context.SaveChanges();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Get all cars that matches to the data of the parameter.
        /// </summary>
        /// <param name="searchObj">object with the fields to compare</param>
        /// <returns>list of all cars that answers to the parameter object</returns>
        public IEnumerable<Car> GetCarsBySearch(CarSearch searchObj)
        {
            //לכתוב בדיקות שהחיפוש עובד גם במקרה ויש פרמטר אחד לחיפוש ולא כמה במקביל
            if (searchObj == null)
            {
                throw new Exception("Search Obj Is NULL");
            }

            var query = from car in context.Cars
                .Include("CarType")
                .Include("CarRentals")
                        where (car.CarType.Manufacturer == searchObj.Manufacturer || searchObj.Manufacturer == null)
                        && (car.CarType.Model == searchObj.Model || searchObj.Model == null)
                        && (car.CarType.Gear == searchObj.GearType)
                        && (car.CarRentals.All(rental => rental.StartDate.CompareTo(searchObj.LeaseTimeTo) == -1 && rental.ReturnDate.CompareTo(searchObj.LeaseTimeFrom) == -1
                        || rental.StartDate.CompareTo(searchObj.LeaseTimeTo) == 1 && rental.ReturnDate.CompareTo(searchObj.LeaseTimeFrom) == 1) )
                        && car.RightToRent == true
                        select car;

            return query.ToList();

        }

        /// <summary>
        /// Get car entity by a car number.
        /// </summary>
        /// <param name="carNumber">the car number of the car entity</param>
        /// <returns>the car entity</returns>
        public Car FindCar(int carNumber)
        {
            return context.Cars.FirstOrDefault(c => c.CarNumber == carNumber);
        }

        /// <summary>
        /// All car numbers that in Db.
        /// </summary>
        /// <returns>int array of car numbers that in Db</returns>
        public int[] CarNumbersInDb()
        {
            return context.Cars.Where(c => c.RightToRent == true).Select(c => c.CarNumber).ToArray();
        }

        /// <summary>
        /// Check a specific car if he have rentals in Db.
        /// </summary>
        /// <param name="id">the id of the car entity</param>
        /// <returns>true if he have rentals, else false</returns>
        public bool carHaveRentals(int id)
        {
            return context.CarsRentals.Any(cr => cr.Car.Id == id);
        }

        /// <summary>
        /// Image of a specific car.
        /// </summary>
        /// <param name="id">the id of the car</param>
        /// <returns>the image of the car</returns>
        public byte[] GetImageById(int id)
        {
            return context.Cars.Where(c => c.Id == id).Select(c => c.Image).SingleOrDefault();
        }

        /// <summary>
        /// Inserting an image to a specific car.
        /// </summary>
        /// <param name="id">the id of the car entity that in Db</param>
        /// <param name="image">the image to insert to the car</param>
        /// <returns>true if the image update of the car succeded, else false</returns>
        public bool InsertImageToCar(int id , byte[] image)
        {
            Car car = context.Cars.Include(c => c.Branch).Include(c => c.CarType).Where(c => c.Id == id).SingleOrDefault();
            if (car != null)
            {
                context.Entry(car).State = EntityState.Modified;
                car.Image = image;
                context.SaveChanges();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Disposing context.
        /// </summary>
        public void Dispose()
        {
            context.Dispose();
        }
    }
}
