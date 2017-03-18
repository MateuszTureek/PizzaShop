using PizzaShop.Repositories.CMS.Interfaces;
using PizzaShop.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using PizzaShop.Models.PizzaShopModels.CMS;

namespace PizzaShop.Repositories.CMS.Classes
{
    public class GalleryItemRepository : Repository<int, GalleryItem>, IGalleryItemRepository
    {
        public GalleryItemRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public List<GalleryItem> GetByPosition()
        {
            var result = _dbSet.OrderBy(o => o.Position).ToList();
            return result;
        }
    }
}