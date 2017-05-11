using PizzaShop.Models.PizzaShopModels.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PizzaShop.Areas.Admin.Models.ViewModels
{
    public class PizzaViewModel
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "Nazwa")]
        [StringLength(50, ErrorMessage = "Max. liczba znaków {1}.")]
        [RegularExpression(@"^[a-zA-ZąćęłńóśźżĄĘŁŃÓŚŹŻ\s]*$", ErrorMessage = "Pole powinno składać się z liter lub cyfr.")]
        public string Name { get; set; }
        
        [Required]
        [RegularExpression(@"\d+(\,\d{1,3})?", ErrorMessage = "Niewprawidłowa cena. Max. 3 znaki po przecinku.")]
        public decimal? PriceForSmall { get; set; }

        [Required]
        [Display(Name = "Cena za średnią")]
        [RegularExpression(@"\d+(\,\d{1,3})?", ErrorMessage = "Niewprawidłowa cena. Max. 3 znaki po przecinku.")]
        public decimal? PriceForMedium { get; set; }

        [Required]
        [Display(Name = "Cena za dużą")]
        [RegularExpression(@"\d+(\,\d{1,3})?", ErrorMessage = "Niewprawidłowa cena. Max. 3 znaki po przecinku.")]
        public decimal? PriceForLarge { get; set; }

        [Display(Name = "Składniki")]
        public List<int> Components { get; set; }
    }
}