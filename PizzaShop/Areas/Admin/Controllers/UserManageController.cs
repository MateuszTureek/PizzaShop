using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PizzaShop.Areas.Admin.Models.ViewModels;
using PizzaShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PizzaShop.Areas.Admin.Controllers
{
    [Authorize(Roles = "superAdmin")]
    public class UserManageController : Controller
    {
        private ApplicationUserManager _userManager;

        public UserManageController()
        {
            _userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        }

        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult ListUsers()
        {
            var users = _userManager.Users.ToList();

            List<UserViewModel> model = new List<UserViewModel>();
            IList<string> roles;
            for (var i = 0; i < users.Count; ++i)
            {
                roles = Task.Run(() => _userManager.GetRolesAsync(users[i].Id)).Result;
                model.Add(new UserViewModel()
                {
                    Email = users[i].Email,
                    Id = users[i].Id,
                    UserName = users[i].UserName,
                    Roles = roles as List<string>
                });
            }
            
            return PartialView("_UserPartial", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "UserManage", new { area = "admin" });
                }
                AddErrors(new IdentityResult("User is not deleted."));
                return RedirectToAction("Index", "Home", new { area = "admin" });
            }
            return RedirectToAction("Index", "Home", new { area = "admin" });
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}