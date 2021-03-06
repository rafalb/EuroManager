﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using EuroManager.Common;

namespace EuroManager.WorldSimulator.Domain
{
    public class CupSeason : TournamentSeason
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Justification = "Required by Entity Framework")]
        public CupSeason(Cup cup, DateTime startDate, DateTime endDate, IEnumerable<Team> teams)
            : base(cup, startDate, endDate)
        {
            DayOfWeek = cup.DayOfWeek;
            Frequency = cup.Frequency;

            AddTeams(teams);

            Stages = cup.StagesOrdered.Select(s => s.CreateStageSeason()).ToList();
            InitializeStages();
        }

        protected CupSeason()
        {
        }

        public DayOfWeek DayOfWeek { get; private set; }

        public int Frequency { get; private set; }

        public virtual List<CupStageSeason> Stages { get; private set; }

        public IEnumerable<CupStageSeason> StagesOrdered
        {
            get { return Stages.OrderBy(s => s.StageNumber); }
        }

        public int CurrentStageIndex { get; private set; }

        public CupStageSeason CurrentStage
        {
            get { return CurrentStageIndex < Stages.Count ? StagesOrdered.ElementAt(CurrentStageIndex) : null; }
        }

        private Cup Cup
        {
            get { return (Cup)Tournament; }
        }

        public override void ScheduleFixtures(Action<Fixture> addFixture)
        {
            if (CurrentStageIndex < Stages.Count)
            {
                Phase = TournamentPhase.InProgress;
                CurrentStage.ScheduleFixtures(addFixture);
                NextSchedulingDate = null;
            }
        }

        public override void ApplyResult(MatchResult result)
        {
            base.ApplyResult(result);

            CurrentStage.ApplyResult(result);

            if (CurrentStage.IsFinished)
            {
                var previousStage = CurrentStage;
                CurrentStageIndex += 1;

                if (CurrentStageIndex == Stages.Count)
                {
                    Phase = TournamentPhase.Finished;

                    PromotedTeams.AddRange(previousStage.PromotedTeams);
                    PromotionPlayOffTeams.AddRange(previousStage.Teams.Except(PromotedTeams).ToArray());
                }
                else
                {
                    CurrentStage.Activate(previousStage.PromotedTeams);
                    NextSchedulingDate = World.Date;
                }
            }
        }

        public override TournamentSeason AdvanceSeason(IEnumerable<Team> relegatedFromHigher, IEnumerable<Team> promotedFromLower)
        {
            IEnumerable<Team> updatedTeams = Teams;

            if (relegatedFromHigher.Any())
            {
                updatedTeams = updatedTeams.Except(PromotedTeams).Union(relegatedFromHigher);
            }

            if (promotedFromLower.Any())
            {
                updatedTeams = updatedTeams.Except(RelegatedTeams).Union(promotedFromLower);
            }

            IsActive = false;

            var nextSeason = new CupSeason(Cup, StartDate.AddYears(1), EndDate.AddYears(1), updatedTeams.ToArray());
            return nextSeason;
        }

        private void InitializeStages()
        {
            Stages.ForEach(s => s.CupSeason = this);

            ScheduleRoundDates();

            CurrentStageIndex = 0;
            CurrentStage.Activate(Teams);
        }

        private void ScheduleRoundDates()
        {
            var scheduler = new Scheduler();
            var roundDates = scheduler.ScheduleRoundDates(StartDate, EndDate, DayOfWeek, Frequency,
                StagesOrdered.Select(s => s.RoundCount).ToArray());

            int roundCount = 0;

            foreach (var stage in StagesOrdered)
            {
                var stageDates = roundDates.Skip(roundCount).Take(stage.RoundCount).ToArray();
                stage.ScheduleRoundDates(stageDates);

                roundCount += stage.RoundCount;
            }
        }
    }
}
