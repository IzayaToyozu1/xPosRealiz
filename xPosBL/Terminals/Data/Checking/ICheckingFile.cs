using System;
using System.Threading;

namespace xPosBL.Terminals.Data.Checking
{
    public interface ICheckingFile
    {
        event EventHandler<string> EventMessage;

        void Check();
    }
}
