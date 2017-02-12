using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PizzaShop.Models.PizzaShopModels.Entities
{
    public class PizzaSizePrice
    {
        public int PizzaID { get; set; }
        public virtual Pizza Pizza { get; set; }

        public int PizzaSizeID { get; set; }
        public virtual PizzaSize PizzaSize { get; set; }

        public decimal Price { get; set; }
    }
}