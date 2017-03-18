using AutoMapper;
using PizzaShop.Areas.Admin.Models.ViewModels;
using PizzaShop.Models.PizzaShopModels.CMS;
using PizzaShop.Models.PizzaShopModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PizzaShop.Mappers
{
    public class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            Mapper.Initialize(i =>
            {
                i.CreateMap<MenuItem, MenuItemViewModel>();
                i.CreateMap<MenuItemViewModel, MenuItem>();
                i.CreateMap<SliderItem, SliderItemViewModel>();
                i.CreateMap<SliderItemViewModel, SliderItem>();
                i.CreateMap<InformationItemViewModel, InformationItem>();
                i.CreateMap<InformationItem, InformationItemViewModel>();
                i.CreateMap<EventViewModel, Event>();
                i.CreateMap<Event, EventViewModel>();
                i.CreateMap<NewViewModel, News>();
                i.CreateMap<News, NewViewModel>();
                i.CreateMap<Sauce, SauceViewModel>();
                i.CreateMap<SauceViewModel, Sauce>();
                i.CreateMap<Salad, SaladViewModel>();
                i.CreateMap<SaladViewModel, Salad>();
                i.CreateMap<Drink, DrinkViewModel>();
                i.CreateMap<DrinkViewModel, Drink>();
            });
        }
    }
}