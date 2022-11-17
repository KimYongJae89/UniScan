using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using DynMvp.Base;

namespace DynMvp.Authentication
{
    public class RoleList
    {
        List<Role> roleList = new List<Role>();

        public void AddRole(Role role)
        {
            roleList.Add(role);
        }

        public void RemoveRole(Role role)
        {
            roleList.Remove(role);
        }
    }

    public class Role
    {
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public PermissionControl permissionControl = new PermissionControl();
    }
}
