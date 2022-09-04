using System;
using System.Threading;
using System.Threading.Tasks;

namespace xPosBL.Timer
{
    public class TimerTest
    {
        private bool _stopid = false;

        public event TimerWorked EventTimerWorked;
        public event TimerTick EventTimerTickSecond;
        public event TimerStop EventTimerStop;
        public event TimerResum EventTimerResum;

        public bool IsWorking { get { return _stopid; } }
        public long DefaultTimer { get; set; } = 60;
        public string TimerText
        {
            get { return String.Format("{0:d2}:{1:d2}:{2:d2}", Time.Hours, Time.Minutes, Time.Seconds); }
        }
        public TimeSpan Time { get; internal set; }

        public TimerTest()
        {
            Time = TimeSpan.FromSeconds(DefaultTimer);
        }

        public void Start()
        {
            _stopid = true;
            EventTimerResum?.Invoke(this, new TimerEventResum());
            Task task = new Task(() =>
            {
                while (true)
                {
                    if (_stopid)
                    {
                        if (Time.Seconds == 0 && Time.Minutes == 0 && Time.Hours == 0)
                        {
                            EventTimerWorked?.Invoke(this, new TimerWorkedEventArgs());
                            UpdateTimer();
                        }
                        Thread.Sleep(100);
                        EventTimerTickSecond?.Invoke(this, new TimerTickSecondEventArgs(TimerText));
                        Time = Time.Add(-TimeSpan.FromMilliseconds(100));
                    }
                }
            });
            task.Start();
        }

        public void Stop()
        {
            _stopid = false;
            EventTimerStop?.Invoke(this, new TimerEventStop());
        }

        public void Resume()
        {
            _stopid = true;
            EventTimerResum?.Invoke(this, new TimerEventResum());
        }

        public void UpdateTimer()
        {
            Time = TimeSpan.FromSeconds(DefaultTimer);
        }
    }
}
