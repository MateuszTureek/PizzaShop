using PizzaShop.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PizzaShop.Controllers
{
    public class MenuController : Controller
    {
        readonly MenuUnitOfWork _unitOfWork;

        public MenuController(MenuUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ActionResult Pizza()
        {
            var model = _unitOfWork.PizzaRepository.All();
            return View("Pizza", model);
        }

        public ActionResult Salad()
        {
            var model = _unitOfWork.SaladRepository.All();
            return View("Salad", model);
        }

        public ActionResult Sauce()
        {
            var model = _unitOfWork.SauceRepository.All();
            return View("Sauce", model);
        }

        public ActionResult Drink()
        {
            var model = _unitOfWork.DrinkRepository.All();
            return View("Drink", model);
        }
    }
}