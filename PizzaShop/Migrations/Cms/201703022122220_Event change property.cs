namespace PizzaShop.Migrations.Cms
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Eventchangeproperty : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Events", "Content", c => c.String(nullable: false, maxLength: 300));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Events", "Content", c => c.String());
        }
    }
}
