namespace ErrorLog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedAppMoniker : DbMigration
    {
        public override void Up()
        {
            AddColumn("ErrorLog.Apps", "Moniker", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("ErrorLog.Apps", "Moniker");
        }
    }
}
