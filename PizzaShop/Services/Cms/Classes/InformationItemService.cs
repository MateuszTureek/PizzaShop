using PizzaShop.Services.Cms.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PizzaShop.Models.PizzaShopModels.CMS;
using PizzaShop.Repositories.CMS.Interfaces;
using PizzaShop.UnitOfWork;
using Ninject;
using PizzaShop.Services.Image.Interfaces;

namespace PizzaShop.Services.Cms.Classes
{
    public class InformationItemService : IInformationItemService
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IInformationItemRepository _repository;
        readonly IImageService _imageService;

        public InformationItemService([Named("cmsUnit")]IUnitOfWork unitOfWork, IInformationItemRepository repository, IImageService imageService)
        {
            _repository = repository;
            _imageService = imageService;
            _unitOfWork = unitOfWork;
        }

        public void DeleteInfomationItem(InformationItem informationItem)
        {
            _repository.Delete(informationItem);
        }

        public List<InformationItem> GetAllInformationItems()
        {
            var result = _repository.GetAll().ToList();
            return result;
        }

        public InformationItem GetInfomrationItem(int id)
        {
            var result = _repository.Get(id);
            return result;
        }

        public string AddInformationItemImage(HttpPostedFileBase contentImage)
        {
            var virtualPath=_imageService.SaveChosenImage(contentImage);
            return virtualPath;
        }

        public void SaveInfomrationItem()
        {
            _unitOfWork.Commit();
        }

        public void CreateInformationItem(InformationItem infomationItem)
        {
            _repository.Insert(infomationItem);
        }

        public void UpdateInformationItem(InformationItem informationItem)
        {
            _repository.Update(informationItem);
        }
    }
}