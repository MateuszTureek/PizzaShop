using PizzaShop.Services.XmlServices;
using PizzaShop.Services.XmlServices.XmlModels;
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
        IXmlManager _xmlManager;

        public HomeController(HomeUnitOfWork unitOfWork, IXmlManager xmlManager)
        {
            _unitOfWork = unitOfWork;
            _xmlManager = xmlManager;
        }

        public ActionResult Index()
        {
            return View("Index");
        }

        public ActionResult Gallery()
        {
            var model = _unitOfWork.GalleryRepository.All();
            return View("Gallery", model);
        }

        public ActionResult Contact()
        {
            return View("Contact");
        }

        [ChildActionOnly]
        public ActionResult SiteMenuPartial()
        {
            var model = _unitOfWork.MenuItemRepository.All();
            return PartialView("_SiteMenuPartial", model);
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

        public ActionResult ShopContactPartial()
        {
            var model = _xmlManager.GetXmlModel<ShopContact>("ShopContact");
            return PartialView("_ShopContactPartial", model);
        }

        public ActionResult OpeningHoursPartial()
        {
            var model = _xmlManager.GetXmlModel<OpeningHours>("OpeningHours");
            return PartialView("_OpeningHoursPartial", model);
        }

        public ActionResult DeliveryContact()
        {
            var model = _xmlManager.GetXmlModel<ShopContact>("ShopContact");
            string phone = model.Address.DeliveryContact;
            return Content(phone);
        }
    }
}