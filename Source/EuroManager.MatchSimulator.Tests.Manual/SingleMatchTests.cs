using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EuroManager.MatchSimulator.Domain;
using EuroManager.WorldEditor.Loader;

namespace EuroManager.MatchSimulator.Tests.Manual
{
    public class SingleMatchTests
    {
        private TeamConverter teamConverter;

        public SingleMatchTests(World world)
        {
            teamConverter = new TeamConverter(world);
        }

        public void Perform()
        {
            Team team1 = teamConverter.CreateTeam("Newcastle United");
            Team team2 = teamConverter.CreateTeam("FC Barcelona");

            var match = new Match(team1, team2, isNeutralGround: false, isExtraTimeRequired: true);
            var simulator = new Simulator(MatchRandomizer.Current);
            simulator.Play(match);

            var printingVisitor = new MatchEventPrintingVisitor();

            foreach (var group in match.Events.GroupBy(e => Tuple.Create(e.Minute, e.Extended)).OrderBy(g => g.Key))
            {
                if (group.Key.Item2 == 0)
                {
                    Console.Write("{0,5}: ", group.Key.Item1);
                }
                else
                {
                    Console.Write("{0,3}+{1}: ", group.Key.Item1, group.Key.Item2);
                }

                var events = group.ToArray();

                for (int i = 0; i < events.Length; i++)
                {
                    events[i].Visit(printingVisitor);

                    if (i < events.Length - 1)
                    {
                        Console.Write(", ");
                    }
                }

                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine(match);
            Console.WriteLine();

            PrintPlayerRatings(match.Team1);
            PrintPlayerRatings(match.Team2);
        }

        private static void PrintPlayerRatings(Team team)
        {
            Console.WriteLine(team.Name);
            Console.WriteLine("-------------------");

            foreach (var player in team.Squad)
            {
                Console.WriteLine("{0,-15} {1,2}", player, (int)Math.Ceiling(player.Rating * 10));
            }

            Console.WriteLine();
        }
    }
}
