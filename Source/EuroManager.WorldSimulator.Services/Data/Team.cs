using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EuroManager.Common.Domain;

namespace EuroManager.WorldSimulator.Services.Data
{
    public class Team
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ShortName { get; set; }

        public TeamStrategy Strategy { get; set; }
    }
}
