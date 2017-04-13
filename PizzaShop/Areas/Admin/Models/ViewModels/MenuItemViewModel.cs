using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PizzaShop.Areas.Admin.Models.ViewModels
{
    public class MenuItemViewModel
    {
        [RegularExpression("^[0-9]*$", ErrorMessage = "Pole {0} powinno skłądać się z samych cyfr.")]
        public int ID { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage ="Pole {0} powinno skłądać się z samych liter.")]
        [Display(Name = "Tytuł")]
        [MaxLength(30, ErrorMessage = "Max. liczba znaków {1}")]
        public string Title { get; set; }

        [Required]
        [Display(Name ="Nazwa akcji")]
        [RegularExpression("^[a-zA-Z]*$", ErrorMessage = "Pole {0} powinno skłądać się z samych liter.")]
        [MaxLength(50, ErrorMessage = "Max. liczba znaków {1}")]
        public string ActionName { get; set; }

        [Required]
        [Display(Name ="Nazwa kontrolera")]
        [RegularExpression("^[a-zA-Z]*$", ErrorMessage = "Pole {0} powinno skłądać się z samych liter.")]
        [MaxLength(30, ErrorMessage ="Max. liczba znaków {1}")]
        public string ControllerName { get; set; }

        [Range(1,100)]
        [Display(Name ="Pozycja")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Pole {0} powinno skłądać się z samych cyfr.")]
        public int Position { get; set; }
    }
}