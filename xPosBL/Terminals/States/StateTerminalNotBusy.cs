using System;
using System.Data;
using System.Text;
using xPosBL.Terminals.Data.Checking;
using xPosBL.Terminals.Data.Sending;
using System.Threading;
using xPosBL.GoodsDirectories.CreateSprav;

namespace xPosBL.Terminals.States
{
    public class StateTerminalNotBusy : IStateTerminal
    {
        private readonly ISending _sending;
        private readonly Terminal _terminal;
        private readonly CancellationToken _token;
        private bool _procIsWorking;
        
        public DataTable GoodsUpdate {get; set; }

        public event EventHandler<string> EventMessage;

        public bool ProcIsWorking => _procIsWorking;

        public StateTerminalNotBusy(Terminal terminal, ISending sending, CancellationToken token)
        {
            _sending = sending;
            _terminal = terminal;
            _token = token;
        }

        public void ChangedState()
        {
            #region проверка отмены
            AdditionalFunctions.ThrowExceptionToken(_token);
            #endregion

            CheckingCatalogGoods check = new CheckingCatalogGoods(_terminal, _token);

            #region проверка отмены
            AdditionalFunctions.ThrowExceptionToken(_token);
            #endregion

            check.EventMessage += EventMessage;
            #region проверка отмены
            AdditionalFunctions.ThrowExceptionToken(_token);
            #endregion

            IStateTerminal state = new StateTerminalReadData(_terminal, check, _token);
            #region проверка отмены
            AdditionalFunctions.ThrowExceptionToken(_token);
            #endregion

            state.EventMessage += EventMessage;
            #region проверка отмены
            AdditionalFunctions.ThrowExceptionToken(_token);
            #endregion

            _terminal.State = state;
            #region проверка отмены
            AdditionalFunctions.ThrowExceptionToken(_token);
            #endregion

        }

        public void Execude()
        {
            _procIsWorking = true;
            if (GoodsUpdate.Rows.Count != 0)
            {
                EventMessage?.Invoke(_terminal, $"Формирование файла \"AIn\".");

                #region проверка отмены

                AdditionalFunctions.ThrowExceptionToken(_token);

                #endregion

                ICreateSprav<StringBuilder> sprav;
                if (_terminal.TypeId != 4)
                    sprav = new CreateCatalogGoodsRevaluation();
                else
                    sprav = new CreateCatalogGoodsRevaluationVVO();
                StringBuilder ainText = sprav.Create(_terminal.Setting.FileNameSprav, GoodsUpdate);
                #region проверка отмены

                AdditionalFunctions.ThrowExceptionToken(_token);

                #endregion

                EventMessage?.Invoke(_terminal, $"Оправка цен на кассу.");
                #region проверка отмены

                AdditionalFunctions.ThrowExceptionToken(_token);

                #endregion

                _terminal.HashFile = ainText.ToString().GetHashCode();
                #region проверка отмены

                AdditionalFunctions.ThrowExceptionToken(_token);

                #endregion

                _sending.Send(ainText.ToString());
                #region проверка отмены

                AdditionalFunctions.ThrowExceptionToken(_token);

                #endregion

                EventMessage?.Invoke(_terminal, $"Оправка цен на кассу - закончена.");

                #region проверка отмены

                AdditionalFunctions.ThrowExceptionToken(_token);

                #endregion
            }
            else
            {
                EventMessage?.Invoke(_terminal, "Товара для кассы нету.");
                #region проверка отмены

                AdditionalFunctions.ThrowExceptionToken(_token);

                #endregion
            }
            _procIsWorking = false;

            #region проверка отмены
            AdditionalFunctions.ThrowExceptionToken(_token);
            #endregion
        }

        public void ChangProcIsFalse()
        {
            _procIsWorking = false;
        }
    }
}
