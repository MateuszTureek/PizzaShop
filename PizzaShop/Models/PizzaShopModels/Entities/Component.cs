using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PizzaShop.Models.PizzaShopModels.Entities
{
    public class Component
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Pizza> Pizzas { get; set; }
    }
}