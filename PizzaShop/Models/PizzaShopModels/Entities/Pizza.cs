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
        public decimal Price { get; set; }

        public int PSizeID { get; set; }
        public virtual PizzaSize PizzaSize { get; set; }

        public virtual ICollection<Component> Components { get; set; }
    }
}