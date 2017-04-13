using PizzaShop.Services.Xml.XmlModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Xml.Serialization;

namespace PizzaShop.Services.Xml
{
    public class XmlManager : IXmlManager
    {
        XmlSerializer _serializer = null;
        StreamReader _sReader = null;
        StreamWriter _sWriter = null;

        public void CreateXmlFile<T>(string fileName, T model) where T : class
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

        public ContactAndHoursViewModel GetFullShopInformation()
        {
            var shopContact = GetXmlModel<ShopContact>(GlobalXmlManager.ContactFileName);
            var openingHours = GetXmlModel<OpeningHours>(GlobalXmlManager.OpeningHoursFileName);
            ContactAndHoursViewModel viewModel = new ContactAndHoursViewModel()
            {
                Address = shopContact.Address,
                Contact = shopContact.Contact,
                WorksDays = openingHours.WorksDays
            };
            return viewModel;
        }
    }
}