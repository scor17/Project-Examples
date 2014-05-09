namespace healthApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Services",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        created = c.DateTime(nullable: false),
                        PatientID = c.String(nullable: false),
                        ClientFirstName = c.String(nullable: false, maxLength: 12),
                        ClientLastName = c.String(nullable: false, maxLength: 12),
                        RoomNo = c.String(),
                        Task = c.String(),
                        duration = c.Int(nullable: false),
                        dtStart = c.DateTime(nullable: false),
                        dtEnd = c.DateTime(),
                        freq = c.String(nullable: false),
                        interval = c.Int(),
                        count = c.Int(),
                        byDay = c.String(),
                        byMonthDay = c.Int(),
                        dtStartWD = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Tasks",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        taskID = c.Int(nullable: false),
                        PatientID = c.String(),
                        ClientFirstName = c.String(),
                        ClientLastName = c.String(),
                        RoomNo = c.String(),
                        Task = c.String(),
                        tDate = c.DateTime(nullable: false),
                        duration = c.Int(nullable: false),
                        actual = c.Int(nullable: false),
                        comments = c.String(),
                        isComplete = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Tasks");
            DropTable("dbo.Services");
        }
    }
}
