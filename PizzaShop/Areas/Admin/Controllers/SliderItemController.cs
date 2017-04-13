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
            var model = _service.SliderItemList();
            return View("Index", model);
        }

        public ActionResult CreatePartial()
        {
            return PartialView("_CreatePartial");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Exclude = "ID, PictureUrl")]SliderItemViewModel model, HttpPostedFileBase PictureContent)
        {
            if (ModelState.IsValid && PictureContent != null && PictureContent.ContentLength > 0 && PictureContent.ContentType.Contains("image"))
            {
                var result = _service.MapViewModelToModel(model);
                result.PictureUrl = _service.AddSliderItemImage(PictureContent);
                _service.CreateSliderItem(result);
                _service.SaveSliderItem();
                return RedirectToAction("Index");
            }
            return View("Index");
        }

        public ActionResult Delete(int? id)
        {
            var model = _service.GetSliderItem((int)id);
            if (model != null)
            {
                if (Request.IsAjaxRequest())
                {
                    _service.DeleteSliderItem(model);
                    _service.SaveSliderItem();
                    return Json("", JsonRequestBehavior.AllowGet);
                }
                return RedirectToAction("Index");
            }
            return HttpNotFound();
        }

        public ActionResult Edit(int? id)
        {
            var model = _service.GetSliderItem((int)id);
            if (model != null)
            {
                var viewModel = _service.MapModelToViewModel(model);
                if (Request.IsAjaxRequest())
                    return PartialView("_EditPartial", viewModel);
            }
            return HttpNotFound("Nie znaleziono szukanego elementu.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SliderItemViewModel model)
        {
            if (ModelState.IsValid)
            {
                var currentModel = _service.GetSliderItem(model.ID);
                if (currentModel != null)
                {
                    var result = _service.MapViewModelToModel(model);
                    _service.UpdateSliderItem(result);
                    _service.SaveSliderItem();
                    return RedirectToAction("Index");
                }
                return HttpNotFound();
            }
            return new HttpStatusCodeResult(HttpStatusCode.NotModified);
        }
    }
}