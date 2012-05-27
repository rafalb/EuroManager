using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace EuroManager.WorldSimulator.Domain
{
    public class Player : IEntity
    {
        public Player(World world, string name, int defensiveSkills, int offensiveSkills, int form)
        {
            World = world;
            Name = name;
            DefensiveSkills = defensiveSkills;
            OffensiveSkills = offensiveSkills;
            Form = form;
        }

        protected Player()
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

        public int DefensiveSkills { get; private set; }

        public int OffensiveSkills { get; private set; }

        public int Form { get; private set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
