using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace CRUD_Automatico
{
    internal class AutoCrud
    {
        private MySqlConnection _conxSql;

        private List<string> _nomeColunas;
        public List<string> NomeColunas { get { return _nomeColunas; } }

        private string _idColumn;
        public string ID { get { return _idColumn; } }

        public AutoCrud(MySqlConnection conxSql)
        {
            _conxSql = conxSql;

            _nomeColunas = GetColumnNames();
            _idColumn = GetTheIdColumn();
        }

       /*
        * 
        *  Funções CRUD
        *  
        */

        // Adicionar
        public bool Add(Dictionary<string, string> values)
        {
            try
            {
                _conxSql.Open();

                string nomeColunas = "";
                string paramColunas = "";
                for (int i = 0; i < _nomeColunas.Count; i++)
                {
                    if (_nomeColunas[i].Equals(_idColumn)) continue;

                    nomeColunas += _nomeColunas[i];
                    paramColunas += "@" + _nomeColunas[i];

                    if (i < _nomeColunas.Count - 1)
                    {
                        nomeColunas += ", ";
                        paramColunas += ", ";
                    }
                }

                var comando = new MySqlCommand(
                    $"INSERT INTO {BDInfo.Table} ({nomeColunas}) values ({paramColunas});", _conxSql);

                DefineParameters(comando, values);

                comando.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao adicionar ao BD\n\n" + ex.Message);
                return false;
            }
            finally
            {
                _conxSql.Close();
            }
        }

        // Editar
        public bool Update(Dictionary<string, string> values, int id)
        {
            try
            {
                _conxSql.Open();

                string set = "";
                for (int i = 0; i < _nomeColunas.Count; i++)
                {
                    if (_nomeColunas[i].Equals(_idColumn))
                        continue;

                    set += $"{_nomeColunas[i]} = @{_nomeColunas[i]}";

                    if (i < _nomeColunas.Count - 1) set += ", ";
                }

                var comando = new MySqlCommand(
                    $"UPDATE {BDInfo.Table} SET {set} WHERE {_idColumn} = {id};", _conxSql);

                DefineParameters(comando, values);

                comando.ExecuteNonQuery();
                _conxSql.Close();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao editar no BD\n\n" + ex.Message);
                return false;
            }
            finally
            {
                _conxSql.Close();
            }
        }

        // Excluir
        public bool Remove(int ID)
        {
            try
            {
                _conxSql.Open();
                var comando = new MySqlCommand(
                    $"DELETE FROM {BDInfo.Table} WHERE {_idColumn} = {ID}", _conxSql);
                comando.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao remover do BD\n" + ex.Message);
                return false;
            }
            finally
            {
                _conxSql.Close();
            }
        }

        // pesquisa um row pelo id
        public MySqlDataReader SearchRow(int ID)
        {
            try
            {
                _conxSql.Open();
                var comando = new MySqlCommand(
                    $"SELECT * FROM {BDInfo.Table} WHERE {_idColumn} = {ID};", _conxSql);

                var reader = comando.ExecuteReader();

                return reader;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao pesquisar no BD\n" + ex.Message);
                return null;
            }
            finally
            {
                _conxSql.Close();
            }
        }

        // pesquisa todos os rows
        public MySqlDataReader GetAllData()
        {
            try
            {
                _conxSql.Open();

                var comando = new MySqlCommand(
                    $"SELECT * FROM {BDInfo.Table};", _conxSql);

                var reader = comando.ExecuteReader();

                return reader;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao pesquisar todos no BD\n" + ex.Message);
                _conxSql.Close();
                return null;
            }
        }

        public MySqlDataReader GetInterval(int start, int end)
        {
            try
            {
                _conxSql.Open();

                var comando = new MySqlCommand(
                    $"SELECT * FROM {BDInfo.Table} ORDER BY {_idColumn} LIMIT {end} OFFSET {start};", _conxSql);

                var reader = comando.ExecuteReader();

                return reader;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao pesquisar todos no BD\n" + ex.Message);
                _conxSql.Close();
                return null;
            }
        }

        // Define os parametros e valores do comando sql
        private void DefineParameters(MySqlCommand comando, Dictionary<string, string> values)
        {
            for (int i = 0; i < _nomeColunas.Count; i++)
            {
                string coluna = _nomeColunas[i];

                if (coluna.Equals(_idColumn)) continue;

                if (GetColumnType(coluna).Contains("date"))
                    values[coluna] = ChangeDateFormat(values[coluna]);

                var paramColuna = $"@{coluna}";

                comando.Parameters.AddWithValue(paramColuna, values[coluna]);
            }
        }



        /*
         * 
         *  Funções para informação sobre colunas
         *  
         */

        // retorna um dicionario com o nome e dados de todas as colunas
        public Dictionary<string, MySqlColumn> GetColumnsInformations()
        {
            try
            {
                if (_conxSql.State != ConnectionState.Open)
                    _conxSql.Open();

                Dictionary<string, MySqlColumn> colunas = new Dictionary<string, MySqlColumn>();

                var schemaCommand = new MySqlCommand(
                    $"select " +
                    $"COLUMN_NAME, " +
                    $"IS_NULLABLE, " +
                    $"COLUMN_KEY, " +
                    $"DATA_TYPE, " +
                    $"EXTRA " +
                    $"from information_schema.columns " +
                    $"WHERE TABLE_NAME = @TableName " +
                    $"AND TABLE_SCHEMA = @TableSchema;", _conxSql);

                schemaCommand.Parameters.AddWithValue("@TableName", BDInfo.Table);
                schemaCommand.Parameters.AddWithValue("@TableSchema", BDInfo.DataBase);

                var reader = schemaCommand.ExecuteReader();

                while (reader.Read())
                {
                    string nome = reader["COLUMN_NAME"].ToString();
                    bool nullable = reader["IS_NULLABLE"].ToString().Equals("YES");
                    string dataType = reader["DATA_TYPE"].ToString();
                    bool key = reader["COLUMN_KEY"].ToString().Equals("PRI");
                    string extra = reader["EXTRA"].ToString();

                    colunas.Add(nome, new MySqlColumn(nome, nullable, dataType, key, extra));
                }
                reader.Close();

                return colunas;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao pegar colunas do BD\n" + ex.Message);
                return null;
            }
            finally
            {
                _conxSql.Close();
            }
        }

        // retorna uma lista com o nome de todas as colunas
        public List<string> GetColumnNames()
        {
            try
            {
                if (_conxSql.State != ConnectionState.Open)
                    _conxSql.Open();

                List<string> list = new List<string>();

                var schemaCommand = new MySqlCommand(
                    $"select COLUMN_NAME from information_schema.columns WHERE TABLE_NAME = '{BDInfo.Table}' AND TABLE_SCHEMA = '{BDInfo.DataBase}';", _conxSql);

                var reader = schemaCommand.ExecuteReader();

                while (reader.Read())
                {
                    string nome = reader["COLUMN_NAME"].ToString();
                    list.Add(nome);
                }
                reader.Close();

                return list;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao pegar ID do BD\n" + ex.Message);
                return null;
            }
            finally
            {
                _conxSql.Close();
            }
        }

        // retorna os dados da coluna pelo nome
        public MySqlColumn GetColumnInformation(string col)
        {
            try
            {
                if (_conxSql.State != ConnectionState.Open)
                    _conxSql.Open();

                var schemaCommand = new MySqlCommand(
                    $"select COLUMN_NAME, IS_NULLABLE, COLUMN_KEY, DATA_TYPE, EXTRA from information_schema.columns WHERE TABLE_NAME = '{BDInfo.Table}' AND TABLE_SCHEMA = '{BDInfo.DataBase}' AND COLUMN_NAME = {col};", _conxSql);

                var reader = schemaCommand.ExecuteReader();

                reader.Read();

                string nome = reader["COLUMN_NAME"].ToString();
                bool nullable = reader["IS_NULLABLE"].ToString().Equals("YES");
                string dataType = reader["DATA_TYPE"].ToString();
                bool key = reader["COLUMN_KEY"].ToString().Equals("PRI");
                string extra = reader["EXTRA"].ToString();

                reader.Close();

                return new MySqlColumn(nome, nullable, dataType, key, extra);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao pegar colunas do BD\n" + ex.Message);
                _conxSql.Close();
                return null;
            }
        }

        // retorna em string o data type da coluna
        private string GetColumnType(string col)
        {
            try
            {
                if (_conxSql.State != ConnectionState.Open)
                    _conxSql.Open();

                var schemaCommand = new MySqlCommand(
                    $"select DATA_TYPE from information_schema.columns WHERE TABLE_NAME = '{BDInfo.Table}' AND TABLE_SCHEMA = '{BDInfo.DataBase}' AND COLUMN_NAME = '{col}';", _conxSql);

                var reader = schemaCommand.ExecuteReader();

                reader.Read();
                string dt = reader["DATA_TYPE"].ToString();
                reader.Close();

                return dt;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao pegar data type do BD\n" + ex.Message);
                return null;
            }
        }

        // retorna o nome da coluna id (chave primaria)
        private string GetTheIdColumn()
        {
            try
            {
                if (_conxSql.State != ConnectionState.Open)
                    _conxSql.Open();

                var schemaCommand = new MySqlCommand(
                    $"select COLUMN_NAME from information_schema.columns WHERE TABLE_NAME = '{BDInfo.Table}' AND TABLE_SCHEMA = '{BDInfo.DataBase}' AND COLUMN_KEY = 'PRI';", _conxSql);

                var reader = schemaCommand.ExecuteReader();

                reader.Read();
                string nome = reader["COLUMN_NAME"].ToString();
                reader.Close();

                return nome;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao pegar ID do BD\n" + ex.Message);
                return null;
            }
            finally
            {
                _conxSql.Close();
            }
        }



        //
        // Outros
        //

        // muda o formato da data brasileira (dd/MM/aaaa) para americana (aaaa/MM/dd)
        public string ChangeDateFormat(string date)
        {
            if (date.Length < 11)
                return DateTime.Parse(date).ToString("yyyy/MM/dd");
            else
                return DateTime.Parse(date).ToString("yyyy/MM/dd HH:mm:ss");
        }

        public bool TestaConexao()
        {
            try
            {
                _conxSql.Open();
                Console.WriteLine("Conexão ao DB concluida");
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao conectar ao BD\n" + ex.Message);
                return false;
            }
            finally
            {
                _conxSql.Close();
            }
        }
    }
}
