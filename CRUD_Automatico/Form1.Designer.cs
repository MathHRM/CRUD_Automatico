﻿namespace CRUD_Automatico
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
            ((System.ComponentModel.ISupportInitialize)(this.dataGD)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGD
            // 
            this.dataGD.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGD.Location = new System.Drawing.Point(12, 269);
            this.dataGD.Name = "dataGD";
            this.dataGD.Size = new System.Drawing.Size(700, 169);
            this.dataGD.TabIndex = 0;
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
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(724, 450);
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
    }
}
