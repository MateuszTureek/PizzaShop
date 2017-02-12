using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PizzaShop.Models.PizzaShopModels.CMS
{
    public class SliderItem
    {
        public int ID { get; set; }
        public int Position { get; set; }
        public string PictureUrl { get; set; }
        public string ShortDescription { get; set; }
    }
}