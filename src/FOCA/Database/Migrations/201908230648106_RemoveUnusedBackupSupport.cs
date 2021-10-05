namespace FOCA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveUnusedBackupSupport : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.HttpMapTypesFiles", "HttpMap_Id", "dbo.HttpMaps");
            DropIndex("dbo.HttpMapTypesFiles", new[] { "HttpMap_Id" });
            DropColumn("dbo.HttpMapTypesFiles", "Extension");
            DropColumn("dbo.HttpMapTypesFiles", "HttpMap_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.HttpMapTypesFiles", "HttpMap_Id", c => c.Int());
            AddColumn("dbo.HttpMapTypesFiles", "Extension", c => c.String());
            CreateIndex("dbo.HttpMapTypesFiles", "HttpMap_Id");
            AddForeignKey("dbo.HttpMapTypesFiles", "HttpMap_Id", "dbo.HttpMaps", "Id");
        }
    }
}
