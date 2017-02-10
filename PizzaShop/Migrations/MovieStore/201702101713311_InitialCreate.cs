namespace PizzaShop.Migrations.MovieStore
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "pizza.Components",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 30),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "pizza.Pizzas",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Price = c.Decimal(nullable: false, precision: 5, scale: 2),
                        PSizeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("pizza.PizzaSizes", t => t.PSizeID, cascadeDelete: true)
                .Index(t => t.PSizeID);
            
            CreateTable(
                "pizza.PizzaSizes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Size = c.String(nullable: false, maxLength: 5),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "pizza.Salads",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 30),
                        Price = c.Decimal(nullable: false, precision: 5, scale: 3),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "pizza.Drinks",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Price = c.Decimal(nullable: false, precision: 5, scale: 2),
                        Capacity = c.Single(nullable: false),
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
            
            CreateTable(
                "pizza.Sauces",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Price = c.Decimal(nullable: false, precision: 5, scale: 2),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "pizza.SrcPictures",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Src = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "pizza.PizzaComponents",
                c => new
                    {
                        PizzaIDRef = c.Int(nullable: false),
                        CompIDRef = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PizzaIDRef, t.CompIDRef })
                .ForeignKey("pizza.Pizzas", t => t.PizzaIDRef, cascadeDelete: true)
                .ForeignKey("pizza.Components", t => t.CompIDRef, cascadeDelete: true)
                .Index(t => t.PizzaIDRef)
                .Index(t => t.CompIDRef);
            
            CreateTable(
                "pizza.SaladComponents",
                c => new
                    {
                        SaladIDRef = c.Int(nullable: false),
                        CompRefID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SaladIDRef, t.CompRefID })
                .ForeignKey("pizza.Salads", t => t.SaladIDRef, cascadeDelete: true)
                .ForeignKey("pizza.Components", t => t.CompRefID, cascadeDelete: true)
                .Index(t => t.SaladIDRef)
                .Index(t => t.CompRefID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("pizza.SaladComponents", "CompRefID", "pizza.Components");
            DropForeignKey("pizza.SaladComponents", "SaladIDRef", "pizza.Salads");
            DropForeignKey("pizza.Pizzas", "PSizeID", "pizza.PizzaSizes");
            DropForeignKey("pizza.PizzaComponents", "CompIDRef", "pizza.Components");
            DropForeignKey("pizza.PizzaComponents", "PizzaIDRef", "pizza.Pizzas");
            DropIndex("pizza.SaladComponents", new[] { "CompRefID" });
            DropIndex("pizza.SaladComponents", new[] { "SaladIDRef" });
            DropIndex("pizza.PizzaComponents", new[] { "CompIDRef" });
            DropIndex("pizza.PizzaComponents", new[] { "PizzaIDRef" });
            DropIndex("pizza.Pizzas", new[] { "PSizeID" });
            DropTable("pizza.SaladComponents");
            DropTable("pizza.PizzaComponents");
            DropTable("pizza.SrcPictures");
            DropTable("pizza.Sauces");
            DropTable("pizza.Events");
            DropTable("pizza.Drinks");
            DropTable("pizza.Salads");
            DropTable("pizza.PizzaSizes");
            DropTable("pizza.Pizzas");
            DropTable("pizza.Components");
        }
    }
}
