using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace EuroManager.WorldEditor.Loader
{
    public class ClubRef
    {
        [XmlAttribute]
        public string Id { get; set; }
    }
}
