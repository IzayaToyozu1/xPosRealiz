using System;

namespace xPosBL.Timer
{
    public delegate void TimerResum(object sender, EventArgs eResum);

    public class TimerEventResum: EventArgs
    {
    }
}
