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
    public class PizzaRepository : Repository<int,Pizza>, IPizzaRepository
    {
        public PizzaRepository(DbContext context) : base(context)
        {
        }

        public List<Component> GetPizzaComponentsById(int? id)
        {
            var result = _dbSet.Find(id).Components.ToList();
            return result;
        }

        public List<PizzaSizePrice> GetPizzaSizePrice(int? id)
        {
            var result = _dbSet.Find(id).PizzaSizePrices.OrderBy(s => s.Price).ToList();
            return result;
        }
    }
}