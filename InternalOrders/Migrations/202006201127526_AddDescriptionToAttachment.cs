namespace InternalOrders.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDescriptionToAttachment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Attachments", "Description", c => c.String(nullable: false, maxLength: 256));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Attachments", "Description");
        }
    }
}
