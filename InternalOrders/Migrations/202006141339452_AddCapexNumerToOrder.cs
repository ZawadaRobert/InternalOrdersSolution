namespace InternalOrders.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCapexNumerToOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "CapexNumber", c => c.String(maxLength: 5));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "CapexNumber");
        }
    }
}
