using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PizzaShop.Services.Xml
{
    public static class GlobalXmlManager
    {
        private static string _contactFileName = "ShopContact";
        private static string _openingHoursFileName = "OpeningHours";

        public static string ContactFileName { get { return _contactFileName; } }
        public static string OpeningHoursFileName { get { return _openingHoursFileName; } }
    }
}