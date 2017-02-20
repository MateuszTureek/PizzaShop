using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PizzaShop.Areas.Admin.Controllers
{
    public class ControlPanelController : Controller
    {
        // GET: Admin/ControlPanel
        public ActionResult Index()
        {
            return View();
        }
    }
}