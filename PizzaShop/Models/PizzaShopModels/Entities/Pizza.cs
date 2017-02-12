using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PizzaShop.Models.PizzaShopModels.Entities
{
    public class Pizza
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Component> Components { get; set; }

        public virtual ICollection<PizzaSizePrice> PizzaSizePrices { get; set; }
    }
}