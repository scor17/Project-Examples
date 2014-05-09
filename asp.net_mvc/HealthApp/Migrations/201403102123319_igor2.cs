namespace healthApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class igor2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tasks", "serviceRating", c => c.Int(nullable: false));
            DropColumn("dbo.Tasks", "serviceQuality");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tasks", "serviceQuality", c => c.Int(nullable: false));
            DropColumn("dbo.Tasks", "serviceRating");
        }
    }
}
