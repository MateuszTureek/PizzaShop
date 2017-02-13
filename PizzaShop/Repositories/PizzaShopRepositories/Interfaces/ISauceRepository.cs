using PizzaShop.Models.PizzaShopModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaShop.Repositories.PizzaShopRepositories.Interfaces
{
    public interface ISauceRepository : IGetRepository<Sauce>, IChangeRepository<Sauce>
    {
    }
}
