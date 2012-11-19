using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace EuroManager.WorldEditor.Loader
{
    [XmlRoot("World")]
    public class World
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public int Year { get; set; }

        public DivisionSet EuropeanCups { get; set; }

        [XmlElement("League")]
        public League[] Leagues { get; set; }

        public ClubSet RestOfWorld { get; set; }
    }
}
