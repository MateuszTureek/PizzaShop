namespace PizzaShop.Migrations.MovieStore
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Version_11 : DbMigration
    {
        public override void Up()
        {
            DropTable("pizza.Events");
            DropTable("pizza.SrcPictures");
        }
        
        public override void Down()
        {
            CreateTable(
                "pizza.SrcPictures",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Src = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "pizza.Events",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 50),
                        Content = c.String(nullable: false, maxLength: 250),
                    })
                .PrimaryKey(t => t.ID);
            
        }
    }
}
