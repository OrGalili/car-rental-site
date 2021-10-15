using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using Model;
using DAL;
using System.Configuration.Provider;

namespace BL
{
    public class RoleManager: RoleProvider
    {
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            if (usernames == null || usernames.Any(u => u == null)) throw new ArgumentNullException("usernames");
            if (roleNames == null || roleNames.Any(r => r == null)) throw new ArgumentNullException("roleNames");
            if (usernames.Length == 0 || usernames.Any(u => u == string.Empty))
                throw new ArgumentException("usernames cannot be an empty array, and any username must not be an empty string", "usernames");
            if (roleNames.Length == 0 || roleNames.Any(r => r == string.Empty))
                throw new ArgumentException("roleNames cannot be an empty array, and any role name must not be an empty string", "roleNames");

            using (CarsRentalContext context = new CarsRentalContext())
            {
                if (!usernames.All(u => context.Users.Any(uInContext => uInContext.Username.ToLower() == u.ToLower())))
                    throw new ProviderException("One or more usernames do not exist");
                if (!roleNames.All(r => context.Roles.Any(rInContext => rInContext.RoleName.ToLower() == r.ToLower())))
                    throw new ProviderException("One or more role names do not exist");

                IEnumerable<Role> rolesInContext = context.Roles.Where(r => roleNames.Any(rn => rn.ToLower() == r.RoleName.ToLower()));
                IEnumerable<User> usersInContext = context.Users.Where(u => usernames.Any(un => un.ToLower() == u.Username.ToLower()));
                foreach (User user in usersInContext)
                {
                    IEnumerable<Role> newRolesForUser = rolesInContext.Except(user.Roles);
                    foreach (Role role in newRolesForUser)
                    {
                        user.Roles.Add(role);
                    }
                }
                context.SaveChanges();
            }
        }

        //לא צריך לממש
        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override void CreateRole(string roleName)
        {
            if (roleName == null)
            {
                throw new ArgumentNullException("roleName");
            }
            string errMsg = null;
            if (roleName == string.Empty)
            {
                errMsg = "The role name cannot be an empty string";
            }
            if (roleName.Contains(","))
            {
                errMsg = "The role name cannot contains a comma";
            }
            if (roleName.Length > Role.MAX_ROLENAME_LENGTH)
            {
                errMsg = "The role name length must be less then or equal to " + Role.MAX_ROLENAME_LENGTH;
            }
            if (errMsg != null)
            {
                throw new ArgumentException(errMsg, "roleName");
            }
            using (CarsRentalContext context = new CarsRentalContext())
            {
                Role exists = context.Roles.SingleOrDefault(r => r.RoleName.ToLower() == roleName.ToLower());
                if (exists != null)
                {
                    throw new ProviderException("The role name already exists");
                }
                Role newRole = new Role() { RoleName = roleName };
                context.Roles.Add(newRole);
                context.SaveChanges();
            }
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            if (roleName==null)
            {
                throw new ArgumentNullException("roleName");
            }
            if (roleName==string.Empty)
            {
                throw new ArgumentException("The role name cannot be an empty string");
            }
            using (CarsRentalContext context = new CarsRentalContext())
            {
                Role role = context.Roles.SingleOrDefault(r => r.RoleName.ToLower() == roleName.ToLower());
                if (role == null)
                {
                    throw new ProviderException("The supplied role name does not exist");
                }
                if (role.Users.Count > 0)
                {
                    if (throwOnPopulatedRole)
                    {
                        throw new ProviderException("The role already contains users, and the supplied parameter 'throwOnPopulatedRole' set to be true");
                    }
                    foreach (User user in role.Users)
                    {
                        user.Roles.Remove(role);
                    }
                }
                context.Roles.Remove(role);
                context.SaveChanges();
                return true;
            }
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            if (roleName == null) throw new ArgumentNullException("roleName");
            if (roleName == string.Empty) throw new ArgumentException("RoleName cannot be an empty string", "roleName");

            using (CarsRentalContext context = new CarsRentalContext())
            {
                Role role = context.Roles.SingleOrDefault(r => r.RoleName.ToLower() == roleName.ToLower());
                if (role == null) throw new ProviderException("The role name does not exist");

                return (from u in role.Users
                        where u.Username.ToLower().Contains(usernameToMatch.ToLower())
                        orderby u.Username
                        select u.Username)
                        .ToArray();
            }
        }

        public override string[] GetAllRoles()
        {
            using (CarsRentalContext context = new CarsRentalContext())
            {
                return context.Roles.Select(r => r.RoleName).ToArray();
            }
        }

        public override string[] GetRolesForUser(string username)
        {
            if (username == null) throw new ArgumentNullException("username");
            if (username == string.Empty) throw new ArgumentException("username cannot be an empty string", "username");

            using (CarsRentalContext context = new CarsRentalContext())
            {
                User user = context.Users.SingleOrDefault(u => u.Username.ToLower() == username.ToLower());
                if (user == null) throw new ProviderException("The username does not exist");

                return user.Roles.Select(r => r.RoleName).ToArray();
            }
        }

        public override string[] GetUsersInRole(string roleName)
        {
            if (roleName == null) throw new ArgumentNullException("roleName");
            if (roleName == string.Empty) throw new ArgumentException("RoleName cannot be an empty string", "roleName");

            using (CarsRentalContext context = new CarsRentalContext())
            {
                Role role = context.Roles.SingleOrDefault(r => r.RoleName.ToLower() == roleName.ToLower());
                if (role == null) throw new ProviderException("The role name does not exist");

                return role.Users.Select(u => u.Username).ToArray();
            }
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            if (username == null) throw new ArgumentNullException("username");
            if (username == string.Empty) throw new ArgumentException("username cannot be an empty string", "username");
            if (roleName == null) throw new ArgumentNullException("roleName");
            if (roleName == string.Empty) throw new ArgumentException("RoleName cannot be an empty string", "roleName");

            using (CarsRentalContext context = new CarsRentalContext())
            {
                User user = context.Users.SingleOrDefault(u => u.Username.ToLower() == username.ToLower());
                if (user == null) throw new ProviderException("The username does not exist");
                Role role = context.Roles.SingleOrDefault(r => r.RoleName.ToLower() == roleName.ToLower());
                if (role == null) throw new ProviderException("The role name does not exist");

                return user.Roles.Contains(role);
            }
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            if (usernames == null || usernames.Any(u => u == null)) throw new ArgumentNullException("usernames");
            if (roleNames == null || roleNames.Any(r => r == null)) throw new ArgumentNullException("roleNames");
            if (usernames.Length == 0 || usernames.Any(u => u == string.Empty))
                throw new ArgumentException("usernames cannot be an empty array, and any username must not be an empty string", "usernames");
            if (roleNames.Length == 0 || roleNames.Any(r => r == string.Empty))
                throw new ArgumentException("roleNames cannot be an empty array, and any role name must not be an empty string", "roleNames");

            using (CarsRentalContext context = new CarsRentalContext())
            {
                if (!usernames.All(u => context.Users.Any(uInContext => uInContext.Username.ToLower() == u.ToLower())))
                    throw new ProviderException("One or more usernames do not exist");
                if (!roleNames.All(r => context.Roles.Any(rInContext => rInContext.RoleName.ToLower() == r.ToLower())))
                    throw new ProviderException("One or more role names do not exist");

                IEnumerable<Role> rolesInContext = context.Roles.Where(r => roleNames.Any(rn => rn.ToLower() == r.RoleName.ToLower()));
                IEnumerable<User> usersInContext = context.Users.Where(u => usernames.Any(un => un.ToLower() == u.Username.ToLower()));
                foreach (User user in usersInContext)
                {
                    foreach (Role role in rolesInContext)
                    {
                        user.Roles.Remove(role);
                    }
                }
                context.SaveChanges();
            }
        }

        public override bool RoleExists(string roleName)
        {
            if (roleName == null)
            {
                throw new ArgumentNullException("roleName");
            }
            if (roleName == string.Empty)
            {
                throw new ArgumentException("The role name cannot be an empty string");
            }
            using (CarsRentalContext context = new CarsRentalContext())
            {
                return context.Roles.Any(r => r.RoleName.ToLower() == roleName.ToLower());
            }
        }
    }
}
