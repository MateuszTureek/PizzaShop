using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Xml;
using System.Xml.Serialization;

namespace PizzaShop.Services.XmlServices
{
    public class XmlManager : IXmlManager
    {
        XmlSerializer _serializer = null;
        StreamReader _sReader = null;
        StreamWriter _sWriter = null;

        public void CreateXmlFile<T>(string fileName, T model) where T: class
        {
            using (_sWriter = new StreamWriter(HostingEnvironment.MapPath("~/App_Data/" + fileName + ".xml")))
            {
                _serializer = new XmlSerializer(typeof(T));
                _serializer.Serialize(_sWriter, model);
            }
        }

        public void DeleteXmlFile(string fileName)
        {
            File.Delete(HostingEnvironment.MapPath("~/App_Data/" + fileName + ".xml"));
        }

        public T GetXmlModel<T>(string fileName) where T : class
        {
            using (_sReader = new StreamReader(HostingEnvironment.MapPath("~/App_Data/" + fileName + ".xml")))
            {
                _serializer = new XmlSerializer(typeof(T));
                T model = _serializer.Deserialize(_sReader) as T;
                return model;
            }
        }
    }
}