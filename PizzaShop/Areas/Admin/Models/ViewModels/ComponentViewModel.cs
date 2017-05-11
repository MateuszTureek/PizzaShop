using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PizzaShop.Areas.Admin.Models.ViewModels
{
    public class ComponentViewModel
    {
        public int ID { get; set; }

        [Required]
        [StringLength(30, ErrorMessage ="Max. ilość znaków dla pola {0} to {1}.")]
        [RegularExpression(@"^[a-zA-ZąćęłńóśźżĄĘŁŃÓŚŹŻ\s]*$")]
        public string Name { get; set; }
    }
}