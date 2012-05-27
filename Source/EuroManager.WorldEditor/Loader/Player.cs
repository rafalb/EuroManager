using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using EuroManager.Common.Domain;

namespace EuroManager.WorldEditor.Loader
{
    public class Player
    {
        [XmlIgnore]
        public int NewId { get; set; }

        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public PositionCode Position { get; set; }

        [XmlAttribute]
        public int Defending { get; set; }

        [XmlAttribute]
        public int Attacking { get; set; }

        [XmlAttribute]
        public int Form { get; set; }
    }
}
