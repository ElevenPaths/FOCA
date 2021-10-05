using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace FOCA
{
    public partial class FormAddAltDnsParser : Form
    {
        public FormAddAltDnsParser()
        {
            InitializeComponent();
        }

        private void btSelectFile_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog {Filter = @"Text Files (.txt)|*.txt"};
            if (ofd.ShowDialog() != DialogResult.OK) return;
            txtFilePath.Text = ofd.FileName;
            var subdomains = parseFile(txtFilePath.Text);
            foreach (
                var t in
                    subdomains.Select(
                        subdomain =>
                            new Thread(
                                () =>
                                    Program.data.AddDomain(subdomain, "Subdomain obtained from altdns output file",
                                        Program.cfgCurrent.MaxRecursion, Program.cfgCurrent))))
            {
                t.Start();
            }

            var result = MessageBox.Show(@"Subdomains were added", @"Success", MessageBoxButtons.OK);
            if (result == DialogResult.OK)
            {
                Close();
            }
        }

        /// <summary>
        ///     Reads a text file and parses it. Expected format is the one created by altdns tool.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private IEnumerable<string> parseFile(string path)
        {
            var subdomains = new List<string>();
            using (var reader = File.OpenText(path))
            {
                string l;
                while ((l = reader.ReadLine()) != null)
                {
                    var parts = l.Split(':');
                    if (parts.Length > 0) subdomains.Add(parts[0]);
                }
            }
            return subdomains;
        }
    }
}