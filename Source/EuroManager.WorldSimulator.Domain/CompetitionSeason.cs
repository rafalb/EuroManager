using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace EuroManager.WorldSimulator.Domain
{
    public abstract class CompetitionSeason : IEntity
    {
        protected CompetitionSeason(Competition competition, DateTime startDate, DateTime endDate)
        {
            World = competition.World;
            Competition = competition;
            StartDate = startDate;
            EndDate = endDate;

            IsActive = true;
        }

        protected CompetitionSeason()
        {
        }

        public int Id { get; private set; }

        [Timestamp]
        public byte[] Version { get; private set; }

        public int? WorldId { get; private set; }

        public virtual World World { get; private set; }

        public int? CompetitionId { get; private set; }

        public virtual Competition Competition { get; private set; }

        public bool IsActive { get; protected set; }

        public DateTime StartDate { get; private set; }

        public DateTime EndDate { get; private set; }

        public abstract void AdvanceDate();

        public abstract CompetitionSeason AdvanceSeason();
    }
}
