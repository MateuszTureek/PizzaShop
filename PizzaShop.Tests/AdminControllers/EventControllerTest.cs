using AutoMapper;
using NSubstitute;
using NUnit.Framework;
using PizzaShop.Areas.Admin.Controllers;
using PizzaShop.Areas.Admin.Models.ViewModels;
using PizzaShop.Models.PizzaShopModels.CMS;
using PizzaShop.Repositories.CMS.Interfaces;
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
    public class EventControllerTest
    {
        [Test]
        public void Index()
        {
            // Arrange
            var events = new List<Event>()
            {
                new Event() { AddedDate=DateTime.Now,Title="Wydarzenie 1", Content="Treść wydarzenia 1" },
                new Event() { AddedDate=DateTime.Now,Title="Wydarzenie 2", Content="Treść wydarzenia 2" },
                new Event() { AddedDate=DateTime.Now,Title="Wydarzenie 3", Content="Treść wydarzenia 3" }
            };
            var repository = Substitute.For<IEventRepository>();
            var mappper = Substitute.For<IMapper>();
            var controller = new EventController(repository, mappper);

            controller.TempData["ModelIsNotValid"] = "Fake content.";
            controller.ViewBag.ModelIsNotValid = controller.TempData["ModelIsNotValid"];
            repository.GetAll().Returns(events);

            // Act
            var result = controller.Index() as ViewResult;
            var viewName = result.ViewName;
            var model = result.Model as List<Event>;
            var viewBag = controller.TempData["ModelIsNotValid"];

            // Assert
            Assert.That(result, !Is.Null);
            Assert.That("Fake content.", Is.EqualTo(viewBag));
            Assert.That(viewName, Is.EqualTo(viewName));
            Assert.That(model, !Is.Null);
            Assert.That(3, Is.EqualTo(model.Count));
        }

        [Test]
        public void CreatePartial()
        {
            // Arrange
            var repository = Substitute.For<IEventRepository>();
            var mapper = Substitute.For<IMapper>();
            var controller = new EventController(repository, mapper);

            // Act
            var result = controller.CreatePartial() as PartialViewResult;
            var viewName = result.ViewName;

            //Assert
            Assert.That(result, !Is.Null);
            Assert.That("_CreatePartial", Is.EqualTo(viewName));
        }

        [Test]
        public void Good_Post_Create()
        {
            // Arrange
            var eventObj = new Event()
            {
                Title = "Wydarzenie",
                Content = "Treść wydarzenia"
            };
            var eventViewModel = new EventViewModel()
            {
                Title = "Wydarzenie",
                Content = "Treść wydarzenia"
            };
            var repository = Substitute.For<IEventRepository>();
            var mapper = Substitute.For<IMapper>();
            var validator = new ModelValidator<EventViewModel>(eventViewModel);
            var controller = new EventController(repository, mapper);

            mapper.Map<EventViewModel, Event>(eventViewModel).Returns(eventObj);
            eventObj.AddedDate = DateTime.Now;
            repository.Insert(eventObj);
            repository.Save();

            // Act
            var result = controller.Create(eventViewModel) as RedirectToRouteResult;
            var actionName = result.RouteValues.Values.ElementAt(0);
            var valid = validator.IsValid();

            // Assert
            Assert.That(result, !Is.Null);
            Assert.That("Index", Is.EqualTo(actionName));
            Assert.That(valid, Is.True);
        }

        [Test]
        public void Post_Create_Is_Not_Valid()
        {
            // Arrange
            var eventViewModel = new EventViewModel()
            {
                //Title = "Wydarzenie 1", //required field
                Content = "Treść wydarzenia 1"
            };
            var repository = Substitute.For<IEventRepository>();
            var mapper = Substitute.For<IMapper>();
            var controller = new EventController(repository, mapper);
            var validator = new ModelValidator<EventViewModel>(eventViewModel);

            // Act
            var valid = validator.IsValid();
            validator.AddToModelError(controller);
            var result = controller.Create(eventViewModel) as RedirectToRouteResult;
            var actionName = result.RouteValues.Values.ElementAt(0);
            var tempData = controller.TempData["ModelIsNotValid"];

            // Assert
            Assert.That(result, !Is.Null);
            Assert.That("Index", Is.EqualTo(actionName));
            Assert.That("Wystąpił błąd w formularzu, spróbuj ponownie.", Is.EqualTo(tempData));
            Assert.That(valid, Is.False);
        }

        [Test]
        public void Good_Delete()
        {
            // Arrange
            var eventObj = new Event()
            {
                ID = 1,
                Title = "Wydarzenie 1",
                Content = "Treść wydarzenia 1",
                AddedDate = DateTime.Now
            };
            var id = 1;
            var repository = Substitute.For<IEventRepository>();
            var mapper = Substitute.For<IMapper>();

            var fakeController = new FakeController();
            fakeController.PrepareFakeAjaxRequest();
            var controller = new EventController(repository, mapper);
            controller.ControllerContext = fakeController.GetControllerContext<EventController>(new RouteData(), controller);

            repository.Get(id).Returns(eventObj);
            repository.Delete(eventObj);
            repository.Save();

            // Act
            var result = controller.Delete(id) as JsonResult;
            var jsonRequestBehavior = result.JsonRequestBehavior;
            var data = result.Data;

            // Assert
            Assert.That(result, !Is.Null);
            Assert.That(JsonRequestBehavior.AllowGet, Is.EqualTo(jsonRequestBehavior));
            Assert.That("", Is.EqualTo(data));
        }

        [Test]
        public void Delete_Id_Is_Null()
        {
            // Arrange
            int? id = null;
            var repository = Substitute.For<IEventRepository>();
            var mapper = Substitute.For<IMapper>();
            var controller = new EventController(repository, mapper);

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
            var eventObj = new Event()
            {
                ID = 1,
                Title = "Wydarzenie 1",
                Content = "Treść wydarzenia 1",
                AddedDate = DateTime.Now
            };
            var id = 1;
            var repository = Substitute.For<IEventRepository>();
            var mapper = Substitute.For<IMapper>();

            var fakeController = new FakeController();
            fakeController.PrepareFakeRequest();
            var controller = new EventController(repository, mapper);
            controller.ControllerContext = fakeController.GetControllerContext<EventController>(new RouteData(), controller);

            repository.Get(id).Returns(eventObj);
            
            // Act
            var result = controller.Delete(id) as RedirectToRouteResult;
            var actionName = result.RouteValues.Values.ElementAt(0);
            var ajaxRequest = controller.Request.IsAjaxRequest();

            // Assert
            Assert.That(result, !Is.Null);
            Assert.That("Index", Is.EqualTo(actionName));
            Assert.That(ajaxRequest, Is.False);
        }

        [Test]
        public void Delete_EventObj_Is_Null()
        {
            // Arrange
            Event eventObj = null;
            var id = 0;
            var repository = Substitute.For<IEventRepository>();
            var mapper = Substitute.For<IMapper>();
            var controller = new EventController(repository, mapper);

            repository.Get(id).Returns(eventObj);

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
            var eventObj = new Event()
            {
                ID = 1,
                Title = "Wydarzenie 1",
                Content = "Treść wydarzenia 1",
                AddedDate = new DateTime(2017, 2, 2)
            };
            var eventViewModel = new EventViewModel()
            {
                ID = 1,
                Title = "Wydarzenie 1",
                Content = "Treść wydarzenia 1",
                AddedDate = new DateTime(2017, 2, 2)
            };
            var id = 1;
            var repository = Substitute.For<IEventRepository>();
            var mapper = Substitute.For<IMapper>();

            var fakeController = new FakeController();
            fakeController.PrepareFakeAjaxRequest();
            var controller = new EventController(repository, mapper);
            controller.ControllerContext = fakeController.GetControllerContext<EventController>(new RouteData(), controller);

            repository.Get(id).Returns(eventObj);
            mapper.Map<Event, EventViewModel>(eventObj).Returns(eventViewModel);

            // Act
            var result = controller.Edit(id) as PartialViewResult;
            var viewName = result.ViewName;
            var model = result.Model;

            //Assert
            Assert.That(result, !Is.Null);
            Assert.That("_EditPartial", Is.EqualTo(viewName));
            Assert.That(model, !Is.Null);
        }

        [Test]
        public void Get_Edit_Not_Ajax_Request()
        {
            // Arrange
            var eventObj = new Event()
            {
                ID = 1,
                Title = "Wydarzenie 1",
                Content = "Treść wydarzenia 1",
                AddedDate = new DateTime(2017, 2, 2)
            };
            var eventViewModel = new EventViewModel()
            {
                ID = 1,
                Title = "Wydarzenie 1",
                Content = "Treść wydarzenia 1",
                AddedDate = new DateTime(2017, 2, 2)
            };
            var id = 1;
            var repository = Substitute.For<IEventRepository>();
            var mapper = Substitute.For<IMapper>();

            var fakeController = new FakeController();
            fakeController.PrepareFakeRequest();
            var controller = new EventController(repository, mapper);
            controller.ControllerContext = fakeController.GetControllerContext<EventController>(new RouteData(), controller);

            repository.Get(id).Returns(eventObj);
            mapper.Map<Event, EventViewModel>(eventObj).Returns(eventViewModel);

            // Act
            var result = controller.Edit(id) as RedirectToRouteResult;
            var actionName = result.RouteValues.Values.ElementAt(0);
            var ajaxRequest = controller.Request.IsAjaxRequest();

            //Assert
            Assert.That(result, !Is.Null);
            Assert.That("Index", Is.EqualTo(actionName));
            Assert.That(ajaxRequest, Is.False);
        }

        [Test]
        public void Get_Edit_Model_Is_Null()
        {
            // Arrange
            Event eventObj = null;
            var id = 0;
            var repository = Substitute.For<IEventRepository>();
            var mapper = Substitute.For<IMapper>();

            var controller = new EventController(repository, mapper);
            repository.Get(id).Returns(eventObj);

            // Act
            var result = controller.Edit(id) as HttpNotFoundResult;
            var statusCode = result.StatusCode;
            
            //Assert
            Assert.That(result, !Is.Null);
            Assert.That(404, Is.EqualTo(statusCode));
        }

        [Test]
        public void Good_Post_Edit()
        {
            // Arrange
            var eventObj = new Event()
            {
                ID = 1,
                Title = "Wydarzenie ",
                Content = "Treść wydarzenia"
            };
            var resultEventObj = new Event()
            {
                ID = 1,
                Title = "Wydarzenie Nowe",
                Content = "Treść.",
            };
            var eventViewModel = new EventViewModel()
            {
                ID = 1,
                Title = "Wydarzenie",
                Content = "Treść.",
            };
            var id = 1;
            var repository = Substitute.For<IEventRepository>();
            var mapper = Substitute.For<IMapper>();
            var controller = new EventController(repository, mapper);
            var validator = new ModelValidator<EventViewModel>(eventViewModel);

            repository.Get(id).Returns(eventObj);
            mapper.Map<EventViewModel, Event>(eventViewModel,eventObj).Returns(resultEventObj);
            eventObj.AddedDate = DateTime.Now;
            repository.Update(eventObj);
            repository.Save();

            // Act
            var valid = validator.IsValid();
            validator.AddToModelError(controller);
            var result = controller.Edit(eventViewModel) as RedirectToRouteResult;
            var actionName = result.RouteValues.Values.ElementAt(0);

            //Assert
            Assert.That(result, !Is.Null);
            Assert.That("Index", Is.EqualTo(actionName));
            Assert.That(valid, Is.True);
        }

        [Test]
        public void Post_Edit_Model_Not_Valid()
        {
            // Arrange
            var eventViewModel = new EventViewModel()
            {
                ID = 1,
                //Title = "Wydarzenie 1", // required field
                //Content = "Treść wydarzenia 1", // required field
            };
            var repository = Substitute.For<IEventRepository>();
            var mapper = Substitute.For<IMapper>();
            var controller = new EventController(repository, mapper);
            var validator = new ModelValidator<EventViewModel>(eventViewModel);

            // Act
            var valid = validator.IsValid();
            validator.AddToModelError(controller);
            var result = controller.Edit(eventViewModel) as RedirectToRouteResult;
            var actionName = result.RouteValues.Values.ElementAt(0);
            var tempData = controller.TempData["ModelIsNotValid"];

            //Assert
            Assert.That(result, !Is.Null);
            Assert.That("Wystąpił błąd w formularzu, spróbuj ponownie.", Is.EqualTo(tempData));
            Assert.That("Index", Is.EqualTo(actionName));
            Assert.That(valid, Is.False);
        }

        [Test]
        public void Post_Edit_Event_Is_Null()
        {
            // Arrange
            Event eventObj = null;
            var eventViewModel = new EventViewModel()
            {
                ID = -1,
                Title = "Wydarzenie",
                Content = "Treść wydarzenia",
            };
            var id = -1;
            var repository = Substitute.For<IEventRepository>();
            var mapper = Substitute.For<IMapper>();
            var controller = new EventController(repository, mapper);
            var validator = new ModelValidator<EventViewModel>(eventViewModel);

            repository.Get(id).Returns(eventObj);

            // Act
            var valid = validator.IsValid();
            validator.AddToModelError(controller);
            var result = controller.Edit(eventViewModel) as HttpNotFoundResult;
            var statusCode = result.StatusCode;

            //Assert
            Assert.That(result, !Is.Null);
            Assert.That(404, Is.EqualTo(statusCode));
            Assert.That(valid, Is.True);
        }
    }
}
