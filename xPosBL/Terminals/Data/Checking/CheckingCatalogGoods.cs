using System;
using System.IO;
using System.Text;
using System.Threading;


namespace xPosBL.Terminals.Data.Checking
{
    public class CheckingCatalogGoods : ICheckingFile
    {
        private Terminal _terminal;
        private readonly int _limitProg;
        private int _limitNow = 0;
        private readonly CancellationToken _token;

        public event EventHandler<string> EventMessage;

        public CheckingCatalogGoods(Terminal terminal, CancellationToken token)
        {
            _terminal = terminal;
            _limitProg = terminal.Setting.LimitCheckingFile;
            _token = token;
        }

        public void Check()
        {
            #region проверка отмены
            AdditionalFunctions.ThrowExceptionToken(_token);
            #endregion

            if (_limitNow == _limitProg)
            {
                #region проверка отмены
                AdditionalFunctions.ThrowExceptionToken(_token);
                #endregion

                _limitNow = 0;
                #region проверка отмены
                AdditionalFunctions.ThrowExceptionToken(_token);
                #endregion

                _terminal.State.ChangedState();
                #region проверка отмены
                AdditionalFunctions.ThrowExceptionToken(_token);
                #endregion

                EventMessage(_terminal, "Лимит повторов проверки файла исчерпан.");
                #region проверка отмены
                AdditionalFunctions.ThrowExceptionToken(_token);
                #endregion

                return;
            }
            #region проверка отмены
            AdditionalFunctions.ThrowExceptionToken(_token);
            #endregion

            if (File.Exists(_terminal.Path + "sprav.txt"))
            {
                #region проверка отмены
                AdditionalFunctions.ThrowExceptionToken(_token);
                #endregion

                for (int i = 0; i < 12; i++)
                {
                    #region проверка отмены
                    AdditionalFunctions.ThrowExceptionToken(_token);
                    #endregion

                    Thread.Sleep(10000);
                }
                _limitNow++;
                EventMessage(_terminal, "В папке AIn, файл sprav.txt еще не удален!");
                #region проверка отмены
                AdditionalFunctions.ThrowExceptionToken(_token);
                #endregion
                //TODO Провести рефактаринг
                Check();
                #region проверка отмены
                AdditionalFunctions.ThrowExceptionToken(_token);
                #endregion

                return;
            }
            #region проверка отмены
            AdditionalFunctions.ThrowExceptionToken(_token);
            #endregion

            StringBuilder strFile = new StringBuilder();
            #region проверка отмены
            AdditionalFunctions.ThrowExceptionToken(_token);
            #endregion

            strFile.Clear().Append(File.ReadAllText(_terminal.Path + _terminal.Setting.FileNameSprav));
            #region проверка отмены
            AdditionalFunctions.ThrowExceptionToken(_token);
            #endregion

            if (strFile[8] == '#')
            {
                #region проверка отмены
                AdditionalFunctions.ThrowExceptionToken(_token);
                #endregion

                for (int i = 0; i < 12; i++)
                {
                    #region проверка отмены
                    AdditionalFunctions.ThrowExceptionToken(_token);
                    #endregion

                    Thread.Sleep(10000);
                }
                _limitNow++; //TODO Провести рефактаринг
                EventMessage(_terminal, "В папке AIn, файл AIn еще не прочитался!");
                #region проверка отмены
                AdditionalFunctions.ThrowExceptionToken(_token);
                #endregion

                Check();
                #region проверка отмены
                AdditionalFunctions.ThrowExceptionToken(_token);
                #endregion

                return;
            }
            #region проверка отмены
            AdditionalFunctions.ThrowExceptionToken(_token);
            #endregion

            strFile[8] = '#';
            #region проверка отмены
            AdditionalFunctions.ThrowExceptionToken(_token);
            #endregion

            string textTest = strFile.ToString();
            #region проверка отмены
            AdditionalFunctions.ThrowExceptionToken(_token);
            #endregion

            int hash = textTest.GetHashCode();
            #region проверка отмены
            AdditionalFunctions.ThrowExceptionToken(_token);
            #endregion

            if (_terminal.HashFile == hash)
            {
                #region проверка отмены
                AdditionalFunctions.ThrowExceptionToken(_token);
                #endregion

                EventMessage?.Invoke(_terminal, "Файл на кассу дошел без ошибок.");
                #region проверка отмены
                AdditionalFunctions.ThrowExceptionToken(_token);
                #endregion

                string pathLR = _terminal.Path;
                #region проверка отмены
                AdditionalFunctions.ThrowExceptionToken(_token);
                #endregion

                pathLR = pathLR.Remove(pathLR.Length - 5, 4) + $"AOUt\\LoadResult{_terminal.Number}.txt";
                #region проверка отмены
                AdditionalFunctions.ThrowExceptionToken(_token);
                #endregion

                string result = "";
            #region проверка отмены
                AdditionalFunctions.ThrowExceptionToken(_token);
                #endregion

                using (StreamReader stream = new StreamReader(pathLR))
                {
                    #region проверка отмены
                    AdditionalFunctions.ThrowExceptionToken(_token);
                    #endregion

                    stream.ReadLine();
                    stream.ReadLine();
                    #region проверка отмены
                    AdditionalFunctions.ThrowExceptionToken(_token);
                    #endregion

                    while (!stream.EndOfStream)
                    {
                        #region проверка отмены
                        AdditionalFunctions.ThrowExceptionToken(_token);
                        #endregion

                        result += stream.ReadLine();
                    }
                    #region проверка отмены
                    AdditionalFunctions.ThrowExceptionToken(_token);
                    #endregion

                }
                #region проверка отмены
                AdditionalFunctions.ThrowExceptionToken(_token);
                #endregion

                if (result == "Ok")
                {
                    #region проверка отмены
                    AdditionalFunctions.ThrowExceptionToken(_token);
                    #endregion

                    EventMessage?.Invoke(_terminal, "Товар успешно отправлен.");
                }
                else
                {
                    #region проверка отмены
                    AdditionalFunctions.ThrowExceptionToken(_token);
                    #endregion

                    string path = Directory.GetCurrentDirectory();
                    if (!Directory.Exists(path + "\\LoadResult"))
                        Directory.CreateDirectory(path + "\\LoadResult");
                    if (!Directory.Exists(path + "\\LoadResult\\Terminal_" + _terminal.Number))
                        Directory.CreateDirectory(path + "\\LoadResult\\Terminal_" + _terminal.Number);
                    File.Copy(pathLR, path + $"\\LoadResult\\Terminal_{_terminal.Number} " +
                                      $"\\LoadResult_{DateTime.Now.Day}_{DateTime.Now.Month}_" +
                                      $"{DateTime.Now.Hour}-{DateTime.Now.Minute}-{DateTime.Now.Second}.txt");
                    EventMessage?.Invoke(_terminal, "Товар прочитался с ошибкой.");
                    EventMessage?.Invoke(_terminal, "Текст ошибки: " + result);
                }
            }
            else
            {
                #region проверка отмены
                AdditionalFunctions.ThrowExceptionToken(_token);
                #endregion

                EventMessage?.Invoke(_terminal, "Файл не полностью был записан на кассу.");
            }
            #region проверка отмены
            AdditionalFunctions.ThrowExceptionToken(_token);
            #endregion

            _terminal.State.ChangedState();
            #region проверка отмены
            AdditionalFunctions.ThrowExceptionToken(_token);
            #endregion

        }
    }
}
