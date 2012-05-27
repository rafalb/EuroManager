using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EuroManager.WorldSimulator.Domain;

namespace EuroManager.WorldSimulator.Services.Data
{
    public class Tournament
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Level { get; set; }
    }
}
