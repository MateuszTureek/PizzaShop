using PizzaShop.Models.PizzaShopModels.Entities;
using PizzaShop.Repositories.PizzaShopRepositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace PizzaShop.Repositories.PizzaShopRepositories.Classes
{
    public class ComponentRepository : Repository<Component>, IGetRepository<Component>, IChangeRepository<Component>, IComponentRepository
    {
        public ComponentRepository(DbContext context) : base(context)
        {
        }
    }
}