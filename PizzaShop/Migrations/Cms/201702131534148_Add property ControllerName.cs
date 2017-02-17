namespace PizzaShop.Migrations.Cms
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddpropertyControllerName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MenuItems", "ControllerName", c => c.String(nullable: false, maxLength: 30));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MenuItems", "ControllerName");
        }
    }
}
