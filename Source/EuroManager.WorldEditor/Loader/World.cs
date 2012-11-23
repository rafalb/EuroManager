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
        public string Name { get; set; }

        public int Year { get; set; }

        public IEnumerable<League> Leagues { get; set; }

        public IEnumerable<Division> Divisions { get; set; }

        public IEnumerable<Club> Clubs { get; set; }

        public IEnumerable<ClubRef> EuropeanCupsClubs { get; set; }

        public IEnumerable<Player> Players { get; set; }
    }
}
