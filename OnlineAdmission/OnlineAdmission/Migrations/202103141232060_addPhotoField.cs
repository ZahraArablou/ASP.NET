namespace OnlineAdmission.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addPhotoField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Applications", "Photo", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Applications", "Photo");
        }
    }
}
