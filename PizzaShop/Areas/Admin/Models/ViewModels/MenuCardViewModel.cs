using PizzaShop.Models.PizzaShopModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PizzaShop.Areas.Admin.Models.ViewModels
{
    public class MenuCardViewModel
    {
        public List<Drink> Drinks { get; set; }

        public List<Pizza> Pizzas { get; set; }

        public List<Salad> Salads { get; set; }

        public List<Sauce> Sauces { get; set; }
    }
}