using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace EuroManager.WorldSimulator.Domain
{
    public abstract class Competition : IEntity
    {
        protected Competition(World world, string name)
        {
            World = world;
            Name = name;
        }

        protected Competition()
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

        public override string ToString()
        {
            return Name;
        }
    }
}
