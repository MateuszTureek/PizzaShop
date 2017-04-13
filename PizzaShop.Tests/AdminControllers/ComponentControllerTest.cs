using System;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;
using NSubstitute;
using PizzaShop.Repositories.Shop.Interfaces;
using PizzaShop.Areas.Admin.Controllers;
using PizzaShop.Models.PizzaShopModels.Entities;
using System.Web.Mvc;

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
            ComponentController controller = new ComponentController(componentRepository);

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
    }
}
