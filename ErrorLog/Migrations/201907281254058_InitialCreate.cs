namespace ErrorLog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "ErrorLog.Apps",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "ErrorLog.Logs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Timestamp = c.DateTime(nullable: false),
                        Message = c.String(),
                        StackTrace = c.String(),
                        Level = c.Int(nullable: false),
                        App_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("ErrorLog.Apps", t => t.App_Id)
                .Index(t => t.App_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("ErrorLog.Logs", "App_Id", "ErrorLog.Apps");
            DropIndex("ErrorLog.Logs", new[] { "App_Id" });
            DropTable("ErrorLog.Logs");
            DropTable("ErrorLog.Apps");
        }
    }
}
