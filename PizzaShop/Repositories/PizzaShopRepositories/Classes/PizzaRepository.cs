using PizzaShop.Models.PizzaShopModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using PizzaShop.Repositories.PizzaShopRepositories.Interfaces;

namespace PizzaShop.Repositories.PizzaShopRepositories.Classes
{
    public class PizzaRepository : Repository<Pizza>, IPizzaRepository
    {
        public PizzaRepository(DbContext context) : base(context)
        {
        }
    }
}