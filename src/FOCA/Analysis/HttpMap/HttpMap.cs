using FOCA.Analysis.FingerPrinting;
using FOCA.Analysis.Technology;
using FOCA.Database.Entities;
using FOCA.ModifiedComponents;
using FOCA.TaskManager;
using MetadataExtractCore.Diagrams;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;

namespace FOCA.Analysis.HttpMap
{
    /// <summary>
    ///     Generate a MAP of Folders/Files and possible backup/old versions of Files
    /// </summary>
    [Serializable]
    public class HttpMap : IDisposable
    {
        public enum SearchStatus
        {
            NotInitialized = 0,
            Searching = 1,
            Finished = 2,
            Pasive = 3
        };

        [JsonIgnore]
        public int Id { get; set; }

        /// <summary>
        ///     Modified filenames for backup files search actions
        /// </summary>
        [JsonIgnore]
        public ThreadSafeList<string> BackupModifiedFilenames { get; set; }

        /// <summary>
        ///     Documents found
        /// </summary>
        public virtual ThreadSafeList<string> Documents { get; set; }

        /// <summary>
        ///     Original Files found
        /// </summary>
        public virtual ThreadSafeList<string> Files { get; set; }

        /// <summary>
        ///     List of extracted directories
        /// </summary>
        public ThreadSafeList<string> Folders { get; set; }

        /// <summary>
        ///     List of endpoints where parameters were found
        /// </summary>
        public ThreadSafeList<string> Parametrized { get; set; }

        /// <summary>
        ///     All links search status
        /// </summary>
        public SearchStatus SearchingAllLinks;

        /// <summary>
        ///     Insecure methods search status
        /// </summary>
        public SearchStatus SearchingMethods;

        /// <summary>
        ///     Mutex fuzz search status
        /// </summary>
        public SearchStatus SearchingMutexFuzz;

        /// <summary>
        ///     Open directories search status
        /// </summary>
        public SearchStatus SearchingOpenFolders;

        /// <summary>
        ///     Technology search status
        /// </summary>
        public SearchStatus SearchingTechnology;

        /// <summary>
        ///     Default constructor
        /// </summary>
        public HttpMap()
        {
            Documents = new ThreadSafeList<string>();
            Files = new ThreadSafeList<string>();
            BackupModifiedFilenames = new ThreadSafeList<string>();
            Folders = new ThreadSafeList<string>();
            Parametrized = new ThreadSafeList<string>();

            SearchingAllLinks = SearchStatus.NotInitialized;
            SearchingMethods = SearchStatus.NotInitialized;
            SearchingOpenFolders = SearchStatus.NotInitialized;
            SearchingTechnology = SearchStatus.NotInitialized;
            SearchingMutexFuzz = SearchStatus.NotInitialized;
        }

        /// <summary>
        ///     Check for the contents of the robots.txt file of a given host at a given port
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        public static void CheckRobots(string host, int port)
        {
            try
            {
                var domain = Program.data.GetDomain(host);
                if (domain == null)
                    return;
                if (domain.robotsAnalyzed)
                    return;
                domain.robotsAnalyzed = true;

                var protocol = ((port == 443) ? "https://" : "http://");

                string robotspath = protocol + host + ":" + port + "/robots.txt";
                string sRobots = String.Empty;

                try
                {
                    HttpWebRequest robotsRequest = HttpWebRequest.CreateHttp(robotspath);
                    robotsRequest.AllowAutoRedirect = false;
                    using (HttpWebResponse response = (HttpWebResponse)robotsRequest.GetResponse())
                    {
                        if (response.StatusCode != HttpStatusCode.OK)
                            return;

                        using (var reader = new StreamReader(response.GetResponseStream()))
                        {
                            sRobots = reader.ReadToEnd();
                        }
                    }
                }
                catch (Exception)
                {
                }

                using (var sr = new StringReader(sRobots))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.Split(' ').Length <= 1)
                            continue;
                        var path = line.Split(' ')[1];
                        if (path.Contains("*"))
                            continue;
                        path = path.Replace("$", "");

                        if (!path.StartsWith("/"))
                            continue;
                        var url = protocol + host + path;
                        Program.LogThis(new Log(Log.ModuleType.Fuzzer,
                            "[robots.txt] File found on " + robotspath + ": " + url,
                            Log.LogType.medium));
                        domain.map.AddUrl(url);
                    }
                }
            }
            catch (Exception)
            {
                Program.LogThis(new Log(Log.ModuleType.WebSearch, "Error while checking for robots.txt file for " + host,
                    Log.LogType.low));
            }
        }

        /// <summary>
        ///     Add Folders, file and mutex Files from a URL
        /// </summary>
        /// <param name="url"></param>
        public void AddUrl(string url)
        {
            if (ExistFile(url))
                return;
            if (url.Contains('?'))
            {
                if (!Parametrized.Any(s => s.Contains(url.Split('?')[0])))
                {
                    Parametrized.Add(url);
                }
            }
            AddCleanUrl(url);
            ExtractFolders(url);
            ExtractApacheUser(url);

            try
            {
                var uri = new Uri(url);
                var domain = Program.data.GetDomain(uri.Host);

                if (domain != null)
                {
                    if (domain.techAnalysis == null)
                        domain.techAnalysis = new TechnologyAnalysis();

                    var extension = uri.AbsolutePath.Split('.').Last();
                    foreach (var tech in from tech in domain.techAnalysis.SelectedTechnologies
                                         where tech.extension == extension
                                         let exists = tech.GetURLs().Any(urlOfTech => uri.ToString() == urlOfTech)
                                         where !exists
                                         select tech)
                    {
                        tech.AddURL(uri.ToString());
                    }
                }
            }
            catch
            {
            }
            // notify that a new URL was added
#if PLUGINS
            var tPluginOnNewUrl = new Thread(Program.data.plugins.OnNewURL) { IsBackground = true };
            object[] oUrl = { new object[] { url } };
            tPluginOnNewUrl.Start(oUrl);
#endif
        }

        /// <summary>
        ///     Remove parameters and DOM elements from a given URL
        /// </summary>
        /// <param name="url"></param>
        private void AddCleanUrl(string url)
        {
            var uri = new Uri(url);
            Files.Add(uri.GetLeftPart(UriPartial.Path));
        }

        /// <summary>
        ///     Add a document to the project's map
        /// </summary>
        /// <param name="urlDocument"></param>
        public void AddDocument(string urlDocument)
        {
            if (ExistDocument(urlDocument)) return;
            try
            {
                var uri = new Uri(urlDocument);

                Documents.Add(urlDocument);
                ExtractFolders(urlDocument);
                ExtractApacheUser(urlDocument);
                if ((Program.cfgCurrent.FingerPrintingAllHttp) || (Program.cfgCurrent.PassiveFingerPrintingHttp))
                {
                    if (urlDocument.ToLower().Contains(".jsp") || urlDocument.ToLower().Contains("jsp") ||
                        urlDocument.ToLower().Contains("jserv") ||
                        urlDocument.ToLower().Contains("servlet") ||
                        urlDocument.ToLower().Contains("srv") ||
                        urlDocument.ToLower().Contains("jsf") ||
                        urlDocument.ToLower().Contains("app") ||
                        urlDocument.ToLower().Contains("do") ||
                        urlDocument.ToLower().Contains("server"))
                    {
                        FingerPrintJspFiles(urlDocument);
                    }
                    if (urlDocument.ToLower().Contains(".tpl"))
                        FingerPrintTplFiles(urlDocument);
                }
                var domain = Program.data.GetDomain(uri.Host);
                if (domain == null) return;
                var computersDomains = Program.data.GetComputerDomainsFromDomainsItem(domain);

                foreach (var cdi in computersDomains.Items)
                {
                    if (urlDocument.Contains(".xsp"))
                        cdi.Computer.Software.AddUniqueItem(new ApplicationsItem("Cocoon", ""));

                    if (urlDocument.Contains(".nsf"))
                        cdi.Computer.Software.AddUniqueItem(new ApplicationsItem("Lotus Notes", ""));

                    if (urlDocument.Contains(".lasso"))
                        cdi.Computer.Software.AddUniqueItem(new ApplicationsItem("Lasso Webserver", ""));

                    if (urlDocument.Contains(".tpl") || urlDocument.Contains(".dna"))
                        cdi.Computer.Software.AddUniqueItem(new ApplicationsItem("WebDNA", ""));
                }
            }
            catch
            {
            }
        }

        /// <summary>
        ///     Fingerprinting actions for JSP files
        /// </summary>
        /// <param name="url"></param>
        private static void FingerPrintJspFiles(string url)
        {
            var u = new Uri(url);
            var ssl = url.StartsWith("https://");
            var port = (ssl) ? 443 : 80;

            var domain = Program.data.GetDomain(u.Host);
            if (domain == null)
                return;
            if (domain.jspFingerprintingAnalyzed) return;
            var fprintingHost = new HTTP(domain.Domain, "/" + u.AbsolutePath, port, ssl);

            fprintingHost.FingerPrintingFinished += FingerPrintingEventHandler.AsociateFingerPrinting;
            fprintingHost.FingerPrintingError += FingerPrintingEventHandler.fprinting_FingerPrintingError;

            var tHost = new Thread(fprintingHost.AnalyzeErrorsJsp) { IsBackground = true };
            Program.data.tasker.AddTask(new TaskFOCA(tHost, null,
                "Fingerprinting JSP HTTP (" + domain.Domain + ":" + port + "/" + u.AbsolutePath + ")"));
            domain.jspFingerprintingAnalyzed = true;
            Program.data.tasker.AddTask(new FOCA.TaskManager.TaskFOCA(tHost, null, "Fingerprinting JSP HTTP (" + domain.Domain + ":" + port + "/" + u.AbsolutePath + ")"));
        }

        /// <summary>
        ///     Fingerprinting actions for TPL files
        /// </summary>
        /// <param name="url"></param>
        private static void FingerPrintTplFiles(string url)
        {
            var u = new Uri(url);
            var ssl = url.StartsWith("https://");
            var port = (ssl) ? 443 : 80;

            var domain = Program.data.GetDomain(u.Host);
            if (domain == null)
                return;
            if (domain.tplFingerprintingAnalyzed) return;

            var fprintingHost = new HTTP(domain.Domain, "/" + u.AbsolutePath, port, ssl);

            fprintingHost.FingerPrintingFinished += FingerPrintingEventHandler.AsociateFingerPrinting;
            fprintingHost.FingerPrintingError += FingerPrintingEventHandler.fprinting_FingerPrintingError;
            var tHost = new Thread(fprintingHost.AnalyzeErrorsTpl) { IsBackground = true };
            Program.data.tasker.AddTask(new TaskFOCA(tHost, null,
                "Fingerprinting TPL HTTP (" + domain.Domain + ":" + port + "/" + u.AbsolutePath + ")"));
            domain.tplFingerprintingAnalyzed = true;
        }

        /// <summary>
        ///     Extract an username from an user's path of Apache and add it to the computer's users list
        /// </summary>
        /// <param name="path"></param>
        private static void ExtractApacheUser(string path)
        {
            var c = Regex.Match(path, @"\~[^/]*", RegexOptions.IgnoreCase);
            var uri = new Uri(path);

            if (!c.Success) return;
            var user = c.Value.Substring(1, c.Value.Length - 1);

            foreach (var t in Program.data.computerDomains.Items)
            {
                var domItem = t.Domain;
                if (domItem.Domain != uri.Host) continue;
                var userItem = new UserItem
                {
                    Name = user,
                    Notes = path
                };
                var exists = t.Computer.Users.Items.Any(u => u.Name == user);
                if (!exists)
                    t.Computer.Users.Items.Add(userItem);
            }
        }

        /// <summary>
        ///     Returns a URLs list for the backups search action
        /// </summary>
        /// <param name="urls"></param>
        /// <returns></returns>
        public static ThreadSafeList<string> MutexFileRuntime(ThreadSafeList<string> urls)
        {
            var allUrls = new ThreadSafeList<string>();

            try
            {
                var mutexfolders = _ExtractFoldersRuntime(urls);
                foreach (var url in urls)
                {
                    if (!String.IsNullOrWhiteSpace(url))
                    {
                        var uri = new Uri(url);
                        var file = System.IO.Path.GetFileName(uri.LocalPath);
                        var fileName = System.IO.Path.GetFileNameWithoutExtension(file);
                        var fileExtension = System.IO.Path.GetExtension(file);
                        var path = uri.AbsoluteUri;

                        if (uri.AbsoluteUri.IndexOf(file, StringComparison.Ordinal) > 0)
                            path = path.Remove(uri.AbsoluteUri.IndexOf(file, StringComparison.Ordinal));

                        var mutex = _MutexFileRuntime(path, HttpUtility.UrlEncode(fileName),
                            HttpUtility.UrlEncode(fileExtension));

                        if (mutex == null)
                            return allUrls;

                        foreach (var mutexUrl in mutex)
                        {
                            allUrls.Add(mutexUrl);
                        }
                    }
                }

                foreach (var mutexFolder in mutexfolders)
                {
                    allUrls.Add(mutexFolder);
                }
            }
            catch (Exception)
            {

            }

            return allUrls;
        }

        /// <summary>
        ///     Generate possible backup file names
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <param name="fileExtension"></param>
        /// <returns></returns>
        private static ThreadSafeList<string> _MutexFileRuntime(string path, string fileName, string fileExtension)
        {
            var lstFiles = new ThreadSafeList<string>();

            // ToDo use a list provided by the user
            string[] backupExt = { "old", "bak", "back", "1", "2", "txt", "~", "save", "backup" };
            string[] oldVersions = { "1", "2", "_1", "_2", "_backup" };

            fileExtension = fileExtension.Trim('.');

            foreach (var newFile in (from newExt in backupExt.Concat(oldVersions)
                                     let aux = path.Split(new[] { "/" }, StringSplitOptions.None)
                                     let realPath = string.Join("/", aux.Take(aux.Length - 1)) + "/"
                                     select (string.Equals(newExt, "~"))
                                         ? realPath + fileName + "." + fileExtension + newExt
                                         : realPath + fileName + "." + fileExtension + "." + newExt).Where(
                        newFile => !lstFiles.Contains(newFile)))
            {
                lstFiles.Add(newFile);
            }

            return lstFiles;
        }

        /// <summary>
        ///     Extract existing folders from a given list of URLs
        /// </summary>
        /// <param name="lsturls"></param>
        /// <returns></returns>
        private static ThreadSafeList<string> _ExtractFoldersRuntime(IEnumerable<string> lsturls)
        {
            var folders = new ThreadSafeList<string>();
            var foldersBackups = new ThreadSafeList<string>();

            try
            {
                foreach (var url in lsturls)
                {
                    if (!String.IsNullOrWhiteSpace(url))
                    {

                        var u = new Uri(url);
                        if (u.ToString().EndsWith("//"))
                        {
                            var auxSingleUrl = new ThreadSafeList<string> { u.ToString().Remove(u.ToString().Length - 1, 1) };
                            return _ExtractFoldersRuntime(auxSingleUrl);
                        }

                        var offSetProtocol = url.IndexOf("://", StringComparison.Ordinal);
                        var protocol = url.Substring(0, offSetProtocol);

                        var foldersSplit = u.AbsolutePath.Split('/');
                        var path = string.Empty;

                        for (var i = 0; i < foldersSplit.Length; i++)
                        {
                            if (i + 1 != foldersSplit.Length)
                                path += foldersSplit[i] + "/";
                            if (folders.Contains(protocol + "://" + u.Host + path) || path.Contains("."))
                                continue;
                            folders.Add(protocol + "://" + u.Host + path);

                            // ToDo use a list provided by the user
                            string[] compressExt = { ".zip", ".rar", ".tar", ".gz", ".tar.gz" };
                            var path1 = path;
                            foreach (
                                var extension in
                                    compressExt.Where(
                                        extension =>
                                            protocol + "://" + u.Host + path1.Substring(0, path1.Length - 1) !=
                                            protocol + "://" + u.Host)
                                        .Where(
                                            extension =>
                                                !foldersBackups.Contains(protocol + "://" + u.Host +
                                                                         path1.Substring(0, path1.Length - 1) +
                                                                         extension)))
                            {
                                foldersBackups.Add(protocol + "://" + u.Host + path.Substring(0, path.Length - 1) +
                                                   extension);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

            }

            return foldersBackups;
        }

        /// <summary>
        /// Extract folders from a given URL
        /// </summary>
        /// <param name="url"></param>
        private void ExtractFolders(string url)
        {
            var u = new Uri(url);
            var port = (u.Port == 80) ? "" : ":" + u.Port;

            if (u.ToString().EndsWith("//"))
            {
                ExtractFolders(u.ToString().Remove(u.ToString().Length - 1, 1));
                return;
            }

            var offSetProtocol = url.IndexOf("://", StringComparison.Ordinal);
            var protocol = url.Substring(0, offSetProtocol);

            var foldersSplit = u.AbsolutePath.Split('/');
            var path = "";
            for (var i = 0; i < foldersSplit.Length; i++)
            {
                if (i + 1 != foldersSplit.Length)
                    path += foldersSplit[i] + "/";

                if (ExistFolder(protocol + "://" + u.Host + port + path) || path.Contains("."))
                    continue;
                Folders.Add(protocol + "://" + u.Host + port + path);

                // ToDo use a list provided by the user
                string[] ficheros = { ".listing", ".DS_Store", "WS_FTP.LOG" };
                var path1 = path;
                foreach (
                    var fichero in
                        ficheros.Where(fichero => !ExistMutexFile(protocol + "://" + u.Host + port + path1 + fichero)))
                {
                    BackupModifiedFilenames.Add(protocol + "://" + u.Host + port + path + fichero);
                }

                // ToDo use a list provided by the user
                string[] compressExt = { ".zip", ".rar", ".tar", ".gz", ".tar.gz" };
                foreach (
                    var extension in
                        compressExt.Where(
                            extension =>
                                protocol + "://" + u.Host + path1.Substring(0, path1.Length - 1) !=
                                protocol + "://" + u.Host)
                            .Where(
                                extension =>
                                    !ExistMutexFile(protocol + "://" + u.Host + port +
                                                    path1.Substring(0, path1.Length - 1) +
                                                    extension)))
                {
                    BackupModifiedFilenames.Add(protocol + "://" + u.Host + port +
                                                path.Substring(0, path.Length - 1) + extension);
                }
            }
        }

        /// <summary>
        /// Check if a file already exists in the project
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private bool ExistFile(string url)
        {
            return Files.Contains(url);
        }

        /// <summary>
        /// Check if a backup filename is already generated
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private bool ExistMutexFile(string url)
        {
            return BackupModifiedFilenames.Contains(url);
        }

        /// <summary>
        /// Check if a document was already added to the project
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private bool ExistDocument(string url)
        {
            return Documents.Contains(url);
        }

        /// <summary>
        /// Check if a folder was already added to the project
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private bool ExistFolder(string url)
        {
            return Folders.Contains(url);
        }

        #region IDisposable Support

        private bool _disposedValue; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    Files.Dispose();
                    BackupModifiedFilenames.Dispose();
                    Documents.Dispose();
                    Folders.Dispose();
                    Parametrized.Dispose();
                }

                Files = null;
                BackupModifiedFilenames = null;
                Documents = null;
                Folders = null;
                Parametrized = null;

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}
