using PizzaShop.Models.PizzaShopModels.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PizzaShop.Models.PizzaShopModels
{
    public class PizzaShopDbContext : DbContext
    {
        public PizzaShopDbContext()
            : base("name=PizzaShopConnection") { }

        public virtual DbSet<Component> Components { get; set; }
        public virtual DbSet<Drink> Drinks { get; set; }
        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<Pizza> Pizzas { get; set; }
        public virtual DbSet<PizzaSize> PizzaSizes { get; set; }
        public virtual DbSet<Salad> Salads { get; set; }
        public virtual DbSet<Sauce> Sauces { get; set; }
        public virtual DbSet<SrcPicture> SrcPictures { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("pizza");
            modelBuilder.Entity<Event>().HasKey(k => k.ID);
            modelBuilder.Entity<Event>().Property(p => p.Title).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<Event>().Property(p => p.Content).HasMaxLength(250).IsRequired();
            modelBuilder.Entity<Drink>().HasKey(k => k.ID);
            modelBuilder.Entity<Drink>().Property(p => p.Name).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<Drink>().Property(p => p.Price).HasPrecision(5, 2).IsRequired();
            modelBuilder.Entity<Drink>().Property(p => p.Capacity).IsRequired();
            modelBuilder.Entity<SrcPicture>().HasKey(k => k.ID);
            modelBuilder.Entity<SrcPicture>().Property(p => p.Src).IsMaxLength().IsRequired();
            modelBuilder.Entity<Sauce>().HasKey(k => k.ID);
            modelBuilder.Entity<Sauce>().Property(p => p.Name).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<Sauce>().Property(p => p.Price).HasPrecision(5, 2).IsRequired();
            modelBuilder.Entity<Pizza>().HasKey(k => k.ID);
            modelBuilder.Entity<Pizza>().Property(p => p.Name).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<Pizza>().Property(p => p.Price).HasPrecision(5, 2).IsRequired();
            modelBuilder.Entity<PizzaSize>().HasKey(k => k.ID);
            modelBuilder.Entity<PizzaSize>().Property(p => p.Size).HasMaxLength(5).IsRequired();
            modelBuilder.Entity<Component>().HasKey(k => k.ID);
            modelBuilder.Entity<Component>().Property(p => p.Name).HasMaxLength(30).IsRequired();
            modelBuilder.Entity<Salad>().HasKey(k => k.ID);
            modelBuilder.Entity<Salad>().Property(p => p.Name).HasMaxLength(30).IsRequired();
            modelBuilder.Entity<Salad>().Property(p => p.Price).HasPrecision(5, 3).IsRequired();

            modelBuilder.Entity<Pizza>().HasRequired(r => r.PizzaSize).WithMany(m => m.Pizzas).HasForeignKey(f => f.PSizeID);
            modelBuilder.Entity<Pizza>().HasMany(h => h.Components).WithMany(w => w.Pizzas).Map(m =>
            {
                m.MapLeftKey("PizzaIDRef");
                m.MapRightKey("CompIDRef");
            });
            modelBuilder.Entity<Salad>().HasMany(h => h.Components).WithMany(w => w.Salads).Map(m =>
            {
                m.MapLeftKey("SaladIDRef");
                m.MapRightKey("CompRefID");
            });

            base.OnModelCreating(modelBuilder);
        }

    }
}