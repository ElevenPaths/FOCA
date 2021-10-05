using System;
using System.Windows.Forms;

namespace FOCA
{
    public partial class FormValueNotes : Form
    {
        public FormValueNotes()
        {
            InitializeComponent();
        }

        public string Value { get; set; }
        public string Notes { get; set; }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Value = txtValue.Text;
            Notes = txtNotes.Text;
            DialogResult = DialogResult.OK;
        }
    }
}