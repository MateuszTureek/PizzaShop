using PizzaShop.Repositories.CMS.Interfaces;
using PizzaShop.Services.Cms.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using PizzaShop.Models.PizzaShopModels.CMS;

namespace PizzaShop.Services.Cms.Classes
{
    public class HomePresentationService : IHomePresentationService
    {
        readonly IGalleryItemRepository _galleryItemRepository;
        readonly IMenuItemRepository _menuItemRepository;
        readonly ISliderItemRepository _sliderItemRepository;
        readonly IInformationItemRepository _informationRepository;
        readonly IEventRepository _eventRepository;
        readonly INewsRepository _newsRepository;

        public HomePresentationService(IGalleryItemRepository galleryItemRepository,
                                       IMenuItemRepository menuItemRepository,
                                       ISliderItemRepository sliderItemRepository,
                                       IInformationItemRepository informationItemRepository,
                                       IEventRepository eventRepository,
                                       INewsRepository newsRepository)
        {
            _galleryItemRepository = galleryItemRepository;
            _menuItemRepository = menuItemRepository;
            _sliderItemRepository = sliderItemRepository;
            _informationRepository = informationItemRepository;
            _eventRepository = eventRepository;
            _newsRepository = newsRepository;
        }

        public List<GalleryItem> GetAllGalleryItems()
        {
            var result = _galleryItemRepository.GetByPosition().ToList();
            return result;
        }

        public List<MenuItem> GetAllMenuItems()
        {
            var result = _menuItemRepository.GetByPosition().ToList();
            return result;
        }

        public List<SliderItem> GetAllSliderItems()
        {
            var result = _sliderItemRepository.GetByPosition().ToList();
            return result;
        }

        public List<InformationItem> GetAllInformationItems()
        {
            var result = _informationRepository.GetByPosition().ToList();
            return result;
        }

        public List<Event> GetAllEvents()
        {
            var result = _eventRepository.GetByAddedDate().ToList();
            return result;
        }

        public List<News> GetAllNews()
        {
            var result = _newsRepository.GetByAddedDate().ToList();
            return result;
        }
    }
}