using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Model;
using BL;

namespace MvcUI.nadav.Models
{
    public class CarVM
    {
        public int Id { get; set; }

        [Required, RegularExpression("[0-9]*", ErrorMessage = "קילומטראז' מכיל אך ורק מספרים")]
        public int CurrentMileage { get; set; }

        public byte[] Image { get; set; }

        [Required]
        public bool RightToRent { get; set; }

        [Required, RegularExpression("[0-9]{7}", ErrorMessage = "מספר רכב אינו חוקי")]
        public int CarNumber { get; set; }

        [Required, RegularExpression("[0-9]*", ErrorMessage = "עלות יום איחור מכיל אך ורק מספרים")]
        public decimal CostOfDayDelay { get; set; }

        public string Manufacturer { get; set; }

        public string Model { get; set; }

        public string BranchName { get; set; }

        public Car CreateCarObject()
        {
            BranchManager b_manager = new BranchManager();
            CarTypeManager ct_manager = new CarTypeManager();
            CarRentalManager cr_manager = new CarRentalManager();
            CarsManager c_manager = new CarsManager();

            /*Car car = c_manager.FindCar(CarNumber);
            if(car == null)
            {

            }*/

            return new Car
            {
                Id = Id,
                CarNumber = CarNumber,
                CostOfDayDelay = CostOfDayDelay,
                CurrentMileage = CurrentMileage,
                Image = Image,
                RightToRent = RightToRent,
                Branch = b_manager.FindBranch(BranchName),
                CarType = ct_manager.FindCarType(Manufacturer, Model),
                CarRentals =cr_manager.GetCarRentals(Id)
            };
        }
    }
}