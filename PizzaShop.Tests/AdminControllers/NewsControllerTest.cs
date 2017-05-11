using NUnit.Framework;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PizzaShop.Models.PizzaShopModels.CMS;
using PizzaShop.Repositories.CMS.Interfaces;
using AutoMapper;
using PizzaShop.Areas.Admin.Controllers;
using System.Web.Mvc;
using PizzaShop.Areas.Admin.Models.ViewModels;
using PizzaShop.Tests.Classes;
using System.Web.Routing;

namespace PizzaShop.Tests.AdminControllers
{
    [TestFixture]
    public class NewsControllerTest
    {
        [Test]
        public void Index()
        {
            // Arrange
            var news = new List<News>()
            {
                new News() { AddedDate=DateTime.Now, Position=1, Title="New 1",Content="Quisque nulla nunc, tempor eu lorem non, pharetra laoreet massa." },
                new News() { AddedDate=DateTime.Now, Position=2, Title="New 2",Content="Nunc iaculis, elit eu aliquam placerat, diam est feugiat urna, et lacinia tellus lectus a sem. " },
                new News() { AddedDate=DateTime.Now, Position=3, Title="New 3",Content="In imperdiet tellus ex, sed efficitur odio dignissim eget." }
            };
            var service = Substitute.For<INewsRepository>();
            var mapper = Substitute.For<IMapper>();
            var controller = new NewsController(service, mapper);

            controller.TempData["ModelIsNotValid"] = "Fake content.";
            controller.ViewBag.ModelIsNotValid = controller.TempData["ModelIsNotValid"];
            service.GetAll().Returns(news);

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
            var service = Substitute.For<INewsRepository>();
            var mapper = Substitute.For<IMapper>();
            var controller = new NewsController(service, mapper);

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
            var newsViewModel = new NewsViewModel()
            {
                AddedDate = DateTime.Now,
                Position = 1,
                Title = "New",
                Content = "Quisque nulla nunc, tempor eu lorem non, pharetra laoreet massa."
            };
            var news = new News()
            {
                AddedDate = DateTime.Now,
                Position = 1,
                Title = "New",
                Content = "Quisque nulla nunc, tempor eu lorem non, pharetra laoreet massa."
            };
            var validator = new ModelValidator<NewsViewModel>(newsViewModel);
            var service = Substitute.For<INewsRepository>();
            var mapper = Substitute.For<IMapper>();
            var controller = new NewsController(service, mapper);

            mapper.Map<NewsViewModel, News>(newsViewModel).Returns(news);
            service.Insert(news);
            service.Save();

            // Act
            var valid = validator.IsValid();
            validator.AddToModelError(controller);
            var result = controller.Create(newsViewModel) as RedirectToRouteResult;
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
            var newsViewModel = new NewsViewModel()
            {
                AddedDate = DateTime.Now,
                //Position = 1,
                //Title = "New 1",
                Content = "Quisque nulla nunc, tempor eu lorem non, pharetra laoreet massa."
            };
            var validator = new ModelValidator<NewsViewModel>(newsViewModel);
            var service = Substitute.For<INewsRepository>();
            var mapper = Substitute.For<IMapper>();

            var controller = new NewsController(service, mapper);

            // Act
            var valid = validator.IsValid();
            validator.AddToModelError(controller);
            var result = controller.Create(newsViewModel) as RedirectToRouteResult;
            var tempData = controller.TempData["ModelIsNotValid"];

            // Assert
            Assert.That(valid, Is.False);
            Assert.That(result, !Is.Null);
            Assert.That("Wystąpił błąd w formularzu, spróbuj ponownie.", Is.EqualTo(tempData));
        }

        [Test]
        public void Good_Delete()
        {
            // Arrange
            var id = 1;
            var news = new News()
            {
                ID = 1,
                AddedDate = DateTime.Now,
                Position = 1,
                Title = "New 1",
                Content = "Quisque nulla nunc, tempor eu lorem non, pharetra laoreet massa."
            };
            var service = Substitute.For<INewsRepository>();
            var mapper = Substitute.For<IMapper>();
            var fakeController = new FakeController();
            var controller = new NewsController(service, mapper);

            fakeController.PrepareFakeAjaxRequest();
            controller.ControllerContext = fakeController.GetControllerContext<NewsController>(new RouteData(), controller);
            service.Get(id).Returns(news);
            service.Delete(news);
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
            var service = Substitute.For<INewsRepository>();
            var mapper = Substitute.For<IMapper>();
            var controller = new NewsController(service, mapper);

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
            var news = new News()
            {
                ID = 1,
                AddedDate = DateTime.Now,
                Position = 1,
                Title = "New 1",
                Content = "Quisque nulla nunc, tempor eu lorem non, pharetra laoreet massa."
            };
            var service = Substitute.For<INewsRepository>();
            var mapper = Substitute.For<IMapper>();
            var fakeController = new FakeController();
            var controller = new NewsController(service, mapper);

            fakeController.PrepareFakeRequest();
            controller.ControllerContext = fakeController.GetControllerContext<NewsController>(new RouteData(), controller);
            service.Get(id).Returns(news);

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
            News news = null;
            var service = Substitute.For<INewsRepository>();
            var mapper = Substitute.For<IMapper>();
            var controller = new NewsController(service, mapper);

            service.Get(id).Returns(news);

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
            var news = new News()
            {
                ID=1,
                AddedDate = DateTime.Now,
                Position = 1,
                Title = "New 1",
                Content = "Quisque nulla nunc, tempor eu lorem non, pharetra laoreet massa."
            };
            var newsViewModel = new NewsViewModel()
            {
                ID=1,
                AddedDate = DateTime.Now,
                Position = 1,
                Title = "New 1",
                Content = "Quisque nulla nunc, tempor eu lorem non, pharetra laoreet massa."
            };
            var service = Substitute.For<INewsRepository>();
            var mapper = Substitute.For<IMapper>();
            var fakeController = new FakeController();
            var controller = new NewsController(service, mapper);

            fakeController.PrepareFakeAjaxRequest();
            controller.ControllerContext = fakeController.GetControllerContext<NewsController>(new RouteData(), controller);
            service.Get(id).Returns(news);
            mapper.Map<News, NewsViewModel>(news).Returns(newsViewModel);

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
            var service = Substitute.For<INewsRepository>();
            var mapper = Substitute.For<IMapper>();
            var controller = new NewsController(service, mapper);

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
            var news = new News()
            {
                ID = 1,
                AddedDate = DateTime.Now,
                Position = 1,
                Title = "New 1",
                Content = "Quisque nulla nunc, tempor eu lorem non, pharetra laoreet massa."
            };
            var newsViewModel = new NewsViewModel()
            {
                ID = 1,
                AddedDate = DateTime.Now,
                Position = 1,
                Title = "New 1",
                Content = "Quisque nulla nunc, tempor eu lorem non, pharetra laoreet massa."
            };
            var service = Substitute.For<INewsRepository>();
            var mapper = Substitute.For<IMapper>();
            var fakeController = new FakeController();
            var controller = new NewsController(service, mapper);

            fakeController.PrepareFakeRequest();
            controller.ControllerContext = fakeController.GetControllerContext<NewsController>(new RouteData(), controller);
            service.Get(id).Returns(news);
            mapper.Map<News, NewsViewModel>(news).Returns(newsViewModel);

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
            News news = null;
            var service = Substitute.For<INewsRepository>();
            var mapper = Substitute.For<IMapper>();
            var controller = new NewsController(service, mapper);

            service.Get(id).Returns(news);

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
            var news = new News()
            {
                ID = 1,
                AddedDate = DateTime.Now,
                Position = 1,
                Title = "New",
                Content = "Quisque nulla nunc, tempor eu lorem non, pharetra laoreet massa."
            };
            var resultNews = new News()
            {
                ID = 1,
                AddedDate = DateTime.Now,
                Position = 1,
                Title = "New ",
                Content = "Quisque , tempor eu lorem non, pharetra laoreet massa."
            };
            var newsViewModel = new NewsViewModel()
            {
                ID = 1,
                AddedDate = DateTime.Now,
                Position = 1,
                Title = "New ",
                Content = "Quisque , tempor eu lorem non, pharetra laoreet massa."
            };
            var service = Substitute.For<INewsRepository>();
            var mapper = Substitute.For<IMapper>();
            var validator = new ModelValidator<NewsViewModel>(newsViewModel);
            var fakeController = new FakeController();
            var controller = new NewsController(service, mapper);

            fakeController.PrepareFakeAjaxRequest();
            controller.ControllerContext = fakeController.GetControllerContext<NewsController>(new RouteData(), controller);
            service.Get(news.ID).Returns(news);
            mapper.Map<NewsViewModel, News>(newsViewModel,news).Returns(resultNews);
            service.Update(news);
            service.Save();

            // Act
            var valid = validator.IsValid();
            validator.AddToModelError(controller);
            var result = controller.Edit(newsViewModel) as RedirectToRouteResult;
            var actionName = result.RouteValues.Values.ElementAt(0);
            var ajaxRequest = controller.Request.IsAjaxRequest();

            // Assert
            Assert.That(result, !Is.Null);
            Assert.That("Index", Is.EqualTo(actionName));
            Assert.That(ajaxRequest, Is.True);
            Assert.That(valid, Is.True);
        }

        [Test]
        public void Post_Edit_News_Is_Null()
        {
            // Arrange
            var id = -1;
            News news = null;
            var newsViewModel = new NewsViewModel()
            {
                ID = 1,
                AddedDate = DateTime.Now,
                Position = 1,
                Title = "New",
                Content = "Quisque nulla nunc, tempor eu lorem non, pharetra laoreet massa."
            };
            var service = Substitute.For<INewsRepository>();
            var mapper = Substitute.For<IMapper>();
            var validator = new ModelValidator<NewsViewModel>(newsViewModel);
            var controller = new NewsController(service, mapper);

            service.Get(id).Returns(news);

            // Act
            var valid = validator.IsValid();
            validator.AddToModelError(controller);
            var result = controller.Edit(newsViewModel) as HttpNotFoundResult;
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
            var newsViewModel = new NewsViewModel()
            {
                ID = 1,
                AddedDate = DateTime.Now,
                //Position = 1,
                Title = "New 1",
                //Content = "Quisque nulla nunc, tempor eu lorem non, pharetra laoreet massa."
            };
            var service = Substitute.For<INewsRepository>();
            var mapper = Substitute.For<IMapper>();
            var controller = new NewsController(service, mapper);
            var validator = new ModelValidator<NewsViewModel>(newsViewModel);

            // Act
            var valid = validator.IsValid();
            validator.AddToModelError(controller);
            var result = controller.Edit(newsViewModel) as RedirectToRouteResult;
            var actionName = result.RouteValues.Values.ElementAt(0);
            var tempData = controller.TempData["ModelIsNotValid"];

            // Assert
            Assert.That(result, !Is.Null);
            Assert.That(valid, Is.False);
            Assert.That("Index", Is.EqualTo(actionName));
        }
    }
}
