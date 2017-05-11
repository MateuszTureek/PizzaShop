using System;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;
using NSubstitute;
using PizzaShop.Repositories.Shop.Interfaces;
using PizzaShop.Areas.Admin.Controllers;
using PizzaShop.Models.PizzaShopModels.Entities;
using System.Web.Mvc;
using AutoMapper;
using PizzaShop.Areas.Admin.Models.ViewModels;
using PizzaShop.Tests.Classes;
using System.Web.Routing;
using System.Linq;

namespace PizzaShop.Tests.AdminControllers
{
    [TestFixture]
    public class ComponentControllerTest
    {
        [Test]
        public void ComponentListPartial()
        {
            // Arrange
            var components = new List<Component>()
            {
                new Component() { Name="Ser" },
                new Component() { Name="Salami pepperoni" },
                new Component() { Name="Kurczak" },
                new Component() { Name="Szynka" },
                new Component() { Name="Pieczarki" },
                new Component() { Name="Ananas" },
                new Component() { Name="Papryka" },
                new Component() { Name="Czosnek" },
                new Component() { Name="Sałata lodowa" },
                new Component() { Name="Pomidory" },
                new Component() { Name="Ogórki" },
                new Component() { Name="Czarne oliwki" },
                new Component() { Name="Sos" },
                new Component() { Name="Kukurydza" },
                new Component() { Name="Czerwona cebula" }
            };
            var componentRepository = Substitute.For<IComponentRepository>();
            var mapper = Substitute.For<IMapper>();
            ComponentController controller = new ComponentController(componentRepository, mapper);

            // Act
            componentRepository.GetAll().Returns(components);
            var result = controller.ComponentListPartial() as PartialViewResult;
            var viewName = result.ViewName;
            var model = result.Model as List<Component>;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("_ComponentsPartial", viewName);
            Assert.IsNotNull(model);
            Assert.That(15, Is.EqualTo(model.Count));
        }

        [Test]
        public void Index()
        {
            // Arrange
            var components = new List<Component>()
            {
                new Component() { Name="Ser" },
                new Component() { Name="Salami pepperoni" },
                new Component() { Name="Kurczak" },
                new Component() { Name="Szynka" },
                new Component() { Name="Pieczarki" },
                new Component() { Name="Ananas" },
                new Component() { Name="Papryka" },
                new Component() { Name="Czosnek" }
            };
            var repository = Substitute.For<IComponentRepository>();
            var mapper = Substitute.For<IMapper>();
            var controller = new ComponentController(repository, mapper);

            controller.TempData["ModelIsNotValid"] = "Fake content.";
            controller.ViewBag.ModelIsNotValid = controller.TempData["ModelIsNotValid"];
            repository.GetAll().Returns(components);

            // Act
            var result = controller.Index() as ViewResult;
            var viewName = result.ViewName;
            var model = result.Model as List<Component>;
            var viewBag = controller.ViewBag.ModelIsNotValid;

            // Assert
            Assert.That("Fake content.", Is.EqualTo(viewBag));
            Assert.That("Index", Is.EqualTo(viewName));
            Assert.IsNotNull(model);
            Assert.That(8, Is.EqualTo(model.Count));
        }

        [Test]
        public void CreatePartial()
        {
            // Arrange
            var repository = Substitute.For<IComponentRepository>();
            var mapper = Substitute.For<IMapper>();
            var controller = new ComponentController(repository, mapper);

            // Act
            var result = controller.CreatePartial() as PartialViewResult;
            var viewName = result.ViewName;

            //Assert
            Assert.That(result, !Is.Null);
            Assert.That("_CreatePartial", Is.EqualTo(viewName));
        }

        [Test]
        public void Post_Create_Model_Is_Valid()
        {
            // Arrange
            var component = new Component()
            {
                Name="Sałata"
            };
            var componentViewModel = new ComponentViewModel()
            {
                Name = "Sałata"
            };
            var repository = Substitute.For<IComponentRepository>();
            var mapper = Substitute.For<IMapper>();
            var controller = new ComponentController(repository, mapper);
            var validator = new ModelValidator<ComponentViewModel>(componentViewModel);

            mapper.Map<ComponentViewModel, Component>(componentViewModel).Returns(component);
            repository.Insert(component);
            repository.Save();

            // Act
            var valid = validator.IsValid();
            var result = controller.Create(componentViewModel) as RedirectToRouteResult;
            var actionName = result.RouteValues.Values.ElementAt(0); //only action name

            // Assert
            Assert.That(result, !Is.Null);
            Assert.That("Index", Is.EqualTo(actionName));
            Assert.That(valid, Is.True);
        }

        [Test]
        public void Post_Create_Model_Is_Not_Valid()
        {
            // Arrange
            var componentViewModel = new ComponentViewModel()
            {
                Name = "Sałata 11"
            };
            var repository = Substitute.For<IComponentRepository>();
            var mapper = Substitute.For<IMapper>();
            var controller = new ComponentController(repository, mapper);
            var validator = new ModelValidator<ComponentViewModel>(componentViewModel);

            // Act
            var valid = validator.IsValid();
            validator.AddToModelError(controller);
            var result = controller.Create(componentViewModel) as RedirectToRouteResult;
            var actionName = result.RouteValues.Values.ElementAt(0);
            var tempData = controller.TempData["ModelIsNotValid"];

            // Assert
            Assert.That(result, !Is.Null);
            Assert.That("Wystąpił błąd w formularzu, spróbuj ponownie.", Is.EqualTo(tempData));
            Assert.That("Index", Is.EqualTo(actionName));
            Assert.That(valid, Is.False);
        }

        [Test]
        public void Good_Delete()
        {
            // Arrange 
            var component = new Component()
            {
                Name = "Sałata"
            };
            var id = 1;
            var repository = Substitute.For<IComponentRepository>();
            var mapper = Substitute.For<IMapper>();

            var fakeController = new FakeController();
            fakeController.PrepareFakeAjaxRequest();
            var controller = new ComponentController(repository, mapper);
            controller.ControllerContext = fakeController.GetControllerContext<ComponentController>(new RouteData(), controller);

            repository.Get(id).Returns(component);
            repository.Delete(component);
            repository.Save();

            // Act
            var result = controller.Delete(id) as JsonResult;
            var ajaxRequest = controller.Request.IsAjaxRequest();
            var jsonRequestBehavior = result.JsonRequestBehavior;
            var data = result.Data;

            // Assert
            Assert.That(result, !Is.Null);
            Assert.That(ajaxRequest, Is.True);
            Assert.That(jsonRequestBehavior, Is.EqualTo(JsonRequestBehavior.AllowGet));
        }

        [Test]
        public void Delete_Id_Is_Null()
        {
            // Arrange
            int? id = null;
            var repository = Substitute.For<IComponentRepository>();
            var mapper = Substitute.For<IMapper>();
            var controller = new ComponentController(repository, mapper);

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
            var component = new Component()
            {
                Name = "Sałata"
            };
            var id = 1;
            var repository = Substitute.For<IComponentRepository>();
            var mapper = Substitute.For<IMapper>();

            var fakeController = new FakeController();
            fakeController.PrepareFakeRequest();
            var controller = new ComponentController(repository, mapper);
            controller.ControllerContext = fakeController.GetControllerContext<ComponentController>(new RouteData(), controller);

            repository.Get(id).Returns(component);

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
        public void Delete_Component_Is_Null()
        {
            // Arrange 
            Component component = null;
            var id = 1;
            var repository = Substitute.For<IComponentRepository>();
            var mapper = Substitute.For<IMapper>();

            var controller = new ComponentController(repository, mapper);
            repository.Get(id).Returns(component);

            // Act
            var result = controller.Delete(id) as HttpNotFoundResult;
            var statusCode = result.StatusCode;

            // Assert
            Assert.That(result, !Is.Null);
            Assert.That(404, Is.EqualTo(statusCode));
        }

        [Test]
        public void Get_Edit_Id_Is_Null()
        {
            // Arrange
            int? id = null;
            var repository = Substitute.For<IComponentRepository>();
            var mapper = Substitute.For<IMapper>();
            var controller = new ComponentController(repository, mapper);

            // Act
            var result = controller.Edit(id) as HttpStatusCodeResult;
            var statusCode = result.StatusCode;

            // Assert
            Assert.That(result, !Is.Null);
            Assert.That(400, Is.EqualTo(statusCode));
        }

        [Test]
        public void Good_Get_Edit()
        {
            // Arrange
            var component = new Component()
            {
                Name = "Sałata"
            };
            var componentViewModel = new ComponentViewModel()
            {
                Name = "Sałata"
            };
            var id = 1;
            var repository = Substitute.For<IComponentRepository>();
            var mapper = Substitute.For<IMapper>();

            var fakeController = new FakeController();
            fakeController.PrepareFakeAjaxRequest();
            var controller = new ComponentController(repository, mapper);
            controller.ControllerContext = fakeController.GetControllerContext<ComponentController>(new RouteData(), controller);

            repository.Get(id).Returns(component);
            mapper.Map<Component, ComponentViewModel>(component).Returns(componentViewModel);

            // Act
            var result = controller.Edit(id) as PartialViewResult;
            var viewName = result.ViewName;
            var model = result.Model;
            var ajaxRequest = controller.Request.IsAjaxRequest();

            // Assert
            Assert.That(result, !Is.Null);
            Assert.That("_EditPartial", Is.EqualTo(viewName));
            Assert.That(model, !Is.Null);
            Assert.That(ajaxRequest, Is.True);
        }

        [Test]
        public void Get_Edit_Not_Ajax_Request()
        {
            // Arrange
            var component = new Component()
            {
                Name = "Sałata"
            };
            var componentViewModel = new ComponentViewModel()
            {
                Name = "Sałata"
            };
            var id = 1;
            var repository = Substitute.For<IComponentRepository>();
            var mapper = Substitute.For<IMapper>();

            var fakeController = new FakeController();
            fakeController.PrepareFakeRequest();
            var controller = new ComponentController(repository, mapper);
            controller.ControllerContext = fakeController.GetControllerContext<ComponentController>(new RouteData(), controller);

            repository.Get(id).Returns(component);
            mapper.Map<Component, ComponentViewModel>(component).Returns(componentViewModel);

            // Act
            var result = controller.Edit(id) as RedirectToRouteResult;
            var ajaxRequest = controller.Request.IsAjaxRequest();
            var actionName = result.RouteValues.Values.ElementAt(0);

            // Assert
            Assert.That(result, !Is.Null);
            Assert.That(ajaxRequest, Is.False);
            Assert.That("Index", Is.EqualTo(actionName));
        }

        [Test]
        public void Get_Edit_Component_Is_Null()
        {
            // Arrange
            Drink drink = null;
            var id = 0;
            var repository = Substitute.For<IDrinkRepository>();
            var mapper = Substitute.For<IMapper>();
            var controller = new DrinkController(repository, mapper);
            repository.Get(id).Returns(drink);

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
            var component = new Component()
            {
                Name = "Sałata"
            };
            var componentViewModel = new ComponentViewModel()
            {
                Name = "Sałata"
            };
            var repository = Substitute.For<IComponentRepository>();
            var mapper = Substitute.For<IMapper>();
            var controller = new ComponentController(repository, mapper);
            var validator = new ModelValidator<ComponentViewModel>(componentViewModel);

            repository.Get(componentViewModel.ID).Returns(component);
            mapper.Map<ComponentViewModel, Component>(componentViewModel).Returns(component);
            repository.Update(component);
            repository.Save();

            // Act
            var result = controller.Edit(componentViewModel) as RedirectToRouteResult;
            var actionName = result.RouteValues.Values.ElementAt(0);
            var valid = validator.IsValid();

            // Assert
            Assert.That(result, !Is.Null);
            Assert.That("Index", Is.EqualTo(actionName));
            Assert.That(valid, Is.True);
        }

        [Test]
        public void Post_Edit_Component_Is_Null()
        {
            // Arrange
            Component component = null;
            var componentViewModel = new ComponentViewModel()
            {
                Name = "Sałata"
            };
            var id = componentViewModel.ID;
            var repository = Substitute.For<IComponentRepository>();
            var mapper = Substitute.For<IMapper>();
            var validator = new ModelValidator<ComponentViewModel>(componentViewModel);
            var controller = new ComponentController(repository, mapper);

            repository.Get(id).Returns(component);

            // Act
            var result = controller.Edit(componentViewModel) as HttpNotFoundResult;
            var statusCode = result.StatusCode;
            var valid = validator.IsValid();

            // Assert
            Assert.That(result, !Is.Null);
            Assert.That(404, Is.EqualTo(statusCode));
            Assert.That(valid, Is.True);
        }

        [Test]
        public void Post_Edit_Not_Valid()
        {
            // Arrange
            var componentViewModel = new ComponentViewModel()
            {
                Name = "Sałata 112"
            };
            var repository = Substitute.For<IComponentRepository>();
            var mapper = Substitute.For<IMapper>();
            var controller = new ComponentController(repository, mapper);
            var validator = new ModelValidator<ComponentViewModel>(componentViewModel);

            // Act
            var valid = validator.IsValid();
            validator.AddToModelError(controller);
            var result = controller.Edit(componentViewModel) as ViewResult;
            var viewName = result.ViewName;
            var tempData = controller.TempData["ModelIsNotValid"];

            // Assert
            Assert.That(result, !Is.Null);
            Assert.That(valid, Is.False);
            Assert.That("Index", Is.EqualTo(viewName));
            Assert.That("Wystąpił błąd w formularzu, spróbuj ponownie.", Is.EqualTo(tempData));
        }
    }
}
