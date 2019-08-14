using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FOCA.Database.Entities
{
    public class MetaExtractor
    {
        public int Id { get; set; }

        public Emails FoundEmails { get; set; }

        public Dates FoundDates { get; set; }

        public Printers FoundPrinters { get; set; }

        public Paths FoundPaths { get; set; }

        public OldVersions FoundOldVersions { get; set; }

        public History FoundHistory { get; set; }

        public MetaData FoundMetaData { get; set; }

        public Users FoundUsers { get; set; }

        public Servers FoundServers { get; set; }

        public Passwords FoundPasswords { get; set; }

        public byte[] Thumbnail { get; set; }

        [MaxLength(128)]
        [Required]
        public string Discriminator { get; set; }

        public MetaExtractor()
        {
            FoundEmails = new Emails();
            FoundDates = new Dates();
            FoundPrinters = new Printers();
            FoundPaths = new Paths();
            FoundOldVersions = new OldVersions();
            FoundHistory = new History();
            FoundMetaData = new MetaData();
            FoundUsers = new Users();
            FoundServers = new Servers();
            FoundPasswords = new Passwords();
        }

        public MetaExtractor(MetadataExtractCore.Diagrams.FileMetadata fileMetadata)
        {
            this.FoundMetaData = new MetaData()
            {
                Applications = new Applications() { Items = new ModifiedComponents.ThreadSafeList<ApplicationsItem>(fileMetadata.Applications.Select(p => new ApplicationsItem(p.Value, p.Source))) },
                Title = fileMetadata.Title,
                Category = fileMetadata.Category,
                Codification = fileMetadata.Codification,
                Comments = fileMetadata.Comments,
                Company = fileMetadata.Company,
                DataBase = fileMetadata.DataBase,
                Description = fileMetadata.Description,
                EditTime = fileMetadata.EditTime,
                Keywords = fileMetadata.Keywords,
                Language = fileMetadata.Language,
                Model = fileMetadata.Model,
                OperativeSystem = fileMetadata.OperatingSystem,
                Statistic = fileMetadata.Statistic,
                Subject = fileMetadata.Subject,
                Template = fileMetadata.Template,
                UserInfo = fileMetadata.UserInfo,
                VersionNumber = fileMetadata.VersionNumber,
            };

            this.FoundDates = new Dates()
            {
                CreationDate = fileMetadata.Dates.CreationDate.GetValueOrDefault(),
                CreationDateSpecified = fileMetadata.Dates.CreationDate.HasValue,
                DatePrinting = fileMetadata.Dates.PrintingDate.GetValueOrDefault(),
                DatePrintingSpecified = fileMetadata.Dates.PrintingDate.HasValue,
                ModificationDate = fileMetadata.Dates.ModificationDate.GetValueOrDefault(),
                ModificationDateSpecified = fileMetadata.Dates.ModificationDate.HasValue
            };

            this.Discriminator = "MetaExtractor";
            this.Thumbnail = fileMetadata.Thumbnail;

            this.FoundUsers = new Users() { Items = new ModifiedComponents.ThreadSafeList<UserItem>(fileMetadata.Users.Select(p => new UserItem() { Name = p.Value, IsComputerUser = p.IsComputerUser, Notes = p.Notes })) };
            this.FoundEmails = new Emails() { Items = new List<EmailsItem>(fileMetadata.Emails.Select(p => new EmailsItem() { Mail = p.Value })) };
            this.FoundHistory = new History() { Items = new List<HistoryItem>(fileMetadata.History.Select(p => new HistoryItem() { Author = p.Value, Comments = p.Comments, Path = p.Path })) };
            this.FoundOldVersions = new OldVersions() { Items = new List<OldVersionsItem>(fileMetadata.OldVersions.Select(p => new OldVersionsItem() { Author = p.Value, Comments = p.Comments, Date = p.Date.GetValueOrDefault(), Path = p.Path, SpecificDate = p.Date.HasValue })) };
            this.FoundPasswords = new Passwords() { Items = new List<PasswordsItem>(fileMetadata.Passwords.Select(p => new PasswordsItem(p.Value, p.Type, p.Source))) };
            this.FoundPaths = new Paths() { Items = new List<PathsItem>(fileMetadata.Paths.Select(p => new PathsItem() { Path = p.Value, IsComputerFolder = p.IsComputerFolder, RemoteUsers = new Users() })) };
            this.FoundPrinters = new Printers() { Items = new List<PrintersItem>(fileMetadata.Printers.Select(p => new PrintersItem() { Printer = p.Value, RemoteUsers = new Users() })) };
            this.FoundServers = new Servers() { Items = new List<ServersItem>(fileMetadata.Servers.Select(p => new ServersItem(p.Value, p.Source))) };
        }
    }
}