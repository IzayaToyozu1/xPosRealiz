using Nwuram.Framework.Project;
using Nwuram.Framework.Settings.Connection;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using xPosBL;

namespace xPosRealiz
{
    static class Program
    {
        /// <summary>e
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            string line;
            StreamReader file = new StreamReader(Application.StartupPath + @"\config.ini");
            String[] words = { };
            while ((line = file.ReadLine()) != null)
            {
                words = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (words.Count() > 0)
                    break;
            }
            file.Close();

            if (words.Count() > 0)
                Project.FillSettings(words);
            Config.hCntMain = new Procedures(ConnectionSettings.GetServer(), ConnectionSettings.GetDatabase(), ConnectionSettings.GetUsername(), ConnectionSettings.GetPassword(), ConnectionSettings.ProgramName);
            Config.hCntMainKassRealiz = new Procedures(ConnectionSettings.GetServer("2"), ConnectionSettings.GetDatabase("2"), ConnectionSettings.GetUsername(), ConnectionSettings.GetPassword(), ConnectionSettings.ProgramName);
            Config.hCntSecond = new Procedures(ConnectionSettings.GetServer("3"), ConnectionSettings.GetDatabase("3"), ConnectionSettings.GetUsername(), ConnectionSettings.GetPassword(), ConnectionSettings.ProgramName);

            Application.Run(new MainForm());
        }
    }
}
