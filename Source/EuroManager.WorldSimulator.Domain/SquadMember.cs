using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using EuroManager.Common.Domain;

namespace EuroManager.WorldSimulator.Domain
{
    public class SquadMember : IEntity
    {
        public SquadMember(PositionCode position, int playerId)
        {
            Position = position;
            PlayerId = playerId;
        }

        public SquadMember(PositionCode position, Player player)
        {
            Position = position;
            Player = player;
        }

        protected SquadMember()
        {
        }

        public int Id { get; private set; }

        [Timestamp]
        public byte[] Version { get; private set; }

        public int? TeamId { get; private set; }

        public int PositionId { get; private set; }

        public PositionCode Position
        {
            get { return (PositionCode)PositionId; }
            private set { PositionId = (int)value; }
        }

        public int? PlayerId { get; private set; }

        public Player Player { get; private set; }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0} {1}", Position, Player.Name);
        }
    }
}
