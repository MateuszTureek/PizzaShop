using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PizzaShop.Services.Image.Interfaces
{
    public interface IImageService
    {
        string SaveChosenImage(HttpPostedFileBase pictureContent);
    }
}
