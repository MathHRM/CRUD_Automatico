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

        private List<string> _colunas = new List<string>();
        public List<string> Colunas { get { return _colunas; } }

        public Cadastro()
        {
            GetColumnNames();
        }

        public bool Adicionar(Dictionary<string, string> values)
        {
            try
            {
                _conxSql.Open();

                string colunasstr = "";
                string colunasSet = "";
                for (int i = 0; i < _colunas.Count; i++)
                {
                    if (verifyColId(_colunas[i])) continue;
                    colunasstr += _colunas[i];
                    colunasSet += "@" + _colunas[i];

                    if (i < _colunas.Count - 1)
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
                Console.WriteLine("Erro ao adicionar ao BD\n" + ex.Message);
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
                for (int i = 0; i < Colunas.Count; i++)
                {
                    set += $"{Colunas[i]} = @{Colunas[i]}";
                    if (i < Colunas.Count - 1) set += ", ";
                }

                var comando = new MySqlCommand(
                    $"UPDATE {BDInfo.Table} SET {set} WHERE id = {id};", _conxSql);

                SetParametros(comando, values);

                comando.ExecuteNonQuery();
                _conxSql.Close();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao adicionar ao BD\n" + ex.Message);
                return false;
            }
            finally
            {
                _conxSql.Close();
            }
        }


        private void SetParametros(MySqlCommand comando, Dictionary<string, string> values)
        {
            for (int i = 0; i < Colunas.Count; i++)
            {
                if (verifyColId(_colunas[i])) continue;

                var column = $"@{_colunas[i]}";
                comando.Parameters.Add(column, MySqlDbType.VarChar, 50);
                comando.Parameters[column].Value = values[_colunas[i]];
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
                Console.WriteLine("Erro ao remover do BD" + ex.Message);
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
                    $"SELECT * FROM {BDInfo.Table} WHERE id = {ID};", _conxSql);

                var reader = comando.ExecuteReader();

                return reader;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro aoremover do BD" + ex.Message);
                return null;
            }
            finally
            {
                _conxSql.Close();
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
                Console.WriteLine("Erro ao pesquisar do BD" + ex.Message);
                _conxSql.Close();
                return null;
            }
        }



        private void GetColumnNames()
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
                    _colunas.Add(col.Field<String>("ColumnName"));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao pegar colunas do BD" + ex.Message);
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
                MessageBox.Show("Erro ao conectar ao BD" + ex.Message);
                return false;
            }
            finally
            {
                _conxSql.Close();
            }
        }

        private bool verifyColId(string col)
        {
            string id = col.ToLower();
            return col.Equals("id") ||
                   col.Contains("_id") ||
                   col.Contains("id_");
        }
    }
}
