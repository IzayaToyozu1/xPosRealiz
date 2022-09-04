using System;
using System.IO;
using Newtonsoft.Json;
 

namespace xPosBL
{
    [Serializable]
    public class SettingXPOS 
    {
        private string _fileNameSprav = "AIn";
        private string _fileNameFullSprav = "FULL";
        private bool _MKOandGST = false;
        private int _limitCheckingFile = 3;

        public string FileNameSprav 
        {
            get 
            { 
                return _fileNameSprav; 
            }
            set 
            {
                _fileNameSprav = value.Length < 25 ? value : _fileNameSprav;
                Save();
            } 
        }
        public string FileNameFullSprav
        {
            get
            {
                return _fileNameFullSprav;
            }
            set
            {
                _fileNameFullSprav = value.Length < 25 ? value : _fileNameFullSprav;
                Save();
            }
        }
        public bool MKOandGST { 
            get 
            {
                return _MKOandGST;
            }
            set 
            {
                _MKOandGST = value;
                Save();
            } 
        }
        public int LimitCheckingFile 
        {
            get 
            { 
                return _limitCheckingFile; 
            }
            set 
            { 
                _limitCheckingFile = value;
                Save();
            } 
        }

        public SettingXPOS()
        {
            Load();
        }

        private void Load()
        {
            string path = Directory.GetCurrentDirectory() + "\\Settings";
            if (!File.Exists(path))
            {
                Save();
                return;
            }
            using (StreamReader stream = new StreamReader(path))
            {
                object[] obj = JsonConvert.DeserializeObject<object[]>(stream.ReadToEnd());
                _fileNameSprav = obj[0].ToString();
                _fileNameFullSprav = obj[1].ToString();
                _MKOandGST = Convert.ToBoolean(obj[2]);
                _limitCheckingFile = Convert.ToInt32(obj[3]);
            }
        }

        private void Save()
        {
            string path = Directory.GetCurrentDirectory() + "\\Settings";
            using (StreamWriter stream = new StreamWriter(path))
            {
                object[] obj = new object[4];
                obj[0] = _fileNameSprav;
                obj[1] = _fileNameFullSprav;
                obj[2] = _MKOandGST;
                obj[3] = _limitCheckingFile;
                stream.Write(JsonConvert.SerializeObject(obj));
            }
        }
    }
}
