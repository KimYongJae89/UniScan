using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Data.Database.PostgreSQL13
{
    public class TableInfo
    {

        public TableInfo(Dictionary<string, object> dbData)
        {
            SchemaName = Database.Parse<string>(dbData["schemaname"]);
            TableName = Database.Parse<string>(dbData["tablename"]);
            TableOwner = Database.Parse<string>(dbData["tableowner"]);
            TableSpace = Database.Parse<string>(dbData["tablespace"]);
            HasIndexes = Database.Parse<bool>(dbData["hasindexes"]);
            HasRules = Database.Parse<bool>(dbData["hasrules"]);
            HasTriggers = Database.Parse<bool>(dbData["hastriggers"]);
            RowSecurity = Database.Parse<bool>(dbData["rowsecurity"]);
        }

        public string SchemaName { get; set; }
        public string TableName { get; set; }
        public string TableOwner { get; set; }
        public string TableSpace { get; set; }
        public bool HasIndexes { get; set; }
        public bool HasRules { get; set; }
        public bool HasTriggers { get; set; }
        public bool RowSecurity { get; set; }
    }
}
