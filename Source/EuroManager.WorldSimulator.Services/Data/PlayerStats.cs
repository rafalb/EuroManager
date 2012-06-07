using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EuroManager.Common.Domain;

namespace EuroManager.WorldSimulator.Services.Data
{
    public class PlayerStats
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public PositionCode Position { get; set; }

        public int Played { get; set; }

        public double Rating { get; set; }
    }
}
