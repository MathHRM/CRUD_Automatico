using System.Windows.Forms;

namespace CRUD_Automatico
{
    public partial class InputControl : UserControl
    {
        private string _colRef;
        public string Column { get { return _colRef; } }

        public string Text
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

        public InputControl(MySqlColumn col)
        {
            InitializeComponent();

            lbl.Text = col.Nome;
            _colRef = col.Nome;

            if (col.IsKey)
            {
                this.isId = true;
                setReadOnly(true);
            }

            if (col.IsNullable)
            {
                _nullable = true;
            }

            if (col.Nome.ToLower().Contains("cpf"))
            {
                inpt.Mask = "000.000.000-00";
                return;
            }

            if (col.Nome.ToLower().Contains("telefone"))
            {
                inpt.Mask = "(00) 0 0000-0000";
                return;
            }

            if (col.StrDataType.Equals("date"))
            {
                inpt.Mask = "00/00/0000";
                return;
            }
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
