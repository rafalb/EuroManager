using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace EuroManager.WorldSimulator.Domain
{
    public abstract class Tournament : IEntity
    {
        protected Tournament(Competition competition, string name, int level)
        {
            World = competition.World;
            Competition = competition;
            Name = name;
            Level = level;
        }

        protected Tournament()
        {
        }

        public int Id { get; private set; }

        [Timestamp]
        public byte[] Version { get; private set; }

        public int? WorldId { get; private set; }

        public virtual World World { get; private set; }

        public int? CompetitionId { get; private set; }

        public virtual Competition Competition { get; private set; }

        [Required]
        [StringLength(100)]
        public string Name { get; private set; }

        public int Level { get; private set; }
    }
}
