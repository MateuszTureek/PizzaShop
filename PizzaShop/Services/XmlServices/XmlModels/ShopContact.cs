using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace PizzaShop.Services.XmlServices.XmlModels
{
    [Serializable]
    public class Contact
    {
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
    }

    [Serializable]
    public class Address
    {
        public string Email { get; set; }
        public string InformationContact { get; set; }
        public string DeliveryContact { get; set; }
    }

    [Serializable]
    [XmlRoot("ShopContact"),XmlType("ShopContact")]
    public class ShopContact
    {
        public Contact Contact { get; set; }
        public Address Address { get; set; }
    }
}