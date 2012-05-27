using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace EuroManager.WorldEditor.Loader
{
    public class WorldLoader
    {
        public World LoadWorld(Stream stream)
        {
            Console.WriteLine("Loading world data...");

            var serializer = new XmlSerializer(typeof(World));
            var world = (World)serializer.Deserialize(stream);

            return world;
        }
    }
}
