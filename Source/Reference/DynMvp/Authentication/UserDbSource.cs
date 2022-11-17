using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;

using DynMvp.Base;

//using FirebirdSql.Data.FirebirdClient;

namespace DynMvp.Authentication
{
    public class UserDbSource : UserSource
    {
        string dbPath = "";

        public UserDbSource(string dbPath)
        {
            this.dbPath = dbPath;
        }

        //private bool Open(out DbConnection connection)
        //{
        //    //FbConnectionStringBuilder sb;
        //    //sb = new FbConnectionStringBuilder();
        //    //sb.UserID = "dba";
        //    //sb.Password = "planbss";
        //    //sb.Database = dbPath;
        //    //sb.DataSource = "localhost";
        //    //sb.Port = 3050;
        //    //sb.ConnectionLifeTime = 15;
        //    //sb.Pooling = true;
        //    //sb.MinPoolSize = 0;
        //    //sb.MaxPoolSize = 50;
        //    //sb.PacketSize = 8192;
        //    //sb.ServerType = FbServerType.Default;
        //    //sb.Charset = "NONE";

        //    connection = new FbConnection(sb.ConnectionString);

        //    //try
        //    //{
        //    //    connection.Open();
        //    //}
        //    //catch (FbException ex)
        //    //{
        //    //    LogHelper.Error(LoggerType.Error, String.Format(StringManager.GetString(this.GetType().FullName, "Database is not installed properly") + " - {0}", ex.Message));
        //    //    return false;
        //    //}

        //    return true;
        //}

        public override void LoadUserList(UserList userList)
        {
            DbConnection connection = null;
            //if (Open(out connection) == false)
            //    return;

            DbCommand command = connection.CreateCommand();
            command.CommandText = "select * from user_list";
            DbDataReader dataReader = command.ExecuteReader();

            while (dataReader.Read())
            {
                User user = new User();
                user.Id = dataReader["user_id"].ToString();
                user.PasswordHash = dataReader["password"].ToString();

                string permissions = dataReader["permissions"].ToString();
                user.permissionControl.SetPermissions(permissions);

                userList.AddUser(user);
            }

            connection.Close();
        }

        public override void SaveUserList(UserList userList)
        {
            DbConnection connection = null;
            //if (Open(out connection) == false)
            //    return;

            DbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);

            DbCommand command = connection.CreateCommand();
            command.Transaction = transaction;

            // 기존 자료를 삭제한다.
            command.CommandText = "delete from user_list";
            command.ExecuteNonQuery();

            foreach (User user in userList)
            {
                command.CommandText = String.Format("insert into user_list (user_id, password, permissions) values ('{0}', '{1}', '{2}' )",
                    user.Id, user.PasswordHash, user.permissionControl.GetPermissions());
                command.ExecuteNonQuery();
            }

            transaction.Commit();

            connection.Close();
        }

        public override void LoadRoleList(RoleList roleList)
        {
            DbConnection connection = null;
            //if (Open(out connection) == false)
            //    return;

            DbCommand command = connection.CreateCommand();
            command.CommandText = "select * from role_list";
            DbDataReader dataReader = command.ExecuteReader();

            while (dataReader.Read())
            {
                Role role = new Role();
                role.Name = dataReader["name"].ToString();

                string permissions = dataReader["permissions"].ToString();
                role.permissionControl.SetPermissions(permissions);

                roleList.AddRole(role);
            }

            connection.Close();
        }
    }
}
