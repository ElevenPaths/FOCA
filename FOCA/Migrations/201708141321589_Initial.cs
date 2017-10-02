namespace FOCA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ComputerDomainsItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdProject = c.Int(nullable: false),
                        Source = c.String(),
                        Computer_Id = c.Int(),
                        Domain_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ComputersItems", t => t.Computer_Id)
                .ForeignKey("dbo.DomainsItems", t => t.Domain_Id)
                .Index(t => t.Computer_Id)
                .Index(t => t.Domain_Id);
            
            CreateTable(
                "dbo.ComputersItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdProject = c.Int(nullable: false),
                        localName = c.String(),
                        NotOS = c.Boolean(nullable: false),
                        os = c.Int(nullable: false),
                        type = c.Int(nullable: false),
                        name = c.String(),
                        Description_Id = c.Int(),
                        Printers_Id = c.Int(),
                        RemoteFolders_Id = c.Int(),
                        RemotePasswords_Id = c.Int(),
                        RemotePrinters_Id = c.Int(),
                        RemoteUsers_Id = c.Int(),
                        Software_Id = c.Int(),
                        Users_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Descriptions", t => t.Description_Id)
                .ForeignKey("dbo.Printers", t => t.Printers_Id)
                .ForeignKey("dbo.Paths", t => t.RemoteFolders_Id)
                .ForeignKey("dbo.Passwords", t => t.RemotePasswords_Id)
                .ForeignKey("dbo.Printers", t => t.RemotePrinters_Id)
                .ForeignKey("dbo.Users", t => t.RemoteUsers_Id)
                .ForeignKey("dbo.Applications", t => t.Software_Id)
                .ForeignKey("dbo.Users", t => t.Users_Id)
                .Index(t => t.Description_Id)
                .Index(t => t.Printers_Id)
                .Index(t => t.RemoteFolders_Id)
                .Index(t => t.RemotePasswords_Id)
                .Index(t => t.RemotePrinters_Id)
                .Index(t => t.RemoteUsers_Id)
                .Index(t => t.Software_Id)
                .Index(t => t.Users_Id);
            
            CreateTable(
                "dbo.Descriptions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DescriptionsItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Source = c.String(),
                        Descriptions_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Descriptions", t => t.Descriptions_Id)
                .Index(t => t.Descriptions_Id);
            
            CreateTable(
                "dbo.Printers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PrintersItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Printer = c.String(),
                        RemoteUsers_Id = c.Int(),
                        Printers_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.RemoteUsers_Id)
                .ForeignKey("dbo.Printers", t => t.Printers_Id)
                .Index(t => t.RemoteUsers_Id)
                .Index(t => t.Printers_Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IsComputerUser = c.Boolean(nullable: false),
                        Notes = c.String(),
                        Users_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Users_Id)
                .Index(t => t.Users_Id);
            
            CreateTable(
                "dbo.Paths",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PathsItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Path = c.String(),
                        IsComputerFolder = c.Boolean(nullable: false),
                        RemoteUsers_Id = c.Int(),
                        Paths_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.RemoteUsers_Id)
                .ForeignKey("dbo.Paths", t => t.Paths_Id)
                .Index(t => t.RemoteUsers_Id)
                .Index(t => t.Paths_Id);
            
            CreateTable(
                "dbo.Passwords",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PasswordsItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Password = c.String(),
                        Type = c.String(),
                        Source = c.String(),
                        Passwords_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Passwords", t => t.Passwords_Id)
                .Index(t => t.Passwords_Id);
            
            CreateTable(
                "dbo.Applications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ApplicationsItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Source = c.String(),
                        Applications_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Applications", t => t.Applications_Id)
                .Index(t => t.Applications_Id);
            
            CreateTable(
                "dbo.DomainsItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdProject = c.Int(nullable: false),
                        Domain = c.String(),
                        Source = c.String(),
                        jspFingerprintingAnalyzed = c.Boolean(nullable: false),
                        tplFingerprintingAnalyzed = c.Boolean(nullable: false),
                        robotsAnalyzed = c.Boolean(nullable: false),
                        map_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.HttpMaps", t => t.map_Id)
                .Index(t => t.map_Id);
            
            CreateTable(
                "dbo.FingerPrintings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Host = c.String(),
                        Port = c.Int(nullable: false),
                        Version = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        DomainsItem_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DomainsItems", t => t.DomainsItem_Id)
                .Index(t => t.DomainsItem_Id);
            
            CreateTable(
                "dbo.HttpMaps",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.HttpMapTypesFiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdHttMap = c.Int(nullable: false),
                        IdType = c.Int(nullable: false),
                        Value = c.String(),
                        Extension = c.String(),
                        HttpMap_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.HttpMaps", t => t.HttpMap_Id)
                .Index(t => t.HttpMap_Id);
            
            CreateTable(
                "dbo.ComputerIPsItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdProject = c.Int(nullable: false),
                        Source = c.String(),
                        Computer_Id = c.Int(),
                        Ip_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ComputersItems", t => t.Computer_Id)
                .ForeignKey("dbo.IPsItems", t => t.Ip_Id)
                .Index(t => t.Computer_Id)
                .Index(t => t.Ip_Id);
            
            CreateTable(
                "dbo.IPsItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdProject = c.Int(nullable: false),
                        Ip = c.String(),
                        Source = c.String(),
                        activeDNS = c.Boolean(nullable: false),
                        ZoneTransfer = c.Boolean(nullable: false),
                        Information_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ExtendedIPInformations", t => t.Information_Id)
                .Index(t => t.Information_Id);
            
            CreateTable(
                "dbo.ExtendedIPInformations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OS = c.String(),
                        Country = c.String(),
                        ServerBanner = c.String(),
                        ShodanResponse = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Configurations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BingApiKey = c.String(),
                        DuckDuckKey = c.String(),
                        DefaultDnsCacheSnooping = c.String(),
                        FingerPrintingAllFtp = c.Boolean(nullable: false),
                        FingerPrintingAllHttp = c.Boolean(nullable: false),
                        FingerPrintingAllSmtp = c.Boolean(nullable: false),
                        FingerPrintingDns = c.Boolean(nullable: false),
                        GoogleApiCx = c.String(),
                        GoogleApiKey = c.String(),
                        MaxRecursion = c.Int(nullable: false),
                        NumberOfTasks = c.Int(nullable: false),
                        ParallelDnsQueries = c.Int(nullable: false),
                        PasiveFingerPrintingSmtp = c.Boolean(nullable: false),
                        PassiveFingerPrintingHttp = c.Boolean(nullable: false),
                        ProjectConfigFile = c.String(),
                        ResolveHost = c.Boolean(nullable: false),
                        ScanNetranges255 = c.Boolean(nullable: false),
                        ShodanApiKey = c.String(),
                        SimultaneousDownloads = c.Int(nullable: false),
                        SPathsPlugins = c.String(),
                        UseAllDns = c.Boolean(nullable: false),
                        UseHead = c.Boolean(nullable: false),
                        webSearcherEngine = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FilesITems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdProject = c.Int(nullable: false),
                        Ext = c.String(),
                        ModifiedDate = c.DateTime(nullable: false),
                        URL = c.String(),
                        Path = c.String(),
                        Date = c.DateTime(nullable: false),
                        Size = c.Int(nullable: false),
                        Downloaded = c.Boolean(nullable: false),
                        Processed = c.Boolean(nullable: false),
                        Metadata_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MetaExtractors", t => t.Metadata_Id)
                .Index(t => t.Metadata_Id);
            
            CreateTable(
                "dbo.MetaExtractors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Thumbnail = c.Binary(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        FoundDates_Id = c.Int(),
                        FoundEmails_Id = c.Int(),
                        FoundHistory_Id = c.Int(),
                        FoundMetaData_Id = c.Int(),
                        FoundOldVersions_Id = c.Int(),
                        FoundPasswords_Id = c.Int(),
                        FoundPaths_Id = c.Int(),
                        FoundPrinters_Id = c.Int(),
                        FoundServers_Id = c.Int(),
                        FoundUsers_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Dates", t => t.FoundDates_Id)
                .ForeignKey("dbo.Emails", t => t.FoundEmails_Id)
                .ForeignKey("dbo.Histories", t => t.FoundHistory_Id)
                .ForeignKey("dbo.MetaDatas", t => t.FoundMetaData_Id)
                .ForeignKey("dbo.OldVersions", t => t.FoundOldVersions_Id)
                .ForeignKey("dbo.Passwords", t => t.FoundPasswords_Id)
                .ForeignKey("dbo.Paths", t => t.FoundPaths_Id)
                .ForeignKey("dbo.Printers", t => t.FoundPrinters_Id)
                .ForeignKey("dbo.Servers", t => t.FoundServers_Id)
                .ForeignKey("dbo.Users", t => t.FoundUsers_Id)
                .Index(t => t.FoundDates_Id)
                .Index(t => t.FoundEmails_Id)
                .Index(t => t.FoundHistory_Id)
                .Index(t => t.FoundMetaData_Id)
                .Index(t => t.FoundOldVersions_Id)
                .Index(t => t.FoundPasswords_Id)
                .Index(t => t.FoundPaths_Id)
                .Index(t => t.FoundPrinters_Id)
                .Index(t => t.FoundServers_Id)
                .Index(t => t.FoundUsers_Id);
            
            CreateTable(
                "dbo.Dates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreationDateSpecified = c.Boolean(nullable: false),
                        ModificationDateSpecified = c.Boolean(nullable: false),
                        DatePrintingSpecified = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Emails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EmailsItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Mail = c.String(),
                        Emails_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Emails", t => t.Emails_Id)
                .Index(t => t.Emails_Id);
            
            CreateTable(
                "dbo.Histories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.HistoryItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Author = c.String(),
                        Comments = c.String(),
                        Path = c.String(),
                        History_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Histories", t => t.History_Id)
                .Index(t => t.History_Id);
            
            CreateTable(
                "dbo.MetaDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Subject = c.String(),
                        DataBase = c.String(),
                        Category = c.String(),
                        Codification = c.String(),
                        Comments = c.String(),
                        Company = c.String(),
                        Description = c.String(),
                        Statistic = c.String(),
                        Language = c.String(),
                        UserInfo = c.String(),
                        VersionNumber = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Keywords = c.String(),
                        Template = c.String(),
                        OperativeSystem = c.String(),
                        EditTime = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Model = c.String(),
                        Applications_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Applications", t => t.Applications_Id)
                .Index(t => t.Applications_Id);
            
            CreateTable(
                "dbo.OldVersions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OldVersionsItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Author = c.String(),
                        Comments = c.String(),
                        SpecificDate = c.Boolean(nullable: false),
                        Path = c.String(),
                        OldVersions_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.OldVersions", t => t.OldVersions_Id)
                .Index(t => t.OldVersions_Id);
            
            CreateTable(
                "dbo.Servers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ServersItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Source = c.String(),
                        Servers_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Servers", t => t.Servers_Id)
                .Index(t => t.Servers_Id);
            
            CreateTable(
                "dbo.Limits",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdProject = c.Int(nullable: false),
                        Range = c.String(),
                        Lower = c.Int(nullable: false),
                        Higher = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Plugins",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        description = c.String(),
                        name = c.String(),
                        _namespace = c.String(),
                        path = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProjectName = c.String(),
                        ProjectSaveFile = c.String(),
                        ProjectState = c.Int(nullable: false),
                        Domain = c.String(),
                        FolderToDownload = c.String(),
                        ProjectDate = c.DateTime(nullable: false),
                        ProjectNotes = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RelationsItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdProject = c.Int(nullable: false),
                        Source = c.String(),
                        Domain_Id = c.Int(),
                        Ip_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DomainsItems", t => t.Domain_Id)
                .ForeignKey("dbo.IPsItems", t => t.Ip_Id)
                .Index(t => t.Domain_Id)
                .Index(t => t.Ip_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RelationsItems", "Ip_Id", "dbo.IPsItems");
            DropForeignKey("dbo.RelationsItems", "Domain_Id", "dbo.DomainsItems");
            DropForeignKey("dbo.FilesITems", "Metadata_Id", "dbo.MetaExtractors");
            DropForeignKey("dbo.MetaExtractors", "FoundUsers_Id", "dbo.Users");
            DropForeignKey("dbo.MetaExtractors", "FoundServers_Id", "dbo.Servers");
            DropForeignKey("dbo.ServersItems", "Servers_Id", "dbo.Servers");
            DropForeignKey("dbo.MetaExtractors", "FoundPrinters_Id", "dbo.Printers");
            DropForeignKey("dbo.MetaExtractors", "FoundPaths_Id", "dbo.Paths");
            DropForeignKey("dbo.MetaExtractors", "FoundPasswords_Id", "dbo.Passwords");
            DropForeignKey("dbo.MetaExtractors", "FoundOldVersions_Id", "dbo.OldVersions");
            DropForeignKey("dbo.OldVersionsItems", "OldVersions_Id", "dbo.OldVersions");
            DropForeignKey("dbo.MetaExtractors", "FoundMetaData_Id", "dbo.MetaDatas");
            DropForeignKey("dbo.MetaDatas", "Applications_Id", "dbo.Applications");
            DropForeignKey("dbo.MetaExtractors", "FoundHistory_Id", "dbo.Histories");
            DropForeignKey("dbo.HistoryItems", "History_Id", "dbo.Histories");
            DropForeignKey("dbo.MetaExtractors", "FoundEmails_Id", "dbo.Emails");
            DropForeignKey("dbo.EmailsItems", "Emails_Id", "dbo.Emails");
            DropForeignKey("dbo.MetaExtractors", "FoundDates_Id", "dbo.Dates");
            DropForeignKey("dbo.ComputerIPsItems", "Ip_Id", "dbo.IPsItems");
            DropForeignKey("dbo.IPsItems", "Information_Id", "dbo.ExtendedIPInformations");
            DropForeignKey("dbo.ComputerIPsItems", "Computer_Id", "dbo.ComputersItems");
            DropForeignKey("dbo.ComputerDomainsItems", "Domain_Id", "dbo.DomainsItems");
            DropForeignKey("dbo.DomainsItems", "map_Id", "dbo.HttpMaps");
            DropForeignKey("dbo.HttpMapTypesFiles", "HttpMap_Id", "dbo.HttpMaps");
            DropForeignKey("dbo.FingerPrintings", "DomainsItem_Id", "dbo.DomainsItems");
            DropForeignKey("dbo.ComputerDomainsItems", "Computer_Id", "dbo.ComputersItems");
            DropForeignKey("dbo.ComputersItems", "Users_Id", "dbo.Users");
            DropForeignKey("dbo.ComputersItems", "Software_Id", "dbo.Applications");
            DropForeignKey("dbo.ApplicationsItems", "Applications_Id", "dbo.Applications");
            DropForeignKey("dbo.ComputersItems", "RemoteUsers_Id", "dbo.Users");
            DropForeignKey("dbo.ComputersItems", "RemotePrinters_Id", "dbo.Printers");
            DropForeignKey("dbo.ComputersItems", "RemotePasswords_Id", "dbo.Passwords");
            DropForeignKey("dbo.PasswordsItems", "Passwords_Id", "dbo.Passwords");
            DropForeignKey("dbo.ComputersItems", "RemoteFolders_Id", "dbo.Paths");
            DropForeignKey("dbo.PathsItems", "Paths_Id", "dbo.Paths");
            DropForeignKey("dbo.PathsItems", "RemoteUsers_Id", "dbo.Users");
            DropForeignKey("dbo.ComputersItems", "Printers_Id", "dbo.Printers");
            DropForeignKey("dbo.PrintersItems", "Printers_Id", "dbo.Printers");
            DropForeignKey("dbo.PrintersItems", "RemoteUsers_Id", "dbo.Users");
            DropForeignKey("dbo.UserItems", "Users_Id", "dbo.Users");
            DropForeignKey("dbo.ComputersItems", "Description_Id", "dbo.Descriptions");
            DropForeignKey("dbo.DescriptionsItems", "Descriptions_Id", "dbo.Descriptions");
            DropIndex("dbo.RelationsItems", new[] { "Ip_Id" });
            DropIndex("dbo.RelationsItems", new[] { "Domain_Id" });
            DropIndex("dbo.ServersItems", new[] { "Servers_Id" });
            DropIndex("dbo.OldVersionsItems", new[] { "OldVersions_Id" });
            DropIndex("dbo.MetaDatas", new[] { "Applications_Id" });
            DropIndex("dbo.HistoryItems", new[] { "History_Id" });
            DropIndex("dbo.EmailsItems", new[] { "Emails_Id" });
            DropIndex("dbo.MetaExtractors", new[] { "FoundUsers_Id" });
            DropIndex("dbo.MetaExtractors", new[] { "FoundServers_Id" });
            DropIndex("dbo.MetaExtractors", new[] { "FoundPrinters_Id" });
            DropIndex("dbo.MetaExtractors", new[] { "FoundPaths_Id" });
            DropIndex("dbo.MetaExtractors", new[] { "FoundPasswords_Id" });
            DropIndex("dbo.MetaExtractors", new[] { "FoundOldVersions_Id" });
            DropIndex("dbo.MetaExtractors", new[] { "FoundMetaData_Id" });
            DropIndex("dbo.MetaExtractors", new[] { "FoundHistory_Id" });
            DropIndex("dbo.MetaExtractors", new[] { "FoundEmails_Id" });
            DropIndex("dbo.MetaExtractors", new[] { "FoundDates_Id" });
            DropIndex("dbo.FilesITems", new[] { "Metadata_Id" });
            DropIndex("dbo.IPsItems", new[] { "Information_Id" });
            DropIndex("dbo.ComputerIPsItems", new[] { "Ip_Id" });
            DropIndex("dbo.ComputerIPsItems", new[] { "Computer_Id" });
            DropIndex("dbo.HttpMapTypesFiles", new[] { "HttpMap_Id" });
            DropIndex("dbo.FingerPrintings", new[] { "DomainsItem_Id" });
            DropIndex("dbo.DomainsItems", new[] { "map_Id" });
            DropIndex("dbo.ApplicationsItems", new[] { "Applications_Id" });
            DropIndex("dbo.PasswordsItems", new[] { "Passwords_Id" });
            DropIndex("dbo.PathsItems", new[] { "Paths_Id" });
            DropIndex("dbo.PathsItems", new[] { "RemoteUsers_Id" });
            DropIndex("dbo.UserItems", new[] { "Users_Id" });
            DropIndex("dbo.PrintersItems", new[] { "Printers_Id" });
            DropIndex("dbo.PrintersItems", new[] { "RemoteUsers_Id" });
            DropIndex("dbo.DescriptionsItems", new[] { "Descriptions_Id" });
            DropIndex("dbo.ComputersItems", new[] { "Users_Id" });
            DropIndex("dbo.ComputersItems", new[] { "Software_Id" });
            DropIndex("dbo.ComputersItems", new[] { "RemoteUsers_Id" });
            DropIndex("dbo.ComputersItems", new[] { "RemotePrinters_Id" });
            DropIndex("dbo.ComputersItems", new[] { "RemotePasswords_Id" });
            DropIndex("dbo.ComputersItems", new[] { "RemoteFolders_Id" });
            DropIndex("dbo.ComputersItems", new[] { "Printers_Id" });
            DropIndex("dbo.ComputersItems", new[] { "Description_Id" });
            DropIndex("dbo.ComputerDomainsItems", new[] { "Domain_Id" });
            DropIndex("dbo.ComputerDomainsItems", new[] { "Computer_Id" });
            DropTable("dbo.RelationsItems");
            DropTable("dbo.Projects");
            DropTable("dbo.Plugins");
            DropTable("dbo.Limits");
            DropTable("dbo.ServersItems");
            DropTable("dbo.Servers");
            DropTable("dbo.OldVersionsItems");
            DropTable("dbo.OldVersions");
            DropTable("dbo.MetaDatas");
            DropTable("dbo.HistoryItems");
            DropTable("dbo.Histories");
            DropTable("dbo.EmailsItems");
            DropTable("dbo.Emails");
            DropTable("dbo.Dates");
            DropTable("dbo.MetaExtractors");
            DropTable("dbo.FilesITems");
            DropTable("dbo.Configurations");
            DropTable("dbo.ExtendedIPInformations");
            DropTable("dbo.IPsItems");
            DropTable("dbo.ComputerIPsItems");
            DropTable("dbo.HttpMapTypesFiles");
            DropTable("dbo.HttpMaps");
            DropTable("dbo.FingerPrintings");
            DropTable("dbo.DomainsItems");
            DropTable("dbo.ApplicationsItems");
            DropTable("dbo.Applications");
            DropTable("dbo.PasswordsItems");
            DropTable("dbo.Passwords");
            DropTable("dbo.PathsItems");
            DropTable("dbo.Paths");
            DropTable("dbo.UserItems");
            DropTable("dbo.Users");
            DropTable("dbo.PrintersItems");
            DropTable("dbo.Printers");
            DropTable("dbo.DescriptionsItems");
            DropTable("dbo.Descriptions");
            DropTable("dbo.ComputersItems");
            DropTable("dbo.ComputerDomainsItems");
        }
    }
}
