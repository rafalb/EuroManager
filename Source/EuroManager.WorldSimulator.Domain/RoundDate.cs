using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace EuroManager.WorldSimulator.Domain
{
    public class RoundDate : IEntity
    {
        public RoundDate(DateTime date, int round)
        {
            Date = date;
            Round = round;
        }

        protected RoundDate()
        {
        }

        public int Id { get; private set; }

        [Timestamp]
        public byte[] Version { get; private set; }

        public DateTime Date { get; private set; }

        public int Round { get; private set; }
    }
}
