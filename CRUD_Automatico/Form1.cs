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

            foreach (var col in colunas)
            {
                var input = new InputControl(col);

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

            if (!validadeInputs())
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
        }

        private bool validadeInputs()
        {
            foreach (InputControl i in inptPainel.Controls)
            {
                if(!i.isId)
                    if (i.Value == string.Empty) return false;
            }
            return true;
        }
    }
}
