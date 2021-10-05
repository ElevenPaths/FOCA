using System;
using System.Threading;
using System.Windows.Forms;

namespace FOCA
{
    public partial class FormAddDomain : Form
    {
        public FormAddDomain()
        {
            InitializeComponent();
            string text;
            if (Program.data.Project.Domain != null)
                text = "subdomain" + "." + Program.data.Project.Domain;
            else
                text = "domain.com";

            txtDomain.Text = text;
        }

        private void btAddDomain_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtDomain.Text))
                return;

            var t = new Thread(AddDomain);
            t.Start();
            MessageBox.Show(@"Domain added successfully", @"Added", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        ///     Add a new domain to the project and set its origin to "Manually added domain"
        /// </summary>
        private void AddDomain()
        {
            Program.data.AddDomain(txtDomain.Text, "Manually added domain", Program.cfgCurrent.MaxRecursion,
                Program.cfgCurrent);
        }
    }
}
