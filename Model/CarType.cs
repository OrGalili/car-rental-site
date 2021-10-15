using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    /// <summary>
    /// CarType entity.
    /// one car type can have many cars.
    /// related to car entity.
    /// </summary>
    public class CarType
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage="יש למלא את שדה שם יצרן") ,MinLength(2,ErrorMessage="שם יצרן חייב להכיל לפחות שני תווים")]
        public string  Manufacturer { get; set; }
        
        [Required(ErrorMessage="יש למלא את שדה שם הדגם")]
        public string Model { get; set; }

        [Required(ErrorMessage = "יש למלא שדה עלות יומית"), RegularExpression("[0-9]*.[0-9]*", ErrorMessage = "עלות יומית לא יכולה להכיל תו שאינו ספרה")]
        public decimal DailyCost { get; set; }

        [Required]
        public Gear Gear { get; set; }

        public virtual ICollection<Car> Cars { get; set; }
    }

    public enum Gear
    {
        Manual,
        Auto
    }
}
