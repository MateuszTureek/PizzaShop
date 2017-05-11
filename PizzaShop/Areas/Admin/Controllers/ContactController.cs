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
            ViewBag.ModelIsNotValid = TempData["ModelIsNotValid"];
            var shopContact = _xmlManager.GetXmlModel<ShopContact>(GlobalXmlManager.ContactFileName);
            return View("Index", shopContact);
        }

        public ActionResult Edit()
        {
            if (!Request.IsAjaxRequest())
            {
                return RedirectToAction("Index");
            }
            var shopContact = _xmlManager.GetXmlModel<ShopContact>(GlobalXmlManager.ContactFileName);
            if (shopContact == null)
            {
                return HttpNotFound();
            }
            var shopContextViewModel = _mappper.Map<ShopContact, ShopContactViewModel>(shopContact);
            return PartialView("_EditPartial", shopContextViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ShopContactViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ModelIsNotValid"] = "Wystąpił błąd w formularzu, spróbuj ponownie.";
                return RedirectToAction("Index");
            }
            var shopContact = _mappper.Map<ShopContactViewModel, ShopContact>(model);
            _xmlManager.CreateXmlFile<ShopContact>(GlobalXmlManager.ContactFileName, shopContact);
            return RedirectToAction("Index");
        }
    }
}