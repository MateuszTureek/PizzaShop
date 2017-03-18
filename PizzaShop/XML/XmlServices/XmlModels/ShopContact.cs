using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace PizzaShop.XML.Services.XmlServices.XmlModels
{
    [Serializable]
    public class Contact
    {
        [Display(Name="Kod pocztowy")]
        public string PostalCode { get; set; }
        [Display(Name ="Miasto")]
        public string City { get; set; }
        [Display(Name ="Ulica")]
        public string Street { get; set; }
    }

    [Serializable]
    public class Address
    {
        [Display(Name ="E-Mail")]
        public string Email { get; set; }
        [Display(Name ="Telefon kontaktowy")]
        public string InformationContact { get; set; }
        [Display(Name ="Telefon zamówienia")]
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