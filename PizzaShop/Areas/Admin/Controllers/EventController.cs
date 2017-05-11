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
    public class EventController : Controller
    {
        readonly IEventRepository _repository;
        readonly IMapper _mapper;

        public EventController(IEventRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public ActionResult Index()
        {
            ViewBag.ModelIsNotValid = TempData["ModelIsNotValid"];
            var events = _repository.GetAll().ToList();
            return View("Index", events);
        }

        public ActionResult CreatePartial()
        {
            return PartialView("_CreatePartial");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Exclude = "ID,AddedDate")]EventViewModel evnetViewModel)
        {
            if (!ModelState.IsValid)
            {
                TempData["ModelIsNotValid"] = "Wystąpił błąd w formularzu, spróbuj ponownie.";
                return RedirectToAction("Index");
            }
            var eventObj = _mapper.Map<EventViewModel, Event>(evnetViewModel);
            eventObj.AddedDate = DateTime.Now;
            _repository.Insert(eventObj);
            _repository.Save();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var eventObj = _repository.Get(id);
            if (eventObj == null)
                return HttpNotFound();
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index");
            _repository.Delete(eventObj);
            _repository.Save();
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var eventObj = _repository.Get(id);
            if (eventObj == null)
                return HttpNotFound();
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index");
            var eventViewModel = _mapper.Map<Event, EventViewModel>(eventObj);
            return PartialView("_EditPartial", eventViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Exclude = "AddedDate")]EventViewModel eventViewModel)
        {
            if (!ModelState.IsValid)
            {
                TempData["ModelIsNotValid"] = "Wystąpił błąd w formularzu, spróbuj ponownie.";
                return RedirectToAction("Index");
            }
            var eventObj = _repository.Get(eventViewModel.ID);
            if (eventObj == null)
                return HttpNotFound();
            var result = _mapper.Map<EventViewModel, Event>(eventViewModel, eventObj);
            result.AddedDate = DateTime.Now;
            _repository.Update(result);
            _repository.Save();
            return RedirectToAction("Index");
        }
    }
}