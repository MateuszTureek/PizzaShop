using AutoMapper;
using PizzaShop.Areas.Admin.Models.ViewModels;
using PizzaShop.Models.PizzaShopModels.CMS;
using PizzaShop.Repositories.CMS.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace PizzaShop.Areas.Admin.Controllers
{
    [Authorize]
    public class MenuItemController : Controller
    {
        readonly IMenuItemRepository _repository;

        public MenuItemController(IMenuItemRepository repository)
        {
            _repository = repository;
        }

        public ActionResult Index()
        {
            var model = _repository.GetAll();
            return View("Index", model);
        }

        public ActionResult CreatePartial()
        {
            return PartialView("_CreatePartial");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Exclude = "ID")]MenuItemViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = Mapper.Map<MenuItemViewModel, MenuItem>(model);
                _repository.Insert(result);
                _repository.Save();
                return RedirectToAction("Index");
            }
            return View("Index");
        }

        public ActionResult Delete(int? id)
        {
            var model = _repository.Get((int)id);
            if (model != null)
            {
                if (Request.IsAjaxRequest())
                {
                    _repository.Delete(model);
                    _repository.Save();
                    return Json("", JsonRequestBehavior.AllowGet);
                }
                return RedirectToAction("Index");
            }
            return HttpNotFound();
        }

        public ActionResult Edit(int? id)
        {
            var model = _repository.Get((int)id);
            if (model != null)
            {
                MenuItemViewModel viewModel = null;
                viewModel = Mapper.Map<MenuItem, MenuItemViewModel>(model, viewModel);
                if (Request.IsAjaxRequest())
                    return PartialView("_EditPartial", viewModel);
            }
            return HttpNotFound("Nie znaleziono szukanego elementu.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MenuItemViewModel model)
        {
            if (ModelState.IsValid)
            {
                var currentModel = _repository.Get(model.ID);
                if (currentModel != null)
                {
                    var result = currentModel = Mapper.Map<MenuItemViewModel, MenuItem>(model, currentModel);
                    _repository.Update(result);
                    _repository.Save();
                    return RedirectToAction("Index");
                }
                return HttpNotFound();
            }
            return new HttpStatusCodeResult(HttpStatusCode.NotModified);
        }
    }
}