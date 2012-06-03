using System;
using System.Collections.Generic;
using EuroManager.MatchSimulator.Domain.Actions;

namespace EuroManager.MatchSimulator.Domain
{
    public interface IMatchRandomizer
    {
        int ExtendedTime();

        Team AttackingTeam(Team team1, Team team2, bool isNeutralGround);

        bool IsFirstTeamStartingPenaltyShootout();

        bool TryPass(double passing, double positioning, double marking);

        bool TryDribble(double dribbling, double tackling);

        ShotResult TryShoot(double shooting, double blocking, double goalkeeping);

        bool TryPenaltyKick();

        Player PlayerConfrontingDribbler(IEnumerable<Player> players, double[] chances);

        Player PlayerConfrontingShooter(IEnumerable<Player> players, double[] chances);

        Player PlayerInitiatingAttack(IEnumerable<Player> players, double[] chances);

        Player PlayerMarkingPassReceiver(IEnumerable<Player> players, double[] chances);

        Player PlayerReceivingPass(IEnumerable<Player> players, double[] chances);

        IMatchAction NextAction(int level, Player player);
    }
}
