using DynMvp.Base;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Data.Database.PostgreSQL13
{
    public class DatabaseSettings
    {
        public string Server { get;set; }
        public string DatabaseName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class Database
    {
        DatabaseSettings settings;

        private NpgsqlConnection connection;
        private NpgsqlTransaction transaction;

        public Database(DatabaseSettings settings)
        {
            this.settings = settings;
        }

        public void Connect()
        {
            var connectionString = $"host={settings.Server};username={settings.UserName};password={settings.Password};database={settings.DatabaseName}";
            this.connection = new NpgsqlConnection(connectionString);
            this.connection.Open();

            //try
            //{
            //    this.connection = new NpgsqlConnection(connectionString);
            //    this.connection.Open();
            //    return true;
            //}
            //catch (Exception ex)
            //{
            //    LogHelper.Error(LoggerType.Error, ex);
            //    this.connection?.Dispose();
            //    this.connection = null;
            //    return false;
            //}
        }

        public void Disconnect()
        {
            //EndTransaction();

            this.connection?.Close();
            this.connection?.Dispose();
            this.connection = null;
        }

        public TableInfo[] GetTables()
        {
            string query = "select * from pg_catalog.pg_tables where schemaname ='public'";

            Connect();
            var command = new NpgsqlCommand(query, connection);
            NpgsqlDataReader reader = command.ExecuteReader();

            List<TableInfo> list = new List<TableInfo>();
            while (reader.Read())
            {
                var dbData = new Dictionary<string, object>();
                for (var i = 0; i < reader.FieldCount; i++)
                    dbData.Add(reader.GetName(i), reader.GetValue(i));

                TableInfo tableInfo = new TableInfo(dbData);
                list.Add(tableInfo);
            }
            Disconnect();
            return list.ToArray();
        }

        private void BeginTransaction()
        {
            Connect();
            this.transaction = this.connection.BeginTransaction();
        }

        private void EndTransaction()
        {
            try
            {
                transaction.Commit();
                transaction.Dispose();
                transaction = null;
            }
            catch (Exception ex)
            {
                LogHelper.Debug(LoggerType.Error, ex.Message);
            }

            Disconnect();
        }

        private void Transaction(Action action)
        {
            Connect();
            using (NpgsqlTransaction transaction = this.connection.BeginTransaction())
            {
                action();

                transaction.Commit();
            }
            Disconnect();
        }

        private bool InsertData(string tableName, DBRecord record)
        {
            return InsertData("public", tableName, record);
        }

        private bool InsertData(string schemaName, string tableName, DBRecord record)
        {
            var columnString = string.Join(", ", record.Columns.Select(f => $"\"{f}\""));
            var dataString = string.Join(", ", record.Values.Select(f => $"\"{f}\""));
            var query = $"insert into \"{schemaName}\".\"{tableName}\" (" + columnString + ") values (" + dataString + ");";

            try
            {
                Action action = new Action(() =>
                {
                    var command = new NpgsqlCommand(query, connection);
                    command?.ExecuteNonQuery();
                });

                Transaction(action);

                //BeginTransaction();

                //var command = new NpgsqlCommand(query, connection);
                //if (command == null)
                //    return false;
                //command.ExecuteNonQuery();

                //EndTransaction();
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Error, ex);
                return false;
            }
        }

        private List<DBRecord> SelectData(string tableName, string whereQuery = "")
        {
            var query = $"select * from \"{tableName}\" {whereQuery}";
            List<DBRecord> dataList = new List<DBRecord>();

            Action action = new Action(() =>
            {
                var command = new NpgsqlCommand(query, connection);
                if (command != null)
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        DBRecord record = new DBRecord();
                        for (var i = 0; i < reader.FieldCount; i++)
                            record.Add(reader.GetName(i), reader.GetValue(i));
                        dataList.Add(record);
                    }
                }
            });

            Transaction(action);

            return dataList;
        }

        public static T Parse<T>(object value) 
        {
            if (value.GetType() == typeof(System.DBNull))
                return default(T);

            return (T)value;
        }
    }
}
