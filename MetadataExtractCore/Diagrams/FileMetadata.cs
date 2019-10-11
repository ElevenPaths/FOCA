using System;
using System.Collections.Generic;

namespace MetadataExtractCore.Diagrams
{
    [Serializable]
    public class FileMetadata
    {
        public ICollection<Application> Applications { get; set; }
        public ICollection<Email> Emails { get; set; }
        public ICollection<Printer> Printers { get; set; }
        public ICollection<Path> Paths { get; set; }
        public ICollection<OldVersion> OldVersions { get; set; }
        public ICollection<History> History { get; set; }
        public ICollection<User> Users { get; set; }
        public ICollection<Server> Servers { get; set; }
        public ICollection<Password> Passwords { get; set; }
        public Dictionary<string, Dictionary<string, string>> Makernotes { get; set; }
        public Dictionary<string, FileMetadata> EmbeddedImages { get; set; }

        public Dates Dates;

        public byte[] Thumbnail { get; set; }

        public string Title { get; set; }
        public string Subject { get; set; }

        public string DataBase { get; set; }

        public string Category { get; set; }

        public string Codification { get; set; }

        public string Comments { get; set; }

        public string Company { get; set; }

        public string Description { get; set; }

        public string Statistic { get; set; }

        public string Language { get; set; }

        public string UserInfo { get; set; }

        public decimal VersionNumber { get; set; }

        public string Keywords { get; set; }

        public string Template { get; set; }

        public string OperatingSystem { get; set; }

        public decimal EditTime { get; set; }

        public string Model { get; set; }

        public GeoLocation GPS { get; set; }

        public FileMetadata()
        {
            this.Applications = new HashSet<Application>();
            this.Emails = new HashSet<Email>();
            this.History = new HashSet<History>();
            this.OldVersions = new HashSet<OldVersion>();
            this.Passwords = new HashSet<Password>();
            this.Paths = new HashSet<Path>();
            this.Printers = new HashSet<Printer>();
            this.Servers = new HashSet<Server>();
            this.Users = new HashSet<User>();
            this.Makernotes = new Dictionary<string, Dictionary<string, string>>();
            this.EmbeddedImages = new Dictionary<string, FileMetadata>();
        }

        public bool HasMetadata()
        {
            return this.Users.Count > 0 ||
                   this.Applications.Count > 0 ||
                   this.Emails.Count > 0 ||
                   this.Paths.Count > 0 ||
                   this.Servers.Count > 0 ||
                   this.OldVersions.Count > 0 ||
                   this.History.Count > 0 ||
                   this.Printers.Count > 0 ||
                   this.Passwords.Count > 0 ||
                   this.Makernotes.Count > 0 ||
                   !String.IsNullOrWhiteSpace(this.Category) ||
                   !String.IsNullOrWhiteSpace(this.Codification) ||
                   !String.IsNullOrWhiteSpace(this.Comments) ||
                   !String.IsNullOrWhiteSpace(this.Company) ||
                   !String.IsNullOrWhiteSpace(this.DataBase) ||
                   !String.IsNullOrWhiteSpace(this.Description) ||
                   !String.IsNullOrWhiteSpace(this.Keywords) ||
                   !String.IsNullOrWhiteSpace(this.Language) ||
                   !String.IsNullOrWhiteSpace(this.Model) ||
                   !String.IsNullOrWhiteSpace(this.OperatingSystem) ||
                   !String.IsNullOrWhiteSpace(this.Statistic) ||
                   !String.IsNullOrWhiteSpace(this.Subject) ||
                   !String.IsNullOrWhiteSpace(this.Template) ||
                   !String.IsNullOrWhiteSpace(this.Title) ||
                   !String.IsNullOrWhiteSpace(this.UserInfo) ||
                   this.GPS != null ||
                   this.VersionNumber > 0 ||
                   this.EditTime > 0 ||
                   this.Dates.CreationDate.HasValue ||
                   this.Dates.ModificationDate.HasValue ||
                   this.Dates.PrintingDate.HasValue;
        }

        public void AddRange<T>(params T[] values) where T : MetadataValue
        {
            foreach (var item in values)
            {
                this.Add(item);
            }
        }

        public void Add<T>(T value) where T : MetadataValue
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (value.IsValid())
            {
                if (value is Application)
                {
                    this.Applications.Add(value as Application);
                }
                else if (value is User)
                {
                    this.Users.Add(value as User);
                }
                else if (value is Path)
                {
                    this.Paths.Add(value as Path);
                }
                else if (value is Email)
                {
                    this.Emails.Add(value as Email);
                }
                else if (value is History)
                {
                    this.History.Add(value as History);
                }
                else if (value is OldVersion)
                {
                    this.OldVersions.Add(value as OldVersion);
                }
                else if (value is Printer)
                {
                    this.Printers.Add(value as Printer);
                }
                else if (value is Server)
                {
                    this.Servers.Add(value as Server);
                }
                else if (value is Password)
                {
                    this.Passwords.Add(value as Password);
                }
            }
        }
    }
}
