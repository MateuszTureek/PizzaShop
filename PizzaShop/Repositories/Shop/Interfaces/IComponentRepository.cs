using PizzaShop.Models.PizzaShopModels.Entities;
using PizzaShop.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaShop.Repositories.Shop.Interfaces
{
    public interface IComponentRepository : IRepository<int, Component>
    {
    }
}
