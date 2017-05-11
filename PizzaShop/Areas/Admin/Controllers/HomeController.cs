using PizzaShop.Areas.Admin.Models.ViewModels;
using PizzaShop.Services.shop.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PizzaShop.Areas.Admin.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        readonly IMenuCardService _service;

        public HomeController(IMenuCardService service)
        {
            _service = service;
        }

        public ActionResult Index()
        {
            var menuCartViewModel = new MenuCardViewModel()
            {
                Drinks = _service.GetAllDrinks(),
                Pizzas = _service.GetAllPizzas(),
                Salads = _service.GetAllSalads(),
                Sauces = _service.GetAllSauces()
            };
            return View("Index", menuCartViewModel);
        }

        public ActionResult LoadingPartial()
        {
            return PartialView("_LoadingPartial");
        }
    }
}