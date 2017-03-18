using PizzaShop.CustomModelBinders;
using PizzaShop.Mappers;
using PizzaShop.Models.PizzaShopModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace PizzaShop
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //inititial databases
            Database.SetInitializer<PizzaShopDbContext>(new PizzaShopSampleData());
            Database.SetInitializer<CmsDbContext>(new CmsSampleData());
            //mapowanie objektów
            AutoMapperConfig.RegisterMappings();
            //własny modelbinder służący do wlasnej walidacji pola decimal z ceną
            //należy jescze obsłużyc javascipt
            ModelBinders.Binders.Add(typeof(decimal), new DecimalModelBilder());
            ModelBinders.Binders.Add(typeof(decimal?), new DecimalModelBilder());
        }
    }
}
