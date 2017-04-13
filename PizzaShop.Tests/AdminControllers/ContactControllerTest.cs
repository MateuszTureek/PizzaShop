using AutoMapper;
using NSubstitute;
using NUnit.Framework;
using PizzaShop.Areas.Admin.Controllers;
using PizzaShop.Services.Xml;
using PizzaShop.Services.Xml.XmlModels;
using PizzaShop.Tests.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PizzaShop.Tests.AdminControllers
{
    [TestFixture]
    public class ContactControllerTest
    {
        [Test]
        public void Index()
        {
            // Arrange
            var shopContact = new ShopContact()
            {
                Address = new Address() { DeliveryContact = "111 111 111", Email = "fakeEmail@gmail.com", InformationContact = "000 000 000" },
                Contact = new Contact() { City = "FakeCisty", PostalCode = "22-333", Street = "ul. fake 111" }
            };

            var xmlManager = Substitute.For<IXmlManager>();
            var mapper = Substitute.For<IMapper>();

            ContactController controller = new ContactController(xmlManager, mapper);

            xmlManager.GetXmlModel<ShopContact>(GlobalXmlManager.ContactFileName).Returns(shopContact);

            // Act
            var result = controller.Index() as ViewResult;
            var viewName = result.ViewName;
            var model = result.Model;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", viewName);
            Assert.IsNotNull(model);
        }

        [Test]
        public void Get_Edit_Ajax_Request()
        {
            // Arrange
            var shopContact = new ShopContact()
            {
                Address = new Address() { DeliveryContact = "111 111 111", Email = "fakeEmail@gmail.com", InformationContact = "000 000 000" },
                Contact = new Contact() { City = "FakeCisty", PostalCode = "22-333", Street = "ul. fake 111" }
            };
            var shopContactViewModel = new ShopContactViewModel()
            {
                DeliveryContact = "111 111 111",
                Email = "fakeEmail@gmail.com",
                InformationContact = "000 000 000",
                City = "FakeCisty",
                PostalCode = "22-333",
                Street = "ul. fake 111"
            };

            var xmlManager = Substitute.For<IXmlManager>();
            var mapper = Substitute.For<IMapper>();
            
            var fakeController = new FakeController();
            fakeController.PrepareFakeAjaxRequest();
            var controller = new ContactController(xmlManager, mapper);
            controller.ControllerContext = fakeController.
                                            GetControllerContext<ContactController>(new RouteData(), controller);
            
            xmlManager.GetXmlModel<ShopContact>(GlobalXmlManager.ContactFileName).Returns(shopContact);
            mapper.Map<ShopContact, ShopContactViewModel>(shopContact).Returns(shopContactViewModel);

            // Act
            var result = controller.Edit() as PartialViewResult;
            var ajaxRequest = controller.Request.IsAjaxRequest();
            var viewName = result.ViewName;
            var model = result.Model;

            // Assert
            Assert.IsTrue(ajaxRequest);
            Assert.IsNotNull(result);
            Assert.AreEqual("_EditPartial", viewName);
            Assert.IsNotNull(model);
        }

        [Test]
        public void Get_Edit_Not_Ajax_Request()
        {
            // Arrange
            var xmlManager = Substitute.For<IXmlManager>();
            var mapper = Substitute.For<IMapper>();
            
            var fakeController = new FakeController();
            fakeController.PrepareFakeRequest();
            var controller = new ContactController(xmlManager, mapper);
            controller.ControllerContext = fakeController.GetControllerContext<ContactController>(new RouteData(), controller);

            // Act
            var result = controller.Edit() as HttpStatusCodeResult;
            var ajaxRequest = controller.Request.IsAjaxRequest();
            var statusCode = result.StatusCode;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(ajaxRequest);
            Assert.That(400, Is.EqualTo(statusCode));
        }

        [Test]
        public void Get_Edit_Ajax_Request_ShopContact_Null()
        {
            // Arrange
            var xmlManager = Substitute.For<IXmlManager>();
            var mapper = Substitute.For<IMapper>();

            var fakeController = new FakeController();
            fakeController.PrepareFakeAjaxRequest();
            var controller = new ContactController(xmlManager, mapper);
            controller.ControllerContext = fakeController.GetControllerContext<ContactController>(new RouteData(), controller);

            ShopContact shopContact = null;
            xmlManager.GetXmlModel<ShopContact>(GlobalXmlManager.ContactFileName).Returns(shopContact);

            // Act
            var result = controller.Edit() as HttpStatusCodeResult;
            var ajaxRequest = controller.Request.IsAjaxRequest();
            var statusCode = result.StatusCode;

            // Assert
            Assert.IsTrue(ajaxRequest);
            Assert.IsNotNull(result);
            Assert.That(404, Is.EqualTo(statusCode));
        }

        [Test]
        public void Post_Edit_Model_Is_Valid()
        {
            // Arrange
            var shopContactViewModel = new ShopContactViewModel()
            {
                DeliveryContact = "111 111 111",
                Email = "fakeEmail@gmail.com",
                InformationContact = "000 000 000",
                City = "FakeCisty",
                PostalCode = "22-333",
                Street = "ul. fake 111",
            };
            var shopContact = new ShopContact()
            {
                Address = new Address() { DeliveryContact = "111 111 111", Email = "fakeEmail@gmail.com", InformationContact = "000 000 000" },
                Contact = new Contact() { City = "FakeCisty", PostalCode = "22-333", Street = "ul. fake 111" }
            };
            var xmlManager = Substitute.For<IXmlManager>();
            var mapper = Substitute.For<IMapper>();

            mapper.Map<ShopContactViewModel, ShopContact>(shopContactViewModel).Returns(shopContact);
            xmlManager.CreateXmlFile<ShopContact>(GlobalXmlManager.ContactFileName, shopContact);

            var controller = new ContactController(xmlManager, mapper);
            var validator = new ModelValidator<ShopContactViewModel>(shopContactViewModel);

            //Act
            var result = controller.Edit(shopContactViewModel) as RedirectToRouteResult;
            var actionName = result.RouteValues.Values.ElementAt(0); //only action name
            var modelIsValid = validator.IsValid();

            //Assert
            Assert.IsNotNull(result);
            Assert.That("Index", Is.EqualTo(actionName));
            Assert.IsTrue(modelIsValid);
        }

        [Test]
        public void Post_Edit_Model_Is_Not_Valid()
        {
            // Arrange
            var shopContactViewModel = new ShopContactViewModel()
            {
                DeliveryContact = "111 111 111",
                Email = "fakeEmail@gmail.com",
                InformationContact = "000 000 000",
                City = "FakeCisty",
                PostalCode = "22-333"/*,
                Street = "ul. fake 111"*/ // field is required
            };
            var shopContact = new ShopContact()
            {
                Address = new Address() { DeliveryContact = "111 111 111", Email = "fakeEmail@gmail.com", InformationContact = "000 000 000" },
                Contact = new Contact() { City = "FakeCisty", PostalCode = "22-333", Street = "ul. fake 111" }
            };
            var xmlManager = Substitute.For<IXmlManager>();
            var mapper = Substitute.For<IMapper>();

            mapper.Map<ShopContactViewModel, ShopContact>(shopContactViewModel).Returns(shopContact);
            xmlManager.CreateXmlFile<ShopContact>(GlobalXmlManager.ContactFileName, shopContact);

            var controller = new ContactController(xmlManager, mapper);
            var validator = new ModelValidator<ShopContactViewModel>(shopContactViewModel);

            //Act
            var valid = validator.IsValid();
            validator.AddToModelError(controller);
            var result = controller.Edit(shopContactViewModel) as HttpStatusCodeResult;
            var statusCode = result.StatusCode;

            //Assert
            Assert.That(result,!Is.Null);
            Assert.That(304, Is.EqualTo(statusCode));
            Assert.That(valid, Is.False);
        }
    }
}
