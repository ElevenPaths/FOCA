using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using System.ServiceProcess;
using System.Threading;

namespace FOCA
{
    static class Program
    {
        private static bool Running;
        public static string ProgramVersion = Application.ProductVersion.Remove(3);
        public static string ProgramName = Application.ProductName + " " + ProgramVersion;

        public static Data data;

        public static Configuration cfgCurrent = new Configuration();

        public static FormMain FormMainInstance;
        public static FormOptions FormOptionsInstance;
        private readonly static string strPathErrorLog = Path.GetDirectoryName(Application.ExecutablePath) + "\\errorlog.txt";

        public static bool DesingMode()
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

        private static void InitializeErrorLog()
        {
            File.Delete(strPathErrorLog);
        }

        public static void WriteErrorInLog(string strMessage)
        {
            try
            {
                using (var sw = new StreamWriter(strPathErrorLog, true))
                {
                    sw.WriteLine("Exception: " + strMessage);
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Validate Service Sql.
        /// </summary>
        /// <returns></returns>
        public static bool IsSQLServerRunning()
        {
            try
            {
                var process = Process.GetProcessesByName("sqlservr");
                if (process.Length == 0) return false;

                //Validate Sql Express
                var serviceController = new ServiceController("MSSQL$SQLEXPRESS", Environment.MachineName);

                if (serviceController.DisplayName != null)
                    return serviceController.Status != ServiceControllerStatus.Stopped;

                //Validate Sql Server
                serviceController = new ServiceController("MSSQLServer", Environment.MachineName);

                return serviceController.Status != ServiceControllerStatus.Stopped;
            }
            catch (InvalidOperationException e)
            {
                //Throwed if you are not running an MSSQL EXPRESS instance on the local machine
                return false;
            }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Process.GetCurrentProcess().PriorityBoostEnabled = true;
            Application.EnableVisualStyles();

            var fsf = new FormSplashFOCA("Open Source");
            var t = new Thread(new ThreadStart(delegate
            {
                Application.Run(fsf);
            }));
            t.Start();

            //Load the FOCA
            Running = true;

            if (!IsSQLServerRunning())
            {
                var f = MessageBox.Show(
                    "A SQL server must be installed and running. We recommend you to use SQL Server Express Edition",
                    "Missing SQL Server", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (f == DialogResult.OK)
                {
                    try
                    {
                        Process.Start("https://www.microsoft.com/en-us/sql-server/sql-server-editions-express");
                    }
                    catch (Exception)
                    {
                    }
                    Environment.Exit(0);
                }
            }

            data = new Data();
            InitializeErrorLog();
            FormMainInstance = new FormMain();
            FormOptionsInstance = new FormOptions();

            var canStart = false;
            do
            {
                try
                {
                    fsf.Invoke(new MethodInvoker(delegate
                    {
                        fsf.Close();
                        canStart = true;
                    }));
                }
                catch
                {
                    Thread.Sleep(900);
                    canStart = false;
                }
            } while (canStart == false);

            Application.Run(FormMainInstance);
        }
    }
}
