namespace PizzaShop.Migrations.Cms
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InformationAddPositionproperty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InformationItems", "Position", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.InformationItems", "Position");
        }
    }
}
