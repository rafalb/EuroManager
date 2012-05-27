using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace EuroManager.WorldEditor.Loader
{
    public class League
    {
        [XmlIgnore]
        public int NewId { get; set; }

        [XmlElement("Division")]
        public Division[] Divisions { get; set; }

        [XmlAttribute]
        public string Name { get; set; }
    }
}
