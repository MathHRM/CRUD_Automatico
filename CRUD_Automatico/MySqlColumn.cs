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
using System.Reflection;

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
            _dataType = getSqlType(_strDataType);
            _key = key;
            _extra = extra;
        }

        public MySqlDbType getSqlType(string type)
        {
            // Um loop pelo enum mysqlType
            // se a representacao em string do enum for igual o tipo
            // fornecido, retorna o enum
            // se nao for encontrado uma representação, retorna varchar

            MySqlDbType value = MySqlDbType.VarChar;
            bool encontrado = false;

            // string passa a representação exatamente igual do enum
            foreach (MySqlDbType dbtype in Enum.GetValues(typeof(MySqlDbType)))
            {
                if (dbtype.ToString().ToLower().Equals(type))
                {
                    encontrado = true;
                    value = dbtype;
                }
            }

            // valor mais aproximado se não encontrado a representação correta
            if(!encontrado)
            foreach (MySqlDbType dbtype in Enum.GetValues(typeof(MySqlDbType)))
            {
                if (dbtype.ToString().ToLower().Contains(type))
                {
                    value = dbtype;
                }
            }

            return value;
        }
    }
}
