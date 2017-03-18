using PizzaShop.Models.PizzaShopModels.Entities;
using PizzaShop.Repositories.Shop.Interfaces;
using PizzaShop.Services.shop.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PizzaShop.Services.shop.Classes
{
    public class MenuCardService : IMenuCardService
    {
        readonly IDrinkRepository _drinkRepository;
        readonly IPizzaRepository _pizzaRepository;
        readonly ISaladRepository _saladRepository;
        readonly ISauceRepository _sauceRepository;

        public MenuCardService(IDrinkRepository drinkRepository,
                               IPizzaRepository pizzaRepository,
                               ISaladRepository saladRepository,
                               ISauceRepository sauceRepository)
        {
            _drinkRepository = drinkRepository;
            _pizzaRepository = pizzaRepository;
            _saladRepository = saladRepository;
            _sauceRepository = sauceRepository;
        }

        public List<Drink> GetAllDrinks()
        {
            var result = _drinkRepository.GetAll().ToList();
            return result;
        }

        public List<Pizza> GetAllPizzas()
        {
            var result = _pizzaRepository.GetAll().ToList();
            return result;
        }

        public List<Salad> GetAllSalads()
        {
            var result = _saladRepository.GetAll().ToList();
            return result;
        }

        public List<Sauce> GetAllSauces()
        {
            var result = _sauceRepository.GetAll().ToList();
            return result;
        }
    }
}