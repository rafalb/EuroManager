using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EuroManager.Common.Domain;

namespace EuroManager.MatchSimulator.Domain.Tests.Builders
{
    public class TeamBuilder
    {
        private string name = "Team";
        private TeamStrategy strategy = TeamStrategy.Center;
        private int skills = 75;

        public TeamBuilder Named(string name)
        {
            this.name = name;
            return this;
        }

        public TeamBuilder WithStrategy(TeamStrategy strategy)
        {
            this.strategy = strategy;
            return this;
        }

        public TeamBuilder Skilled(int skills)
        {
            this.skills = skills;
            return this;
        }

        public Team Build()
        {
            var team = new Team(name, strategy);

            Position[] positions =
            {
                Position.Goalkeeper,
                Position.RightBack, Position.RightCenterBack, Position.LeftCenterBack, Position.LeftBack,
                Position.RightMidfielder, Position.RightCenterMidfielder, Position.LeftCenterMidfielder, Position.LeftMidfielder,
                Position.RightStriker, Position.LeftStriker
            };

            for (int i = 1; i <= 11; i++)
            {
                team.AddSquadPlayer(i, "Player " + i, positions[i - 1], skills, skills, skills);
            }

            return team;
        }
    }
}
