namespace CRUD_Automatico
{
    internal static class BDInfo
    {
        // informações sobre o db
        private static string _server = "localhost";
        private static string _uid = "root";
        private static string _pwd = "senhasql";
        private static string _database = "mydb";

        // tabela
        public static string Table { get { return "mytable"; } }
        public static string DataBase { get { return _database; } }

        // string server
        public static string Server
        {
            get
            {
                return $"server={_server};uid={_uid};pwd={_pwd};database={_database};";
            }
        }
    }
}
