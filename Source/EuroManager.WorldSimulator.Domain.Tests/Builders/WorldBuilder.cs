using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EuroManager.WorldSimulator.Domain.Tests.Builders
{
    public class WorldBuilder
    {
        public World Build()
        {
            return new World("Test", DateTime.Now, 2012);
        }
    }
}
