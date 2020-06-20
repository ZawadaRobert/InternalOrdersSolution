namespace InternalOrders.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialNew : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Approvers",
                c => new
                    {
                        ApproverId = c.Int(nullable: false, identity: true),
                        Queue = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        OrderId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ApproverId)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.OrderId);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        OrderId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        Date = c.DateTime(nullable: false),
                        CustomerFunded = c.Boolean(nullable: false),
                        Currency = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.OrderId);
            
            CreateTable(
                "dbo.Attachments",
                c => new
                    {
                        AttachmentId = c.Int(nullable: false, identity: true),
                        FileName = c.String(nullable: false),
                        FileExtension = c.String(nullable: false),
                        FileData = c.Binary(nullable: false),
                        OrderId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AttachmentId)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .Index(t => t.OrderId);
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        ItemId = c.Int(nullable: false, identity: true),
                        RekordIndex = c.String(maxLength: 7),
                        Name = c.String(),
                        Price = c.Single(nullable: false),
                        Quantity = c.Int(nullable: false),
                        DeliveryDate = c.DateTime(nullable: false),
                        OrderId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ItemId)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .Index(t => t.OrderId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        PasswordHash = c.String(nullable: false),
                        Department = c.Int(nullable: false),
                        IsAdmin = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        RoleId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RoleId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Approvers", "UserId", "dbo.Users");
            DropForeignKey("dbo.Roles", "UserId", "dbo.Users");
            DropForeignKey("dbo.Items", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.Attachments", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.Approvers", "OrderId", "dbo.Orders");
            DropIndex("dbo.Roles", new[] { "UserId" });
            DropIndex("dbo.Items", new[] { "OrderId" });
            DropIndex("dbo.Attachments", new[] { "OrderId" });
            DropIndex("dbo.Approvers", new[] { "OrderId" });
            DropIndex("dbo.Approvers", new[] { "UserId" });
            DropTable("dbo.Roles");
            DropTable("dbo.Users");
            DropTable("dbo.Items");
            DropTable("dbo.Attachments");
            DropTable("dbo.Orders");
            DropTable("dbo.Approvers");
        }
    }
}
