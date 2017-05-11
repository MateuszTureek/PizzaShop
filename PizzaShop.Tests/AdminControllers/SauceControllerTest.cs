using AutoMapper;
using NSubstitute;
using NUnit.Framework;
using PizzaShop.Areas.Admin.Controllers;
using PizzaShop.Areas.Admin.Models.ViewModels;
using PizzaShop.Models.PizzaShopModels.Entities;
using PizzaShop.Repositories.Shop.Interfaces;
using PizzaShop.Tests.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace PizzaShop.Tests.AdminControllers
{
    [TestFixture]
    public class SauceControllerTest
    {
        [Test]
        public void Index()
        {
            // Arrange
            var sauces = new List<Sauce>()
            {
                new Sauce() { Name="Ostry",Price=2.00M },
                new Sauce() { Name="Czosnkowo-ziołowy",Price=2.00M },
                new Sauce() { Name="Pomodorowy",Price=3.00M }
            };
            var service = Substitute.For<ISauceRepository>();
            var mapper = Substitute.For<IMapper>();
            var controller = new SauceController(service, mapper);

            controller.TempData["ModelIsNotValid"] = "Fake content.";
            controller.ViewBag.ModelIsNotValid = controller.TempData["ModelIsNotValid"];
            service.GetAll().Returns(sauces);

            // Act
            var result = controller.Index() as ViewResult;
            var viewName = result.ViewName;
            var model = result.Model;
            var tempData = controller.TempData["ModelIsNotValid"];

            //Assert
            Assert.That(result, !Is.Null);
            Assert.That("Fake content.", Is.EqualTo(tempData));
            Assert.That("Index", Is.EqualTo(viewName));
            Assert.That(model, !Is.Null);
        }

        [Test]
        public void CreatePartial()
        {
            // Arrange
            var service = Substitute.For<ISauceRepository>();
            var mapper = Substitute.For<IMapper>();
            var controller = new SauceController(service, mapper);

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
            var sauceViewModel = new SauceViewModel()
            {
                Name = "Ostry",
                Price = 2.00M
            };
            var sauce = new Sauce()
            {
                Name = "Ostry",
                Price = 2.00M
            };
            var validator = new ModelValidator<SauceViewModel>(sauceViewModel);
            var service = Substitute.For<ISauceRepository>();
            var mapper = Substitute.For<IMapper>();
            var controller = new SauceController(service, mapper);

            mapper.Map<SauceViewModel, Sauce>(sauceViewModel).Returns(sauce);
            service.Insert(sauce);
            service.Save();

            // Act
            var valid = validator.IsValid();
            validator.AddToModelError(controller);
            var result = controller.Create(sauceViewModel) as RedirectToRouteResult;
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
            var sauceViewModel = new SauceViewModel()
            {
                Name = "Ostry",
                //Price = 2.00M
            };
            var validator = new ModelValidator<SauceViewModel>(sauceViewModel);
            var service = Substitute.For<ISauceRepository>();
            var mapper = Substitute.For<IMapper>();

            var controller = new SauceController(service, mapper);

            // Act
            var valid = validator.IsValid();
            validator.AddToModelError(controller);
            var result = controller.Create(sauceViewModel) as RedirectToRouteResult;
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
            var sauce = new Sauce()
            {
                ID = id,
                Name = "Ostry",
                Price = 2.00M
            };
            var service = Substitute.For<ISauceRepository>();
            var mapper = Substitute.For<IMapper>();
            var fakeController = new FakeController();
            var controller = new SauceController(service, mapper);

            fakeController.PrepareFakeAjaxRequest();
            controller.ControllerContext = fakeController.GetControllerContext<SauceController>(new RouteData(), controller);
            service.Get(id).Returns(sauce);
            service.Delete(sauce);
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
        public void Delete_Id_Is_Not_Null()
        {
            // Arrange
            int? id = null;
            var service = Substitute.For<ISauceRepository>();
            var mapper = Substitute.For<IMapper>();
            var controller = new SauceController(service, mapper);

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
            var sauce = new Sauce()
            {
                ID = id,
                Name = "Ostry",
                Price = 2.00M
            };
            var service = Substitute.For<ISauceRepository>();
            var mapper = Substitute.For<IMapper>();
            var fakeController = new FakeController();
            var controller = new SauceController(service, mapper);

            fakeController.PrepareFakeRequest();
            controller.ControllerContext = fakeController.GetControllerContext<SauceController>(new RouteData(), controller);
            service.Get(id).Returns(sauce);

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
            Sauce sauce = null;
            var service = Substitute.For<ISauceRepository>();
            var mapper = Substitute.For<IMapper>();
            var controller = new SauceController(service, mapper);

            service.Get(id).Returns(sauce);

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
            var sauce = new Sauce()
            {
                ID = id,
                Name = "Ostry",
                Price = 2.00M
            };
            var sauceViewModel = new SauceViewModel()
            {
                ID = id,
                Name = "Ostry",
                Price = 2.00M
            };
            var service = Substitute.For<ISauceRepository>();
            var mapper = Substitute.For<IMapper>();
            var fakeController = new FakeController();
            var controller = new SauceController(service, mapper);

            fakeController.PrepareFakeAjaxRequest();
            controller.ControllerContext = fakeController.GetControllerContext<SauceController>(new RouteData(), controller);
            service.Get(id).Returns(sauce);
            mapper.Map<Sauce, SauceViewModel>(sauce).Returns(sauceViewModel);

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
            var service = Substitute.For<ISauceRepository>();
            var mapper = Substitute.For<IMapper>();
            var controller = new SauceController(service, mapper);

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
            var sauce = new Sauce()
            {
                ID = id,
                Name = "Ostry",
                Price = 2.00M
            };
            var sauceViewModel = new SauceViewModel()
            {
                ID = id,
                Name = "Ostry",
                Price = 2.00M
            };
            var service = Substitute.For<ISauceRepository>();
            var mapper = Substitute.For<IMapper>();
            var fakeController = new FakeController();
            var controller = new SauceController(service, mapper);

            fakeController.PrepareFakeRequest();
            controller.ControllerContext = fakeController.GetControllerContext<SauceController>(new RouteData(), controller);
            service.Get(id).Returns(sauce);
            mapper.Map<Sauce, SauceViewModel>(sauce).Returns(sauceViewModel);

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
            Sauce sauce = null;
            var service = Substitute.For<ISauceRepository>();
            var mapper = Substitute.For<IMapper>();
            var controller = new SauceController(service, mapper);

            service.Get(id).Returns(sauce);

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
            var sauce = new Sauce()
            {
                ID = 1,
                Name = "Ostry",
                Price = 2.00M
            };
            var sauceViewModel = new SauceViewModel()
            {
                ID = 1,
                Name = "Ostry",
                Price = 2.00M
            };
            var service = Substitute.For<ISauceRepository>();
            var mapper = Substitute.For<IMapper>();
            var validator = new ModelValidator<SauceViewModel>(sauceViewModel);
            var fakeController = new FakeController();
            var controller = new SauceController(service, mapper);

            fakeController.PrepareFakeAjaxRequest();
            controller.ControllerContext = fakeController.GetControllerContext<SauceController>(new RouteData(), controller);
            service.Get(sauce.ID).Returns(sauce);
            mapper.Map<SauceViewModel, Sauce>(sauceViewModel).Returns(sauce);
            service.Update(sauce);
            service.Save();

            // Act
            var valid = validator.IsValid();
            validator.AddToModelError(controller);
            var result = controller.Edit(sauceViewModel) as RedirectToRouteResult;
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
            Sauce sauce = null;
            var sauceViewModel = new SauceViewModel()
            {
                ID = id,
                Name = "Ostry",
                Price = 2.00M
            };
            var service = Substitute.For<ISauceRepository>();
            var mapper = Substitute.For<IMapper>();
            var validator = new ModelValidator<SauceViewModel>(sauceViewModel);
            var controller = new SauceController(service, mapper);

            service.Get(id).Returns(sauce);

            // Act
            var valid = validator.IsValid();
            validator.AddToModelError(controller);
            var result = controller.Edit(sauceViewModel) as HttpNotFoundResult;
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
            var sauceViewModel = new SauceViewModel()
            {
                ID = 1,
                //Name = "Ostry",
                Price = 2.00M
            };
            var service = Substitute.For<ISauceRepository>();
            var mapper = Substitute.For<IMapper>();
            var controller = new SauceController(service, mapper);
            var validator = new ModelValidator<SauceViewModel>(sauceViewModel);

            // Act
            var valid = validator.IsValid();
            validator.AddToModelError(controller);
            var result = controller.Edit(sauceViewModel) as RedirectToRouteResult;
            var actionName = result.RouteValues.Values.ElementAt(0);
            var tempData = controller.TempData["ModelIsNotValid"];

            // Assert
            Assert.That(result, !Is.Null);
            Assert.That(valid, Is.False);
            Assert.That("Wystąpił błąd w formularzu, spróbuj ponownie.", Is.EqualTo(tempData));
            Assert.That("Index", Is.EqualTo(actionName));
        }
    }
}
