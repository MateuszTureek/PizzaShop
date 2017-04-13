using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PizzaShop.Areas.Admin.Models.ViewModels
{
    public class DrinkViewModel
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "Nazwa")]
        [RegularExpression(@"^[a-zA-Z0-9\s-]*$", ErrorMessage = "Niepoprawna nazwa.")]
        public string Name { get; set; }

        [Range(double.MinValue,double.MaxValue)]
        [Display(Name = "Cena")]
        [RegularExpression(@"\d+(\,\d{1,3})?", ErrorMessage = "Niewprawidłowa cena. Max. 3 znaki po przecinku.")]
        public decimal Price { get; set; }

        [Required]
        [Display(Name = "Pojemność")]
        [RegularExpression(@"\d+(\,\d{1,3})?", ErrorMessage = "Niewprawidłowa pojemność. Max. 3 znaki po przecinku.")]
        public float Capacity { get; set; }
    }
}