using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EuroManager.WorldSimulator.Domain
{
    public interface IEntity
    {
        int Id { get; }

        byte[] Version { get; }
    }
}
