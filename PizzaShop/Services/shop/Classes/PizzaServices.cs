using Ninject;
using PizzaShop.Models.PizzaShopModels.Entities;
using PizzaShop.Repositories.Shop.Interfaces;
using PizzaShop.Services.shop.Interfaces;
using PizzaShop.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PizzaShop.Services.shop.Classes
{
    public class PizzaService : IPizzaService
    {
        readonly IUnitOfWork _unityOfWork;
        readonly IPizzaRepository _pizzaRepository;
        readonly IComponentRepository _componentRepository;
        readonly IPizzaSizePriceRepository _pizzaSizePriceRepository;
        readonly IPizzaSizeRepository _pizzaSizeRepsitory;

        public PizzaService([Named("shopUnit")]IUnitOfWork unityOfWork,
                            IPizzaRepository pizzaRepository,
                            IComponentRepository componentRepository,
                            IPizzaSizePriceRepository pizzaSizePriceRepository,
                            IPizzaSizeRepository pizzaSizeRepsitory)
        {
            _unityOfWork = unityOfWork;
            _pizzaRepository = pizzaRepository;
            _componentRepository = componentRepository;
            _pizzaSizePriceRepository = pizzaSizePriceRepository;
            _pizzaSizeRepsitory = pizzaSizeRepsitory;
        }

        public void CreatePizza(Pizza pizza)
        {
            _pizzaRepository.Insert(pizza);
        }

        public void CreatePizzaSizePrice(PizzaSizePrice pizzaSizePrice)
        {
            _pizzaSizePriceRepository.Insert(pizzaSizePrice);
        }

        public void DeletePizza(Pizza pizza)
        {
            _pizzaRepository.Delete(pizza);
        }

        public Pizza GetPizza(int id)
        {
            var result = _pizzaRepository.Get(id);
            return result;
        }

        public List<Pizza> GetAllPizzas()
        {
            var result = _pizzaRepository.GetAll().ToList();
            return result;
        }

        public List<Component> GetAllComponents()
        {
            var result = _componentRepository.GetAll().ToList();
            return result;
        }

        public void SavePizza()
        {
            _unityOfWork.Commit();
        }

        public List<Component> FindComponents(List<int> ids)
        {
            if (ids != null && ids.Count != 0)
            {
                var result = new List<Component>();
                for (var i = 0; i < ids.Count; ++i)
                    result.Add(_componentRepository.Get(ids[i]));
                return result;
            }
            return null;
        }

        public List<Component> FindNotPizzaComponent(int? id)
        {
            var components = GetAllComponents().AsQueryable();
            var pizzaComponents = FindComponents(id).AsQueryable();
            var result = components.Where(w => pizzaComponents.All(a => a.ID != w.ID)).ToList();
            return result;
        }

        public List<Component> FindComponents(int? id)
        {
            var result = _pizzaRepository.GetPizzaComponentsById(id);
            return result;
        }

        public List<PizzaSize> GetAllPizzaSizes()
        {
            var result = _pizzaSizeRepsitory.GetBySize();
            return result;
        }

        public List<PizzaSizePrice> GetPizzaSizePrices(int? id)
        {
            var result = _pizzaRepository.GetPizzaSizePrice(id);
            return result;
        }

        public void CreatePizzaSizePrice(List<PizzaSizePrice> pizzaSizePrice)
        {
            _pizzaSizePriceRepository.Insert(pizzaSizePrice);
        }

        public void UpdatePizza(Pizza pizza)
        {
            _pizzaRepository.Update(pizza);
        }
    }
}