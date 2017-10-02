using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace FOCA
{
    public partial class FormAddIpRange : Form
    {
        public FormAddIpRange()
        {
            InitializeComponent();
        }

        private void btAddIp_Click(object sender, EventArgs e)
        {
            try
            {
                var oct1From = int.Parse(txtOct1From.Text);
                var oct2From = int.Parse(txtOct2From.Text);
                var oct3From = int.Parse(txtOct3From.Text);
                var oct4From = int.Parse(txtOct4From.Text);

                var oct1To = int.Parse(txtOct1To.Text);
                var oct2To = int.Parse(txtOct2To.Text);
                var oct3To = int.Parse(txtOct3To.Text);
                var oct4To = int.Parse(txtOct4To.Text);

                var ip1 = ulong.Parse((oct1From + oct2From + oct3From + oct4From).ToString());
                var ip2 = ulong.Parse((oct1To + oct2To + oct3To + oct4To).ToString());

                if ((oct1From >= 0) && (oct1From <= 255) && (oct1To >= 0) && (oct1To <= 255) &&
                    (oct2From >= 0) && (oct2From <= 255) && (oct2To >= 0) && (oct2To <= 255) &&
                    (oct3From >= 0) && (oct3From <= 255) && (oct3To >= 0) && (oct3To <= 255) &&
                    (oct4From >= 0) && (oct4From <= 255) && (oct4To >= 0) && (oct4To <= 255) &&
                    (ip1 < ip2))
                {
                    var lstIps = GenerateIpList(oct1From, oct2From, oct3From, oct4From,
                        oct1To, oct2To, oct3To, oct4To);
                    var t = new Thread(AddIps);
                    t.Start(lstIps);

                    MessageBox.Show(@"Ip range added successfully", @"Done", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    Error();
                }
            }
            catch
            {
                Error();
            }
        }

        /// <summary>
        ///     Add a list of IP addresses to the project
        /// </summary>
        /// <param name="oLstIps">List of IP addresses as string objects</param>
        private static void AddIps(object oLstIps)
        {
            var lstIps = oLstIps as List<string>;

            if (lstIps == null) return;
            foreach (var ip in lstIps)
            {
                AddIp(ip);
            }
        }

        /// <summary>
        ///     Add an IP address to the project
        /// </summary>
        /// <param name="ip">IP address</param>
        private static void AddIp(object ip)
        {
            Program.data.AddIP(ip as string, "Manually added IP", Program.cfgCurrent.MaxRecursion);
        }

        /// <summary>
        ///     Check if there are more IP addresses in a given range of octets
        /// </summary>
        /// <param name="oct1From"></param>
        /// <param name="oct2From"></param>
        /// <param name="oct3From"></param>
        /// <param name="oct4From"></param>
        /// <param name="oct1To"></param>
        /// <param name="oct2To"></param>
        /// <param name="oct3To"></param>
        /// <param name="oct4To"></param>
        /// <returns>true if there are more IP addresses</returns>
        private static bool MoreIps(int oct1From, int oct2From, int oct3From, int oct4From,
            int oct1To, int oct2To, int oct3To, int oct4To)
        {
            if (oct1From < oct1To)
                return true;
            if ((oct1From <= oct1To) && (oct2From < oct2To))
                return true;
            if ((oct1From <= oct1To) && (oct2From <= oct2To) && (oct3From < oct3To))
                return true;
            return (oct1From <= oct1To) && (oct2From <= oct2To) && (oct3From <= oct3To) && (oct4From <= oct4To);
        }

        /// <summary>
        ///     Given a range of octets, generate a range of IP addresses
        /// </summary>
        /// <param name="oct1From"></param>
        /// <param name="oct2From"></param>
        /// <param name="oct3From"></param>
        /// <param name="oct4From"></param>
        /// <param name="oct1To"></param>
        /// <param name="oct2To"></param>
        /// <param name="oct3To"></param>
        /// <param name="oct4To"></param>
        /// <returns>List which contains the IP addresses</returns>
        private List<string> GenerateIpList(int oct1From, int oct2From, int oct3From, int oct4From,
            int oct1To, int oct2To, int oct3To, int oct4To)
        {
            var lstIps = new List<string>();

            while (MoreIps(oct1From, oct2From, oct3From, oct4From, oct1To, oct2To, oct3To, oct4To))
            {
                if (oct2From > 255)
                {
                    oct1From++;
                    oct2From = 0;
                }
                if (oct3From > 255)
                {
                    oct2From++;
                    oct3From = 0;
                }
                if (oct4From > 255)
                {
                    oct3From++;
                    oct4From = 0;
                }

                var ip = oct1From + "." + oct2From + "." + oct3From + "." + oct4From;
                lstIps.Add(ip);

                oct4From++;
            }
            return lstIps;
        }

        /// <summary>
        ///     Create a message box showing an invalid IP address or range error
        /// </summary>
        private static void Error()
        {
            MessageBox.Show(@"Invalid IP Address or range", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}