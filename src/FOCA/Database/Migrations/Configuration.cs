namespace FOCA.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<FOCA.Database.FocaContextDb>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            this.MigrationsDirectory = @"Database\Migrations\";
        }

        protected override void Seed(FOCA.Database.FocaContextDb contextDb)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    contextDb.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
