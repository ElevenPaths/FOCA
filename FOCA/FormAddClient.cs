using FOCA.Database.Entities;
using System;
using System.Threading;
using System.Windows.Forms;

namespace FOCA
{
    public partial class FormAddClient : Form
    {
        public FormAddClient()
        {
            InitializeComponent();
        }

        private void btAddDomain_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPcName.Text))
                return;

            var t = new Thread(AddPc);
            t.Start();
            MessageBox.Show(@"Domain successfully added", @"Added", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        ///     Add a new computer to the project and set its operating system to unknown
        /// </summary>
        private void AddPc()
        {
            var computer = new ComputersItem
            {
                name = txtPcName.Text,
                os = OperatingSystem.OS.Unknown,
                NotOS = true
            };
            Program.data.computers.Items.Add(computer);
        }
    }
}