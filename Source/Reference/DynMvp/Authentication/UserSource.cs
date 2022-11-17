using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynMvp.Authentication
{
    public abstract class UserSource
    {
        public abstract void LoadUserList(UserList userList);
        public abstract void SaveUserList(UserList userList);
        public abstract void LoadRoleList(RoleList roleList);
    }
}
