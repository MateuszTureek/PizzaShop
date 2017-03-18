using System;
using System.Data.Entity;
using System.Collections.Generic;
using PizzaShop.Repository;
using PizzaShop.Models.PizzaShopModels.CMS;
using PizzaShop.Repositories.CMS.Interfaces;
using System.Linq;

namespace PizzaShop.Repositories.CMS.Classes
{
    public class InformationItemRepository : Repository<int, InformationItem>, IInformationItemRepository
    {
        public InformationItemRepository(DbContext context) : base(context)
        {
        }

        public List<InformationItem> GetByPosition()
        {
            var result= _dbSet.OrderBy(o => o.Position).ToList();
            return result;
        }
    }
}