namespace CRUD_Automatico
{
    partial class InputControl
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

        #region Código gerado pelo Designer de Componentes

        /// <summary> 
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.inpt = new System.Windows.Forms.TextBox();
            this.lbl = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // inpt
            // 
            this.inpt.Location = new System.Drawing.Point(74, 3);
            this.inpt.Name = "inpt";
            this.inpt.Size = new System.Drawing.Size(194, 20);
            this.inpt.TabIndex = 0;
            // 
            // lbl
            // 
            this.lbl.AutoSize = true;
            this.lbl.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl.Location = new System.Drawing.Point(3, 3);
            this.lbl.Name = "lbl";
            this.lbl.Size = new System.Drawing.Size(50, 18);
            this.lbl.TabIndex = 1;
            this.lbl.Text = "label1";
            // 
            // InputControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lbl);
            this.Controls.Add(this.inpt);
            this.Name = "InputControl";
            this.Size = new System.Drawing.Size(277, 32);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox inpt;
        private System.Windows.Forms.Label lbl;
    }
}
