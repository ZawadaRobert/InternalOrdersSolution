namespace InternalOrders.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RestrictStringLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Orders", "Name", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Orders", "Description", c => c.String(maxLength: 512));
            AlterColumn("dbo.Attachments", "FileName", c => c.String(nullable: false, maxLength: 256));
            AlterColumn("dbo.Attachments", "FileExtension", c => c.String(nullable: false, maxLength: 16));
            AlterColumn("dbo.Items", "Name", c => c.String(maxLength: 64));
            AlterColumn("dbo.Users", "Name", c => c.String(nullable: false, maxLength: 64));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Items", "Name", c => c.String());
            AlterColumn("dbo.Attachments", "FileExtension", c => c.String(nullable: false));
            AlterColumn("dbo.Attachments", "FileName", c => c.String(nullable: false));
            AlterColumn("dbo.Orders", "Description", c => c.String());
            AlterColumn("dbo.Orders", "Name", c => c.String(nullable: false));
        }
    }
}
