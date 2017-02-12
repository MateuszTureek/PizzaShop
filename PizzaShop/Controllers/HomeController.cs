using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PizzaShop.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Main page.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View("Index");
        }

        /// <summary>
        /// Menu page.
        /// </summary>
        /// <returns></returns>
        public ActionResult Menu()
        {
            return View("Menu");
        }


        /// <summary>
        /// Gallery page
        /// </summary>
        /// <returns></returns>
        public ActionResult Gallery()
        {
            return View("Gallery");
        }

        /// <summary>
        /// Contact page.
        /// </summary>
        /// <returns></returns>
        public ActionResult Contact()
        {
            return View("Contact");
        }
    }
}