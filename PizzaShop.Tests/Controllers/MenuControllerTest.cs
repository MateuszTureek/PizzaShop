using System;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;
using NSubstitute;
using PizzaShop.Models.PizzaShopModels.Entities;
using PizzaShop.Controllers;
using PizzaShop.Services.shop.Interfaces;
using System.Web.Mvc;

namespace PizzaShop.Tests.Controllers
{
    [TestFixture]
    public class MenuControllerTest
    {
        [Test]
        public void Pizza()
        {
            //Arrange
            var components = new List<Component>()
            {
                new Component() { Name="Ser" },
                new Component() { Name="Salami pepperoni" },
                new Component() { Name="Kurczak" },
                new Component() { Name="Szynka" },
                new Component() { Name="Pieczarki" },
                new Component() { Name="Ananas" },
                new Component() { Name="Papryka" }
            };
            var pizzas = new List<Pizza>()
            {
                new Pizza { Name="Margarita",Components=new List<Component>() { components[0] }  },
                new Pizza { Name="Salame",Components=new List<Component>() { components[0], components[1] }  },
                new Pizza { Name="Pollo",Components=new List<Component>() { components[0],components[2] }  }
            };
            var menuCart = Substitute.For<IMenuCardService>();
            MenuController controller = new MenuController(menuCart);

            //Act
            menuCart.GetAllPizzas().Returns(pizzas);
            var result = controller.Pizza() as ViewResult;
            var viewName = result.ViewName;
            var model = result.Model;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Pizza", viewName);
            Assert.IsNotNull(model);
        }

        [Test]
        public void Salad()
        {
            //Arrange
            var components = new List<Component>()
            {
                new Component() { Name="Pomidory" },
                new Component() { Name="Ogórki" },
                new Component() { Name="Sałata" },
                new Component() { Name="Marchew" }
            };
            var salads = new List<Salad>()
            {
                new Salad() { Name="Greco",Price=14.00M },
                new Salad() { Name="Pollo",Price=16.00M },
                new Salad() { Name="Mexico",Price=16.00M }
            };
            var menuCart = Substitute.For<IMenuCardService>();
            MenuController controller = new MenuController(menuCart);

            //Act
            menuCart.GetAllSalads().Returns(salads);
            var result = controller.Salad() as ViewResult;
            var viewName = result.ViewName;
            var model = result.Model as List<Salad>;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Salad", viewName);
            Assert.IsNotNull(model);
        }

        [Test]
        public void Sauce()
        {
            var sauces = new List<Sauce>()
            {
                new Sauce() { Name="Ostry",Price=2.00M },
                new Sauce() { Name="Czosnkowo-ziołowy",Price=2.00M },
                new Sauce() { Name="Pomodorowy",Price=3.00M }
            };
            var menuCart = Substitute.For<IMenuCardService>();
            MenuController controller = new MenuController(menuCart);

            //Act
            menuCart.GetAllSauces().Returns(sauces);
            var result = controller.Sauce() as ViewResult;
            var viewName = result.ViewName;
            var model = result.Model as List<Sauce>;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Sauce", viewName);
            Assert.IsNotNull(model);
        }

        [Test]
        public void Drink()
        {
            var drinks = new List<Drink>()
            {
                new Drink() { Name="Mirinda",Price=5.00M,Capacity=0.5f },
                new Drink() { Name="7Up",Price=5.00M,Capacity=0.5f },
                new Drink() { Name="Woda gazowana",Price=3.00M,Capacity=0.5f },
                new Drink() { Name="Woda niegazowana",Price=3.00M,Capacity=0.5f },
                new Drink() { Name="Sok pomarańczowy",Price=4.00M, Capacity=0.33f }
            };
            var menuCart = Substitute.For<IMenuCardService>();
            MenuController controller = new MenuController(menuCart);

            //Act
            menuCart.GetAllDrinks().Returns(drinks);
            var result = controller.Drink() as ViewResult;
            var viewName = result.ViewName;
            var model = result.Model as List<Drink>;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Drink", viewName);
            Assert.IsNotNull(model);
        }
    }
}
