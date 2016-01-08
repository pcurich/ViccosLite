using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Hosting;

namespace ViccosLite.Core.Data
{
    public class DataSettingsManager
    {
        private const char SEPARATOR = ':';
        private const string FILENAME = "Settings.txt";

        protected virtual string MapPath(string path)
        {
            if (HostingEnvironment.IsHosted)
            {
                //hosted
                return HostingEnvironment.MapPath(path);
            }
            //No host por ejemplo un test 
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            path = path.Replace("~/", "").TrimStart('/').Replace('/', '\\');
            return Path.Combine(baseDirectory, path);
        }

        protected virtual DataSettings ParseSettings(string text)
        {
            var shellSettings = new DataSettings();
            if (String.IsNullOrEmpty(text))
                return shellSettings;

            //Manera antigua de lectura. Esto permite un inesperado comportamiento cuando los usuarios de FTP transfieren archivos como ASCII (\r\n becomes \n)
            //var settings = text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            var settings = new List<string>();
            using (var reader = new StringReader(text))
            {
                string str;
                while ((str = reader.ReadLine()) != null)
                    settings.Add(str);
            }

            foreach (var setting in settings)
            {
                var separatorIndex = setting.IndexOf(SEPARATOR);
                if (separatorIndex == -1)
                {
                    continue;
                }
                var key = setting.Substring(0, separatorIndex).Trim();
                var value = setting.Substring(separatorIndex + 1).Trim();

                switch (key)
                {
                    case "DataProvider":
                        shellSettings.DataProvider = value;
                        break;
                    case "DataConnectionString":
                        shellSettings.DataConnectionString = value;
                        break;
                    default:
                        shellSettings.RawDataSettings.Add(key, value);
                        break;
                }
            }

            return shellSettings;
        }

        protected virtual string ComposeSettings(DataSettings settings)
        {
            if (settings == null)
                return "";

            return string.Format("DataProvider: {0}{2}DataConnectionString: {1}{2}",
                settings.DataProvider,
                settings.DataConnectionString,
                Environment.NewLine);
        }

        public virtual DataSettings LoadSettings(string filePath = null)
        {
            if (String.IsNullOrEmpty(filePath))
            {
                //use webHelper.MapPath en lugar de HostingEnvironment.MapPath el cual no esta permitido en test unit
                filePath = Path.Combine(MapPath("~/App_Data/"), FILENAME);
            }
            if (!File.Exists(filePath))
                return new DataSettings(); //No encontro el archivo setting.txt

            var text = File.ReadAllText(filePath);
            return ParseSettings(text);
        }

        public virtual void SaveSettings(DataSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");

            //use webHelper.MapPath instead of HostingEnvironment.MapPath which is not available in unit tests
            var filePath = Path.Combine(MapPath("~/App_Data/"), FILENAME);
            if (!File.Exists(filePath))
            {
                using (File.Create(filePath))
                {
                    //Se usa 'using' para cerrar el archivo despues de haberse creado
                }
            }

            var text = ComposeSettings(settings);
            File.WriteAllText(filePath, text);
        }
    }
}