using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace FOCA
{
    public partial class FormAbout : Form
    {
        public FormAbout()
        {
            InitializeComponent();
            panel2.Anchor = AnchorStyles.Top;
            MinimumSize = new Size(Width, Height - panel2.Top - 10);
            Height -= panel2.Top - 10;
            panel2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            panel2.Top = 10;
        }

        public override sealed Size MinimumSize
        {
            get { return base.MinimumSize; }
            set { base.MinimumSize = value; }
        }

        private void FormAbout_Load(object sender, EventArgs e)
        {
            Text = $"About {Program.ProgramName}";
            labelVersion.Text = Program.ProgramName;
            labelBuild.Text = Application.ProductVersion;
        }

        private void buttonShodan_Click(object sender, EventArgs e)
        {
            OpenLink("https://www.shodan.io/", false);
        }

        private void picAbout_Click(object sender, EventArgs e)
        {
            OpenLink("https://www.elevenpaths.com", false);
        }

        /// <summary>
        ///     Open a link, it may be an HTTP or mailto address
        /// </summary>
        /// <param name="url"></param>
        /// <param name="isMailto"></param>
        private void OpenLink(string url, bool isMailto)
        {
            try
            {
                Process.Start(url);
            }
            catch
            {
                var msg = isMailto
                    ? $"No mail application found, write to this mail address: {url}"
                    : $"No browser found, visit this site: {url}";
                MessageBox.Show(msg, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void lnkContacts_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenLink("mailto:" + (sender as LinkLabel)?.Tag, true);
        }
    }
}