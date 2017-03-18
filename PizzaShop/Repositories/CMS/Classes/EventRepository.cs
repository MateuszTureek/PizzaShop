using PizzaShop.Models.PizzaShopModels.CMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using PizzaShop.Repository;
using PizzaShop.Repositories.CMS.Interfaces;

namespace PizzaShop.Repositories.CMS.Classes
{
    public class EventRepository : Repository<int, Event>, IEventRepository
    {
        public EventRepository(DbContext context) : base(context)
        {
        }

        public List<Event> GetByAddedDate()
        {
            var result = _dbSet.OrderBy(o => o.AddedDate).ToList();
            return result;
        }
    }
}