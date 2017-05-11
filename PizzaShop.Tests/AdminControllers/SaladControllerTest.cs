using NUnit.Framework;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PizzaShop.Models.PizzaShopModels.Entities;
using PizzaShop.Repositories.Shop.Interfaces;
using AutoMapper;
using PizzaShop.Areas.Admin.Controllers;
using System.Web.Mvc;
using PizzaShop.Areas.Admin.Models.ViewModels;
using PizzaShop.Tests.Classes;
using System.Web.Routing;

namespace PizzaShop.Tests.AdminControllers
{
    [TestFixture]
    public class SaladControllerTest
    {
        [Test]
        public void Index()
        {
            // Arrange
            var salads = new List<Salad>()
            {
                new Salad() { Name="Greco",Price=14.00M },
                new Salad() { Name="Pollo",Price=16.00M },
                new Salad() { Name="Mexico",Price=16.00M }
            };
            var service = Substitute.For<ISaladRepository>();
            var mapper = Substitute.For<IMapper>();
            var controller = new SaladController(service, mapper);

            controller.TempData["ModelIsNotValid"] = "Fake content.";
            controller.ViewBag.ModelIsNotValid = controller.TempData["ModelIsNotValid"];
            service.GetAll().Returns(salads);

            // Act
            var result = controller.Index() as ViewResult;
            var viewName = result.ViewName;
            var model = result.Model;
            var viewBag = controller.ViewBag.ModelIsNotValid;

            //Assert
            Assert.That(result, !Is.Null);
            Assert.That("Fake content.", Is.EqualTo(viewBag));
            Assert.That("Index", Is.EqualTo(viewName));
            Assert.That(model, !Is.Null);
        }

        [Test]
        public void CreatePartial()
        {
            // Arrange
            var service = Substitute.For<ISaladRepository>();
            var mapper = Substitute.For<IMapper>();
            var controller = new SaladController(service, mapper);

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
            var saladViewModel = new SaladViewModel()
            {
                Name = "Greco",
                Price = 14.00M,
            };
            var news = new Salad()
            {
                Name = "Greco",
                Price = 14.00M
            };
            var validator = new ModelValidator<SaladViewModel>(saladViewModel);
            var service = Substitute.For<ISaladRepository>();
            var mapper = Substitute.For<IMapper>();
            var controller = new SaladController(service, mapper);

            mapper.Map<SaladViewModel, Salad>(saladViewModel).Returns(news);
            service.Insert(news);
            service.Save();

            // Act
            var valid = validator.IsValid();
            validator.AddToModelError(controller);
            var result = controller.Create(saladViewModel) as RedirectToRouteResult;
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
            var saladViewModel = new SaladViewModel()
            {
                Name = "Greco",
                //Price = 14.00M,
            };
            var validator = new ModelValidator<SaladViewModel>(saladViewModel);
            var service = Substitute.For<ISaladRepository>();
            var mapper = Substitute.For<IMapper>();

            var controller = new SaladController(service, mapper);

            // Act
            var valid = validator.IsValid();
            validator.AddToModelError(controller);
            var result = controller.Create(saladViewModel) as RedirectToRouteResult;
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
            var salad = new Salad()
            {
                ID = id,
                Name = "Greco",
                Price = 14.00M,
            };
            var service = Substitute.For<ISaladRepository>();
            var mapper = Substitute.For<IMapper>();
            var fakeController = new FakeController();
            var controller = new SaladController(service, mapper);

            fakeController.PrepareFakeAjaxRequest();
            controller.ControllerContext = fakeController.GetControllerContext<SaladController>(new RouteData(), controller);
            service.Get(id).Returns(salad);
            service.Delete(salad);
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
            var service = Substitute.For<ISaladRepository>();
            var mapper = Substitute.For<IMapper>();
            var controller = new SaladController(service, mapper);

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
            var salad = new Salad()
            {
                ID = id,
                Name = "Greco",
                Price = 14.00M,
            };
            var service = Substitute.For<ISaladRepository>();
            var mapper = Substitute.For<IMapper>();
            var fakeController = new FakeController();
            var controller = new SaladController(service, mapper);

            fakeController.PrepareFakeRequest();
            controller.ControllerContext = fakeController.GetControllerContext<SaladController>(new RouteData(), controller);
            service.Get(id).Returns(salad);

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
        public void Delete_Salad_Null()
        {
            // Arrange
            var id = -1;
            Salad salad = null;
            var service = Substitute.For<ISaladRepository>();
            var mapper = Substitute.For<IMapper>();
            var controller = new SaladController(service, mapper);

            service.Get(id).Returns(salad);

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
            var salad = new Salad()
            {
                ID = id,
                Name = "Greco",
                Price = 14.00M,
            };
            var saladViewModel = new SaladViewModel()
            {
                ID = id,
                Name = "Greco",
                Price = 14.00M,
            };
            var service = Substitute.For<ISaladRepository>();
            var mapper = Substitute.For<IMapper>();
            var fakeController = new FakeController();
            var controller = new SaladController(service, mapper);

            fakeController.PrepareFakeAjaxRequest();
            controller.ControllerContext = fakeController.GetControllerContext<SaladController>(new RouteData(), controller);
            service.Get(id).Returns(salad);
            mapper.Map<Salad, SaladViewModel>(salad).Returns(saladViewModel);

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
            var service = Substitute.For<ISaladRepository>();
            var mapper = Substitute.For<IMapper>();
            var controller = new SaladController(service, mapper);

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
            var salad = new Salad()
            {
                ID = id,
                Name = "Greco",
                Price = 14.00M,
            };
            var saladViewModel = new SaladViewModel()
            {
                ID = id,
                Name = "Greco",
                Price = 14.00M,
            };
            var service = Substitute.For<ISaladRepository>();
            var mapper = Substitute.For<IMapper>();
            var fakeController = new FakeController();
            var controller = new SaladController(service, mapper);

            fakeController.PrepareFakeRequest();
            controller.ControllerContext = fakeController.GetControllerContext<SaladController>(new RouteData(), controller);
            service.Get(id).Returns(salad);
            mapper.Map<Salad, SaladViewModel>(salad).Returns(saladViewModel);

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
            Salad salad = null;
            var service = Substitute.For<ISaladRepository>();
            var mapper = Substitute.For<IMapper>();
            var controller = new SaladController(service, mapper);

            service.Get(id).Returns(salad);

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
            var salad = new Salad()
            {
                ID = 1,
                Name = "Greco",
                Price = 14.00M,
            };
            var saladViewModel = new SaladViewModel()
            {
                ID = 1,
                Name = "Greco",
                Price = 14.00M,
            };
            var service = Substitute.For<ISaladRepository>();
            var mapper = Substitute.For<IMapper>();
            var validator = new ModelValidator<SaladViewModel>(saladViewModel);
            var fakeController = new FakeController();
            var controller = new SaladController(service, mapper);

            fakeController.PrepareFakeAjaxRequest();
            controller.ControllerContext = fakeController.GetControllerContext<SaladController>(new RouteData(), controller);
            service.Get(salad.ID).Returns(salad);
            mapper.Map<SaladViewModel, Salad>(saladViewModel).Returns(salad);
            service.Update(salad);
            service.Save();

            // Act
            var valid = validator.IsValid();
            validator.AddToModelError(controller);
            var result = controller.Edit(saladViewModel) as RedirectToRouteResult;
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
            Salad salad = null;
            var saladViewModel = new SaladViewModel()
            {
                ID = id,
                Name = "Greco",
                Price = 14.00M,
            };
            var service = Substitute.For<ISaladRepository>();
            var mapper = Substitute.For<IMapper>();
            var validator = new ModelValidator<SaladViewModel>(saladViewModel);
            var controller = new SaladController(service, mapper);

            service.Get(id).Returns(salad);

            // Act
            var valid = validator.IsValid();
            validator.AddToModelError(controller);
            var result = controller.Edit(saladViewModel) as HttpNotFoundResult;
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
            var saladViewModel = new SaladViewModel()
            {
                ID = 1,
                //Name = "Greco",
                Price = 14.00M,
            };
            var service = Substitute.For<ISaladRepository>();
            var mapper = Substitute.For<IMapper>();
            var controller = new SaladController(service, mapper);
            var validator = new ModelValidator<SaladViewModel>(saladViewModel);

            // Act
            var valid = validator.IsValid();
            validator.AddToModelError(controller);
            var result = controller.Edit(saladViewModel) as RedirectToRouteResult;
            var actionName = result.RouteValues.Values.ElementAt(0);
            var tempData = controller.TempData["ModelIsNotValid"];

            // Assert
            Assert.That(result, !Is.Null);
            Assert.That(valid, Is.False);
            Assert.That("Index", Is.EqualTo(actionName));
            Assert.That("Wystąpił błąd w formularzu, spróbuj ponownie.", Is.EqualTo(tempData));
        }
    }
}
