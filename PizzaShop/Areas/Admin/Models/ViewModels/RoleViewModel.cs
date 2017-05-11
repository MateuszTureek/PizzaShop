using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PizzaShop.Areas.Admin.Models.ViewModels
{
    public class RoleViewModel
    {
        public string Id { get; set; }

        [Required]
        [Display(Name="Nazwa")]
        [StringLength(256, ErrorMessage ="Pole {0} max. {1} znaków.")]
        [RegularExpression(@"^[a-zA-ZąćęłńóśźżĄĘŁŃÓŚŹŻ\s]*$")]
        public string Name { get; set; }
    }
}