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
        readonly IMapper _mapper;
        public InformationItemController(IInformationItemService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public ActionResult Index()
        {
            var model = _service.GetAllInformationItems();
            return View("Index", model);
        }

        public ActionResult CreatePartial()
        {
            return PartialView("_CreatePartial");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Exclude = "ID, PictureUrl")]InformationItemViewModel model, HttpPostedFileBase PictureContent)
        {
            if (ModelState.IsValid && PictureContent != null && PictureContent.ContentLength > 0 && PictureContent.ContentType.Contains("image"))
            {
                var result = _mapper.Map<InformationItemViewModel, InformationItem>(model);
                result.PictureUrl = _service.AddInformationItemImage(PictureContent);
                _service.CreateInformationItem(result);
                _service.SaveInfomrationItem();
                return RedirectToAction("Index");
            }
            return View("Index");
        }

        public ActionResult Delete(int? id)
        {
            var model = _service.GetInfomrationItem((int)id);
            if (model != null)
            {
                if (Request.IsAjaxRequest())
                {
                    _service.DeleteInfomationItem(model);
                    _service.SaveInfomrationItem();
                    return Json("", JsonRequestBehavior.AllowGet);
                }
                return RedirectToAction("Index");
            }
            return HttpNotFound();
        }

        public ActionResult Edit(int? id)
        {
            var model = _service.GetInfomrationItem((int)id);
            if (model != null)
            {
                var viewModel = _mapper.Map<InformationItem, InformationItemViewModel>(model);
                if (Request.IsAjaxRequest())
                    return PartialView("_EditPartial", viewModel);
            }
            return HttpNotFound("Nie znaleziono szukanego elementu.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(InformationItemViewModel model)
        {
            if (ModelState.IsValid)
            {
                var currentModel = _service.GetInfomrationItem(model.ID);
                if (currentModel != null)
                {
                    var result = _mapper.Map<InformationItemViewModel, InformationItem>(model,currentModel);
                    _service.UpdateInformationItem(result);
                    _service.SaveInfomrationItem();
                    return RedirectToAction("Index");
                }
                return HttpNotFound();
            }
            return new HttpStatusCodeResult(HttpStatusCode.NotModified);
        }
    }
}