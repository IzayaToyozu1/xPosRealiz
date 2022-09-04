using System;
using System.Threading;

namespace xPosBL.Terminals.Data.Sending
{
    public interface ISending
    {
        event EventHandler<string> EventMessage;

        void Send(string text);
    }

    public interface ISending<T> : ISending
    {
        new T Send(string text);
    }
}
