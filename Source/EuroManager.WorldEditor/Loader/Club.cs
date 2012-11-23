using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using EuroManager.Common.Domain;

namespace EuroManager.WorldEditor.Loader
{
    public class Club
    {
        public string Id { get; set; }
        
        public int NewId { get; set; }

        public string Name { get; set; }

        public string DivisionId { get; set; }

        public TeamStrategy Strategy { get; set; }
    }
}
