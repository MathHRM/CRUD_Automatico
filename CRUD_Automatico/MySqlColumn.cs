using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace CRUD_Automatico
{
    public class MySqlColumn
    {
        private string _nome;
        private bool _nullable;
        private string _strDataType;
        private MySqlDbType _dataType;
        private bool _key;
        private string _extra;

        public string Nome { get { return _nome; } }
        public bool IsNullable { get { return _nullable; } }
        public string StrDataType { get { return _strDataType; } }
        public MySqlDbType DataType { get { return _dataType; } }
        public bool IsKey { get { return _key; } }
        public string Extra { get { return _extra; } }

        public List<string> Constraints { get; set; }

        public MySqlColumn(string nome, bool nullable, string dataType, bool key, string extra)
        {
            _nome = nome;
            _nullable = nullable;
            _strDataType = dataType;
            _dataType = setDataType(dataType);
            _key = key;
            _extra = extra;
        }

        private MySqlDbType setDataType(string dataType)
        {
            switch(dataType)
            {
                case "varchar":
                    return MySqlDbType.VarChar;
                    break;

                case "int":
                    return MySqlDbType.Int64;
                    break;

                default:
                    return MySqlDbType.VarChar;
            }
        }
    }
}
