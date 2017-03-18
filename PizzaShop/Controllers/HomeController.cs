using PizzaShop.Services.Cms.Interfaces;
using PizzaShop.XML.Services.XmlServices;
using PizzaShop.XML.Services.XmlServices.XmlModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PizzaShop.Controllers
{
    public class HomeController : Controller
    {
        readonly IHomePresentationService _service;
        readonly IXmlManager _xmlManager;

        public HomeController(IHomePresentationService service, IXmlManager xmlManager)
        {
            _service = service;
            _xmlManager = xmlManager;
        }

        public ActionResult Index()
        {
            return View("Index");
        }

        public ActionResult Gallery()
        {
            var model = _service.GetAllGalleryItems();
            return View("Gallery", model);
        }

        public ActionResult Contact()
        {
            return View("Contact");
        }

        [ChildActionOnly]
        public ActionResult SiteMenuPartial()
        {
            var model = _service.GetAllMenuItems();
            return PartialView("_SiteMenuPartial", model);
        }

        [ChildActionOnly]
        public ActionResult SliderPartial()
        {
            var model = _service.GetAllSliderItems();
            return PartialView("_SliderPartial", model);
        }

        [ChildActionOnly]
        public ActionResult InformationPartial()
        {
            var model = _service.GetAllInformationItems();
            return PartialView("_InformationPartial", model);
        }

        [ChildActionOnly]
        public ActionResult EventPartial()
        {
            var model = _service.GetAllEvents();
            return PartialView("_EventPartial", model);
        }

        [ChildActionOnly]
        public ActionResult NewPartial()
        {
            var model = _service.GetAllNews();
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