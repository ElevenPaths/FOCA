using System;
using System.Windows.Forms;
using PluginsAPI;

namespace FocaPluginExample
{
    public partial class FrmPluginExample : Form
    {
        public FrmPluginExample()
        {
            InitializeComponent();
        }

        private void btnSendFoca_Click(object sender, EventArgs e)
        {
            try
            {
                var uri = new Uri(txtDomain.Text);
                SendUriToFoca((object) uri.AbsoluteUri);
            }
            catch
            {
                MessageBox.Show(@"Invalid URI", "", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }


        /// <summary>
        /// Send value to Foca and analize network for this domain.
        /// </summary>
        /// <param name="value"></param>
        private void SendUriToFoca(object o)
        {
            try
            {
                Import.ImportEventCaller(new ImportObject((Import.Operation) 1, (object) o.ToString()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
