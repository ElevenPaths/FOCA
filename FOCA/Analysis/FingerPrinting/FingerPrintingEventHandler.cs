using FOCA.Database.Entities;
using FOCA.ModifiedComponents;
using MetadataExtractCore.Diagrams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace FOCA.Analysis.FingerPrinting
{
    public static class FingerPrintingEventHandler
    {
        public static void fprinting_FingerPrintingError(object sender, EventArgs e)
        {
            FingerPrinting fp = (FingerPrinting)sender;
            Log l = new Log(Log.ModuleType.FingingerPrinting, "Error fingerprinting " + fp.Host + ":" + fp.Port, Log.LogType.error);


            Program.LogThis(l);
        }

        #region HTTP
        static public void data_NewWebDomain(object sender, EventArgs e)
        {
            // Nuevo dominio web extraido
            if (sender is DomainsItem domain)
            {
                //Solo se hace fingerPrinting a los dominios principales y alternativos
                List<string> mainDomains = new List<string>();

                mainDomains.Add(Program.data.Project.Domain);
                mainDomains.AddRange(Program.data.Project.AlternativeDomains);
                if (!mainDomains.Any(D => domain.Domain.EndsWith(D)))
                    return;

                bool existeFP = false;
                for (int fpI = 0; fpI < domain.fingerPrinting.Count; fpI++)
                {
                    FingerPrinting fp = domain.fingerPrinting[fpI];

                    if (fp is HTTP)
                        existeFP = true;
                }
                if (existeFP) // Si ya existe un fp previo de HTTP, no se vuelve a realizar
                    return;

                // Se hace el fingerprinting por HOST/80/443 y IP/80/443

                // FP por HOST:80
                FingerPrinting fprintingHost = new HTTP(domain.Domain, "/", 80, false);
                fprintingHost.FingerPrintingFinished += new EventHandler(AsociateFingerPrinting);
                fprintingHost.FingerPrintingError += new EventHandler(fprinting_FingerPrintingError);
                Thread tHost = new Thread(new ThreadStart(fprintingHost.GetVersion));
                tHost.IsBackground = true;
                Program.data.tasker.AddTask(new FOCA.TaskManager.TaskFOCA(tHost, null, "Fingerprinting HTTP (" + domain.Domain + ":80)"));


                // FP por HOST:443 SSL
                fprintingHost = new HTTP(domain.Domain, "/", 443, true);
                fprintingHost.FingerPrintingFinished += new EventHandler(AsociateFingerPrinting);
                fprintingHost.FingerPrintingError += new EventHandler(fprinting_FingerPrintingError);
                tHost = new Thread(new ThreadStart(fprintingHost.GetVersion));
                tHost.IsBackground = true;
                Program.data.tasker.AddTask(new FOCA.TaskManager.TaskFOCA(tHost, null, "Fingerprinting HTTPS (" + domain.Domain + ":443)"));

                try
                {
                    // FP por IP:80
                    string ip = Program.data.GetResolutionIPs(domain.Domain)[0].Ip;
                    FingerPrinting fprintingIP = new HTTP(ip, "/", 80, false);

                    fprintingIP.FingerPrintingFinished += new EventHandler(AsociateFingerPrinting);
                    fprintingIP.FingerPrintingError += new EventHandler(fprinting_FingerPrintingError);
                    Thread tIP = new Thread(new ThreadStart(fprintingIP.GetVersion));
                    tIP.IsBackground = true;
                    Program.data.tasker.AddTask(new FOCA.TaskManager.TaskFOCA(tIP, null, "Fingerprinting HTTP (" + ip + ":80)"));

                    // FP por IP:443 SSL
                    ip = Program.data.GetResolutionIPs(domain.Domain)[0].Ip;
                    fprintingIP = new HTTP(ip, "/", 443, true);

                    fprintingIP.FingerPrintingFinished += new EventHandler(AsociateFingerPrinting);
                    fprintingIP.FingerPrintingError += new EventHandler(fprinting_FingerPrintingError);
                    tIP = new Thread(new ThreadStart(fprintingIP.GetVersion));
                    tIP.IsBackground = true;
                    Program.data.tasker.AddTask(new TaskManager.TaskFOCA(tIP, null, "Fingerprinting HTTPS (" + ip + ":443)"));
                }
                catch
                { }
            }
        }

        static public void data_NewWebDomain(object sender)
        {
            data_NewWebDomain(sender, null);
        }
        #endregion

        #region MX
        static public void data_NewMXDomain(object sender, EventArgs e)
        {
            // Nuevo dominio MX extraido
            if (sender is DomainsItem domain)
            {
                //Solo se hace fingerPrinting a los dominios principales y alternativos
                List<string> mainDomains = new List<string>();
                if (string.IsNullOrEmpty(Program.data.Project.Domain))
                    return;

                mainDomains.Add(Program.data.Project.Domain);
                mainDomains.AddRange(Program.data.Project.AlternativeDomains);
                if (!mainDomains.Any(D => domain.Domain.EndsWith(D)))
                    return;

                bool existeFP = false;
                for (int fpI = 0; fpI < domain.fingerPrinting.Count; fpI++)
                {
                    FingerPrinting fp = domain.fingerPrinting[fpI];

                    if (fp is SMTP)
                        existeFP = true;
                }
                if (existeFP) // Si ya existe un fp previo de SMTP, no se vuelve a realizar
                    return;

                FingerPrinting fprinting = new SMTP(domain.Domain, 25);
                fprinting.FingerPrintingFinished += new EventHandler(AsociateFingerPrinting);
                Thread t = new Thread(new ThreadStart(fprinting.GetVersion));
                t.IsBackground = true;
                Program.data.tasker.AddTask(new FOCA.TaskManager.TaskFOCA(t, null, "Fingerprinting SMTP (" + domain.Domain + ":25)"));
            }
        }

        static public void data_NewMXDomain(object sender)
        {
            data_NewMXDomain(sender, null);
        }
        #endregion MX

        #region FTP
        static public void data_NewFTPDomain(object sender, EventArgs e)
        {
            // Nuevo dominio FTP extraido
            if (sender is DomainsItem domain)
            {
                //Solo se hace fingerPrinting a los dominios principales y alternativos
                List<string> mainDomains = new List<string>();
                if (string.IsNullOrEmpty(Program.data.Project.Domain))
                    return;
                mainDomains.Add(Program.data.Project.Domain);
                mainDomains.AddRange(Program.data.Project.AlternativeDomains);
                if (!mainDomains.Any(D => domain.Domain.EndsWith(D)))
                    return;

                bool existeFP = false;
                for (int fpI = 0; fpI < domain.fingerPrinting.Count; fpI++)
                {
                    FingerPrinting fp = domain.fingerPrinting[fpI];

                    if (fp is FTP)
                        existeFP = true;
                }
                if (existeFP) // Si ya existe un fp previo de FTP, no se vuelve a realizar
                    return;

                FingerPrinting fprinting = new FTP(domain.Domain, 21);
                fprinting.FingerPrintingFinished += new EventHandler(AsociateFingerPrinting);
                Thread t = new Thread(new ThreadStart(fprinting.GetVersion));
                t.IsBackground = true;
                Program.data.tasker.AddTask(new FOCA.TaskManager.TaskFOCA(t, null, "Fingerprinting FTP (" + domain.Domain + ":21)"));
            }
        }

        static public void data_NewFTPDomain(object sender)
        {
            data_NewFTPDomain(sender, null);
        }
        #endregion FTP

        #region DNS
        static public void data_NewDNSDomain(object sender, EventArgs e)
        {
            // Nuevo dominio DNS extraido
            if (sender is DomainsItem domain)
            {
                //Solo se hace fingerPrinting a los dominios principales y alternativos
                if (string.IsNullOrEmpty(Program.data.Project.Domain))
                    return;

                bool existeFP = false;
                for (int fpI = 0; fpI < domain.fingerPrinting.Count; fpI++)
                {
                    FingerPrinting fp = domain.fingerPrinting[fpI];

                    if (fp is DNS)
                        existeFP = true;
                }
                if (existeFP) // Si ya existe un fp previo de DNS, no se vuelve a realizar
                    return;

                FingerPrinting fprinting = new DNS(domain.Domain);
                fprinting.FingerPrintingFinished += new EventHandler(AsociateFingerPrinting);
                Thread t = new Thread(new ThreadStart(fprinting.GetVersion));
                t.IsBackground = true;
                Program.data.tasker.AddTask(new FOCA.TaskManager.TaskFOCA(t, null, "Fingerprinting DNS (" + domain.Domain + ":53)"));
            }
        }

        static public void data_NewDNSDomain(object sender)
        {
            data_NewDNSDomain(sender, null);
        }
        #endregion DNS

        static public void AsociateFingerPrinting(object sender, EventArgs e)
        {
            // Añade al DomainItem del dominio la información del fingerprinting
            if (sender == null)
                return;
            DomainsItem domain = null;
            if (sender is HTTP http)
            {
                domain = Program.data.GetDomain(http.Host);
                if (domain != null)
                    domain.fingerPrinting.Add(http);
                else
                {
                    string ip = http.Host;
                    if (ip == null)
                        return;
                    ThreadSafeList<DomainsItem> dominiosAsociados = Program.data.GetResolutionDomains(ip);
                    foreach (DomainsItem dAsociado in dominiosAsociados)
                    {
                        dAsociado.fingerPrinting.Add(http);
                    }
                }
            }
            else if (sender is SMTP smtp)
            {
                domain = Program.data.GetDomain(smtp.Host);
                {
                    domain.fingerPrinting.Add(smtp);
                }
            }
            else if (sender is FTP ftp)
            {
                domain = Program.data.GetDomain(ftp.Host);
                {
                    domain.fingerPrinting.Add(ftp);
                }
            }
            else if (sender is DNS dns)
            {
                domain = Program.data.GetDomain(dns.Host);
                {
                    domain.fingerPrinting.Add(dns);
                }
            }

            // Actualiza la información de los servidores existentes
            ActualizaSOServidores(domain);
        }

        private static void ActualizaSOServidores(DomainsItem di)
        {
            if (di == null)
                return;
            int i = 0;
            ThreadSafeList<ComputerDomainsItem> lstRes = new ThreadSafeList<ComputerDomainsItem>(Program.data.computerDomains.Items.Where(C => C.Domain.Domain == di.Domain));
            //Se buscan los equipos que están relacionados con este dominio
            foreach (ComputerDomainsItem cdi in lstRes)
            {
                i++;
                {
                    //Se usa el fingerPrint del dominio para asignar un SO al servidor
                    for (int fpI = 0; fpI < di.fingerPrinting.Count; fpI++)
                    {
                        Analysis.FingerPrinting.FingerPrinting fp = di.fingerPrinting[fpI];
                        //Nos fiamos mas del so obtenido mediante fingerprinting que el que pueda haber sido obtenido en Shodan
                        if (fp.os != OperatingSystem.OS.Unknown)
                        {
                            cdi.Computer.os = fp.os;
                            break;
                        }
                        //Se extrae software del banner
                        foreach (string software in BannerAnalysis.GetSoftwareFromBanner(fp.Version))
                        {
                            if (!cdi.Computer.Software.Items.Any(A => A.Name.ToLower() == software.ToLower()))
                                cdi.Computer.Software.Items.Add(new ApplicationsItem(software, string.Format("{0} FingerPrinting Banner: {1}", di.Domain, fp.Version)));
                        }
                    }
                }
            }
        }
    }
}
