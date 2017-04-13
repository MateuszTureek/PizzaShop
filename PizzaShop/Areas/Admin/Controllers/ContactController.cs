using AutoMapper;
using PizzaShop.Services.Xml;
using PizzaShop.Services.Xml.XmlModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace PizzaShop.Areas.Admin.Controllers
{
    [Authorize]
    public class ContactController : Controller
    {
        readonly IXmlManager _xmlManager;
        readonly IMapper _mappper;

        public ContactController(IXmlManager xmlManager, IMapper mapper)
        {
            _xmlManager = xmlManager;
            _mappper = mapper;
        }

        public ActionResult Index()
        {
            var model = _xmlManager.GetXmlModel<ShopContact>(GlobalXmlManager.ContactFileName);
            return View("Index", model);
        }

        public ActionResult Edit()
        {
            if (Request.IsAjaxRequest())
            {
                var shopContact = _xmlManager.GetXmlModel<ShopContact>(GlobalXmlManager.ContactFileName);
                if (shopContact != null)
                {
                    var model = _mappper.Map<ShopContact, ShopContactViewModel>(shopContact);
                    return PartialView("_EditPartial", model);
                }
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ShopContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                var shopContact = _mappper.Map<ShopContactViewModel, ShopContact>(model);
                _xmlManager.CreateXmlFile<ShopContact>(GlobalXmlManager.ContactFileName, shopContact);
                return RedirectToAction("Index");
            }
            return new HttpStatusCodeResult(HttpStatusCode.NotModified);
        }
    }
}