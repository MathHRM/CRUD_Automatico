using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRUD_Automatico
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            var testaConexao = new Cadastro();
            testaConexao.TestaConexao();
            setAllInputs();
            ShowAll();
        }








        private void setAllInputs()
        {
            Cadastro c = new Cadastro();
            var colunas = c.ColunasAtributos;
            var nomeColunas = c.NomeColunas;

            foreach (var nCol in nomeColunas)
            {
                InputControl input;
                if (!colunas[nCol].Equals("PRIMARY KEY"))
                    input = new InputControl(nCol);
                else
                    input = new InputControl(nCol, true);

                inptPainel.Controls.Add(input);
            }
        }

        private void ShowAll()
        {
            Cadastro pesquisar = new Cadastro();
            var tabela = pesquisar.PesquisarTodos();

            DataTable dt = new DataTable();
            dt.Load(tabela);

            dataGD.DataSource = dt;
            dataGD.AutoResizeColumns();
        }

        private void inptAdicionar_Click(object sender, EventArgs e)
        {
            var values = new Dictionary<string, string>();
            Cadastro adicionar;

            if (!inputsPreenchidos())
            {
                MessageBox.Show("Preencha todos os inputs");
                return;
            }

            foreach (InputControl i in inptPainel.Controls)
            {
                if (!i.isId)
                    values.Add(i.Column, i.Value);
            }

            adicionar = new Cadastro();
            bool adicionado = adicionar.Adicionar(values);
            if(!adicionado)
            {
                MessageBox.Show("Erro ao adicionar, verifique os dados e tente novamente");
                return;
            }

            ShowAll();
            limparInputs();
        }

        private bool inputsPreenchidos()
        {
            foreach (InputControl i in inptPainel.Controls)
            {
                if(!i.isId)
                    if (i.Value == string.Empty) return false;
            }
            return true;
        }

        private void limparInputs()
        {
            foreach (InputControl i in inptPainel.Controls)
            {
                i.Input.Text = string.Empty;
            }
        }

        private void inptPesquisar_Click(object sender, EventArgs e)
        {
            foreach( InputControl i in inptPainel.Controls)
            {
                i.setReadOnly(!i.isId);
            }
            inptPesquisar.Enabled = false;
            inptConfirmarPesquisa.Enabled = true;
        }

        private void inptConfirmarPesquisa_Click(object sender, EventArgs e)
        {
            pesquisar();
        }

        private void pesquisar()
        {
            Cadastro p = new Cadastro();
            string stringId = "";
            foreach (InputControl input in inptPainel.Controls)
            {
                if (input.isId) stringId = input.Input.Text;
            }

            if (stringId == string.Empty)
            {
                MessageBox.Show("Digite um id");
                return;
            }

            int id = int.Parse(stringId);
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
                i.Input.Text = resultado[i.Column].ToString();
            }

            inptConfirmarPesquisa.Visible = false;
            desativarInputs();
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
                i.setReadOnly(false);
            }
        }

        private void desativarBotoes()
        {

        }

        private void dataGD_SelectionChanged(object sender, EventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            int contLinhas = dgv.Rows.Count;
            string idSelecionado;

            if (contLinhas > 0)
            {
                bool rowSelecionado = dataGD.SelectedRows.Count > 0;
                if (rowSelecionado)
                {
                    idSelecionado = dataGD.Rows[dataGD.SelectedRows[0].Index].Cells[0].Value.ToString();
                    foreach (InputControl i in inptPainel.Controls)
                    {
                        if (i.isId) i.Input.Text = idSelecionado;
                    }
                    pesquisar();
                }
            }
        }
    }
}
