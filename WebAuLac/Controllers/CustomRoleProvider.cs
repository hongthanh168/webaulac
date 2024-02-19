using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Linq;
using System.Web;
using System.Web.Security;
using WebAuLac.Models;

namespace WebAuLac.Controllers // Chú ý tên namespace này
{
    public class CustomRoleProvider : RoleProvider
    {
        AuLacEntities db = new AuLacEntities(); //khai bao context

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

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string name)
        {
            //// tạo biến getrole, so sánh xem UserID đang đăng nhập có giống với tên trong db ko
            //HRM_USER account = db.HRM_USER.Single(x => x.UserName.Equals(name));
            //if (account != null) // Nếu giống
            //{
            //    IQueryable<HRM_ROLE> list = db.HRM_ROLE.Where(x => x.HRM_USERROLES.Select(ur => ur.UserID).Contains(account.UserID));
            //    return list.Select(r => r.RoleName).ToArray();
            //    //string temp = string.Join("|", arr);
            //    //return temp.Split('|');
            //}
            //else
                return new String[] { };
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}