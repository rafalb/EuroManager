using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.EntityClient;
using System.Linq;
using System.Text;
using AutoMapper;
using EuroManager.WorldSimulator.DataAccess;

namespace EuroManager.WorldSimulator.Services
{
    public class Bootstrapper
    {
        public void Initialize()
        {
            InitializeAutoMapper();
            InitializeDatabase();
        }

        private void InitializeAutoMapper()
        {
            Mapper.CreateMap<Domain.Goal, Data.Goal>();
            Mapper.CreateMap<Domain.MatchResult, Data.MatchResult>();
            Mapper.CreateMap<Domain.MatchResult, Data.MatchResultDetails>();
            Mapper.CreateMap<Domain.PlayerMatchStats, Data.PlayerMatchStats>();
            Mapper.CreateMap<Domain.Team, Data.Team>();
            Mapper.CreateMap<Domain.TeamStats, Data.TeamStats>();
        }

        private void InitializeDatabase()
        {
            using (var context = new WorldContext())
            {
                context.Database.Initialize(force: false);
            }
        }
    }
}
