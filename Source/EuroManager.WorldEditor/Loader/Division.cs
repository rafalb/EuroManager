using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace EuroManager.WorldEditor.Loader
{
    public class Division
    {
        [XmlIgnore]
        public int NewId { get; set; }

        [XmlAttribute]
        public int Level { get; set; }

        [XmlAttribute]
        public string Name { get; set; }

        [XmlElement("Club")]
        public Club[] Clubs { get; set; }
    }
}
