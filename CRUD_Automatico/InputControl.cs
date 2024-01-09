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
        public string Value { get { return inpt.Text; } }
        public string Column { get { return _colRef; } }
        public TextBox Input { get { return inpt; } } 

        public InputControl(string col)
        {
            InitializeComponent();
            inpt.Name = $"inpt{col}";
            lbl.Text = col;
            _colRef = col;
        }
    }
}
