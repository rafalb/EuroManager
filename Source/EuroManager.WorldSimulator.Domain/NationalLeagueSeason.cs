using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace EuroManager.WorldSimulator.Domain
{
    public class NationalLeagueSeason : CompetitionSeason
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Justification = "Required by Entity Framework")]
        public NationalLeagueSeason(NationalLeague league, DateTime startDate, DateTime endDate)
            : base(league, startDate, endDate)
        {
            Divisions = new List<TournamentSeason>();
        }

        protected NationalLeagueSeason()
        {
        }

        public virtual List<TournamentSeason> Divisions { get; private set; }

        public virtual LeaguePlayOffsSeason PlayOffs { get; private set; }

        public bool ArePlayOffsFinished { get; private set; }

        public NationalLeague League
        {
            get { return (NationalLeague)Competition; }
        }

        public void AddDivisionSeason(TournamentSeason divisionSeason)
        {
            Divisions.Add(divisionSeason);
        }

        public override void AdvanceDate()
        {
            if (PlayOffs == null && Divisions.All(d => d.IsFinished))
            {
                OnAllDivisionsFinished();
            }

            if (!ArePlayOffsFinished && PlayOffs != null && PlayOffs.IsFinished)
            {
                OnPlayOffsFinished();
                ArePlayOffsFinished = true;
            }
        }

        public override CompetitionSeason AdvanceSeason()
        {
            var newDivisionSeasons = new List<TournamentSeason>();
            var divisions = Divisions.OrderBy(d => d.Level).ToArray();

            for (int level = 1; level <= divisions.Length; level++)
            {
                var relegatedFromHigher = level > 1 ? divisions[level - 2].RelegatedTeams : Enumerable.Empty<Team>();
                var promotedFromLower = level < divisions.Length ? divisions[level].PromotedTeams : Enumerable.Empty<Team>();

                var newDivisionSeason = divisions[level - 1].AdvanceSeason(relegatedFromHigher, promotedFromLower);
                newDivisionSeasons.Add(newDivisionSeason);
            }

            PlayOffs.AdvanceSeason(Enumerable.Empty<Team>(), Enumerable.Empty<Team>());
            IsActive = false;

            var newSeason = new NationalLeagueSeason(League, StartDate.AddYears(1), EndDate.AddYears(1));
            newSeason.Divisions = newDivisionSeasons;
            return newSeason;
        }

        private void OnAllDivisionsFinished()
        {
            var playOffPairs = CreatePlayOffPairs();
            PlayOffs = new LeaguePlayOffsSeason(League.PlayOffs, World.Date.AddDays(3), EndDate, playOffPairs);
        }

        private void OnPlayOffsFinished()
        {
            foreach (var tie in PlayOffs.Ties)
            {
                TournamentSeason winnerDivision = DivisionByTeam(tie.Winner);
                TournamentSeason loserDivision = DivisionByTeam(tie.Loser);

                if (winnerDivision.Level > loserDivision.Level)
                {
                    winnerDivision.PromoteTeam(tie.Winner);
                    loserDivision.RelegateTeam(tie.Loser);
                }
            }
        }

        private TournamentSeason DivisionByTeam(Team team)
        {
            return Divisions.First(d => d.Teams.Contains(team));
        }

        private IEnumerable<TeamPair> CreatePlayOffPairs()
        {
            var pairs = new List<TeamPair>();
            var divisions = Divisions.OrderBy(d => d.Level).ToArray();

            for (int level = 1; level < divisions.Length; level++)
            {
                var divisionPairs = CreatePairsForDivisions(divisions[level - 1], divisions[level]);
                pairs.AddRange(divisionPairs);
            }

            return pairs;
        }

        private IEnumerable<TeamPair> CreatePairsForDivisions(TournamentSeason division, TournamentSeason lowerDivision)
        {
            return Enumerable.Zip(division.RelegationPlayOffTeams, lowerDivision.PromotionPlayOffTeams.AsEnumerable().Reverse(),
                (t1, t2) => new TeamPair(t2, t1)).ToArray();
        }
    }
}
