using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using EuroManager.Common.Domain;

namespace EuroManager.WorldEditor.Loader
{
    public class Club
    {
        [XmlIgnore]
        public int NewId { get; set; }

        [XmlAttribute]
        public string Id { get; set; }

        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public TeamStrategy Strategy { get; set; }

        [XmlElement("Player")]
        public Player[] Players { get; set; }
    }
}
