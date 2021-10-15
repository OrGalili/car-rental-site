using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    /// <summary>
    /// the car entity. 
    /// one car can be rented many times, but not in the same date.
    /// related to carType, Branch and carRental entities.
    /// </summary>
    public class Car
    {
        public int Id { get; set; }

        [Required, RegularExpression("[0-9]*", ErrorMessage = "קילומטראז' מכיל אך ורק מספרים")]
        public int CurrentMileage { get; set; }

        public byte[] Image { get; set; }

        [Required]
        public bool RightToRent { get; set; }

        [Required, RegularExpression("[0-9]{7}", ErrorMessage = "מספר רכב אינו חוקי")]
        public int CarNumber { get; set; }

        [Required, RegularExpression("[0-9]*.[0-9]*", ErrorMessage = "עלות יום איחור מכיל אך ורק מספרים")]
        public decimal CostOfDayDelay { get; set; }

        public virtual CarType CarType { get; set; }
        public virtual Branch Branch { get; set; }
        public virtual ICollection<CarRental> CarRentals { get; set; }
    }
}
