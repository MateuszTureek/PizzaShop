using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PizzaShop.Areas.Admin.Models.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }

        [Required]
        [Display(Name ="Login")]
        public string UserName { get; set; }

        [Required]
        [Display(Name ="Emial")]
        [EmailAddress]
        public string Email { get; set; }

        public List<string> Roles { get; set; }
    }
}