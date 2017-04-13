using NSubstitute;
using NUnit.Framework;
using PizzaShop.Areas.Admin.Controllers;
using PizzaShop.Areas.Admin.Models.ViewModels;
using PizzaShop.Models.PizzaShopModels.CMS;
using PizzaShop.Services.Cms.Interfaces;
using PizzaShop.Tests.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PizzaShop.Tests.AdminControllers
{
    [TestFixture]
    public class SliderItemControllerTest
    {
        [Test]
        public void Index()
        {
            // Arrange
            var sliderItems = new List<SliderItem>()
            {
                new SliderItem() { Position=1,ShortDescription="Slider description 1",PictureUrl="/Content/Images/pizzaSlide_1.jpg" },
                new SliderItem() { Position=2,ShortDescription="Slider description 2",PictureUrl="/Content/Images/pizzaSlide_2.jpg" },
                new SliderItem() { Position=3,ShortDescription="Slider description 3",PictureUrl="/Content/Images/pizzaSlide_3.jpg" }
            };
            var service = Substitute.For<ISliderItemService>();
            var controller = new SliderItemController(service);

            service.SliderItemList().Returns(sliderItems);

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
            var service = Substitute.For<ISliderItemService>();
            var controller = new SliderItemController(service);

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
            var sliderItemViewModel = new SliderItemViewModel()
            {
                Position = 1,
                ShortDescription = "Slider description 1",
                PictureUrl = "/Content/Images/pizzaSlide_1.jpg"
            };
            var sliderItem = new SliderItem()
            {
                Position = 1,
                ShortDescription = "Slider description 1",
                PictureUrl = "/Content/Images/pizzaSlide_1.jpg"
            };
            var fileName = "text.jpg";
            var fileLength = 1000;
            var contentType = "image";
            var fakeImage = Substitute.For<HttpPostedFileBase>();
            var validator = new ModelValidator<SliderItemViewModel>(sliderItemViewModel);
            var service = Substitute.For<ISliderItemService>();

            fakeImage.FileName.Returns(fileName);
            fakeImage.ContentType.Returns(contentType);
            fakeImage.ContentLength.Returns(fileLength);

            var controller = new SliderItemController(service);

            service.MapViewModelToModel(sliderItemViewModel).Returns(sliderItem);
            sliderItem.PictureUrl = service.AddSliderItemImage(fakeImage);
            service.CreateSliderItem(sliderItem);
            service.SaveSliderItem();

            // Act
            var valid = validator.IsValid();
            validator.AddToModelError(controller);
            var result = controller.Create(sliderItemViewModel, fakeImage) as RedirectToRouteResult;
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
            var sliderItemViewModel = new SliderItemViewModel()
            {
                //Position = 1,
                ShortDescription = "Slider description 1",
                PictureUrl = "/Content/Images/pizzaSlide_1.jpg"
            };
            var fileName = "text.jpg";
            var fileLength = 1000;
            var contentType = "image";
            var fakeImage = Substitute.For<HttpPostedFileBase>();
            var validator = new ModelValidator<SliderItemViewModel>(sliderItemViewModel);
            var service = Substitute.For<ISliderItemService>();

            fakeImage.FileName.Returns(fileName);
            fakeImage.ContentType.Returns(contentType);
            fakeImage.ContentLength.Returns(fileLength);

            var controller = new SliderItemController(service);

            // Act
            var valid = validator.IsValid();
            validator.AddToModelError(controller);
            var result = controller.Create(sliderItemViewModel, fakeImage) as ViewResult;
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
            var sliderItemViewModel = new SliderItemViewModel()
            {
                Position = 1,
                ShortDescription = "Slider description 1",
                PictureUrl = "/Content/Images/pizzaSlide_1.jpg"
            };
            HttpPostedFileBase fakeImage = null;
            var service = Substitute.For<ISliderItemService>();
            var controller = new SliderItemController(service);

            // Act
            var result = controller.Create(sliderItemViewModel, fakeImage) as ViewResult;
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
            var sliderItemViewModel = new SliderItemViewModel()
            {
                Position = 1,
                ShortDescription = "Slider description 1",
                PictureUrl = "/Content/Images/pizzaSlide_1.jpg"
            };
            var fakeImage = Substitute.For<HttpPostedFileBase>();
            var service = Substitute.For<ISliderItemService>();
            var controller = new SliderItemController(service);

            // Act
            var result = controller.Create(sliderItemViewModel, fakeImage) as ViewResult;
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
            var sliderItemViewModel = new SliderItemViewModel()
            {
                Position = 1,
                ShortDescription = "Slider description 1",
                PictureUrl = "/Content/Images/pizzaSlide_1.jpg"
            };
            var fileName = "text.jpg";
            var fileLength = 1000;
            var contentType = "text";
            var fakeImage = Substitute.For<HttpPostedFileBase>();
            var service = Substitute.For<ISliderItemService>();

            fakeImage.FileName.Returns(fileName);
            fakeImage.ContentType.Returns(contentType);
            fakeImage.ContentLength.Returns(fileLength);

            var controller = new SliderItemController(service);

            // Act
            var result = controller.Create(sliderItemViewModel, fakeImage) as ViewResult;
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
            var sliderItem = new SliderItem()
            {
                ID = id,
                Position = 1,
                ShortDescription = "Slider description 1",
                PictureUrl = "/Content/Images/pizzaSlide_1.jpg"
            };
            var service = Substitute.For<ISliderItemService>();
            var fakeController = new FakeController();
            var controller = new SliderItemController(service);

            fakeController.PrepareFakeAjaxRequest();
            controller.ControllerContext = fakeController.GetControllerContext<SliderItemController>(new RouteData(), controller);
            service.GetSliderItem(id).Returns(sliderItem);
            service.DeleteSliderItem(sliderItem);
            service.SaveSliderItem();

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
            var sliderItem = new SliderItem()
            {
                ID = id,
                Position = 1,
                ShortDescription = "Slider description 1",
                PictureUrl = "/Content/Images/pizzaSlide_1.jpg"
            };
            var service = Substitute.For<ISliderItemService>();
            var fakeController = new FakeController();
            var controller = new SliderItemController(service);

            fakeController.PrepareFakeRequest();
            controller.ControllerContext = fakeController.GetControllerContext<SliderItemController>(new RouteData(), controller);
            service.GetSliderItem(id).Returns(sliderItem);

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
            SliderItem sliderItem = null;
            var service = Substitute.For<ISliderItemService>();
            var controller = new SliderItemController(service);

            service.GetSliderItem(id).Returns(sliderItem);

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
            var sliderItemViewModel = new SliderItemViewModel()
            {
                ID = id,
                Position = 1,
                ShortDescription = "Slider description 1",
                PictureUrl = "/Content/Images/pizzaSlide_1.jpg"
            };
            var sliderItem = new SliderItem()
            {
                ID = id,
                Position = 1,
                ShortDescription = "Slider description 1",
                PictureUrl = "/Content/Images/pizzaSlide_1.jpg"
            };
            var service = Substitute.For<ISliderItemService>();
            var fakeController = new FakeController();
            var controller = new SliderItemController(service);

            fakeController.PrepareFakeAjaxRequest();
            controller.ControllerContext = fakeController.GetControllerContext<SliderItemController>(new RouteData(), controller);
            service.GetSliderItem(id).Returns(sliderItem);
            service.MapModelToViewModel(sliderItem).Returns(sliderItemViewModel);

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
            var sliderItemViewModel = new SliderItemViewModel()
            {
                ID = id,
                Position = 1,
                ShortDescription = "Slider description 1",
                PictureUrl = "/Content/Images/pizzaSlide_1.jpg"
            };
            var sliderItem = new SliderItem()
            {
                ID = id,
                Position = 1,
                ShortDescription = "Slider description 1",
                PictureUrl = "/Content/Images/pizzaSlide_1.jpg"
            };
            var service = Substitute.For<ISliderItemService>();
            var fakeController = new FakeController();
            var controller = new SliderItemController(service);

            fakeController.PrepareFakeRequest();
            controller.ControllerContext = fakeController.GetControllerContext<SliderItemController>(new RouteData(), controller);
            service.GetSliderItem(id).Returns(sliderItem);
            service.MapModelToViewModel(sliderItem).Returns(sliderItemViewModel);
            
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
            SliderItem sliderItem = null;
            var service = Substitute.For<ISliderItemService>();
            var controller = new SliderItemController(service);

            service.GetSliderItem(id).Returns(sliderItem);

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
            var sliderItemViewModel = new SliderItemViewModel()
            {
                ID = id,
                Position = 1,
                ShortDescription = "Slider description 1",
                PictureUrl = "/Content/Images/pizzaSlide_1.jpg"
            };
            var sliderItem = new SliderItem()
            {
                ID = id,
                Position = 1,
                ShortDescription = "Slider description 1",
                PictureUrl = "/Content/Images/pizzaSlide_1.jpg"
            };
            var service = Substitute.For<ISliderItemService>();
            var validator = new ModelValidator<SliderItemViewModel>(sliderItemViewModel);
            var fakeController = new FakeController();
            var controller = new SliderItemController(service);

            fakeController.PrepareFakeAjaxRequest();
            controller.ControllerContext = fakeController.GetControllerContext<SliderItemController>(new RouteData(), controller);
            service.GetSliderItem(sliderItem.ID).Returns(sliderItem);
            service.MapViewModelToModel(sliderItemViewModel).Returns(sliderItem);
            service.UpdateSliderItem(sliderItem);
            service.SaveSliderItem();

            // Act
            var valid = validator.IsValid();
            validator.AddToModelError(controller);
            var result = controller.Edit(sliderItemViewModel) as RedirectToRouteResult;
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
            SliderItem sliderItem = null;
            var sliderItemViewModel = new SliderItemViewModel()
            {
                ID = id,
                Position = 1,
                ShortDescription = "Slider description 1",
                PictureUrl = "/Content/Images/pizzaSlide_1.jpg"
            };
            var service = Substitute.For<ISliderItemService>();
            var validator = new ModelValidator<SliderItemViewModel>(sliderItemViewModel);
            var controller = new SliderItemController(service);

            service.GetSliderItem(id).Returns(sliderItem);

            // Act
            var valid = validator.IsValid();
            validator.AddToModelError(controller);
            var result = controller.Edit(sliderItemViewModel) as HttpNotFoundResult;
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
            var sliderItemViewModel = new SliderItemViewModel()
            {
                Position = 1,
                //ShortDescription = "Slider description 1",
                PictureUrl = "/Content/Images/pizzaSlide_1.jpg"
            };
            var service = Substitute.For<ISliderItemService>();
            var controller = new SliderItemController(service);
            var validator = new ModelValidator<SliderItemViewModel>(sliderItemViewModel);

            // Act
            var valid = validator.IsValid();
            validator.AddToModelError(controller);
            var result = controller.Edit(sliderItemViewModel) as HttpStatusCodeResult;
            var statusCode = result.StatusCode;

            // Assert
            Assert.That(result, !Is.Null);
            Assert.That(valid, Is.False);
            Assert.That(304, Is.EqualTo(statusCode));
        }
    }
}
