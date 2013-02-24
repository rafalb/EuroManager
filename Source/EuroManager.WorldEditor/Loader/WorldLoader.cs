using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using EuroManager.Common.Domain;

namespace EuroManager.WorldEditor.Loader
{
    public class WorldLoader
    {
        public World LoadWorld(string path)
        {
            World world;
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties='Excel 12.0 xml;HDR=YES;'";

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                world = LoadWorld(LoadDataTableFromSheet(connection, "World"));
                world.Leagues = LoadLeagues(LoadDataTableFromSheet(connection, "Leagues"));
                world.Divisions = LoadDivisions(LoadDataTableFromSheet(connection, "Divisions"));
                world.Clubs = LoadClubs(LoadDataTableFromSheet(connection, "Clubs"));
                world.EuropeanCupsClubs = LoadEuropeanCupsClubs(LoadDataTableFromSheet(connection, "EuropeanCups"));
                world.Players = LoadPlayers(LoadDataTableFromSheet(connection, "Players"));
            }

            return world;
        }

        private DataTable LoadDataTableFromSheet(OleDbConnection connection, string sheetName)
        {
            DataSet dataSet = new DataSet();
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter("select * from [" + sheetName + "$]", connection);
            dataAdapter.Fill(dataSet);

            return dataSet.Tables[0];
        }

        private World LoadWorld(DataTable worldData)
        {
            var worldRow = worldData.Rows[0];

            World world = new World
            {
                Name = Convert.ToString(worldRow["Name"]),
                Year = Convert.ToInt32(worldRow["Year"])
            };

            return world;
        }

        private IEnumerable<League> LoadLeagues(DataTable leagueData)
        {
            var leagues = new League[leagueData.Rows.Count];

            for (int i = 0; i < leagues.Length; i++)
            {
                var row = leagueData.Rows[i];

                leagues[i] = new League
                {
                    Id = Convert.ToString(row["Id"]),
                    Name = Convert.ToString(row["Name"])
                };
            }

            return leagues;
        }

        private IEnumerable<Division> LoadDivisions(DataTable divisionData)
        {
            var divisions = new Division[divisionData.Rows.Count];

            for (int i = 0; i < divisions.Length; i++)
            {
                var row = divisionData.Rows[i];

                divisions[i] = new Division
                {
                    Id = Convert.ToString(row["Id"]),
                    Name = Convert.ToString(row["Name"]),
                    LeagueId = Convert.ToString(row["League"]),
                    Level = Convert.ToInt32(row["Level"])
                };
            }

            return divisions;
        }

        private IEnumerable<Club> LoadClubs(DataTable clubData)
        {
            var clubs = new Club[clubData.Rows.Count];

            for (int i = 0; i < clubs.Length; i++)
            {
                var row = clubData.Rows[i];

                clubs[i] = new Club
                {
                    Id = Convert.ToString(row["Id"]),
                    Name = Convert.ToString(row["Name"]),
                    DivisionId = Convert.ToString(row["Division"]),
                    Strategy = ParseEnum<TeamStrategy>(row["Strategy"])
                };
            }

            return clubs;
        }

        private IEnumerable<ClubRef> LoadEuropeanCupsClubs(DataTable europeanCupsData)
        {
            var clubs = new ClubRef[europeanCupsData.Rows.Count];

            for (int i = 0; i < clubs.Length; i++)
            {
                var row = europeanCupsData.Rows[i];

                clubs[i] = new ClubRef
                {
                    ClubId = Convert.ToString(row["Club"]),
                    Level = Convert.ToInt32(row["Level"])
                };
            }

            return clubs;
        }

        private IEnumerable<Player> LoadPlayers(DataTable playerData)
        {
            var players = new Player[playerData.Rows.Count];

            for (int i = 0; i < players.Length; i++)
            {
                var row = playerData.Rows[i];

                players[i] = new Player
                {
                    Name = Convert.ToString(row["Name"]),
                    ClubId = Convert.ToString(row["Club"]),
                    Position = ParseEnum<PositionCode>(row["Position"]),
                    Defending = Convert.ToInt32(row["Defending"]),
                    Attacking = Convert.ToInt32(row["Attacking"]),
                    Form = Convert.ToInt32(row["Form"])
                };
            }

            return players.ToArray();
        }

        private T ParseEnum<T>(object value)
        {
            return (T)Enum.Parse(typeof(T), Convert.ToString(value));
        }
    }
}
