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
        readonly IMapper _mapper;

        public MenuItemController(IMenuItemRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public ActionResult Index()
        {
            ViewBag.ModelIsNotValid = TempData["ModelIsNotValid"];
            var menuItems = _repository.GetAll().ToList();
            return View("Index", menuItems);
        }

        public ActionResult CreatePartial()
        {
            return PartialView("_CreatePartial");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Exclude = "ID")]MenuItemViewModel menuItemViewModel)
        {
            if (!ModelState.IsValid)
            {
                TempData["ModelIsNotValid"] = "Wystąpił błąd w formularzu, spróbuj ponownie.";
                return RedirectToAction("Index");
            }
            var menuItem = _mapper.Map<MenuItemViewModel, MenuItem>(menuItemViewModel);
            _repository.Insert(menuItem);
            _repository.Save();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var menuItem = _repository.Get(id);
            if (menuItem == null)
                return HttpNotFound();
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index");
            _repository.Delete(menuItem);
            _repository.Save();
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var menuItem = _repository.Get(id);
            if (menuItem == null)
                return HttpNotFound();
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index");
            var menuItemViewModel = _mapper.Map<MenuItem, MenuItemViewModel>(menuItem);
            return PartialView("_EditPartial", menuItemViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MenuItemViewModel menuItemViewModel)
        {
            if (!ModelState.IsValid)
            {
                TempData["ModelIsNotValid"] = "Wystąpił błąd w formularzu, spróbuj ponownie.";
                return RedirectToAction("Index");
            }
            var menuItem = _repository.Get(menuItemViewModel.ID);
            if (menuItem == null)
                return HttpNotFound();
            var result = _mapper.Map<MenuItemViewModel, MenuItem>(menuItemViewModel, menuItem);
            _repository.Update(result);
            _repository.Save();
            return RedirectToAction("Index");
        }
    }
}