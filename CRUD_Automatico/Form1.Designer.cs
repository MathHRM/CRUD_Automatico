namespace CRUD_Automatico
{
    partial class Form1
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGD = new System.Windows.Forms.DataGridView();
            this.inptPainel = new System.Windows.Forms.FlowLayoutPanel();
            this.inptAdicionar = new System.Windows.Forms.Button();
            this.inptPesquisar = new System.Windows.Forms.Button();
            this.inptConfirmarPesquisa = new System.Windows.Forms.Button();
            this.inptEditar = new System.Windows.Forms.Button();
            this.inptRemover = new System.Windows.Forms.Button();
            this.inptConfirmarEdicao = new System.Windows.Forms.Button();
            this.inptCancelar = new System.Windows.Forms.Button();
            this.inptConfirmarRemover = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGD)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGD
            // 
            this.dataGD.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGD.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGD.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGD.Location = new System.Drawing.Point(12, 281);
            this.dataGD.Name = "dataGD";
            this.dataGD.Size = new System.Drawing.Size(700, 186);
            this.dataGD.TabIndex = 0;
            this.dataGD.SelectionChanged += new System.EventHandler(this.dataGD_SelectionChanged);
            // 
            // inptPainel
            // 
            this.inptPainel.AutoScroll = true;
            this.inptPainel.Location = new System.Drawing.Point(12, 12);
            this.inptPainel.Name = "inptPainel";
            this.inptPainel.Size = new System.Drawing.Size(700, 205);
            this.inptPainel.TabIndex = 1;
            // 
            // inptAdicionar
            // 
            this.inptAdicionar.Location = new System.Drawing.Point(12, 223);
            this.inptAdicionar.Name = "inptAdicionar";
            this.inptAdicionar.Size = new System.Drawing.Size(75, 23);
            this.inptAdicionar.TabIndex = 2;
            this.inptAdicionar.Text = "Adicionar";
            this.inptAdicionar.UseVisualStyleBackColor = true;
            this.inptAdicionar.Click += new System.EventHandler(this.inptAdicionar_Click);
            // 
            // inptPesquisar
            // 
            this.inptPesquisar.Location = new System.Drawing.Point(255, 223);
            this.inptPesquisar.Name = "inptPesquisar";
            this.inptPesquisar.Size = new System.Drawing.Size(75, 23);
            this.inptPesquisar.TabIndex = 3;
            this.inptPesquisar.Text = "Pesquisar";
            this.inptPesquisar.UseVisualStyleBackColor = true;
            this.inptPesquisar.Click += new System.EventHandler(this.inptPesquisar_Click);
            // 
            // inptConfirmarPesquisa
            // 
            this.inptConfirmarPesquisa.Location = new System.Drawing.Point(637, 223);
            this.inptConfirmarPesquisa.Name = "inptConfirmarPesquisa";
            this.inptConfirmarPesquisa.Size = new System.Drawing.Size(75, 23);
            this.inptConfirmarPesquisa.TabIndex = 4;
            this.inptConfirmarPesquisa.Text = "Pesqusiar";
            this.inptConfirmarPesquisa.UseVisualStyleBackColor = true;
            this.inptConfirmarPesquisa.Click += new System.EventHandler(this.inptConfirmarPesquisa_Click);
            // 
            // inptEditar
            // 
            this.inptEditar.Location = new System.Drawing.Point(93, 223);
            this.inptEditar.Name = "inptEditar";
            this.inptEditar.Size = new System.Drawing.Size(75, 23);
            this.inptEditar.TabIndex = 5;
            this.inptEditar.Text = "Editar";
            this.inptEditar.UseVisualStyleBackColor = true;
            this.inptEditar.Click += new System.EventHandler(this.inptEditar_Click);
            // 
            // inptRemover
            // 
            this.inptRemover.Location = new System.Drawing.Point(174, 223);
            this.inptRemover.Name = "inptRemover";
            this.inptRemover.Size = new System.Drawing.Size(75, 23);
            this.inptRemover.TabIndex = 6;
            this.inptRemover.Text = "Remover";
            this.inptRemover.UseVisualStyleBackColor = true;
            this.inptRemover.Click += new System.EventHandler(this.inptRemover_Click);
            // 
            // inptConfirmarEdicao
            // 
            this.inptConfirmarEdicao.Location = new System.Drawing.Point(637, 252);
            this.inptConfirmarEdicao.Name = "inptConfirmarEdicao";
            this.inptConfirmarEdicao.Size = new System.Drawing.Size(75, 23);
            this.inptConfirmarEdicao.TabIndex = 7;
            this.inptConfirmarEdicao.Text = "Confirmar";
            this.inptConfirmarEdicao.UseVisualStyleBackColor = true;
            this.inptConfirmarEdicao.Click += new System.EventHandler(this.confirmarEdicao_Click);
            // 
            // inptCancelar
            // 
            this.inptCancelar.Location = new System.Drawing.Point(475, 223);
            this.inptCancelar.Name = "inptCancelar";
            this.inptCancelar.Size = new System.Drawing.Size(75, 23);
            this.inptCancelar.TabIndex = 8;
            this.inptCancelar.Text = "Cancelar";
            this.inptCancelar.UseVisualStyleBackColor = true;
            this.inptCancelar.Click += new System.EventHandler(this.inptCancelar_Click);
            // 
            // inptConfirmarRemover
            // 
            this.inptConfirmarRemover.Location = new System.Drawing.Point(556, 223);
            this.inptConfirmarRemover.Name = "inptConfirmarRemover";
            this.inptConfirmarRemover.Size = new System.Drawing.Size(75, 23);
            this.inptConfirmarRemover.TabIndex = 9;
            this.inptConfirmarRemover.Text = "Excluir";
            this.inptConfirmarRemover.UseVisualStyleBackColor = true;
            this.inptConfirmarRemover.Click += new System.EventHandler(this.inptConfirmarRemover_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(724, 498);
            this.Controls.Add(this.inptConfirmarRemover);
            this.Controls.Add(this.inptCancelar);
            this.Controls.Add(this.inptConfirmarEdicao);
            this.Controls.Add(this.inptRemover);
            this.Controls.Add(this.inptEditar);
            this.Controls.Add(this.inptConfirmarPesquisa);
            this.Controls.Add(this.inptPesquisar);
            this.Controls.Add(this.inptAdicionar);
            this.Controls.Add(this.inptPainel);
            this.Controls.Add(this.dataGD);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dataGD)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGD;
        private System.Windows.Forms.FlowLayoutPanel inptPainel;
        private System.Windows.Forms.Button inptAdicionar;
        private System.Windows.Forms.Button inptPesquisar;
        private System.Windows.Forms.Button inptConfirmarPesquisa;
        private System.Windows.Forms.Button inptEditar;
        private System.Windows.Forms.Button inptRemover;
        private System.Windows.Forms.Button inptConfirmarEdicao;
        private System.Windows.Forms.Button inptCancelar;
        private System.Windows.Forms.Button inptConfirmarRemover;
    }
}

