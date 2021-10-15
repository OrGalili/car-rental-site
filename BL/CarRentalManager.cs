using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using DAL;
using System.Data.Entity;
using System.Xml.Linq;
using System.Linq.Expressions;
using System.Data.Linq;

namespace BL
{
    /// <summary>
    /// Manage the CarRental entities that in Db.
    /// </summary>
    public class CarRentalManager:IDisposable
    {
        CarsRentalContext context = new CarsRentalContext();

        /// <summary>
        /// Indicating if a spesific car is available in a certain time.
        /// </summary>
        /// <param name="carNumber">the car number of the car to check</param>
        /// <param name="startDate">the start date to rent</param>
        /// <param name="EndDate">the end date to rent</param>
        /// <returns>true if the car is available in that time, else false</returns>
        public bool IsAvailableForRent(int carNumber, DateTime startDate, DateTime EndDate)
        {
            Car car = context.Cars.Include("CarRentals").FirstOrDefault(c => c.CarNumber == carNumber);

            bool isAvilable = car.CarRentals.All(rental => rental.StartDate.CompareTo(EndDate) == -1 && rental.ReturnDate.CompareTo(startDate) == -1
                ||rental.StartDate.CompareTo(EndDate) == 1 && rental.ReturnDate.CompareTo(startDate) == 1);

            return isAvilable;
        }

        /// <summary>
        /// Adding a new row in the car Rental table that in the Db.
        /// </summary>
        /// <param name="username">the username of the user who wants to rent</param>
        /// <param name="carNumber">the car number of the car to rent</param>
        /// <param name="startDate">the start date to rent</param>
        /// <param name="endDate">the end date to rent</param>
        /// <returns>true if the add succeded , else false</returns>
        public bool AddRent(string username, int carNumber, DateTime startDate, DateTime endDate)
        {
            User user = context.Users.Where(u => u.Username == username).SingleOrDefault();
            Car car = context.Cars.Where(c => c.CarNumber == carNumber).SingleOrDefault();

            if (user != null && car != null)
            {
                CarRental rent = new CarRental()
                {
                    User = user,
                    Car = car,
                    StartDate = startDate,
                    ReturnDate = endDate,
                    ActualReturnDate = endDate
                };

                context.CarsRentals.Add(rent);
                context.SaveChanges();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Check the data rent of a specific user.
        /// </summary>
        /// <param name="username">the user name of the user to check</param>
        /// <returns>a list of rents of the user given</returns>
        public IEnumerable<CarRental> userRentalsHistory(string username)
        {
            return context.CarsRentals.Where(c => c.User.Username == username).ToArray();
        }

        /// <summary>
        /// takes all rentals in Db and translating it to a CarRentalVM object.
        /// </summary>
        /// <returns>a list of CarRentalVM objects</returns>
        public IEnumerable<object> allRentalsInDbAsVM()
        {
            IEnumerable<CarRental> rentals = context.CarsRentals.Include(c => c.Car).Include(r => r.User).ToArray();

            foreach (CarRental rental in rentals)
            {
                User user = context.Users.Find(rental.User.Id);
                Car car = context.Cars.Find(rental.Car.Id);
                context.Entry(rental).State = EntityState.Detached;
                context.Entry(user).State = EntityState.Detached;
                context.Entry(car).State = EntityState.Detached;
                rental.Car = car;
                rental.User = user;
            }

            var rentalsVM = from rental in rentals 
                        select new
                        {
                            rental.Id,
                            rental.StartDate,
                            rental.ReturnDate,
                            rental.ActualReturnDate,
                            rental.User.Username,
                            rental.Car.CarNumber
                        };

            return rentalsVM.Reverse();
        }

        /// <summary>
        /// check if a given user have rents in the rental table that in Db.
        /// </summary>
        /// <param name="id">the id of the user given</param>
        /// <returns>true if the user not have rentals , else false</returns>
        public bool UserNotHaveRentalsHistory(int id)
        {
            return !context.CarsRentals.Any(c => c.User.Id== id);
        }

        /// <summary>
        /// Deleting a carRental entity in the Db.
        /// </summary>
        /// <param name="id">id of the carRental entity to delete</param>
        /// <returns>true if delete succeded , else false</returns>
        public bool DeleteRent(int id)
        {
            CarRental rental = context.CarsRentals.Find(id);

            if (rental != null)
            {
                context.CarsRentals.Remove(rental);
                context.SaveChanges();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Updating the ActualReturnDate field of an existing entity of CarRental.
        /// </summary>
        /// <param name="id">the id of the car rental entity</param>
        /// <returns>true if update succeded, else false</returns>
        public bool UpdataActualReturnDateRentalToNow(int id)
        {
            CarRental carRental = context.CarsRentals.Find(id);
            if(carRental!=null)
            {
                context.Entry(carRental).State = EntityState.Modified;
                carRental.ActualReturnDate = DateTime.Now;
                context.SaveChanges();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Updating the fields of an existing entity of CarRental According to the data of the parameter.
        /// </summary>
        /// <param name="rental">the rental with the updated fields</param>
        /// <returns>true if the update succeded , else false</returns>
        public bool UpdateRent(CarRental rental)
        {
            IEnumerable<CarRental> carRentals = rental.Car.CarRentals.Where(cr => cr.Id != rental.Id).ToArray();
            bool Avialable = carRentals.All(cr => cr.StartDate.CompareTo(rental.ReturnDate) == -1 && cr.ReturnDate.CompareTo(rental.StartDate) == -1 ||
            cr.StartDate.CompareTo(rental.ReturnDate) == 1 && cr.ReturnDate.CompareTo(rental.StartDate) == 1);

            if (Avialable)
            {
                try
                {
                    CarRental r = context.CarsRentals.Include(cr => cr.User).Include(cr => cr.Car).First(cr => cr.Id == rental.Id);
                    context.Entry(r).State = EntityState.Modified;
                    r.Car = context.Cars.Find(rental.Car.Id);
                    r.User = context.Users.Find(rental.User.Id);
                    r.StartDate = rental.StartDate;
                    r.ReturnDate = rental.ReturnDate;
                    r.ActualReturnDate = rental.ActualReturnDate;
                    context.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {

                }
            }
            return false;
        }

        /// <summary>
        /// Finding a car rental in Db according to his id.
        /// </summary>
        /// <param name="id">the id of the car rental entity to get</param>
        /// <returns>the car rental entity that requested</returns>
        public CarRental FindRental(int id)
        {
            return context.CarsRentals.Find(id);
        }

        /// <summary>
        /// The sum of a specific rent.
        /// calculating according to a dayli cost and day delay cost if passed the return date.
        /// </summary>
        /// <param name="id">the id of the car rental to calculate his sum</param>
        /// <returns>the sum to pay</returns>
        public decimal SumRental(int id)
        {
            CarRental carRental = FindRental(id);
            TimeSpan difference;
            decimal costOfDayDelay = context.Cars.Where(c => c.Id == carRental.Car.Id).Select(c => c.CostOfDayDelay).SingleOrDefault();
            decimal dailyCost = context.CarTypes.Where(ct => ct.Id == carRental.Car.CarType.Id).Select(ct => ct.DailyCost).SingleOrDefault();

            if (carRental.ActualReturnDate > carRental.ReturnDate)
            {
                difference = carRental.ActualReturnDate - carRental.ReturnDate;
                decimal sumCostOfDayDelays = costOfDayDelay * difference.Days;
                difference = carRental.ReturnDate - carRental.StartDate;
                decimal sumDailyCost = dailyCost * difference.Days;

                return sumCostOfDayDelays + sumDailyCost;
            }

            else
            {
                difference = carRental.ActualReturnDate - carRental.StartDate;
                return dailyCost * difference.Days;
            }  
        }

        /// <summary>
        /// Getting from Db all the rentals of a specific car.
        /// </summary>
        /// <param name="id">the id of the specific car</param>
        /// <returns>list of car Rentals of the given car</returns>
        public ICollection<CarRental> GetCarRentals(int id)
        {
            return context.CarsRentals.Where(cr => cr.Car.Id == id).ToArray();
        }

        /// <summary>
        /// Getting from Db all the rentals of a specific car.
        /// </summary>
        /// <param name="carNumber">the car number of the specific car</param>
        /// <returns>list of car Rentals of the given car</returns>
        public ICollection<CarRental> GetCarRentalsByCarNumber(int carNumber)
        {
            ICollection<CarRental> carRentals = context.CarsRentals.Where(cr => cr.Car.CarNumber == carNumber).ToArray();
            User user ;
            foreach(CarRental carRental in carRentals)
            {
                user = context.Users.Find(carRental.User.Id);
                context.Entry(carRental).State = EntityState.Detached;
                context.Entry(user).State = EntityState.Detached;
                carRental.User = user;
            }

            return carRentals;
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
