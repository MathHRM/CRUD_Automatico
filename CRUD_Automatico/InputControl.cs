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
    public partial class InputControl : UserControl
    {
        private string _colRef;
        public string Column { get { return _colRef; } }

        public string Value
        {
            get { return inpt.Text; }
            set { inpt.Text = value; }
        }

        public MaskedTextBox Input { get { return inpt; } } 

        public bool isId { get; }

        private bool _nullable = false;
        public bool IsNullable { get { return _nullable; } }

        public bool MascaraCompleta { get { return inpt.MaskFull; } }


        public InputControl(string col)
        {
            InitializeComponent();

            lbl.Text = col;
            _colRef = col;
        }

        public InputControl(string col, string properties)
        {
            InitializeComponent();

            if (properties.Contains("PRIMARY KEY")) { 
                this.isId = true; 
                setReadOnly(true);
            }

            if (properties.Contains("NULLABLE"))
                _nullable = true;

            if (properties.Contains("READ ONLY"))
                setReadOnly(true);

            if (col.ToLower().Contains("cpf"))
                inpt.Mask = "000.000.000-00";

            if (col.ToLower().Contains("telefone"))
                inpt.Mask = "(00) 0 0000-0000";

            lbl.Text = col;
            _colRef = col;
        }

        public void setReadOnly(bool enabled)
        {
            Input.Enabled = !enabled;
        }

        public void setMask(string mask)
        {
            inpt.Mask = mask;
        }
    }
}
