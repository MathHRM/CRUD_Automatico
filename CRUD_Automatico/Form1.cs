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
            var colunas = c.Colunas;
            var nomeColunas = c.NomeColunas;

            foreach (var col in nomeColunas)
            {
                var input = new InputControl(col, colunas[col].Equals("PRIMARY KEY"));

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
            adicionar.Adicionar(values);
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
            Cadastro p = new Cadastro();
            string strId = "";
            foreach (InputControl i in inptPainel.Controls)
            {
                if (i.isId) strId = i.Input.Text;
            }

            if (strId == string.Empty)
            {
                MessageBox.Show("Digite um id");
                return;
            }

            int id = int.Parse(strId);
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
        }
    }
}
