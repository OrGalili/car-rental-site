using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    /// <summary>
    /// CarRental Entity. one rent have one car and one user
    /// related to car and user entities.
    /// </summary>
    public class CarRental
    {
        public int Id { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime ReturnDate { get; set; }
        [Required]
        public DateTime ActualReturnDate { get; set; }
        
        public virtual User User { get; set; }
       
        public virtual Car Car { get; set; }

    }
}
