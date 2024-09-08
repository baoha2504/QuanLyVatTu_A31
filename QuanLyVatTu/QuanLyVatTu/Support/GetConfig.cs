using Newtonsoft.Json;
using QuanLyVatTu.Class;
using System.IO;

namespace QuanLyVatTu.Support
{
    internal class GetConfig
    {
        public Config ReadConfig(string path)
        {
            string configFilePath = Path.Combine(path, "config.json");
            if (!File.Exists(configFilePath))
            {
                throw new FileNotFoundException("Config file not found");
            }

            string json = File.ReadAllText(configFilePath);
            Config config = JsonConvert.DeserializeObject<Config>(json);
            return config;
        }

        public void UpdateLogin(string path, string username, string password)
        {
            //string configFilePath = Path.Combine(path, "config.json");
            //Config config = ReadConfig(path);
            //config.login.username = username;
            //config.login.password = password;

            //string updatedJson = JsonConvert.SerializeObject(config, Formatting.Indented);
            //File.WriteAllText(configFilePath, updatedJson);
        }
    }
}
