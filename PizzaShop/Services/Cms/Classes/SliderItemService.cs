using Ninject;
using PizzaShop.Repositories.CMS.Interfaces;
using PizzaShop.Services.Cms.Interfaces;
using PizzaShop.Services.Image.Interfaces;
using PizzaShop.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PizzaShop.Models.PizzaShopModels.CMS;
using AutoMapper;
using PizzaShop.Areas.Admin.Models.ViewModels;

namespace PizzaShop.Services.Cms.Classes
{
    public class SliderItemService : ISliderItemService
    {
        readonly IUnitOfWork _unitOfWork;
        readonly ISliderItemRepository _repository;
        readonly IImageService _imageService;
        readonly IMapper _mapper;

        public SliderItemService([Named("cmsUnit")]IUnitOfWork unitOfWork, ISliderItemRepository repository,
                                                   IImageService imageService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _imageService = imageService;
            _mapper = mapper;
        }

        public List<SliderItem> SliderItemList()
        {
            var result = _repository.GetAll().ToList();
            return result;
        }

        public SliderItem GetSliderItem(int id)
        {
            var result = _repository.Get(id);
            return result;
        }

        public void CreateSliderItem(SliderItem sliderItem)
        {
            _repository.Insert(sliderItem);
        }

        public void DeleteSliderItem(SliderItem sliderItem)
        {
            _repository.Delete(sliderItem);
        }

        public void UpdateSliderItem(SliderItem sliderItem)
        {
            _repository.Update(sliderItem);
        }

        public string AddSliderItemImage(HttpPostedFileBase contentImage)
        {
            var virtualPath = _imageService.SaveChosenImage(contentImage);
            return virtualPath;
        }

        public void SaveSliderItem()
        {
            _unitOfWork.Commit();
        }


        public SliderItem MapViewModelToModel(SliderItemViewModel viewModel)
        {
            var model = _mapper.Map<SliderItemViewModel, SliderItem>(viewModel);
            return model;
        }

        public SliderItem MapViewModelToModel(SliderItemViewModel viewModel, SliderItem sliderItem)
        {
            var model = _mapper.Map<SliderItemViewModel, SliderItem>(viewModel, sliderItem);
            return model;
        }

        public SliderItemViewModel MapModelToViewModel(SliderItem model)
        {
            var viewModel = _mapper.Map<SliderItem, SliderItemViewModel>(model);
            return viewModel;
        }
    }
}