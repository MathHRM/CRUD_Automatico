using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace CRUD_Automatico
{
    internal class Cadastro
    {
        private MySqlConnection _conxSql = new MySqlConnection(BDInfo.Server);

        private Dictionary<string, MySqlColumn> _colunas = new Dictionary<string, MySqlColumn>();

        public Dictionary<string, MySqlColumn> Colunas 
        { 
            get 
            {
                if( _colunas.Count < 1 )
                {
                    SetColumns();
                }
                return _colunas; 
            } 
        }

        private List<string> _nomeColunas;
        public List<string> NomeColunas { get { return _nomeColunas; } }

        private string _id;

        public string ID { get { return _id; } }

        public Cadastro()
        {
            _nomeColunas = getColumnsName();
            _id = getIdColumn();
        }

        public bool Adicionar(Dictionary<string, string> values)
        {
            try
            {
                _conxSql.Open();

                string nomeColunas = "";
                string valoresColunas = "";
                for (int i = 0; i < _nomeColunas.Count; i++)
                {
                    if (_nomeColunas[i].Equals( _id )) continue;

                    nomeColunas += _nomeColunas[i];
                    valoresColunas += "@" + _nomeColunas[i];

                    if (i < _nomeColunas.Count - 1)
                    {
                        nomeColunas += ", ";
                        valoresColunas += ", ";
                    }
                }

                var comando = new MySqlCommand(
                    $"INSERT INTO {BDInfo.Table} ({nomeColunas}) values ({valoresColunas});", _conxSql);

                definirParametros(comando, values);

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

        public bool Update(Dictionary<string, string> values, int id)
        {
            try
            {
                _conxSql.Open();

                string set = "";
                for (int i = 0; i < _nomeColunas.Count; i++)
                {
                    if (_nomeColunas[i].Equals(_id))
                        continue;
                    
                    set += $"{_nomeColunas[i]} = @{_nomeColunas[i]}";

                    if (i < _nomeColunas.Count - 1) set += ", ";
                }

                Console.WriteLine("set: " + set);

                var comando = new MySqlCommand(
                    $"UPDATE {BDInfo.Table} SET {set} WHERE { _id } = {id};", _conxSql);

                definirParametros(comando, values);

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

        private void definirParametros(MySqlCommand comando, Dictionary<string, string> values)
        {
            for (int i = 0; i < _nomeColunas.Count; i++)
            {
                string coluna = _nomeColunas[i];

                if (coluna.Equals( _id )) continue;

                var colunaParam = $"@{coluna}";

                /*comando.Parameters.Add(colunaParam, colunaAtual.DataType);
                comando.Parameters[colunaParam].Value = values[coluna];*/

                comando.Parameters.AddWithValue(colunaParam, values[coluna]);
            }
        }

        public void Remover(int ID)
        {
            try
            {
                _conxSql.Open();
                var comando = new MySqlCommand(
                    $"DELETE FROM {BDInfo.Table} WHERE { _id } = {ID}", _conxSql);
                comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao remover do BD\n" + ex.Message);
            }
            finally
            {
                _conxSql.Close();
            }
        }

        public MySqlDataReader Pesquisar(int ID)
        {
            try
            {
                _conxSql.Open();
                var comando = new MySqlCommand(
                    $"SELECT * FROM {BDInfo.Table} WHERE { _id } = {ID};", _conxSql);

                Console.WriteLine($"SELECT * FROM {BDInfo.Table} WHERE { _id } = {ID};");

                var reader = comando.ExecuteReader();

                return reader;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao pesquisar no BD\n" + ex.Message);
                _conxSql.Close();
                return null;
            }
        }

        public MySqlDataReader PesquisarTodos()
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

        private void SetColumns()
        {
            try
            {
                if(_conxSql.State != ConnectionState.Open)
                    _conxSql.Open();

                var schemaCommand = new MySqlCommand(
                    $"select COLUMN_NAME, IS_NULLABLE, COLUMN_KEY, DATA_TYPE, EXTRA from information_schema.columns WHERE TABLE_NAME = '{BDInfo.Table}' AND TABLE_SCHEMA = '{BDInfo.DataBase}';", _conxSql);

                var reader = schemaCommand.ExecuteReader();

                while (reader.Read())
                {
                    string nome = reader["COLUMN_NAME"].ToString();
                    bool nullable = reader["IS_NULLABLE"].ToString().Equals("YES");
                    string dataType = reader["DATA_TYPE"].ToString();
                    bool key = reader["COLUMN_KEY"].ToString().Equals("PRI");
                    string extra = reader["EXTRA"].ToString();

                    _colunas.Add(nome, new MySqlColumn(nome, nullable, dataType, key, extra));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao pegar colunas do BD\n" + ex.Message);
            }
            finally
            {
                _conxSql.Close();
            }
        }

        private string getIdColumn()
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

        public List<string> getColumnsName()
        {
            try
            {
                if (_conxSql.State != ConnectionState.Open)
                    _conxSql.Open();

                List<string> list = new List<string>();

                var schemaCommand = new MySqlCommand(
                    $"select COLUMN_NAME from information_schema.columns WHERE TABLE_NAME = '{BDInfo.Table}' AND TABLE_SCHEMA = '{BDInfo.DataBase}';", _conxSql);

                var reader = schemaCommand.ExecuteReader();

                while(reader.Read())
                {
                    string nome = reader["COLUMN_NAME"].ToString();
                    list.Add(nome);
                }

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

    }
}
