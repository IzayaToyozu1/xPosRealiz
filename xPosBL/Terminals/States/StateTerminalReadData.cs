using System;
using System.Threading;
using xPosBL.Terminals.Data.Checking;
using xPosBL.Terminals.Data.Sending;

namespace xPosBL.Terminals.States
{
    public class StateTerminalReadData : IStateTerminal
    {
        private Terminal _terminal;
        private bool _procIsWorking;
        private readonly CancellationToken _token;

        public event EventHandler<string> EventMessage;

        public bool ProcIsWorking => _procIsWorking;

        public ICheckingFile Check { get; set; }

        public StateTerminalReadData(Terminal terminal, ICheckingFile check, CancellationToken token)
        {
            Check = check;
            _terminal = terminal;
            _token = token;
        }

        public void ChangedState()
        {
            #region проверка отмены
            AdditionalFunctions.ThrowExceptionToken(_token);
            #endregion

            SendingCatalogGoods sending = new SendingCatalogGoods(_terminal, _token);
            #region проверка отмены
            AdditionalFunctions.ThrowExceptionToken(_token);
            #endregion

            sending.EventMessage += EventMessage;
            #region проверка отмены
            AdditionalFunctions.ThrowExceptionToken(_token);
            #endregion

            IStateTerminal state = new StateTerminalNotBusy(_terminal, sending, _token);
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
            #region проверка отмены
            AdditionalFunctions.ThrowExceptionToken(_token);
            #endregion

            _procIsWorking = true;
            #region проверка отмены
            AdditionalFunctions.ThrowExceptionToken(_token);
            #endregion

            EventMessage?.Invoke(_terminal, $"Проверка отправки товара на кассу.");
            #region проверка отмены
            AdditionalFunctions.ThrowExceptionToken(_token);
            #endregion

            Check.Check();
            #region проверка отмены
            AdditionalFunctions.ThrowExceptionToken(_token);
            #endregion

            EventMessage?.Invoke(_terminal, $"Проверка отправки товара на кассу - закончена.");
            #region проверка отмены
            AdditionalFunctions.ThrowExceptionToken(_token);
            #endregion

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
