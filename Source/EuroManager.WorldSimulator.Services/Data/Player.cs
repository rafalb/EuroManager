using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EuroManager.Common.Domain;

namespace EuroManager.WorldSimulator.Services.Data
{
    public class Player
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public PositionCode Position { get; set; }

        public int DefensiveSkills { get; set; }

        public int OffensiveSkills { get; set; }

        public int Form { get; set; }
    }
}
