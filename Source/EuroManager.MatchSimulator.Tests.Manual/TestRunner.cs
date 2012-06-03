using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EuroManager.WorldEditor.Loader;

namespace EuroManager.MatchSimulator.Tests.Manual
{
    public class TestRunner
    {
        private World world;

        public void Run()
        {
            //Console.SetWindowSize(120, 60);
            //Console.SetBufferSize(120, 60);

            using (var stream = File.OpenRead("Data.xml"))
            {
                var loader = new WorldLoader();
                world = loader.LoadWorld(stream);
            }

            var singleMatchTests = new SingleMatchTests(world);
            singleMatchTests.Perform();
            Console.ReadLine();

            var multipleMatchesTests = new MultipleMatchesTests(world);
            multipleMatchesTests.Perform(1000);
            Console.ReadLine();
        }
    }
}
