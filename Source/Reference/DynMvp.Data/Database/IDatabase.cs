using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Data.Database
{
    public interface IDatabase
    {
        bool Connect();
        void Disconnect();

        bool BeginTransaction();
        bool EndTransaction();

        bool InsertData(string tableName, params object[] values);
        bool InsertData(string tableName, KeyValuePair<string, object>[] keyValuePairs);

        object[] SelectData(string tableName);
        object[] SelectData(string tableName, params string[] columns);
    }

    public class DBRecord
    {
        Dictionary<string, object> dic = new Dictionary<string, object>();

        public IEnumerable<string> Columns => dic.Keys;
        public IEnumerable<object> Values => dic.Values;

        public void Add(string key, object value)
        {
            dic.Add(key, value);
        }

        public T Get<T>(string key)
        {
            return (T)dic[key];
        }
    }

    public abstract class DatabaseConverter<T>
    {
        public abstract KeyValuePair<string, object>[] ConvertTo(object item);

        public abstract T ConvertFrom<T>(KeyValuePair<string, object>[] pairs);
    }
}
