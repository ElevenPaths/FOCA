using MetadataExtractCore.Diagrams;
using System;
using System.IO;

namespace MetadataExtractCore.Extractors
{
    public class RDPDocument : DocumentExtractor
    {
        public RDPDocument(Stream stm) : base(stm)
        {
        }

        public override FileMetadata AnalyzeFile()
        {
            try
            {
                this.foundMetadata = new FileMetadata();
                using (var sr = new StreamReader(this.fileStream))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        var parametro = string.Empty;
                        var tipo = string.Empty;
                        var valor = string.Empty;

                        try
                        {
                            parametro = line.Split(new char[] { ':' })[0];
                            tipo = line.Split(new char[] { ':' })[1];
                            int entryPoint = parametro.Length + 1 + tipo.Length + 1;
                            valor = line.Substring(entryPoint, line.Length - entryPoint);
                        }
                        catch
                        {
                            return this.foundMetadata;
                        }

                        if (string.IsNullOrEmpty(valor))
                            continue;

                        switch (parametro.ToLower())
                        {
                            case "shell working directory":
                            case "remoteapplicationprogram":
                            case "remoteapplicationname":
                            case "remoteapplicationcmdline":
                                this.foundMetadata.Add(new Diagrams.Path(valor, true));
                                break;
                            case "full address":
                                this.foundMetadata.Add(new Server(valor, "RDP file Analysis"));
                                break;
                            case "gatewayhostname":
                                this.foundMetadata.Add(new Server(valor.Split(new char[] { ':' })[0],
                                    "RDP file Analysis"));
                                break;
                            case "alternate shell":
                                this.foundMetadata.Add(new Diagrams.Path(valor, true));
                                var softName = Analysis.ApplicationAnalysis.GetApplicationsFromString(valor);
                                this.foundMetadata.Add(!string.IsNullOrEmpty(softName)
                                    ? new Application(softName)
                                    : new Application(valor));

                                break;
                            case "username":
                                this.foundMetadata.Add(new User(valor, true));
                                break;
                            case "domain":
                                break;
                            case "password":
                                this.foundMetadata.Add(new Password(valor, "RDP Password"));
                                break;
                            case "password 51":
                                this.foundMetadata.Add(new Password(valor, "RDP Password (Type 51)"));
                                break; ;
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
