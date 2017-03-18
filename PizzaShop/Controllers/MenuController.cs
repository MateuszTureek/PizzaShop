using PizzaShop.Services.shop.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PizzaShop.Controllers
{
    public class MenuController : Controller
    {
        readonly IMenuCardService _service;

        public MenuController(IMenuCardService service)
        {
            _service = service;
        }

        public ActionResult Pizza()
        {
            var model = _service.GetAllPizzas();
            return View("Pizza", model);
        }

        public ActionResult Salad()
        {
            var model = _service.GetAllSalads();
            return View("Salad", model);
        }

        public ActionResult Sauce()
        {
            var model = _service.GetAllSauces();
            return View("Sauce", model);
        }

        public ActionResult Drink()
        {
            var model = _service.GetAllDrinks();
            return View("Drink", model);
        }
    }
}