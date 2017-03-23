using Microsoft.AspNet.Identity.EntityFramework;
using PizzaShop.Areas.Admin.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PizzaShop.Areas.Admin.Controllers
{
    [Authorize(Roles ="superAdmin")]
    public class RoleManageController : Controller
    {
        private RoleStore<IdentityRole> _roleStore;

        public RoleManageController(RoleStore<IdentityRole> roleStore)
        {
            _roleStore = roleStore;
        }

        [ChildActionOnly]
        public ActionResult ListRoles()
        {
            List<IdentityRole> roles = _roleStore.Roles.ToList();
            List<RoleViewModel> model = new List<RoleViewModel>();
            for (var i = 0; i < roles.Count; ++i)
            {
                model.Add(new RoleViewModel() { Id = roles[i].Id, Name = roles[i].Name });
            }
            return PartialView("_RolePartial", model);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RoleViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            await _roleStore.CreateAsync(new IdentityRole()
            {
                Id = Guid.NewGuid().ToString(),
                Name = model.Name
            });
            return RedirectToAction("Index", "UserManage", new { area = "admin" });
        }

        public async Task<ActionResult> Delete(string id)
        {
            var role = await _roleStore.FindByIdAsync(id);
            await _roleStore.DeleteAsync(role);
            return RedirectToAction("Index", "UserManage", new { area = "admin" });
        }
    }
}