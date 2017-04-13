using PizzaShop.Models.PizzaShopModels.CMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaShop.Services.Cms.Interfaces
{
    public interface IHomePresentationService
    {
        List<GalleryItem> GetAllGalleryItems();
        List<MenuItem> GetAllMenuItems();
        List<SliderItem> GetAllSliderItems();
        List<InformationItem> GetAllInformationItems();
        List<Event> GetAllEvents();
        List<News> GetAllNews();
    }
}
