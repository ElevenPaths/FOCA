using MetadataExtractCore.Analysis;
using MetadataExtractCore.Diagrams;
using System;
using System.IO;


namespace MetadataExtractCore.Extractors
{
    public class ICADocument : DocumentExtractor
    {
        /// <summary>
        /// Ctor for class
        /// </summary>
        /// <param name="stm"></param>
        public ICADocument(Stream stm) : base(stm)
        {
        }

        public override FileMetadata AnalyzeFile()
        {
            try
            {
                this.foundMetadata = new FileMetadata();
                using (StreamReader sr = new StreamReader(this.fileStream))
                {
                    string line = string.Empty;

                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] separatedValues = line.Split(new char[] { '=' });
                        if (separatedValues.Length < 2)
                            continue;

                        string key = separatedValues[0].ToLower();
                        string value = line.Remove(0, key.Length + 1);

                        if (String.IsNullOrWhiteSpace(value))
                            continue;

                        if (key.StartsWith("address") ||
                            key.StartsWith("httpbrowseraddress") ||
                            key.StartsWith("tcpbrowseraddress") ||
                            key.StartsWith("sslproxyhost"))
                        {
                            string ipOrHost = value.Split(new char[] { ':' })[0];
                            if (ipOrHost != "*")
                            {
                                this.foundMetadata.Add(new Server(ipOrHost, "ICA file Analysis"));
                            }
                        }
                        else if (key.StartsWith("username"))
                        {
                            this.foundMetadata.Add(new User(value, true));
                        }
                        else if (key.StartsWith("clearpassword") ||
                                 key.StartsWith("password"))
                        {
                            this.foundMetadata.Add(new Password(value, "ICA Clear password"));
                        }
                        else if (key.StartsWith("persistentcachepath") ||
                                 key.StartsWith("workdirectory") ||
                                 key.StartsWith("initialprogram") ||
                                 key.StartsWith("iconpath"))
                        {
                            this.foundMetadata.Add(new Diagrams.Path(value, true));

                            string user = PathAnalysis.ExtractUserFromPath(value);
                            if (user != string.Empty)
                                this.foundMetadata.Add(new User(user, true));

                            string softName = ApplicationAnalysis.GetApplicationsFromString(value);
                            if (!string.IsNullOrEmpty(value))
                                this.foundMetadata.Add(new Application(softName));
                            else
                                this.foundMetadata.Add(new Application(value));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }
            return this.foundMetadata;
        }
    }
}
