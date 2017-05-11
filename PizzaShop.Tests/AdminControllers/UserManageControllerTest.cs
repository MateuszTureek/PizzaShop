using NUnit.Framework;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PizzaShop.Services.Identity.Interfaces;
using PizzaShop.Areas.Admin.Controllers;
using System.Web.Mvc;
using PizzaShop.Models;
using PizzaShop.Areas.Admin.Models.ViewModels;
using Microsoft.AspNet.Identity;

namespace PizzaShop.Tests.AdminControllers
{
    [TestFixture]
    public class UserManageControllerTest
    {
        [Test]
        public void Index()
        {
            // Arrange 
            var service = Substitute.For<IUserService>();
            var controller = new UserManageController(service);

            // Act
            var result = controller.Index() as ViewResult;
            var viewName = result.ViewName;

            // Assert
            Assert.That(result, !Is.Null);
            Assert.That("Index", Is.EqualTo(viewName));
        }

        [Test]
        public void ListUsers()
        {
            // Arrange 
            var users = new List<ApplicationUser>()
            {
                new ApplicationUser() { UserName="superAdmin", Email="superAdmin@gmail.com", PasswordHash="" },
                new ApplicationUser() { UserName="admin", Email="admin@gmail.com", PasswordHash="" }
            };
            var usersViewModel = new List<UserViewModel>()
            {
                new UserViewModel() { UserName="superAdmin", Email="superAdmin@gmail.com", Roles=new List<string>(){ "superAdmin" } },
                new UserViewModel() { UserName="admin", Email="admin@gmail.com", Roles=new List<string>() { "admin" } }
            };
            var service = Substitute.For<IUserService>();
            var controller = new UserManageController(service);

            service.UserList().Returns(users);
            service.UsersToViewModels(users).Returns(usersViewModel);

            // Act
            var result = controller.ListUsers() as PartialViewResult;
            var viewName = result.ViewName;
            var model = result.Model;

            // Assert
            Assert.That(result, !Is.Null);
            Assert.That(model, !Is.Null);
            Assert.That("_UserPartial", Is.EqualTo(viewName));
        }

        [Test]
        public async Task Good_Delete()
        {
            // Arrange
            var id = Guid.NewGuid().ToString();
            var user = new ApplicationUser()
            {
                Id = id,
                UserName = "admin",
                Email = "admin@gmail.com",
                PasswordHash = ""
            };
            var identityResult = IdentityResult.Success;
            var service = Substitute.For<IUserService>();
            var controller = new UserManageController(service);

            service.FindUserAsync(id).Returns(Task.FromResult(user));
            service.DeleteUserAsync(user).Returns(Task.FromResult(identityResult));

            // Act
            var result = await controller.Delete(id) as RedirectToRouteResult;
            var areaName = result.RouteValues.Values.ElementAt(0);
            var actionName = result.RouteValues.Values.ElementAt(1);
            var controllerName = result.RouteValues.Values.ElementAt(2);

            // Assert
            Assert.That(result, !Is.Null);
            Assert.That("Index", Is.EqualTo(actionName));
            Assert.That("UserManage", Is.EqualTo(controllerName));
            Assert.That("admin", Is.EqualTo(areaName));
        }

        [Test]
        public async Task Delete_User_Is_Null()
        {
            // Arrange
            var id = "wdhjhfjdhj";
            ApplicationUser user = null;
            var service = Substitute.For<IUserService>();
            var controller = new UserManageController(service);

            // Act
            var result = await controller.Delete(id) as HttpNotFoundResult;
            var statusCode = result.StatusCode;

            service.FindUserAsync(id).Returns(Task.FromResult(user));

            // Assert
            Assert.That(result, !Is.Null);
            Assert.That(404, Is.EqualTo(statusCode));
        }

        [Test]
        public async Task Delete_Not_Success()
        {
            // Arrange
            var id = Guid.NewGuid().ToString();
            var user = new ApplicationUser()
            {
                Id = id,
                UserName = "admin",
                Email = "admin@gmail.com",
                PasswordHash = ""
            };
            var identityResult = IdentityResult.Failed("User not exist.");
            var service = Substitute.For<IUserService>();
            var controller = new UserManageController(service);

            service.FindUserAsync(id).Returns(Task.FromResult(user));
            service.DeleteUserAsync(user).Returns(Task.FromResult(identityResult));

            // Act
            var result = await controller.Delete(id) as HttpStatusCodeResult;
            var statusCode = result.StatusCode;

            service.FindUserAsync(id).Returns(Task.FromResult(user));

            // Assert
            Assert.That(result, !Is.Null);
            Assert.That(400, Is.EqualTo(statusCode));
        }
    }
}
