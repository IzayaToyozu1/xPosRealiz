using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Data;
using System.Threading;
using xPosBL.Terminals.Controls;
using xPosBL.Terminals.Data;
using xPosBL.Timer;
using xPosBL.GoodsDirectories.GetGoods;
using xPosBL.Terminals;
using xPosBL.Terminals.States;
using xPosBL.GoodsDirectories;

namespace xPosBL
{
    public class XPos
    {
        private readonly object locking = new object();
        private readonly object locking2 = new object();

        private readonly TimerTest _timer;
        private readonly ControlerTerminal _terminalControl;
        private readonly DataGoodsDB _dataGoods;
        private readonly SaveLoadTerminalData _saveLoad;
        private readonly CatalogGoods _catGoods;
        private DataTable _dataGoodsUpdate;
        private DataTable _dataGoodsUpdateVvo;
        private readonly GetGoodsFromServer _getGoods;
        private Dictionary<string, Action<object>> _methEventTerminal;
        private readonly SettingsSprav _settingsSprav;
        private long _lastIdGoods;
        private CancellationTokenSource _cancellataionTokSour;
        private CancellationToken _token;
        private readonly Queue<string> _logApp;
        private bool _isNotUpdateTableGoods = false;

        public EventHandler ChangedTerminal;

        public event EventHandler<string> EventNotification;
        public event EventHandler<string> EventTimerTick;

        public event EventHandler StartLoadData;
        public event EventHandler StopLoadData;
        public event EventHandler ChangedTerminals;
        public event EventHandler ChangedDataGoodsUpdates;

        public TimerStop SubTimerStop;
        public TimerResum SubTimerResum;

        public SettingXPOS SettingXpos => _terminalControl.Settings;
        public SettingsSprav SettingsSprav => _catGoods.SettingsSprav;
        public DataTable Terminals => _terminalControl.Terminals;
        public DataTable DataGoodsUpdates => _dataGoodsUpdate?.Copy();
        public DataTable DataGoodsUpdatesVvo => _dataGoodsUpdateVvo?.Copy();

        public XPos()
        {
            _saveLoad = new SaveLoadTerminalData();
            _dataGoods = new DataGoodsDB(_saveLoad);
            _terminalControl = new ControlerTerminal(_dataGoods, _token);
            _getGoods = new GetGoodsFromServer();
            _settingsSprav = new SettingsSprav();
            _catGoods = new CatalogGoods(_terminalControl.Settings.FileNameFullSprav, _settingsSprav);
            _timer = new TimerTest();
            _logApp = new Queue<string>();
        }

        public void CreateFullCatalogGoods(bool terminalWork)
        {
            _catGoods.CreateFullSprav(terminalWork);
        }

        public void TimerStart()
        {
            Initialize();
        }

        public void TimerStop()
        {
            _timer.Stop();
            if(_cancellataionTokSour != null)
                _cancellataionTokSour.Cancel();
        }

        public void TimerResum()
        {
            _timer.Resume();
            if (_cancellataionTokSour != null)
            {
                _cancellataionTokSour.Dispose();
                _cancellataionTokSour = null;
            }
        }

        public void SetDefaultTimer(int seconds)
        {
            _timer.DefaultTimer = seconds;
            _timer.UpdateTimer();
            SQL.setSettings("grpt", seconds.ToString());
        }

        public long GetDefaultTimer()
        {
            return _timer.DefaultTimer;
        }

        public TerminalType[] GetTerminalType()
        {
            return _terminalControl.Type;
        }

        public void FindTerminalsType(int id)
        {
            _terminalControl.SetTerminalFilter(id);
        }

        public void SubEventsTimer(TimerStop timerStop, TimerResum timerResum)
        {
            _timer.EventTimerStop += timerStop;
            _timer.EventTimerResum += timerResum;
        }

        private void Initialize()
        {
            _methEventTerminal = new Dictionary<string, Action<object>>
            {
                ["Произошла ошибка при отправке файла."] = ChangedGoodsPrepared, //SendingCatalogGoods
                ["Лимит повторов проверки файла исчерпан."] = ChangedGoodsPrepared, //CheckingCatalogGoods
                ["Файл не в формате xPos."] = ChangedGoodsPrepared, //CheckingCatalogGoods
                ["Товар успешно отправлен."] = ChangedGoodsDone, //
                ["Товар прочитался с ошибкой."] = ChangedGoodsDone, //CheckingCatalogGoods
                ["Файл не полностью был записан на кассе."] = ChangedGoodsPrepared, //CheckingCatalogGoods
                ["Товара для кассы нету."] = ChangedDataGoodsInDone
            };
            //TimerTest
            _timer.EventTimerTickSecond += TimerTick;
            _timer.EventTimerWorked += TimerWorked;
            _timer.DefaultTimer = long.Parse(SQL.getSettings("grpt").Rows[0]["value"].ToString());
            _timer.UpdateTimer();
            _timer.EventTimerStop += SubTimerStop;
            _timer.EventTimerResum += SubTimerResum;

            //terminalControl
            _terminalControl.Message += EventMessageTerminal;
            _terminalControl.UpdateTerminalWorking();
            _terminalControl.ChangedTerminals += ChangedTerminal;


            

            //CatalogGoods
            _catGoods.EventMessage += EventMessageTerminal;
            Task.Run(() =>
            {
                EventNotification?.Invoke(this, "Загрузка данных.");
                StartLoadData?.Invoke(this, EventArgs.Empty);
                _dataGoodsUpdate = _getGoods.GetGoods();
                _dataGoodsUpdateVvo = _getGoods.GetGoodsVVO();
                if (_dataGoodsUpdate != null)
                {
                    _dataGoodsUpdate.DefaultView.Sort = "id_goodsUpdate desc";
                }
                if (_dataGoodsUpdateVvo != null)
                {
                    _dataGoodsUpdateVvo.DefaultView.Sort = "id_goodsUpdate desc";
                }
                foreach (var term in _terminalControl.TerminalWork)
                {
                    AddTerminalWork(term);
                }
                GC.Collect();
                StopLoadData?.Invoke(this, EventArgs.Empty);
                EventNotification?.Invoke(this, "Загрузка данных - завершена.");
                _timer.Start();
            }, _token);
            _lastIdGoods = SQL.getLastIdSprav();
            SQL.setLastId();
            _terminalControl.UpdateTerminals();
            _terminalControl.Terminals.RowChanged += ChangedTerminalControl;
        }

        private void UpdateTableGoods()
        {
            if (_isNotUpdateTableGoods)
            {
                return;
            }

            try
            {
                #region проверка отмены

                AdditionalFunctions.ThrowExceptionToken(_token);

                #endregion

                _isNotUpdateTableGoods = true;
                DataTable dt;

                #region проверка отмены

                AdditionalFunctions.ThrowExceptionToken(_token);

                #endregion#region проверка отмены

                if (_dataGoodsUpdate != null)
                {
                    #region проверка отмены

                    AdditionalFunctions.ThrowExceptionToken(_token);

                    #endregion

                    _dataGoodsUpdate.DefaultView.Sort = "id_goodsUpdate desc";
                    _dataGoodsUpdate.DefaultView.RowFilter = "";
                    int lastIdGoodsUpLocal = Convert.ToInt32(_dataGoodsUpdate.DefaultView[0]["id_goodsUpdate"]);
                    dt = _getGoods.GetGoods(lastIdGoodsUpLocal);

                    #region проверка отмены

                    AdditionalFunctions.ThrowExceptionToken(_token);

                    #endregion

                    int idLastTerm = _getGoods.GetLastIdGoodTerminal();

                    #region проверка отмены

                    AdditionalFunctions.ThrowExceptionToken(_token);

                    #endregion

                    if (dt != null)
                    {
                        #region проверка отмены

                        AdditionalFunctions.ThrowExceptionToken(_token);

                        #endregion

                        dt.DefaultView.Sort = "id_goodsUpdate desc";

                        #region проверка отмены

                        AdditionalFunctions.ThrowExceptionToken(_token);

                        #endregion

                        foreach (DataRow row in dt.Rows)
                        {
                            #region проверка отмены

                            AdditionalFunctions.ThrowExceptionToken(_token);

                            #endregion

                            _dataGoodsUpdate.Rows.Add(row.ItemArray);
                        }
                    }

                    _dataGoodsUpdate.DefaultView.Sort = "id_goodsUpdate desc";

                    #region проверка отмены

                    AdditionalFunctions.ThrowExceptionToken(_token);

                    #endregion

                    _dataGoodsUpdate.DefaultView.RowFilter = "";

                    #region проверка отмены

                    AdditionalFunctions.ThrowExceptionToken(_token);

                    #endregion

                    if (idLastTerm >
                        Convert.ToInt32(
                            _dataGoodsUpdate.DefaultView[_dataGoodsUpdate.DefaultView.Count - 1]["id_goodsUpdate"]))
                        ;
                    {
                        #region проверка отмены

                        AdditionalFunctions.ThrowExceptionToken(_token);

                        #endregion

                        _dataGoodsUpdate.DefaultView.RowFilter = "id_goodsUpdate <= " + idLastTerm;

                        #region проверка отмены

                        AdditionalFunctions.ThrowExceptionToken(_token);

                        #endregion

                        Queue<DataRow> RowsDelete = new Queue<DataRow>();

                        #region проверка отмены

                        AdditionalFunctions.ThrowExceptionToken(_token);

                        #endregion

                        foreach (DataRowView row in _dataGoodsUpdate.DefaultView)
                        {
                            #region проверка отмены

                            AdditionalFunctions.ThrowExceptionToken(_token);

                            #endregion

                            RowsDelete.Enqueue(row.Row);
                        }

                        foreach (var row in RowsDelete)
                        {
                            #region проверка отмены

                            AdditionalFunctions.ThrowExceptionToken(_token);

                            #endregion

                            _dataGoodsUpdate.Rows.Remove(row);
                        }
                    }

                    #region проверка отмены

                    AdditionalFunctions.ThrowExceptionToken(_token);

                    #endregion

                    _dataGoodsUpdate.DefaultView.RowFilter = "";
                    _dataGoodsUpdate.DefaultView.Sort = "id_goodsUpdate desc";

                    #region проверка отмены

                    AdditionalFunctions.ThrowExceptionToken(_token);

                    #endregion
                }

                if (_dataGoodsUpdateVvo != null && _lastIdGoods > Convert.ToInt64(
                        _dataGoodsUpdateVvo.DefaultView[_dataGoodsUpdateVvo.DefaultView.Count - 1]
                            ["id_goodsUpdate"]))
                {
                    #region проверка отмены

                    AdditionalFunctions.ThrowExceptionToken(_token);

                    #endregion

                    dt = _getGoods.GetGoodsVVO(Convert.ToInt32(_lastIdGoods));

                    #region проверка отмены

                    AdditionalFunctions.ThrowExceptionToken(_token);

                    #endregion

                    int idLastTerm = _getGoods.GetLastIdGoodTerminal();

                    #region проверка отмены

                    AdditionalFunctions.ThrowExceptionToken(_token);

                    #endregion

                    if (dt != null)
                    {
                        #region проверка отмены

                        AdditionalFunctions.ThrowExceptionToken(_token);

                        #endregion

                        dt.DefaultView.Sort = "id_goodsUpdate desc";

                        #region проверка отмены

                        AdditionalFunctions.ThrowExceptionToken(_token);

                        #endregion

                        foreach (DataRow row in dt.Rows)
                        {
                            #region проверка отмены

                            AdditionalFunctions.ThrowExceptionToken(_token);

                            #endregion

                            _dataGoodsUpdateVvo.Rows.Add(row.ItemArray);
                        }
                    }

                    _dataGoodsUpdateVvo.DefaultView.Sort = "id_goodsUpdate desc";

                    #region проверка отмены

                    AdditionalFunctions.ThrowExceptionToken(_token);

                    #endregion

                    if (idLastTerm >
                        Convert.ToInt32(
                            _dataGoodsUpdateVvo.DefaultView[_dataGoodsUpdateVvo.DefaultView.Count - 1][
                                "id_goodsUpdate"])) ;
                    {
                        #region проверка отмены

                        AdditionalFunctions.ThrowExceptionToken(_token);

                        #endregion

                        _dataGoodsUpdateVvo.DefaultView.RowFilter = "id_goodsUpdate <= " + idLastTerm;

                        #region проверка отмены

                        AdditionalFunctions.ThrowExceptionToken(_token);

                        #endregion

                        for (int i = 0; i < _dataGoodsUpdateVvo.DefaultView.Count; i++)
                        {
                            #region проверка отмены

                            AdditionalFunctions.ThrowExceptionToken(_token);

                            #endregion

                            _dataGoodsUpdateVvo.DefaultView.Delete(i);
                        }
                    }

                    #region проверка отмены

                    AdditionalFunctions.ThrowExceptionToken(_token);

                    #endregion

                    _dataGoodsUpdateVvo.DefaultView.RowFilter = "";
                    _dataGoodsUpdateVvo.DefaultView.Sort = "id_goodsUpdate desc";
                }

                ChangedDataGoodsUpdates?.Invoke(new object(), EventArgs.Empty);

                _lastIdGoods = SQL.getLastIdSprav();

                #region проверка отмены

                AdditionalFunctions.ThrowExceptionToken(_token);

                #endregion

                GC.Collect();
                _isNotUpdateTableGoods = false;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void TimerWorked(object sender, TimerWorkedEventArgs e)
        {
            _cancellataionTokSour = new CancellationTokenSource();
            _token = _cancellataionTokSour.Token;
            Task.Factory.StartNew((Action)(() =>
            {
                try
                {
                    EventNotification?.Invoke(this, "Отчет таймера закончен!");
                    #region проверка отмены
                    AdditionalFunctions.ThrowExceptionToken(_token);
                    #endregion

                    var termWork = _terminalControl.TerminalWork.ToArray();
                    #region проверка отмены
                    AdditionalFunctions.ThrowExceptionToken(_token);
                    #endregion

                    Task[] tasks = new Task[termWork.Length];
                    #region проверка отмены
                    AdditionalFunctions.ThrowExceptionToken(_token);
                    #endregion

                    int i = 0;
                    #region проверка отмены
                    AdditionalFunctions.ThrowExceptionToken(_token);
                    #endregion

                    foreach (var term in termWork)
                    {
                        #region Проверка отмены
                        AdditionalFunctions.ThrowExceptionToken(_token);
                        #endregion

                        Task task = new Task((Action)(() =>
                        {
                            try
                            {
                                AdditionalFunctions.ThrowExceptionToken(_token);
                                if (term.Worker)
                                {
                                    #region проверка отмены
                                    AdditionalFunctions.ThrowExceptionToken(_token);
                                    #endregion

                                    EventMessageTerminal(term, $"В данный момент проходит проверка или оправка цен.");
                                    #region
                                    AdditionalFunctions.ThrowExceptionToken(_token);
                                    #endregion

                                    return;
                                }
                                #region проверка отмены
                                AdditionalFunctions.ThrowExceptionToken(_token);
                                #endregion

                                term.Worker = true;

                                if (term.State is StateTerminalNotBusy state)
                                {
                                    DataTable d;
                                    #region проверка отмены
                                    AdditionalFunctions.ThrowExceptionToken(_token);
                                    #endregion
                                    if (term.TypeId != 4)
                                    {
                                        d = _dataGoodsUpdate.Copy();
                                        #region проверка отмены
                                        AdditionalFunctions.ThrowExceptionToken(_token);
                                        #endregion
                                    }
                                    else
                                    {
                                        d = _dataGoodsUpdateVvo.Copy();
                                        #region проверка отмены
                                        AdditionalFunctions.ThrowExceptionToken(_token);
                                        #endregion
                                    }
                                    #region проверка отмены
                                    AdditionalFunctions.ThrowExceptionToken(_token);
                                    #endregion

                                    if (term.TypeId != 4)
                                        DeleteColumsTerminal(d, _dataGoodsUpdate);
                                    else
                                        DeleteColumsTerminal(d, _dataGoodsUpdateVvo);
                                    #region проверка отмены
                                    AdditionalFunctions.ThrowExceptionToken(_token);
                                    #endregion

                                    d = _catGoods.GetGoods(d, term.TypeId, term.IdGU);
                                    #region проверка отмены
                                    AdditionalFunctions.ThrowExceptionToken(_token);
                                    #endregion
                                    state.GoodsUpdate = d;
                                    #region проверка отмены
                                    AdditionalFunctions.ThrowExceptionToken(_token);
                                    #endregion

                                    if (d != null && d.Rows.Count != 0)
                                    {
                                        #region проверка отмены
                                        AdditionalFunctions.ThrowExceptionToken(_token);
                                        #endregion

                                        d.DefaultView.Sort = "id_goodsUpdate desc";
                                        int lastIdGoods = Convert.ToInt32(d.DefaultView[0]["id_goodsUpdate"]);
                                        #region проверка отмены
                                        AdditionalFunctions.ThrowExceptionToken(_token);
                                        #endregion

                                        AddGoods(term, lastIdGoods);
                                        #region проверка отмены
                                        AdditionalFunctions.ThrowExceptionToken(_token);
                                        #endregion
                                    }
                                    #region проверка отмены
                                    AdditionalFunctions.ThrowExceptionToken(_token);
                                    #endregion
                                }
                                #region проверка отмены
                                AdditionalFunctions.ThrowExceptionToken(_token);
                                #endregion

                                term.State.Execude();
                                #region проверка отмены
                                AdditionalFunctions.ThrowExceptionToken(_token);
                                #endregion

                                term.Worker = false;
                                GC.Collect();
                            }
                            catch (OperationCanceledException)
                            {
                                term.Worker = false;
                                term.State.ChangProcIsFalse();
                                EventMessageTerminal(term, "Процесс отправки товаров прерван!"); 
                            }
                            catch (Exception exc)
                            {
                                term.Worker = false;
                                term.State.ChangProcIsFalse();
                                EventMessageTerminal(term, "Произошла ошибка: " + exc.Message + "\n|" + exc.StackTrace +"|");
                            }
                        }), _token);
                        #region проверка отмены
                        AdditionalFunctions.ThrowExceptionToken(_token);
                        #endregion

                        task.Start();
                        #region проверка отмены
                        AdditionalFunctions.ThrowExceptionToken(_token);
                        #endregion

                        tasks[i++] = task;
                        #region проверка отмены
                        AdditionalFunctions.ThrowExceptionToken(_token);
                        #endregion
                    }
                    #region проверка отмены
                    AdditionalFunctions.ThrowExceptionToken(_token);
                    #endregion

                    Task.WaitAll();
                    #region проверка отмены
                    AdditionalFunctions.ThrowExceptionToken(_token);
                    #endregion

                    UpdateTableGoods();
                    #region проверка отмены
                    AdditionalFunctions.ThrowExceptionToken(_token);
                    #endregion
                }
                catch (OperationCanceledException operationExc)
                {
                    EventMessageTerminal(new object(), "Процесс отправки товаров прерван!");
                    _isNotUpdateTableGoods = false;
                }
                catch (Exception exc)
                {
                    EventMessageTerminal(new object(), "Произошла следующая ошибка: " + exc.Message + "\n" + exc.StackTrace);
                    _isNotUpdateTableGoods = false;
                }
                
            }), _token);
        }

        private void DeleteColumsTerminal(DataTable tCopy, DataTable goodsUpdate)
        {
            foreach (DataColumn column in goodsUpdate.Columns)
            {
                if (column.ColumnName.Contains("Terminal"))
                    tCopy.Columns.Remove(column.ColumnName);
            }
        }

        private void TimerTick(object sender, TimerTickSecondEventArgs e)
        {
            EventTimerTick?.Invoke(this, string.Format("До обновления: {0:d2}:{1:d2}:{2:d2}", _timer.Time.Hours, _timer.Time.Minutes, _timer.Time.Seconds));
        }

        private void AddTerminalWork(Terminal term)
        {
            if (term.TypeId == 4 && _dataGoodsUpdateVvo != null)
                DataGoods(_dataGoodsUpdateVvo);
            else if (term.TypeId != 4 && _dataGoodsUpdate != null)
                DataGoods(_dataGoodsUpdate);
            void DataGoods(DataTable dt)
            {
               
                if (!dt.Columns.Contains($"Terminal_{term.Number}"))
                    dt.Columns.Add($"Terminal_{term.Number}", typeof(string));
            }
        }

        private void EventMessageTerminal(object sender, string message)
        {
            lock (locking)
            {
                var t = sender as Terminal;
                if (_methEventTerminal.ContainsKey(message))
                    _methEventTerminal[message]?.Invoke(sender);
                if (!Directory.Exists(Directory.GetCurrentDirectory() + "\\Loging"))
                    Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\Loging");
                string pathLog = Directory.GetCurrentDirectory() + "\\Loging\\Log" + DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + ".txt";
                bool isNotReadFile = true;
                try
                {
                    FileStream stream = File.OpenRead(pathLog);
                    stream.Close();
                    stream.Dispose();
                }
                catch
                {
                    isNotReadFile = false;
                }
                if (_logApp.Count != 0 && isNotReadFile) 
                {
                    int count = _logApp.Count;
                    for(int i = 0; i < count; i++)
                    {
                        File.AppendAllText(pathLog, _logApp.Dequeue() + Environment.NewLine);
                    }
                }
                if (!File.Exists(pathLog))
                {
                    FileStream str = File.Create(pathLog);
                    str.Close();
                }
                string time = DateTime.Now.ToString("[HH:mm:ss]");
                if (t != null)
                {
                    EventNotification?.Invoke(t, $"Касса {t.Number}: " + message);
                    string logText = time + $" Касса {t.Number}: " + message;
                    if(isNotReadFile)
                        File.AppendAllText(pathLog, logText + Environment.NewLine);
                    else
                        _logApp.Enqueue(logText);
                    _terminalControl.UpdateTerminals();
                    _terminalControl.Terminals.RowChanged += ChangedTerminalControl;
                }
                else
                {
                    EventNotification?.Invoke(sender, message); 
                    string logText = time + " " + message;
                    if(isNotReadFile)
                        File.AppendAllText(pathLog, logText + Environment.NewLine);
                    else
                        _logApp.Enqueue(logText);
                }
            }
        }

        private void ChangedTerminalControl(object sender, EventArgs e)
        {
            foreach (var term in _terminalControl.TerminalWork)
            {
                AddTerminalWork(term);
            }
            if (_dataGoodsUpdate == null) return;
            foreach (DataRow term in _terminalControl.Terminals.Rows)
            {
                bool isHere = false;
                foreach (var t in _terminalControl.TerminalWork)
                {
                    if (Convert.ToInt32(term["Number"]) == t.Number)
                    {
                        isHere = true;
                        break;
                    }
                }
                if (isHere)
                    continue;
                if (Convert.ToInt32(term["id_TerminalType"]) != 4 && _dataGoodsUpdate.Columns.Contains("Terminal_" + term["Number"])) 
                {
                    DataColumn col = _dataGoodsUpdate.Columns["Terminal_" + term["Number"]];
                    _dataGoodsUpdate.Columns.Remove(col);
                }
            }
            if (_dataGoodsUpdateVvo == null) return;
            foreach (DataRow term in _terminalControl.Terminals.Rows)
            {
                bool isHere = false;
                foreach (var t in _terminalControl.TerminalWork)
                {
                    if (Convert.ToInt32(term["Number"]) == t.Number)
                    {
                        isHere = true;
                        break;
                    }
                }
                if (isHere)
                    continue;
                if (Convert.ToInt32(term["id_TerminalType"]) == 4 && _dataGoodsUpdateVvo.Columns.Contains("Terminal_" + term["Number"]))
                {
                    DataColumn col = _dataGoodsUpdateVvo.Columns["Terminal_" + term["Number"]];
                    _dataGoodsUpdateVvo.Columns.Remove(col);
                }
            }
            ChangedTerminals?.Invoke(sender, e);
        }

        private void AddGoods(Terminal term, int lastIdGoodsTerm)
        {
            lock(locking)
            {
                term.LastIdGU = lastIdGoodsTerm;
                if (term.TypeId != 4)
                    AddGoodsDT(_dataGoodsUpdate, term, lastIdGoodsTerm);
                else
                    AddGoodsDT(_dataGoodsUpdateVvo, term, lastIdGoodsTerm);
            }
        }

        private void AddGoodsDT(DataTable goods, Terminal term, int lastIdGoodsTerm)
        {
            goods.DefaultView.RowFilter = "id_goodsUpdate > " + term.IdGU + " and id_goodsUpdate <= " + lastIdGoodsTerm;
            foreach(DataRowView row in goods.DefaultView)
            {
                row.Row["Terminal_" + term.Number] = "Send";
            }
            ChangedDataGoodsUpdates?.Invoke(new object(), EventArgs.Empty);
        }

        private void ChangedGoodsPrepared(object obj)
        {
            Terminal term = obj as Terminal;
            if (term.TypeId != 4)
                ChangedDataGoods(_dataGoodsUpdate, term, "Prepared");
            else
                ChangedDataGoods(_dataGoodsUpdateVvo, term, "Prepared");
            SQL.setLastId();
        }

        private void ChangedGoodsDone(object obj)
        {
            Terminal term = obj as Terminal;
            if (term.TypeId != 4 && _dataGoodsUpdate != null)
            {
                ChangedDataGoods(_dataGoodsUpdate, term, "Done");
                _dataGoodsUpdate.DefaultView.RowFilter = "";
            }
            else if(_dataGoodsUpdateVvo != null)
            {
                ChangedDataGoods(_dataGoodsUpdateVvo, term, "Done");//TODO
                _dataGoodsUpdateVvo.DefaultView.RowFilter = "";
            }
            SQL.setLastId(term.Number, term.LastIdGU);
            _terminalControl.UpdateTerminals();
            _terminalControl.Terminals.RowChanged += ChangedTerminalControl;
        }

        private void ChangedDataGoodsInDone(object obj)
        {
            Terminal term = obj as Terminal;
            DataTable goods;
            if (term.TypeId != 4)
                goods= _dataGoodsUpdate;
            else
            {
                goods = _dataGoodsUpdateVvo;
            }
            goods.DefaultView.RowFilter = "id_goodsUpdate <= " + term.IdGU;//TODO
            if(goods.Columns.Contains("Terminal_"+term.Number))
                foreach (DataRowView row in goods.DefaultView)
                {
                    row.Row["Terminal_"+term.Number] = "Done";
                }

            goods.DefaultView.RowFilter = "";
            ChangedDataGoodsUpdates?.Invoke(new object(), EventArgs.Empty);
        }

        private void ChangedDataGoods(DataTable goods, Terminal term, string statusGoods)
        {
            goods.DefaultView.RowFilter = "Terminal_" + term.Number + " = 'send' " +
                                          "or Terminal_" + term.Number + " = 'Prepared'";
            if (goods.Columns.Contains("Terminal_"+term.Number))
                foreach (DataRowView row in goods.DefaultView)
                {
                    row.Row["Terminal_" + term.Number] = statusGoods;
                    if (statusGoods == "Prepared")
                        SQL.setJSprav(term.Number, Convert.ToInt32(row.Row["id_tovar"]));
                }
            goods.DefaultView.RowFilter = "";
            ChangedDataGoodsUpdates?.Invoke(new object(), EventArgs.Empty);
        }
    
        public void SubCreateEndSpr(EventHandler startCreatSpr, EventHandler endCreatSpr)
        {
            _catGoods.StartCreateSprav += startCreatSpr;
            _catGoods.EndCreateSprav += endCreatSpr;
        }

        public void ChangedChoiceTerminal(int numTerm)
        {
            _terminalControl.ChangedTerminalChoice(numTerm);
        }

        public void ChangedAllTermChoice()
        {
            _terminalControl.ChangedAllTerminalChoice();
        }
    }
}
