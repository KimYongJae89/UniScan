using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using DynMvp.Base;

namespace DynMvp.Authentication
{
    class UserTextSource : UserSource
    {
        string fileName = "";

        public UserTextSource(string fileName)
        {
            this.fileName = fileName;
        }

        public override void LoadUserList(UserList userList)
        {
            if (fileName == "")
            {
                fileName = String.Format("{0}\\UserList.dat", Configuration.ConfigFolder);
            }

            if (File.Exists(fileName) == false)
                return;

            string[] lines = File.ReadAllLines(fileName, Encoding.Default);
            
            for(int i = 1; i < lines.Length; i++)
            {
                string line = lines[i];

                string[] words = line.Split(new char[] { ',' });
                if (words.Count() == 3)
                {
                    User user = new User();
                    user.Id = words[0].Trim();
                    user.PasswordHash = words[1].Trim();
                    user.UserType = (UserType)Enum.Parse(typeof(UserType), words[2].Trim());
                    //user.permissionControl.SetPermissions(permissions);

                    userList.AddUser(user);
                }
            }
        }

        public override void SaveUserList(UserList userList)
        {
            StringBuilder resultStringBuilder = new StringBuilder();

            resultStringBuilder.Append(StringManager.GetString("id, password, permission"));
            resultStringBuilder.AppendLine();

            foreach (User user in userList)
            {
                resultStringBuilder.Append(String.Format("{0}, {1}, {2}", user.Id, user.PasswordHash, user.UserType));
                resultStringBuilder.AppendLine();
            }

            string dir = Path.GetDirectoryName(fileName);
            string file = Path.GetFileName(fileName);
            if (Directory.Exists(dir))
                File.WriteAllText(fileName, resultStringBuilder.ToString(), Encoding.Default);
            else
                File.WriteAllText(Path.Combine(Environment.CurrentDirectory,"UserList.dat"), resultStringBuilder.ToString(), Encoding.Default);
        }

        public override void LoadRoleList(RoleList roleList)
        {

        }
    }
}
