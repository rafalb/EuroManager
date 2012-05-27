using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace EuroManager.WorldEditor.Loader
{
    public class DivisionSet
    {
        public ClubRefSet Division1 { get; set; }

        public ClubRefSet Division2 { get; set; }

        public ClubRefSet Division3 { get; set; }
    }
}
