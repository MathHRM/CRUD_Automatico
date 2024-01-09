using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD_Automatico
{
    internal static class BDInfo
    {
        private static string _server = "localhost";
        private static string _uid = "root";
        private static string _pwd = "senhasql";
        private static string _database = "mydb";
        public static string Table { get { return "mytable"; } }

        public static string Server
        {
            get
            {
                return $"server={_server};uid={_uid};pwd={_pwd};database={_database};";
            }
        }
    }
}
