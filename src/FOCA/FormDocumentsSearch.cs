using System;
using System.Drawing;
using System.Windows.Forms;

namespace FOCA
{
    public partial class FormDocumentsSearch : Form
    {
        public FormDocumentsSearch()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     Create the form and align it
        /// </summary>
        /// <param name="frmParent">Parent form, will be used to set the position of the new form</param>
        public FormDocumentsSearch(Form frmParent) : this()
        {
            Left = frmParent.Left + (frmParent.Width/2) - Width/2;
            Top = frmParent.Top + (frmParent.Height/2) - Height/2;
        }

        private void listBoxFound_SelectedValueChanged(object sender, EventArgs e)
        {
            if (lstDocumentsFound.SelectedItem == null) return;
            BringToFront();

            var tnSearched =
                Program.FormMainInstance.TreeViewMetadataSearchDocument(lstDocumentsFound.SelectedItem.ToString());
            if (tnSearched == null) return;

            Program.FormMainInstance.TreeView.SelectedNode = tnSearched;
            Program.FormMainInstance.TreeViewProjectAfterSelect(null,
                new TreeViewEventArgs(Program.FormMainInstance.TreeView.SelectedNode));
            tnSearched.ForeColor = SystemColors.ActiveCaption;
        }
    }
}