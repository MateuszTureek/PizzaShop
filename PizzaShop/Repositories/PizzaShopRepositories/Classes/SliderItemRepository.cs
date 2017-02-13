using PizzaShop.Models.PizzaShopModels.CMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace PizzaShop.Repositories.PizzaShopRepositories.Classes
{
    public class SliderItemRepository : Repository<SliderItem>
    {
        public SliderItemRepository(DbContext context) : base(context)
        {
        }
    }
}