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
        public Dictionary<string, MySqlColumn> Colunas { get { return _colunas; } }

        private List<string> _nomeColunas;
        public List<string> NomeColunas { get { return _nomeColunas; } }

        private string _id;
        public string ID { get { return _id; } }

        public Cadastro()
        {
            GetColumn();
            _nomeColunas = new List<string>(_colunas.Keys);
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
                    if (_nomeColunas[i].Equals(_id)) continue;

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
                for (int i = 0; i < NomeColunas.Count; i++)
                {
                    if (_nomeColunas[i].Equals(_id)) continue;
                    set += $"{NomeColunas[i]} = @{NomeColunas[i]}";
                    if (i < NomeColunas.Count - 1) set += ", ";
                }

                var comando = new MySqlCommand(
                    $"UPDATE {BDInfo.Table} SET {set} WHERE {_id} = {id};", _conxSql);

                definirParametros(comando, values);

                comando.ExecuteNonQuery();
                _conxSql.Close();
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

        private void definirParametros(MySqlCommand comando, Dictionary<string, string> values)
        {
            for (int i = 0; i < Colunas.Count; i++)
            {
                string coluna = _nomeColunas[i];
                MySqlColumn colunaAtual = _colunas[coluna];

                if (colunaAtual.IsKey) continue;

                var colunaParam = $"@{_nomeColunas[i]}";

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
                    $"DELETE FROM {BDInfo.Table} WHERE {_id} = {ID}", _conxSql);
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
                    $"SELECT * FROM {BDInfo.Table} WHERE {_id} = {ID};", _conxSql);

                var reader = comando.ExecuteReader();

                return reader;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao remover do BD\n" + ex.Message);
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
                Console.WriteLine("Erro ao pesquisar do BD\n" + ex.Message);
                _conxSql.Close();
                return null;
            }
        }

        private void GetColumn()
        {
            try
            {
                _conxSql.Open();

                DataTable schema2 = null;

                var schemaCommand2 = new MySqlCommand(
                    $"select COLUMN_NAME, IS_NULLABLE, COLUMN_KEY, DATA_TYPE, EXTRA, CHARACTER_MAXIMUM_LENGTH, NUMERIC_PRECISION from information_schema.columns WHERE TABLE_NAME = '{BDInfo.Table}' AND TABLE_SCHEMA = '{BDInfo.DataBase}';", _conxSql);

                var reader2 = schemaCommand2.ExecuteReader();

                while (reader2.Read())
                {
                    string nome = reader2["COLUMN_NAME"].ToString();
                    bool nullable = reader2["IS_NULLABLE"].ToString().Equals("YES");
                    string dataType = reader2["DATA_TYPE"].ToString();
                    bool key = reader2["COLUMN_KEY"].ToString().Equals("PRI");
                    string extra = reader2["EXTRA"].ToString();

                    if (key)
                        _id = nome;

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
