using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace EuroManager.WorldEditor.Loader
{
    public class Division
    {
        public string Id { get; set; }

        public int NewId { get; set; }

        public string Name { get; set; }

        public string LeagueId { get; set; }

        public int Level { get; set; }
    }
}
