using PizzaShop.Models.PizzaShopModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaShop.Services.shop.Interfaces
{
    public interface IPizzaService
    {
        /// <summary>
        /// Create new pizza.
        /// </summary>
        /// <param name="pizza"></param>
        void CreatePizza(Pizza pizza);
        /// <summary>
        /// Create new pizzaSizePirce for pizza.
        /// </summary>
        /// <param name="pizzaSizePrice"></param>
        void CreatePizzaSizePrice(PizzaSizePrice pizzaSizePrice);
        /// <summary>
        /// Create new several pizzaSizePrice for pizza.
        /// </summary>
        /// <param name="pizzaSizePrice"></param>
        void CreatePizzaSizePrice(List<PizzaSizePrice> pizzaSizePrice);
        /// <summary>
        /// Delete pizza.
        /// </summary>
        /// <param name="pizza"></param>
        void DeletePizza(Pizza pizza);
        /// <summary>
        /// Get pizza by pizza id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Pizza GetPizza(int id);
        /// <summary>
        /// Get all pizzas.
        /// </summary>
        /// <returns></returns>
        List<Pizza> GetAllPizzas();
        /// <summary>
        /// Get all pizzaSizes.
        /// </summary>
        /// <returns></returns>
        List<PizzaSize> GetAllPizzaSizes();
        /// <summary>
        /// Get pizza's pizzaSizePrices.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<PizzaSizePrice> GetPizzaSizePrices(int? id);
        /// <summary>
        /// Get all components.
        /// </summary>
        /// <returns></returns>
        List<Component> GetAllComponents();
        /// <summary>
        /// Get components by list component id.
        /// </summary>
        /// <param name="ids"></param>
        /// <returns>List component.</returns>
        List<Component> FindComponents(List<int> ids);
        /// <summary>
        /// Get pizza's components.
        /// </summary>
        /// <param name="pizza"></param>
        /// <returns></returns>
        List<Component> FindComponents(int? id);
        /// <summary>
        /// Get not pizza's components.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<Component> FindNotPizzaComponent(int? id);
        /// <summary>
        /// Update pizza.
        /// </summary>
        /// <param name="pizza"></param>
        void UpdatePizza(Pizza pizza);
        /// <summary>
        /// Save whole pizza.
        /// </summary>
        void SavePizza();
    }
}
