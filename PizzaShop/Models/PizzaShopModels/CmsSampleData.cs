using PizzaShop.Models.PizzaShopModels.CMS;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PizzaShop.Models.PizzaShopModels
{
    public class CmsSampleData : DropCreateDatabaseIfModelChanges<CmsDbContext>
    {
        private void AddToContext<T>(List<T> items, CmsDbContext context) where T : class
        {
            DbSet<T> dbSet = context.Set<T>();
            foreach (var i in items)
                dbSet.Add(i);
        }

        protected override void Seed(CmsDbContext context)
        {
            List<MenuItem> menuItems = new List<MenuItem>()
            {
                new MenuItem() { Position=1, Title="Strona główna", ActionName="Index" },
                new MenuItem() { Position=2, Title="Menu", ActionName="SiteMenu" },
                new MenuItem() { Position=3, Title="Galeria", ActionName="Gallery" },
                new MenuItem() { Position=4, Title="Kontakt", ActionName="Contact" }
            };
            AddToContext<MenuItem>(menuItems, context);
            List<Event> events = new List<Event>()
            {
                new Event() { AddedDate=DateTime.Now,Title="Wydarzenie 1", Content="Treść wydarzenia 1" },
                new Event() { AddedDate=DateTime.Now,Title="Wydarzenie 2", Content="Treść wydarzenia 2" },
                new Event() { AddedDate=DateTime.Now,Title="Wydarzenie 3", Content="Treść wydarzenia 3" }
            };
            AddToContext<Event>(events, context);
            List<InformationItem> informationItems = new List<InformationItem>()
            {
                new InformationItem() { PictureUrl="/Content/Images/pizza_1.jpg", Title="Menu",Content="Ut at viverra arcu. Donec sed interdum nulla, eu porttitor lorem. Morbi efficitur sapien ullamcorper lorem vestibulum, ut aliquet turpis consequat. Suspendisse ac nibh non velit scelerisque bibendum. " },
                new InformationItem() { PictureUrl="/Content/Images/pizza_2.jpg", Title="Promocja", Content="Praesent in placerat risus, et ornare lectus. Praesent turpis tortor, consectetur quis enim id, consequat pharetra velit. Nullam tempor convallis ante at finibus. " },
                new InformationItem() { PictureUrl="/Content/Images/pizza_3.jpg", Title="O nas", Content="Suspendisse sed enim porttitor, auctor urna et, bibendum enim. In imperdiet tellus ex, sed efficitur odio dignissim eget. Ut fermentum ipsum eget lorem ornare condimentum. " }
            };
            AddToContext<InformationItem>(informationItems, context);
            List<SliderItem> sliderItems = new List<SliderItem>()
            {
                new SliderItem() { Position=1,ShortDescription="Slider description 1",PictureUrl="/Content/Images/pizzaSlide_1.jpg" },
                new SliderItem() { Position=2,ShortDescription="Slider description 2",PictureUrl="/Content/Images/pizzaSlide_2.jpg" },
                new SliderItem() { Position=3,ShortDescription="Slider description 3",PictureUrl="/Content/Images/pizzaSlide_1.jpg" }
            };
            AddToContext<SliderItem>(sliderItems, context);
            List<New> news = new List<New>()
            {
                new New() { AddedDate=DateTime.Now, Position=1, Title="New 1",Content="Quisque nulla nunc, tempor eu lorem non, pharetra laoreet massa." },
                new New() { AddedDate=DateTime.Now, Position=1, Title="New 1",Content="Nunc iaculis, elit eu aliquam placerat, diam est feugiat urna, et lacinia tellus lectus a sem. " },
                new New() { AddedDate=DateTime.Now, Position=1, Title="New 1",Content="In imperdiet tellus ex, sed efficitur odio dignissim eget." }
            };
            AddToContext<New>(news, context);
            List<GalleryItem> galleryItems = new List<GalleryItem>()
            {
                new GalleryItem() { Position=1, PictureUrl="/Content/Images/pizza_1.jpg" },
                new GalleryItem() { Position=2, PictureUrl="/Content/Images/pizza_2.jpg" },
                new GalleryItem() { Position=3, PictureUrl="/Content/Images/pizza_3.jpeg" },
                new GalleryItem() { Position=4, PictureUrl="/Content/Images/pizzaSlide_1.jpg" },
                new GalleryItem() { Position=5, PictureUrl="/Content/Images/pizzaSlide_2.jpg" },
                new GalleryItem() { Position=6, PictureUrl="/Content/Images/pizzaSlide_3.jpg" }
            };
            AddToContext<GalleryItem>(galleryItems, context);

            base.Seed(context);
        }
    }
}