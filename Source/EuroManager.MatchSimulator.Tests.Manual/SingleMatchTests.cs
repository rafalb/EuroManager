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
            Team team2 = teamConverter.CreateTeam("Wisla Krakow");

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

            PrintPlayerStats(match.Team1);
            PrintPlayerStats(match.Team2);
        }

        private static void PrintPlayerStats(Team team)
        {
            Console.WriteLine("{0,-18}  G  A  P+ P- D+ D- S+ S- Sb I+ I- T+ T- B+ B- G+ G- R", team.Name);
            Console.WriteLine("-------------------------------------------------------------------------");

            foreach (var player in team.Squad)
            {
                Console.Write("{0,-4}{1,-14} ", player.Position, player.Name);
                Console.Write("{0,2} {1,2} {2,2} {3,2} {4,2} {5,2} {6,2} {7,2} {8,2} ",
                    player.Goals, player.Assists, player.PassesCompleted, player.PassesFailed,
                    player.DribblesCompleted, player.DribblesFailed, player.ShotsOnTarget, player.ShotsMissed, player.ShotsBlocked);

                Console.Write("{0,2} {1,2} {2,2} {3,2} {4,2} {5,2} {6,2} {7,2} ",
                    player.PassesIntercepted, player.PassesAllowed, player.TacklesCompleted, player.TacklesFailed,
                    player.ShotsPrevented, player.ShotsAllowed, player.ShotsSaved, player.ShotsNotSaved);

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("{0,2}", player.FinalRating);
                Console.ResetColor();

                Console.WriteLine();
            }

            Console.WriteLine();
        }
    }
}
