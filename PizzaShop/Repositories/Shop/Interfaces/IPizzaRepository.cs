using PizzaShop.Models.PizzaShopModels.Entities;
using PizzaShop.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaShop.Repositories.Shop.Interfaces
{
    public interface IPizzaRepository : IRepository<int?, Pizza>
    {
        /// <summary>
        /// Get pizza's component by pizza id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<Component> GetPizzaComponentsById(int? id);
        /// <summary>
        /// Get pizza's pizzaSizePrices.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<PizzaSizePrice> GetPizzaSizePrice(int? id);
    }
}
