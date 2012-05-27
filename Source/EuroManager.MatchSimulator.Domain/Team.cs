using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EuroManager.Common.Domain;

namespace EuroManager.MatchSimulator.Domain
{
    public class Team
    {
        private List<Player> squad = new List<Player>();
        private IMatchRandomizer random = MatchRandomizer.Current;
        
        public Team(string name, TeamStrategy strategy)
        {
            Name = name;
            Strategy = strategy;
        }

        public string Name { get; private set; }

        public TeamStrategy Strategy { get; private set; }

        public IEnumerable<Player> Squad
        {
            get { return squad; }
        }

        public Team Opponent { get; set; }

        public Player Goalkeeper
        {
            get { return squad.Single(p => p.IsGoalkeeper); }
        }

        public void AddSquadPlayer(int id, string name, Position position, int defensiveSkills, int offensiveSkills, int form)
        {
            AddSquadPlayer(new Player(id, name, this, position, defensiveSkills, offensiveSkills, form));
        }

        public void AddSquadPlayer(Player player)
        {
            squad.Add(player);
        }

        public Player PickPlayerInitiatingAttack()
        {
            var chances = squad.Select(p => p.ChanceToInitiateAttack(Strategy)).ToArray();
            return random.PlayerInitiatingAttack(squad, chances);
        }

        public Player PickPlayerToReceivePass(Player passingPlayer)
        {
            var chances = squad.Select(p => p.ChanceToReceivePass(passingPlayer, Strategy)).ToArray();
            return random.PlayerReceivingPass(squad, chances);
        }

        public Player PickPlayerToMarkPassReceiver(Player receiver)
        {
            var chances = squad.Select(p => p.ChanceToMarkPassReceiver(receiver)).ToArray();
            return random.PlayerMarkingPassReceiver(squad, chances);
        }

        public Player PickPlayerToConfrontDribbler(Player dribbler)
        {
            var chances = squad.Select(p => p.ChanceToConfrontDribbler(dribbler)).ToArray();
            return random.PlayerConfrontingDribbler(squad, chances);
        }

        public Player PickPlayerToConfrontShooter(Player shooter)
        {
            var chances = squad.Select(p => p.ChanceToConfrontShooter(shooter)).ToArray();
            return random.PlayerConfrontingShooter(squad, chances);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
