using PizzaShop.Models.PizzaShopModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using PizzaShop.Repository;
using PizzaShop.Repositories.Shop.Interfaces;

namespace PizzaShop.Repositories.Shop.Classes
{
    public class DrinkRepository : Repository<int?, Drink>, IDrinkRepository
    {
        public DrinkRepository(DbContext context) : base(context)
        {
        }
    }
}