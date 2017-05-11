using AutoMapper;
using PizzaShop.Areas.Admin.Models.ViewModels;
using PizzaShop.Models.PizzaShopModels.CMS;
using PizzaShop.Services.Cms.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace PizzaShop.Areas.Admin.Controllers
{
    [Authorize]
    public class InformationItemController : Controller
    {
        readonly IInformationItemService _service;
        public InformationItemController(IInformationItemService service)
        {
            _service = service;
        }

        public ActionResult Index()
        {
            ViewBag.ModelIsNotValid = TempData["ModelIsNotValid"];
            var informationItems = _service.GetAllInformationItems();
            return View("Index", informationItems);
        }

        public ActionResult CreatePartial()
        {
            return PartialView("_CreatePartial");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Exclude = "ID, PictureUrl")]InformationItemViewModel informationItemViewModel, HttpPostedFileBase PictureContent)
        {
            if (!ModelState.IsValid)
            {
                TempData["ModelIsNotValid"] = "Wystąpił błąd w formularzu, spróbuj ponownie.";
                return RedirectToAction("Index");
            }
            if (PictureContent == null || PictureContent.ContentLength <= 0 || !PictureContent.ContentType.Contains("image"))
            {
                TempData["ModelIsNotValid"] = "Zdjęcie nie zostało przesłane prawidłowo. Spróbuj ponownie.";
                return RedirectToAction("Index");
            }
            var informationItem = _service.MapViewModelToObject(informationItemViewModel);
            informationItem.PictureUrl = _service.AddInformationItemImage(PictureContent);
            _service.CreateInformationItem(informationItem);
            _service.SaveInfomrationItem();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var informationItem = _service.GetInfomrationItem((int)id);
            if (informationItem == null)
                return HttpNotFound();
            if (!Request.IsAjaxRequest())
            {
                TempData["ModelIsNotValid"] = "Wystąpił błąd w formularzu, spróbuj ponownie.";
                return RedirectToAction("Index");
            }
            _service.DeleteInfomationItem(informationItem);
            _service.SaveInfomrationItem();
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var informationItem = _service.GetInfomrationItem((int)id);
            if (informationItem == null)
                return HttpNotFound();
            if (!Request.IsAjaxRequest())
            {
                TempData["ModelIsNotValid"] = "Wystąpił błąd w formularzu, spróbuj ponownie.";
                return RedirectToAction("Index");
            }
            var viewModel = _service.MapObjectToViewModel(informationItem);
            return PartialView("_EditPartial", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(InformationItemViewModel informationItemViewModel)
        {
            if (!ModelState.IsValid)
            {
                TempData["ModelIsNotValid"] = "Wystąpił błąd w formularzu, spróbuj ponownie.";
                return RedirectToAction("Index");
            }
            var informationItem = _service.GetInfomrationItem(informationItemViewModel.ID);
            if (informationItem == null)
                return HttpNotFound();
            var result = _service.MapViewModelToObject(informationItemViewModel, informationItem);
            _service.UpdateInformationItem(result);
            _service.SaveInfomrationItem();
            return RedirectToAction("Index");
        }
    }
}