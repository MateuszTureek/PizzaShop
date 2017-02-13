using PizzaShop.Models.PizzaShopModels.Entities;
using PizzaShop.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using PizzaShop.Repositories.PizzaShopRepositories.Interfaces;

namespace PizzaShop.Repositories.PizzaShopRepositories.Classes
{
    public class PizzaSizeRepository : Repository<PizzaSize>, IPizzaSizeRepository
    {
        public PizzaSizeRepository(DbContext context) : base(context)
        {
        }
    }
}