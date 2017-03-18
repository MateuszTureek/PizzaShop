using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PizzaShop.Areas.Admin.Models.ViewModels
{
    public class SliderItemViewModel
    {
        public int ID { get; set; }

        [Required]
        [Display(Name="Pozycja")]
        [RegularExpression("^[0-9]*$")]
        public int Position { get; set; }

        [Display(Name ="Zdjęcie")]
        public string PictureUrl { get; set; }

        [Required]
        [Display(Name ="Opis")]
        [MaxLength(30,ErrorMessage ="Pole {0} max. liczba znaków {1}")]
        public string ShortDescription { get; set; }
    }
}