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
    public class SauceController : Controller
    {
        readonly ISauceRepository _repository;
        readonly IMapper _mapper;

        public SauceController(ISauceRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public ActionResult Index()
        {
            ViewBag.ModelIsNotValid = TempData["ModelIsNotValid"];
            var sauces = _repository.GetAll().ToList();
            return View("Index", sauces);
        }

        public ActionResult CreatePartial()
        {
            return PartialView("_CreatePartial");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Exclude = "ID")]SauceViewModel sauceViewModel)
        {
            if (!ModelState.IsValid)
            {
                TempData["ModelIsNotValid"] = "Wystąpił błąd w formularzu, spróbuj ponownie.";
                return RedirectToAction("Index");
            }
            var sauce = _mapper.Map<SauceViewModel, Sauce>(sauceViewModel);
            _repository.Insert(sauce);
            _repository.Save();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var sauce = _repository.Get(id);
            if (sauce == null)
                return HttpNotFound();
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index");
            _repository.Delete(sauce);
            _repository.Save();
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var sauce = _repository.Get(id);
            if (sauce == null)
                return HttpNotFound();
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index");
            var sauceViewModel = _mapper.Map<Sauce, SauceViewModel>(sauce);
            return PartialView("_EditPartial", sauceViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SauceViewModel sauceViewModel)
        {
            if (!ModelState.IsValid)
            {
                TempData["ModelIsNotValid"] = "Wystąpił błąd w formularzu, spróbuj ponownie.";
                return RedirectToAction("Index");
            }
            var sauce = _repository.Get(sauceViewModel.ID);
            if (sauce == null)
                return HttpNotFound();
            var result = _mapper.Map<SauceViewModel, Sauce>(sauceViewModel, sauce);
            _repository.Update(result);
            _repository.Save();
            return RedirectToAction("Index");
        }
    }
}