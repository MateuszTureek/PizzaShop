using PizzaShop.Services.Image.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace PizzaShop.Services.Image.Classes
{
    public class ImageService : IImageService
    {
        string _virtualPath = "/Content/Images";
        string _physicalPath;

        public ImageService()
        {
            _physicalPath = HostingEnvironment.MapPath(_virtualPath);
        }

        public string SaveChosenImage(HttpPostedFileBase pictureContent)
        {
            string fileName = Path.GetFileName(pictureContent.FileName);
            using (var bReader = new BinaryReader(pictureContent.InputStream))
            {
                var binaryImg = bReader.ReadBytes(pictureContent.ContentLength);
                using (var bWriter = new BinaryWriter(new FileStream(_physicalPath + "\\" + fileName, FileMode.Create)))
                {
                    bWriter.Write(binaryImg);
                }
            }
            return _virtualPath + "/" + fileName;
        }
    }
}