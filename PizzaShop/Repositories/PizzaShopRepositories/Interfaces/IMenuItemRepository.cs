using PizzaShop.Models.PizzaShopModels.CMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaShop.Repositories.PizzaShopRepositories.Interfaces
{
    public interface IMenuItemRepository : IGetRepository<MenuItem>, IChangeRepository<MenuItem>
    {
    }
}
