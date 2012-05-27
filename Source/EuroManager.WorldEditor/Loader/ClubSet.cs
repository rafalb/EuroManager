using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace EuroManager.WorldEditor.Loader
{
    public class ClubSet
    {
        [XmlElement("Club")]
        public Club[] Clubs { get; set; }
    }
}
