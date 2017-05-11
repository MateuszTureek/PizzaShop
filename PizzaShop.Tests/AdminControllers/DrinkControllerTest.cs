using System;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;
using NSubstitute;
using PizzaShop.Repositories.Shop.Interfaces;
using AutoMapper;
using PizzaShop.Areas.Admin.Controllers;
using PizzaShop.Models.PizzaShopModels.Entities;
using System.Web.Mvc;
using PizzaShop.Areas.Admin.Models.ViewModels;
using System.Linq;
using PizzaShop.Tests.Classes;
using System.Web.Routing;

namespace PizzaShop.Tests.AdminControllers
{
    [TestFixture]
    public class DrinkControllerTest
    {
        [Test]
        public void Index()
        {
            // Arrange
            var drinks = new List<Drink>()
            {
                new Drink() { Name="Mirinda",Price=5.00M,Capacity=0.5f },
                new Drink() { Name="7Up",Price=5.00M,Capacity=0.5f },
                new Drink() { Name="Woda gazowana",Price=3.00M,Capacity=0.5f },
                new Drink() { Name="Woda niegazowana",Price=3.00M,Capacity=0.5f },
                new Drink() { Name="Sok pomarańczowy",Price=4.00M, Capacity=0.33f }
            };
            var repository = Substitute.For<IDrinkRepository>();
            var mapper = Substitute.For<IMapper>();
            var controller = new DrinkController(repository, mapper);

            controller.TempData["ModelIsNotValid"] = "Fake content.";
            controller.ViewBag.ModelIsNotValid = controller.TempData["ModelIsNotValid"];
            repository.GetAll().Returns(drinks);

            // Act
            var result = controller.Index() as ViewResult;
            var viewName = result.ViewName;
            var model = result.Model as List<Drink>;
            var viewBag = controller.ViewBag.ModelIsNotValid;

            // Assert
            Assert.That("Fake content.", Is.EqualTo(viewBag));
            Assert.That("Index", Is.EqualTo(viewName));
            Assert.IsNotNull(model);
            Assert.That(5, Is.EqualTo(model.Count));
        }

        [Test]
        public void CreatePartial()
        {
            // Arrange
            var repository = Substitute.For<IDrinkRepository>();
            var mapper = Substitute.For<IMapper>();
            var controller = new DrinkController(repository, mapper);

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
            var drink = new Drink()
            {
                Name = "Mirinda",
                Price = 5.00M,
                Capacity = 0.5f
            };
            var drinkViewModel = new DrinkViewModel()
            {
                Name = "Mirinda",
                Price = 5.00M,
                Capacity = 0.5f
            };
            var repository = Substitute.For<IDrinkRepository>();
            var mapper = Substitute.For<IMapper>();
            var controller = new DrinkController(repository, mapper);
            var validator = new ModelValidator<DrinkViewModel>(drinkViewModel);

            mapper.Map<DrinkViewModel, Drink>(drinkViewModel).Returns(drink);
            repository.Insert(drink);
            repository.Save();

            // Act
            var valid = validator.IsValid();
            var result = controller.Create(drinkViewModel) as RedirectToRouteResult;
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
            var drinkViewModel = new DrinkViewModel()
            {
                //Name = "Mirinda", // required field
                Price = 5.00M,
                //Capacity = 0.5f // required field
            };
            var repository = Substitute.For<IDrinkRepository>();
            var mapper = Substitute.For<IMapper>();
            var controller = new DrinkController(repository, mapper);
            var validator = new ModelValidator<DrinkViewModel>(drinkViewModel);

            // Act
            var valid = validator.IsValid();
            validator.AddToModelError(controller);
            var result = controller.Create(drinkViewModel) as RedirectToRouteResult;
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
            var drink = new Drink()
            {
                ID = 1,
                Name = "Mirinda",
                Price = 5.00M,
                Capacity = 0.5f
            };
            var id = 1;
            var repository = Substitute.For<IDrinkRepository>();
            var mapper = Substitute.For<IMapper>();

            var fakeController = new FakeController();
            fakeController.PrepareFakeAjaxRequest();
            var controller = new DrinkController(repository, mapper);
            controller.ControllerContext = fakeController.GetControllerContext<DrinkController>(new RouteData(), controller);

            repository.Get(id).Returns(drink);
            repository.Delete(drink);
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
            var repository = Substitute.For<IDrinkRepository>();
            var mapper = Substitute.For<IMapper>();
            var controller = new DrinkController(repository, mapper);

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
            var drink = new Drink()
            {
                ID = 1,
                Name = "Mirinda",
                Price = 5.00M,
                Capacity = 0.5f
            };
            var id = 1;
            var repository = Substitute.For<IDrinkRepository>();
            var mapper = Substitute.For<IMapper>();

            var fakeController = new FakeController();
            fakeController.PrepareFakeRequest();
            var controller = new DrinkController(repository, mapper);
            controller.ControllerContext = fakeController.GetControllerContext<DrinkController>(new RouteData(), controller);

            repository.Get(id).Returns(drink);

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
        public void Delete_Drink_Is_Null()
        {
            // Arrange 
            Drink drink = null;
            var id = 1;
            var repository = Substitute.For<IDrinkRepository>();
            var mapper = Substitute.For<IMapper>();

            var controller = new DrinkController(repository, mapper);
            repository.Get(id).Returns(drink);

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
            var repository = Substitute.For<IDrinkRepository>();
            var mapper = Substitute.For<IMapper>();
            var controller = new DrinkController(repository, mapper);

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
            var drink = new Drink()
            {
                ID = 1,
                Name = "Mirinda",
                Price = 5.00M,
                Capacity = 0.5f
            };
            var drinkViewModel = new DrinkViewModel()
            {
                Name = "Mirinda",
                Price = 5.00M,
                Capacity = 0.5f
            };
            var id = 1;
            var repository = Substitute.For<IDrinkRepository>();
            var mapper = Substitute.For<IMapper>();

            var fakeController = new FakeController();
            fakeController.PrepareFakeAjaxRequest();
            var controller = new DrinkController(repository, mapper);
            controller.ControllerContext = fakeController.GetControllerContext<DrinkController>(new RouteData(), controller);

            repository.Get(id).Returns(drink);
            mapper.Map<Drink, DrinkViewModel>(drink).Returns(drinkViewModel);

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
            var drink = new Drink()
            {
                ID = 1,
                Name = "Mirinda",
                Price = 5.00M,
                Capacity = 0.5f
            };
            var drinkViewModel = new DrinkViewModel()
            {
                Name = "Mirinda",
                Price = 5.00M,
                Capacity = 0.5f
            };
            var id = 1;
            var repository = Substitute.For<IDrinkRepository>();
            var mapper = Substitute.For<IMapper>();

            var fakeController = new FakeController();
            fakeController.PrepareFakeRequest();
            var controller = new DrinkController(repository, mapper);
            controller.ControllerContext = fakeController.GetControllerContext<DrinkController>(new RouteData(), controller);

            repository.Get(id).Returns(drink);
            mapper.Map<Drink, DrinkViewModel>(drink).Returns(drinkViewModel);

            // Act
            var result = controller.Edit(id) as RedirectToRouteResult;
            var ajaxRequest = controller.Request.IsAjaxRequest();
            var actionName= result.RouteValues.Values.ElementAt(0);

            // Assert
            Assert.That(result, !Is.Null);
            Assert.That(ajaxRequest, Is.False);
            Assert.That("Index", Is.EqualTo(actionName));
        }

        [Test]
        public void Get_Edit_Drink_Is_Null()
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
            var drink = new Drink()
            {
                ID = 1,
                Name = "Mirinda",
                Price = 5.00M,
                Capacity = 0.5f
            };
            var drinkViewModel = new DrinkViewModel()
            {
                ID = 1,
                Name = "Mirinda",
                Price = 5.00M,
                Capacity = 0.5f
            };
            var repository = Substitute.For<IDrinkRepository>();
            var mapper = Substitute.For<IMapper>();
            var controller = new DrinkController(repository, mapper);
            var validator = new ModelValidator<DrinkViewModel>(drinkViewModel);

            repository.Get(drinkViewModel.ID).Returns(drink);
            mapper.Map<DrinkViewModel, Drink>(drinkViewModel).Returns(drink);
            repository.Update(drink);
            repository.Save();

            // Act
            var result = controller.Edit(drinkViewModel) as RedirectToRouteResult;
            var actionName = result.RouteValues.Values.ElementAt(0);
            var valid = validator.IsValid();

            // Assert
            Assert.That(result, !Is.Null);
            Assert.That("Index", Is.EqualTo(actionName));
            Assert.That(valid, Is.True);
        }

        [Test]
        public void Post_Edit_Drink_Is_Null()
        {
            // Arrange
            Drink drink = null;
            var drinkViewModel = new DrinkViewModel()
            {
                ID = -1,
                Name = "Mirinda",
                Price = 5.00M,
                Capacity = 0.5f
            };
            var id = drinkViewModel.ID;
            var repository = Substitute.For<IDrinkRepository>();
            var mapper = Substitute.For<IMapper>();
            var validator = new ModelValidator<DrinkViewModel>(drinkViewModel);
            var controller = new DrinkController(repository, mapper);

            repository.Get(id).Returns(drink);

            // Act
            var result = controller.Edit(drinkViewModel) as HttpNotFoundResult;
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
            var drinkViewModel = new DrinkViewModel()
            {
                ID = 1,
                //Name = "Mirinda",
                Price = 5.00M,
                //Capacity = 0.5f
            };
            var repository = Substitute.For<IDrinkRepository>();
            var mapper = Substitute.For<IMapper>();
            var controller = new DrinkController(repository, mapper);
            var validator = new ModelValidator<DrinkViewModel>(drinkViewModel);

            // Act
            var valid = validator.IsValid();
            validator.AddToModelError(controller);
            var result = controller.Edit(drinkViewModel) as ViewResult;
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
