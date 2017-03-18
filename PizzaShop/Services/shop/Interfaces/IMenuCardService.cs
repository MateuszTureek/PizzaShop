using PizzaShop.Models.PizzaShopModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaShop.Services.shop.Interfaces
{
    public interface IMenuCardService
    {
        List<Drink> GetAllDrinks();
        List<Pizza> GetAllPizzas();
        List<Salad> GetAllSalads();
        List<Sauce> GetAllSauces();
    }
}
