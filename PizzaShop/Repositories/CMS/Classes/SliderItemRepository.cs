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
    public class SliderItemRepository : Repository<int?, SliderItem>, ISliderItemRepository
    {
        public SliderItemRepository(DbContext context) : base(context)
        {
        }

        public List<SliderItem> GetByPosition()
        {
            var result = _dbSet.OrderBy(o => o.Position).ToList();
            return result;
        }
    }
}