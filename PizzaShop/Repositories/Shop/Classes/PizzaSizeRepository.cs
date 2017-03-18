using PizzaShop.Models.PizzaShopModels.Entities;
using PizzaShop.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using PizzaShop.Repository;
using PizzaShop.Repositories.Shop.Interfaces;

namespace PizzaShop.Repositories.Shop.Classes
{
    public class PizzaSizeRepository : Repository<int, PizzaSize>, IPizzaSizeRepository
    {
        public PizzaSizeRepository(DbContext context) : base(context)
        {
        }

        public List<PizzaSize> GetBySize()
        {
            var result = _dbSet.OrderBy(o => o.Size).ToList();
            return result;
        }
    }
}