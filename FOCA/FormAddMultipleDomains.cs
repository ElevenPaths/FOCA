using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace FOCA
{
    public partial class FormAddMultipleDomains : Form
    {
        public FormAddMultipleDomains()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var hostnames = new List<string>();
            var ofd = new OpenFileDialog
            {
                Filter = @"Text files (*.txt)|*.txt",
                CheckPathExists = true
            };
            if (ofd.ShowDialog() != DialogResult.OK) return;
            // read file into list
            using (var reader = File.OpenText(ofd.FileName))
            {
                string l;
                while ((l = reader.ReadLine()) != null)
                {
                    if (validateURL(l))
                    {
                        hostnames.Add(l);
                    }
                }
            }

            var t = new Thread(AddDomains);
            t.Start(hostnames);
            MessageBox.Show(@"Domains added successfully", @"Added domains", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        /// <summary>
        ///     Given a list of domains, add them to the project
        /// </summary>
        /// <param name="o">Object which contains the list of domains</param>
        private void AddDomains(object o)
        {
            var list = o as List<string>;
            if (list == null) return;
            foreach (var s in list)
            {
                Program.data.AddDomain(s, "Manually added domain", Program.cfgCurrent.MaxRecursion, Program.cfgCurrent);
            }
        }

        /// <summary>
        ///     Given an URL, validate it
        /// </summary>
        /// <param name="url">URL to be validated</param>
        /// <returns>true if it is a valid URL</returns>
        private bool validateURL(string url)
        {
            var validUrl =
                new Regex(
                    @"^((http|https|www):\/\/)?([a-zA-Z0-9\~\!\@\#\$\%\^\&\*\(\)_\-\=\+\\\/\?\.\:\;\'\,]*)(\.)([a-zA-Z0-9\~\!\@\#\$\%\^\&\*\(\)_\-\=\+\\\/\?\.\:\;\'\,]+)");
            return validUrl.Match(url).Success;
        }
    }
}