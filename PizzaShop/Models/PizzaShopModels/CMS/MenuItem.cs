using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PizzaShop.Models.PizzaShopModels.CMS
{
    public class MenuItem
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public int Position { get; set; }
    }
}