namespace InternalOrders.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveFileExtensionFromAttachment : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Attachments", "FileExtension");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Attachments", "FileExtension", c => c.String(nullable: false, maxLength: 16));
        }
    }
}
