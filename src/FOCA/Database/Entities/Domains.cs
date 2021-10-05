using FOCA.Analysis.FingerPrinting;
using FOCA.Analysis.HttpMap;
using FOCA.Analysis.Technology;
using FOCA.ModifiedComponents;
using Newtonsoft.Json;
using System;

namespace FOCA.Database.Entities
{
    [Serializable]
    public class PanelInformationOptionsSerializableClass : IDisposable
    {
        // Log tab
        public ThreadSafeList<string> log_Log = new ThreadSafeList<string>();
        // Files tab
        public ThreadSafeList<bool> files_Extensions = new ThreadSafeList<bool>();
        public bool files_Google = true;
        public bool files_Bing = true;

        public bool crawling_BingEnabled = true;
        public bool crawling_GoogleEnabled = true;

        public PanelInformationOptionsSerializableClass()
        {
            int nExtensions = 22;
            for (int i = 0; i < nExtensions; i++)
                files_Extensions.Add(true);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    log_Log.Dispose();
                    files_Extensions.Dispose();
                }

                log_Log = null;
                files_Extensions = null;

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }

    [Serializable]
    public class Domains : IDisposable
    {
        private ThreadSafeList<DomainsItem> itemsField;

        public Domains()
        {
            itemsField = new ThreadSafeList<DomainsItem>();
        }

        public ThreadSafeList<DomainsItem> Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    itemsField.Dispose();
                }

                itemsField = null;

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }

    [Serializable]
    public class DomainsItem : BaseItem, IDisposable
    {
        [JsonIgnore]
        public int Id { get; set; }

        [JsonIgnore]
        public override int IdProject { get; set; }

        public string Domain { get; set; }

        public string Source { get; set; }

        [JsonIgnore]
        public ThreadSafeList<FingerPrinting> fingerPrinting { get; set; } = new ThreadSafeList<FingerPrinting>();

        public virtual HttpMap map { get; set; } = new HttpMap();

        [JsonIgnore]
        public TechnologyAnalysis techAnalysis = new TechnologyAnalysis();

        [JsonIgnore]
        public bool jspFingerprintingAnalyzed { get; set; } = false;
        [JsonIgnore]
        public bool tplFingerprintingAnalyzed { get; set; } = false;
        [JsonIgnore]
        public bool robotsAnalyzed { get; set; } = false;

        [JsonIgnore]
        public OptionStatus multiplesChoisesAnalyzed = OptionStatus.NotAnalyzed;
        [JsonIgnore]
        public OperatingSystem.OS os = OperatingSystem.OS.Unknown;

        [JsonIgnore]
        public PanelInformationOptionsSerializableClass informationOptions =
            new PanelInformationOptionsSerializableClass();

        public enum OptionStatus
        {
            NotAnalyzed = 0,
            Analyzing = 1,
            AnalyzedNoVulnerable = 2,
            AnalyzedVulnerable = 3
        }

        public DomainsItem() : base() { }

        public DomainsItem(string domain, string source) : base()
        {
            this.Domain = domain;
            this.Source = source;
            informationOptions = new PanelInformationOptionsSerializableClass();
        }

        public override string ToString()
        {
            return this.Domain;
        }

        public void AnalyzeTechnology()
        {
            this.map.SearchingTechnology = HttpMap.SearchStatus.Searching;
            techAnalysis.DetailedSearch(this);
            techAnalysis.EndAnalysis += delegate
            {
                this.map.SearchingTechnology = HttpMap.SearchStatus.Finished;
            };
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    map.Dispose();
                    fingerPrinting.Dispose();
                    informationOptions.Dispose();
                }

                fingerPrinting = null;
                map = null;
                informationOptions = null;

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}