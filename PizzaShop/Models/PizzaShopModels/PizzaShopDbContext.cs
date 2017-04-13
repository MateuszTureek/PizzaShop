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
        public virtual DbSet<Pizza> Pizzas { get; set; }
        public virtual DbSet<PizzaSize> PizzaSizes { get; set; }
        public virtual DbSet<Salad> Salads { get; set; }
        public virtual DbSet<Sauce> Sauces { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("pizza");
            modelBuilder.Entity<Drink>().HasKey(k => k.ID);
            modelBuilder.Entity<Drink>().Property(p => p.Name).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<Drink>().Property(p => p.Price).HasPrecision(5, 2).IsRequired();
            modelBuilder.Entity<Drink>().Property(p => p.Capacity).IsRequired();
            modelBuilder.Entity<Sauce>().HasKey(k => k.ID);
            modelBuilder.Entity<Sauce>().Property(p => p.Name).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<Sauce>().Property(p => p.Price).HasPrecision(5, 2).IsRequired();
            modelBuilder.Entity<Pizza>().HasKey(k => k.ID);
            modelBuilder.Entity<Pizza>().Property(p => p.Name).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<PizzaSize>().HasKey(k => k.ID);
            modelBuilder.Entity<PizzaSize>().Property(p => p.Size).HasMaxLength(5).IsRequired();
            modelBuilder.Entity<Component>().HasKey(k => k.ID);
            modelBuilder.Entity<Component>().Property(p => p.Name).HasMaxLength(30).IsRequired();
            modelBuilder.Entity<Salad>().HasKey(k => k.ID);
            modelBuilder.Entity<Salad>().Property(p => p.Name).HasMaxLength(30).IsRequired();
            modelBuilder.Entity<Salad>().Property(p => p.Price).HasPrecision(5, 2).IsRequired();
            modelBuilder.Entity<PizzaSizePrice>().HasKey(k => new { k.PizzaID, k.PizzaSizeID });
            modelBuilder.Entity<PizzaSizePrice>().Property(p => p.Price).IsRequired();

            modelBuilder.Entity<Pizza>().HasMany(m => m.PizzaSizePrices).WithRequired(r => r.Pizza);
            modelBuilder.Entity<PizzaSize>().HasMany(m => m.PizzaSizePrices).WithRequired(r => r.PizzaSize);
            modelBuilder.Entity<Pizza>().HasMany(h => h.Components).WithMany(w => w.Pizzas).Map(m =>
            {
                m.MapLeftKey("PizzaIDRef");
                m.MapRightKey("CompIDRef");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}