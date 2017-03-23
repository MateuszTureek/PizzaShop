[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(PizzaShop.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(PizzaShop.App_Start.NinjectWebCommon), "Stop")]

namespace PizzaShop.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using System.Data.Entity;
    using Models.PizzaShopModels;
    using Repositories.CMS.Interfaces;
    using Repositories.CMS.Classes;
    using Services.Cms.Interfaces;
    using Services.Cms.Classes;
    using Repositories.Shop.Interfaces;
    using Repositories.Shop.Classes;
    using UnitOfWork;
    using Services.shop.Classes;
    using Services.shop.Interfaces;
    using XML.XmlServices;
    using XML.Services.XmlServices;
    using Microsoft.AspNet.Identity;
    using Models;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin.Security;
    using Microsoft.AspNet.Identity.EntityFramework;

    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            //XML
            kernel.Bind<IXmlManager>().To<XmlManager>();
            //CONTEXTS
            kernel.Bind(typeof(DbContext)).To(typeof(CmsDbContext)).Named("cms");
            kernel.Bind(typeof(DbContext)).To(typeof(PizzaShopDbContext)).Named("shop");
            kernel.Bind(typeof(DbContext)).To(typeof(ApplicationDbContext)).Named("identity");
            var cmsDbContext = kernel.Get<DbContext>("cms");
            var pizzaShopDbContext = kernel.Get<DbContext>("shop");
            var identity = kernel.Get<DbContext>("identity");
            //UNIT OF WORK
            kernel.Bind<IUnitOfWork>().To<UnitOfWork>().Named("cmsUnit").WithConstructorArgument(cmsDbContext);
            kernel.Bind<IUnitOfWork>().To<UnitOfWork>().Named("shopUnit").WithConstructorArgument(pizzaShopDbContext);
            //SERVICES
            kernel.Bind<IHomePresentationService>().To<HomePresentationService>().WithConstructorArgument(cmsDbContext);
            kernel.Bind<IMenuCardService>().To<MenuCardService>();
            kernel.Bind<IPizzaService>().To<PizzaService>();
            //REPOSITORIES
            //cms
            kernel.Bind<ISliderItemRepository>().To<SliderItemRepository>().WithConstructorArgument(cmsDbContext);
            kernel.Bind<IGalleryItemRepository>().To<GalleryItemRepository>().WithConstructorArgument(cmsDbContext);
            kernel.Bind<IMenuItemRepository>().To<MenuItemRepository>().WithConstructorArgument(cmsDbContext);
            kernel.Bind<IInformationItemRepository>().To<InformationItemRepository>().WithConstructorArgument(cmsDbContext);
            kernel.Bind<INewsRepository>().To<NewsRepository>().WithConstructorArgument(cmsDbContext);
            kernel.Bind<IEventRepository>().To<EventRepository>().WithConstructorArgument(cmsDbContext);
            //shop
            kernel.Bind<IDrinkRepository>().To<DrinkRepository>().WithConstructorArgument(pizzaShopDbContext);
            kernel.Bind<IPizzaRepository>().To<PizzaRepository>().WithConstructorArgument(pizzaShopDbContext);
            kernel.Bind<ISaladRepository>().To<SaladRepository>().WithConstructorArgument(pizzaShopDbContext);
            kernel.Bind<ISauceRepository>().To<SauceRepository>().WithConstructorArgument(pizzaShopDbContext);
            kernel.Bind<IPizzaSizeRepository>().To<PizzaSizeRepository>().WithConstructorArgument(pizzaShopDbContext);
            kernel.Bind<IPizzaSizePriceRepository>().To<PizzaSizePriceRepository>().WithConstructorArgument(pizzaShopDbContext);
            kernel.Bind<IComponentRepository>().To<ComponentRepository>().WithConstructorArgument(pizzaShopDbContext);

            kernel.Bind<RoleStore<IdentityRole>>().ToSelf().WithConstructorArgument(identity);
        }
    }
}
