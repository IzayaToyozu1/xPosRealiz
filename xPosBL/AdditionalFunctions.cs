using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace xPosBL
{
    public static class AdditionalFunctions
    {
        public static void ThrowExceptionToken(CancellationToken token)
        {
            if (token.IsCancellationRequested)
                token.ThrowIfCancellationRequested();
        }
    }
}
