using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// validation attribute : check if the user is above 18 years.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class Above18YearsAttribute : ValidationAttribute
    {
        public Above18YearsAttribute()
        {
            ErrorMessage = "משתמש במערכת חייב להיות לפחות בן 18";
        }

        public override bool IsValid(object value)
        {
            if (value is DateTime)
            {
                DateTime valueAsDateTime = (DateTime)value;
                TimeSpan difference = DateTime.Now.Subtract(valueAsDateTime);
                int years = difference.Days / 365;
                return years >=18;
            }
            else
                return false;
        }
    }


}
