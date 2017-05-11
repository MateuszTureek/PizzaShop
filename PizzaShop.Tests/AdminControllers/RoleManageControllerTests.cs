using NUnit.Framework;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using PizzaShop.Areas.Admin.Models.ViewModels;
using PizzaShop.Areas.Admin.Controllers;
using AutoMapper;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using PizzaShop.Services.Identity.Interfaces;
using PizzaShop.Tests.Classes;

namespace PizzaShop.Tests.AdminControllers
{
    [TestFixture]
    public class RoleManageControllerTests
    {
        [Test]
        public void ListRoles()
        {
            // Arrange
            var id_1 = Guid.NewGuid().ToString();
            var id_2 = Guid.NewGuid().ToString();
            var roles = new List<IdentityRole>()
            {
                new IdentityRole() { Id=id_1, Name ="admin" },
                new IdentityRole() { Id=id_2, Name ="superAdmin" }
            };
            var modelRoles = new List<RoleViewModel>()
            {
                new RoleViewModel() { Id=id_1, Name ="admin" },
                new RoleViewModel() { Id=id_2, Name ="superAdmin" }
            };
            var service = Substitute.For<IRoleService>();
            var mapper = Substitute.For<IMapper>();
            var controller = new RoleManageController(service, mapper);

            service.RoleList().Returns(roles);
            service.MapRoleListToViewModelList(roles).Returns(modelRoles);
            // Act
            var result = controller.ListRoles() as PartialViewResult;
            var viewName = result.ViewName;
            var model = result.Model;

            // Assert
            Assert.That(result, !Is.Null);
            Assert.That("_RolePartial", Is.EqualTo(viewName));
            Assert.That(model, !Is.Null);
        }

        [Test]
        public void Get_Create()
        {
            // Arrange
            var mapper = Substitute.For<IMapper>();
            var service = Substitute.For<IRoleService>();
            var controller = new RoleManageController(service, mapper);

            // Act
            var result = controller.Create() as ViewResult;
            var viewName = result.ViewName;

            // Assert
            Assert.That("Create", Is.EqualTo(viewName));
        }

        [Test]
        public async Task Post_Create_Model_Not_Valid()
        {
            // Arrange
            var roleViewModel = new RoleViewModel
            {
                //Name ="admin"
            };
            var validator = new ModelValidator<RoleViewModel>(roleViewModel);
            var service = Substitute.For<IRoleService>();
            var mapper = Substitute.For<IMapper>();

            var controller = new RoleManageController(service, mapper);

            // Act
            var valid = validator.IsValid();
            validator.AddToModelError(controller);
            var result = await controller.Create(roleViewModel) as ViewResult;
            var viewName = result.ViewName;
            var model = result.Model;

            // Assert
            Assert.That(valid, Is.False);
            Assert.That(result, !Is.Null);
            Assert.That(model, !Is.Null);
            Assert.That("Create", Is.EqualTo(viewName));
        }

        [Test]
        public async Task Post_Create_Model_Is_Valid()
        {
            // Arrange
            string id = Guid.NewGuid().ToString();
            var roleViewModel = new RoleViewModel()
            {
                Id = id,
                Name = "admin"
            };
            var validator = new ModelValidator<RoleViewModel>(roleViewModel);
            var service = Substitute.For<IRoleService>();
            var mapper = Substitute.For<IMapper>();
            var controller = new RoleManageController(service, mapper);

            await service.CreateRoleAsync(roleViewModel.Name);

            // Act
            var valid = validator.IsValid();
            validator.AddToModelError(controller);
            var result = await controller.Create(roleViewModel) as RedirectToRouteResult;
            var areaName = result.RouteValues.Values.ElementAt(0);
            var actionName = result.RouteValues.Values.ElementAt(1);
            var controllerName = result.RouteValues.Values.ElementAt(2);

            // Assert
            Assert.That(result, !Is.Null);
            Assert.That(valid, Is.True);
            Assert.That("Index", Is.EqualTo(actionName));
            Assert.That("UserManage", Is.EqualTo(controllerName));
            Assert.That("admin", Is.EqualTo(areaName));
        }

        [Test]
        public async Task Good_Delete()
        {
            // Arrange
            string id = Guid.NewGuid().ToString();
            var role = new IdentityRole()
            {
                Id = id,
                Name = "admin"
            };
            var service = Substitute.For<IRoleService>();
            var mapper = Substitute.For<IMapper>();
            var controller = new RoleManageController(service, mapper);

            service.FindByIdAsync(id).Returns(Task.FromResult(role));
            await service.DeleteRoleAcync(role);

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
        public async Task Delete_Id_Is_Null()
        {
            // Arrange
            string id = string.Empty;
            var service = Substitute.For<IRoleService>();
            var mapper = Substitute.For<IMapper>();
            var controller = new RoleManageController(service, mapper);

            // Act
            var result = await controller.Delete(id) as HttpStatusCodeResult;
            var statusCode = result.StatusCode;

            // Assert
            Assert.That(result, !Is.Null);
            Assert.That(400, Is.EqualTo(statusCode));
        }

        [Test]
        public async Task Delete_Role_Is_Null()
        {
            // Arrange
            string id = "hkddkj";
            IdentityRole role = null;
            var service = Substitute.For<IRoleService>();
            var mapper = Substitute.For<IMapper>();
            var controller = new RoleManageController(service, mapper);

            service.FindByIdAsync(id).Returns(Task.FromResult(role));

            // Act
            var result = await controller.Delete(id) as HttpNotFoundResult;
            var statusCode = result.StatusCode;

            // Assert
            Assert.That(result, !Is.Null);
            Assert.That(404, Is.EqualTo(statusCode));
        }
    }
}
