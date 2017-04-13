using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PizzaShop.Services.Xml.XmlModels
{
    public class ShopContactViewModel
    {
        [Required]
        [Display(Name = "Kod pocztowy")]
        [DataType(DataType.PostalCode)]
        public string PostalCode { get; set; }

        [Required]
        [Display(Name = "Miasto")]
        public string City { get; set; }

        [Required]
        [Display(Name = "Ulica")]
        public string Street { get; set; }

        [Required]
        [Display(Name = "E-Mail")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Telefon kontaktowy")]
        [RegularExpression(@"^[0-9\s]*$")]
        public string InformationContact { get; set; }

        [Required]
        [Display(Name = "Telefon zamówienia")]
        [RegularExpression(@"^[0-9\s]*$")]
        public string DeliveryContact { get; set; }
    }
}