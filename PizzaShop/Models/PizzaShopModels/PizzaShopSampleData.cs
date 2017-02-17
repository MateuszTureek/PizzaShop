using PizzaShop.Models.PizzaShopModels.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PizzaShop.Models.PizzaShopModels
{
    public class PizzaShopSampleData : DropCreateDatabaseIfModelChanges<PizzaShopDbContext>
    {
        private void AddToContext<T>(List<T> items,PizzaShopDbContext context) where T : class
        {
            DbSet<T> dbSet = context.Set<T>();
            foreach (var i in items)
                dbSet.Add(i);
        }

        protected override void Seed(PizzaShopDbContext context)
        {
            List<Component> components = new List<Component>()
            {
                new Component() { Name="Ser" },
                new Component() { Name="Salami pepperoni" },
                new Component() { Name="Kurczak" },
                new Component() { Name="Szynka" },
                new Component() { Name="Pieczarki" },
                new Component() { Name="Ananas" },
                new Component() { Name="Papryka" },
                new Component() { Name="Czosnek" },
                new Component() { Name="Sałata lodowa" },
                new Component() { Name="Pomidory" },
                new Component() { Name="Ogórki" },
                new Component() { Name="Czarne oliwki" },
                new Component() { Name="Sos" },
                new Component() { Name="Kukurydza" },
                new Component() { Name="Czerwona cebula" }
            };
            AddToContext<Component>(components, context);
            List<Drink> drinks = new List<Drink>()
            {
                new Drink() { Name="Mirinda",Price=5.00M,Capacity=0.5f },
                new Drink() { Name="7Up",Price=5.00M,Capacity=0.5f },
                new Drink() { Name="Woda gazowana",Price=3.00M,Capacity=0.5f },
                new Drink() { Name="Woda niegazowana",Price=3.00M,Capacity=0.5f },
                new Drink() { Name="Sok pomarańczowy",Price=4.00M, Capacity=0.33f }
            };
            AddToContext<Drink>(drinks, context);
            List<PizzaSize> pizzaSizes = new List<PizzaSize>()
            {
                new PizzaSize() { Size="24cm" },
                new PizzaSize() { Size="32cm" },
                new PizzaSize() { Size="45cm" }
            };
            AddToContext<PizzaSize>(pizzaSizes, context);
            List<Pizza> pizzas = new List<Pizza>()
            {
                new Pizza { Name="Margarita",Components=new List<Component>() { components[0] }  },
                new Pizza { Name="Salame",Components=new List<Component>() { components[0], components[1] }  },
                new Pizza { Name="Pollo",Components=new List<Component>() { components[0],components[2] }  }
            };
            AddToContext<Pizza>(pizzas, context);
            List<PizzaSizePrice> pizzaSizePrices = new List<PizzaSizePrice>()
            {
                new PizzaSizePrice() { Pizza=pizzas[0],PizzaSize=pizzaSizes[0],Price=9.00M },
                new PizzaSizePrice() { Pizza=pizzas[1],PizzaSize=pizzaSizes[0],Price=11.50M },
                new PizzaSizePrice() { Pizza=pizzas[2],PizzaSize=pizzaSizes[0],Price=11.50M },
                new PizzaSizePrice() { Pizza=pizzas[0], PizzaSize=pizzaSizes[1],Price=11.00M },
                new PizzaSizePrice() { Pizza=pizzas[1], PizzaSize=pizzaSizes[1],Price=14.00M },
                new PizzaSizePrice() { Pizza=pizzas[2], PizzaSize=pizzaSizes[1],Price=14.00M },
                new PizzaSizePrice() { Pizza=pizzas[0], PizzaSize=pizzaSizes[2],Price=21.00M },
                new PizzaSizePrice() { Pizza=pizzas[1], PizzaSize=pizzaSizes[2],Price=25.00M },
                new PizzaSizePrice() { Pizza=pizzas[2], PizzaSize=pizzaSizes[2],Price=25.00M }
            };
            AddToContext<PizzaSizePrice>(pizzaSizePrices, context);
            List<Salad> salads = new List<Salad>()
            {
                new Salad() { Name="Greco",Price=14.00M,Components=new List<Component>()
                { components[8],components[9],components[10],components[11],components[12],components[0] } },
                new Salad() { Name="Pollo",Price=16.00M, Components=new List<Component>()
                { components[8],components[9],components[10],components[11], components[2],components[5],components[12] } },
                new Salad() { Name="Mexico",Price=16.00M,Components=new List<Component>()
                { components[8],components[9],components[2],components[14],components[12],components[6] } }
            };
            AddToContext<Salad>(salads, context);
            List<Sauce> sauces = new List<Sauce>()
            {
                new Sauce() { Name="Ostry",Price=2.00M },
                new Sauce() { Name="Czosnkowo-ziołowy",Price=2.00M },
                new Sauce() { Name="Pomodorowy",Price=3.00M }
            };
            AddToContext<Sauce>(sauces, context);

            base.Seed(context);
        }
    }
}