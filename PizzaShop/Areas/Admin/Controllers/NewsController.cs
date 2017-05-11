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
    public class NewsController : Controller
    {
        readonly INewsRepository _repository;
        readonly IMapper _mapper;

        public NewsController(INewsRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public ActionResult Index()
        {
            ViewBag.ModelIsNotValid = TempData["ModelIsNotValid"];
            var newsList = _repository.GetAll().ToList();
            return View("Index", newsList);
        }

        public ActionResult CreatePartial()
        {
            return PartialView("_CreatePartial");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Exclude = "ID,AddedDate")]NewsViewModel newsViewModel)
        {
            if (!ModelState.IsValid)
            {
                TempData["ModelIsNotValid"] = "Wystąpił błąd w formularzu, spróbuj ponownie.";
                return RedirectToAction("Index");
            }
            var result = _mapper.Map<NewsViewModel, News>(newsViewModel);
            result.AddedDate = DateTime.Now;
            _repository.Insert(result);
            _repository.Save();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var model = _repository.Get(id);
            if (model == null)
                return HttpNotFound();
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index");
            _repository.Delete(model);
            _repository.Save();
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var news = _repository.Get(id);
            if (news == null)
                return HttpNotFound();
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index");
            var viewModel = _mapper.Map<News, NewsViewModel>(news);
            return PartialView("_EditPartial", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(NewsViewModel newsViewModel)
        {
            if (!ModelState.IsValid)
            {
                TempData["ModelIsNotValid"] = "Wystąpił błąd w formularzu, spróbuj ponownie.";
                return RedirectToAction("Index");
            }
            var news = _repository.Get(newsViewModel.ID);
            if (news == null)
                return HttpNotFound();
            var result = _mapper.Map<NewsViewModel, News>(newsViewModel, news);
            result.AddedDate = DateTime.Now;
            _repository.Update(result);
            _repository.Save();
            return RedirectToAction("Index");
        }
    }
}