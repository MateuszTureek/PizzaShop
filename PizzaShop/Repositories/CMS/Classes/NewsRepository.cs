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
    public class NewsRepository : Repository<int, News>, INewsRepository
    {
        public NewsRepository(DbContext context) : base(context)
        {
        }

        public List<News> GetByAddedDate()
        {
            var result = _dbSet.OrderBy(o => o.AddedDate).ToList();
            return result;
        }
    }
}