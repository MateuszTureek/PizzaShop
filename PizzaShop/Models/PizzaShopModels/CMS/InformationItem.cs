using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PizzaShop.Models.PizzaShopModels.CMS
{
    public class InformationItem
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string PictureUrl { get; set; }
        public string Content { get; set; }
    }
}