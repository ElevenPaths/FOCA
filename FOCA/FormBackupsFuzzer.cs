using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using FOCA.Analysis.HttpMap;
using FOCA.ModifiedComponents;

namespace FOCA
{
    /// <summary>
    /// Backups Fuzzer
    /// </summary>
    public partial class FormBackupsFuzzer : Form
    {
        private List<int> validStatusCodes;

        public FormBackupsFuzzer()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     Constructor that initializes the interface and generates the possible different URLs from a given URL
        /// </summary>
        /// <param name="url"></param>
        public FormBackupsFuzzer(string url)
        {
            InitializeComponent();
            validStatusCodes = new List<int>();
            panelUrls.lstView.Columns.Add("URL").Width = panelUrls.lstView.Width;

            var mutexUrls = HttpMap.MutexFileRuntime(new ThreadSafeList<string> {url});
            if (mutexUrls.Count == 0)
                return;

            foreach (var u in mutexUrls)
            {
                panelUrls.lstView.Items.Add(u);
            }
        }

        /// <summary>
        /// Launch fuzzing tasks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = false;
            var aux = txtValidCodes.Text.Split(new[] {","}, StringSplitOptions.None).ToList();
            try
            {
                validStatusCodes = aux.ToList().ConvertAll(int.Parse);
            }
            catch
            {
                MessageBox.Show(
                    @"Invalid value at the valid status codes input box. They must be integers separated by the ',' character. Please, check it.",
                    @"Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnStart.Enabled = true;
                return;
            }
            try
            {
                foreach (ListViewItem item in panelUrls.lstView.Items)
                {
                    var result = ValidateUrlsResponse(item.Text);
                    if (!validStatusCodes.Contains(result))
                    {
                        item.BackColor = Color.IndianRed;
                        continue;
                    }
                    item.BackColor = Color.LawnGreen;
                    Program.LogThis(new Log(Log.ModuleType.Fuzzer,
                        "File found -- " + item.Text + " - status code: " + result,
                        Log.LogType.high));
                }
            }
            catch (Exception ex)
            {
                Program.LogThis(new Log(Log.ModuleType.Fuzzer, ex.Message, Log.LogType.medium));
            }
            btnStart.Enabled = true;
        }

        private int ValidateUrlsResponse(string item)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(item);
                var response = (HttpWebResponse)request.GetResponse();
                return (int)response.StatusCode;
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    return (int) ((HttpWebResponse) ex.Response).StatusCode;
                }
            }
            return -1;
        }

        /// <summary>
        ///     Requests asynchronously the given URLs
        /// </summary>
        /// <param name="urls"></param>
        /// <returns></returns>
        private async Task<int[]> RequestUrlsAsync(ICollection urls)
        {
            var results = Enumerable.Repeat(-1, urls.Count).ToArray();
            for (var i = 0; i < urls.Count; i++)
            {
                results[i] = await CheckUrl(panelUrls.lstView.Items[i].Text);
            }
            return results;
        }

        /// <summary>
        ///     Requests a given URL and returns the response's status code
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private static async Task<int> CheckUrl(string url)
        {
            int statusCode;
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var resp = await httpClient.GetAsync(url);
                    statusCode = (int) resp.StatusCode;
                }
            }
            catch
            {
                statusCode = -1;
            }
            return statusCode;
        }
    }
}