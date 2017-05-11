using PizzaShop.Areas.Admin.Models.ViewModels;
using PizzaShop.Services.Cms.Interfaces;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace PizzaShop.Areas.Admin.Controllers
{
    [Authorize]
    public class SliderItemController : Controller
    {
        ISliderItemService _service;

        public SliderItemController(ISliderItemService service)
        {
            _service = service;
        }

        public ActionResult Index()
        {
            ViewBag.ModelIsNotValid = TempData["ModelIsNotValid"];
            var sliderItems = _service.SliderItemList();
            return View("Index", sliderItems);
        }

        public ActionResult CreatePartial()
        {
            return PartialView("_CreatePartial");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Exclude = "ID, PictureUrl")]SliderItemViewModel sliderItemViewModel, HttpPostedFileBase PictureContent)
        {
            if(!ModelState.IsValid)
            {
                TempData["ModelIsNotValid"] = "Wystąpił błąd w formularzu, spróbuj ponownie.";
                return RedirectToAction("Index");
            }

            if (PictureContent == null || PictureContent.ContentLength <= 0 || !PictureContent.ContentType.Contains("image"))
            {
                TempData["ModelIsNotValid"] = "Zdjęcie nie zostało przesłane prawidłowo. Spróbuj ponownie.";
                return RedirectToAction("Index");
            }
            var sliderItem = _service.MapViewModelToModel(sliderItemViewModel);
            sliderItem.PictureUrl = _service.AddSliderItemImage(PictureContent);
            _service.CreateSliderItem(sliderItem);
            _service.SaveSliderItem();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var sliderItem = _service.GetSliderItem((int)id);
            if (sliderItem == null)
                return HttpNotFound();
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index");
            _service.DeleteSliderItem(sliderItem);
            _service.SaveSliderItem();
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var sliderItem = _service.GetSliderItem((int)id);
            if (sliderItem == null)
                return HttpNotFound();
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index");
            var sliderItemViewModel = _service.MapModelToViewModel(sliderItem);
            return PartialView("_EditPartial", sliderItemViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SliderItemViewModel sliderItemViewModel)
        {
            if (!ModelState.IsValid)
            {
                TempData["ModelIsNotValid"] = "Wystąpił błąd w formularzu, spróbuj ponownie.";
                return RedirectToAction("Index");
            }
            var sliderItem = _service.GetSliderItem(sliderItemViewModel.ID);
            if (sliderItem == null)
                return HttpNotFound();
            var result = _service.MapViewModelToModel(sliderItemViewModel, sliderItem);
            _service.UpdateSliderItem(result);
            _service.SaveSliderItem();
            return RedirectToAction("Index");
        }
    }
}