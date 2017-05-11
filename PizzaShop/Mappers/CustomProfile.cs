using AutoMapper;
using Microsoft.AspNet.Identity.EntityFramework;
using Ninject;
using PizzaShop.Areas.Admin.Models.ViewModels;
using PizzaShop.Models;
using PizzaShop.Models.PizzaShopModels.CMS;
using PizzaShop.Models.PizzaShopModels.Entities;
using PizzaShop.Services.Xml.XmlModels;
using System.Collections.Generic;

namespace PizzaShop.Mappers
{
    public class CustomProfile : Profile
    {
        public CustomProfile() : base(nameof(CustomProfile)) { Configure(); }

        protected void Configure()
        {
            CreateMap<MenuItem, MenuItemViewModel>();
            CreateMap<MenuItemViewModel, MenuItem>();
            CreateMap<SliderItem, SliderItemViewModel>();
            CreateMap<SliderItemViewModel, SliderItem>();
            CreateMap<InformationItemViewModel, InformationItem>();
            CreateMap<InformationItem, InformationItemViewModel>();
            CreateMap<EventViewModel, Event>();
            CreateMap<Event, EventViewModel>();
            CreateMap<NewsViewModel, News>();
            CreateMap<News, NewsViewModel>();
            CreateMap<Sauce, SauceViewModel>();
            CreateMap<SauceViewModel, Sauce>();
            CreateMap<Salad, SaladViewModel>();
            CreateMap<SaladViewModel, Salad>();
            CreateMap<Drink, DrinkViewModel>();
            CreateMap<DrinkViewModel, Drink>();
            CreateMap<ComponentViewModel, Component>().ForMember(d => d.Pizzas, s => s.Ignore());
            CreateMap<Component, ComponentViewModel>();
            /* ================================== */
            CreateMap<ShopContactViewModel, ShopContact>().
                ForMember<Address>(dest => dest.Address, input => input.MapFrom(s => new Address()
                {
                    DeliveryContact = s.DeliveryContact,
                    Email = s.Email,
                    InformationContact = s.InformationContact
                })).
                ForMember<Contact>(dest => dest.Contact, input => input.MapFrom(s => new Contact()
                {
                    City = s.City,
                    PostalCode = s.PostalCode,
                    Street = s.Street
                })
            );
            /* ================================== */
            CreateMap<ShopContact, ShopContactViewModel>().
                ForMember<string>(d => d.DeliveryContact, i => i.MapFrom(s => s.Address.DeliveryContact)).
                ForMember<string>(d => d.Email, i => i.MapFrom(s => s.Address.Email)).
                ForMember<string>(d => d.InformationContact, i => i.MapFrom(s => s.Address.InformationContact)).
                ForMember<string>(d => d.City, i => i.MapFrom(s => s.Contact.City)).
                ForMember<string>(d => d.PostalCode, i => i.MapFrom(s => s.Contact.PostalCode)).
                ForMember<string>(d => d.Street, i => i.MapFrom(s => s.Contact.Street));
            /* ================================== */
            CreateMap<IdentityRole, RoleViewModel>();
            CreateMap<ApplicationUser, UserViewModel>().ForMember(d => d.Roles, s => s.Ignore());
        }
    }
}