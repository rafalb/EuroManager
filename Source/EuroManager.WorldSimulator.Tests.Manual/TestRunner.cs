using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using EuroManager.WorldEditor;
using EuroManager.WorldEditor.Loader;
using EuroManager.WorldSimulator.Services;
using EuroManager.WorldSimulator.Services.Data;

namespace EuroManager.WorldSimulator.Tests.Manual
{
    public class TestRunner
    {
        private WorldSimulatorService worldSimulator = new WorldSimulatorService();

        public void Run()
        {
            Console.SetWindowSize(120, 60);
            Console.SetBufferSize(120, 600);

            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            var bootstrapper = new Bootstrapper();
            bootstrapper.Initialize();

            int worldCount = worldSimulator.GetWorldCount();
            bool createNewWorld = worldCount == 0;

            if (worldCount > 0)
            {
                Console.Write("Create new world (Y/N)? ");
                string answer = Console.ReadLine();
                createNewWorld = answer.Trim().StartsWith("Y", StringComparison.OrdinalIgnoreCase);
            }

            if (createNewWorld)
            {
                CreateNewWorld();
            }

            var multiSeasonTests = new MultipleSeasonTests();
            multiSeasonTests.Perform();
        }

        private void CreateNewWorld()
        {
            World world;

            var worldLoader = new WorldLoader();
            world = worldLoader.LoadWorld("Data.xlsx");

            var worldCreator = new WorldCreator();
            int worldId = worldCreator.CreateWorld(world);

            Console.WriteLine("Initializing season...");
            worldSimulator.SwitchDefaultWorld(worldId);
        }
    }
}
