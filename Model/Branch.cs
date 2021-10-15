using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    /// <summary>
    /// Branch entity.
    /// one branch can have many cars.
    /// related to car entity.
    /// </summary>
    public class Branch
    {
        public int Id { get; set; }
        
        [Required]
        public string Address { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<Car> Cars { get; set; }
    }
}
