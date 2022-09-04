using System;
using System.IO;
using Newtonsoft.Json;

namespace xPosBL.Terminals.Data
{
    public class SaveLoadTerminalData : ISaveLoad
    {
        public T Load<T>()where T: class
        {
            string path = Directory.GetCurrentDirectory() + @"\settings.json";
            return Load<T>(path);
        }

        public T Load<T>(string path) where T : class
        {
            if (!File.Exists(path)) return null;
            string jsonString = File.ReadAllText(path);
            T idTerminal = JsonConvert.DeserializeObject<T>(jsonString);
            return idTerminal;
        }

        public void Save<T>(T obj) where T : class
        {
            string path = Directory.GetCurrentDirectory() + @"\settings.json";
            Save<T>(obj, path);
        }

        public void Save<T>(T obj, string path) where T : class
        {
            File.WriteAllText(path, JsonConvert.SerializeObject(obj));
        }
    }
}
