namespace healthApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class igor1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tasks", "serviceQuality", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tasks", "serviceQuality");
        }
    }
}
