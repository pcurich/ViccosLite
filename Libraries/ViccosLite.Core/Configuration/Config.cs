using System.Configuration;
using System.Xml;

namespace ViccosLite.Core.Configuration
{
    public class Config : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, XmlNode section)
        {
            var config = new Config();
            
            //todo agregar items en el web.xml
            return config;
        }

        public string UserAgentStringsPath { get; private set; }
        public bool IgnoreStartupTasks { get; set; }
        public string EngineType { get; private set; }
        public bool DynamicDiscovery { get; set; }
    }
}