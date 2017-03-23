using AutoMapper;
using PizzaShop.Areas.Admin.Models.ViewModels;
using PizzaShop.Models.PizzaShopModels.CMS;
using PizzaShop.Repositories.CMS.Interfaces;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace PizzaShop.Areas.Admin.Controllers
{
    [Authorize]
    public class SliderItemController : Controller
    {
        ISliderItemRepository _repository;
        string _virtualPath = "/Content/Images";
        string _physicalPath;

        public SliderItemController(ISliderItemRepository repository)
        {
            _repository = repository;
            _physicalPath = HostingEnvironment.MapPath(_virtualPath);
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
        public ActionResult Create([Bind(Exclude = "ID, PictureUrl")]SliderItemViewModel model, HttpPostedFileBase PictureContent)
        {
            if (ModelState.IsValid && PictureContent != null && PictureContent.ContentLength > 0)
            {
                string fileName = Path.GetFileName(PictureContent.FileName);
                using (var bReader = new BinaryReader(PictureContent.InputStream))
                {
                    var binaryImg = bReader.ReadBytes(PictureContent.ContentLength);
                    using (var bWriter = new BinaryWriter(new FileStream(_physicalPath + "\\" + fileName, FileMode.Create)))
                    {
                        bWriter.Write(binaryImg);
                    }
                }
                var result = Mapper.Map<SliderItemViewModel, SliderItem>(model);
                result.PictureUrl = _virtualPath + "/" + fileName;
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
                SliderItemViewModel viewModel = null;
                viewModel = Mapper.Map<SliderItem, SliderItemViewModel>(model, viewModel);
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
                var currentModel = _repository.Get(model.ID);
                if (currentModel != null)
                {
                    var result = currentModel = Mapper.Map<SliderItemViewModel, SliderItem>(model, currentModel);
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