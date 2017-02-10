using PizzaShop.Models.PizzaShopModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using PizzaShop.Repositories.PizzaShopRepositories.Interfaces;

namespace PizzaShop.Repositories.PizzaShopRepositories.Classes
{
    public class SauceRepository : Repository<Sauce>, IGetRepository<Sauce>, IChangeRepository<Sauce>, ISauceRepository
    {
        public SauceRepository(DbContext context) : base(context)
        {
        }
    }
}