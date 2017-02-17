using PizzaShop.Models.PizzaShopModels.CMS;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PizzaShop.Models.PizzaShopModels
{
    public class CmsDbContext : DbContext
    {
        public CmsDbContext()
            : base("name=CmsDbConnection")
        { }

        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<GalleryItem> GalleryItems { get; set; }
        public virtual DbSet<InformationItem> InformationItems { get; set; }
        public virtual DbSet<MenuItem> MenuItems { get; set; }
        public virtual DbSet<New> News { get; set; }
        public virtual DbSet<SliderItem> SliderItems { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>().HasKey(k => k.ID);
            modelBuilder.Entity<Event>().Property(p => p.AddedDate).IsRequired();
            modelBuilder.Entity<Event>().Property(p => p.Title).HasMaxLength(30).IsRequired();
            modelBuilder.Entity<GalleryItem>().HasKey(k => k.ID);
            modelBuilder.Entity<GalleryItem>().Property(p => p.PictureUrl).IsMaxLength();
            modelBuilder.Entity<GalleryItem>().Property(p => p.Position).IsRequired();
            modelBuilder.Entity<InformationItem>().HasKey(k => k.ID);
            modelBuilder.Entity<InformationItem>().Property(p => p.Content).HasMaxLength(250).IsRequired();
            modelBuilder.Entity<InformationItem>().Property(p => p.PictureUrl).IsMaxLength().IsRequired();
            modelBuilder.Entity<InformationItem>().Property(p => p.Title).HasMaxLength(30).IsRequired();
            modelBuilder.Entity<MenuItem>().HasKey(k => k.ID);
            modelBuilder.Entity<MenuItem>().Property(p => p.ActionName).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<MenuItem>().Property(p => p.Position).IsRequired();
            modelBuilder.Entity<MenuItem>().Property(p => p.Title).HasMaxLength(30).IsRequired();
            modelBuilder.Entity<MenuItem>().Property(p => p.ControllerName).HasMaxLength(30).IsRequired();
            modelBuilder.Entity<New>().HasKey(k => k.ID);
            modelBuilder.Entity<New>().Property(p => p.AddedDate).IsRequired();
            modelBuilder.Entity<New>().Property(p => p.Content).HasMaxLength(250).IsRequired();
            modelBuilder.Entity<New>().Property(p => p.Position).IsRequired();
            modelBuilder.Entity<New>().Property(p => p.Title).HasMaxLength(30).IsRequired();
            modelBuilder.Entity<SliderItem>().HasKey(k => k.ID);
            modelBuilder.Entity<SliderItem>().Property(p => p.PictureUrl).IsMaxLength().IsRequired();
            modelBuilder.Entity<SliderItem>().Property(p => p.Position).IsRequired();
            modelBuilder.Entity<SliderItem>().Property(p => p.ShortDescription).HasMaxLength(30).IsOptional();

            base.OnModelCreating(modelBuilder);
        }
    }
}