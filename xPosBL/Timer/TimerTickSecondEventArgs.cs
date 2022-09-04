using System;

namespace xPosBL.Timer
{
    public delegate void TimerTick(object sender, TimerTickSecondEventArgs e);
    public delegate void TimerWorked(object sender, TimerWorkedEventArgs e);

    public class TimerTickSecondEventArgs : EventArgs
    {
        public string TimerText { get; }

        public TimerTickSecondEventArgs(string timerText)
        {
            TimerText = timerText;
        }
    }

    public class TimerWorkedEventArgs: EventArgs
    {
        public TimerWorkedEventArgs() { }
    }
}
