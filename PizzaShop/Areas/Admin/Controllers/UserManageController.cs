using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PizzaShop.Areas.Admin.Models.ViewModels;
using PizzaShop.Models;
using PizzaShop.Services.Identity.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PizzaShop.Areas.Admin.Controllers
{
    [Authorize(Roles = "superAdmin")]
    public class UserManageController : Controller
    {
        readonly IUserService _service;

        public UserManageController(IUserService service)
        {
            _service = service;
        }

        public ActionResult Index()
        {
            return View("Index");
        }

        [ChildActionOnly]
        public ActionResult ListUsers()
        {
            var users = _service.UserList();
            var userViewModelList = _service.UsersToViewModels(users);
            return PartialView("_UserPartial", userViewModelList);
        }

        public async Task<ActionResult> Delete(string id)
        {
            if (id == string.Empty)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var user = await _service.FindUserAsync(id);
            if (user == null)
                return HttpNotFound();
            var result = await _service.DeleteUserAsync(user);
            if (!result.Succeeded)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            return RedirectToAction("Index", "UserManage", new { area = "admin" });
        }
    }
}