using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using EuroManager.Common.Domain;
using EuroManager.MatchSimulator.Domain;
using EuroManager.MatchSimulator.Domain.Events;
using EuroManager.WorldEditor.Loader;

namespace EuroManager.MatchSimulator.Tests.Manual
{
    public partial class App : Application
    {
        private TestRunner testRunner = new TestRunner();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            testRunner.Run();
            App.Current.Shutdown();
        }
    }
}
