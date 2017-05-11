using PizzaShop.Models.PizzaShopModels.CMS;
using PizzaShop.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaShop.Repositories.CMS.Interfaces
{
    public interface IInformationItemRepository : IRepository<int?, InformationItem>
    {
        List<InformationItem> GetByPosition();
    }
}
