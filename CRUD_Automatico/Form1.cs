using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace CRUD_Automatico
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            var testaConexao = new Cadastro(new MySqlConnection(BDInfo.Server));
            testaConexao.TestaConexao();
            definirInputs();
            updateTabela();

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
            Cadastro adicionar;

            if (!inputsPreenchidos())
            {
                MessageBox.Show("Preencha todos os campos");
                return;
            }

            foreach (InputControl i in inptPainel.Controls)
            {
                if (!i.isId)
                    values.Add(i.Column, i.Value);
            }

            adicionar = new Cadastro(new MySqlConnection(BDInfo.Server));
            bool adicionado = adicionar.Adicionar(values);
            if (!adicionado)
            {
                MessageBox.Show("Erro ao adicionar, verifique os dados e tente novamente");
                return;
            }

            updateTabela();
            limparInputs();
        }

        // Editar
        private void inptEditar_Click(object sender, EventArgs e)
        {
            if (isIdEmpty())
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
                    values.Add(i.Column, i.Value);
                else
                    stringId = i.Value;
            }


            int id = int.Parse(stringId);

            Cadastro update = new Cadastro(new MySqlConnection(BDInfo.Server));
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
            updateTabela();
        }

        // Remover
        private void inptRemover_Click(object sender, EventArgs e)
        {
            if (isIdEmpty())
            {
                modoPesquisa();
            }
        }
        private void inptConfirmarRemover_Click(object sender, EventArgs e)
        {
            int id = int.Parse(idSelecionado());

            Cadastro excluir = new Cadastro(new MySqlConnection(BDInfo.Server));
            bool removido = excluir.Remover(id);
            if (!removido)
            {
                MessageBox.Show("Erro ao remover");
                return;
            }

            updateTabela();

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
        private void updateTabela()
        {
            Cadastro pesquisar = new Cadastro(new MySqlConnection(BDInfo.Server));
            var tabela = pesquisar.PesquisarTodos();

            DataTable dt = new DataTable();
            dt.Load(tabela);

            dataGD.DataSource = dt;
            dataGD.AutoResizeColumns();
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

            foreach (InputControl i in inptPainel.Controls)
            {
                if (i.isId) i.Value = idRowSelecionado.ToString();
            }
            pesquisar();
        }



        // adiciona os inputs baseado nas colunas da tabela
        private void definirInputs()
        {
            Cadastro c = new Cadastro(new MySqlConnection(BDInfo.Server));
            var colunas = c.GetColumns();

            if(colunas == null)
            {
                MessageBox.Show("Erro ao exibir inputs");
                return;
            }

            foreach (var nomeCol in colunas.Keys)
            {
                InputControl input = new InputControl(colunas[nomeCol]);

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
            Cadastro p = new Cadastro(new MySqlConnection(BDInfo.Server));

            if (isIdEmpty())
            {
                MessageBox.Show("Digite um id");
                return;
            }

            int id;
            if (!int.TryParse(idSelecionado(), out id))
            {
                MessageBox.Show("ID inválido");
                return;
            }
            var resultado = p.Pesquisar(id);

            if (resultado == null)
            {
                MessageBox.Show("Funcionario não encontrado: não existe");
                return;
            }

            if (!resultado.HasRows)
            {
                MessageBox.Show("Funcionario não encontrado: sem dados");
                return;
            }

            resultado.Read();

            foreach (InputControl i in inptPainel.Controls)
            {
                i.Value = resultado[i.Column].ToString();
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
            bool preenchidos = true;

            for (int i = 0; i < inptPainel.Controls.Count; i++)
            {
                InputControl inpt = (InputControl)inptPainel.Controls[i];

                if (!inpt.isId)
                    continue;

                if (!inpt.IsNullable && inpt.Value.Equals(string.Empty))
                {
                    preenchidos = false;
                    continue;
                }

                if (!inpt.MascaraCompleta)
                    preenchidos = false;
            }

            return preenchidos;
        }

        private void limparInputs()
        {
            foreach (InputControl i in inptPainel.Controls)
            {
                i.Value = string.Empty;
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

        private bool isIdEmpty()
        {
            foreach (InputControl i in inptPainel.Controls)
            {
                if (i.isId) return i.Value == string.Empty;
            }
            return false;
        }
        private string idSelecionado()
        {
            foreach (InputControl i in inptPainel.Controls)
            {
                if (i.isId) return i.Value;
            }
            return "";
        }
    }
}
