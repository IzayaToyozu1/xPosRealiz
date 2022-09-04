using System;

namespace xPosBL.Timer
{
    public delegate void TimerStop(object sender, EventArgs eStop);

    public class TimerEventStop: EventArgs
    {
    }
}
