namespace PizzaShop.Migrations.Cms
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 30),
                        Content = c.String(),
                        AddedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.GalleryItems",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Position = c.Int(nullable: false),
                        PictureUrl = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.InformationItems",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 30),
                        PictureUrl = c.String(nullable: false),
                        Content = c.String(nullable: false, maxLength: 250),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.MenuItems",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 30),
                        ActionName = c.String(nullable: false, maxLength: 50),
                        Position = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.News",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 30),
                        Content = c.String(nullable: false, maxLength: 250),
                        Position = c.Int(nullable: false),
                        AddedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.SliderItems",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Position = c.Int(nullable: false),
                        PictureUrl = c.String(nullable: false),
                        ShortDescription = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SliderItems");
            DropTable("dbo.News");
            DropTable("dbo.MenuItems");
            DropTable("dbo.InformationItems");
            DropTable("dbo.GalleryItems");
            DropTable("dbo.Events");
        }
    }
}
