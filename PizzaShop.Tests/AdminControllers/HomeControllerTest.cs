using NSubstitute;
using NUnit.Framework;
using PizzaShop.Areas.Admin.Controllers;
using PizzaShop.Areas.Admin.Models.ViewModels;
using PizzaShop.Models.PizzaShopModels.Entities;
using PizzaShop.Services.shop.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PizzaShop.Tests.AdminControllers
{
    [TestFixture]
    public class HomeControllerTest
    {
        [Test]
        public void Index()
        {
            // Arrange
            var drinks = new List<Drink>()
            {
                new Drink() { Name="Mirinda",Price=5.00M,Capacity=0.5f },
                new Drink() { Name="7Up",Price=5.00M,Capacity=0.5f },
                new Drink() { Name="Woda gazowana",Price=3.00M,Capacity=0.5f }
            };
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
            var pizzas = new List<Pizza>()
            {
                new Pizza { Name="Margarita",Components=new List<Component>() { components[0] }  },
                new Pizza { Name="Salame",Components=new List<Component>() { components[0], components[1] }  },
                new Pizza { Name="Pollo",Components=new List<Component>() { components[0],components[2] }  }
            };
            var salads = new List<Salad>()
            {
                new Salad() { Name="Greco",Price=14.00M },
                new Salad() { Name="Pollo",Price=16.00M },
                new Salad() { Name="Mexico",Price=16.00M }
            };
            var sauces = new List<Sauce>()
            {
                new Sauce() { Name="Ostry",Price=2.00M },
                new Sauce() { Name="Czosnkowo-ziołowy",Price=2.00M },
                new Sauce() { Name="Pomodorowy",Price=3.00M }
            };
            var viewModel = new MenuCardViewModel()
            {
                Drinks = drinks,
                Pizzas = pizzas,
                Salads = salads,
                Sauces = sauces
            };
            var menuService = Substitute.For<IMenuCardService>();
            var controller = new HomeController(menuService);

            menuService.GetAllDrinks().Returns(drinks);
            menuService.GetAllPizzas().Returns(pizzas);
            menuService.GetAllSalads().Returns(salads);
            menuService.GetAllSauces().Returns(sauces);

            // Act
            var result = controller.Index() as ViewResult;
            var viewName = result.ViewName;
            var model = result.Model as MenuCardViewModel;

            // Assert
            Assert.That(result, !Is.Null);
            Assert.That("Index", Is.EqualTo(viewName));
            Assert.That(model, !Is.Null);
        }

        [Test]
        public void LoadingPartial()
        {
            // Arrange
            var menuCartService = Substitute.For<IMenuCardService>();
            var controller = new HomeController(menuCartService);

            // Act
            var result = controller.LoadingPartial() as PartialViewResult;
            var viewName = result.ViewName;

            // Assert
            Assert.That(result, !Is.Null);
            Assert.That("_LoadingPartial", Is.EqualTo(viewName));
        }
    }
}
