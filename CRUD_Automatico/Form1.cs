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
        int paginationEnd = PAGINATION_SIZE;

        Dictionary<string, InputControl> Name_InputsPair = new Dictionary<string, InputControl>();

        public Form1()
        {
            InitializeComponent();
            var testaConexao = new AutoCrud(new MySqlConnection(BDInfo.Server));
            testaConexao.TestaConexao();
            defineInputs();
            updateTable(paginationStart, paginationEnd);

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

            if (!inputsPreenchidos())
            {
                MessageBox.Show("Preencha todos os campos");
                return;
            }

            foreach (InputControl i in inptPainel.Controls)
            {
                if (!i.isId)
                    values.Add(i.Column, i.InputText);
            }

            adicionar = new AutoCrud(new MySqlConnection(BDInfo.Server));
            bool adicionado = adicionar.Add(values);
            if (!adicionado)
            {
                MessageBox.Show("Erro ao adicionar, verifique os dados e tente novamente");
                return;
            }

            updateTable(paginationStart, paginationEnd);
            limparInputs();
        }

        // Editar
        private void inptEditar_Click(object sender, EventArgs e)
        {
            if (IsIdInputEmpty())
            {
                modoPesquisa();
                return;
            }

            // caso clicado após uma pesquisa irá editar o resultado
            desativarBotoes();
            ativarInputs();

            inptCancelar.Visible = true;
            inptConfirmarRemover.Visible = false;
            inptConfirmarEdicao.Visible = true;
        }
        private void confirmarEdicao_Click(object sender, EventArgs e)
        {
            var values = new Dictionary<string, string>();
            string stringId = "";

            if (!inputsPreenchidos())
            {
                MessageBox.Show("Preencha todos os campos");
                return;
            }

            foreach (InputControl i in inptPainel.Controls)
            {
                if (!i.isId)
                    values.Add(i.Column, i.InputText);
                else
                    stringId = i.InputText;
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

            ativarBotoes();
            limparInputs();
            updateTable(paginationStart, paginationEnd);
        }

        // Remover
        private void inptRemover_Click(object sender, EventArgs e)
        {
            if (IsIdInputEmpty())
            {
                modoPesquisa();
            }
        }
        private void inptConfirmarRemover_Click(object sender, EventArgs e)
        {
            int id = int.Parse(idSelecionado());

            AutoCrud excluir = new AutoCrud(new MySqlConnection(BDInfo.Server));
            bool removido = excluir.Remove(id);
            if (!removido)
            {
                MessageBox.Show("Erro ao remover");
                return;
            }

            updateTable(paginationStart ,paginationEnd);

            limparInputs();
            ativarBotoes();
            ativarInputs();
            inptConfirmarRemover.Visible = false;
            inptCancelar.Visible = false;
        }

        // Pesquisar
        private void inptPesquisar_Click(object sender, EventArgs e)
        {
            modoPesquisa();
        }
        private void inptConfirmarPesquisa_Click(object sender, EventArgs e)
        {
            pesquisar();
        }

        // Cancelar
        private void inptCancelar_Click(object sender, EventArgs e)
        {
            ativarInputs();
            ativarBotoes();
            limparInputs();

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
        private int updateTable(int start, int end)
        {
            AutoCrud pesquisar = new AutoCrud(new MySqlConnection(BDInfo.Server));
            var tabela = pesquisar.GetInterval(start, end);

            DataTable dt = new DataTable();
            dt.Load(tabela);

            Console.WriteLine($"start = {start} /// end = {end}");
            Console.WriteLine($"res length: {dt.Rows.Count}\n");

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
                Name_InputsPair[dataGD.Columns[i].Name].InputText = dataGD.Rows[dataGD.SelectedRows[0].Index].Cells[i].Value.ToString();
            }
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
        private void modoPesquisa()
        {
            foreach (InputControl i in inptPainel.Controls)
            {
                i.setReadOnly(!i.isId);
            }
            desativarBotoes();
            inptConfirmarPesquisa.Visible = true;
            inptCancelar.Visible = true;
        }

        // Pesquisa baseado no id fornecido
        private void pesquisar()
        {
            AutoCrud p = new AutoCrud(new MySqlConnection(BDInfo.Server));

            if (IsIdInputEmpty())
            {
                MessageBox.Show("Digite um id");
                return;
            }

            int id;
            if (!int.TryParse(idSelecionado(), out id))
            {
                MessageBox.Show("ID inválido");
                limparInputs();
                return;
            }

            var row = p.SearchRow(id).Rows;

            if (row == null)
            {
                MessageBox.Show("Funcionario não encontrado: erro");
                limparInputs();
                return;
            }

            if (row.Count == 0)
            {
                MessageBox.Show("Funcionario não encontrado: sem dados");
                limparInputs();
                return;
            }

            foreach (InputControl i in inptPainel.Controls)
            {
                i.InputText = row[0][i.Column].ToString();
            }

            desativarInputs();
            desativarBotoes();
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
        private bool inputsPreenchidos()
        {
            for (int i = 0; i < inptPainel.Controls.Count; i++)
            {
                InputControl inpt = (InputControl)inptPainel.Controls[i];

                if (!inpt.isId)
                    continue;

                if (!inpt.IsNullable && inpt.InputText.Equals(string.Empty))
                    return false;

                if (!inpt.MascaraCompleta)
                    return false;
            }
            return true;
        }

        private void limparInputs()
        {
            foreach (InputControl i in inptPainel.Controls)
            {
                i.InputText = string.Empty;
            }
        }
        private void desativarInputs()
        {
            foreach (InputControl i in inptPainel.Controls)
            {
                i.setReadOnly(true);
            }
        }
        private void ativarInputs()
        {
            foreach (InputControl i in inptPainel.Controls)
            {
                if (!i.isId)
                    i.setReadOnly(false);
            }
        }
        private void desativarBotoes()
        {
            inptAdicionar.Visible = false;
            inptEditar.Visible = false;
            inptRemover.Visible = false;
            inptPesquisar.Visible = false;
        }
        private void ativarBotoes()
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
                if (i.isId) return i.InputText == string.Empty;
            }
            return false;
        }
        private string idSelecionado()
        {
            foreach (InputControl i in inptPainel.Controls)
            {
                if (i.isId) return i.InputText;
            }
            return "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            paginationStart = paginationEnd;
            // +1 para ter a certeza q qnd retornar um numero de elementos igual
            // a PAGINATION_SIZE, seja realmente o final da tabela
            paginationEnd += PAGINATION_SIZE + 1; 
            ButtonPreviousPage.Enabled = true;

            int numberOfRows = updateTable(paginationStart, paginationEnd);
            
            if(numberOfRows <= PAGINATION_SIZE)
                ButtonNextPage.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            paginationEnd = paginationStart;
            paginationStart = paginationStart - PAGINATION_SIZE > 0 ? paginationStart - PAGINATION_SIZE -1 : 0;

            if (paginationStart <= PAGINATION_SIZE/2)
                ButtonPreviousPage.Enabled = false;

            updateTable(paginationStart, paginationEnd);
            
            ButtonNextPage.Enabled = true;
        }
    }
}
