using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// Role entity. one role can have many users.
    /// related to user entity.
    /// </summary>
    public class Role
    {
        public const int MAX_ROLENAME_LENGTH = 100;

        public int RoleId { get; set; }

        [Required, StringLength(MAX_ROLENAME_LENGTH), /*Index(IsUnique = true)*/]
        public string RoleName { get; set; }

        public virtual ICollection<User> Users { get; set; }

        public Role()
        {
            Users = new HashSet<User>();
        }

        //check why not set straight the field 'Users' to hash set? maybe beacuse the DB cant manage with it
    }
}
