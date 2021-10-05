using com.rusanu.dataconnectiondialog;
using FOCA.Database;
using FOCA.Database.Entities;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Net;
using System.Windows.Forms;

namespace FOCA
{
    static class Program
    {
        private static bool Running;
        private const string FocaDatabaseName = "Foca";
        private const string SQLExpressConnectionString = @"Server=.\SQLEXPRESS;Initial Catalog=" + FocaDatabaseName + ";MultipleActiveResultSets=True;Integrated Security=true;Connection Timeout=3";
        public static string ProgramVersion = Application.ProductVersion;
        public static string ProgramName = Application.ProductName + " " + ProgramVersion;

        public static Data data;

        public static Configuration cfgCurrent = new Configuration();

        public static FormMain FormMainInstance;
        public static FormOptions FormOptionsInstance;

        private static FormSplashFOCA splashScreen;

        public static bool DesignMode()
        {
            return !Running;
        }

        public static void LogThis(Log log)
        {
            FormMainInstance.panelLogs.LogThis(log);
        }

        public static void ChangeStatus(string newStatus)
        {
            FormMainInstance.ChangeStatus(newStatus);
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            System.Threading.Thread splashThread = new System.Threading.Thread(new System.Threading.ThreadStart(() =>
              {
                  splashScreen = new FormSplashFOCA(ProgramVersion);
                  Application.Run(splashScreen);
                  Application.ExitThread();
              }));

            splashThread.SetApartmentState(System.Threading.ApartmentState.STA);
            splashThread.Start();

            Application.DoEvents();
            //Load the FOCA
            Running = true;


            SqlConnectionStringBuilder connectionStringBuilder = null;
            bool csUpdated = false;

            try
            {
                ConnectionStringSettings csFromConfig = ConfigurationManager.ConnectionStrings[nameof(FocaContextDb)];
                //If there is no connection string configured, try with SQLEXPRESS instance
                if (csFromConfig == null || String.IsNullOrEmpty(csFromConfig.ConnectionString))
                {
                    connectionStringBuilder = new SqlConnectionStringBuilder(SQLExpressConnectionString);
                    csUpdated = true;
                }
                else
                {
                    connectionStringBuilder = new SqlConnectionStringBuilder(csFromConfig.ConnectionString);
                    if (String.IsNullOrWhiteSpace(connectionStringBuilder.InitialCatalog))
                    {
                        connectionStringBuilder.InitialCatalog = FocaDatabaseName;
                    }
                }
            }
            catch (ArgumentException)
            {
                csUpdated = true;
                connectionStringBuilder = new SqlConnectionStringBuilder(SQLExpressConnectionString);
            }

            while (!FocaContextDb.IsDatabaseAvailable(connectionStringBuilder.ToString()))
            {
                splashScreen.Invoke((MethodInvoker)(() => MessageBox.Show(splashScreen, "FOCA needs a SQL database. Please setup your connection and try again.", "Database not found", MessageBoxButtons.OK, MessageBoxIcon.Error)));

                // Display the connection dialog
                using (DataConnectionDialog dlg = new DataConnectionDialog(connectionStringBuilder))
                {
                    DialogResult connectionResult = DialogResult.Cancel;
                    splashScreen.Invoke((MethodInvoker)(() => connectionResult = dlg.ShowDialog(splashScreen)));

                    if (DialogResult.OK != connectionResult)
                    {
                        Environment.Exit(0);
                    }
                }
                csUpdated = true;
            }

            if (csUpdated)
            {
                UpdateConnectionString(connectionStringBuilder.ToString());
            }

            data = new Data();
            FormMainInstance = new FormMain();
            FormMainInstance.TopMost = true;
            FormMainInstance.Shown += FormMainInstance_Shown;

            FormOptionsInstance = new FormOptions();

            InitializeServicePointManager();

            Application.Run(FormMainInstance);
        }

        private static void FormMainInstance_Shown(object sender, EventArgs e)
        {
            splashScreen?.Invoke((Action)delegate
            {
                splashScreen.Close();
                splashScreen.Dispose();
            });
            FormMainInstance.Activate();
            FormMainInstance.TopMost = false;
        }

        private static void UpdateConnectionString(string connectionString)
        {
            try
            {
                System.Configuration.Configuration configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                configFile.ConnectionStrings.ConnectionStrings.Clear();
                configFile.ConnectionStrings.ConnectionStrings.Add(new ConnectionStringSettings(nameof(FocaContextDb), connectionString, "System.Data.SQLCLient"));
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.ConnectionStrings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {
                MessageBox.Show("The database connection string could not be saved to the configuration file.", "Unsaved connection string", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private static void InitializeServicePointManager()
        {
            ServicePointManager.ServerCertificateValidationCallback += (s, c, ch, ssl) => true;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
        }
    }
}
