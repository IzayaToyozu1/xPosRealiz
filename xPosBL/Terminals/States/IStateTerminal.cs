using System;
using System.Threading;

namespace xPosBL.Terminals.States
{
    public interface IStateTerminal
    {
        event EventHandler<string> EventMessage;

        bool ProcIsWorking { get; }

        void ChangProcIsFalse();

        void ChangedState();
        void Execude();
    }
}
