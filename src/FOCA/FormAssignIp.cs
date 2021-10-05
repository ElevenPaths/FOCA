using FOCA.Database.Entities;
using System;
using System.Linq;
using System.Windows.Forms;

namespace FOCA
{
    public partial class FormAssignIp : Form
    {
        ComputersItem computer;

        public FormAssignIp(ComputersItem computer)
        {
            InitializeComponent();
            this.computer = computer;
        }

        private void FormAsignIp_Load(object sender, EventArgs e)
        {
            foreach (var ipsItem in Program.data.computerIPs.Items.Where(ipsItem => ipsItem.Computer == computer))
            {
                lstIps.Items.Add(ipsItem.Ip.Ip);
            }
        }

        private void btDel_Click(object sender, EventArgs e)
        {
            var ip = txtIp.Text;
            foreach (var i in Program.data.computerIPs.Items.Where(i => i.Ip.Ip == ip).ToList())
            {
                Program.data.computerIPs.Items.Remove(i);
            }
        }

        private void btAdd_Click(object sender, EventArgs e)
        {
            var ip = txtIp.Text;
            Program.data.computerIPs.Items.Add(new ComputerIPsItem(computer, new IPsItem(ip, "Added manually"), "Added manually"));
        }
    }
}
