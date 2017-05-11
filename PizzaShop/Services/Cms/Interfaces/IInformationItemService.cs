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
    public interface IInformationItemService
    {
        void CreateInformationItem(InformationItem infomationItem);
        void UpdateInformationItem(InformationItem informationItem);
        List<InformationItem> GetAllInformationItems();
        void DeleteInfomationItem(InformationItem informationItem);
        InformationItem GetInfomrationItem(int id);
        string AddInformationItemImage(HttpPostedFileBase contentImage);
        void SaveInfomrationItem();
        InformationItem MapViewModelToObject(InformationItemViewModel viewModel);
        InformationItemViewModel MapObjectToViewModel(InformationItem model);
        InformationItem MapViewModelToObject(InformationItemViewModel viewModel, InformationItem model);
    }
}
