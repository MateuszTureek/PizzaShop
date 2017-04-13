using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using PizzaShop.Controllers;
using NUnit.Framework;
using NSubstitute;
using System.Data.Entity;
using PizzaShop.Services.Cms.Interfaces;
using PizzaShop.Models.PizzaShopModels.CMS;
using PizzaShop.Services.Xml.XmlModels;
using PizzaShop.Services.Xml;

namespace PizzaShop.Tests.Controllers
{
    [TestFixture]
    public class HomeControllerTest
    {
        [Test]
        public void Index()
        {
            // Arrange
            var homePresentationService = Substitute.For<IHomePresentationService>();
            var xmlManager = Substitute.For<IXmlManager>();
            HomeController controller = new HomeController(homePresentationService, xmlManager);

            // Act
            ViewResult result = controller.Index() as ViewResult;
            string viewName = result.ViewName;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", viewName);
        }

        [Test]
        public void Gallery()
        {
            // Arrange
            List<GalleryItem> galleryItems = new List<GalleryItem>()
            {
                new GalleryItem() { Position=1, PictureUrl="/Content/Images/pizza_1.jpg" },
                new GalleryItem() { Position=2, PictureUrl="/Content/Images/pizza_2.jpg" },
                new GalleryItem() { Position=3, PictureUrl="/Content/Images/pizza_3.jpeg" },
                new GalleryItem() { Position=4, PictureUrl="/Content/Images/pizzaSlide_1.jpg" },
                new GalleryItem() { Position=5, PictureUrl="/Content/Images/pizzaSlide_2.jpg" },
                new GalleryItem() { Position=6, PictureUrl="/Content/Images/pizzaSlide_3.jpg" }
            };

            var homePresentationService = Substitute.For<IHomePresentationService>();
            var xmlManager = Substitute.For<IXmlManager>();
            HomeController controller = new HomeController(homePresentationService, xmlManager);

            // Act
            homePresentationService.GetAllGalleryItems().Returns(galleryItems);
            ViewResult result = controller.Gallery() as ViewResult;
            string viewName = result.ViewName;
            var model = result.Model as List<GalleryItem>;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Gallery", viewName);
            Assert.IsNotNull(model);
        }

        [Test]
        public void Contact()
        {
            // Arrange
            ContactAndHoursViewModel contactAndHoursViewModel = new ContactAndHoursViewModel()
            {
                Address = new Address()
                {
                    DeliveryContact = "111 111 111",
                    Email = "fakeEmail@gmail.com",
                    InformationContact = "000 000 000"
                },
                Contact = new Contact()
                {
                    City = "Poland",
                    PostalCode = "11-212",
                    Street = "fake street"
                },
                WorksDays = new List<Days>()
                {
                    new Days()
                    {
                        Name="Pon-Pt.",WorkHours="8.00-22.00"
                    },
                    new Days()
                    {
                        Name="Sob-Nedz.",WorkHours="10.00-21.00"
                    }
                }
            };
            var homePresentationService = Substitute.For<IHomePresentationService>();
            var xmlManager = Substitute.For<IXmlManager>();
            HomeController controller = new HomeController(homePresentationService, xmlManager);

            // Act
            xmlManager.GetFullShopInformation().Returns(contactAndHoursViewModel);
            ViewResult result = controller.Contact() as ViewResult;
            string viewName = result.ViewName;
            var model = result.Model;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Contact", viewName);
            Assert.IsNotNull(model);
        }

        [Test]
        public void SiteMenuPartial()
        {
            // Arrange
            var menuItems = new List<MenuItem>()
            {
                new MenuItem() { Position = 1, Title = "Strona główna", ActionName = "Index" },
                new MenuItem() { Position = 2, Title = "Menu", ActionName = "Menu" },
                new MenuItem() { Position = 3, Title = "Galeria", ActionName = "Gallery" },
                new MenuItem() { Position = 4, Title = "Kontakt", ActionName = "Contact" }
            };
            var homePresentationService = Substitute.For<IHomePresentationService>();
            var xmlManager = Substitute.For<IXmlManager>();
            HomeController controller = new HomeController(homePresentationService, xmlManager);

            // Act
            homePresentationService.GetAllMenuItems().Returns(menuItems);
            var result = controller.SiteMenuPartial() as PartialViewResult;
            string viewName = result.ViewName;
            var model = result.Model as List<MenuItem>;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("_SiteMenuPartial", viewName);
            Assert.NotNull(model);
        }

        [Test]
        public void SliderPartial()
        {
            //Arrange
            var sliderItems = new List<SliderItem>()
            {
                new SliderItem() { Position=1,ShortDescription="Slider description 1",PictureUrl="/Content/Images/pizzaSlide_1.jpg" },
                new SliderItem() { Position=2,ShortDescription="Slider description 2",PictureUrl="/Content/Images/pizzaSlide_2.jpg" },
                new SliderItem() { Position=3,ShortDescription="Slider description 3",PictureUrl="/Content/Images/pizzaSlide_1.jpeg" }
            };
            var homePresentationService = Substitute.For<IHomePresentationService>();
            var xmlManager = Substitute.For<IXmlManager>();
            HomeController controller = new HomeController(homePresentationService, xmlManager);

            //Act
            homePresentationService.GetAllSliderItems().Returns(sliderItems);
            PartialViewResult result = controller.SliderPartial() as PartialViewResult;
            string viewName = result.ViewName;
            List<SliderItem> model = result.Model as List<SliderItem>;

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual("_SliderPartial", viewName);
            Assert.NotNull(model);
        }

        [Test]
        public void InformationPartial()
        {
            //Arrange
            var infoItems = new List<InformationItem>()
            {
                new InformationItem() { PictureUrl="/Content/Images/pizza_1.jpg", Title="Menu",Content="Ut at viverra arcu. Donec sed interdum nulla, eu porttitor lorem. Morbi efficitur sapien ullamcorper lorem vestibulum, ut aliquet turpis consequat. Suspendisse ac nibh non velit scelerisque bibendum. " },
                new InformationItem() { PictureUrl="/Content/Images/pizza_2.jpg", Title="Promocja", Content="Praesent in placerat risus, et ornare lectus. Praesent turpis tortor, consectetur quis enim id, consequat pharetra velit. Nullam tempor convallis ante at finibus. " },
                new InformationItem() { PictureUrl="/Content/Images/pizza_3.jpg", Title="O nas", Content="Suspendisse sed enim porttitor, auctor urna et, bibendum enim. In imperdiet tellus ex, sed efficitur odio dignissim eget. Ut fermentum ipsum eget lorem ornare condimentum. " }
            };
            var homePresentationService = Substitute.For<IHomePresentationService>();
            var xmlManager = Substitute.For<IXmlManager>();
            HomeController controller = new HomeController(homePresentationService, xmlManager);

            //Act
            homePresentationService.GetAllInformationItems().Returns(infoItems);
            var result = controller.InformationPartial() as PartialViewResult;
            string viewName = result.ViewName;
            var model = result.Model as List<InformationItem>;

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual("_InformationPartial", viewName);
            Assert.NotNull(model);
        }

        [Test]
        public void EventPartial()
        {
            //Arrange
            var eventItems = new List<Event>()
            {
                new Event() { AddedDate=DateTime.Now,Title="Wydarzenie 1", Content="Treść wydarzenia 1" },
                new Event() { AddedDate=DateTime.Now,Title="Wydarzenie 2", Content="Treść wydarzenia 2" },
                new Event() { AddedDate=DateTime.Now,Title="Wydarzenie 3", Content="Treść wydarzenia 3" }
            };
            var homePresentationService = Substitute.For<IHomePresentationService>();
            var xmlManager = Substitute.For<IXmlManager>();
            HomeController controller = new HomeController(homePresentationService, xmlManager);

            //Act
            homePresentationService.GetAllEvents().Returns(eventItems);
            PartialViewResult result = controller.EventPartial() as PartialViewResult;
            string viewName = result.ViewName;
            var model = result.Model as List<Event>;

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual("_EventPartial", viewName);
            Assert.NotNull(model);
        }

        [Test]
        public void NewsPartial()
        {
            //Act
            var news = new List<News>()
            {
                new News() { AddedDate=DateTime.Now, Position=1, Title="New 1",Content="Quisque nulla nunc, tempor eu lorem non, pharetra laoreet massa." },
                new News() { AddedDate=DateTime.Now, Position=1, Title="New 1",Content="Nunc iaculis, elit eu aliquam placerat, diam est feugiat urna, et lacinia tellus lectus a sem. " },
                new News() { AddedDate=DateTime.Now, Position=1, Title="New 1",Content="In imperdiet tellus ex, sed efficitur odio dignissim eget." }
            };
            var homePresentationService = Substitute.For<IHomePresentationService>();
            var xmlManager = Substitute.For<IXmlManager>();
            HomeController controller = new HomeController(homePresentationService, xmlManager);

            //Act
            homePresentationService.GetAllNews().Returns(news);
            PartialViewResult result = controller.NewsPartial() as PartialViewResult;
            string viewName = result.ViewName;
            var model = result.Model as List<News>;

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual("_NewPartial", viewName);
            Assert.NotNull(model);
        }

        [Test]
        public void ShopContactPartial()
        {
            //Arrange
            var shopContact = new ShopContact()
            {
                Address = new Address() { DeliveryContact = "111 111 111", Email = "fakeEmail@gmail.com", InformationContact = "000 000 000" },
                Contact = new Contact() { City = "FakeCisty", PostalCode = "22-333", Street = "ul. fake 111" }
            };
            var homePresentationService = Substitute.For<IHomePresentationService>();
            var xmlManager = Substitute.For<IXmlManager>();
            HomeController controller = new HomeController(homePresentationService, xmlManager);

            //Act
            xmlManager.GetXmlModel<ShopContact>("ShopContact").Returns(shopContact);
            PartialViewResult result = controller.ShopContactPartial() as PartialViewResult;
            string viewName = result.ViewName;
            var model = result.Model as ShopContact;

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual("_ShopContactPartial", viewName);
            Assert.NotNull(model);
        }

        [Test]
        public void OpeningHoursPartial()
        {
            //Arrange
            var openingHours = new OpeningHours()
            {
                WorksDays = new List<Days>()
                {
                    new Days() { Name="Pon-Czw",WorkHours="12.00 - 22.00" },
                    new Days() { Name="Pt-Sob",WorkHours="12.00 - 22.00" },
                    new Days() { Name="Niedz",WorkHours="12.00 - 22.00" }
                }
            };
            var homePresentationService = Substitute.For<IHomePresentationService>();
            var xmlManager = Substitute.For<IXmlManager>();
            HomeController controller = new HomeController(homePresentationService, xmlManager);

            //Act
            xmlManager.GetXmlModel<OpeningHours>("OpeningHours").Returns(openingHours);
            PartialViewResult result = controller.OpeningHoursPartial() as PartialViewResult;
            string viewName = result.ViewName;
            OpeningHours model = result.Model as OpeningHours;

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual("_OpeningHoursPartial", viewName);
            Assert.NotNull(model);
        }

        [Test]
        public void DeliveryContact()
        {
            //Arrange
            var shopContact = new ShopContact()
            {
                Address = new Address() { DeliveryContact = "111 111 111", Email = "fakeEmail@gmail.com", InformationContact = "000 000 000" },
                Contact = new Contact() { City = "FakeCisty", PostalCode = "22-333", Street = "ul. fake 111" }
            };
            var homePresentationService = Substitute.For<IHomePresentationService>();
            var xmlManager = Substitute.For<IXmlManager>();
            HomeController controller = new HomeController(homePresentationService, xmlManager);

            //Act
            xmlManager.GetXmlModel<ShopContact>("ShopContact").Returns(shopContact);
            ContentResult result = controller.DeliveryContact() as ContentResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(shopContact.Address.DeliveryContact, result.Content);
        }
    }
}
