using PizzaShop.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PizzaShop.Controllers
{
    public class HomeController : Controller
    {
        HomeUnitOfWork _unitOfWork;

        public HomeController(HomeUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index()
        {
            return View("Index");
        }

        public ActionResult Menu()
        {
            return View("Menu");
        }

        public ActionResult Gallery()
        {
            return View("Gallery");
        }

        public ActionResult Contact()
        {
            return View("Contact");
        }
        
        [ChildActionOnly]
        public ActionResult SiteMenuPartial()
        {
            var model = _unitOfWork.MenuItemRepository.All();
            return PartialView("_SiteMenuPartial",model);
        }

        [ChildActionOnly]
        public ActionResult SliderPartial()
        {
            var model = _unitOfWork.SliderItemRepository.All();
            return PartialView("_SliderPartial", model);
        }

        [ChildActionOnly]
        public ActionResult InformationPartial()
        {
            var model = _unitOfWork.InformationItemRepository.All();
            return PartialView("_InformationPartial", model);
        }

        [ChildActionOnly]
        public ActionResult EventPartial()
        {
            var model = _unitOfWork.EventRepository.All();
            return PartialView("_EventPartial", model);
        }

        [ChildActionOnly]
        public ActionResult NewPartial()
        {
            var model = _unitOfWork.NewRepository.All();
            return PartialView("_NewPartial", model);
        }
    }
}