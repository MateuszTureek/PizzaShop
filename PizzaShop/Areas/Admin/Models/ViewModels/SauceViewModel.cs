using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PizzaShop.Areas.Admin.Models.ViewModels
{
    public class SauceViewModel
    {
        public int ID { get; set; }

        [Required]
        [Display(Name="Nazwa")]
        [RegularExpression(@"^[a-zA-Z0-9\s-]*$", ErrorMessage ="Niepoprawna nazwa.")]
        public string Name { get; set; }

        [Required]
        [Display(Name="Cena")]
        [RegularExpression(@"\d+(\,\d{1,3})?", ErrorMessage = "Niewprawidłowa cena. Max. 3 znaki po przecinku.")]
        public decimal? Price { get; set; }
    }
}