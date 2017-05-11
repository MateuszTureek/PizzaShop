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
    public class ComponentController : Controller
    {
        IComponentRepository _repository;
        IMapper _mapper;

        public ComponentController(IComponentRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [ChildActionOnly]
        public ActionResult ComponentListPartial()
        {
            var components = _repository.GetAll().ToList();
            return PartialView("_ComponentsPartial",components);
        }

        public ActionResult Index()
        {
            ViewBag.ModelIsNotValid = TempData["ModelIsNotValid"];
            var components = _repository.GetAll().ToList();
            return View("Index", components);
        }

        public ActionResult CreatePartial()
        {
            return PartialView("_CreatePartial");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Exclude = "ID")]ComponentViewModel componentViewModel)
        {
            if (!ModelState.IsValid)
            {
                TempData["ModelIsNotValid"] = "Wystąpił błąd w formularzu, spróbuj ponownie.";
                return RedirectToAction("Index");
            }
            var result = _mapper.Map<ComponentViewModel, Component>(componentViewModel);
            _repository.Insert(result);
            _repository.Save();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var component = _repository.Get(id);
            if (component == null)
                return HttpNotFound();
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index");
            _repository.Delete(component);
            _repository.Save();
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var component = _repository.Get(id);
            if (component == null)
                return HttpNotFound();
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index");
            var viewModel = _mapper.Map<Component, ComponentViewModel>(component);
            return PartialView("_EditPartial", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ComponentViewModel componentViewModel)
        {
            if (!ModelState.IsValid)
            {
                TempData["ModelIsNotValid"] = "Wystąpił błąd w formularzu, spróbuj ponownie.";
                return View("Index");
            }
            var component = _repository.Get(componentViewModel.ID);
            if (component == null)
                return HttpNotFound();
            var result = _mapper.Map<ComponentViewModel, Component>(componentViewModel, component);
            _repository.Update(result);
            _repository.Save();
            return RedirectToAction("Index");
        }
    }
}