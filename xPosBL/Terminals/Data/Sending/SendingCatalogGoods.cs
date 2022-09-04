using System;
using System.IO;
using System.Text;
using System.Threading;

namespace xPosBL.Terminals.Data.Sending
{
    public class SendingCatalogGoods : ISending
    {
        private Terminal _terminal;
        private int _limit = 0;
        private readonly CancellationToken _token;

        public event EventHandler<string> EventMessage;

        public SendingCatalogGoods(Terminal terminal, CancellationToken token)
        {
            _terminal = terminal;
            _token = token;
        }

        public void Send(string textAIn)
        {

            #region проверка отмены
            AdditionalFunctions.ThrowExceptionToken(_token);
            #endregion

            if (_limit == 6)
            {
                #region проверка отмены
                AdditionalFunctions.ThrowExceptionToken(_token);
                #endregion

                _limit = 0;
                #region проверка отмены
                AdditionalFunctions.ThrowExceptionToken(_token);
                #endregion

                return;
            }
            #region проверка отмены
            AdditionalFunctions.ThrowExceptionToken(_token);
            #endregion

            string path = _terminal.Path;
            #region проверка отмены
            AdditionalFunctions.ThrowExceptionToken(_token);
            #endregion

            string fileName = _terminal.Setting.FileNameSprav;
            #region проверка отмены
            AdditionalFunctions.ThrowExceptionToken(_token);
            #endregion

            if (Check(path))
            {
                #region проверка отмены
                AdditionalFunctions.ThrowExceptionToken(_token);
                #endregion

                Thread.Sleep(10000);

                #region проверка отмены
                AdditionalFunctions.ThrowExceptionToken(_token);
                #endregion

                _limit++;
                #region проверка отмены
                AdditionalFunctions.ThrowExceptionToken(_token);
                #endregion

                Send(textAIn);
                #region проверка отмены
                AdditionalFunctions.ThrowExceptionToken(_token);
                #endregion

                return;
            }
            #region проверка отмены
            AdditionalFunctions.ThrowExceptionToken(_token);
            #endregion

            //TODO Сделать рефактаринг
            if (File.Exists(path + fileName))
            {
                #region проверка отмены
                AdditionalFunctions.ThrowExceptionToken(_token);
                #endregion

                if (File.Exists(path + fileName + _terminal.HashFile))
                {
                    #region проверка отмены
                    AdditionalFunctions.ThrowExceptionToken(_token);
                    #endregion

                    File.Delete(path + fileName + _terminal.HashFile);
                }
                #region проверка отмены
                AdditionalFunctions.ThrowExceptionToken(_token);
                #endregion

                File.Move(path + fileName, path + fileName + _terminal.HashFile);
                #region проверка отмены
                AdditionalFunctions.ThrowExceptionToken(_token);
                    #endregion
            }
            #region проверка отмены
            AdditionalFunctions.ThrowExceptionToken(_token);
            #endregion

            File.WriteAllText(path + fileName + "T", textAIn);
            #region проверка отмены
            AdditionalFunctions.ThrowExceptionToken(_token);
            #endregion

            int HashFile = File.ReadAllText(path + fileName + "T").GetHashCode();
            #region проверка отмены
            AdditionalFunctions.ThrowExceptionToken(_token);
            #endregion

            if (_terminal.HashFile == HashFile)
            {
                try
                {

                    #region проверка отмены
                    AdditionalFunctions.ThrowExceptionToken(_token);
                    #endregion

                    File.Move(path + fileName + "T", path + fileName);
                    #region проверка отмены
                    AdditionalFunctions.ThrowExceptionToken(_token);
                    #endregion

                    File.WriteAllText(path + "sprav.txt", "");
                    #region проверка отмены
                    AdditionalFunctions.ThrowExceptionToken(_token);
                    #endregion

                    _terminal.State.ChangedState();
                    #region проверка отмены
                    AdditionalFunctions.ThrowExceptionToken(_token);
                    #endregion

                    EventMessage?.Invoke(_terminal, "Файл успешно создан.");
                    #region проверка отмены
                    AdditionalFunctions.ThrowExceptionToken(_token);
                    #endregion

                }
                catch(OperationCanceledException)
                {
                    if(File.Exists(path + fileName + "T"))
                    {
                        File.Delete(path + fileName + "T");
                    }
                    if(File.Exists(path + "sprav.txt"))
                    {
                        File.Delete(path + "sprav.txt");
                    }
                    File.Move(path + fileName + _terminal.HashFile, path + fileName);
                    throw;
                    #region проверка отмены
                    AdditionalFunctions.ThrowExceptionToken(_token);
                    #endregion
                }
            }
            else
            {
                #region проверка отмены
                AdditionalFunctions.ThrowExceptionToken(_token);
                #endregion

                File.Delete(path + fileName + "T");
                #region проверка отмены
                AdditionalFunctions.ThrowExceptionToken(_token);
                #endregion

                EventMessage?.Invoke( _terminal, "Произошла ошибка при отправке файла.\nПовторная отправка.");
                #region проверка отмены
                AdditionalFunctions.ThrowExceptionToken(_token);
                #endregion
                _limit++;

                Send(textAIn);
                #region проверка отмены
                AdditionalFunctions.ThrowExceptionToken(_token);
                #endregion
            }
        }

        private bool Check(string path)
        {
            return File.Exists(path + "sprav.txt"); 
        }
    }
}
