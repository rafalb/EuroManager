using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;

namespace EuroManager.WorldSimulator.Domain
{
    public class World : IEntity
    {
        public World(string name, DateTime createdDate, int startYear)
        {
            Name = name;
            CreatedDate = createdDate;
            StartYear = startYear;

            SeasonYear = StartYear;
            Date = SeasonStartDate.AddDays(-1);
        }

        protected World()
        {
        }

        public int Id { get; private set; }

        [Timestamp]
        public byte[] Version { get; private set; }

        [StringLength(100)]
        public string Name { get; private set; }

        public DateTime CreatedDate { get; private set; }

        public bool IsDefault { get; set; }

        public int StartYear { get; private set; }

        public DateTime Date { get; private set; }

        public int SeasonNumber
        {
            get { return SeasonYear - StartYear + 1; }
        }

        public int SeasonYear { get; private set; }

        public DateTime SeasonStartDate
        {
            get { return new DateTime(SeasonYear, 08, 01); }
        }

        public DateTime SeasonEndDate
        {
            get { return new DateTime(SeasonYear + 1, 07, 31); }
        }

        public DateTime NextSeasonStartDate
        {
            get { return SeasonStartDate.AddYears(1); }
        }

        public bool IsSeasonEnd
        {
            get { return Date == SeasonEndDate; }
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0} {1}", CreatedDate, Name);
        }

        public void SetAsDefault(World currentDefaultWorld)
        {
            if (currentDefaultWorld != null)
            {
                currentDefaultWorld.IsDefault = false;
            }

            IsDefault = true;
        }

        public void AdvanceDate(bool areAnyFixturesLeftToday)
        {
            if (IsSeasonEnd)
            {
                throw new InvalidOperationException("Cannot advance date: season has ended.");
            }

            if (areAnyFixturesLeftToday)
            {
                throw new InvalidOperationException("Cannot advance date: there are still fixtures to be played.");
            }

            Date = Date.AddDays(1).Date;
        }

        public void AdvanceSeason(bool areAnyFixturesLeftToday)
        {
            if (!IsSeasonEnd)
            {
                throw new InvalidOperationException("Cannot advance season: current season has not ended yet.");
            }

            if (areAnyFixturesLeftToday)
            {
                throw new InvalidOperationException("Cannot advance season: there are still fixtures to be played.");
            }

            SeasonYear += 1;
        }
    }
}
