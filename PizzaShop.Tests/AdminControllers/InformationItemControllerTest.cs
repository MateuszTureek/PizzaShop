using AutoMapper;
using NSubstitute;
using NUnit.Framework;
using PizzaShop.Areas.Admin.Controllers;
using PizzaShop.Areas.Admin.Models.ViewModels;
using PizzaShop.Models.PizzaShopModels.CMS;
using PizzaShop.Services.Cms.Interfaces;
using PizzaShop.Tests.Classes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PizzaShop.Tests.AdminControllers
{
    [TestFixture]
    public class InformationItemControllerTest
    {
        [Test]
        public void Index()
        {
            // Arrange
            var informationItems = new List<InformationItem>()
            {
                new InformationItem() { Position=1, PictureUrl="/Content/Images/pizza_1.jpg", Title="Menu",Content="Ut at viverra arcu. Donec sed interdum nulla, eu porttitor lorem. Morbi efficitur sapien ullamcorper lorem vestibulum, ut aliquet turpis consequat. Suspendisse ac nibh non velit scelerisque bibendum. " },
                new InformationItem() { Position=2, PictureUrl="/Content/Images/pizza_2.jpg", Title="Promocja", Content="Praesent in placerat risus, et ornare lectus. Praesent turpis tortor, consectetur quis enim id, consequat pharetra velit. Nullam tempor convallis ante at finibus. " },
                new InformationItem() { Position=3, PictureUrl="/Content/Images/pizza_3.jpeg", Title="O nas", Content="Suspendisse sed enim porttitor, auctor urna et, bibendum enim. In imperdiet tellus ex, sed efficitur odio dignissim eget. Ut fermentum ipsum eget lorem ornare condimentum. " }
            };
            var service = Substitute.For<IInformationItemService>();
            var mapper = Substitute.For<IMapper>();
            var controller = new InformationItemController(service, mapper);

            service.GetAllInformationItems().Returns(informationItems);

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
            var service = Substitute.For<IInformationItemService>();
            var mapper = Substitute.For<IMapper>();
            var controller = new InformationItemController(service, mapper);

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
        public void Post_Create_Model_Is_Valid_File_Good()
        {
            // Arrange
            var informationItemViewModel = new InformationItemViewModel()
            {
                Position = 1,
                Title = "Menu",
                Content = "Ut at viverra arcu. Donec sed interdum nulla, eu porttitor lorem. Morbi efficitur sapien ullamcorper lorem vestibulum, ut aliquet turpis consequat. Suspendisse ac nibh non velit scelerisque bibendum. "
            };
            var informationItem = new InformationItem()
            {
                Position = 1,
                Title = "Menu",
                Content = "Ut at viverra arcu. Donec sed interdum nulla, eu porttitor lorem. Morbi efficitur sapien ullamcorper lorem vestibulum, ut aliquet turpis consequat. Suspendisse ac nibh non velit scelerisque bibendum. "
            };
            var fileName = "text.jpg";
            var fileLength = 1000;
            var contentType = "image";
            var fakeImage = Substitute.For<HttpPostedFileBase>();
            var validator = new ModelValidator<InformationItemViewModel>(informationItemViewModel);
            var service = Substitute.For<IInformationItemService>();
            var mapper = Substitute.For<IMapper>();

            fakeImage.FileName.Returns(fileName);
            fakeImage.ContentType.Returns(contentType);
            fakeImage.ContentLength.Returns(fileLength);

            var controller = new InformationItemController(service, mapper);

            mapper.Map<InformationItemViewModel, InformationItem>(informationItemViewModel).Returns(informationItem);
            informationItem.PictureUrl = service.AddInformationItemImage(fakeImage);
            service.CreateInformationItem(informationItem);
            service.SaveInfomrationItem();

            // Act
            var valid = validator.IsValid();
            validator.AddToModelError(controller);
            var result = controller.Create(informationItemViewModel, fakeImage) as RedirectToRouteResult;
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
            var informationItemViewModel = new InformationItemViewModel()
            {
                //Position = 1,
                //Title = "Menu",
                Content = "Ut at viverra arcu. Donec sed interdum nulla, eu porttitor lorem. Morbi efficitur sapien ullamcorper lorem vestibulum, ut aliquet turpis consequat. Suspendisse ac nibh non velit scelerisque bibendum. "
            };
            var fileName = "text.jpg";
            var fileLength = 1000;
            var contentType = "image";
            var fakeImage = Substitute.For<HttpPostedFileBase>();
            var validator = new ModelValidator<InformationItemViewModel>(informationItemViewModel);
            var service = Substitute.For<IInformationItemService>();
            var mapper = Substitute.For<IMapper>();

            fakeImage.FileName.Returns(fileName);
            fakeImage.ContentType.Returns(contentType);
            fakeImage.ContentLength.Returns(fileLength);

            var controller = new InformationItemController(service, mapper);

            // Act
            var valid = validator.IsValid();
            validator.AddToModelError(controller);
            var result = controller.Create(informationItemViewModel, fakeImage) as ViewResult;
            var viewName = result.ViewName;
            var model = result.Model;

            // Assert
            Assert.That(valid, Is.False);
            Assert.That(result, !Is.Null);
            Assert.That("Index", Is.EqualTo(viewName));
            Assert.That(model, Is.Null);
        }

        [Test]
        public void Post_Create_File_Is_Null()
        {
            // Arrange
            var informationItemViewModel = new InformationItemViewModel()
            {
                Position = 1,
                Title = "Menu",
                Content = "Ut at viverra arcu. Donec sed interdum nulla, eu porttitor lorem. Morbi efficitur sapien ullamcorper lorem vestibulum, ut aliquet turpis consequat. Suspendisse ac nibh non velit scelerisque bibendum. "
            };
            HttpPostedFileBase fakeImage = null;
            var service = Substitute.For<IInformationItemService>();
            var mapper = Substitute.For<IMapper>();
            var controller = new InformationItemController(service, mapper);

            // Act
            var result = controller.Create(informationItemViewModel, fakeImage) as ViewResult;
            var viewName = result.ViewName;
            var model = result.Model;


            // Assert
            Assert.That(result, !Is.Null);
            Assert.That("Index", Is.EqualTo(viewName));
            Assert.That(model, Is.Null);
        }

        [Test]
        public void Post_Create_File_Content_Lenght_0()
        {
            // Arrange
            var informationItemViewModel = new InformationItemViewModel()
            {
                Position = 1,
                Title = "Menu",
                Content = "Ut at viverra arcu. Donec sed interdum nulla, eu porttitor lorem. Morbi efficitur sapien ullamcorper lorem vestibulum, ut aliquet turpis consequat. Suspendisse ac nibh non velit scelerisque bibendum. "
            };
            var fakeImage = Substitute.For<HttpPostedFileBase>();
            var service = Substitute.For<IInformationItemService>();
            var mapper = Substitute.For<IMapper>();
            var controller = new InformationItemController(service, mapper);

            // Act
            var result = controller.Create(informationItemViewModel, fakeImage) as ViewResult;
            var viewName = result.ViewName;
            var model = result.Model;

            // Assert
            Assert.That(result, !Is.Null);
            Assert.That("Index", Is.EqualTo(viewName));
            Assert.That(model, Is.Null);
        }

        [Test]
        public void Post_Create_Content_Type_Is_Bad()
        {
            // Arrange
            var informationItemViewModel = new InformationItemViewModel()
            {
                Position = 1,
                Title = "Menu",
                Content = "Ut at viverra arcu. Donec sed interdum nulla, eu porttitor lorem. Morbi efficitur sapien ullamcorper lorem vestibulum, ut aliquet turpis consequat. Suspendisse ac nibh non velit scelerisque bibendum. "
            };
            var fileName = "text.jpg";
            var fileLength = 1000;
            var contentType = "text";
            var fakeImage = Substitute.For<HttpPostedFileBase>();
            var service = Substitute.For<IInformationItemService>();
            var mapper = Substitute.For<IMapper>();

            fakeImage.FileName.Returns(fileName);
            fakeImage.ContentType.Returns(contentType);
            fakeImage.ContentLength.Returns(fileLength);

            var controller = new InformationItemController(service, mapper);

            // Act
            var result = controller.Create(informationItemViewModel, fakeImage) as ViewResult;
            var viewName = result.ViewName;
            var model = result.Model;

            // Assert
            Assert.That(result, !Is.Null);
            Assert.That("Index", Is.EqualTo(viewName));
            Assert.That(model, Is.Null);
        }

        [Test]
        public void Delete_Good()
        {
            // Arrange
            var id = 1;
            var informationItem = new InformationItem()
            {
                ID = 1,
                PictureUrl = "/Content/Images/pizza_1.jpg",
                Position = 1,
                Title = "Menu",
                Content = "Ut at viverra arcu. Donec sed interdum nulla, eu porttitor lorem. Morbi efficitur sapien ullamcorper lorem vestibulum, ut aliquet turpis consequat. Suspendisse ac nibh non velit scelerisque bibendum. "
            };
            var service = Substitute.For<IInformationItemService>();
            var mapper = Substitute.For<IMapper>();
            var fakeController = new FakeController();
            var controller = new InformationItemController(service, mapper);

            fakeController.PrepareFakeAjaxRequest();
            controller.ControllerContext = fakeController.GetControllerContext<InformationItemController>(new RouteData(), controller);
            service.GetInfomrationItem(id).Returns(informationItem);
            service.DeleteInfomationItem(informationItem);
            service.SaveInfomrationItem();

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
            var informationItem = new InformationItem()
            {
                ID = 1,
                PictureUrl = "/Content/Images/pizza_1.jpg",
                Position = 1,
                Title = "Menu",
                Content = "Ut at viverra arcu. Donec sed interdum nulla, eu porttitor lorem. Morbi efficitur sapien ullamcorper lorem vestibulum, ut aliquet turpis consequat. Suspendisse ac nibh non velit scelerisque bibendum. "
            };
            var service = Substitute.For<IInformationItemService>();
            var mapper = Substitute.For<IMapper>();
            var fakeController = new FakeController();
            var controller = new InformationItemController(service, mapper);

            fakeController.PrepareFakeRequest();
            controller.ControllerContext = fakeController.GetControllerContext<InformationItemController>(new RouteData(), controller);
            service.GetInfomrationItem(id).Returns(informationItem);

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
            InformationItem informationItem = null;
            var service = Substitute.For<IInformationItemService>();
            var mapper = Substitute.For<IMapper>();
            var controller = new InformationItemController(service, mapper);

            service.GetInfomrationItem(id).Returns(informationItem);

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
            var informationItem = new InformationItem()
            {
                ID = 1,
                PictureUrl = "/Content/Images/pizza_1.jpg",
                Position = 1,
                Title = "Menu",
                Content = "Ut at viverra arcu. Donec sed interdum nulla, eu porttitor lorem. Morbi efficitur sapien ullamcorper lorem vestibulum, ut aliquet turpis consequat. Suspendisse ac nibh non velit scelerisque bibendum. "
            };
            var informationItemViewModel = new InformationItemViewModel()
            {
                ID = 1,
                PictureUrl = "/Content/Images/pizza_1.jpg",
                Position = 1,
                Title = "Menu",
                Content = "Ut at viverra arcu. Donec sed interdum nulla, eu porttitor lorem. Morbi efficitur sapien ullamcorper lorem vestibulum, ut aliquet turpis consequat. Suspendisse ac nibh non velit scelerisque bibendum. "
            };
            var service = Substitute.For<IInformationItemService>();
            var mapper = Substitute.For<IMapper>();
            var fakeController = new FakeController();
            var controller = new InformationItemController(service, mapper);

            fakeController.PrepareFakeAjaxRequest();
            controller.ControllerContext = fakeController.GetControllerContext<InformationItemController>(new RouteData(), controller);
            service.GetInfomrationItem(id).Returns(informationItem);
            mapper.Map<InformationItem, InformationItemViewModel>(informationItem).Returns(informationItemViewModel);

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
        public void Get_Edit_Not_Ajax_Request()
        {
            // Arrange
            var id = 1;
            var informationItem = new InformationItem()
            {
                ID = 1,
                PictureUrl = "/Content/Images/pizza_1.jpg",
                Position = 1,
                Title = "Menu",
                Content = "Ut at viverra arcu. Donec sed interdum nulla, eu porttitor lorem. Morbi efficitur sapien ullamcorper lorem vestibulum, ut aliquet turpis consequat. Suspendisse ac nibh non velit scelerisque bibendum. "
            };
            var informationItemViewModel = new InformationItemViewModel()
            {
                ID = 1,
                PictureUrl = "/Content/Images/pizza_1.jpg",
                Position = 1,
                Title = "Menu",
                Content = "Ut at viverra arcu. Donec sed interdum nulla, eu porttitor lorem. Morbi efficitur sapien ullamcorper lorem vestibulum, ut aliquet turpis consequat. Suspendisse ac nibh non velit scelerisque bibendum. "
            };
            var service = Substitute.For<IInformationItemService>();
            var mapper = Substitute.For<IMapper>();
            var fakeController = new FakeController();
            var controller = new InformationItemController(service, mapper);

            fakeController.PrepareFakeRequest();
            controller.ControllerContext = fakeController.GetControllerContext<InformationItemController>(new RouteData(), controller);
            service.GetInfomrationItem(id).Returns(informationItem);
            mapper.Map<InformationItem, InformationItemViewModel>(informationItem).Returns(informationItemViewModel);

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
        public void Get_Edit_Model_Null()
        {
            // Arrange
            var id = 1;
            InformationItem informationItem = null;
            var service = Substitute.For<IInformationItemService>();
            var mapper = Substitute.For<IMapper>();
            var controller = new InformationItemController(service, mapper);

            service.GetInfomrationItem(id).Returns(informationItem);

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
            var informationItem = new InformationItem()
            {
                ID = 1,
                PictureUrl = "/Content/Images/pizza_1.jpg",
                Position = 1,
                Title = "Menu",
                Content = "Ut at viverra arcu. Donec sed interdum nulla, eu porttitor lorem. Morbi efficitur sapien ullamcorper lorem vestibulum, ut aliquet turpis consequat. Suspendisse ac nibh non velit scelerisque bibendum. "
            };
            var informationItemViewModel = new InformationItemViewModel()
            {
                ID = 1,
                PictureUrl = "/Content/Images/pizza_1.jpg",
                Position = 1,
                Title = "Menu",
                Content = "Ut at viverra arcu. Donec sed interdum nulla, eu porttitor lorem. Morbi efficitur sapien ullamcorper lorem vestibulum, ut aliquet turpis consequat. Suspendisse ac nibh non velit scelerisque bibendum. "
            };
            var service = Substitute.For<IInformationItemService>();
            var mapper = Substitute.For<IMapper>();
            var validator = new ModelValidator<InformationItemViewModel>(informationItemViewModel);
            var fakeController = new FakeController();
            var controller = new InformationItemController(service, mapper);

            fakeController.PrepareFakeAjaxRequest();
            controller.ControllerContext = fakeController.GetControllerContext<InformationItemController>(new RouteData(), controller);
            service.GetInfomrationItem(informationItem.ID).Returns(informationItem);
            mapper.Map<InformationItemViewModel, InformationItem>(informationItemViewModel).Returns(informationItem);
            service.UpdateInformationItem(informationItem);
            service.SaveInfomrationItem();

            // Act
            var valid = validator.IsValid();
            validator.AddToModelError(controller);
            var result = controller.Edit(informationItemViewModel) as RedirectToRouteResult;
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
            InformationItem informationItem = null;
            var informationItemViewModel = new InformationItemViewModel()
            {
                PictureUrl = "/Content/Images/pizza_1.jpg",
                Position = 1,
                Title = "Menu",
                Content = "Ut at viverra arcu. Donec sed interdum nulla, eu porttitor lorem. Morbi efficitur sapien ullamcorper lorem vestibulum, ut aliquet turpis consequat. Suspendisse ac nibh non velit scelerisque bibendum. "
            };
            var service = Substitute.For<IInformationItemService>();
            var mapper = Substitute.For<IMapper>();
            var validator = new ModelValidator<InformationItemViewModel>(informationItemViewModel);
            var controller = new InformationItemController(service, mapper);

            service.GetInfomrationItem(id).Returns(informationItem);

            // Act
            var valid = validator.IsValid();
            validator.AddToModelError(controller);
            var result = controller.Edit(informationItemViewModel) as HttpNotFoundResult;
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
            var informationItemViewModel = new InformationItemViewModel()
            {
                PictureUrl = "/Content/Images/pizza_1.jpg",
                //Position = 1,
                //Title = "Menu",
                Content = "Ut at viverra arcu. Donec sed interdum nulla, eu porttitor lorem. Morbi efficitur sapien ullamcorper lorem vestibulum, ut aliquet turpis consequat. Suspendisse ac nibh non velit scelerisque bibendum. "
            };
            var service = Substitute.For<IInformationItemService>();
            var mapper = Substitute.For<IMapper>();
            var controller = new InformationItemController(service, mapper);
            var validator = new ModelValidator<InformationItemViewModel>(informationItemViewModel);

            // Act
            var valid = validator.IsValid();
            validator.AddToModelError(controller);
            var result = controller.Edit(informationItemViewModel) as HttpStatusCodeResult;
            var statusCode = result.StatusCode;

            // Assert
            Assert.That(result, !Is.Null);
            Assert.That(valid, Is.False);
            Assert.That(304, Is.EqualTo(statusCode));
        }
    }
}
