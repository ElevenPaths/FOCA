using System;
using System.IO;
using System.Windows.Forms;

namespace FOCA
{
    /// <summary>
    ///     Select the path where your subdomains wordlist is located
    /// </summary>
    public partial class PanelTryCommonNames : UserControl
    {
        public PanelTryCommonNames()
        {
            InitializeComponent();
        }

        public string ListCommonNamesFileName
        {
            set { txtCommonNamesPath.Text = value; }
            get { return txtCommonNamesPath.Text; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCommonNamesPath.Text))
                openFileDialog.InitialDirectory = Application.StartupPath + @"\DNSDictionary\";
            else
                openFileDialog.InitialDirectory = Path.GetDirectoryName(txtCommonNamesPath.Text);
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtCommonNamesPath.Text = openFileDialog.FileName;
            }
        }
    }
}