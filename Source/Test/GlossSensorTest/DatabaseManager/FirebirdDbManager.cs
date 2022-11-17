using DynMvp.Base;
using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlossSensorTest
{
    [Serializable]
    public class DbConnectionInfo
    {
        public static DbConnectionInfo DefaultFbConnectionInfo
            = new DbConnectionInfo(
                "localhost",
                3050,
                Path.Combine(Environment.CurrentDirectory, "..\\Database\\MEASURE_RESULT.fdb"),
                "sysdba",
                "masterkey");

        public static DbConnectionInfo DefaultNpgsqlConnectionInfo
            = new DbConnectionInfo(
                "127.0.0.1",
                5432,
                "postgres",
                "postgres",
                "1");

        public string DataSource { get; set; }
        public int Port { get; set; }
        public string Database { get; set; }
        public string UserID { get; set; }
        public string Password { get; set; }

        public DbConnectionInfo()
        {

        }

        public DbConnectionInfo(string dataSource, int port, string database, string userID, string password)
        {
            DataSource = dataSource;
            Port = port;
            Database = database;
            UserID = userID;
            Password = password;
        }
    }


    public delegate void OnRowUpdatedDelegate(FbRowUpdatedEventArgs args);

    // Firebird Database Manager
    public class FirebirdDbManager : IDisposable
    {
        FbConnectionStringBuilder fbConnectionInfo = new FbConnectionStringBuilder();

        FbConnection connection;
        FbTransaction transaction;
        public bool Connected { get => connection != null; }
        string dbPath;

        public FbConnectionStringBuilder FbConnectionInfo { get => fbConnectionInfo; set => fbConnectionInfo = value; }
        public string DbPath { get => dbPath; }

        public FirebirdDbManager()
        {

        }

        public void Dispose()
        {
            Disconnect();
        }

        public void Disconnect()
        {
            if (connection == null)
                return;

            //CommitTransaction();

            connection.Close();
            connection.Dispose();
            connection = null;
        }

        public bool Connect(string dbPath, string dataSource = "localhost")
        {
            this.dbPath = dbPath;

            fbConnectionInfo.DataSource = dataSource;
            fbConnectionInfo.Database = dbPath;
            fbConnectionInfo.UserID = "SYSDBA";
            fbConnectionInfo.Password = "masterkey";
            fbConnectionInfo.ServerType = FbServerType.Default;
            fbConnectionInfo.Charset = "NONE";

            //fbConnectionInfo.Pooling = false;
            //fbConnectionInfo.Dialect = 3;
            //fbConnectionInfo.ConnectionLifeTime = 0;//15;
            //fbConnectionInfo.PacketSize = 16384;

            //fbConnectionInfo.MinPoolSize = 0;
            //fbConnectionInfo.MaxPoolSize = 500;

            try
            {
                connection = new FbConnection(fbConnectionInfo.ToString());
                connection.Open();
            }
            catch (FbException ex)
            {
                LogHelper.Error(LoggerType.Inspection, "Can't connect to Firebird DB : " + ex.Message);
                connection.Dispose();
                connection = null;
            }
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Inspection, "Can't connect to Firebird DB : " + ex.Message);
                connection?.Dispose();
                connection = null;
            }

            return connection != null;
        }

        public void BeginTransaction()
        {
            //if (transaction != null)
            //    CommitTransaction();

            if (connection == null)
                return;

            // Firebird 빨라지는 방법 No.44
            FbTransactionOptions op = new FbTransactionOptions();
            op.TransactionBehavior = FbTransactionBehavior.NoAutoUndo;

            try
            {
                transaction = connection.BeginTransaction(op);
            }
            catch (Exception ex)
            {
            }
        }

        public void CommitTransaction()
        {
            if (connection == null)
                return;

            if (transaction == null)
                return;

            try
            {
                transaction.Commit();
            }
            catch (Exception ex)
            {
            }
        }

        public void RollBackTransaction()
        {
            if (connection == null)
                return;

            if (transaction == null)
                return;

            try
            {
                transaction.Rollback();
            }
            catch (Exception ex)
            {
            }
        }

        private FbCommand CreateCommand(string query)
        {
            return new FbCommand(query, connection, transaction);
        }



        public bool CreateTable(string tableName)
        {
            FbCommand command = CreateCommand(string.Format("CREATE TABLE {0}", tableName));

            if (command != null)
            {
                // 테이블 생성
                return true;
            }

            return false;
        }

        //public bool AddCollumn(string tableName, object defualtObj, bool isUnique)
        //{
        //    int defualtLength = 50; //byte

        //    if (CreateCommand(out bCommand command))
        //    {
        //        string typeName = null;
        //        if (defualtObj is int)
        //            typeName = "INTEFGER";
        //        else if (defualtObj is string)
        //            typeName = string.Format("VARCHAR({0})", defualtLength));
        //        else if (defualtObj is bool)
        //            typeName = "VARCHAR(5) DEFAULT FALSE";
        //        else if (defualtObj is int)
        //            typeName = "INTEGER";
        //        else if (defualtObj is int)
        //            typeName = "INTEGER";

        //        {
        //            command.CommandText = string.Format("ALTER TABLE {0} ADD ( {1} ) INTEGER NOT NULL", tableName, defualtObj);
        //            return true;
        //        }
        //        else if (obj is bool)
        //        {
        //            command.CommandText = string.Format("CREATE TABLE {0}", name);
        //            return true;
        //        }
        //    }


        //    switch (obj.GetType())
        //    {
        //        case int:

        //            break;
        //    }

        //}

        public bool CommitImage(string query, byte[] image, bool runTransaction = true)
        {
            if (connection == null)
                return false;

            if (runTransaction)
                BeginTransaction();

            //query = "INSERT INTO DUST_IMAGE(IMAGE_DATA) VALUES(@IMAGE_DATA)";

            FbCommand command = CreateCommand(query);
            command.CommandType = CommandType.Text;
            command.Parameters.Add("@IMAGE_DATA", FbDbType.Binary).Value = image;

            try
            {
                command.ExecuteNonQuery();

                if (runTransaction)
                    CommitTransaction();
            }
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Inspection, "Can't Commit to Firebird DB : " + ex.Message);
                return false;
            }
            finally
            {
                command.Dispose();
            }

            return true;
        }



        public bool Commit(string query, string[] parameters, byte[][] blobDatas, bool runTransaction = true)
        {
            if (runTransaction)
                BeginTransaction();

            try
            {
                FbCommand command = CreateCommand(query);

                for (int i = 0; i < parameters.Length; i++)
                {
                    if (blobDatas[i] != null)
                        command.Parameters.Add(parameters[i], FbDbType.Binary).Value = blobDatas[i];
                }
                command.ExecuteNonQuery();

                if (runTransaction)
                    CommitTransaction();
            }
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Inspection, "Can't Commit to Firebird DB : " + ex.Message);
                Debug.WriteLine("{0}", ex.Message);
                return false;
            }
            finally
            {
                //command.Dispose();
            }

            return true;
        }

        public bool Commit(string query, List<string> parameters, List<byte[]> blobDatas, bool runTransaction = true)
        {
            if (runTransaction)
                BeginTransaction();

            try
            {
                FbCommand command = CreateCommand(query);

                for (int i = 0; i < parameters.Count; i++)
                {
                    if (blobDatas[i] != null)
                        command.Parameters.Add(parameters[i], FbDbType.Binary).Value = blobDatas[i];
                }
                command.ExecuteNonQuery();

                if (runTransaction)
                    CommitTransaction();
            }
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Inspection, "Can't Commit to Firebird DB : " + ex.Message);
                Debug.WriteLine("{0}", ex.Message);
                return false;
            }
            finally
            {
                //command.Dispose();
            }

            return true;
        }

        public bool Commit(string query, bool runTransaction = true)
        {
            if (runTransaction)
                BeginTransaction();

            FbCommand command = CreateCommand(query);

            try
            {
                command.ExecuteNonQuery();

                if (runTransaction)
                    CommitTransaction();
            }
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Inspection, "Can't Commit to Firebird DB : " + ex.Message);
                return false;
            }
            finally
            {
                command.Dispose();
            }

            return true;
        }

        public async Task<bool> CommitAsync(string query, string[] parameters, byte[][] blobDatas, bool runTransaction = true)
        {
            if (runTransaction)
                BeginTransaction();

            FbCommand command = CreateCommand(query);

            for (int i = 0; i < parameters.Length; i++)
            {
                if (blobDatas[i] != null)
                    command.Parameters.Add(parameters[i], FbDbType.Binary).Value = blobDatas[i];
            }

            try
            {
                await command.ExecuteNonQueryAsync();

                if (runTransaction)
                    CommitTransaction();
            }
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Inspection, "Can't Commit to Firebird DB : " + ex.Message);
                Debug.WriteLine("{0}", ex.Message);
                return false;
            }
            finally
            {
                command.Dispose();
            }

            return true;
        }

        public async Task<bool> CommitAsync(string query, bool runTransaction = true)
        {
            if (runTransaction)
                BeginTransaction();

            FbCommand command = CreateCommand(query);

            try
            {
                await command.ExecuteNonQueryAsync();

                if (runTransaction)
                    CommitTransaction();
            }
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Inspection, "Can't Commit to Firebird DB : " + ex.Message);
                return false;
            }
            finally
            {
                command.Dispose();
            }

            return true;
        }



        public List<Dictionary<string, object>> Read(string query)
        {
            if (connection == null)
                return null;

            List<Dictionary<string, object>> dataList = new List<Dictionary<string, object>>();

            FbCommand command = CreateCommand(query);
            command.CommandTimeout = 0;

            try
            {
                FbDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var dbData = new Dictionary<string, object>();
                    for (int i = 0; i < reader.FieldCount; i++)
                        dbData.Add(reader.GetName(i), reader.GetValue(i));

                    dataList.Add(dbData);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Inspection, "Can't Read to Firebird DB : " + ex.Message);
            }
            finally
            {
                command.Dispose();
            }

            return dataList;
        }

        public async Task<List<Dictionary<string, object>>> ReadAsync(string query)
        {
            if (connection == null)
                return null;

            List<Dictionary<string, object>> dataList = new List<Dictionary<string, object>>();

            FbCommand command = CreateCommand(query);
            command.CommandTimeout = 0;

            try
            {
                DbDataReader reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    var dbData = new Dictionary<string, object>();
                    for (int i = 0; i < reader.FieldCount; i++)
                        dbData.Add(reader.GetName(i), reader.GetValue(i));

                    dataList.Add(dbData);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Inspection, "Can't Read to Firebird DB : " + ex.Message);
            }
            finally
            {
                command.Dispose();
            }

            return dataList;
        }



        public List<Dictionary<string, object>> Select(string table, string[] cols = null, string where = "")
        {
            if (connection == null)
                return null;

            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT ");

            if (cols == null || cols.Count() == 0)
            {
                sb.Append("*");
            }
            else
            {
                int count = cols.Count();
                for (int i = 0; i < count; i++)
                {
                    sb.AppendFormat("\"{0}\"", cols[i]);

                    if (i != count - 1)
                        sb.Append(",");
                }
            }

            sb.Append(" FROM ");

            sb.AppendFormat("{0}", table);
            if (where != "")
                sb.AppendFormat(" WHERE {0}", where);

            return Read(sb.ToString());
        }

        public bool Insert(string table, string[] cols, object[] data, bool runTransaction = true)
        {
            if (connection == null)
                return false;

            if (cols.Count() != data.Count())
                return false;

            int count = cols.Count();

            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("INSERT INTO {0} (", table);
            for (int i = 0; i < count; i++)
            {
                sb.AppendFormat("{0}", cols[i]);
                if (i == count - 1)
                    sb.Append(") VALUES (");
                else
                    sb.Append(",");
            }

            for (int i = 0; i < count; i++)
            {
                if (data[i].GetType() == typeof(string))
                    sb.AppendFormat("\'{0}\'", data[i]);
                else
                    sb.AppendFormat("{0}", data[i]);

                if (i == count - 1)
                    sb.Append(")");
                else
                    sb.Append(",");
            }

            return Commit(sb.ToString(), runTransaction);
        }

        public bool Delete(string table, string where = "", bool runTransaction = true)
        {
            /* DELETE FROM MODEL_DATA WHERE NAME='1' */

            if (connection == null)
                return true;

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("DELETE FROM {0}", table);
            if (where != "")
                sb.AppendFormat(" WHERE {0}", where);

            return Commit(sb.ToString(), runTransaction);
        }
    }
}