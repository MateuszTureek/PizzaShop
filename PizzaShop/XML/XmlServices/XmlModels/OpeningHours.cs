using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace PizzaShop.XML.Services.XmlServices.XmlModels
{
    [Serializable]
    [XmlRoot("OpeningHours")]
    public class OpeningHours
    {
        public List<Days> WorksDays { get; set; }
    }

    [Serializable]
    public class Days
    {
        [XmlText]
        public string WorkHours { get; set; }
        [XmlAttribute]
        public string Name { get; set; }
    }
}   