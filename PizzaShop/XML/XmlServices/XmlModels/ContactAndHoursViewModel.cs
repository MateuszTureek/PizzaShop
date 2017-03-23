using PizzaShop.XML.Services.XmlServices.XmlModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PizzaShop.XML.XmlServices.XmlModels
{
    public class ContactAndHoursViewModel
    {
        public Contact Contact { get; set; }
        public Address Address { get; set; }
        public List<Days> WorksDays { get; set; }
    }
}