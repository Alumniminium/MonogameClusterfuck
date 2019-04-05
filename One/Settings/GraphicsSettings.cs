using System;
using System.IO;
using Newtonsoft.Json;

namespace MonoGameClusterFuck.Settings
{
    public class GraphicsSettings
    {
        private static GraphicsSettings _instance;
        public static GraphicsSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GraphicsSettings();
                    _instance.Load();
                }

                return _instance;// ?? (_instance = new GraphicsSettings());
            }
        }

        public const string ConfigPath  = "GraphicsConfig.json";
        public int Width = 2560;
        public int Height = 1440;
        public bool VSync = true;
        public bool Fullscreen = true;

        private GraphicsSettings()
        {
            
        }

        public void Load()
        {
            if (File.Exists(ConfigPath))
            {
                var json = File.ReadAllText(ConfigPath);
                _instance = JsonConvert.DeserializeObject<GraphicsSettings>(json);
            }
        }
        public void Save()
        {
            var json = JsonConvert.SerializeObject(_instance,Formatting.Indented);
            File.WriteAllText(ConfigPath,json);
        }
    }
}