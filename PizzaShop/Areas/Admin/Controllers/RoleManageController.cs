using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PizzaShop.Areas.Admin.Models.ViewModels;
using PizzaShop.Services.Identity.Interfaces;
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
        readonly IRoleService _service;
        readonly IMapper _mapper;

        public RoleManageController(IRoleService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [ChildActionOnly]
        public ActionResult ListRoles()
        {
            var roles = _service.RoleList();
            var model = _mapper.Map<List<IdentityRole>, List<RoleViewModel>>(roles);
            return PartialView("_RolePartial", model);
        }

        public ActionResult Create()
        {
            return View("Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", model);
            }
            await _service.CreateRoleAsync(model.Name);
            return RedirectToAction("Index", "UserManage", new { area = "admin" });
        }

        public async Task<ActionResult> Delete(string id)
        {
            var role = await _service.FindByIdAsync(id);
            if (role != null)
            {
                await _service.DeleteRoleAcync(role);
                return RedirectToAction("Index", "UserManage", new { area = "admin" });
            }
            return HttpNotFound();
        }
    }
}