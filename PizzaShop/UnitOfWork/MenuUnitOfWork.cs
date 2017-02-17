using Ninject;
using PizzaShop.Models.PizzaShopModels.Entities;
using PizzaShop.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PizzaShop.UnitOfWork
{
    public class MenuUnitOfWork : UnitOfWork
    {
        public MenuUnitOfWork(DbContext conetxt) : base(conetxt)
        {
        }

        [Inject]
        public IGetRepository<Pizza> PizzaRepository { get; set; }

        [Inject]
        public IGetRepository<Salad> SaladRepository { get; set; }

        [Inject]
        public IGetRepository<Sauce> SauceRepository { get; set; }

        [Inject]
        public IGetRepository<Drink> DrinkRepository { get; set; }
    }
}