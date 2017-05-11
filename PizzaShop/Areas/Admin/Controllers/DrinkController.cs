using AutoMapper;
using PizzaShop.Areas.Admin.Models.ViewModels;
using PizzaShop.Models.PizzaShopModels.Entities;
using PizzaShop.Repositories.Shop.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace PizzaShop.Areas.Admin.Controllers
{
    [Authorize]
    public class DrinkController : Controller
    {
        readonly IDrinkRepository _repository;
        readonly IMapper _mapper;

        public DrinkController(IDrinkRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public ActionResult Index()
        {
            ViewBag.ModelIsNotValid = TempData["ModelIsNotValid"];
            var drinks = _repository.GetAll().ToList();
            return View("Index", drinks);
        }

        public ActionResult CreatePartial()
        {
            return PartialView("_CreatePartial");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Exclude = "ID")]DrinkViewModel drinkViewModel)
        {
            if (!ModelState.IsValid)
            {
                TempData["ModelIsNotValid"] = "Wystąpił błąd w formularzu, spróbuj ponownie.";
                return RedirectToAction("Index");
            }
            var result = _mapper.Map<DrinkViewModel, Drink>(drinkViewModel);
            _repository.Insert(result);
            _repository.Save();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var drink = _repository.Get(id);
            if (drink == null)
                return HttpNotFound();
            if(!Request.IsAjaxRequest())
                return RedirectToAction("Index");
            _repository.Delete(drink);
            _repository.Save();
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var drink = _repository.Get(id);
            if (drink == null)
                return HttpNotFound();
            if(!Request.IsAjaxRequest())
                return RedirectToAction("Index");
            var viewModel = _mapper.Map<Drink, DrinkViewModel>(drink);
            return PartialView("_EditPartial", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DrinkViewModel drinkViewModel)
        {
            if (!ModelState.IsValid)
            {
                TempData["ModelIsNotValid"] = "Wystąpił błąd w formularzu, spróbuj ponownie.";
                return View("Index");
            }
            var drink = _repository.Get(drinkViewModel.ID);
            if (drink == null)
                return HttpNotFound();
            var result = _mapper.Map<DrinkViewModel, Drink>(drinkViewModel, drink);
            _repository.Update(result);
            _repository.Save();
            return RedirectToAction("Index");
        }
    }
}