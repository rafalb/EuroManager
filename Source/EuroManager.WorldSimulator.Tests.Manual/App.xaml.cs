using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;

namespace EuroManager.WorldSimulator.Tests.Manual
{
    public partial class App : Application
    {    
        private TestRunner testRunner = new TestRunner();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            testRunner.Run();
        }
    }
}
