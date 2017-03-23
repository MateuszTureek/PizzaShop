using PizzaShop.XML.Services.XmlServices;
using PizzaShop.XML.Services.XmlServices.XmlModels;
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

        public ContactController(IXmlManager xmlManager)
        {
            _xmlManager = xmlManager;
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
                var model = _xmlManager.GetXmlModel<ShopContact>("ShopContact");
                if (model != null)
                    return PartialView("_EditPartial", model);
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ShopContact model)
        {
            if(ModelState.IsValid)
            {
                _xmlManager.CreateXmlFile<ShopContact>("ShopContact", model);
                return RedirectToAction("Index");
            }
            return new HttpStatusCodeResult(HttpStatusCode.NotModified);
        }
    }
}