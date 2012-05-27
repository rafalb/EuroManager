using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using EuroManager.Common;

namespace EuroManager.WorldSimulator.Domain
{
    public class NationalLeague : Competition
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Justification = "Required by Entity Framework")]
        public NationalLeague(World world, string name)
            : base(world, name)
        {
            Divisions = new List<Tournament>();
        }

        protected NationalLeague()
        {
        }

        public virtual List<Tournament> Divisions { get; private set; }

        public int? PlayOffsId { get; private set; }

        public virtual LeaguePlayOffs PlayOffs { get; set; }

        public void AddDivision(Tournament division)
        {
            Divisions.Add(division);
        }
    }
}
