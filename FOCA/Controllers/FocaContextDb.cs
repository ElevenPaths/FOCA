using System.Data.Entity;
using FOCA.Plugins;
using MetadataExtractCore.Diagrams;

namespace FOCA.Controllers
{
    public class FocaContextDb : DbContext
    {
        public DbSet<Project> Projects { get; set; }
        public DbSet<FilesITem> Files { get; set; }
        public DbSet<DomainsItem> Domains { get; set; }
        public DbSet<ComputersItem> Computers { get; set; }
        public DbSet<ComputerIPsItem> ComputerIps { get; set; }
        public DbSet<ComputerDomainsItem> ComputerDomain { get; set; }
        public DbSet<Limits> Limits { get; set; }
        public DbSet<RelationsItem> Relations { get; set; }
        public DbSet<Configuration> Configurations { get; set; }
        public DbSet<IPsItem> Ips { get; set; }
        public DbSet<HttpMapTypesFiles> HttpMapTypesFiles { get; set; }
        public DbSet<Plugin> Plugins { get; set; } 
    }
}
