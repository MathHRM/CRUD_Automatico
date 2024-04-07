using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace CRUD_Automatico
{
    public partial class Form1 : Form
    {
        const int PAGINATION_SIZE = 10;
        int paginationStart = 0;

        Dictionary<string, InputControl> Name_InputsPair = new Dictionary<string, InputControl>();

        public Form1()
        {
            InitializeComponent();
            var testaConexao = new AutoCrud(new MySqlConnection(BDInfo.Server));
            testaConexao.TestaConexao();
            defineInputs();
            updateTable(paginationStart, PAGINATION_SIZE);

            inptCancelar.Visible = false;
            inptConfirmarEdicao.Visible = false;
            inptConfirmarPesquisa.Visible = false;
            inptConfirmarRemover.Visible = false;
        }


        /*
         * 
         * Botões
         * 
         */
        
        // Adicionar
        private void inptAdicionar_Click(object sender, EventArgs e)
        {
            var values = new Dictionary<string, string>();
            AutoCrud adicionar;

            if (!IsInputsFilleds())
            {
                MessageBox.Show("Preencha todos os campos");
                return;
            }

            foreach (InputControl i in inptPainel.Controls)
            {
                if (!i.isId)
                    values.Add(i.Column, i.Text);
            }

            adicionar = new AutoCrud(new MySqlConnection(BDInfo.Server));
            bool adicionado = adicionar.Add(values);
            if (!adicionado)
            {
                MessageBox.Show("Erro ao adicionar, verifique os dados e tente novamente");
                return;
            }

            updateTable(paginationStart, PAGINATION_SIZE);
            ClearInputs();
        }

        // Editar
        private void inptEditar_Click(object sender, EventArgs e)
        {
            if (IsIdInputEmpty())
            {
                SearchMode();
                return;
            }

            // caso clicado após uma pesquisa irá editar o resultado
            DesactivateButtons();
            ActivateInputs();

            inptCancelar.Visible = true;
            inptConfirmarRemover.Visible = false;
            inptConfirmarEdicao.Visible = true;
        }
        private void confirmarEdicao_Click(object sender, EventArgs e)
        {
            var values = new Dictionary<string, string>();
            string stringId = "";

            if (!IsInputsFilleds())
            {
                MessageBox.Show("Preencha todos os campos");
                return;
            }

            foreach (InputControl i in inptPainel.Controls)
            {
                if (!i.isId)
                    values.Add(i.Column, i.Text);
                else
                    stringId = i.Text;
            }


            int id = int.Parse(stringId);

            AutoCrud update = new AutoCrud(new MySqlConnection(BDInfo.Server));
            bool atualizado = update.Update(values, id);

            if (!atualizado)
            {
                MessageBox.Show("Funcionario não atualizado, verifique os dados ou tente novamente");
                return;
            }

            inptConfirmarEdicao.Visible = false;
            inptCancelar.Visible = false;

            ActivateButtons();
            ClearInputs();
            updateTable(paginationStart, PAGINATION_SIZE);
        }

        // Remover
        private void inptRemover_Click(object sender, EventArgs e)
        {
            if (IsIdInputEmpty())
            {
                SearchMode();
            }
        }
        private void inptConfirmarRemover_Click(object sender, EventArgs e)
        {
            int id = int.Parse(SelectedId());

            AutoCrud excluir = new AutoCrud(new MySqlConnection(BDInfo.Server));
            bool removido = excluir.Remove(id);
            if (!removido)
            {
                MessageBox.Show("Erro ao remover");
                return;
            }

            updateTable(paginationStart ,PAGINATION_SIZE);

            ClearInputs();
            ActivateButtons();
            ActivateInputs();
            inptConfirmarRemover.Visible = false;
            inptCancelar.Visible = false;
        }

        // Pesquisar
        private void inptPesquisar_Click(object sender, EventArgs e)
        {
            SearchMode();
        }
        private void inptConfirmarPesquisa_Click(object sender, EventArgs e)
        {
            Search();
        }

        // Cancelar
        private void inptCancelar_Click(object sender, EventArgs e)
        {
            ActivateInputs();
            ActivateButtons();
            ClearInputs();

            inptConfirmarEdicao.Visible = false;
            inptConfirmarPesquisa.Visible = false;
            inptConfirmarRemover.Visible = false;
            inptCancelar.Visible = false;
        }



        /*
         * 
         *  Tabela
         *  
         */

        // mostra os dados da tabela
        private int updateTable(int start, int limit)
        {
            AutoCrud pesquisar = new AutoCrud(new MySqlConnection(BDInfo.Server));
            DataTable dt = pesquisar.GetInterval(start, limit);

            dataGD.DataSource = dt;
            dataGD.AutoResizeColumns();

            return dt.Rows.Count;
        }

        // muda os dados quando clicado na linha
        private void dataGD_SelectionChanged(object sender, EventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            int contLinhas = dgv.Rows.Count;

            if (contLinhas < 1)
                return;

            if (dataGD.SelectedRows.Count < 1)
                return;

            var idRowSelecionado = dataGD.Rows[dataGD.SelectedRows[0].Index].Cells[0].Value;

            if (idRowSelecionado == null)
                return;

            for (int i = 0; i < dataGD.Columns.Count; i++)
            {
                string columnName = dataGD.Columns[i].Name;
                InputControl input = Name_InputsPair[columnName];
                int idFromSelectedRow = dataGD.SelectedRows[0].Index;
                int indexOfCurrentColumn = i;

                input.Text = dataGD.Rows[idFromSelectedRow].Cells[indexOfCurrentColumn].Value.ToString();
            }

            DesativateInputs();
            DesactivateButtons();
            inptConfirmarPesquisa.Visible = false;
            inptEditar.Visible = true;
            inptCancelar.Visible = true;
            inptConfirmarRemover.Visible = true;
        }



        // adiciona os inputs baseado nas colunas da tabela
        private void defineInputs()
        {
            AutoCrud c = new AutoCrud(new MySqlConnection(BDInfo.Server));
            var colunas = c.GetColumnsInformations();

            if(colunas == null)
            {
                MessageBox.Show("Erro ao exibir inputs");
                return;
            }

            foreach (var nomeCol in colunas.Keys)
            {
                InputControl input = new InputControl(colunas[nomeCol]);

                Name_InputsPair.Add( nomeCol, input );
                inptPainel.Controls.Add(input);
            }
        }



        // muda os inputs para permitir a pesquisa por ID
        private void SearchMode()
        {
            foreach (InputControl i in inptPainel.Controls)
            {
                i.setReadOnly(!i.isId);
            }
            DesactivateButtons();
            inptConfirmarPesquisa.Visible = true;
            inptCancelar.Visible = true;
        }

        // Pesquisa baseado no id fornecido
        private void Search()
        {
            AutoCrud p = new AutoCrud(new MySqlConnection(BDInfo.Server));

            if (IsIdInputEmpty())
            {
                MessageBox.Show("Digite um id");
                return;
            }

            int id;
            if (!int.TryParse(SelectedId(), out id))
            {
                MessageBox.Show("ID inválido");
                ClearInputs();
                return;
            }

            var row = p.SearchRow(id).Rows;

            if (row == null)
            {
                MessageBox.Show("Funcionario não encontrado: erro");
                ClearInputs();
                return;
            }

            if (row.Count == 0)
            {
                MessageBox.Show("Funcionario não encontrado: sem dados");
                ClearInputs();
                return;
            }

            foreach (InputControl i in inptPainel.Controls)
            {
                i.Text = row[0][i.Column].ToString();
            }

            DesativateInputs();
            DesactivateButtons();
            inptConfirmarPesquisa.Visible = false;
            inptEditar.Visible = true;
            inptCancelar.Visible = true;
            inptConfirmarRemover.Visible = true;
        }



        /*
         * 
         *  Funções auxiliares
         *  
         */

        // verifica se os inputs obrigatorios foram preenchidos
        private bool IsInputsFilleds()
        {
            for (int i = 0; i < inptPainel.Controls.Count; i++)
            {
                InputControl inpt = (InputControl)inptPainel.Controls[i];

                if (inpt.isId)
                    continue;

                if (!inpt.IsNullable && inpt.Text.Equals(string.Empty))
                    return false;

                if (!inpt.MascaraCompleta)
                    return false;
            }
            return true;
        }

        private void ClearInputs()
        {
            foreach (InputControl i in inptPainel.Controls)
            {
                i.Text = string.Empty;
            }
        }
        private void DesativateInputs()
        {
            foreach (InputControl i in inptPainel.Controls)
            {
                i.setReadOnly(true);
            }
        }
        private void ActivateInputs()
        {
            foreach (InputControl i in inptPainel.Controls)
            {
                if (!i.isId)
                    i.setReadOnly(false);
            }
        }
        private void DesactivateButtons()
        {
            inptAdicionar.Visible = false;
            inptEditar.Visible = false;
            inptRemover.Visible = false;
            inptPesquisar.Visible = false;
        }
        private void ActivateButtons()
        {
            inptAdicionar.Visible = true;
            inptEditar.Visible = true;
            inptRemover.Visible = true;
            inptPesquisar.Visible = true;
        }

        private bool IsIdInputEmpty()
        {
            foreach (InputControl i in inptPainel.Controls)
            {
                if (i.isId) return i.Text == string.Empty;
            }
            return false;
        }
        private string SelectedId()
        {
            foreach (InputControl i in inptPainel.Controls)
            {
                if (i.isId) return i.Text;
            }
            return "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            paginationStart += PAGINATION_SIZE;
            ButtonPreviousPage.Enabled = true;

            int numberOfRows = updateTable(paginationStart, PAGINATION_SIZE);
            Console.WriteLine("Next retornou: " +numberOfRows);
            
            if(numberOfRows < PAGINATION_SIZE)
                ButtonNextPage.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            paginationStart = paginationStart - PAGINATION_SIZE >= 0
                ? paginationStart - PAGINATION_SIZE 
                : 0;

            if (paginationStart < PAGINATION_SIZE)
                ButtonPreviousPage.Enabled = false;

            Console.WriteLine("Prev retornou: " + updateTable(paginationStart, PAGINATION_SIZE));
            
            ButtonNextPage.Enabled = true;
        }
    }
}
