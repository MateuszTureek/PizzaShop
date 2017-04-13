using PizzaShop.Services.Xml.XmlModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaShop.Services.Xml
{
    public interface IXmlManager
    {
        /// <summary>
        /// Create xml file of type T and given name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="model"></param>
        void CreateXmlFile<T>(string name, T model) where T : class;

        /// <summary>
        /// Delete file of name file name.
        /// </summary>
        /// <param name="fileName"></param>
        void DeleteXmlFile(string fileName);

        /// <summary>
        /// Get xml file as object of type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        T GetXmlModel<T>(string fileName) where T : class;

        /// <summary>
        /// Get all information about shop.
        /// </summary>
        /// <returns></returns>
        ContactAndHoursViewModel GetFullShopInformation();
    }
}
