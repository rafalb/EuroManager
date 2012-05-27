using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using EuroManager.WorldSimulator.Domain;

namespace EuroManager.WorldSimulator.DataAccess
{
    public class WorldModelMapper
    {
        public void MapModel(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NationalLeague>().ToTable("NationalLeagues");

            modelBuilder.Entity<NationalLeagueSeason>().ToTable("NationalLeagueSeasons");

            modelBuilder.Entity<Cup>().ToTable("Cups");
            modelBuilder.Entity<League>().ToTable("Leagues");
            modelBuilder.Entity<LeaguePlayOffs>().ToTable("LeaguePlayOffs");

            modelBuilder.Entity<CupSeason>().ToTable("CupSeasons");
            modelBuilder.Entity<LeagueSeason>().ToTable("LeagueSeasons");
            modelBuilder.Entity<LeaguePlayOffsSeason>().ToTable("LeaguePlayOffsSeasons");

            modelBuilder.Entity<GroupStage>().ToTable("GroupStages");
            modelBuilder.Entity<KnockoutStage>().ToTable("KnockoutStages");
            modelBuilder.Entity<TieKnockoutStage>().ToTable("TieKnockoutStages");

            modelBuilder.Entity<GroupStageSeason>().ToTable("GroupStageSeasons");
            modelBuilder.Entity<KnockoutStageSeason>().ToTable("KnockoutStageSeasons");
            modelBuilder.Entity<TieKnockoutStageSeason>().ToTable("TieKnockoutStageSeasons");

            modelBuilder.Entity<TournamentSeason>().HasMany<Team>(t => t.Teams).WithMany();
            modelBuilder.Entity<TournamentSeason>().HasMany<Team>(t => t.PromotedTeams).WithMany().Map(c => c.ToTable("TournamentSeasonPromotedTeams"));
            modelBuilder.Entity<TournamentSeason>().HasMany<Team>(t => t.RelegatedTeams).WithMany().Map(c => c.ToTable("TournamentSeasonRelegatedTeams"));
            modelBuilder.Entity<TournamentSeason>().HasMany<Team>(t => t.PromotionPlayOffTeams).WithMany().Map(c => c.ToTable("TournamentSeasonPromotionPlayOffsTeams"));
            modelBuilder.Entity<TournamentSeason>().HasMany<Team>(t => t.RelegationPlayOffTeams).WithMany().Map(c => c.ToTable("TournamentSeasonRelegationPlayOffsTeams"));
        }
    }
}
