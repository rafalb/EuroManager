using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace EuroManager.WorldEditor.Loader
{
    public class DivisionSet
    {
        public ClubRefSet ChampionsLeague { get; set; }

        public ClubRefSet EuropaLeague { get; set; }
    }
}
