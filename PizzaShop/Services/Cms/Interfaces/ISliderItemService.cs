using PizzaShop.Areas.Admin.Models.ViewModels;
using PizzaShop.Models.PizzaShopModels.CMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PizzaShop.Services.Cms.Interfaces
{
    public interface ISliderItemService
    {
        List<SliderItem> SliderItemList();
        SliderItem GetSliderItem(int id);
        void CreateSliderItem(SliderItem sliderItem);
        void DeleteSliderItem(SliderItem sliderItem);
        void UpdateSliderItem(SliderItem sliderItem);
        string AddSliderItemImage(HttpPostedFileBase contentImage);
        void SaveSliderItem();
        SliderItem MapViewModelToModel(SliderItemViewModel viewModel);
        SliderItemViewModel MapModelToViewModel(SliderItem model);
    }
}
