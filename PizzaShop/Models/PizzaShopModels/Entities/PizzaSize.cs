using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PizzaShop.Models.PizzaShopModels.Entities
{
    public class PizzaSize
    {
        public int ID { get; set; }
        public string Size { get; set; }

        public virtual ICollection<PizzaSizePrice> PizzaSizePrices { get; set; }
    }
}