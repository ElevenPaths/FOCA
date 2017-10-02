using System;
using System.IO;
using MetadataExtractCore.Analysis;
using MetadataExtractCore.Utilities;
using MetadataExtractCore.Diagrams;


namespace MetadataExtractCore.Metadata
{
    [Serializable]
    public class ICADocument : MetaExtractor
    {
        /// <summary>
        /// Propertie Serialize
        /// </summary>
        public ICADocument() { }

        /// <summary>
        /// Ctor for class
        /// </summary>
        /// <param name="stm"></param>
        public ICADocument(Stream stm)
        {
            this.stm = new MemoryStream();
            Functions.CopyStream(stm, this.stm);
        }

        public override void analyzeFile()
        {
            try
            {
                StreamReader sr = new StreamReader(this.stm);
                string line = string.Empty;

                while ((line = sr.ReadLine()) != null)
                {
                    string parametro = string.Empty;
                    string valor = string.Empty;

                    try
                    {
                        parametro = line.Split(new char[] { '=' })[0];

                        int entryPoint = parametro.Length + 1;
                        valor = line.Substring(entryPoint, line.Length - entryPoint);
                    }
                    catch
                    {
                        continue;
                    }

                    if (string.IsNullOrEmpty(valor))
                        continue;

                    if (parametro.ToString().ToLower().StartsWith("Address".ToLower()))
                    {
                        string ipOrHost = valor.Split(new char[] { ':' })[0];
                        FoundServers.AddUniqueItem(new ServersItem(ipOrHost, "ICA file Analysis"));
                    }
                    else if (parametro.ToString().ToLower().StartsWith("HttpBrowserAddress".ToLower()))
                    {
                        string ipOrHost = valor.Split(new char[] { ':' })[0];
                        FoundServers.AddUniqueItem(new ServersItem(ipOrHost, "ICA file Analysis"));
                    }
                    else if (parametro.ToString().ToLower().StartsWith("TcpBrowserAddress".ToLower()))
                    {
                        string ipOrHost = valor.Split(new char[] { ':' })[0];
                        FoundServers.AddUniqueItem(new ServersItem(ipOrHost, "ICA file Analysis"));
                    }
                    else if (parametro.ToString().ToLower().StartsWith("Username".ToLower()))
                    {
                        FoundUsers.AddUniqueItem(valor, true);
                    }
                    else if (parametro.ToString().ToLower().StartsWith("ClearPassword".ToLower()))
                    {
                        FoundPasswords.AddUniqueItem(new PasswordsItem(valor, "ICA Clear password"));
                    }
                    else if (parametro.ToString().ToLower().StartsWith("Password".ToLower()))
                    {
                        FoundPasswords.AddUniqueItem(new PasswordsItem(valor, "ICA password"));
                    }
                    else if ( (parametro.ToString().ToLower().StartsWith("PersistentCachePath".ToLower())) ||
                              (parametro.ToString().ToLower().StartsWith("WorkDirectory".ToLower()))       ||
                              (parametro.ToString().ToLower().StartsWith("InitialProgram".ToLower()))
                            )
                    {
                        FoundPaths.AddUniqueItem(valor, true);

                        string user = PathAnalysis.ExtractUserFromPath(valor);
                        if (user != string.Empty)
                            FoundUsers.AddUniqueItem(user, true);

                        string softName = Analysis.ApplicationAnalysis.GetApplicationsFromString(valor);
                        if (!string.IsNullOrEmpty(valor))
                            FoundMetaData.Applications.AddUniqueItem(new ApplicationsItem(softName));
                        else
                            FoundMetaData.Applications.AddUniqueItem(new ApplicationsItem(valor));
                    }
                    else if (parametro.ToString().ToLower().StartsWith("IconPath".ToLower()))
                    {
                        FoundPaths.AddUniqueItem(valor, true);

                        string user = PathAnalysis.ExtractUserFromPath(valor);
                        if (user != string.Empty)
                            FoundUsers.AddUniqueItem(user, true);

                        string softName = Analysis.ApplicationAnalysis.GetApplicationsFromString(valor);
                        if (!string.IsNullOrEmpty(valor))
                            FoundMetaData.Applications.AddUniqueItem(new ApplicationsItem(softName));
                        else
                            FoundMetaData.Applications.AddUniqueItem(new ApplicationsItem(valor));
                    }
                    else if (parametro.ToString().ToLower().StartsWith("SSLProxyHost".ToLower()))
                    {
                        string ipOrHost = valor.Split(new char[] { ':' })[0];
                        if (ipOrHost != "*")
                            FoundServers.AddUniqueItem(new ServersItem(ipOrHost, "ICA file Analysis"));
                    }

                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());

            }
        }
    }
}
