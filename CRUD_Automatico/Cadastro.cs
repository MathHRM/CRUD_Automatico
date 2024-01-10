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

        private Dictionary<string, string> _colunasAtrr = new Dictionary<string, string>();

        public Dictionary<string, string> ColunasAtributos { get { return _colunasAtrr; } }

        private List<string> _nomeColunas;
        public List<string> NomeColunas { get { return _nomeColunas; } }
        private string _id;
        public string ID { get { return _id; } }

        public Cadastro()
        {
            GetColumn();
            _nomeColunas = new List<string>(_colunasAtrr.Keys);
        }

        public bool Adicionar(Dictionary<string, string> values)
        {
            try
            {
                _conxSql.Open();

                string colunasstr = "";
                string colunasSet = "";
                for (int i = 0; i < _nomeColunas.Count; i++)
                {
                    if (_nomeColunas[i].Equals(_id)) continue;

                    colunasstr += _nomeColunas[i];
                    colunasSet += "@" + _nomeColunas[i];

                    if (i < _nomeColunas.Count - 1)
                    {
                        colunasstr += ", ";
                        colunasSet += ", ";
                    }
                }
                Console.WriteLine(colunasstr);
                Console.WriteLine(colunasSet);

                var comando = new MySqlCommand(
                    $"INSERT INTO {BDInfo.Table} ({colunasstr}) values ({colunasSet});", _conxSql);

                SetParametros(comando, values);

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

                SetParametros(comando, values);

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


        private void SetParametros(MySqlCommand comando, Dictionary<string, string> values)
        {
            for (int i = 0; i < ColunasAtributos.Count; i++)
            {
                if (_nomeColunas[i].Equals(_id)) continue;

                var column = $"@{_nomeColunas[i]}";
                comando.Parameters.Add(column, MySqlDbType.VarChar, 50);
                comando.Parameters[column].Value = values[_nomeColunas[i]];
            }
        }



        public void Remover(int ID)
        {
            try
            {
                _conxSql.Open();
                var comando = new MySqlCommand(
                    $"DELETE FROM {BDInfo.Table} WHERE id={ID}", _conxSql);
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
            List<string> lista = new List<string>();
            DataTable schema = null;

            try
            {
                _conxSql.Open();
                
                var schemaCommand = new MySqlCommand(
                    $"SELECT * FROM {BDInfo.Table}", _conxSql);

                var reader = schemaCommand.ExecuteReader(CommandBehavior.SchemaOnly);
                schema = reader.GetSchemaTable();

                foreach (DataRow col in schema.Rows)
                {
                    string constraint = "";
                    if (col.Field<Boolean>("IsKey"))
                    {
                        _id = col.Field<String>("ColumnName");
                        constraint = "PRIMARY KEY";
                    }

                    _colunasAtrr.Add(col.Field<String>("ColumnName"), constraint);
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
