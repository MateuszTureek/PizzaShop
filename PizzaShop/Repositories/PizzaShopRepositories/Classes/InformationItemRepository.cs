using PizzaShop.Models.PizzaShopModels.CMS;
using PizzaShop.Repositories.PizzaShopRepositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace PizzaShop.Repositories.PizzaShopRepositories.Classes
{
    public class InformationItemRepository : Repository<InformationItem>, IInformationItemRepository
    {
        public InformationItemRepository(DbContext context) : base(context)
        {
        }
    }
}