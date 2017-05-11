using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using System.Web.Mvc;
using AutoMapper;
using PizzaShop.Models.PizzaShopModels.CMS;
using PizzaShop.Repositories.CMS.Interfaces;
using PizzaShop.Areas.Admin.Controllers;
using PizzaShop.Areas.Admin.Models.ViewModels;
using PizzaShop.Tests.Classes;
using System.Web.Routing;

namespace PizzaShop.Tests.AdminControllers
{
    [TestFixture]
    public class MenuItemControllerTest
    {
        [Test]
        public void Index()
        {
            // Arrange
            var menuItems = new List<MenuItem>()
            {
                new MenuItem() { Position=1, Title="Strona główna", ActionName="Index", ControllerName="Home" },
                new MenuItem() { Position=2, Title="Menu", ActionName="Pizza", ControllerName="Menu" },
                new MenuItem() { Position=3, Title="Galeria", ActionName="Gallery", ControllerName="Home" },
                new MenuItem() { Position=4, Title="Kontakt", ActionName="Contact", ControllerName="Home" }
            };
            var service = Substitute.For<IMenuItemRepository>();
            var mapper = Substitute.For<IMapper>();
            var controller = new MenuItemController(service, mapper);
            controller.TempData["ModelIsNotValid"] = "Fake content.";
            controller.ViewBag.ModelIsNotValid = controller.TempData["ModelIsNotValid"];

            service.GetAll().Returns(menuItems);

            // Act
            var result = controller.Index() as ViewResult;
            var viewName = result.ViewName;
            var model = result.Model;
            var viewBag = controller.ViewBag.ModelIsNotValid;

            //Assert
            Assert.That(result, !Is.Null);
            Assert.That("Index", Is.EqualTo(viewName));
            Assert.That(model, !Is.Null);
            Assert.That("Fake content.", Is.EqualTo(viewBag));
        }

        [Test]
        public void CreatePartial()
        {
            // Arrange
            var service = Substitute.For<IMenuItemRepository>();
            var mapper = Substitute.For<IMapper>();
            var controller = new MenuItemController(service, mapper);

            // Act
            var result = controller.CreatePartial() as PartialViewResult;
            var viewName = result.ViewName;
            var model = result.Model;

            // Assert
            Assert.That(result, !Is.Null);
            Assert.That("_CreatePartial", Is.EqualTo(viewName));
            Assert.That(model, Is.Null);
        }

        [Test]
        public void Good_Post_Create()
        {
            // Arrange
            var menuItemViewModel = new MenuItemViewModel()
            {
                Position = 1,
                Title = "Strona glowna",
                ActionName = "Index",
                ControllerName = "Home"
            };
            var menuItem = new MenuItem()
            {
                Position = 1,
                Title = "Strona glowna",
                ActionName = "Index",
                ControllerName = "Home"
            };
            var validator = new ModelValidator<MenuItemViewModel>(menuItemViewModel);
            var service = Substitute.For<IMenuItemRepository>();
            var mapper = Substitute.For<IMapper>();
            var controller = new MenuItemController(service, mapper);

            mapper.Map<MenuItemViewModel, MenuItem>(menuItemViewModel).Returns(menuItem);
            service.Insert(menuItem);
            service.Save();

            // Act
            var valid = validator.IsValid();
            validator.AddToModelError(controller);
            var result = controller.Create(menuItemViewModel) as RedirectToRouteResult;
            var actionName = result.RouteValues.Values.ElementAt(0);

            // Assert
            Assert.That(result, !Is.Null);
            Assert.That("Index", Is.EqualTo(actionName));
            Assert.That(valid, Is.True);

        }

        [Test]
        public void Post_Create_Model_Not_Valid()
        {
            // Arrange
            var menuItemViewModel = new MenuItemViewModel()
            {
                Position = 1,
                Title = "Strona główna",
                //ActionName = "Index",
                //ControllerName = "Home"
            };
            var validator = new ModelValidator<MenuItemViewModel>(menuItemViewModel);
            var service = Substitute.For<IMenuItemRepository>();
            var mapper = Substitute.For<IMapper>();

            var controller = new MenuItemController(service, mapper);

            // Act
            var valid = validator.IsValid();
            validator.AddToModelError(controller);
            var result = controller.Create(menuItemViewModel) as RedirectToRouteResult;
            var actionName = result.RouteValues.Values.ElementAt(0);
            var tempData = controller.TempData["ModelIsNotValid"];

            // Assert
            Assert.That(valid, Is.False);
            Assert.That(result, !Is.Null);
            Assert.That("Index", Is.EqualTo(actionName));
            Assert.That("Wystąpił błąd w formularzu, spróbuj ponownie.", Is.EqualTo(tempData));
        }

        [Test]
        public void Good_Delete()
        {
            // Arrange
            var id = 1;
            var menuItem = new MenuItem()
            {
                ID = 1,
                Position = 1,
                Title = "Strona główna",
                ActionName = "Index",
                ControllerName = "Home"
            };
            var service = Substitute.For<IMenuItemRepository>();
            var mapper = Substitute.For<IMapper>();
            var fakeController = new FakeController();
            var controller = new MenuItemController(service, mapper);

            fakeController.PrepareFakeAjaxRequest();
            controller.ControllerContext = fakeController.GetControllerContext<MenuItemController>(new RouteData(), controller);
            service.Get(id).Returns(menuItem);
            service.Delete(menuItem);
            service.Save();

            // Act
            var result = controller.Delete(id) as JsonResult;
            var ajaxRequest = controller.Request.IsAjaxRequest();
            var jsonRequestBehavior = result.JsonRequestBehavior;
            var data = result.Data;

            // Assert
            Assert.That(result, !Is.Null);
            Assert.That(ajaxRequest, Is.True);
            Assert.That(JsonRequestBehavior.AllowGet, Is.EqualTo(jsonRequestBehavior));
            Assert.That("", Is.EqualTo(data));
        }

        [Test]
        public void Delete_Id_Is_Null()
        {
            // Arrange
            int? id = null;
            var mapper = Substitute.For<IMapper>();
            var service = Substitute.For<IMenuItemRepository>();
            var controller = new MenuItemController(service, mapper);

            // Act
            var result = controller.Delete(id) as HttpStatusCodeResult;
            var statusCode = result.StatusCode;

            // Assert
            Assert.That(result, !Is.Null);
            Assert.That(400, Is.EqualTo(statusCode));
        }

        [Test]
        public void Delete_Not_Ajax_Request()
        {
            // Arrange
            var id = 1;
            var menuItem = new MenuItem()
            {
                ID = 1,
                Position = 1,
                Title = "Strona główna",
                ActionName = "Index",
                ControllerName = "Home"
            };
            var service = Substitute.For<IMenuItemRepository>();
            var mapper = Substitute.For<IMapper>();
            var fakeController = new FakeController();
            var controller = new MenuItemController(service, mapper);

            fakeController.PrepareFakeRequest();
            controller.ControllerContext = fakeController.GetControllerContext<MenuItemController>(new RouteData(), controller);
            service.Get(id).Returns(menuItem);

            // Act
            var result = controller.Delete(id) as RedirectToRouteResult;
            var ajaxRequest = controller.Request.IsAjaxRequest();
            var actionName = result.RouteValues.Values.ElementAt(0);

            // Assert
            Assert.That(result, !Is.Null);
            Assert.That(ajaxRequest, Is.False);
            Assert.That("Index", Is.EqualTo(actionName));
        }

        [Test]
        public void Delete_Model_Null()
        {
            // Arrange
            var id = -1;
            MenuItem menuItem = null;
            var service = Substitute.For<IMenuItemRepository>();
            var mapper = Substitute.For<IMapper>();
            var controller = new MenuItemController(service, mapper);

            service.Get(id).Returns(menuItem);

            // Act
            var result = controller.Delete(id) as HttpNotFoundResult;
            var statusCode = result.StatusCode;

            // Assert
            Assert.That(result, !Is.Null);
            Assert.That(404, Is.EqualTo(statusCode));
        }

        [Test]
        public void Good_Get_Edit()
        {
            // Arrange
            var id = 1;
            var menuItem = new MenuItem()
            {
                ID = 1,
                Position = 1,
                Title = "Strona główna",
                ActionName = "Index",
                ControllerName = "Home"
            };
            var menuItemViewModel = new MenuItemViewModel()
            {
                ID = 1,
                Position = 1,
                Title = "Strona główna",
                ActionName = "Index",
                ControllerName = "Home"
            };
            var service = Substitute.For<IMenuItemRepository>();
            var mapper = Substitute.For<IMapper>();
            var fakeController = new FakeController();
            var controller = new MenuItemController(service, mapper);

            fakeController.PrepareFakeAjaxRequest();
            controller.ControllerContext = fakeController.GetControllerContext<MenuItemController>(new RouteData(), controller);
            service.Get(id).Returns(menuItem);
            mapper.Map<MenuItem, MenuItemViewModel>(menuItem).Returns(menuItemViewModel);

            // Act
            var result = controller.Edit(id) as PartialViewResult;
            var viewName = result.ViewName;
            var model = result.Model;
            var ajaxRequest = controller.Request.IsAjaxRequest();

            // Assert
            Assert.That(result, !Is.Null);
            Assert.That("_EditPartial", Is.EqualTo(viewName));
            Assert.That(ajaxRequest, Is.True);
            Assert.That(model, !Is.Null);
        }

        [Test]
        public void Get_Edit_Id_Is_Null()
        {
            // Arrange
            int? id = null;
            var service = Substitute.For<IMenuItemRepository>();
            var mapper = Substitute.For<IMapper>();
            var fakeController = new FakeController();
            var controller = new MenuItemController(service, mapper);

            // Act
            var result = controller.Edit(id) as HttpStatusCodeResult;
            var statusCode = result.StatusCode;

            // Assert
            Assert.That(result, !Is.Null);
            Assert.That(400, Is.EqualTo(statusCode));

        }

        [Test]
        public void Get_Edit_Not_Ajax_Request()
        {
            // Arrange
            var id = 1;
            var menuItem = new MenuItem()
            {
                ID = 1,
                Position = 1,
                Title = "Strona główna",
                ActionName = "Index",
                ControllerName = "Home"
            };
            var menuItemViewModel = new MenuItemViewModel()
            {
                ID = 1,
                Position = 1,
                Title = "Strona główna",
                ActionName = "Index",
                ControllerName = "Home"
            };
            var service = Substitute.For<IMenuItemRepository>();
            var mapper = Substitute.For<IMapper>();
            var fakeController = new FakeController();
            var controller = new MenuItemController(service, mapper);

            fakeController.PrepareFakeRequest();
            controller.ControllerContext = fakeController.GetControllerContext<MenuItemController>(new RouteData(), controller);
            service.Get(id).Returns(menuItem);
            mapper.Map<MenuItem, MenuItemViewModel>(menuItem).Returns(menuItemViewModel);

            // Act
            var result = controller.Edit(id) as RedirectToRouteResult;
            var actionName = result.RouteValues.Values.ElementAt(0);
            var ajaxRequest = controller.Request.IsAjaxRequest();

            // Assert
            Assert.That(result, !Is.Null);
            Assert.That("Index", Is.EqualTo(actionName));
            Assert.That(ajaxRequest, Is.False);
        }

        [Test]
        public void Get_Edit_Model_Is_Null()
        {
            // Arrange
            var id = 1;
            MenuItem menuItem = null;
            var service = Substitute.For<IMenuItemRepository>();
            var mapper = Substitute.For<IMapper>();
            var controller = new MenuItemController(service, mapper);

            service.Get(id).Returns(menuItem);

            // Act
            var result = controller.Edit(id) as HttpNotFoundResult;
            var statusCode = result.StatusCode;
            
            // Assert
            Assert.That(result, !Is.Null);
            Assert.That(404, Is.EqualTo(statusCode));
        }

        [Test]
        public void Good_Post_Edit()
        {
            // Arrange
            var menuItem = new MenuItem()
            {
                ID = 1,
                Position = 1,
                Title = "Strona glowna",
                ActionName = "Index",
                ControllerName = "Home"
            };
            var menuItemViewModel = new MenuItemViewModel()
            {
                ID = 1,
                Position = 1,
                Title = "Strona glowna",
                ActionName = "Index",
                ControllerName = "Home"
            };
            var service = Substitute.For<IMenuItemRepository>();
            var mapper = Substitute.For<IMapper>();
            var validator = new ModelValidator<MenuItemViewModel>(menuItemViewModel);
            var fakeController = new FakeController();
            var controller = new MenuItemController(service, mapper);

            fakeController.PrepareFakeAjaxRequest();
            controller.ControllerContext = fakeController.GetControllerContext<MenuItemController>(new RouteData(), controller);
            service.Get(menuItem.ID).Returns(menuItem);
            mapper.Map<MenuItemViewModel, MenuItem>(menuItemViewModel).Returns(menuItem);
            service.Update(menuItem);
            service.Save();

            // Act
            var valid = validator.IsValid();
            validator.AddToModelError(controller);
            var result = controller.Edit(menuItemViewModel) as RedirectToRouteResult;
            var actionName = result.RouteValues.Values.ElementAt(0);
            var ajaxRequest = controller.Request.IsAjaxRequest();

            // Assert
            Assert.That(result, !Is.Null);
            Assert.That("Index", Is.EqualTo(actionName));
            Assert.That(ajaxRequest, Is.True);
            Assert.That(valid, Is.True);
        }

        [Test]
        public void Post_Edit_Model_Is_Null()
        {
            // Arrange
            var id = -1;
            MenuItem menuItem = null;
            var menuItemViewModel = new MenuItemViewModel()
            {
                ID = 1,
                Position = 1,
                Title = "Strona glowna",
                ActionName = "Index",
                ControllerName = "Home"
            };
            var service = Substitute.For<IMenuItemRepository>();
            var mapper = Substitute.For<IMapper>();
            var validator = new ModelValidator<MenuItemViewModel>(menuItemViewModel);
            var controller = new MenuItemController(service, mapper);

            service.Get(id).Returns(menuItem);

            // Act
            var valid = validator.IsValid();
            validator.AddToModelError(controller);
            var result = controller.Edit(menuItemViewModel) as HttpNotFoundResult;
            var statusCode = result.StatusCode;

            // Assert
            Assert.That(result, !Is.Null);
            Assert.That(valid, Is.True);
            Assert.That(404, Is.EqualTo(statusCode));
        }

        [Test]
        public void Post_Edit_Is_Not_Valid()
        {
            // Arrange
            var menuItemViewModel = new MenuItemViewModel()
            {
                ID = 1,
                //Position = 1,
                Title = "Strona główna",
                //ActionName = "Index",
                ControllerName = "Home"
            };
            var service = Substitute.For<IMenuItemRepository>();
            var mapper = Substitute.For<IMapper>();
            var controller = new MenuItemController(service, mapper);
            var validator = new ModelValidator<MenuItemViewModel>(menuItemViewModel);

            // Act
            var valid = validator.IsValid();
            validator.AddToModelError(controller);
            var result = controller.Edit(menuItemViewModel) as RedirectToRouteResult;
            var actionName = result.RouteValues.Values.ElementAt(0);
            var tempData = controller.TempData["ModelIsNotValid"];

            // Assert
            Assert.That(result, !Is.Null);
            Assert.That(valid, Is.False);
            Assert.That("Index", Is.EqualTo(actionName));
        }
    }
}
