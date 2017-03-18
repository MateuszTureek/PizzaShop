using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaShop.XML.Services.XmlServices
{
    public interface IXmlManager
    {
        void CreateXmlFile<T>(string name, T model) where T : class;
        void DeleteXmlFile(string fileName);
        T GetXmlModel<T>(string fileName) where T : class;
    }
}
