using PizzaShop.Areas.Admin.Models.ViewModels;
using PizzaShop.Models.PizzaShopModels.Entities;
using PizzaShop.Services.shop.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace PizzaShop.Areas.Admin.Controllers
{
    [Authorize]
    public class PizzaController : Controller
    {
        readonly IPizzaService _service;

        public PizzaController(IPizzaService service)
        {
            _service = service;
        }

        public ActionResult Index()
        {
            ViewBag.ModelIsNotValid = TempData["ModelIsNotValid"];
            var pizzas = _service.GetAllPizzas();
            return View("Index", pizzas);
        }

        public ActionResult CreatePartial()
        {
            ViewBag.Components = _service.GetAllComponents();
            return PartialView("_CreatePartial");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Exclude = "ID")]PizzaViewModel pizzaViewModel)
        {
            if (!ModelState.IsValid)
            {
                TempData["ModelIsNotValid"] = "Wystąpił błąd w formularzu, spróbuj ponownie.";
                return RedirectToAction("Index");
            }
            _service.CreatePizza(pizzaViewModel);
            _service.SavePizza();
            return RedirectToAction("Index", "Pizza");
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var pizza = _service.GetPizza(id);
            if (pizza == null)
                return HttpNotFound();
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index");
            _service.DeletePizza(pizza);
            _service.SavePizza();
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var pizza = _service.GetPizza((int)id);
            if (pizza == null)
                return HttpNotFound();
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index");
            ViewBag.Components = _service.FindNotPizzaComponent(id);
            ViewBag.CurrentComponents = _service.FindComponents(id);
            var pizzaSizePrices = _service.GetPizzaSizePrices(id);
            var pizzaViewModel = new PizzaViewModel()
            {
                ID = pizza.ID,
                Name = pizza.Name,
                PriceForSmall = pizzaSizePrices[0].Price,
                PriceForMedium = pizzaSizePrices[1].Price,
                PriceForLarge = pizzaSizePrices[2].Price
            };
            return PartialView("_EditPartial", pizzaViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PizzaViewModel pizzaViewModel)
        {
            if (!ModelState.IsValid)
            {
                TempData["ModelIsNotValid"] = "Wystąpił błąd w formularzu, spróbuj ponownie.";
                return RedirectToAction("Index");
            }
            _service.UpdatePizza(pizzaViewModel);
            _service.SavePizza();
            return RedirectToAction("Index");
        }
    }
}
