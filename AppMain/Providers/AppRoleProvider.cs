// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TMA_AppRoleProvider.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the AppRoleProvider type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

using DBHelper;
using DBHelper.Schema;
using AppContext = DBHelper.Schema.DBLAccountOpeningContext;

namespace AppMain.Providers
{
    public class AppRoleProvider: RoleProvider
    {
       

        public override string[] GetRolesForUser(string username)
        {
            using (AppContext db = new DBLAccountOpeningContext())
            {
                AppUser user = db.AppUsers.FirstOrDefault(u => u.Email.Equals(username, StringComparison.CurrentCultureIgnoreCase) || u.Email.Equals(username, StringComparison.CurrentCultureIgnoreCase));

         var roles= from ur in db.AppUsers
                    from r in db.Roles
                    where ur.RoleId == r.RoleId
                    select r.RoleName;
                return roles.ToArray(); 
            }
        }



        public override bool IsUserInRole(string username, string roleName)
        {
            using (AppContext db = new AppContext())
            {
                AppUser user = db.AppUsers.FirstOrDefault(u => u.Email.Equals(username, StringComparison.CurrentCultureIgnoreCase) || u.Email.Equals(username, StringComparison.CurrentCultureIgnoreCase));

                var roles = from ur in db.AppUsers
                    from r in db.Roles
                    where ur.RoleId == r.RoleId
                    select r.RoleName;
                if (user != null)
                    return roles.Any(r => r.Equals(roleName, StringComparison.CurrentCultureIgnoreCase));
                else
                    return false;
            }
        }
        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName { get; set; }
    }


}