using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostgreSQL13
{
    class Options
    {
        public string Server { get; set; } = "localhost";
        public string Username { get; set; } = "postgres";
        public string Password { get; set; } = "masterkey";
        public string Database { get; set; } = "cosmo";
    }
}
