using PizzaShop.Models.PizzaShopModels.CMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaShop.Repositories.PizzaShopRepositories.Interfaces
{
    public interface IEventRepository : IGetRepository<Event>, IChangeRepository<Event>
    {
    }
}
