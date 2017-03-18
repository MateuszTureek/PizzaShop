﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PizzaShop.Areas.Admin.Models.ViewModels
{
    public class InformationItemViewModel
    {
        public int ID { get; set; }

        [Required]
        [Display(Name ="Tytuł")]
        [StringLength(30,ErrorMessage ="Max. liczba znaków {1}.")]
        public string Title { get; set; }

        [Display(Name ="Zdjęcie url")]
        public string PictureUrl { get; set; }

        [Required]
        [Display(Name ="Treść")]
        [MaxLength(250,ErrorMessage ="Max. liczba znaków {1}")]
        public string Content { get; set; }

        [Required]
        [Display(Name ="Pozycja")]
        public int Position { get; set; }
    }
}