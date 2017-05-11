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
    public class SaladController : Controller
    {
        readonly ISaladRepository _repository;
        readonly IMapper _mapper;

        public SaladController(ISaladRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public ActionResult Index()
        {
            ViewBag.ModelIsNotValid = TempData["ModelIsNotValid"];
            var salads = _repository.GetAll().ToList();
            return View("Index", salads);
        }

        public ActionResult CreatePartial()
        {
            return PartialView("_CreatePartial");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Exclude = "ID")]SaladViewModel saladViewModel)
        {
            if (!ModelState.IsValid)
            {
                TempData["ModelIsNotValid"] = "Wystąpił błąd w formularzu, spróbuj ponownie.";
                return RedirectToAction("Index");
            }
            var salad = _mapper.Map<SaladViewModel, Salad>(saladViewModel);
            _repository.Insert(salad);
            _repository.Save();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var salad = _repository.Get(id);
            if (salad == null)
                return HttpNotFound();
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index");
            _repository.Delete(salad);
            _repository.Save();
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var salad = _repository.Get(id);
            if (salad == null)
                return HttpNotFound();
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index");
            var viewModel = _mapper.Map<Salad, SaladViewModel>(salad);
            return PartialView("_EditPartial", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SaladViewModel saladViewModel)
        {
            if (!ModelState.IsValid)
            {
                TempData["ModelIsNotValid"] = "Wystąpił błąd w formularzu, spróbuj ponownie.";
                return RedirectToAction("Index");
            }
            var salad = _repository.Get(saladViewModel.ID);
            if (salad == null)
                return HttpNotFound();
            var result = _mapper.Map<SaladViewModel, Salad>(saladViewModel, salad);
            _repository.Update(result);
            _repository.Save();
            return RedirectToAction("Index");
        }
    }
}