using Newtonsoft.Json;
using System.Reflection;

namespace Utils
{
    public class TestConfig
    {
        public string DBConnection { get; set; }
        public string DBName { get; set; }
        public int InitSamplePetCount { get; set; }
        public static TestConfig Load(string name)
        {
            string workingFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string configFile = Path.Join(workingFolder, $"{name}.json");
            if (File.Exists(configFile))
                return JsonConvert.DeserializeObject<TestConfig>(File.ReadAllText(configFile));
            return null;
        }
    }
}