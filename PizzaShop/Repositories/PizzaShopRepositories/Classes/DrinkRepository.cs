using PizzaShop.Models.PizzaShopModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using PizzaShop.Repositories.PizzaShopRepositories.Interfaces;

namespace PizzaShop.Repositories.PizzaShopRepositories.Classes
{
    public class DrinkRepository : Repository<Drink>, IGetRepository<Drink>, IChangeRepository<Drink>, IDrinkRepository
    {
        public DrinkRepository(DbContext context) : base(context)
        {
        }
    }
}