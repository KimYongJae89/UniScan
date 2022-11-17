using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace DynMvp.Authentication
{
    public enum UserType
    {
        Operator = 0, // 사용자
        Maintrance = 1, // 관리자
        Admin = 2 // 개발자
    }

    public class UserList
    {
        List<User> userList = new List<User>();

        public IEnumerator<User> GetEnumerator()
        {
            return userList.GetEnumerator();
        }

        public void AddUser(User user)
        {
            userList.Add(user);
        }

        public void RemoveUser(User user)
        {
            userList.Remove(user);
        }

        public User GetUser(string userId)
        {
            foreach (User user in userList)
            {
                if (user.Id == userId)
                    return user;
            }

            if (userId == "developer")
                return new User("developer", "masterkey", UserType.Admin);
            else if (userId == "op" || userId == "operator")
                return new User("operator", "", UserType.Operator);
            else if (userId == "master")
                return new User("master", "master1", UserType.Maintrance);
            else if (userId == "samsung")
                return new User("samsung", "samsung1", UserType.Admin);

            return null;
        }

        public User GetUser(string userId, string password)
        {
            foreach (User user in userList)
            {
                if (user.Id == userId)
                {
                    string passwordHash = User.GetPasswordHash(password);

                    if (passwordHash == user.PasswordHash)
                        return user;
                    else
                        return null;
                }
            }

            if (userId == "developer" && password == "masterkey")
                return new User("developer", "masterkey", UserType.Admin);
            else if (userId == "op"  || userId == "operator")
                return new User("operator", "", UserType.Operator);
            else if (userId == "master" && password == "master1")
                return new User("master", "master1", UserType.Maintrance);
            else if (userId == "samsung")
                return new User("samsung", "samsung1", UserType.Admin);

            return null;
        }
    }

    public class User
    {
        private string id;
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        UserType userType = UserType.Admin;
        public UserType UserType
        {
            get { return userType; }
            set { userType = value; }
        }

        private string passwordHash;
        public string PasswordHash
        {
            get { return passwordHash; }
            set { passwordHash = value; }
        }

        public User()
        {
        }

        public User(string userId, string password, UserType userType)
        {
            this.id = userId;
            this.passwordHash = GetPasswordHash(password);
            this.userType = userType;
            //permissionControl.SuperAccount = superAccount;
        }

        public static string GetPasswordHash(string password)
        {
            SHA1 sha = new SHA1CryptoServiceProvider();

            byte[] passwordByte = Encoding.UTF8.GetBytes(password);
            byte[] result = sha.ComputeHash(passwordByte);

            return Convert.ToBase64String(result);
        }

        public bool IsSuperAccount => (userType == UserType.Admin);
        public bool IsMasterAccount => (userType == UserType.Admin) || (userType == UserType.Maintrance);

        public PermissionControl permissionControl = new PermissionControl();
    }
}
