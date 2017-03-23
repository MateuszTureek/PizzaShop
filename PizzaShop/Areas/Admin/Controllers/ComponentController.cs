using PizzaShop.Repositories.Shop.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PizzaShop.Areas.Admin.Controllers
{
    [Authorize]
    public class ComponentController : Controller
    {
        IComponentRepository _repository;

        public ComponentController(IComponentRepository repository)
        {
            _repository = repository;
        }

        [ChildActionOnly]
        public ActionResult ComponentListPartial()
        {
            var model = _repository.GetAll();
            return PartialView("_ComponentsPartial",model);
        }
    }
}