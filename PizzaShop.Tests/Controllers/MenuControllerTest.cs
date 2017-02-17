using NSubstitute;
using NUnit.Framework;
using PizzaShop.Controllers;
using PizzaShop.Models.PizzaShopModels.Entities;
using PizzaShop.Repositories;
using PizzaShop.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PizzaShop.Tests.Controllers
{
    [TestFixture]
    class MenuControllerTest
    {
        [Test]
        public void Pizza()
        {
            // Arrange
            DbContext context = Substitute.For<DbContext>();
            MenuUnitOfWork unitOfWork = Substitute.For<MenuUnitOfWork>(context);
            unitOfWork.PizzaRepository = Substitute.For<IGetRepository<Pizza>>();
            unitOfWork.PizzaRepository.All().Returns(new List<Pizza>()
            {
                new Pizza { Name="Margarita",Components=new List<Component>() { new Component() { Name="Ser" } }  },
                new Pizza { Name="Salame",Components=new List<Component>() { new Component() { Name="Ser" },new Component() { Name="Salami" } }  },
                new Pizza { Name="Pollo",Components=new List<Component>() { new Component() { Name="Ser" }, new Component() { Name="Kurczak" } } }
            });
            MenuController controller = new MenuController(unitOfWork);

            //Act
            var result = controller.Pizza() as ViewResult;
            var viewName = result.ViewName;
            var model = result.Model as List<Pizza>;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Pizza", viewName);
            Assert.IsNotNull(model);
            Assert.AreEqual(3, model.Count);
        }

        [Test]
        public void Salad()
        {
            // Arrange
            DbContext context = Substitute.For<DbContext>();
            MenuUnitOfWork unitOfWork = Substitute.For<MenuUnitOfWork>(context);
            unitOfWork.SaladRepository = Substitute.For<IGetRepository<Salad>>();
            unitOfWork.SaladRepository.All().Returns(new List<Salad>()
            {
                new Salad() { Name="Greco",Price=14.00M,Components=new List<Component>() { } },
                new Salad() { Name="Pollo",Price=16.00M, Components=new List<Component>() { } },
                new Salad() { Name="Mexico",Price=16.00M,Components=new List<Component>() { } }
            });
            MenuController controller = new MenuController(unitOfWork);

            //Act
            var result = controller.Salad() as ViewResult;
            var viewName = result.ViewName;
            var model = result.Model as List<Salad>;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Salad", viewName);
            Assert.IsNotNull(model);
            Assert.AreEqual(3, model.Count);
        }

        [Test]
        public void Sauce()
        {
            // Arrange
            DbContext context = Substitute.For<DbContext>();
            MenuUnitOfWork unitOfWork = Substitute.For<MenuUnitOfWork>(context);
            unitOfWork.SauceRepository = Substitute.For<IGetRepository<Sauce>>();
            unitOfWork.SauceRepository.All().Returns(new List<Sauce>()
            {
                new Sauce() { Name="Ostry",Price=2.00M },
                new Sauce() { Name="Czosnkowo-ziołowy",Price=2.00M },
                new Sauce() { Name="Pomodorowy",Price=3.00M }
            });
            MenuController controller = new MenuController(unitOfWork);

            //Act
            var result = controller.Sauce() as ViewResult;
            var viewName = result.ViewName;
            var model = result.Model as List<Sauce>;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Sauce", viewName);
            Assert.IsNotNull(model);
            Assert.AreEqual(3, model.Count);
        }

        [Test]
        public void Drink()
        {
            // Arrange
            DbContext context = Substitute.For<DbContext>();
            MenuUnitOfWork unitOfWork = Substitute.For<MenuUnitOfWork>(context);
            unitOfWork.DrinkRepository = Substitute.For<IGetRepository<Drink>>();
            unitOfWork.DrinkRepository.All().Returns(new List<Drink>()
            {
                new Drink() { Name="Mirinda",Price=5.00M,Capacity=0.5f },
                new Drink() { Name="7Up",Price=5.00M,Capacity=0.5f },
                new Drink() { Name="Woda gazowana",Price=3.00M,Capacity=0.5f },
                new Drink() { Name="Woda niegazowana",Price=3.00M,Capacity=0.5f },
                new Drink() { Name="Sok pomarańczowy",Price=4.00M, Capacity=0.33f }
            });
            MenuController controller = new MenuController(unitOfWork);

            //Act
            var result = controller.Drink() as ViewResult;
            var viewName = result.ViewName;
            var model = result.Model as List<Drink>;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Drink", viewName);
            Assert.IsNotNull(model);
            Assert.AreEqual(5, model.Count);
        }
    }
}
