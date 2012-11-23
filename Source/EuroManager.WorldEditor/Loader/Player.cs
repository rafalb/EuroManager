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

        public string Name { get; set; }

        public string ClubId { get; set; }

        public PositionCode Position { get; set; }

        public int Defending { get; set; }

        public int Attacking { get; set; }

        public int Form { get; set; }
    }
}
