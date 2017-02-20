using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using PizzaShop.Controllers;
using NUnit.Framework;
using NSubstitute;
using PizzaShop.UnitOfWork;
using System.Data.Entity;
using PizzaShop.Models.PizzaShopModels.CMS;
using PizzaShop.Repositories.PizzaShopRepositories.Interfaces;
using PizzaShop.Repositories;
using PizzaShop.Services.XmlServices;
using PizzaShop.Services.XmlServices.XmlModels;

namespace PizzaShop.Tests.Controllers
{
    [TestFixture]
    public class HomeControllerTest
    {
        [Test]
        public void Index()
        {
            // Arrange
            DbContext context = Substitute.For<DbContext>();
            HomeUnitOfWork unitOfWork = Substitute.For<HomeUnitOfWork>(context);
            IXmlManager xmlManager = Substitute.For<IXmlManager>();
            HomeController controller = new HomeController(unitOfWork, xmlManager);

            // Act
            ViewResult result = controller.Index() as ViewResult;
            string viewName = result.ViewName;
            var model = result.Model;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", viewName);
        }

        [Test]
        public void Gallery()
        {
            // Arrange
            DbContext context = Substitute.For<DbContext>();
            HomeUnitOfWork unitOfWork = Substitute.For<HomeUnitOfWork>(context);
            unitOfWork.GalleryRepository = Substitute.For<IGetRepository<GalleryItem>>();
            unitOfWork.GalleryRepository.All().Returns(new List<GalleryItem>()
            {
                new GalleryItem() { Position=1, PictureUrl="/Content/Images/pizza_1.jpg" },
                new GalleryItem() { Position=2, PictureUrl="/Content/Images/pizza_2.jpg" },
                new GalleryItem() { Position=3, PictureUrl="/Content/Images/pizza_3.jpeg" },
                new GalleryItem() { Position=4, PictureUrl="/Content/Images/pizzaSlide_1.jpg" },
                new GalleryItem() { Position=5, PictureUrl="/Content/Images/pizzaSlide_2.jpg" },
                new GalleryItem() { Position=6, PictureUrl="/Content/Images/pizzaSlide_3.jpg" }
            });
            IXmlManager xmlManager = Substitute.For<IXmlManager>();
            HomeController controller = new HomeController(unitOfWork, xmlManager);

            // Act
            ViewResult result = controller.Gallery() as ViewResult;
            string viewName = result.ViewName;
            var model = result.Model as List<GalleryItem>;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Gallery", viewName);
            Assert.IsNotNull(model);
            Assert.AreEqual(6, model.Count);
        }

        [Test]
        public void Contact()
        {
            // Arrange
            DbContext context = Substitute.For<DbContext>();
            HomeUnitOfWork unitOfWork = Substitute.For<HomeUnitOfWork>(context);
            IXmlManager xmlManager = Substitute.For<IXmlManager>();
            HomeController controller = new HomeController(unitOfWork, xmlManager);

            // Act
            ViewResult result = controller.Contact() as ViewResult;
            string viewName = result.ViewName;
            var model = result.Model;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Contact", viewName);
        }

        [Test]
        public void SiteMenuPartial()
        {
            // Arrange
            DbContext context = Substitute.For<DbContext>();
            HomeUnitOfWork unitOfWork = Substitute.For<HomeUnitOfWork>(context);
            unitOfWork.MenuItemRepository = Substitute.For<IMenuItemRepository>();
            unitOfWork.MenuItemRepository.All().Returns(new List<MenuItem>()
            {
                new MenuItem() { Position = 1, Title = "Strona główna", ActionName = "Index" },
                new MenuItem() { Position = 2, Title = "Menu", ActionName = "Menu" },
                new MenuItem() { Position = 3, Title = "Galeria", ActionName = "Gallery" },
                new MenuItem() { Position = 4, Title = "Kontakt", ActionName = "Contact" }
            });
            IXmlManager xmlManager = Substitute.For<IXmlManager>();
            HomeController controller = new HomeController(unitOfWork, xmlManager);

            // Act
            PartialViewResult result = controller.SiteMenuPartial() as PartialViewResult;
            string viewName = result.ViewName;
            List<MenuItem> model = result.Model as List<MenuItem>;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("_SiteMenuPartial", viewName);
            Assert.NotNull(model);
            Assert.AreEqual(4, model.Count);
        }

        [Test]
        public void SliderPartial()
        {
            //Arrange
            DbContext context = Substitute.For<DbContext>();
            HomeUnitOfWork unityOfWork = Substitute.For<HomeUnitOfWork>(context);
            unityOfWork.SliderItemRepository = Substitute.For<IGetRepository<SliderItem>>();
            unityOfWork.SliderItemRepository.All().Returns(new List<SliderItem>()
            {
                new SliderItem() { Position=1,ShortDescription="Slider description 1",PictureUrl="/Content/Images/pizzaSlide_1.jpg" },
                new SliderItem() { Position=2,ShortDescription="Slider description 2",PictureUrl="/Content/Images/pizzaSlide_2.jpg" },
                new SliderItem() { Position=3,ShortDescription="Slider description 3",PictureUrl="/Content/Images/pizzaSlide_1.jpeg" }
            });
            IXmlManager xmlManager = Substitute.For<IXmlManager>();
            HomeController controller = new HomeController(unityOfWork, xmlManager);

            //Act
            PartialViewResult result = controller.SliderPartial() as PartialViewResult;
            string viewName = result.ViewName;
            List<SliderItem> model = result.Model as List<SliderItem>;

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual("_SliderPartial", viewName);
            Assert.NotNull(model);
            Assert.AreEqual(3, model.Count);
        }

        [Test]
        public void InformationPartial()
        {
            //Arrange
            DbContext context = Substitute.For<DbContext>();
            HomeUnitOfWork unityOfWork = Substitute.For<HomeUnitOfWork>(context);
            unityOfWork.InformationItemRepository = Substitute.For<IGetRepository<InformationItem>>();
            unityOfWork.InformationItemRepository.All().Returns(new List<InformationItem>()
            {
                new InformationItem() { PictureUrl="/Content/Images/pizza_1.jpg", Title="Menu",Content="Ut at viverra arcu. Donec sed interdum nulla, eu porttitor lorem. Morbi efficitur sapien ullamcorper lorem vestibulum, ut aliquet turpis consequat. Suspendisse ac nibh non velit scelerisque bibendum. " },
                new InformationItem() { PictureUrl="/Content/Images/pizza_2.jpg", Title="Promocja", Content="Praesent in placerat risus, et ornare lectus. Praesent turpis tortor, consectetur quis enim id, consequat pharetra velit. Nullam tempor convallis ante at finibus. " },
                new InformationItem() { PictureUrl="/Content/Images/pizza_3.jpg", Title="O nas", Content="Suspendisse sed enim porttitor, auctor urna et, bibendum enim. In imperdiet tellus ex, sed efficitur odio dignissim eget. Ut fermentum ipsum eget lorem ornare condimentum. " }
            });
            IXmlManager xmlManager = Substitute.For<IXmlManager>();
            HomeController controller = new HomeController(unityOfWork, xmlManager);

            //Act
            PartialViewResult result = controller.InformationPartial() as PartialViewResult;
            string viewName = result.ViewName;
            List<InformationItem> model = result.Model as List<InformationItem>;

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual("_InformationPartial", viewName);
            Assert.NotNull(model);
            Assert.AreEqual(3, model.Count);
        }

        [Test]
        public void EventPartial()
        {
            //Arrange
            DbContext context = Substitute.For<DbContext>();
            HomeUnitOfWork unityOfWork = Substitute.For<HomeUnitOfWork>(context);
            unityOfWork.EventRepository = Substitute.For<IGetRepository<Event>>();
            unityOfWork.EventRepository.All().Returns(new List<Event>()
            {
                new Event() { AddedDate=DateTime.Now,Title="Wydarzenie 1", Content="Treść wydarzenia 1" },
                new Event() { AddedDate=DateTime.Now,Title="Wydarzenie 2", Content="Treść wydarzenia 2" },
                new Event() { AddedDate=DateTime.Now,Title="Wydarzenie 3", Content="Treść wydarzenia 3" }
            });
            IXmlManager xmlManager = Substitute.For<IXmlManager>();
            HomeController controller = new HomeController(unityOfWork, xmlManager);

            //Act
            PartialViewResult result = controller.EventPartial() as PartialViewResult;
            string viewName = result.ViewName;
            List<Event> model = result.Model as List<Event>;

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual("_EventPartial", viewName);
            Assert.NotNull(model);
            Assert.AreEqual(3, model.Count);
        }

        [Test]
        public void NewPartial()
        {
            //Act
            DbContext context = Substitute.For<DbContext>();
            HomeUnitOfWork unityOfWork = Substitute.For<HomeUnitOfWork>(context);
            unityOfWork.NewRepository = Substitute.For<IGetRepository<New>>();
            unityOfWork.NewRepository.All().Returns(new List<New>()
            {
                new New() { AddedDate=DateTime.Now, Position=1, Title="New 1",Content="Quisque nulla nunc, tempor eu lorem non, pharetra laoreet massa." },
                new New() { AddedDate=DateTime.Now, Position=1, Title="New 1",Content="Nunc iaculis, elit eu aliquam placerat, diam est feugiat urna, et lacinia tellus lectus a sem. " },
                new New() { AddedDate=DateTime.Now, Position=1, Title="New 1",Content="In imperdiet tellus ex, sed efficitur odio dignissim eget." }
            });
            IXmlManager xmlManager = Substitute.For<IXmlManager>();
            HomeController controller = new HomeController(unityOfWork, xmlManager);

            //Act
            PartialViewResult result = controller.NewPartial() as PartialViewResult;
            string viewName = result.ViewName;
            List<New> model = result.Model as List<New>;

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual("_NewPartial", viewName);
            Assert.NotNull(model);
            Assert.AreEqual(3, model.Count);
        }

        [Test]
        public void ShopContactPartial()
        {
            DbContext context = Substitute.For<DbContext>();
            HomeUnitOfWork unityOfWork = Substitute.For<HomeUnitOfWork>(context);
            IXmlManager xmlManager = Substitute.For<IXmlManager>();
            xmlManager.GetXmlModel<ShopContact>("ShopContact").Returns(new ShopContact()
            {
                Address = new Address() { DeliveryContact = "111 111 111", Email = "fakeEmail@gmail.com", InformationContact = "000 000 000" },
                Contact = new Contact() { City = "FakeCisty", PostalCode = "22-333", Street = "ul. fake 111" }
            });
            HomeController controller = new HomeController(unityOfWork, xmlManager);

            //Act
            PartialViewResult result = controller.ShopContactPartial() as PartialViewResult;
            string viewName = result.ViewName;
            ShopContact model = result.Model as ShopContact;

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual("_ShopContactPartial", viewName);
            Assert.NotNull(model);
        }

        [Test]
        public void OpeningHoursPartial()
        {
            DbContext context = Substitute.For<DbContext>();
            HomeUnitOfWork unityOfWork = Substitute.For<HomeUnitOfWork>(context);
            IXmlManager xmlManager = Substitute.For<IXmlManager>();
            xmlManager.GetXmlModel<OpeningHours>("OpeningHours").Returns(new OpeningHours()
            {
                WorksDays = new List<Days>()
                {
                    new Days() { Name="Pon-Czw",WorkHours="12.00 - 22.00" },
                    new Days() { Name="Pt-Sob",WorkHours="12.00 - 22.00" },
                    new Days() { Name="Niedz",WorkHours="12.00 - 22.00" }
                }
            });
            HomeController controller = new HomeController(unityOfWork, xmlManager);

            //Act
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
            DbContext context = Substitute.For<DbContext>();
            HomeUnitOfWork unityOfWork = Substitute.For<HomeUnitOfWork>(context);
            IXmlManager xmlManager = Substitute.For<IXmlManager>();
            xmlManager.GetXmlModel<ShopContact>("ShopContact").Returns(new ShopContact()
            {
                Address = new Address() { DeliveryContact = "111 111 111", Email = "fakeEmail@gmail.com", InformationContact = "000 000 000" },
                Contact = new Contact() { City = "FakeCisty", PostalCode = "22-333", Street = "ul. fake 111" }
            });
            HomeController controller = new HomeController(unityOfWork, xmlManager);

            //Act
            ContentResult result = controller.DeliveryContact() as ContentResult;
            
            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("111 111 111", result.Content);
        }
    }
}
