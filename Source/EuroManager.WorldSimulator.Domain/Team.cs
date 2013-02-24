using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using EuroManager.Common.Domain;

namespace EuroManager.WorldSimulator.Domain
{
    public class Team : IEntity
    {
        public Team(World world, string name, TeamStrategy strategy, IEnumerable<SquadMember> squad)
        {
            World = world;
            Name = name;
            Strategy = strategy;
            Squad = squad.ToList();
        }

        protected Team()
        {
        }

        public int Id { get; private set; }

        [Timestamp]
        public byte[] Version { get; private set; }

        public int? WorldId { get; private set; }

        public virtual World World { get; private set; }

        [Required]
        [StringLength(100)]
        public string Name { get; private set; }

        public TeamStrategy Strategy { get; private set; }

        public virtual List<SquadMember> Squad { get; private set; }

        public IEnumerable<Player> Players
        {
            get { return Squad.Select(s => s.Player).ToArray(); }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
