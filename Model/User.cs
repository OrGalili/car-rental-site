using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    public enum Gender { male, female }
   
    /// <summary>
    /// User entity. user can have many roles and many car rentals.
    /// related to carRental and role entities.
    /// </summary>
    public class User
    {
        //[Key]
        public int Id { get; set; }
       
        //למצוא את הנוסחה
        [Required(ErrorMessage="יש למלא שדה תעודת זהות"),RegularExpression("[0-9]{9}",ErrorMessage="תעודת זהות אינה חוקית")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "יש למלא שדה שם פרטי"), MinLength(2, ErrorMessage = "שם פרטי צריך לפחות 2 תווים"), MaxLength(20), RegularExpression("[a-zA-Z'א-ת]*", ErrorMessage = "שם פרטי אינו חוקי")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "יש למלא שדה שם משפחה"), MinLength(2, ErrorMessage = "שם משפחה צריך לפחות 2 תווים"), MaxLength(20), RegularExpression("[a-zA-Z'א-ת]*", ErrorMessage = "שם משפחה אינו חוקי")]
        public string  LastName { get; set; }

        [Required(ErrorMessage="יש למלא שדה כינוי"),MinLength(2,ErrorMessage="כינוי צריך לפחות 2 תווים"),MaxLength(15)]
        public string Username { get; set; }

        [Required(ErrorMessage="יש למלא פרטי תאריך לידה"),Above18Years]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "מין לא מוגדר")]
        public Gender? Gender { get; set; }

        [Required(ErrorMessage="יש למלא שדה אימייל"),EmailAddress(ErrorMessage="האימייל שכתבת אינו עומד בהגדרות כתיבת אימייל")]
        public string Email { get; set; }
        
        [Required(ErrorMessage="יש למלא את שדה הסיסמא"),MinLength(6,ErrorMessage="אורך סיסמא מינמאלי הוא 6")]
        public string Password { get; set; }

        public byte[] Image { get; set; }

        public virtual ICollection<CarRental> carRentals { get; set; }

        public virtual ICollection<Role> Roles { get; set; }

        public User()
        {
            Roles = new HashSet<Role>();
            carRentals = new HashSet<CarRental>();
        }
        //check why not set straight the field 'Roles' to hash set? maybe beacuse the DB cant manage with it
        /*public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();

            if(UserId<=1000000 || UserId>=100000000)
                errors.Add(new ValidationResult("תעודת זהות אינה חוקית"));

            return errors;
        }*/

    }
}
