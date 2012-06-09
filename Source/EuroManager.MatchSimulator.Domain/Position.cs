using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EuroManager.Common.Domain;

namespace EuroManager.MatchSimulator.Domain
{
    public class Position
    {
        #region Definitions
        
        public static readonly Position Goalkeeper = new Position(PositionCode.GK, Location.Goal, Side.Center);
        public static readonly Position RightWingBack = new Position(PositionCode.RWB, Location.Defensive, Side.Right);
        public static readonly Position RightBack = new Position(PositionCode.RB, Location.Back, Side.Right);
        public static readonly Position RightCenterBack = new Position(PositionCode.RCB, Location.Back, Side.RightCenter);
        public static readonly Position CenterBack = new Position(PositionCode.CB, Location.Back, Side.Center);
        public static readonly Position LeftCenterBack = new Position(PositionCode.LCB, Location.Back, Side.LeftCenter);
        public static readonly Position LeftBack = new Position(PositionCode.LB, Location.Back, Side.Left);
        public static readonly Position LeftWingBack = new Position(PositionCode.LWB, Location.Defensive, Side.Left);
        public static readonly Position RightDefendingMidfielder = new Position(PositionCode.RDM, Location.Defensive, Side.RightCenter);
        public static readonly Position CenterDefendingMidfielder = new Position(PositionCode.CDM, Location.Defensive, Side.Center);
        public static readonly Position LeftDefendingMidfielder = new Position(PositionCode.LDM, Location.Defensive, Side.LeftCenter);
        public static readonly Position RightMidfielder = new Position(PositionCode.RM, Location.Midfield, Side.Right);
        public static readonly Position RightCenterMidfielder = new Position(PositionCode.RCM, Location.Midfield, Side.RightCenter);
        public static readonly Position CenterMidfielder = new Position(PositionCode.CM, Location.Midfield, Side.Center);
        public static readonly Position LeftCenterMidfielder = new Position(PositionCode.LCM, Location.Midfield, Side.LeftCenter);
        public static readonly Position LeftMidfielder = new Position(PositionCode.LM, Location.Midfield, Side.Left);
        public static readonly Position CenterAttackingMidfielder = new Position(PositionCode.CAM, Location.Offensive, Side.Center);
        public static readonly Position RightWinger = new Position(PositionCode.RW, Location.Offensive, Side.Right);
        public static readonly Position LeftWinger = new Position(PositionCode.LW, Location.Offensive, Side.Left);
        public static readonly Position RightForward = new Position(PositionCode.RF, Location.Forward, Side.RightCenter);
        public static readonly Position LeftForward = new Position(PositionCode.LF, Location.Forward, Side.LeftCenter);
        public static readonly Position Striker = new Position(PositionCode.ST, Location.Forward, Side.Center);

        private static readonly Dictionary<PositionCode, Position> AllPositions = new Dictionary<PositionCode, Position>
        {
            { PositionCode.GK, Goalkeeper },
            { PositionCode.RWB, RightWingBack },
            { PositionCode.RB, RightBack },
            { PositionCode.RCB, RightCenterBack },
            { PositionCode.CB, CenterBack },
            { PositionCode.LCB, LeftCenterBack },
            { PositionCode.LB, LeftBack },
            { PositionCode.LWB, LeftWingBack },
            { PositionCode.RDM, RightDefendingMidfielder },
            { PositionCode.CDM, CenterDefendingMidfielder },
            { PositionCode.LDM, LeftDefendingMidfielder },
            { PositionCode.RM, RightMidfielder },
            { PositionCode.RCM, RightCenterMidfielder },
            { PositionCode.CM, CenterMidfielder },
            { PositionCode.LCM, LeftCenterMidfielder },
            { PositionCode.LM, LeftMidfielder },
            { PositionCode.CAM, CenterAttackingMidfielder },
            { PositionCode.RW, RightWinger },
            { PositionCode.LW, LeftWinger },
            { PositionCode.RF, RightForward },
            { PositionCode.LF, LeftForward },
            { PositionCode.ST, Striker }
        };

        #endregion Definitions

        private static readonly Dictionary<Location, Location> OppositeLocations = new Dictionary<Location, Location>
        {
            { Location.Goal, Location.Forward },
            { Location.Back, Location.Forward },
            { Location.Defensive, Location.Offensive },
            { Location.Midfield, Location.Midfield },
            { Location.Offensive, Location.Defensive },
            { Location.Forward, Location.Back }
        };

        private static readonly Dictionary<Side, Side> OppositeSides = new Dictionary<Side, Side>
        {
            { Side.Right, Side.Left },
            { Side.RightCenter, Side.LeftCenter },
            { Side.Center, Side.Center },
            { Side.LeftCenter, Side.RightCenter },
            { Side.Left, Side.Right }
        };

        public Position(PositionCode code, Location location, Side side)
        {
            Code = code;
            Location = location;
            Side = side;
        }

        public PositionCode Code { get; private set; }

        public Location Location { get; private set; }

        public Side Side { get; private set; }

        public static Position FromCode(PositionCode code)
        {
            return AllPositions[code];
        }

        public override string ToString()
        {
            return Code.ToString();
        }

        public int DistanceForward(Position other)
        {
            return (int)other.Location - (int)Location;
        }

        public int DistanceSideways(Position other)
        {
            int distance = (int)other.Side - (int)Side;
            return distance >= 0 ? distance : -distance;
        }

        public Position Opposite()
        {
            return new Position(PositionCode.None, OppositeLocations[Location], OppositeSides[Side]);
        }
    }
}
