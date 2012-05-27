using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;

namespace EuroManager.WorldSimulator.Domain
{
    public class Goal : IEntity
    {
        public Goal(bool isForFirstTeam, int minute, int extended, Player scorer)
        {
            IsForFirstTeam = isForFirstTeam;
            Minute = minute;
            Extended = extended;
            Scorer = scorer;
            ScorerName = scorer.Name;
        }

        protected Goal()
        {
        }

        public int Id { get; private set; }

        [Timestamp]
        public byte[] Version { get; private set; }

        public int ResultId { get; private set; }

        public bool IsForFirstTeam { get; private set; }

        public bool IsForSecondTeam
        {
            get { return !IsForFirstTeam; }
        }

        public int Minute { get; private set; }

        public int Extended { get; private set; }

        public int? ScorerId { get; private set; }

        public Player Scorer { get; private set; }

        [StringLength(100)]
        public string ScorerName { get; private set; }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, Extended == 0 ? "{0} {1}" : "{0} {1}+{2}", ScorerName, Minute, Extended);
        }
    }
}
