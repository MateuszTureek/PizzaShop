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
            var model = _service.GetAllPizzas();
            return View("Index", model);
        }

        public ActionResult CreatePartial()
        {
            ViewBag.Components = _service.GetAllComponents();
            return PartialView("_CreatePartial");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Exclude = "ID")]PizzaViewModel pizzaModel)
        {
            if (ModelState.IsValid)
            {
                _service.CreatePizza(pizzaModel);
                _service.SavePizza();
                return RedirectToAction("Index", "Pizza");
            }
            return new HttpStatusCodeResult(HttpStatusCode.NotModified);
        }

        public ActionResult Delete(int? id)
        {
            var model = _service.GetPizza((int)id);
            if (model != null)
            {
                if (Request.IsAjaxRequest())
                {
                    _service.DeletePizza(model);
                    _service.SavePizza();
                    return Json("", JsonRequestBehavior.AllowGet);
                }
                return RedirectToAction("Index");
            }
            return HttpNotFound();
        }

        public ActionResult Edit(int? id)
        {
            var pizza = _service.GetPizza((int)id);
            if (pizza != null)
            {
                if (Request.IsAjaxRequest())
                {
                    ViewBag.Components = _service.FindNotPizzaComponent(id);
                    ViewBag.CurrentComponents = _service.FindComponents(id);
                    var pizzaSizePrices = _service.GetPizzaSizePrices(id);
                    var viewModel = new PizzaViewModel()
                    {
                        ID = pizza.ID,
                        Name = pizza.Name,
                        PriceForSmall = pizzaSizePrices[0].Price,
                        PriceForMedium = pizzaSizePrices[1].Price,
                        PriceForLarge = pizzaSizePrices[2].Price
                    };
                    return PartialView("_EditPartial", viewModel);
                }
            }
            return HttpNotFound("Nie znaleziono szukanego elementu.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PizzaViewModel model)
        {
            if (ModelState.IsValid)
            {
                _service.UpdatePizza(model);
                _service.SavePizza();
                return RedirectToAction("Index");
            }
            return new HttpStatusCodeResult(HttpStatusCode.NotModified);
        }
    }
}
