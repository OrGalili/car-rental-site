using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using BL;
using Model;

namespace MvcUI.nadav.Models
{
    public class CarRentalVM
    {
        public int Id { get; set; }
        
        [Required]
        public DateTime StartDate { get; set; }
        
        [Required]
        public DateTime ReturnDate { get; set; }
        
        [Required]
        public DateTime ActualReturnDate { get; set; }

        public string Username { get; set; }

        public int CarNumber { get; set; }

        public CarRental createCarRentalObject()
        {
            UsersManager u_manager = new UsersManager();
            CarsManager c_manager = new CarsManager();

            return new CarRental
            {
                Id = Id,
                StartDate = StartDate,
                ReturnDate = ReturnDate,
                ActualReturnDate = ActualReturnDate,
                Car = c_manager.FindCar(CarNumber),
                User = u_manager.FindUser(Username)
            };
        }
    }
}