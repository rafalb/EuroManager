using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EuroManager.WorldSimulator.Services.Data
{
    public class PlayerMatchStats
    {
        public int PlayerId { get; set; }

        public string PlayerName { get; set; }

        public int Rating { get; set; }

        public int Goals { get; set; }

        public int Assists { get; set; }
    }
}
