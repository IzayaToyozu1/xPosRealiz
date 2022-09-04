using Newtonsoft.Json;
using System;
using System.IO;

namespace xPosBL.GoodsDirectories
{
    [Serializable]
    public class SettingsSprav
    {
        private bool _mkoAndGst = false;

        public bool MKOandGST
        {
            get
            {
                return _mkoAndGst;
            }
            set
            {
                _mkoAndGst = value;
                Save();
            }
        }

        public SettingsSprav()
        {
            Load();
        }

        private void Load()
        {
            string path = Directory.GetCurrentDirectory() + "\\SettingsSprav";
            if (!File.Exists(path))
            {
                Save();
                return;
            }
            using (StreamReader stream = new StreamReader(path))
            {
                object[] obj = JsonConvert.DeserializeObject<object[]>(stream.ReadToEnd());
                _mkoAndGst = Convert.ToBoolean(obj[0]);
            }
        }

        private void Save()
        {
            string path = Directory.GetCurrentDirectory() + "\\SettingsSprav";
            using (StreamWriter stream = new StreamWriter(path))
            {
                object[] obj = new object[4];
                obj[0] = _mkoAndGst;
                stream.Write(JsonConvert.SerializeObject(obj));
            }
        }
    }
}
