using System;
using System.IO;
using MetadataExtractCore.Utilities;
using MetadataExtractCore.Diagrams;

namespace MetadataExtractCore.Metadata
{
    [Serializable]
    public class RDPDocument : MetaExtractor
    {
        public RDPDocument() { }

        public RDPDocument(Stream stm)
        {
            this.stm = new MemoryStream();
            Functions.CopyStream(stm, this.stm);
        }

        public override void analyzeFile()
        {
            try
            {
                using (var sr = new StreamReader(this.stm))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        var parametro = string.Empty;
                        var tipo = string.Empty;
                        var valor = string.Empty;

                        try
                        {
                            parametro = line.Split(new char[] {':'})[0];
                            tipo = line.Split(new char[] {':'})[1];
                            int entryPoint = parametro.Length + 1 + tipo.Length + 1;
                            valor = line.Substring(entryPoint, line.Length - entryPoint);
                        }
                        catch
                        {
                            return;
                        }

                        if (string.IsNullOrEmpty(valor))
                            continue;

                        switch (parametro.ToLower())
                        {
                            case "shell working directory":
                            case "remoteapplicationprogram":
                            case "remoteapplicationname":
                            case "remoteapplicationcmdline":
                                FoundPaths.AddUniqueItem(valor, true);
                                break;
                            case "full address":
                                FoundServers.AddUniqueItem(new ServersItem(valor, "RDP file Analysis"));
                                break;
                            case "gatewayhostname":
                                FoundServers.AddUniqueItem(new ServersItem(valor.Split(new char[] {':'})[0],
                                    "RDP file Analysis"));
                                break;
                            case "alternate shell":
                                FoundPaths.AddUniqueItem(valor, true);
                                var softName = Analysis.ApplicationAnalysis.GetApplicationsFromString(valor);
                                FoundMetaData.Applications.AddUniqueItem(!string.IsNullOrEmpty(softName)
                                    ? new ApplicationsItem(softName)
                                    : new ApplicationsItem(valor));

                                break;
                            case "username":
                                FoundUsers.AddUniqueItem(valor, true);
                                break;
                            case "domain":
                                break;
                            case "password":
                                FoundPasswords.AddUniqueItem(new PasswordsItem(valor, "RDP Password"));
                                break;
                            case "password 51":
                                FoundPasswords.AddUniqueItem(new PasswordsItem(valor, "RDP Password (Type 51)"));
                                break;;
                        }
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
