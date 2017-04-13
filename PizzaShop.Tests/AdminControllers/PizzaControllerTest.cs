using NUnit.Framework;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PizzaShop.Models.PizzaShopModels.Entities;
using PizzaShop.Services.shop.Interfaces;
using PizzaShop.Areas.Admin.Controllers;
using System.Web.Mvc;
using PizzaShop.Areas.Admin.Models.ViewModels;
using PizzaShop.Tests.Classes;
using System.Web.Routing;

namespace PizzaShop.Tests.AdminControllers
{
    [TestFixture]
    public class PizzaControllerTest
    {
        List<Component> components=null;

        [SetUp]
        public void Set()
        {
            components = new List<Component>()
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
        }

        [Test]
        public void Index()
        {
            // Arrange
            var pizzas = new List<Pizza>()
            {
                new Pizza { Name="Margarita",Components=new List<Component>() { components[0] }  },
                new Pizza { Name="Salame",Components=new List<Component>() { components[0], components[1] }  },
                new Pizza { Name="Pollo",Components=new List<Component>() { components[0],components[2] }  }
            };
            var service = Substitute.For<IPizzaService>();
            var controller = new PizzaController(service);

            service.GetAllPizzas().Returns(pizzas);

            // Act
            var result = controller.Index() as ViewResult;
            var viewName = result.ViewName;
            var model = result.Model;

            //Assert
            Assert.That(result, !Is.Null);
            Assert.That("Index", Is.EqualTo(viewName));
            Assert.That(model, !Is.Null);
        }

        [Test]
        public void CreatePartial()
        {
            // Arrange
            var service = Substitute.For<IPizzaService>();
            var controller = new PizzaController(service);

            service.GetAllComponents().Returns(components);

            // Act
            var result = controller.CreatePartial() as PartialViewResult;
            var viewName = result.ViewName;
            var viewBagComponents = result.ViewBag.Components;

            // Assert
            Assert.That(result, !Is.Null);
            Assert.That(viewBagComponents, !Is.Null);
            Assert.That("_CreatePartial", Is.EqualTo(viewName));
        }

        [Test]
        public void Post_Create_Good()
        {
            // Arrange
            var pizzaViewModel = new PizzaViewModel()
            {
                Name = "Margarita",
                PriceForLarge = 30.00M,
                PriceForMedium = 20.00M,
                PriceForSmall = 15.00M,
                Components = new List<int>() { 1 }
            };
            var validator = new ModelValidator<PizzaViewModel>(pizzaViewModel);
            var service = Substitute.For<IPizzaService>();
            var controller = new PizzaController(service);

            service.CreatePizza(pizzaViewModel);
            service.SavePizza();

            // Act
            var valid = validator.IsValid();
            validator.AddToModelError(controller);
            var result = controller.Create(pizzaViewModel) as RedirectToRouteResult;
            var actionName = result.RouteValues.Values.ElementAt(0);
            var controllerName = result.RouteValues.Values.ElementAt(1);

            // Assert
            Assert.That(result, !Is.Null);
            Assert.That("Index", Is.EqualTo(actionName));
            Assert.That("Pizza", Is.EqualTo(controllerName));
            Assert.That(valid, Is.True);

        }

        [Test]
        public void Post_Create_Model_Not_Valid()
        {
            // Arrange
            var pizzaViewModel = new PizzaViewModel()
            {
                Name = "Margarita",
                PriceForLarge = 30.0000M, // max. 3 chars after ','
                PriceForMedium = 20.00M,
                //PriceForSmall = 15.00M,
                Components = new List<int>() { 1 }
            };
            var service = Substitute.For<IPizzaService>();
            var controller = new PizzaController(service);
            var validator = new ModelValidator<PizzaViewModel>(pizzaViewModel);

            // Act
            var valid = validator.IsValid();
            validator.AddToModelError(controller);
            var result = controller.Create(pizzaViewModel) as HttpStatusCodeResult;
            var statusCode = result.StatusCode;

            // Assert
            Assert.That(valid, Is.False);
            Assert.That(result, !Is.Null);
            Assert.That(304, Is.EqualTo(statusCode));
        }

        [Test]
        public void Delete_Good()
        {
            // Arrange
            var id = 1;
            var pizza = new Pizza()
            {
                ID = id,
                Name = "Margarita"
            };
            var service = Substitute.For<IPizzaService>();
            var fakeController = new FakeController();
            var controller = new PizzaController(service);

            fakeController.PrepareFakeAjaxRequest();
            controller.ControllerContext = fakeController.GetControllerContext<PizzaController>(new RouteData(), controller);
            service.GetPizza(id).Returns(pizza);
            service.DeletePizza(pizza);
            service.SavePizza();

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
        public void Delete_Not_Ajax_Request()
        {
            // Arrange
            var id = 1;
            var pizza = new Pizza()
            {
                ID = id,
                Name = "Margarita"
            };
            var service = Substitute.For<IPizzaService>();
            var fakeController = new FakeController();
            var controller = new PizzaController(service);

            fakeController.PrepareFakeRequest();
            controller.ControllerContext = fakeController.GetControllerContext<PizzaController>(new RouteData(), controller);
            service.GetPizza(id).Returns(pizza);

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
            Pizza pizza = null;
            var service = Substitute.For<IPizzaService>();
            var controller = new PizzaController(service);

            service.GetPizza(id).Returns(pizza);

            // Act
            var result = controller.Delete(id) as HttpNotFoundResult;
            var statusCode = result.StatusCode;

            // Assert
            Assert.That(result, !Is.Null);
            Assert.That(404, Is.EqualTo(statusCode));
        }

        [Test]
        public void Get_Edit_Good()
        {
            // Arrange
            var id = 1;
            var pizza = new Pizza()
            {
                ID = id,
                Name = "Margarita"
            };
            var pizzaViewModel = new PizzaViewModel()
            {
                ID = id,
                Name = "Margarita",
                PriceForLarge = 30.00M,
                PriceForMedium = 20.00M,
                PriceForSmall = 15.00M,
                Components = new List<int>() { 1 }
            };
            var components_1 = new List<Component>()
            {
                new Component() { Name="Ser" },
                new Component() { Name="Salami pepperoni" },
                new Component() { Name="Kurczak" }
            };
            var components_2 = new List<Component>()
            {
                new Component() { Name="Pieczarki" }
            };
            var pizzaSizePrices = new List<PizzaSizePrice>()
            {
                new PizzaSizePrice() { PizzaID=1,PizzaSizeID=1,Price=10.00M },
                new PizzaSizePrice() { PizzaID=1,PizzaSizeID=2,Price=12.00M },
                new PizzaSizePrice() { PizzaID=1,PizzaSizeID=1,Price=15.00M }
            };
            var service = Substitute.For<IPizzaService>();
            var fakeController = new FakeController();
            var controller = new PizzaController(service);

            fakeController.PrepareFakeAjaxRequest();
            controller.ControllerContext = fakeController.GetControllerContext<PizzaController>(new RouteData(), controller);

            service.GetPizza(id).Returns(pizza);
            service.FindNotPizzaComponent(id).Returns(components_1);
            service.FindComponents(id).Returns(components_2);
            service.GetPizzaSizePrices(id).Returns(pizzaSizePrices);

            // Act
            var result = controller.Edit(id) as PartialViewResult;
            var viewName = result.ViewName;
            var model = result.Model;
            var ajaxRequest = controller.Request.IsAjaxRequest();
            var viewBagComponets = result.ViewBag.Components;
            var viewBagCurrentComponents = result.ViewBag.CurrentComponents;

            // Assert
            Assert.That(result, !Is.Null);
            Assert.That("_EditPartial", Is.EqualTo(viewName));
            Assert.That(ajaxRequest, Is.True);
            Assert.That(model, !Is.Null);
            Assert.That(viewBagComponets, !Is.Null);
            Assert.That(viewBagCurrentComponents, !Is.Null);
        }

        [Test]
        public void Get_Edit_Not_Ajax_Request()
        {
            // Arrange
            var id = 1;
            var pizza = new Pizza()
            {
                ID = id,
                Name = "Margarita"
            };
            var pizzaViewModel = new PizzaViewModel()
            {
                ID = id,
                Name = "Margarita",
                PriceForLarge = 30.00M,
                PriceForMedium = 20.00M,
                PriceForSmall = 15.00M,
                Components = new List<int>() { 1 }
            };
            var service = Substitute.For<IPizzaService>();
            var fakeController = new FakeController();
            var controller = new PizzaController(service);

            fakeController.PrepareFakeRequest();
            controller.ControllerContext = fakeController.GetControllerContext<PizzaController>(new RouteData(), controller);
            service.GetPizza(id).Returns(pizza);
            
            // Act
            var result = controller.Edit(id) as HttpNotFoundResult;
            var statusCode = result.StatusCode;
            var ajaxRequest = controller.Request.IsAjaxRequest();

            // Assert
            Assert.That(result, !Is.Null);
            Assert.That(404, Is.EqualTo(statusCode));
            Assert.That(ajaxRequest, Is.False);
        }

        [Test]
        public void Get_Edit_Model_Is_Null()
        {
            // Arrange
            var id = 1;
            Pizza pizza = null;
            var service = Substitute.For<IPizzaService>();
            var controller = new PizzaController(service);

            service.GetPizza(id).Returns(pizza);

            // Act
            var result = controller.Edit(id) as HttpNotFoundResult;
            var statusCode = result.StatusCode;
            var statusDescription = result.StatusDescription;

            // Assert
            Assert.That(result, !Is.Null);
            Assert.That(404, Is.EqualTo(statusCode));
            Assert.That("Nie znaleziono szukanego elementu.", Is.EqualTo(statusDescription));
        }

        [Test]
        public void Post_Edit_Good()
        {
            // Arrange
            var id = 1;
            var pizza = new Pizza()
            {
                ID = id,
                Name = "Margarita"
            };
            var pizzaViewModel = new PizzaViewModel()
            {
                ID = id,
                Name = "Margarita",
                PriceForLarge = 30.00M,
                PriceForMedium = 20.00M,
                PriceForSmall = 15.00M,
                Components = new List<int>() { 1 }
            };
            var service = Substitute.For<IPizzaService>();
            var validator = new ModelValidator<PizzaViewModel>(pizzaViewModel);
            var fakeController = new FakeController();
            var controller = new PizzaController(service);

            fakeController.PrepareFakeAjaxRequest();
            controller.ControllerContext = fakeController.GetControllerContext<PizzaController>(new RouteData(), controller);

            service.UpdatePizza(pizzaViewModel);
            service.SavePizza();

            // Act
            var valid = validator.IsValid();
            validator.AddToModelError(controller);
            var result = controller.Edit(pizzaViewModel) as RedirectToRouteResult;
            var actionName = result.RouteValues.Values.ElementAt(0);
            var ajaxRequest = controller.Request.IsAjaxRequest();

            // Assert
            Assert.That(result, !Is.Null);
            Assert.That("Index", Is.EqualTo(actionName));
            Assert.That(ajaxRequest, Is.True);
            Assert.That(valid, Is.True);
        }

        [Test]
        public void Post_Edit_Is_Not_Valid()
        {
            // Arrange
            var pizzaViewModel = new PizzaViewModel()
            {
                ID = 1,
                Name = "Margarita",
                PriceForLarge = 30.00M,
                //PriceForMedium = 20.00M,
                PriceForSmall = 15.00M,
                Components = new List<int>() { 1 }
            };
            var service = Substitute.For<IPizzaService>();
            var controller = new PizzaController(service);
            var validator = new ModelValidator<PizzaViewModel>(pizzaViewModel);

            service.UpdatePizza(pizzaViewModel);
            service.SavePizza();

            // Act
            var valid = validator.IsValid();
            validator.AddToModelError(controller);
            var result = controller.Edit(pizzaViewModel) as HttpStatusCodeResult;
            var statusCode = result.StatusCode;

            // Assert
            Assert.That(result, !Is.Null);
            Assert.That(valid, Is.False);
            Assert.That(304, Is.EqualTo(statusCode));
        }
    }
}
