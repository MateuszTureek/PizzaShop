using PizzaShop.Models.PizzaShopModels.CMS;
using PizzaShop.Repositories.PizzaShopRepositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace PizzaShop.Repositories.PizzaShopRepositories.Classes
{
    public class MenuItemRepository : Repository<MenuItem>, IMenuItemRepository
    {
        public MenuItemRepository(DbContext context) : base(context)
        {
        }
    }
}