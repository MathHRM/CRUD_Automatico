﻿using System;
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

        public bool isId { get; } 

        public InputControl(string col, bool isId)
        {
            InitializeComponent();
            this.isId = isId;

            if(isId)
                setReadOnly(true);

            lbl.Text = col;
            _colRef = col;
        }

        public void filterId()
        {
            inpt.BackColor = Color.LightGray;
            inpt.ReadOnly = true;
        }

        public void setReadOnly(bool b)
        {
            if(b) 
            {
                inpt.BackColor = Color.LightGray;
                inpt.ReadOnly = true;
            }
            else
            {
                inpt.BackColor = Color.White;
                inpt.ReadOnly = false;
            }
        }
    }
}
