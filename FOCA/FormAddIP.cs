using FOCA.Database.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace FOCA
{
    public partial class FormAddIp : Form
    {
        private string strIp;

        public FormAddIp()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     Add an IP only if it is valid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btAddIp_Click(object sender, EventArgs e)
        {
            if ((txtOct4.Text == @"0") || (txtOct4.Text == @"255"))
            {
                MessageBox.Show(@"Invalid IP address", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                var oct1 = int.Parse(txtOct1.Text);
                var oct2 = int.Parse(txtOct2.Text);
                var oct3 = int.Parse(txtOct3.Text);
                var oct4 = int.Parse(txtOct4.Text);

                if ((oct1 >= 0) && (oct1 <= 255) &&
                    (oct2 >= 0) && (oct2 <= 255) &&
                    (oct3 >= 0) && (oct3 <= 255) &&
                    (oct4 >= 0) && (oct4 <= 255))
                {
                    strIp = oct1 + "." + oct2 + "." + oct3 + "." + oct4;
                }
                else
                {
                    MessageBox.Show(@"Invalid IP address", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                IPsItem ip = Program.data.GetIp(strIp);
                if (ip == null || String.IsNullOrWhiteSpace(ip.Ip))
                {
                    var t = new Thread(AddIp);
                    t.Start();
                    MessageBox.Show(@"Successfully added IP", @"Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                }
                else
                {
                    MessageBox.Show(@"IP address already exists in the project", @"Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch
            {
                MessageBox.Show(@"Invalid IP address", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        ///     Add an IP to the project and set its source to "Manually added IP"
        /// </summary>
        private void AddIp()
        {
            Program.data.AddIP(strIp, "Manually added IP", Program.cfgCurrent.MaxRecursion);

            var anyRelations = Program.data.relations.Items.Any(R => R.Ip != null && R.Ip.Ip == strIp);

            if (anyRelations) return;
            var ipItem = Program.data.GetIp(strIp);
            var computerItem = new ComputersItem
            {
                type = ComputersItem.Tipo.Server,
                name = strIp,
                os = OperatingSystem.OS.Unknown
            };
            Program.data.computers.Items.Add(computerItem);
            Program.data.computerIPs.Items.Add(new ComputerIPsItem(computerItem, ipItem, "Manually added IP"));
        }

        private void FormAddIp_Load(object sender, EventArgs e)
        {

        }
    }
}
