using PizzaShop.Models.PizzaShopModels.CMS;
using PizzaShop.Repositories.PizzaShopRepositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace PizzaShop.Repositories.PizzaShopRepositories.Classes
{
    public class EventRepository : Repository<Event>, IEventRepository
    {
        public EventRepository(DbContext context) : base(context)
        {
        }
    }
}