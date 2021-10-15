using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Model;
using System.Data.Entity;
using System.Data.Linq;
using System.Xml.Linq;

namespace BL
{
    /// <summary>
    /// Manage the user entities that in Db.
    /// </summary>
    public class UsersManager : IDisposable
    {
        public CarsRentalContext CarsRentalContext = new CarsRentalContext();

        /// <summary>
        /// Constructor.
        /// </summary>
        public UsersManager()
        {
            CarsRentalContext = new CarsRentalContext();
        }

        /// <summary>
        /// Inserting new user in Db.
        /// </summary>
        /// <param name="user">the user entity to insert in Db</param>
        /// <returns>the user entity</returns>
        public User RegisterNewUser(User user)
        {
            user.Roles.Add(CarsRentalContext.Roles.Find(1));
            CarsRentalContext.Users.Add(user);
            CarsRentalContext.SaveChanges();
            return user;
        }

        /// <summary>
        /// Disposing context.
        /// </summary>
        public void Dispose()
        {
            CarsRentalContext.Dispose();
        }

        /// <summary>
        /// Check in Db if the user name exist and if this is his password.
        /// </summary>
        /// <param name="username">the user name to check</param>
        /// <param name="password">the password to check</param>
        /// <returns>true if there is a match bettween user name and password, else false</returns>
        public bool IsAuthenticated(string username, string password)
        {
            return CarsRentalContext.Users.Any(u => u.Username == username && u.Password == password);
        }

        /// <summary>
        /// Brings all users in Db.
        /// </summary>
        /// <returns>list of user entities that in Db</returns>
        public IEnumerable<User> GetAllUsers()
        {
            return CarsRentalContext.Users.ToArray();
        }

        /// <summary>
        /// Brings list of users and their related roles.
        /// this function created for transfer data to client side by ajax without errors.
        /// </summary>
        /// <returns>a list of user and his roles objects</returns>
        public IEnumerable<object> getAllUsersAsJSON()
        {
            //יש בעיה בהעברת מידע כשהשדות וירטואליות
            IEnumerable<User> users = CarsRentalContext.Users.Include(u => u.Roles).ToList();
            IEnumerable<Role> roles = CarsRentalContext.Roles.Include(r => r.Users).ToList();

            var usersTableData = from user in users
                                 select new
                                 {
                                     user.BirthDate,
                                     user.Password,
                                     user.Id,
                                     user.FirstName,
                                     user.LastName,
                                     user.UserId,
                                     user.Username,
                                     user.Image,
                                     user.Gender,
                                     user.Email
                                 };

            Dictionary<string, List<string>> usersAndRoles = new Dictionary<string, List<string>>();

            List<string> roleNames = new List<string>();


            foreach (Role role in roles)
            {
                foreach (User user in role.Users)
                {
                    foreach (var userInTable in usersTableData)
                    {
                        if (userInTable.Username == user.Username)
                        {
                            if (!usersAndRoles.ContainsKey(user.Username))
                            {
                                usersAndRoles.Add(user.Username, new List<string> { role.RoleName });
                            }
                            else
                            {
                                usersAndRoles.TryGetValue(user.Username, out roleNames);
                                roleNames.Add(role.RoleName);
                                usersAndRoles.Remove(user.Username);
                                usersAndRoles.Add(user.Username, roleNames.ToList());
                                roleNames.Clear();
                            }
                            break;
                        }
                    }
                }
            }
            //usersTableData and usersAndRoles
            var allUsers = from user in usersTableData
                           join userAndRole in usersAndRoles
                           on user.Username equals userAndRole.Key
                           select new
                           {
                               user.Id,
                               user.Username,
                               user.Password,
                               user.FirstName,
                               user.LastName,
                               user.UserId,
                               user.BirthDate,
                               user.Gender,
                               user.Email,
                               UserRoles = userAndRole.Value
                           };

            return allUsers.Reverse();
        }

        /// <summary>
        /// Check if a spesific user name exist in a given list of user names.
        /// </summary>
        /// <param name="usernames">list of user names</param>
        /// <param name="username">the user name to check if exist</param>
        /// <returns>true if the user name exist in the list , else false</returns>
        private bool isUserExict(string[] usernames, string username)
        {
            foreach (string name in usernames)
            {
                if (name == username)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Deleting a user from the Db.
        /// </summary>
        /// <param name="id">the id of the user to delete</param>
        /// <returns>true if the delete succeded, else false</returns>
        public bool deleteUser(int id)
        {
            User user = CarsRentalContext.Users.Find(id);

            if (user == null)
                return false;

            CarsRentalContext.Users.Remove(user);
            CarsRentalContext.SaveChanges();
            return true;
        }

        /// <summary>
        /// Adding a new user to the Db.
        /// this function created for creating a new entity by ajax in crud users controller.
        /// </summary>
        /// <param name="user">the user entity to add</param>
        /// <param name="roles">the roles of the new user</param>
        /// <returns>true if the add has succeded , else false</returns>
        public bool addUser(User user,string[] roles)
        {
            User userInDb = CarsRentalContext.Users.Where(u => u.Username == user.Username || u.UserId == user.UserId).SingleOrDefault();

            if (userInDb != null)
                return false;


            UpdateUserRoles(user, roles);
            EntityState us = CarsRentalContext.Entry(user).State;
            CarsRentalContext.Users.Add(user);
            us = CarsRentalContext.Entry(user).State;
            CarsRentalContext.SaveChanges();
            return true;
        }

        /// <summary>
        /// Updating an exist user that in Db. 
        /// </summary>
        /// <param name="userUpdates">the user entity updates</param>
        /// <param name="roles">the roles of the user</param>
        /// <returns>true if the update succeded, else false</returns>
        public bool updateUser(User userUpdates,string[] roles)
        {
            UpdateUserRoles(userUpdates, roles);
            CarsRentalContext.SaveChanges();
            return true;
        }

        /// <summary>
        /// Check if a specicfic user name exist in a given users data.
        /// </summary>
        /// <param name="username">the user name to check</param>
        /// <param name="users">the list of users</param>
        /// <returns>true if the user name exist , else false</returns>
        public bool usernameExist(string username, IEnumerable<User> users)
        {
            return users.Any(u => u.Username == username);
        }

        /// <summary>
        /// Check if a specific user id exist in a given users data.
        /// </summary>
        /// <param name="userId">the user id to check</param>
        /// <param name="users">the list of users</param>
        /// <returns>true if the user id exist in the list, else false</returns>
        public bool userIdExist(int userId, IEnumerable<User> users)
        {
            return users.Any(u => u.UserId == userId);
        }

        /// <summary>
        /// Updating roles of a specific user.
        /// </summary>
        /// <param name="user">the user to update</param>
        /// <param name="roles">the roles updates to the user</param>
        public void UpdateUserRoles(User user, string[] roles)
        {
            User userInDb = CarsRentalContext.Users.Where(u => u.Id == user.Id).SingleOrDefault();

            if (userInDb != null)
            {
                //ברגע שלוחצים הרבה על לחצן ערוך קופץ שגיאת חריגה
                EntityState us = CarsRentalContext.Entry(userInDb).State;
                userInDb.Roles.Clear();
                CarsRentalContext.SaveChanges();
                CarsRentalContext.Entry(userInDb).State = EntityState.Detached;
            }
            CarsRentalContext.Entry(user).State = EntityState.Modified;
            foreach (string roleName in roles)
            {
                user.Roles.Add(CarsRentalContext.Roles.Include(u => u.Users).Where(r => r.RoleName == roleName).SingleOrDefault());
            }
        }

        /// <summary>
        /// Bring a user entity as an object.
        /// </summary>
        /// <param name="user">the user to translate to object</param>
        /// <returns>an object with a user data</returns>
        public object getUserAsJSON(User user)
        {
            IEnumerable<Role> roles = CarsRentalContext.Roles.ToArray();

            List<string> UserRoleNames = new List<string>();

            foreach (Role role in roles)
            {
                foreach (Role roleUser in user.Roles)
                {
                    if (role.RoleName == roleUser.RoleName)
                        UserRoleNames.Add(roleUser.RoleName);
                }
            }

            var userAsJSON = new
            {
                user.Id,
                user.UserId,
                user.Username,
                user.FirstName,
                user.LastName,
                user.Password,
                user.Image,
                user.Gender,
                user.Email,
                user.BirthDate,
                UserRoles = UserRoleNames
            };

            return userAsJSON;

        }

        /// <summary>
        /// Brings all users in the Db excpet one.
        /// </summary>
        /// <param name="id">the id of the user to not get in the list</param>
        /// <returns>a list of users without a specific user</returns>
        public IEnumerable<User> GetUsersWithoutUser(int id)
        {
            return CarsRentalContext.Users.Where(u => u.Id != id).ToArray();
        }
        
        /// <summary>
        /// Bring all user names that in Db.
        /// </summary>
        /// <returns>string array with all user names that in Db</returns>
        public string[] UsernamesInDb()
        {
            return CarsRentalContext.Users.Include(us => us.Roles).Where(u => u.Roles.Any(r => r.RoleName == "User")).Select(u => u.Username).ToArray();
        }

        /// <summary>
        /// Bring a user that in Db.
        /// </summary>
        /// <param name="username">the user name of the user to get from the Db</param>
        /// <returns>user entity if the user name exist in Db , else null</returns>
        public User FindUser(string username)
        {
            return CarsRentalContext.Users.Where(u => u.Username == username).SingleOrDefault();
        }

        /// <summary>
        /// Bring the image of a specific user that in Db.
        /// </summary>
        /// <param name="id">the id of the user</param>
        /// <returns>byte array of the image if exist , else null</returns>
        public byte[] GetImageById(int id)
        {
            return CarsRentalContext.Users.Where(u => u.Id == id).Select(u => u.Image).SingleOrDefault();
        }

        /// <summary>
        /// Brings all id(the primary key) users that in Db.
        /// </summary>
        /// <returns>list of id users</returns>
        public int[] GetAllIdsInUserTable()
        {
            return CarsRentalContext.Users.Select(u => u.Id).ToArray();
        }

        /// <summary>
        /// Inserting an image to a specific user.
        /// </summary>
        /// <param name="id">the id of the user to insert image</param>
        /// <param name="image">the image to insert to the user</param>
        /// <returns>true if insert succeded , else false</returns>
        public bool insertImageToUser(int id , byte[] image)
        {
            User user = CarsRentalContext.Users.Find(id);
            if (user != null)
            {
                CarsRentalContext.Entry(user).State = EntityState.Modified;
                user.Image = image;
                CarsRentalContext.SaveChanges();
                return true;
            }

            return false;
        }
    }
}
